using System;

namespace SickDev.BinaryCompressor {
    public partial struct BinaryNumber : IConvertible, IComparable<BinaryNumber>, IEquatable<BinaryNumber> {

        #region GetTypeCode
        public TypeCode GetTypeCode() {
            return value.GetTypeCode();
        }

        public bool ToBoolean(IFormatProvider provider) {
            return ((IConvertible)value).ToBoolean(provider);
        }

        public char ToChar(IFormatProvider provider) {
            return ((IConvertible)value).ToChar(provider);
        }

        public sbyte ToSByte(IFormatProvider provider) {
            return ((IConvertible)value).ToSByte(provider);
        }

        public byte ToByte(IFormatProvider provider) {
            return ((IConvertible)value).ToByte(provider);
        }

        public short ToInt16(IFormatProvider provider) {
            return ((IConvertible)value).ToInt16(provider);
        }

        public ushort ToUInt16(IFormatProvider provider) {
            return ((IConvertible)value).ToUInt16(provider);
        }

        public int ToInt32(IFormatProvider provider) {
            return ((IConvertible)value).ToInt32(provider);
        }

        public uint ToUInt32(IFormatProvider provider) {
            return ((IConvertible)value).ToUInt32(provider);
        }

        public long ToInt64(IFormatProvider provider) {
            return ((IConvertible)value).ToInt64(provider);
        }

        public ulong ToUInt64(IFormatProvider provider) {
            return value;
        }

        public float ToSingle(IFormatProvider provider) {
            return ((IConvertible)value).ToSingle(provider);
        }

        public double ToDouble(IFormatProvider provider) {
            return ((IConvertible)value).ToDouble(provider);
        }

        public decimal ToDecimal(IFormatProvider provider) {
            return ((IConvertible)value).ToDecimal(provider);
        }

        public DateTime ToDateTime(IFormatProvider provider) {
            return ((IConvertible)value).ToDateTime(provider);
        }

        public string ToString(IFormatProvider provider) {
            return value.ToString(provider);
        }

        public object ToType(Type conversionType, IFormatProvider provider) {
            return ((IConvertible)value).ToType(conversionType, provider);
        }
        #endregion

        #region Operators
        public static BinaryNumber operator <<(BinaryNumber binary, int bits) {
            return binary.value << bits;
        }

        public static BinaryNumber operator >>(BinaryNumber binary, int bits) {
            return binary.value >> bits;
        }

        public static BinaryNumber operator |(BinaryNumber binary, IConvertible number) {
            return binary.value | number.ToUInt64(null);
        }

        public static BinaryNumber operator &(BinaryNumber binary, IConvertible number) {
            return binary.value & number.ToUInt64(null);
        }

        public static BinaryNumber operator ^(BinaryNumber binary, IConvertible number) {
            return binary.value ^ number.ToUInt64(null);
        }

        public static bool operator ==(BinaryNumber binary, IConvertible number) {
            return binary.value == number.ToUInt64(null);
        }

        public static bool operator !=(BinaryNumber binary, IConvertible number) {
            return binary.value != number.ToUInt64(null);
        }

        public static implicit operator BinaryNumber(sbyte number) {
            return new BinaryNumber(number);
        }

        public static implicit operator BinaryNumber(byte number) {
            return new BinaryNumber(number);
        }

        public static implicit operator BinaryNumber(ushort number) {
            return new BinaryNumber(number);
        }

        public static implicit operator BinaryNumber(short number) {
            return new BinaryNumber(number);
        }

        public static implicit operator BinaryNumber(uint number){
            return new BinaryNumber(number);
        }

        public static implicit operator BinaryNumber(int number) {
            return new BinaryNumber(number);
        }

        public static implicit operator BinaryNumber(ulong number) {
            return new BinaryNumber(number);
        }

        public static implicit operator BinaryNumber(long number) {
            return new BinaryNumber(number);
        }

        public static implicit operator sbyte(BinaryNumber number) {
            return (sbyte)number.value;
        }

        public static implicit operator byte(BinaryNumber number) {
            return (byte)number.value;
        }

        public static implicit operator ushort(BinaryNumber number) {
            return (ushort)number.value;
        }

        public static implicit operator short(BinaryNumber number) {
            return (short)number.value;
        }

        public static implicit operator uint(BinaryNumber number) {
            return (uint)number.value;
        }

        public static implicit operator int(BinaryNumber number) {
            return (int) number.value;
        }

        public static implicit operator ulong(BinaryNumber number) {
            return number.value;
        }

        public static implicit operator long(BinaryNumber number) {
            return (long)number.value;
        }
        #endregion

        public int CompareTo(BinaryNumber other) {
            return value.CompareTo(other.value);
        }

        public bool Equals(BinaryNumber other) {
            return value.Equals(other.value);
        }

        public override bool Equals(object obj) {
            return value.Equals(obj);
        }

        public override int GetHashCode() {
            return value.GetHashCode();
        }
    }
}