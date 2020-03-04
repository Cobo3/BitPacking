using System;
using DebugBinaryNumber =
#if DEBUG
	SickDev.BitPacking.BinaryNumber
#else
	System.UInt64
#endif
;

namespace SickDev.BitPacking
{
	public class BitReader
	{
		readonly long length;
		readonly byte[] data;
		long byteIndex;
		byte bitIndex;

		long position => byteIndex * BinaryNumber.bitsPerByte + bitIndex;
		public long bitsLeft => length - position;
		public bool canRead => bitsLeft > 0;

		public BitReader(params byte[] data)
		{
			this.data = data;
			length = data.LongLength * BinaryNumber.bitsPerByte;
		}

		public DebugBinaryNumber Read(int bits)
		{
			if (bits < 0 || position + bits > length)
				throw new ArgumentOutOfRangeException($"Attempting to read {bits} bits, but there's only {bitsLeft} bits left");

			DebugBinaryNumber value = 0;

			//For every bit we want to read...
			for (int i = 0; i < bits; i++)
			{
				//...first, get the byte we are currently reading from...
				DebugBinaryNumber @byte = data[byteIndex];
				//...and then get the appropiate bit from that byte
				DebugBinaryNumber mask = MaskUtility.MakeShifted(bitIndex);
				DebugBinaryNumber bit = @byte & mask;

				//Put the bit into the correct position be want to write
				int shiftAmount = i - bitIndex;
				if (shiftAmount < 0)
					bit >>= -shiftAmount;
				else
					bit <<= shiftAmount;

				//And write that bit into the final value
				value |= bit;

				//Update the bit and byte we next have to read from
				bitIndex++;
				if (bitIndex == 8)
				{
					byteIndex++;
					bitIndex = 0;
				}
			}

			return value;
		}
	}
}