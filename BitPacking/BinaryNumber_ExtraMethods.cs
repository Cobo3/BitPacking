using System;

namespace SickDev.BitPacking
{
	public readonly partial struct BinaryNumber : IConvertible, IComparable<BinaryNumber>, IEquatable<BinaryNumber>
	{

		#region IConvertible
		public TypeCode GetTypeCode() => value.GetTypeCode();

		public bool ToBoolean() => ToBoolean(null);
		public bool ToBoolean(IFormatProvider provider) => Convert.ToBoolean(value);
		public char ToChar() => ToChar(null);
		public char ToChar(IFormatProvider provider) => Convert.ToChar(value);
		public sbyte ToSByte() => ToSByte(null);
		public sbyte ToSByte(IFormatProvider provider) => Convert.ToSByte(value);
		public byte ToByte() => ToByte(null);
		public byte ToByte(IFormatProvider provider) => Convert.ToByte(value);
		public short ToInt16() => ToInt16(null);
		public short ToInt16(IFormatProvider provider) => Convert.ToInt16(value);
		public ushort ToUInt16() => ToUInt16(null);
		public ushort ToUInt16(IFormatProvider provider) => Convert.ToUInt16(value);
		public int ToInt32() => ToInt32(null);
		public int ToInt32(IFormatProvider provider) => Convert.ToInt32(value);
		public uint ToUInt32() => ToUInt32(null);
		public uint ToUInt32(IFormatProvider provider) => Convert.ToUInt32(value);
		public long ToInt64() => ToInt64(null);
		public long ToInt64(IFormatProvider provider) => Convert.ToInt64(value);
		public ulong ToUInt64() => value;
		public ulong ToUInt64(IFormatProvider provider) => value;
		public float ToSingle() => ToSingle(null);
		public float ToSingle(IFormatProvider provider) => Convert.ToSingle(value);
		public double ToDouble() => ToDouble(null);
		public double ToDouble(IFormatProvider provider) => Convert.ToDouble(value);
		public decimal ToDecimal() => ToDecimal(null);
		public decimal ToDecimal(IFormatProvider provider) => Convert.ToDecimal(value);
		public DateTime ToDateTime() => ToDateTime(null);
		public DateTime ToDateTime(IFormatProvider provider) => Convert.ToDateTime(value);
		public string ToString(IFormatProvider provider) => ToString();
		public object ToType(Type conversionType) => ToType(conversionType);
		public object ToType(Type conversionType, IFormatProvider provider) => Convert.ChangeType(value, conversionType);
		#endregion

		#region Operators
		public static BinaryNumber operator <<(BinaryNumber binary, int bits) => binary.value << bits;
		public static BinaryNumber operator >>(BinaryNumber binary, int bits) => binary.value >> bits;
		public static BinaryNumber operator |(BinaryNumber binary, IConvertible number) => binary.value | number.ToUInt64(null);
		public static BinaryNumber operator &(BinaryNumber binary, IConvertible number) => binary.value & number.ToUInt64(null);
		public static BinaryNumber operator ^(BinaryNumber binary, IConvertible number) => binary.value ^ number.ToUInt64(null);

		public static bool operator ==(BinaryNumber binary, IConvertible number) => binary.value == number.ToUInt64(null);
		public static bool operator !=(BinaryNumber binary, IConvertible number) => binary.value != number.ToUInt64(null);
		public static bool operator >(BinaryNumber binary, IConvertible number) => binary.value > number.ToUInt64(null);
		public static bool operator <(BinaryNumber binary, IConvertible number) => binary.value < number.ToUInt64(null);
		public static bool operator >=(BinaryNumber binary, IConvertible number) => binary.value >= number.ToUInt64(null);
		public static bool operator <=(BinaryNumber binary, IConvertible number) => binary.value <= number.ToUInt64(null);

		public static implicit operator BinaryNumber(sbyte number) => new BinaryNumber(number);
		public static implicit operator BinaryNumber(byte number) => new BinaryNumber(number);
		public static implicit operator BinaryNumber(ushort number) => new BinaryNumber(number);
		public static implicit operator BinaryNumber(short number) => new BinaryNumber(number);
		public static implicit operator BinaryNumber(uint number) => new BinaryNumber(number);
		public static implicit operator BinaryNumber(int number) => new BinaryNumber(number);
		public static implicit operator BinaryNumber(ulong number) => new BinaryNumber(number);
		public static implicit operator BinaryNumber(long number) => new BinaryNumber(number);

		public static implicit operator sbyte(BinaryNumber number) => (sbyte)number.value;
		public static implicit operator byte(BinaryNumber number) => (byte)number.value;
		public static implicit operator ushort(BinaryNumber number) => (ushort)number.value;
		public static implicit operator short(BinaryNumber number) => (short)number.value;
		public static implicit operator uint(BinaryNumber number) => (uint)number.value;
		public static implicit operator int(BinaryNumber number) => (int)number.value;
		public static implicit operator ulong(BinaryNumber number) => number.value;
		public static implicit operator long(BinaryNumber number) => (long)number.value;
		#endregion

		public int CompareTo(BinaryNumber other) => value.CompareTo(other.value);
		public bool Equals(BinaryNumber other) => value.Equals(other.value);
		public override bool Equals(object obj) => value.Equals(obj);
		public override int GetHashCode() => value.GetHashCode();
	}
}