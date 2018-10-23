using System;
using System.Linq;
using System.Collections.Generic;

namespace SickDev.BinaryCompressor {
    class BinaryCompressor {

        List<BinaryNumber> numbers;
        long maxNumber;
        int maxSignificantBits;
        int bitsUsed;

        int freeBits { get { return numbers.Count * BinaryNumber.maxBits - bitsUsed; } }

        BinaryNumber currentNumber {
            get { return numbers[numbers.Count - 1]; }
            set { numbers[numbers.Count - 1] = value; }
        }

        public BinaryCompressor():this(int.MaxValue) {}
        public BinaryCompressor(long maxNumber) {
            this.maxNumber = maxNumber;
            maxSignificantBits = new BinaryNumber((new BinaryNumber(maxNumber)).significantBits).significantBits;
            numbers = new List<BinaryNumber>();
            CreateNewNumber();
        }

        void CreateNewNumber() {
            numbers.Add(new BinaryNumber(0));
        }

        void Write(long value, int maxSize) {
            if (value > maxNumber)
                throw new System.Exception(string.Format("The input value {0} is greater than the max allowed value {1}", value, maxNumber));

            BinaryNumber number = new BinaryNumber(value);
            number.ToString();
            number.GetBytes();
            while (number.significantBits > freeBits) {
                int leftOverBits = number.significantBits - freeBits;
                BinaryNumber mask = new BinaryNumber(1);
                for (int i = 0; i < leftOverBits - 1; i++) {
                    mask <<= 1;
                    mask |= 1;
                }
                BinaryNumber leftOvers = number & mask;
                number ^= leftOvers;
                number >>= leftOverBits;

                Write(number);
                number = leftOvers;

                //If leftovers are a bunch of 0s, we still have to make sure those are written
                if (number == 0) {
                    for (int i = 0; i < leftOverBits - 1; i++)
                        Write(leftOvers);
                }
            }
            Write(number);
        }

        void Write(BinaryNumber number) {
            void WriteToCurrentNumber(BinaryNumber _number, int significantBits) {
                currentNumber <<= significantBits;
                currentNumber |= _number.value;
                bitsUsed += significantBits;
                if (freeBits == 0)
                    CreateNewNumber();
            }

            //WriteToCurrentNumber(new BinaryNumber(number.significantBits), maxSignificantBits);
            WriteToCurrentNumber(number, number.significantBits);
        }

        public void Write(byte value) {
            Write(value, sizeof(byte));
        }

        public void Write(ushort value) {
            Write(value, sizeof(ushort));
        }

        public void Write(short value) {
            Write(value, sizeof(short));
        }

        public void Write(uint value) {
            Write(value, sizeof(uint));
        }

        public void Write(int value) {
            Write(value, sizeof(int));
        }

        public void Write(long value) {
            Write(value, sizeof(long));
        }

        public byte[] GetBytes() {
            byte[][] bytesPerNumber = new byte[numbers.Count][];
            for (int i = 0; i < bytesPerNumber.Length; i++)
                bytesPerNumber[i] = numbers[i].GetBytes();

            byte[] result = new byte[bytesPerNumber.Sum(x => x.Length)];
            for (int i = 0; i < bytesPerNumber.Length; i++)
                Array.Copy(bytesPerNumber[i], 0, result, i * BinaryNumber.maxBits / BinaryNumber.bitsPerByte, bytesPerNumber[i].Length);
            return result;
        }
    }
}