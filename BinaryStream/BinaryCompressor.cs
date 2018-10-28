using System;
using System.Linq;
using System.Collections.Generic;

namespace SickDev.BinaryCompressor {
    class BinaryCompressor {

        List<BinaryNumber> numbers;
        ulong maxNumber;
        int maxSignificantBits;
        int bitsUsed;

        int freeBits { get { return numbers.Count * BinaryNumber.maxBits - bitsUsed; } }

        BinaryNumber currentNumber {
            get { return numbers[numbers.Count - 1]; }
            set { numbers[numbers.Count - 1] = value; }
        }

        public BinaryCompressor():this(uint.MaxValue) {}
        public BinaryCompressor(ulong maxNumber) {
            this.maxNumber = maxNumber;
            maxSignificantBits = new BinaryNumber((new BinaryNumber(maxNumber)).significantBits).significantBits;
            numbers = new List<BinaryNumber>();
            CreateNewNumber();
        }

        void CreateNewNumber() {
            numbers.Add(new BinaryNumber(0));
        }

        void WriteValue(ulong value) {
            if (value > maxNumber)
                throw new Exception(string.Format("The input value {0} is greater than the max allowed value {1}", value, maxNumber));

            BinaryNumber number = new BinaryNumber(value);
            PreProcessWrite(new BinaryNumber(number.significantBits), maxSignificantBits);
            PreProcessWrite(number);
        }

        void PreProcessWrite(BinaryNumber number) {
            PreProcessWrite(number, number.significantBits);
        }

        void PreProcessWrite(BinaryNumber number, int significantBits) {
            while (significantBits > freeBits) {
                int leftOverBits = significantBits - freeBits;
                ulong mask = 1;
                for (int i = 0; i < leftOverBits - 1; i++) {
                    mask <<= 1;
                    mask |= 1;
                }
                ulong leftOvers = number.value & mask;
                number ^= leftOvers;
                number >>= leftOverBits;

                WriteToCurrentNumber(number, number.significantBits);
                number = new BinaryNumber(leftOvers);
                significantBits = leftOverBits;
            }
            WriteToCurrentNumber(number, significantBits);
        }

        void WriteToCurrentNumber(BinaryNumber number, int significantBits) {
            currentNumber <<= significantBits;
            currentNumber |= number;
            bitsUsed += significantBits;
            if (freeBits == 0)
                CreateNewNumber();
        }

        public void Write(byte value) {
            WriteValue((ulong)value);
        }

        public void Write(ushort value) {
            WriteValue((ulong)value);
        }

        public void Write(short value) {
            WriteValue((ulong)value);
        }

        public void Write(uint value) {
            WriteValue((ulong)value);
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
                bytesPerNumber[i] = numbers[i].GetBytes();

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
    }
}