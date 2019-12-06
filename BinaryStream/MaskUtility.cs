using DebugBinaryNumber =
#if DEBUG
	SickDev.BinaryStream.BinaryNumber
#else
	System.UInt64
#endif
;

namespace SickDev.BinaryStream {
	static class MaskUtility {
		static DebugBinaryNumber[] filledMasks = new DebugBinaryNumber[BinaryNumber.maxBits];

		static MaskUtility() {
			for (int i = 0; i < BinaryNumber.maxBits; i++)
				filledMasks[i] = MakeFilledInternal(i);
		}

		static DebugBinaryNumber MakeFilledInternal(int amount)
		{
			DebugBinaryNumber mask = 0;
			for (int i = 0; i < amount; i++)
				mask |= MakeShifted(i);
			return mask;
		}

		//Create numbers in the form of 0000100 being the 1 in the position determined by the parameter
		public static DebugBinaryNumber MakeShifted(int position) => 1UL << position;

		//Create numbers in the form of 1111111 with as many 1s as amount parameter
		//This is a slightly slower operation than the shifted version, which is why the values are cached
		public static DebugBinaryNumber MakeFilled(int amount) => filledMasks[amount];
	}
}