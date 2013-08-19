using DamienG.Security.Cryptography;
using NUnit.Framework;

namespace DamienG.Tests.Security.Cryptography
{
    [TestFixture]
    public class Crc32Tests : BaseHashAlgorithmTests
    {
        [Test]
        public void StaticDefaultSeedAndPolynomialWithShortAsciiString()
        {
            var actual = Crc32.Compute(SimpleBytesAscii);

            Assert.AreEqual(0x519025e9, actual);
        }

        [Test]
        public void StaticDefaultSeedAndPolynomialWithShortAsciiString2()
        {
            var actual = Crc32.Compute(SimpleBytes2Ascii);

            Assert.AreEqual(0x6ee3ad88, actual);
        }

        [Test]
        public void InstanceDefaultSeedAndPolynomialWith12KBinaryFile()
        {
            var hash = GetTestFileHash(Binary12KFileName, new Crc32());

            Assert.AreEqual(0x9865b070, GetBigEndianUInt32(hash));
        }
    }
}