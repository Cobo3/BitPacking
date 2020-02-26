﻿using System;
using NUnit.Framework;

namespace SickDev.BitPacking.Tests
{
	[TestFixture]
	class BitWritterTests
	{
		[Test]
		public void GetBytes_Returns_Empty_When_NewWritter()
		{
			Assert.IsEmpty(new BitWriter().GetBytes());
		}

		[Test]
		public void GetBytes_Works_When_MoreThan64Bits()
		{
			/* There are 12 entries, 
			 * and we will Write them together in groups of 4.
			 * In total, 96 bits, forcing the Writter to make 2 BinaryNumber
			 * The bytes from GetBytes should match perfectly with these
			 */
			byte[] input = new byte[] 
			{ 
				12, 0, 157, 212,
				255, 2, 42, 128,
				188, 200, 10, 32
			};

			BitWriter writter = new BitWriter();
			for (int i = 0; i < input.Length; i += 4)
				writter.Write(BitConverter.ToUInt32(input, i));
			Assert.AreEqual(input, writter.GetBytes());
		}

		[Test]
		public void GetBytes_Works_When_WriteWithBits()
		{
			/* 1705 is 110 10101001‬
			 * But when writting only 10 bits,
			 * 10 10101001‬ is 681
			 * which is 2 169
			 */
			BitWriter writter = new BitWriter();
			writter.Write(1705, 10);
			Assert.AreEqual(new byte[] { 169, 2 }, writter.GetBytes());
		}
	}
}