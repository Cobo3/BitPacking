namespace SickDev.BinaryCompressor {
	static class MaskUtility{
		public static BinaryNumber MakeShifted(int position) => 1UL << position;

		public static BinaryNumber MakeFilled(int amount)
		{
			BinaryNumber mask = 0;
			for (int i = 0; i < amount; i++)
				mask |= MakeShifted(i);
			return mask;
		}
	}
}
