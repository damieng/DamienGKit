using System;
using System.IO;
using System.Text;
using NUnit.Framework;
using DamienG.Security.Cryptography;

namespace DamienG.Tests.Cryptography
{
	[TestFixture]
	public class Elf32Tests : BaseCryptographyTests
	{
		[Test]
		public void Static_Default_Seed_And_Polynomial_With_Short_ASCII_String()
		{
			UInt32 actual = Elf32.Compute(simpleBytesASCII);
			
			Assert.AreEqual(0x0280c5de, actual);
		}

		[Test]
		public void Instance_Default_Seed_And_Polynomial_With_12K_Binary_File()
		{
			byte[] hash = GetTestFileHash(binary12kFileName, new Elf32());
			
			Assert.AreEqual(0x0a8f8f2, GetBigEndianUInt32(hash));
		}		
	}
}