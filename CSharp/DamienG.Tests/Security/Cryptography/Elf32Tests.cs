using System;
using DamienG.Security.Cryptography;
using Xunit;

namespace DamienG.Tests.Security.Cryptography
{
    public class Elf32Tests : BaseHashAlgorithmTests
    {
        [Fact]
        public void StaticDefaultSeedAndPolynomialWithShortAsciiString()
        {
            var actual = Elf32.Compute(SimpleBytesAscii);
            
            Assert.Equal((UInt32)0x0280c5de, actual);
        }

        [Fact]
        public void StaticDefaultSeedAndPolynomialWithShortAsciiString2()
        {
            var actual = Elf32.Compute(SimpleBytes2Ascii);

            Assert.Equal((UInt32)0x0106193e, actual);
        }

        [Fact]
        public void InstanceDefaultSeedAndPolynomialWith12KBinaryFile()
        {
            var hash = GetTestFileHash(Binary12KFileName, new Elf32());
            
            Assert.Equal((UInt32)0x0a8bf8f2, GetBigEndianUInt32(hash));
        }
    }
}