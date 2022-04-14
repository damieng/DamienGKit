using System;
using DamienG.Security.Cryptography;
using Xunit;

namespace DamienG.Tests.Security.Cryptography
{
    /// <summary>
    /// Reference: https://crccalc.com/?crc=The%20quick%20brown%20fox%20jumps%20over%20the%20lazy%20dog.&method=crc32&datatype=ascii&outtype=0
    /// </summary>
    public class Crc32Tests : BaseHashAlgorithmTests
    {
        [Fact]
        public void StaticDefaultSeedAndPolynomialWithShortAsciiString()
        {
            var actual = Crc32.Compute(SimpleBytesAscii);

            Assert.Equal(0x519025e9UL, actual);
        }

        [Fact]
        public void InstanceWithShortAsciiString_MPEG2()
        {
            // MPEG-2 is also used by STM32 MCUs
            var engine = new Crc32(
                Crc32.DefaultPolynomial,
                Crc32.DefaultSeed,
                0, false, false);

            var actual = engine.ComputeHash(SimpleBytesAscii);

            Assert.Equal(0x5408D342ul, GetBigEndianUInt32(actual));
        }

        [Fact]
        public void InstanceWithShortAsciiString_JAMCRC()
        {
            var engine = new Crc32(
                Crc32.DefaultPolynomial,
                Crc32.DefaultSeed,
                0, true, true);

            var actual = engine.ComputeHash(SimpleBytesAscii);

            Assert.Equal(0xAE6FDA16ul, GetBigEndianUInt32(actual));
        }

        [Fact]
        public void InstanceWithShortAsciiString_BZ2()
        {
            var engine = new Crc32(
                Crc32.DefaultPolynomial,
                Crc32.DefaultSeed,
                0xFFFFFFFF, false, false);

            var actual = engine.ComputeHash(SimpleBytesAscii);

            Assert.Equal(0xABF72CBDul, GetBigEndianUInt32(actual));
        }

        [Fact]
        public void StaticDefaultSeedAndPolynomialWithShortAsciiString2()
        {
            var actual = Crc32.Compute(SimpleBytes2Ascii);

            Assert.Equal(0x6ee3ad88UL, actual);
        }

        [Fact]
        public void InstanceDefaultSeedAndPolynomialWith12KBinaryFile()
        {
            var hash = GetTestFileHash(Binary12KFileName, new Crc32());

            Assert.Equal(0x9865b070UL, GetBigEndianUInt32(hash));
        }

        [Fact]
        public void InstanceDefaultSeedAndPolynomialWith1MBinaryFile()
        {
            var hash = GetTestFileHash(Binary1MFileName, new Crc32());

            Assert.Equal(0xaffeed9fUL, GetBigEndianUInt32(hash));
        }
    }
}