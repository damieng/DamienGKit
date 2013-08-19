using DamienG.Security.Cryptography;
using NUnit.Framework;

namespace DamienG.Tests.Security.Cryptography
{
    [TestFixture]
    public class Crc64Tests : BaseHashAlgorithmTests
    {
        [Test]
        public void IsoStaticDefaultSeedAndPolynomialWithShortAsciiString()
        {
            var actual = Crc64Iso.Compute(SimpleBytesAscii);

            Assert.AreEqual(0x7E210EB1B03E5A1D, actual);
        }

        [Test]
        public void IsoStaticDefaultSeedAndPolynomialWithShortAsciiString2()
        {
            var actual = Crc64Iso.Compute(SimpleBytes2Ascii);

            Assert.AreEqual(0x416B4150508661EE, actual);
        }

        [Test]
        public void IsoInstanceDefaultSeedAndPolynomialWith12KBinaryFile()
        {
            var hash = GetTestFileHash(Binary12KFileName, new Crc64Iso());

            Assert.AreEqual(0x9614CF8EA0EF8E63, GetBigEndianUInt64(hash));
        }
    }
}