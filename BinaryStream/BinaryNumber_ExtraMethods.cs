using System;

namespace SickDev.BinaryStream
{
	public partial struct BinaryNumber : IConvertible, IComparable<BinaryNumber>, IEquatable<BinaryNumber>
	{

		#region GetTypeCode
		public TypeCode GetTypeCode() => value.GetTypeCode();

		public bool ToBoolean(IFormatProvider provider) => ((IConvertible)value).ToBoolean(provider);
		public char ToChar(IFormatProvider provider) => ((IConvertible)value).ToChar(provider);
		public sbyte ToSByte(IFormatProvider provider) => ((IConvertible)value).ToSByte(provider);
		public byte ToByte(IFormatProvider provider) => ((IConvertible)value).ToByte(provider);
		public short ToInt16(IFormatProvider provider) => ((IConvertible)value).ToInt16(provider);
		public ushort ToUInt16(IFormatProvider provider) => ((IConvertible)value).ToUInt16(provider);
		public int ToInt32(IFormatProvider provider) => ((IConvertible)value).ToInt32(provider);
		public uint ToUInt32(IFormatProvider provider) => ((IConvertible)value).ToUInt32(provider);
		public long ToInt64(IFormatProvider provider) => ((IConvertible)value).ToInt64(provider);
		public ulong ToUInt64(IFormatProvider provider) => value;
		public float ToSingle(IFormatProvider provider) => ((IConvertible)value).ToSingle(provider);
		public double ToDouble(IFormatProvider provider) => ((IConvertible)value).ToDouble(provider);
		public decimal ToDecimal(IFormatProvider provider) => ((IConvertible)value).ToDecimal(provider);
		public DateTime ToDateTime(IFormatProvider provider) => ((IConvertible)value).ToDateTime(provider);
		public string ToString(IFormatProvider provider) => value.ToString(provider);
		public object ToType(Type conversionType, IFormatProvider provider) => ((IConvertible)value).ToType(conversionType, provider);
		#endregion

		#region Operators
		public static BinaryNumber operator <<(BinaryNumber binary, int bits) => binary.value << bits;
		public static BinaryNumber operator >>(BinaryNumber binary, int bits) => binary.value >> bits;
		public static BinaryNumber operator |(BinaryNumber binary, IConvertible number) => binary.value | number.ToUInt64(null);
		public static BinaryNumber operator &(BinaryNumber binary, IConvertible number) => binary.value & number.ToUInt64(null);
		public static BinaryNumber operator ^(BinaryNumber binary, IConvertible number) => binary.value ^ number.ToUInt64(null);

		public static bool operator ==(BinaryNumber binary, IConvertible number) => binary.value == number.ToUInt64(null);
		public static bool operator !=(BinaryNumber binary, IConvertible number) => binary.value != number.ToUInt64(null);

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