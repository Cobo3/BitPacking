using System;
using DebugBinaryNumber =
#if DEBUG
	SickDev.BinaryStream.BinaryNumber
#else
	System.UInt64
#endif
;

namespace SickDev.BinaryStream
{
	public class BitReader
	{
		readonly long length;
		readonly byte[] data;
		int position;
		int byteIndex;
		int bitIndex;

		public long bitsLeft => length - position;
		public bool canRead => bitsLeft > 0;

		public BitReader(byte[] data)
		{
			this.data = data;
			length = data.LongLength * BinaryNumber.bitsPerByte;
		}

		public ulong Read(int amountOfBits)
		{
			if (position + amountOfBits > length)
				throw new Exception($"Attempting to read {amountOfBits} bits, but there's only {bitsLeft} bits left");

			DebugBinaryNumber value = 0;

			//For every bit we want to read...
			for (int i = 0; i < amountOfBits; i++)
			{
				//...first, get the byte we are currently reading from...
				DebugBinaryNumber @byte = data[byteIndex];
				DebugBinaryNumber mask = MaskUtility.MakeShifted(bitIndex);
				//...and then get the appropiate bit from that byte
				DebugBinaryNumber bit = @byte & mask;

				//And write that bit into the final value
				int shiftAmount = i - bitIndex;
				if (shiftAmount < 0)
					bit >>= -shiftAmount;
				else
					bit <<= shiftAmount;
				value |= bit;

				//Update the bit and byte we next have to read from
				position++;
				byteIndex = position / BinaryNumber.bitsPerByte;
				bitIndex = position % BinaryNumber.bitsPerByte;
			}

			return value;
		}
	}
}