using NUnit.Framework;

namespace SickDev.BitPacking.Tests
{
	[TestFixture]
	class BinaryNumberTests
	{
		[Test]
		public void ZeroHas1SignificantBit()
		{
			BinaryNumber binary = 1;
			Assert.That(binary.significantBits, Is.EqualTo(1));
		}
	}
}