using System;
using System.Collections.Generic;

namespace SickDev.BinaryCompressor {
    public class BinaryDecompressor {
        byte[] data;
        int sizeBits;
        int _position;
        BinaryNumber currentNumber;
        bool sizeZeroReached;

        int position {
            get { return _position; }
            set {
                _position = value;
                UpdateCurrentNumber();
            }
        }

        int byteIndex { get { return position / BinaryNumber.bitsPerByte; } }
        int bitIndex { get { return position % BinaryNumber.bitsPerByte; } }
        public bool canRead { get { return byteIndex < data.Length && !sizeZeroReached; } }

        public BinaryDecompressor(byte[] data, IConvertible maxNumber) {
            this.data = data;
			BinaryNumber binaryMaxNumber = new BinaryNumber(maxNumber);
			BinaryNumber binarySignifantBits = new BinaryNumber(binaryMaxNumber.significantBits);
			sizeBits = binarySignifantBits.significantBits;
			UpdateCurrentNumber();
        }

        void UpdateCurrentNumber() {
            int position = byteIndex * BinaryNumber.bitsPerByte + bitIndex;
            int endPosition = Math.Min(position + BinaryNumber.maxBits, data.Length * BinaryNumber.bitsPerByte);
            int length = endPosition - position;
            currentNumber = 0;
            for (int i = 0; i < length; i++) {
                int bitIndexInData = position + i;
                int byteIndex = bitIndexInData / BinaryNumber.bitsPerByte;
                int bitIndexInByte = bitIndexInData % BinaryNumber.bitsPerByte;
                BinaryNumber binaryByte = data[byteIndex];
                BinaryNumber mask = (1UL << bitIndexInByte);
                //If it is different than 0, then binaryByte has a "1" bit in that position
                bool writeBit = (binaryByte & mask) != 0;
                if (writeBit)
                    currentNumber |= (1UL << i);
            }
        }

        public BinaryNumber[] ReadAll() {
            List<BinaryNumber> numbers = new List<BinaryNumber>();
            while (canRead)
                numbers.Add(Read());
            return numbers.ToArray();
        }

        public BinaryNumber Read() {
            return ReadNext();
        }

        ulong ReadNext() {
            int size = ReadSize();
            position += sizeBits;
            ulong number = ReadInline(size);
            position += size;

            if (ReadSize() == 0)
                sizeZeroReached = true;

            return number;
        }

        public int ReadSize() {
            return ReadInline(sizeBits);
        }

        BinaryNumber ReadInline(int bits) {
            if (!canRead)
                throw new Exception("There's nothing else to read here");

            BinaryNumber mask = 1;
            for (int i = 0; i < bits - 1; i++) {
                mask <<= 1;
                mask |= 1;
            }
            return currentNumber & mask;
        }
    }
}