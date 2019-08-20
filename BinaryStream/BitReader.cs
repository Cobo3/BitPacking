using System;
using DebugBinaryNumber =
#if DEBUG
	SickDev.BinaryStream.BinaryNumber
#else
	System.UInt64
#endif
;

namespace SickDev.BinaryStream {
    public class BitReader {
		readonly long length;
        readonly byte[] data;
		int position;

		public long bitsLeft => length - position;
        public bool canRead => bitsLeft > 0;

        public BitReader(byte[] data) {
			this.data = data;
			length = data.LongLength * BinaryNumber.bitsPerByte;
        }

        public ulong Read(int bits)
		{
			if (position + bits > length)
				throw new Exception($"Attempting to read {bits} bits, but there's only {bitsLeft} bits left");

			DebugBinaryNumber value = 0;

			int byteIndex = position / BinaryNumber.bitsPerByte;
			int bitIndex = position % BinaryNumber.bitsPerByte;

			for (int i = 0; i < bits; i++)
			{
				DebugBinaryNumber @byte = data[byteIndex];
				DebugBinaryNumber mask = MaskUtility.MakeShifted(bitIndex);
				DebugBinaryNumber maskResult = @byte & mask;
				value |= maskResult << (i - bitIndex);

				position++;
				byteIndex = position / BinaryNumber.bitsPerByte;
				bitIndex = position % BinaryNumber.bitsPerByte;
			}

			return value;
		}
    }
}