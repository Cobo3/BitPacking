using System;
using System.Text;

namespace SickDev.BinaryCompressor {
    partial class BinaryNumber: IConvertible, IComparable<BinaryNumber>, IEquatable<BinaryNumber> {
        public const int maxBits = 64;
        public const int bitsPerByte = 8;

        bool[] bits;

        public ulong value { get; private set; }
        public int significantBits { get; protected set;}
        int bitsToShow { get { return ((significantBits + bitsPerByte - 1) / bitsPerByte) * bitsPerByte; } }

        public BinaryNumber(IConvertible value) {
            this.value = value.ToUInt64(null);
            SetBits();
        }

        void SetBits() {
            bits = new bool[maxBits];
            for (int i = bits.Length-1; i >=0; i--) {
                bits[i] = (value & (1UL << i)) != 0;
                if(significantBits == 0 && bits[i])
                    significantBits = i+1;
            }

            if(significantBits == 0)
                significantBits = 1;
        }

        public byte[] GetBytes() {
            byte[] result = new byte[bitsToShow/bitsPerByte];
            for (int i = 0; i < result.Length; i++)
                result[i] |= (byte)(value >> (i * 8));
            return result;
        }

        public override string ToString() {
            StringBuilder text = new StringBuilder();
            int spaces = 0;
            for (int i = bitsToShow - 1; i >= 0; i--) {
                text.Append(bits[i] ? "1" : "0");
                if (((text.Length - spaces) % bitsPerByte) == 0 && i > 0) {
                    text.Append(" ");
                    spaces++;
                }
            }
            return text.ToString();
        }
    }
}