using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DamienG.Tests.Cryptography
{
	public abstract class BaseCryptographyTests
	{
		protected const string simpleString = @"The quick brown fox jumps over the lazy dog.";
		protected readonly byte[] simpleBytesASCII = ASCIIEncoding.ASCII.GetBytes(simpleString);

		protected readonly string runFolder = AppDomain.CurrentDomain.BaseDirectory;
		protected readonly string binary12kFileName = "binary12k.png";
		
		protected byte[] GetTestFileHash(string name, HashAlgorithm hashAlgorithm)
		{
			string pathToTests = Path.Combine(Path.Combine(runFolder, "Cryptography"), "TestFiles");
			using(FileStream stream = File.Open(Path.Combine(pathToTests, name), FileMode.Open))
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