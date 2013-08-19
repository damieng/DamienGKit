using System;
using System.IO;
using System.Text;
using NUnit.Framework;
using DamienG.Security.Cryptography;

namespace DamienG.Tests.Cryptography
{
	[TestFixture]
	public class CRC32Tests : BaseCryptographyTests
	{
		[Test]
		public void Static_Default_Seed_And_Polynomial_With_Short_ASCII_String()
		{
			UInt32 actual = Crc32.Compute(simpleBytesASCII);
			
			Assert.AreEqual(0x519025e9, actual);
		}
		
		[Test]
		public void Instance_Default_Seed_And_Polynomial_With_12K_Binary_File()
		{
			byte[] hash = GetTestFileHash(binary12kFileName, new Crc32());
			
			Assert.AreEqual(0x9865b070, GetBigEndianUInt32(hash));
		}
	}
}