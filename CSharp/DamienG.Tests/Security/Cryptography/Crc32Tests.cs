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

            Assert.Equal(0x519025e9u, actual);
        }

        [Fact]
        public void StaticDefaultSeedAndPolynomialWithShortAsciiString2()
        {
            var actual = Crc32.Compute(SimpleBytes2Ascii);

            Assert.Equal(0x6ee3ad88u, actual);
        }

        [Fact]
        public void InstanceDefaultSeedAndPolynomialWith12KBinaryFile()
        {
            var hash = GetTestFileHash(Binary12KFileName, new Crc32());

            Assert.Equal(0x9865b070u, GetBigEndianUInt32(hash));
        }

        [Fact]
        public void InstanceDefaultSeedAndPolynomialWith100KBinaryFile()
        {
            var hash = GetTestFileHash(Binary100KFileName, new Crc32());

            Assert.Equal(0xa1a09767u, GetBigEndianUInt32(hash));
        }
    }
}