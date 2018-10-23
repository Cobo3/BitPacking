using System;
using System.Text;

namespace SickDev.BinaryCompressor {
    partial class BinaryNumber: IConvertible, IComparable<BinaryNumber>, IEquatable<BinaryNumber> {
        const bool allowNegativeNumbers = false;
        public const int maxBits = 63; //64 would set the negative flag and compute numbers as 2s complement

        bool[] bits;

        public long value { get; private set; }
        public int significantBits { get; protected set;}
        public int contextBits { get; set; } = 8;

        public BinaryNumber(IConvertible value) {
            this.value = value.ToInt64(null);
            SetBits();
        }

        void SetBits() {
            long value = this.value;
            bool isNegative = value < 0;
            if (isNegative) {
                if (!allowNegativeNumbers)
                    throw new Exception("Negative numbers are not supported yet");
                value *= -1;
            }

            bits = new bool[64];
            for (int i = bits.Length-1; i >=0; i--) {
                bits[i] = (value & (1L << i)) != 0;
                if(significantBits == 0 && bits[i])
                    significantBits = i+1;
            }

            //When supported, we use an extra bit to indicate wether the number is positive (0) or negative (1)
            //When it is not, that extra bit is not wasted
            if (allowNegativeNumbers) {
                if (isNegative)
                    bits[significantBits] = true;
                significantBits++;
            }

            if(significantBits == 0)
                significantBits = 1;
        }

        public override string ToString() {
            StringBuilder text = new StringBuilder();
            int length = ((significantBits + contextBits - 1) / contextBits) * contextBits;
            int spaces = 0;
            for (int i = length - 1; i >= 0; i--) {
                text.Append(bits[i] ? "1" : "0");
                if (((text.Length - spaces) % 4) == 0 && i > 0) {
                    text.Append(" ");
                    spaces++;
                }
            }
            return text.ToString();
        }
    }
}