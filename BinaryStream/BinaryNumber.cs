using System;
using System.Text;

namespace SickDev.BinaryStream {
    public partial struct BinaryNumber: IConvertible, IComparable<BinaryNumber>, IEquatable<BinaryNumber> {
        public const int bitsPerByte = 8;
        public const int maxBits = sizeof(ulong)*bitsPerByte;

		public readonly ulong value;
		string stringRepresentation;

		public int significantBits { get; private set; }

        public BinaryNumber(IConvertible value) {
            this.value = value.ToUInt64(null);
			significantBits = default;
			stringRepresentation = default;
			significantBits = maxBits - CountLeadingZeros(this.value);
        }

		//Taken from https://stackoverflow.com/questions/31374628/fast-way-of-finding-most-and-least-significant-bit-set-in-a-64-bit-integer
		public static int CountLeadingZeros(ulong input)
		{
			if (input == 0) return 64;

			ulong n = 1;

			if ((input >> 32) == 0) { n = n + 32; input = input << 32; }
			if ((input >> 48) == 0) { n = n + 16; input = input << 16; }
			if ((input >> 56) == 0) { n = n + 8; input = input << 8; }
			if ((input >> 60) == 0) { n = n + 4; input = input << 4; }
			if ((input >> 62) == 0) { n = n + 2; input = input << 2; }
			n = n - (input >> 63);

			return (int)n;
		}

		public byte[] GetBytes() => GetBytes(significantBits);

        public byte[] GetBytes(int significantBits) {
            byte[] bytes = new byte[(int)Math.Ceiling((float)significantBits/bitsPerByte)];
            for (int i = 0; i < bytes.Length; i++) {
                int shift = i * 8;
                BinaryNumber shiftedResult = value >> shift;
                BinaryNumber binaryByte = (byte)shiftedResult.value;
				bytes[i] = (byte)binaryByte.value;
            }
            return bytes;
        }

		public override string ToString()
		{
			if (stringRepresentation == null)
			{
				StringBuilder builder = new StringBuilder(Convert.ToString((long)value, 2));
				int zerosLeft = builder.Length % bitsPerByte;
				if (zerosLeft > 0)
					zerosLeft = 8 - zerosLeft;

				for (int i = 0; i < zerosLeft; i++)
					builder.Insert(0, "0");

				int spaces = builder.Length / bitsPerByte;
				for (int i = 1; i < spaces; i++)
					builder.Insert(i * 8 + (i - 1), " ");

				stringRepresentation = builder.ToString();
			}

			return stringRepresentation;
		}
    }
}