using System;
using System.Collections.Generic;

namespace SickDev.BinaryCompressor {
    public class BinaryDecompressor {
        byte[] data;
        int sizeBits;
        int _position;
        BinaryNumber currentNumber;

        int position {
            get { return _position; }
            set {
                _position = value;
                UpdateCurrentNumber();
            }
        }

        int byteIndex { get { return position / BinaryNumber.bitsPerByte; } }
        int bitIndex { get { return position % BinaryNumber.bitsPerByte; } }
        public bool canRead { get { return byteIndex < data.Length; } }

        public BinaryDecompressor(byte[] data, IConvertible maxNumber) {
            this.data = data;
            sizeBits = new BinaryNumber((new BinaryNumber(maxNumber)).significantBits).significantBits;
            UpdateCurrentNumber();
        }

        void UpdateCurrentNumber() {
            int position = byteIndex * BinaryNumber.bitsPerByte + bitIndex;
            int endPosition = Math.Min(position + BinaryNumber.maxBits, data.Length * BinaryNumber.bitsPerByte);
            int length = endPosition - position;
            currentNumber = 0;
            for (int i = 0; i < length; i++) {
                int bitIndexInData = position + i;
                int byteIndex = bitIndexInData / 8;
                int bitIndexInByte = bitIndexInData % 8;
                BinaryNumber binaryByte = data[byteIndex];
                BinaryNumber mask = (1UL << bitIndexInByte);
                BinaryNumber bitToWrite = binaryByte & mask;
                bitToWrite <<= byteIndex * BinaryNumber.bitsPerByte;
                currentNumber |= bitToWrite;
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
            return number;
        }

        public int ReadSize() {
            return ReadInline(sizeBits);
        }

        BinaryNumber ReadInline(int bits) {
            BinaryNumber mask = 1;
            for (int i = 0; i < bits - 1; i++) {
                mask <<= 1;
                mask |= 1;
            }
            return currentNumber & mask;
        }
    }
}