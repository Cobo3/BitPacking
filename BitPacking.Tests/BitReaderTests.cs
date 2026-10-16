using System;
using NUnit.Framework;

namespace SickDev.BitPacking.Tests
{
	[TestFixture]
	class BitReaderTests
	{
		[Test]
		public void BitsLeft_Is_All_After_Construct()
		{
			BitReader reader = new BitReader(1, 2, 3);
			Assert.AreEqual(24, reader.bitsLeft);
		}

		[Test]
		public void BitsLeft_Is_Substracted_After_Read()
		{
			BitReader reader = new BitReader(1, 2, 3);
			reader.Read(5);
			Assert.AreEqual(19, reader.bitsLeft);
		}

		[Test]
		public void Read_Throws_With_NegativeNumber()
		{
			BitReader reader = new BitReader(1, 2, 3);
			Assert.That(()=>reader.Read(-1), Throws.InstanceOf<ArgumentOutOfRangeException>());
		}

		[Test]
		public void Read_Throws_When_ReadingTooManyBits()
		{
			BitReader reader = new BitReader(1, 2, 3);
			Assert.That(()=>reader.Read(25), Throws.InstanceOf<ArgumentOutOfRangeException>());
		}

		[Test]
		public void Read_Throws_When_MoreThan64Bits()
		{
			BitReader reader = new BitReader(new byte[9]);
			Assert.That(() => reader.Read(65), Throws.InstanceOf<ArgumentOutOfRangeException>());
		}

		[Test]
		public void Read_Works()
		{
			BitReader reader = new BitReader(
				0b00000001, 
				0b00000010,
				0b00000011
			);
			Assert.AreEqual(513, (ulong)reader.Read(10));
		}

		[TestCase(0L, 1)]
		[TestCase(1L, 2)]
		[TestCase(-1L, 2)]
		[TestCase(-1023L, 11)]
		[TestCase(long.MaxValue, 64)]
		[TestCase(-long.MaxValue, 64)]
		public void ReadSigned_Returns_ValueWrittenWithWriteSigned(long value, int bits)
		{
			BitWriter writer = new BitWriter();
			writer.WriteSigned(value, bits);
			BitReader reader = new BitReader(writer.GetBytes());
			Assert.AreEqual(value, reader.ReadSigned(bits));
		}

		[Test]
		public void ReadSigned_Throws_With_ZeroBits()
		{
			BitReader reader = new BitReader(1, 2, 3);
			Assert.That(() => reader.ReadSigned(0), Throws.InstanceOf<ArgumentOutOfRangeException>());
		}
	}
}