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

		public static DebugBinaryNumber MakeShifted(int position) => 1UL << position;

		public static DebugBinaryNumber MakeFilled(int amount) => filledMasks[amount];
	}
}
