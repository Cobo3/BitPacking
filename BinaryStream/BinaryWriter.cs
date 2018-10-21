using System.Collections.Generic;

namespace BinaryStream {
    class BinaryWriter {

        List<BinaryNumber> numbers;
        int bitsUsed;

        int freeBits { get { return numbers.Count * BinaryNumber.maxBits - bitsUsed; } }
        BinaryNumber currentNumber {
            get { return numbers[numbers.Count - 1]; }
            set { numbers[numbers.Count - 1] = value; }
        }

        public BinaryWriter() {
            numbers = new List<BinaryNumber>();
            CreateNewNumber();
        }

        void CreateNewNumber() {
            numbers.Add(new BinaryNumber(0));
        }

        void Write(long value, int maxSize) {
            BinaryNumber number = new BinaryNumber(value);
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
                if (number == 0)
                    for (int i = 0; i < leftOverBits - 1; i++)
                        Write(leftOvers);
            }
            Write(number);
        }

        void Write(BinaryNumber number) {
            currentNumber <<= number.significantBits;
            currentNumber |= number.value;
            bitsUsed += number.significantBits;
            if (freeBits == 0)
                CreateNewNumber();
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
    }
}
