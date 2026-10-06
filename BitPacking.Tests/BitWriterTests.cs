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
			BitWriter writer = new BitWriter();
			writer.Write(0UL, 60);
			writer.Write(ulong.MaxValue, 8);
			writer.Write(0, 4);
			Assert.AreEqual(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0xF0, 0x0F }, writer.GetBytes());
		}

		[Test]
		public void Write_Throws_With_InvalidBits([Values(-1, 65)] int bits)
		{
			BitWriter writer = new BitWriter();
			Assert.That(() => writer.Write(1, bits), Throws.InstanceOf<ArgumentOutOfRangeException>());
		}
	}
}