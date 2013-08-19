using System;
using System.Security.Cryptography;

namespace DamienG.Security.Cryptography
{
	public class Elf32 : HashAlgorithm
	{
		private UInt32 hash;

		public Elf32()
		{
			Initialize();
		}

		public override void Initialize()
		{
			hash = 0;
		}

		protected override void HashCore(byte[] buffer, int start, int length)
		{
			hash = CalculateHash(hash, buffer, start, length);
		}

		protected override byte[] HashFinal()
		{
			byte[] hashBuffer = UInt32ToBigEndianBytes(hash);
			this.HashValue = hashBuffer;
			return hashBuffer;
		}

		public override int HashSize {
			get { return 32; }
		}

		public static UInt32 Compute(byte[] buffer)
		{
			return CalculateHash(0, buffer, 0, buffer.Length);
		}

		public static UInt32 Compute(UInt32 polynomial, UInt32 seed, byte[] buffer)
		{
			return CalculateHash(seed, buffer, 0, buffer.Length);
		}

		private static UInt32 CalculateHash(UInt32 seed, byte[] buffer, int start, int size)
		{
			UInt32 hash = seed;
			
			for (int i = start; i < size-start; i++) {
				hash = (hash << 4) + buffer[i];
				UInt32 work = hash & 0xf0000000u;
				hash ^= work >> 24;
				hash &= ~work;
			}
			return hash;
		}

		private byte[] UInt32ToBigEndianBytes(UInt32 uint32)
		{
			byte[] result = BitConverter.GetBytes(uint32);
			
			if (BitConverter.IsLittleEndian)
				Array.Reverse(result);
			
			return result;
		}
	}
}