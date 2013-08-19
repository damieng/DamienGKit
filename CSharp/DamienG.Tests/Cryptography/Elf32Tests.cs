using DamienG.Security.Cryptography;
using NUnit.Framework;

namespace DamienG.Tests.Cryptography
{
    [TestFixture]
    public class Elf32Tests : BaseCryptographyTests
    {
        [Test]
        public void StaticDefaultSeedAndPolynomialWithShortAsciiString()
        {
            var actual = Elf32.Compute(SimpleBytesAscii);
            
            Assert.AreEqual(0x0280c5de, actual);
        }

        [Test]
        public void StaticDefaultSeedAndPolynomialWithShortAsciiString2()
        {
            var actual = Elf32.Compute(SimpleBytes2Ascii);

            Assert.AreEqual(0x0106193e, actual);
        }

        [Test]
        public void InstanceDefaultSeedAndPolynomialWith12KBinaryFile()
        {
            var hash = GetTestFileHash(Binary12KFileName, new Elf32());
            
            Assert.AreEqual(0x0a8bf8f2, GetBigEndianUInt32(hash));
        }
    }
}