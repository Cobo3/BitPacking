using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace SickDev.BinaryCompressor {
    public class BinaryCompressor {
        List<BinaryNumber> numbers = new List<BinaryNumber>();
        int bitsUsed;

        int freeBits => BinaryNumber.maxBits - bitsUsed;

        BinaryNumber currentNumber {
			get => numbers[numbers.Count - 1];
            set => numbers[numbers.Count - 1] = value;
        }

        public BinaryCompressor() {
            CreateNewNumber();
        }

        void CreateNewNumber() {
            numbers.Add(0);
            bitsUsed = 0;
        }

		void WriteValue(BinaryNumber value) => WriteValue(value, value.significantBits);

        void WriteValue(BinaryNumber value, int significantBits) {
			while (significantBits > freeBits) {
                int leftOverBits = significantBits - freeBits;
                BinaryNumber mask = MaskUtility.MakeFilled(freeBits);

                int bitsToShift = freeBits;
				BinaryNumber maskedNumber = value & mask;
                WriteToCurrentNumber(maskedNumber, freeBits);

                value >>= bitsToShift;
                significantBits = leftOverBits;
            }
            WriteToCurrentNumber(value, significantBits);
        }

        void WriteToCurrentNumber(BinaryNumber value, int significantBits) {
            value <<= bitsUsed;
			currentNumber |= value;
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

            byte[] result = new byte[bytesPerNumber.Sum(x => x.Length)];
            int index = 0;

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