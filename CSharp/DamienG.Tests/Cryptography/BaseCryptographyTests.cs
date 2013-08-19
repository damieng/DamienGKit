using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DamienG.Tests.Cryptography
{
    public abstract class BaseCryptographyTests
    {
        protected const string SimpleString = @"The quick brown fox jumps over the lazy dog.";
        protected readonly byte[] SimpleBytesAscii = Encoding.ASCII.GetBytes(SimpleString);

        protected const string SimpleString2 = @"Life moves pretty fast. If you don't stop and look around once in a while, you could miss it.";
        protected readonly byte[] SimpleBytes2Ascii = Encoding.ASCII.GetBytes(SimpleString2);

        protected readonly string RunFolder = AppDomain.CurrentDomain.BaseDirectory;
        protected readonly string Binary12KFileName = "binary12k.png";
        
        protected byte[] GetTestFileHash(string name, HashAlgorithm hashAlgorithm)
        {
            var pathToTests = Path.Combine(Path.Combine(RunFolder, "Cryptography"), "TestFiles");
            using(var stream = File.Open(Path.Combine(pathToTests, name), FileMode.Open))
            {
                return hashAlgorithm.ComputeHash(stream);
            }
        }
        
        protected static UInt32 GetBigEndianUInt32(byte[] bytes)
        {
            if (bytes.Length != 4)
                throw new ArgumentOutOfRangeException("bytes", "Must be 4 bytes in length");
            
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            
            return BitConverter.ToUInt32(bytes, 0);
        }
    }
}