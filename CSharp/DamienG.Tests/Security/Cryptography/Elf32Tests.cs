using System;
using DamienG.Security.Cryptography;
using Xunit;

namespace DamienG.Tests.Security.Cryptography
{
    public class Elf32Tests : BaseHashAlgorithmTests
    {
        [Fact]
        public void StaticComputeWithShortAsciiString()
        {
            var actual = Elf32.Compute(SimpleBytesAscii);
            
            Assert.Equal(0x0280c5deU, actual);
        }

        [Fact]
        public void StaticComputeWithShortAsciiString2()
        {
            var actual = Elf32.Compute(SimpleBytes2Ascii);

            Assert.Equal(0x0106193eU, actual);
        }

        [Fact]
        public void ComputeViaHashAlgorithmWith12KBinaryFile()
        {
            var hash = GetTestFileHash(Binary12KFileName, new Elf32());
            
            Assert.Equal(0x0a8bf8f2U, GetBigEndianUInt32(hash));
        }

        [Fact]
        public void ComputeViaHashAlgorithmWith1MBinaryFile()
        {
            var hash = GetTestFileHash(Binary1MFileName, new Elf32());

            Assert.Equal(0x5838755u, GetBigEndianUInt32(hash));
        }
    }
}