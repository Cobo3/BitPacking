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
	public class BitWriter
	{
		List<BinaryNumber> numbers = new List<BinaryNumber>();
		int bitsUsed;

		int freeBits => BinaryNumber.maxBits - bitsUsed;

		DebugBinaryNumber currentNumber
		{
			get => numbers[numbers.Count - 1];
			set => numbers[numbers.Count - 1] = value;
		}

		public BitWriter() => CreateNewNumber();

		void CreateNewNumber()
		{
			numbers.Add(0);
			bitsUsed = 0;
		}

		void WriteValue(BinaryNumber value) => WriteValue(value, value.significantBits);

		//Write only the specified number of bits of the specified value
		void WriteValue(DebugBinaryNumber value, int significantBits)
		{
			//If we don't have enough space to write the whole value...
			if (significantBits > freeBits)
			{
				significantBits -= freeBits;
				DebugBinaryNumber mask = MaskUtility.MakeFilled(freeBits);

				//...write only the first bits of the value...
				int bitsToShift = freeBits;
				DebugBinaryNumber maskedValue = value & mask;
				WriteToCurrentNumber(maskedValue, freeBits);

				//...and then remove those bits...
				value >>= bitsToShift;
			}

			WriteToCurrentNumber(value, significantBits);
		}

		void WriteToCurrentNumber(DebugBinaryNumber value, int significantBits)
		{
			//Write the value at the end of the currentNumber
			value <<= bitsUsed;
			currentNumber |= value;

			bitsUsed += significantBits;

			if (freeBits == 0)
				CreateNewNumber();
		}

		public byte[] GetBytes()
		{
			int numbersCount = numbers.Count;
			byte[][] bytesPerNumber = new byte[numbersCount][];
			ulong totalBytes = 0;

			//For every number FULLY written, get its bytes
			for (int i = 0; i < numbersCount - 1; i++)
			{
				byte[] bytes = numbers[i].GetBytes(BinaryNumber.maxBits);
				bytesPerNumber[i] = bytes;
				totalBytes += (ulong)bytes.Length;
			}

			//Then get the bytes from the current number. We do this out of the for loop because we only want so many bits and not the whole pack
			//Also, the reason why we create a new BinaryNumber is because currentNumber may not be a BinaryNumber itself
			byte[] lastBytes = new BinaryNumber(currentNumber).GetBytes(bitsUsed);
			bytesPerNumber[numbersCount - 1] = lastBytes;
			totalBytes += (ulong)lastBytes.Length;

			//Here we convert the 2D array into the final 1D result
			byte[] result = new byte[totalBytes];
			int index = 0;

			for (int i = 0; i < numbersCount; i++)
			{
				for (int j = 0; j < bytesPerNumber[i].Length; j++)
				{
					result[index] = bytesPerNumber[i][j];
					index++;
				}
			}

			return result;
		}

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			for (int i = 0; i < numbers.Count; i++)
			{
				builder.Insert(0, numbers[i].ToString());
				if (i != numbers.Count - 1)
					builder.Insert(0, " ");
			}
			return builder.ToString();
		}

		public void Write(BinaryNumber value) => WriteValue(value);
		public void Write(byte value) => WriteValue(value);
		public void Write(ushort value) => WriteValue(value);
		public void Write(short value) => WriteValue((ulong)value);
		public void Write(uint value) => WriteValue(value);
		public void Write(int value) => WriteValue((ulong)value);
		public void Write(ulong value) => WriteValue(value);
		public void Write(long value) => WriteValue((ulong)value);
		public void Write(byte value, int bits) => WriteValue(value, bits);
		public void Write(ushort value, int bits) => WriteValue(value, bits);
		public void Write(short value, int bits) => WriteValue((ulong)value, bits);
		public void Write(uint value, int bits) => WriteValue(value, bits);
		public void Write(int value, int bits) => WriteValue((ulong)value, bits);
		public void Write(ulong value, int bits) => WriteValue(value, bits);
		public void Write(long value, int bits) => WriteValue((ulong)value, bits);
	}
}