using System;
using System.Text;

namespace SickDev.BinaryCompressor {
    public partial struct BinaryNumber: IConvertible, IComparable<BinaryNumber>, IEquatable<BinaryNumber> {
        public const int maxBits = 16;
        public const int bitsPerByte = 8;

        bool[] bits;

        public ulong value { get; private set; }
        public int significantBits { get; private set;}
        int bitsToShow { get { return Math.Min(maxBits, ((significantBits + bitsPerByte - 1) / bitsPerByte) * bitsPerByte); } }

        public BinaryNumber(IConvertible value) {
            this.value = value.ToUInt64(null);
            bits = null;
            significantBits = 0;
            SetBits();
        }

        void SetBits() {
            bits = new bool[maxBits];
            for (int i = bits.Length-1; i >= 0; i--) {
                bits[i] = (value & (1UL << i)) != 0;
                if(significantBits == 0 && bits[i])
                    significantBits = i+1;
            }

            if(significantBits == 0)
                significantBits = 1;
        }

		public byte[] GetBytes() => GetBytes(significantBits);

        public byte[] GetBytes(int significantBits) {
            byte[] bytes = new byte[(int)Math.Ceiling((float)significantBits/bitsPerByte)];
            for (int i = 0; i < bytes.Length; i++) {
                int shift = i * 8;
                BinaryNumber shiftedResult = value >> shift;
                BinaryNumber binaryByte = (byte)shiftedResult.value;
				bytes[i] = (byte)binaryByte.value;
            }
            return bytes;
        }

        public override string ToString() {
            StringBuilder text = new StringBuilder();
            int spaces = 0;
            for (int i = 0; i < bitsToShow; i++){
                text.Insert(0, bits[i] ? "1" : "0");
                if (((text.Length - spaces) % bitsPerByte) == 0 && i < bitsToShow-1) {
                    text.Insert(0, " ");
                    spaces++;
                }
            }
            if ((bitsToShow % bitsPerByte) > 0)
                for (int i = 0; i < bitsPerByte - (bitsToShow % bitsPerByte); i++)
                    text.Insert(0, 0);
            return text.ToString();
        }
    }
}