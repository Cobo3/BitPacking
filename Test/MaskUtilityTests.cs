using NUnit.Framework;

namespace SickDev.BitPacking.Tests
{
	[TestFixture]
	class MaskUtilityTests
	{
		[Test]
		public void MakeShifted_Shifts_1()
		{
			Assert.AreEqual(1024, MaskUtility.MakeShifted(10).value);
		}

		[Test]
		public void MakeFilled_MakesEverything1()
		{
			Assert.AreEqual(1023, MaskUtility.MakeFilled(10).value);
		}
	}
}
