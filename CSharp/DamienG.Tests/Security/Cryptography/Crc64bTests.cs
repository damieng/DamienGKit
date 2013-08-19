using DamienG.Security.Cryptography;
using NUnit.Framework;

namespace DamienG.Tests.Security.Cryptography
{
    [TestFixture]
    public class Crc64bTests : BaseHashAlgorithmTests
    {
        [Test]
        public void StaticDefaultSeedAndPolynomialWithShortAsciiString()
        {
            var actual = Crc64b.Compute(SimpleBytesAscii);

            Assert.AreEqual(0x7E210EB1B03E5A1D, actual);
        }

        [Test]
        public void StaticDefaultSeedAndPolynomialWithShortAsciiString2()
        {
            var actual = Crc64b.Compute(SimpleBytes2Ascii);

            Assert.AreEqual(0x416B4150508661EE, actual);
        }

        [Test]
        public void InstanceDefaultSeedAndPolynomialWith12KBinaryFile()
        {
            var hash = GetTestFileHash(Binary12KFileName, new Crc64b());

            Assert.AreEqual(0x9614CF8EA0EF8E63, GetBigEndianUInt64(hash));
        }
    }
}