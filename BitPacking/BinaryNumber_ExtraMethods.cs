using System;

namespace SickDev.BitPacking
{
	public readonly partial struct BinaryNumber : IConvertible, IComparable<BinaryNumber>, IEquatable<BinaryNumber>
	{

		#region IConvertible
		public TypeCode GetTypeCode() => value.GetTypeCode();

		public bool ToBoolean(IFormatProvider provider = null) => Convert.ToBoolean(value);
		public char ToChar(IFormatProvider provider = null) => Convert.ToChar(value);
		public sbyte ToSByte(IFormatProvider provider = null) => Convert.ToSByte(value);
		public byte ToByte(IFormatProvider provider = null) => Convert.ToByte(value);
		public short ToInt16(IFormatProvider provider = null) => Convert.ToInt16(value);
		public ushort ToUInt16(IFormatProvider provider = null) => Convert.ToUInt16(value);
		public int ToInt32(IFormatProvider provider = null) => Convert.ToInt32(value);
		public uint ToUInt32(IFormatProvider provider = null) => Convert.ToUInt32(value);
		public long ToInt64(IFormatProvider provider = null) => Convert.ToInt64(value);
		public ulong ToUInt64(IFormatProvider provider = null) => value;
		public float ToSingle(IFormatProvider provider = null) => Convert.ToSingle(value);
		public double ToDouble(IFormatProvider provider = null) => Convert.ToDouble(value);
		public decimal ToDecimal(IFormatProvider provider = null) => Convert.ToDecimal(value);
		public DateTime ToDateTime(IFormatProvider provider = null) => Convert.ToDateTime(value);
		public string ToString(IFormatProvider provider) => ToString();
		public object ToType(Type conversionType) => ToType(conversionType);
		public object ToType(Type conversionType, IFormatProvider provider) => Convert.ChangeType(value, conversionType);
		#endregion

		#region Operators
		public static BinaryNumber operator <<(in BinaryNumber binary, int bits) => binary.value << bits;
		public static BinaryNumber operator >>(in BinaryNumber binary, int bits) => binary.value >> bits;
		public static BinaryNumber operator |(in BinaryNumber binary, IConvertible number) => binary.value | ToUInt64(number);
		public static BinaryNumber operator &(in BinaryNumber binary, IConvertible number) => binary.value & ToUInt64(number);
		public static BinaryNumber operator ^(in BinaryNumber binary, IConvertible number) => binary.value ^ ToUInt64(number);

		public static bool operator ==(in BinaryNumber binary, IConvertible number) => binary.value == ToUInt64(number);
		public static bool operator !=(in BinaryNumber binary, IConvertible number) => binary.value != ToUInt64(number);
		public static bool operator >(in BinaryNumber binary, IConvertible number) => binary.value > ToUInt64(number);
		public static bool operator <(in BinaryNumber binary, IConvertible number) => binary.value < ToUInt64(number);
		public static bool operator >=(in BinaryNumber binary, IConvertible number) => binary.value >= ToUInt64(number);
		public static bool operator <=(in BinaryNumber binary, IConvertible number) => binary.value <= ToUInt64(number);

		public static implicit operator BinaryNumber(sbyte number) => new BinaryNumber(number);
		public static implicit operator BinaryNumber(byte number) => new BinaryNumber(number);
		public static implicit operator BinaryNumber(ushort number) => new BinaryNumber(number);
		public static implicit operator BinaryNumber(short number) => new BinaryNumber(number);
		public static implicit operator BinaryNumber(uint number) => new BinaryNumber(number);
		public static implicit operator BinaryNumber(int number) => new BinaryNumber(number);
		public static implicit operator BinaryNumber(ulong number) => new BinaryNumber(number);
		public static implicit operator BinaryNumber(long number) => new BinaryNumber(number);

		public static implicit operator sbyte(in BinaryNumber number) => (sbyte)number.value;
		public static implicit operator byte(in BinaryNumber number) => (byte)number.value;
		public static implicit operator ushort(in BinaryNumber number) => (ushort)number.value;
		public static implicit operator short(in BinaryNumber number) => (short)number.value;
		public static implicit operator uint(in BinaryNumber number) => (uint)number.value;
		public static implicit operator int(in BinaryNumber number) => (int)number.value;
		public static implicit operator ulong(in BinaryNumber number) => number.value;
		public static implicit operator long(in BinaryNumber number) => (long)number.value;
		#endregion

		public int CompareTo(BinaryNumber other) => value.CompareTo(other.value);
		public bool Equals(BinaryNumber other) => value.Equals(other.value);
		public override bool Equals(object obj) => obj is BinaryNumber other && Equals(other);
		public override int GetHashCode() => value.GetHashCode();
	}
}