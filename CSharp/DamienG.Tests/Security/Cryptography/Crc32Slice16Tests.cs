using System;
using DamienG.Security.Cryptography;
using Xunit;

namespace DamienG.Tests.Security.Cryptography
{
    public class Crc32Slice16Tests : BaseHashAlgorithmTests
    {
        [Fact]
        public void StaticDefaultSeedAndPolynomialWithShortAsciiString()
        {
            var actual = Crc32Slice16.Compute(SimpleBytesAscii);

            Assert.Equal(0x519025e9U, actual);
        }

        [Fact]
        public void StaticDefaultSeedAndPolynomialWithShortAsciiString2()
        {
            var actual = Crc32Slice16.Compute(SimpleBytes2Ascii);

            Assert.Equal(0x6ee3ad88U, actual);
        }

        [Fact]
        public void InstanceDefaultSeedAndPolynomialWith12KBinaryFile()
        {
            var hash = GetTestFileHash(Binary12KFileName, new Crc32Slice16());

            Assert.Equal(0x9865b070U, GetBigEndianUInt32(hash));
        }

        [Fact]
        public void InstanceDefaultSeedAndPolynomialWith1MBinaryFile()
        {
            var hash = GetTestFileHash(Binary1MFileName, new Crc32Slice16());

            Assert.Equal(0xaffeed9fUL, GetBigEndianUInt32(hash));
        }
    }
}