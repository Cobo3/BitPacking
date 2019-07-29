using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace SickDev.BinaryCompressor {
    public class BinaryCompressor {
        List<BinaryNumber> numbers = new List<BinaryNumber>();
        BinaryNumber maxNumber;
        int maxSignificantBits;
        int bitsUsed;
		ulong valuesWritten;

        int freeBits => BinaryNumber.maxBits - bitsUsed;

        BinaryNumber currentNumber {
			get => numbers[numbers.Count - 1];
            set => numbers[numbers.Count - 1] = value;
        }

        public BinaryCompressor(IConvertible maxNumber) {
            this.maxNumber = maxNumber.ToUInt64(null);
			this.maxNumber += 2;
			BinaryNumber binarySignifantBits = new BinaryNumber(this.maxNumber.significantBits-1);
            maxSignificantBits = binarySignifantBits.significantBits;
            CreateNewNumber();
        }

        void CreateNewNumber() {
            numbers.Add(0);
            bitsUsed = 0;
        }

        void WriteValue(ulong value) {
			value += 2;
            if (value > maxNumber)
                throw new Exception(string.Format("The input value {0} is greater than the max allowed value {1}", value, maxNumber));

            BinaryNumber number = new BinaryNumber(value);
			int significantBits = number.significantBits;

			//Minus 1 to be able to use "0" as a "1"
			significantBits--;

            //Write first how many significant bits does the number has
			//Minus 1 to remove the most significant bit
            PreProcessWrite(significantBits-1, maxSignificantBits);

			//Then remove the most significant bit since it's always 1
			BinaryNumber mask = MaskUtility.MakeFilled(significantBits);
			number &= mask;

			//Then write the numbe itself
			//Minus 1 since we removed the most significant bit
			PreProcessWrite(number, significantBits);
			valuesWritten++;
        }

        void PreProcessWrite(BinaryNumber number, int significantBits) {
            while (significantBits > freeBits) {
                int leftOverBits = significantBits - freeBits;
                BinaryNumber mask = MaskUtility.MakeFilled(freeBits);

                int bitsToShift = freeBits;
				BinaryNumber maskedNumber = number & mask;
                WriteToCurrentNumber(maskedNumber, freeBits);

                number >>= bitsToShift;
                significantBits = leftOverBits;
            }
            WriteToCurrentNumber(number, significantBits);
        }

        void WriteToCurrentNumber(BinaryNumber number, int significantBits) {
            number <<= bitsUsed;
			currentNumber |= number;
            bitsUsed += significantBits;
            if (bitsUsed < 0)
                throw new Exception("We need a long instead of a int for bitsUsed");
            if (freeBits == 0)
                CreateNewNumber();
        }

        public byte[] GetBytes() {
            byte[][] bytesPerNumber = new byte[numbers.Count][];
			for (int i = 0; i < bytesPerNumber.Length-1; i++)
				bytesPerNumber[i] = numbers[i].GetBytes(BinaryNumber.maxBits);
			bytesPerNumber[bytesPerNumber.Length-1] = currentNumber.GetBytes(bitsUsed);

			byte[] valuesWrittenBytes = BitConverter.GetBytes(valuesWritten);
            byte[] result = new byte[bytesPerNumber.Sum(x => x.Length)+valuesWrittenBytes.Length];
			Array.Copy(valuesWrittenBytes, 0, result, 0, valuesWrittenBytes.Length);
            int index = valuesWrittenBytes.Length;

            for (int i = 0; i < bytesPerNumber.Length; i++) {
                for (int j = 0; j < bytesPerNumber[i].Length; j++) {
                    result[index] = bytesPerNumber[i][j];
                    index++;
                }
            }

            return result;
        }

        public override string ToString() {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < numbers.Count; i++) {
                builder.Insert(0, numbers[i].ToString());
                if (i != numbers.Count - 1)
                    builder.Insert(0, " ");
            }
            return builder.ToString();
		}

		public void Write(byte value) => WriteValue(value);
		public void Write(ushort value) => WriteValue(value);
		public void Write(short value) => WriteValue((ulong)value);
		public void Write(uint value) => WriteValue(value);
		public void Write(int value) => WriteValue((ulong)value);
		public void Write(long value) => WriteValue((ulong)value);
		public void Write(ulong value) => WriteValue(value);
	}
}