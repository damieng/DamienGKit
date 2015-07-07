using System;
using DamienG.Security.Cryptography;
using Xunit;

namespace DamienG.Tests.Security.Cryptography
{
    public class Crc64Tests : BaseHashAlgorithmTests
    {
        [Fact]
        public void IsoStaticDefaultSeedAndPolynomialWithShortAsciiString()
        {
            var actual = Crc64Iso.Compute(SimpleBytesAscii);

            Assert.Equal((UInt64)0x7E210EB1B03E5A1D, actual);
        }

        [Fact]
        public void IsoStaticDefaultSeedAndPolynomialWithShortAsciiString2()
        {
            var actual = Crc64Iso.Compute(SimpleBytes2Ascii);

            Assert.Equal((UInt64)0x416B4150508661EE, actual);
        }

        [Fact]
        public void IsoInstanceDefaultSeedAndPolynomialWith12KBinaryFile()
        {
            var hash = GetTestFileHash(Binary12KFileName, new Crc64Iso());

            Assert.Equal(0x9614CF8EA0EF8E63, GetBigEndianUInt64(hash));
        }
    }
}