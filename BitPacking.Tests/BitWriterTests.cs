using System;
using NUnit.Framework;

namespace SickDev.BitPacking.Tests
{
	[TestFixture]
	class BitWriterTests
	{
		[Test]
		public void GetBytes_Returns_Empty_When_NewWriter()
		{
			Assert.IsEmpty(new BitWriter().GetBytes());
		}

		[Test]
		public void GetBytes_Works_When_MoreThan64Bits()
		{
			/* There are 12 entries, 
			 * and we will Write them together in groups of 4.
			 * In total, 96 bits, forcing the Writer to make 2 BinaryNumber
			 * The bytes from GetBytes should match perfectly with these
			 */
			byte[] input = new byte[] 
			{ 
				12, 0, 157, 212,
				255, 2, 42, 128,
				188, 200, 10, 32
			};

			BitWriter writer = new BitWriter();
			for (int i = 0; i < input.Length; i += 4)
				writer.Write(BitConverter.ToUInt32(input, i));
			Assert.AreEqual(input, writer.GetBytes());
		}

		[Test]
		public void GetBytes_Works_When_WriteWithBits()
		{
			/* 1705 is 110 10101001‬
			 * But when writting only 10 bits,
			 * 10 10101001‬ is 681
			 * which is 2 169
			 */
			BitWriter writer = new BitWriter();
			writer.Write(1705, 10);
			Assert.AreEqual(new byte[] { 169, 2 }, writer.GetBytes());
		}

		[Test]
		public void Write_WithBits_Ignores_HigherBits()
		{
			/* 255 written with 4 bits is 1111.
			 * Writing 0 with 4 bits next must not be affected by the upper bits of 255
			 */
			BitWriter writer = new BitWriter();
			writer.Write(255, 4);
			writer.Write(0, 4);
			Assert.AreEqual(new byte[] { 0b00001111 }, writer.GetBytes());
		}

		[Test]
		public void Write_WithBits_Ignores_HigherBits_When_MoreThan64Bits()
		{
			/* After 60 bits, only 4 of the 8 bits of 11111111 fit in the first 64 bits.
			 * The other 4 go to the next 64 bits, followed by 4 bits of 0: 0000 1111
			 * Without masking, the upper bits of ulong.MaxValue would overwrite those 4 bits of 0: 1111 1111
			 */
			BitWriter writer = new BitWriter();
			writer.Write(0, 60);
			writer.Write(ulong.MaxValue, 8);
			writer.Write(0, 4);
			Assert.AreEqual(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0b11110000, 0b00001111 }, writer.GetBytes());
		}

		[Test]
		public void Write_Throws_With_InvalidBits([Values(-1, 65)] int bits)
		{
			BitWriter writer = new BitWriter();
			Assert.That(() => writer.Write(1, bits), Throws.InstanceOf<ArgumentOutOfRangeException>());
		}

		[Test]
		public void WriteSigned_Writes_SignBit_Before_Magnitude()
		{
			/* -1 is sign 1 and magnitude 1: 1 1
			 * 1 is sign 0 and magnitude 1: 1 0
			 * Written one after the other: 0000 10 11
			 */
			BitWriter writer = new BitWriter();
			writer.WriteSigned(-1, 2);
			writer.WriteSigned(1, 2);
			Assert.AreEqual(new byte[] { 0b00001011 }, writer.GetBytes());
		}

		[Test]
		public void WriteSigned_WithoutBits_Uses_SignificantBits_Of_Magnitude_Plus_SignBit()
		{
			/* 5 is 101, so -5 takes 4 bits: 101 1
			 * The 1 written next must land right after those 4 bits: 1 1011
			 */
			BitWriter writer = new BitWriter();
			writer.WriteSigned(-5);
			writer.Write(1, 1);
			Assert.AreEqual(new byte[] { 0b00011011 }, writer.GetBytes());
		}

		[Test]
		public void WriteSigned_Throws_When_Magnitude_DoesNotFit()
		{
			//4 is 100, which doesn't fit in the 2 bits left after the sign bit
			BitWriter writer = new BitWriter();
			Assert.That(() => writer.WriteSigned(-4, 3), Throws.InstanceOf<ArgumentOutOfRangeException>());
		}

		[Test]
		public void WriteSigned_Throws_For_LongMinValue()
		{
			//The magnitude of long.MinValue needs 64 bits, leaving no room for the sign bit
			BitWriter writer = new BitWriter();
			Assert.That(() => writer.WriteSigned(long.MinValue), Throws.InstanceOf<ArgumentOutOfRangeException>());
		}

		[Test]
		public void WriteSigned_Throws_With_InvalidBits([Values(0, 65)] int bits)
		{
			BitWriter writer = new BitWriter();
			Assert.That(() => writer.WriteSigned(0, bits), Throws.InstanceOf<ArgumentOutOfRangeException>());
		}
	}
}