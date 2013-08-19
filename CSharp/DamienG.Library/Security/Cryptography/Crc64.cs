// Copyright (c) Damien Guard.  All rights reserved.
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// Originally published at http://damieng.com/blog/2007/11/19/calculating-crc-64-in-c-and-net

using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace DamienG.Security.Cryptography
{
    /// <summary>
    /// Implements a 64-bit CRC hash algorithm for a given polynomial.
    /// </summary>
    /// <remarks>
    /// For ISO 3309 compliant 64-bit CRC's use Crc64Iso.
    /// </remarks>
    public class Crc64 : HashAlgorithm
    {
        public const UInt64 DefaultSeed = 0x0;

        private readonly UInt64[] table;

        private readonly UInt64 seed;
        private UInt64 hash;

        public Crc64(UInt64 polynomial)
            :  this(polynomial, DefaultSeed)
        {
        }

        public Crc64(UInt64 polynomial, UInt64 seed)
        {
            table = InitializeTable(polynomial);
            this.seed = hash = seed;
        }

        public override void Initialize()
        {
            hash = seed;
        }

        protected override void HashCore(byte[] buffer, int start, int length)
        {
            hash = CalculateHash(hash, table, buffer, start, length);
        }

        protected override byte[] HashFinal()
        {
            var hashBuffer = UInt64ToBigEndianBytes(hash);
            HashValue = hashBuffer;
            return hashBuffer;
        }

        public override int HashSize { get { return 64; } }

        protected static UInt64 CalculateHash(UInt64 seed, UInt64[] table, IList<byte> buffer, int start, int size)
        {
            var crc = seed;

            for (var i = start; i < size; i++)
                unchecked
                {
                    crc = (crc >> 8) ^ table[(buffer[i] ^ crc) & 0xff];
                }

            return crc;
        }

        private static byte[] UInt64ToBigEndianBytes(UInt64 value)
        {
            var result = BitConverter.GetBytes(value);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(result);

            return result;
        }

        private static UInt64[] InitializeTable(UInt64 polynomial)
        {
            if (polynomial == Crc64Iso.Iso3309Polynomial && Crc64Iso.Table != null)
                return Crc64Iso.Table;

            var createTable = CreateTable(polynomial);

            if (polynomial == Crc64Iso.Iso3309Polynomial)
                Crc64Iso.Table = createTable;

            return createTable;
        }

        protected static ulong[] CreateTable(ulong polynomial)
        {
            var createTable = new UInt64[256];
            for (var i = 0; i < 256; ++i)
            {
                var entry = (UInt64) i;
                for (var j = 0; j < 8; ++j)
                    if ((entry & 1) == 1)
                        entry = (entry >> 1) ^ polynomial;
                    else
                        entry = entry >> 1;
                createTable[i] = entry;
            }
            return createTable;
        }
    }

    public class Crc64Iso : Crc64
    {
        internal static UInt64[] Table;

        public const UInt64 Iso3309Polynomial = 0xD800000000000000;

        public Crc64Iso()
            : base(Iso3309Polynomial)
        {
        }

        public Crc64Iso(UInt64 seed)
            : base(Iso3309Polynomial, seed)
        {
        }

        public static UInt64 Compute(byte[] buffer)
        {
            return Compute(DefaultSeed, buffer);
        }

        public static UInt64 Compute(UInt64 seed, byte[] buffer)
        {
            if (Table == null)
                Table = CreateTable(Iso3309Polynomial);

            return CalculateHash(seed, Table, buffer, 0, buffer.Length);
        }
    }
}
