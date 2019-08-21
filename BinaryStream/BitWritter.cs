﻿using System;
using System.Text;
using System.Collections.Generic;
using DebugBinaryNumber =
#if DEBUG
	SickDev.BinaryStream.BinaryNumber
#else
	System.UInt64
#endif
;

namespace SickDev.BinaryStream {
    public class BitWriter {
        List<BinaryNumber> numbers = new List<BinaryNumber>();
        int bitsUsed;

        int freeBits => BinaryNumber.maxBits - bitsUsed;

		DebugBinaryNumber currentNumber {
			get => numbers[numbers.Count - 1];
            set => numbers[numbers.Count - 1] = value;
        }

        public BitWriter() {
            CreateNewNumber();
        }

        void CreateNewNumber() {
            numbers.Add(0);
            bitsUsed = 0;
        }

		void WriteValue(BinaryNumber value) => WriteValue(value, value.significantBits);

        void WriteValue(DebugBinaryNumber value, int significantBits) {
			while (significantBits > freeBits) {
                int leftOverBits = significantBits - freeBits;
				DebugBinaryNumber mask = MaskUtility.MakeFilled(freeBits);

                int bitsToShift = freeBits;
				DebugBinaryNumber maskedNumber = value & mask;
                WriteToCurrentNumber(maskedNumber, freeBits);

                value >>= bitsToShift;
                significantBits = leftOverBits;
            }
            WriteToCurrentNumber(value, significantBits);
        }

        void WriteToCurrentNumber(DebugBinaryNumber value, int significantBits) {
            value <<= bitsUsed;
			currentNumber |= value;
            bitsUsed += significantBits;
            if (bitsUsed < 0)
                throw new Exception("We need a long instead of a int for bitsUsed");
            if (freeBits == 0)
                CreateNewNumber();
        }

        public byte[] GetBytes() {
			int numbersCount = numbers.Count;
            byte[][] bytesPerNumber = new byte[numbersCount][];
			ulong totalBytes = 0;
			for (int i = 0; i < numbersCount - 1; i++)
			{
				byte[] bytes = numbers[i].GetBytes(BinaryNumber.maxBits);
				bytesPerNumber[i] = bytes;
				totalBytes += (ulong)bytes.Length;
			}
			byte[] lastBytes = new BinaryNumber(currentNumber).GetBytes(bitsUsed);
			bytesPerNumber[numbersCount - 1] = lastBytes;
			totalBytes += (ulong)lastBytes.Length;

            byte[] result = new byte[totalBytes];
            int index = 0;

            for (int i = 0; i < numbersCount; i++) {
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