using System;
using System.Collections.Generic;

namespace SickDev.BinaryCompressor {
    public class BinaryDecompressor {
        byte[] data;
        int sizeBits;
        int _position;
        BinaryNumber currentNumber;
		ulong valuesToRead;

        int position {
            get { return _position; }
            set {
                _position = value;
                UpdateCurrentNumber();
            }
        }

        int byteIndex => position / BinaryNumber.bitsPerByte;
        int bitIndex => position % BinaryNumber.bitsPerByte; //Bit index in byte
        public bool canRead => byteIndex < data.Length && valuesToRead > 0;

        public BinaryDecompressor(byte[] data, IConvertible maxNumber) {
			valuesToRead = BitConverter.ToUInt64(data, 0);
            this.data = new byte[data.Length-sizeof(ulong)];
			Array.Copy(data, sizeof(ulong), this.data, 0, this.data.Length);

			BinaryNumber binaryMaxNumber = new BinaryNumber(maxNumber);
			BinaryNumber binarySignifantBits = new BinaryNumber(binaryMaxNumber.significantBits-1);
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
            int size = ReadSize();
            position += sizeBits;
            BinaryNumber number = ReadInline(size);
            position += size;

			valuesToRead--;
            return number;
        }

        public int ReadSize() => ReadInline(sizeBits)+1;

        BinaryNumber ReadInline(int bits) {
            if (!canRead)
                throw new Exception("There's nothing else to read here");

            BinaryNumber mask = 1;
            for (int i = 0; i < bits - 1; i++) {
                mask <<= 1;
                mask |= 1;
            }
			BinaryNumber value = currentNumber & mask;
            return value;
        }
    }
}