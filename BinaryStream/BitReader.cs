using System;

namespace SickDev.BinaryStream {
    public class BitReader {
        byte[] data;
        int _position;
        BinaryNumber currentNumber;

        int position {
            get { return _position; }
            set {
                _position = value;
                UpdateCurrentNumber();
            }
        }

        int byteIndex => position / BinaryNumber.bitsPerByte;
        int bitIndex => position % BinaryNumber.bitsPerByte; //Bit index in byte
        public bool canRead => byteIndex < data.Length;

        public BitReader(byte[] data) {
			this.data = data;
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
                BinaryNumber mask = MaskUtility.MakeShifted(bitIndexInByte);
                //If it is different than 0, then binaryByte has a "1" bit in that position
                bool writeBit = (binaryByte & mask) != 0;
                if (writeBit)
                    currentNumber |= (1UL << i);
            }
        }

        public BinaryNumber Read(int bits) {
            if (!canRead)
                throw new Exception("There's nothing else to read here");

			BinaryNumber mask = MaskUtility.MakeFilled(bits);
			BinaryNumber value = currentNumber & mask;

            position += bits;
			return value;
        }
    }
}