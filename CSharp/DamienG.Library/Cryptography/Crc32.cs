using System;
using System.Security.Cryptography;

namespace DamienG.Security.Cryptography
{
	public class Crc32 : HashAlgorithm
	{
		public const UInt32 DefaultPolynomial = 0xedb88320u;
		public const UInt32 DefaultSeed = 0xffffffffu;
		
		private static UInt32[] defaultTable;		
		
		private readonly UInt32 seed;
		private readonly UInt32[] table;
		private UInt32 hash;

		public Crc32()
		{
			table = InitializeTable(DefaultPolynomial);
			seed = DefaultSeed;
			Initialize();
		}

		public Crc32(UInt32 polynomial, UInt32 seed)
		{
			table = InitializeTable(polynomial);
			this.seed = seed;
			Initialize();
		}

		public override void Initialize()
		{
			hash = seed;
		}

		protected override void HashCore(byte[] buffer, int start, int length)
		{
			hash = CalculateHash(table, hash, buffer, start, length);
		}

		protected override byte[] HashFinal()
		{
			byte[] hashBuffer = UInt32ToBigEndianBytes(~hash);
			this.HashValue = hashBuffer;
			return hashBuffer;
		}

		public override int HashSize {
			get { return 32; }
		}

		public static UInt32 Compute(byte[] buffer)
		{
			return CalculateHash(InitializeTable(DefaultPolynomial), DefaultSeed, buffer, 0, buffer.Length);
		}

		public static UInt32 Compute(UInt32 seed, byte[] buffer)
		{
			return CalculateHash(InitializeTable(DefaultPolynomial), seed, buffer, 0, buffer.Length);
		}

		public static UInt32 Compute(UInt32 polynomial, UInt32 seed, byte[] buffer)
		{
			return CalculateHash(InitializeTable(polynomial), seed, buffer, 0, buffer.Length);
		}

		private static UInt32[] InitializeTable(UInt32 polynomial)
		{
			if (polynomial == DefaultPolynomial && defaultTable != null)
				return defaultTable;
			
			UInt32[] createTable = new UInt32[256];
			for (int i = 0; i < 256; i++) {
				UInt32 entry = (UInt32)i;
				for (int j = 0; j < 8; j++)
					if ((entry & 1) == 1)
						entry = (entry >> 1) ^ polynomial;
					else
						entry = entry >> 1;
				createTable[i] = entry;
			}
			
			if (polynomial == DefaultPolynomial)
				defaultTable = createTable;
			
			return createTable;
		}

		private static UInt32 CalculateHash(UInt32[] table, UInt32 seed, byte[] buffer, int start, int size)
		{
			UInt32 crc = seed;
			for (int i = start; i < size-start; i++)
				crc = (crc >> 8) ^ table[buffer[i] ^ crc & 0xff];
			return crc;
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