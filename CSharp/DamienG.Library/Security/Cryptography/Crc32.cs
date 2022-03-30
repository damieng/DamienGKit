// Copyright (c) Damien Guard.  All rights reserved.
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0

using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace DamienG.Security.Cryptography
{
    /// <summary>
    /// Implements a 32-bit CRC hash algorithm compatible with Zip etc.
    /// </summary>
    /// <remarks>
    /// Crc32 should only be used for backward compatibility with older file formats
    /// and algorithms. It is not secure enough for new applications.
    /// If you need to call multiple times for the same data either use the HashAlgorithm
    /// interface or remember that the result of one Compute call needs to be ~ (XOR) before
    /// being passed in as the seed for the next Compute call.
    /// </remarks>
    public sealed class Crc32 : HashAlgorithm
    {
        public const UInt32 DefaultPolynomial = 0x04C11DB7u;
        public const UInt32 DefaultSeed = 0xffffffffu;

        static UInt32[] defaultTable;

        readonly UInt32 seed;
        readonly UInt32[] table;
        UInt32 hash;
        UInt32 xorOut = 0xffffffff;
        bool reflectIn = true;
        bool reflectOut = true;

        /// <summary>
        /// Create a new <see cref="Crc32"/> with a <see cref="DefaultPolynomial"/> and <see cref="DefaultSeed"/>.
        /// </summary>
        public Crc32()
            : this(DefaultPolynomial, DefaultSeed)
        {
        }

        /// <summary>
        /// Create a new <see cref="Crc32"/> with a supplied polynomial, seed, and algorithm options..
        /// Note: the "default" CRC32 algorithm reflects both input and output, then flips all bits (XorOut = 0xFFFFFFFF). 
        /// </summary>
        /// <param name="polynomial">The polynomial to use in calculating.</param>
        /// <param name="polynomial">The initial seed to start from.</param>
        public Crc32(UInt32 polynomial, UInt32 seed, UInt32 XorOut = 0xFFFFFFFF, bool refIn = true, bool refOut = true)
        {
            if (!BitConverter.IsLittleEndian)
                throw new PlatformNotSupportedException("Not supported on Big Endian processors");

            table = InitializeTable(polynomial, refIn);
            this.seed = hash = seed;

            this.xorOut = XorOut;
            this.reflectIn = refIn;
            this.reflectOut = refOut;
        }

        /// <inheritdoc/>
        public override void Initialize()
        {
            hash = seed;
        }

        /// <inheritdoc/>
        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            hash = CalculateHash(table, hash, array, ibStart, cbSize, xorOut, reflectIn, reflectOut);
        }

        /// <inheritdoc/>
        protected override byte[] HashFinal()
        {
            var hashBuffer = UInt32ToBigEndianBytes(hash ^ xorOut);
            HashValue = hashBuffer;
            return hashBuffer;
        }

        /// <inheritdoc/>
        public override int HashSize => 32;

        /// <summary>
        /// Calculate the <see cref="Crc32"/> for a given <paramref name="buffer"/> with the
        /// <see cref="DefaultSeed"/> and <see cref="DefaultPolynomial"/>.
        /// </summary>
        /// <param name="buffer">The <see cref="byte[]"/> buffer to calcuate a CRC32 for.</param>
        /// <returns>The CRC32 for the buffer.</returns>
        public static UInt32 Compute(byte[] buffer) => Compute(DefaultSeed, buffer);

        /// <summary>
        /// Calculate the <see cref="Crc32"/> for a given <paramref name="buffer"/> with a
        /// specified <paramref name="seed"/> and <see cref="DefaultPolynomial"/>.
        /// </summary>
        /// <param name="seed">The initial seed to start from.</param>
        /// <param name="buffer">The <see cref="byte[]"/> buffer to calcuate a CRC32 for.</param>
        /// <returns>The CRC32 for the buffer.</returns>
        public static UInt32 Compute(UInt32 seed, byte[] buffer) => Compute(DefaultPolynomial, seed, buffer);

        /// <summary>
        /// Calculate the <see cref="Crc32"/> for a given <paramref name="buffer"/> with a
        /// specified <paramref name="seed"/> and <paramref name="polynomial"/>.
        /// </summary>
        /// <param name="seed">The initial seed to start from.</param>
        /// <param name="buffer">The <see cref="byte[]"/> buffer to calcuate a CRC32 for.</param>
        /// <returns>The CRC32 for the buffer.</returns>
        public static UInt32 Compute(UInt32 polynomial, UInt32 seed, byte[] buffer) => 
            ~CalculateHash(InitializeTable(polynomial), seed, buffer, 0, buffer.Length);

        /// <summary>
        /// Initialize a CRC32 calculation table for a given polynomial.
        /// </summary>
        /// <param name="polynomial">The polynomial to calculate a table for.</param>
        /// <returns>A <see cref="UInt32[]"/> table to be used in calculating a CRC32.</returns>
        static UInt32[] InitializeTable(UInt32 polynomial, bool refIn = true)
        {
            if (polynomial == DefaultPolynomial && defaultTable != null && refIn)
                return defaultTable;

            uint bit; 
            var createTable = new UInt32[256];
            for (var i = 0; i < 256; i++)
            {
                var entry = (UInt32)(refIn ? reflect((uint)i, 8) : (uint)i);

                entry <<= 24;
                for(var j =0; j < 8; j++)
                {
                    bit = entry & (1u << 31);
                    entry <<= 1;
                    if (bit > 0)
                    {
                        entry ^= polynomial;
                    }
                }

                if (refIn)
                {
                    entry = reflect(entry, 32); 
                }
                createTable[i] = entry;
            }

            if (polynomial == DefaultPolynomial && refIn)
                defaultTable = createTable;

            return createTable;
        }

        /// <summary>
        /// Calculate an inverted CRC32 for a given <paramref name="buffer"/> using a polynomial-derived <paramref name="table"/>.
        /// </summary>
        /// <param name="table">The polynomial-derived table such as from <see cref="InitializeTable(UInt32)"/>.</param>
        /// <param name="seed">The initial seed to start from.</param>
        /// <param name="buffer">The <see cref="IList{byte}"/> buffer to calculate the CRC32 from.</param>
        /// <param name="start">What position within the <paramref name="buffer"/> to start calculating from.</param>
        /// <param name="size">How many bytes within the <paramref name="buffer"/> to read in calculating the CRC32.</param>
        /// <returns>The bit-inverted CRC32.</returns>
        /// <remarks>This hash is bit-inverted. Use other methods in this class or <see langword="~"/> the result from this method.</remarks>
        static UInt32 CalculateHash(UInt32[] table, UInt32 seed, IList<byte> buffer, int start, int size, uint xorOut = 0xffffffff, bool refIn = true, bool refOut = true)
        {
            var hash = seed;
            //if (refIn) hash = reflect(hash, 32); 

            if (refIn)
            {
            for (var i = start; i < start + size; i++)
                hash = (hash >> 8) ^ table[buffer[i] ^ hash & 0xff];
            }
            else
            {
                for (var i = start; i < start + size; i++)
                    hash = (hash << 8) ^ table[buffer[i] ^ ((hash >> 24) & 0xff)];
            }

            if (refIn ^ refOut)
            {
                hash = reflect(hash, 32); 
            }

            return hash;
        }

        /// <summary>
        /// Convert a <see cref="UInt32"/> to a <see cref="byte[]"/> taking care
        /// to reverse the bytes on little endian processors.
        /// </summary>
        /// <param name="uint32">The <see cref="UInt32"/> to convert.</param>
        /// <returns>The <see cref="byte[]"/> containing the converted bytes.</returns>
        static byte[] UInt32ToBigEndianBytes(UInt32 uint32)
        {
            var result = BitConverter.GetBytes(uint32);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(result);

            return result;
        }

        /// <summary>
        /// Helper to support bitwise reflecting (reversing) of bytes.
        /// </summary>
        /// <param name="crc"></param>
        /// <param name="bitnum"></param>
        /// <returns></returns>
        public static UInt32 reflect(UInt32 crc, int bitnum)
        {

            // reflects the lower 'bitnum' bits of 'crc'

            UInt32 i, j = 1, crcout = 0;

            for (i = (UInt32)1 << (bitnum - 1); i > 0; i >>= 1)
            {
                if ((crc & i) > 0)
                    crcout |= j;
                j <<= 1;
            }
            return (crcout);
        }

    }
}