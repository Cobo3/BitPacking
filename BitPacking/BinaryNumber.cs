using System;
using System.Text;
using System.Collections.Generic;
using DebugBinaryNumber =
#if DEBUG
	SickDev.BitPacking.BinaryNumber
#else
	System.UInt64
#endif
	;

namespace SickDev.BitPacking
{
	public readonly partial struct BinaryNumber : IConvertible, IComparable<BinaryNumber>, IEquatable<BinaryNumber>
	{
		public const int bitsPerByte = 8;
		public const int maxBits = 64;

		static Dictionary<ulong, string> stringRepresentations = new Dictionary<ulong, string>();

		public readonly ulong value;
		public readonly int significantBits;

		public BinaryNumber(IConvertible value)
		{
			this.value = value.ToUInt64(null);
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
			n -= input >> 63;

			return (int)n;
		}

		//Transforms the whole number into an array of bytes
		public byte[] GetBytes() => GetBytes(significantBits);

		//Transforms the first "bits" into an array of bytes
		public byte[] GetBytes(int bits)
		{
			if (bits < 0 || bits > 64)
				throw new ArgumentOutOfRangeException(nameof(bits), $"Must be 0 < {nameof(bits)} < 64");
			int length = (int)Math.Ceiling((float)bits / bitsPerByte);
			byte[] bytes = new byte[length];

			//clamp value to the bits we were asked for
			DebugBinaryNumber clampedValue = value & MaskUtility.MakeFilled(bits);

			//Here we shift packs of 8 bits to the right so that we can get that particular byte value
			for (int i = 0; i < length; i++)
			{
				bytes[i] = clampedValue;
				clampedValue >>= bitsPerByte;
			}

			return bytes;
		}

		public override string ToString()
		{
			if (!stringRepresentations.TryGetValue(value, out string toString))
			{
				toString = CreateStringRepresentation(value);
				stringRepresentations.Add(value, toString);
			}
			return toString;
		}

		static string CreateStringRepresentation(ulong value)
		{
			//This very first line would suffice...
			StringBuilder builder = new StringBuilder(Convert.ToString((long)value, 2));

			//...but I'm interested in making the string multiple of 8...
			int zerosLeft = builder.Length % bitsPerByte;
			if (zerosLeft > 0)
			{
				zerosLeft = bitsPerByte - zerosLeft;
				for (int i = 0; i < zerosLeft; i++)
					builder.Insert(0, "0");
			}

			//...and separating the bytes for an easier visualization
			int spaces = builder.Length / bitsPerByte;
			for (int i = 1; i < spaces; i++)
				builder.Insert(i * bitsPerByte + i - 1, " ");

			return builder.ToString();
		}
	}
}