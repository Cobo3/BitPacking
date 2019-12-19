using System;
using System.Text;

namespace SickDev.BitPacking
{
	public partial struct BinaryNumber : IConvertible, IComparable<BinaryNumber>, IEquatable<BinaryNumber>
	{
		public const int bitsPerByte = 8;
		public const int maxBits = 64;

		public readonly ulong value;
		string stringRepresentation;

		public int significantBits { get; private set; }

		public BinaryNumber(IConvertible value)
		{
			this.value = value.ToUInt64(null);
			stringRepresentation = default;
			significantBits = maxBits - CountLeadingZeros(this.value);
		}

		//Taken from https://stackoverflow.com/questions/31374628/fast-way-of-finding-most-and-least-significant-bit-set-in-a-64-bit-integer
		public static int CountLeadingZeros(ulong input)
		{
			if (input == 0)
				return 63;

			ulong n = 1;
			if ((input >> 32) == 0) {n += 32; input <<= 32;}
			if ((input >> 48) == 0) {n += 16; input <<= 16;}
			if ((input >> 56) == 0) {n += 8; input <<= 8;}
			if ((input >> 60) == 0) {n += 4; input <<= 4;}
			if ((input >> 62) == 0) {n += 2; input <<= 2;}
			n -= (input >> 63);

			return (int)n;
		}

		//Transforms the whole number into an array of bytes
		public byte[] GetBytes() => GetBytes(significantBits);

		//Transforms the first "significantBits" into an array of bytes
		public byte[] GetBytes(int significantBits)
		{
			int amount = (int)Math.Ceiling((float)significantBits / bitsPerByte);
			byte[] bytes = new byte[amount];

			//Here we shift packs of 8 bits into the right so that we can get that particular byte value
			for (int i = 0; i < amount; i++)
			{
				int shift = i * bitsPerByte;
#if DEBUG
				BinaryNumber shiftedResult = value >> shift;
				BinaryNumber binaryByte = (byte)shiftedResult.value;
				bytes[i] = (byte)binaryByte.value;
#else
				ulong shiftedResult = value >> shift;
				bytes[i] = (byte)shiftedResult;
#endif
			}

			return bytes;
		}

		public override string ToString()
		{
			if (stringRepresentation == null)
				stringRepresentation = CreateStringRepresentation();

			return stringRepresentation;
		}

		string CreateStringRepresentation()
		{
			//This very first line would suffice...
			StringBuilder builder = new StringBuilder(Convert.ToString((long)value, 2));

			int zerosLeft = builder.Length % bitsPerByte;
			if (zerosLeft > 0)
				zerosLeft = bitsPerByte - zerosLeft;

			//...but I'm interested in making the string multiple of 8...
			for (int i = 0; i < zerosLeft; i++)
				builder.Insert(0, "0");

			//...and separate the bytes for an easier visualization
			int spaces = builder.Length / bitsPerByte;
			for (int i = 1; i < spaces; i++)
				builder.Insert(i * bitsPerByte + (i - 1), " ");

			return builder.ToString();
		}
	}
}