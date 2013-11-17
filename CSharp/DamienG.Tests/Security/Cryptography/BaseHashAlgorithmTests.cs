using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DamienG.Tests.Security.Cryptography
{
    public abstract class BaseHashAlgorithmTests
    {
        protected const string SimpleString = @"The quick brown fox jumps over the lazy dog.";
        protected readonly byte[] SimpleBytesAscii = Encoding.ASCII.GetBytes(SimpleString);

        protected const string SimpleString2 = @"Life moves pretty fast. If you don't stop and look around once in a while, you could miss it.";
        protected readonly byte[] SimpleBytes2Ascii = Encoding.ASCII.GetBytes(SimpleString2);

        protected readonly string RunFolder = AppDomain.CurrentDomain.BaseDirectory;
        protected readonly string Binary12KFileName = "binary12k.png";
        
        protected byte[] GetTestFileHash(string name, HashAlgorithm hashAlgorithm)
        {
			var pathToTests = CombinePaths(RunFolder, "Security", "Cryptography", "TestFiles");
            using(var stream = File.Open(Path.Combine(pathToTests, name), FileMode.Open))
                return hashAlgorithm.ComputeHash(stream);
        }

		private string CombinePaths(string baseFolder, params string[] folders)
		{
			var result = baseFolder;
			foreach (var folder in folders)
				result = Path.Combine(result, folder);
			return result;
		}
        
        protected static UInt32 GetBigEndianUInt32(byte[] bytes)
        {
            if (bytes.Length != 4)
                throw new ArgumentOutOfRangeException("bytes", "Must be 4 bytes in length");
            
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            
            return BitConverter.ToUInt32(bytes, 0);
        }

        protected static UInt64 GetBigEndianUInt64(byte[] bytes)
        {
            if (bytes.Length != 8)
                throw new ArgumentOutOfRangeException("bytes", "Must be 8 bytes in length");

            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);

            return BitConverter.ToUInt64(bytes, 0);
        }
    }
}