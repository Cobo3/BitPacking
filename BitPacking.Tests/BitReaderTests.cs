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
		public void Read_Works()
		{
			BitReader reader = new BitReader(
				0b00000001, 
				0b00000010,
				0b00000011
			);
			Assert.AreEqual(513, (ulong)reader.Read(10));
		}
	}
}