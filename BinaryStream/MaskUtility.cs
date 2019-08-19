using DebugBinaryNumber =
#if DEBUG
	SickDev.BinaryStream.BinaryNumber
#else
	System.UInt64
#endif
;

namespace SickDev.BinaryStream {
	static class MaskUtility{
		public static DebugBinaryNumber MakeShifted(int position) => 1UL << position;

		public static DebugBinaryNumber MakeFilled(int amount)
		{
			DebugBinaryNumber mask = 0;
			for (int i = 0; i < amount; i++)
				mask |= MakeShifted(i);
			return mask;
		}
	}
}
