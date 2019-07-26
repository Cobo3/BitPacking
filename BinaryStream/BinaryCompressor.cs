using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace SickDev.BinaryCompressor {
    public class BinaryCompressor {
        List<BinaryNumber> numbers = new List<BinaryNumber>();
        ulong maxNumber;
        int maxSignificantBits;
        int bitsUsed;

        int freeBits { get { return BinaryNumber.maxBits - bitsUsed; } }

        BinaryNumber currentNumber {
            get { return numbers[numbers.Count - 1]; }
            set { numbers[numbers.Count - 1] = value; }
        }

        public BinaryCompressor(IConvertible maxNumber) {
            this.maxNumber = maxNumber.ToUInt64(null);
			BinaryNumber binaryMaxNumber = new BinaryNumber(maxNumber);
			BinaryNumber binarySignifantBits = new BinaryNumber(binaryMaxNumber.significantBits-1);
            maxSignificantBits = binarySignifantBits.significantBits;
            CreateNewNumber();
        }

        void CreateNewNumber() {
            numbers.Add(0);
            bitsUsed = 0;
        }

        void WriteValue(ulong value) {
            if (value > maxNumber)
                throw new Exception(string.Format("The input value {0} is greater than the max allowed value {1}", value, maxNumber));

            BinaryNumber number = new BinaryNumber(value);
            //Write first how many significant bits does the number has
            PreProcessWrite(number.significantBits-1, maxSignificantBits);
            //Then write the numbe itself
            PreProcessWrite(number);
        }

        void PreProcessWrite(BinaryNumber number) {
            PreProcessWrite(number, number.significantBits);
        }

        void PreProcessWrite(BinaryNumber number, int significantBits) {
            while (significantBits > freeBits) {
                int leftOverBits = significantBits - freeBits;
                BinaryNumber mask = 1;
                for (int i = 0; i < freeBits-1; i++) {
                    mask <<= 1;
                    mask |= 1;
                }

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

        public void Write(byte value) {
            WriteValue(value);
        }

        public void Write(ushort value) {
            WriteValue(value);
        }

        public void Write(short value) {
            WriteValue((ulong)value);
        }

        public void Write(uint value) {
            WriteValue(value);
        }

        public void Write(int value) {
            WriteValue((ulong)value);
        }

        public void Write(long value) {
            WriteValue((ulong)value);
        }

        public void Write(ulong value) {
            WriteValue(value);
        }

        public byte[] GetBytes() {
            byte[][] bytesPerNumber = new byte[numbers.Count][];
			for (int i = 0; i < bytesPerNumber.Length; i++)
				bytesPerNumber[i] = numbers[i].GetBytes(BinaryNumber.maxBits);

            int index = 0;
            byte[] result = new byte[bytesPerNumber.Sum(x => x.Length)];
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
    }
}