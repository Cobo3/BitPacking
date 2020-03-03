using NUnit.Framework;
using System.Linq;

namespace SickDev.BitPacking.Tests
{
	[TestFixture]
	class BinaryNumberTests
	{
		[Test]
		public void Value_Is_TheSame()
		{
			BinaryNumber binary = 123456789;
			Assert.AreEqual(binary.value, 123456789);
		}

		[Test]
		public void SignificantBits_Is_1_For_Zero()
		{
			BinaryNumber binary = 0;
			Assert.AreEqual(binary.significantBits, 1);
		}

		[Test]
		public void SignificantBits_Is_1_For_One()
		{
			BinaryNumber binary = 1;
			Assert.AreEqual(binary.significantBits, 1);
		}

		[Test]
		public void SignificantBits_Is_8_For_MaxByte()
		{
			BinaryNumber binary = byte.MaxValue;
			Assert.AreEqual(8, binary.significantBits);
		}

		[Test]
		public void SignificantBits_Is_16_For_MaxUShort()
		{
			BinaryNumber binary = ushort.MaxValue;
			Assert.AreEqual(16, binary.significantBits);
		}

		[Test]
		public void SignificantBits_Is_32_For_MaxUInt()
		{
			BinaryNumber binary = uint.MaxValue;
			Assert.AreEqual(32, binary.significantBits);
		}

		[Test]
		public void SignificantBits_Is_64_For_MaxULong()
		{
			BinaryNumber binary = ulong.MaxValue;
			Assert.AreEqual(64, binary.significantBits);
		}

		[Test]
		public void WorksForNegativeNumbers()
		{
			Assert.DoesNotThrow(() => new BinaryNumber(-1));
		}

		[Test, Sequential]
		public void GetBytes_Returns_1ByteForEvery8Bits([Values(1, 9, 25, 57)] int significantBits, [Values(1, 2, 4, 8)] int numberOfBytes)
		{
			BinaryNumber binary = ulong.MaxValue;
			Assert.AreEqual(numberOfBytes, binary.GetBytes(significantBits).Length);
		}

		[Test]
		public void GetBytes_Returns_RightmostBytes()
		{
			/* The full number is 00000101 00001100 01000010 00100011‬
			 * but we are only getting bytes from 100 01000010 00100011 
			 * That should be 4, 66, 35; reversed
			 */
			BinaryNumber binary = 84689443;
			Assert.AreEqual(new byte[] { 35, 66, 4 }, binary.GetBytes(19));
		}

		[Test]
		public void GetBytes_With_0_Returns_Empty()
		{
			BinaryNumber binary = 84689443;
			Assert.IsEmpty(binary.GetBytes(0));
		}

		[Test]
		public void GetBytes_Throws_With_Negative()
		{
			BinaryNumber binary = 84689443;
			Assert.That(() => binary.GetBytes(-1), Throws.TypeOf<System.ArgumentOutOfRangeException>());
		}

		[Test]
		public void GetBytes_Throws_With_GreaterThan64()
		{
			BinaryNumber binary = 84689443;
			Assert.That(() => binary.GetBytes(65), Throws.TypeOf<System.ArgumentOutOfRangeException>());
		}

		[Test]
		public void ToString_Is_MultipleOf8()
		{
			//This is 11110001001000000
			//But I want it like this
			//000000011110001001000000
			BinaryNumber binary = 123456;
			string binaryString = binary.ToString();
			Assert.That(() => binaryString.Replace(" ", string.Empty).Length % 8, Is.Zero);
		}

		[Test]
		public void ToString_Is_SeparatedByBytes()
		{
			//This is 11110001001000000
			//But I want it like this
			//00000001 11100010 01000000
			BinaryNumber binary = 123456;
			string binaryString = binary.ToString()+" ";
			int chunks = binaryString.Count(x => x == ' ');
			int[] chunksLengths = new int[chunks];

			for (int i = 0; i < chunks; i++)
			{
				int index = binaryString.IndexOf(" ");
				string chunk = binaryString.Substring(0, index);
				chunksLengths[i] = chunk.Length;
				binaryString = binaryString.Remove(0, chunk.Length + 1);
			}

			Assert.That(chunksLengths, Is.All.EqualTo(8));
		}

		[Test]
		public void LeftShiftOperator_Works()
		{
			BinaryNumber binary = new BinaryNumber(8);
			binary <<= 2;
			Assert.AreEqual(binary.value, 32);
		}

		[Test]
		public void RightShiftOperator_Works()
		{
			BinaryNumber binary = new BinaryNumber(32);
			binary >>= 2;
			Assert.AreEqual(binary.value, 8);
		}

		[Test]
		public void BitwiseOrOperator_Works()
		{
			BinaryNumber binary = new BinaryNumber(32);
			binary |= 8;
			Assert.AreEqual(binary.value, 40);
		}

		[Test]
		public void BitwiseAndOperator_Works()
		{
			BinaryNumber binary = new BinaryNumber(40);
			binary &= 12;
			Assert.AreEqual(binary.value, 8);
		}

		[Test]
		public void BitwiseExorOperator_Works()
		{
			BinaryNumber binary = new BinaryNumber(40);
			binary ^= 12;
			Assert.AreEqual(binary.value, 36);
		}

		[Test]
		public void EqualOperator_Works()
		{
			BinaryNumber binary = new BinaryNumber(40);
			BinaryNumber binary2 = new BinaryNumber(40);
			Assert.IsTrue(binary == binary2);
		}

		[Test]
		public void NotEqualOperator_Works()
		{
			BinaryNumber binary = new BinaryNumber(40);
			BinaryNumber binary2 = new BinaryNumber(42);
			Assert.IsTrue(binary != binary2);
		}

		[Test]
		public void GreaterThanOperator_Works()
		{
			BinaryNumber binary = new BinaryNumber(40);
			BinaryNumber binary2 = new BinaryNumber(42);
			Assert.IsTrue(binary2 > binary);
		}

		[Test]
		public void GreaterThanOrEqualOperator_Works()
		{
			BinaryNumber binary = new BinaryNumber(40);
			BinaryNumber binary2 = new BinaryNumber(42);
			Assert.IsTrue(binary2 >= binary);
		}

		[Test]
		public void LessThanOperator_Works()
		{
			BinaryNumber binary = new BinaryNumber(40);
			BinaryNumber binary2 = new BinaryNumber(42);
			Assert.IsTrue(binary < binary2);
		}

		[Test]
		public void LessThanOrEqualOperator_Works()
		{
			BinaryNumber binary = new BinaryNumber(40);
			BinaryNumber binary2 = new BinaryNumber(42);
			Assert.IsTrue(binary <= binary2);
		}
	}
}