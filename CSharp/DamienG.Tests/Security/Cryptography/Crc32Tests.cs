using System;
using DamienG.Security.Cryptography;
using Xunit;

namespace DamienG.Tests.Security.Cryptography
{
    public class Crc32Tests : BaseHashAlgorithmTests
    {
        [Fact]
        public void StaticDefaultSeedAndPolynomialWithShortAsciiString()
        {
            var actual = Crc32.Compute(SimpleBytesAscii);

            Assert.Equal((UInt32)0x519025e9, actual);
        }

        [Fact]
        public void StaticDefaultSeedAndPolynomialWithShortAsciiString2()
        {
            var actual = Crc32.Compute(SimpleBytes2Ascii);

            Assert.Equal((UInt32)0x6ee3ad88, actual);
        }

        [Fact]
        public void InstanceDefaultSeedAndPolynomialWith12KBinaryFile()
        {
            var hash = GetTestFileHash(Binary12KFileName, new Crc32());

            Assert.Equal(0x9865b070, GetBigEndianUInt32(hash));
        }
    }
}