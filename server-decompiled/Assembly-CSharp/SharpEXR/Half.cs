using System;
using System.Diagnostics;
using System.Globalization;

namespace SharpEXR
{
	// Token: 0x0200171B RID: 5915
	[Serializable]
	public struct Half : IComparable, IFormattable, IConvertible, IComparable<Half>, IEquatable<Half>
	{
		// Token: 0x0600B8B6 RID: 47286 RVA: 0x0044486C File Offset: 0x00442A6C
		public Half(float value)
		{
			this = HalfHelper.SingleToHalf(value);
		}

		// Token: 0x0600B8B7 RID: 47287 RVA: 0x0044487A File Offset: 0x00442A7A
		public Half(int value)
		{
			this = new Half((float)value);
		}

		// Token: 0x0600B8B8 RID: 47288 RVA: 0x0044487A File Offset: 0x00442A7A
		public Half(long value)
		{
			this = new Half((float)value);
		}

		// Token: 0x0600B8B9 RID: 47289 RVA: 0x0044487A File Offset: 0x00442A7A
		public Half(double value)
		{
			this = new Half((float)value);
		}

		// Token: 0x0600B8BA RID: 47290 RVA: 0x00444884 File Offset: 0x00442A84
		public Half(decimal value)
		{
			this = new Half((float)value);
		}

		// Token: 0x0600B8BB RID: 47291 RVA: 0x00444893 File Offset: 0x00442A93
		public Half(uint value)
		{
			this = new Half(value);
		}

		// Token: 0x0600B8BC RID: 47292 RVA: 0x00444893 File Offset: 0x00442A93
		public Half(ulong value)
		{
			this = new Half(value);
		}

		// Token: 0x0600B8BD RID: 47293 RVA: 0x0044489E File Offset: 0x00442A9E
		public static Half Negate(Half half)
		{
			return -half;
		}

		// Token: 0x0600B8BE RID: 47294 RVA: 0x004448A6 File Offset: 0x00442AA6
		public static Half Add(Half half1, Half half2)
		{
			return half1 + half2;
		}

		// Token: 0x0600B8BF RID: 47295 RVA: 0x004448AF File Offset: 0x00442AAF
		public static Half Subtract(Half half1, Half half2)
		{
			return half1 - half2;
		}

		// Token: 0x0600B8C0 RID: 47296 RVA: 0x004448B8 File Offset: 0x00442AB8
		public static Half Multiply(Half half1, Half half2)
		{
			return half1 * half2;
		}

		// Token: 0x0600B8C1 RID: 47297 RVA: 0x004448C1 File Offset: 0x00442AC1
		public static Half Divide(Half half1, Half half2)
		{
			return half1 / half2;
		}

		// Token: 0x0600B8C2 RID: 47298 RVA: 0x0012B879 File Offset: 0x00129A79
		public static Half operator +(Half half)
		{
			return half;
		}

		// Token: 0x0600B8C3 RID: 47299 RVA: 0x004448CA File Offset: 0x00442ACA
		public static Half operator -(Half half)
		{
			return HalfHelper.Negate(half);
		}

		// Token: 0x0600B8C4 RID: 47300 RVA: 0x004448D2 File Offset: 0x00442AD2
		public static Half operator ++(Half half)
		{
			return (Half)(half + 1f);
		}

		// Token: 0x0600B8C5 RID: 47301 RVA: 0x004448E5 File Offset: 0x00442AE5
		public static Half operator --(Half half)
		{
			return (Half)(half - 1f);
		}

		// Token: 0x0600B8C6 RID: 47302 RVA: 0x004448F8 File Offset: 0x00442AF8
		public static Half operator +(Half half1, Half half2)
		{
			return (Half)(half1 + half2);
		}

		// Token: 0x0600B8C7 RID: 47303 RVA: 0x0044490E File Offset: 0x00442B0E
		public static Half operator -(Half half1, Half half2)
		{
			return (Half)(half1 - half2);
		}

		// Token: 0x0600B8C8 RID: 47304 RVA: 0x00444924 File Offset: 0x00442B24
		public static Half operator *(Half half1, Half half2)
		{
			return (Half)(half1 * half2);
		}

		// Token: 0x0600B8C9 RID: 47305 RVA: 0x0044493A File Offset: 0x00442B3A
		public static Half operator /(Half half1, Half half2)
		{
			return (Half)(half1 / half2);
		}

		// Token: 0x0600B8CA RID: 47306 RVA: 0x00444950 File Offset: 0x00442B50
		public static bool operator ==(Half half1, Half half2)
		{
			return !Half.IsNaN(half1) && half1.value == half2.value;
		}

		// Token: 0x0600B8CB RID: 47307 RVA: 0x0044496A File Offset: 0x00442B6A
		public static bool operator !=(Half half1, Half half2)
		{
			return half1.value != half2.value;
		}

		// Token: 0x0600B8CC RID: 47308 RVA: 0x0044497D File Offset: 0x00442B7D
		public static bool operator <(Half half1, Half half2)
		{
			return half1 < half2;
		}

		// Token: 0x0600B8CD RID: 47309 RVA: 0x0044498F File Offset: 0x00442B8F
		public static bool operator >(Half half1, Half half2)
		{
			return half1 > half2;
		}

		// Token: 0x0600B8CE RID: 47310 RVA: 0x004449A1 File Offset: 0x00442BA1
		public static bool operator <=(Half half1, Half half2)
		{
			return half1 == half2 || half1 < half2;
		}

		// Token: 0x0600B8CF RID: 47311 RVA: 0x004449B5 File Offset: 0x00442BB5
		public static bool operator >=(Half half1, Half half2)
		{
			return half1 == half2 || half1 > half2;
		}

		// Token: 0x0600B8D0 RID: 47312 RVA: 0x004449C9 File Offset: 0x00442BC9
		public static implicit operator Half(byte value)
		{
			return new Half((float)value);
		}

		// Token: 0x0600B8D1 RID: 47313 RVA: 0x004449C9 File Offset: 0x00442BC9
		public static implicit operator Half(short value)
		{
			return new Half((float)value);
		}

		// Token: 0x0600B8D2 RID: 47314 RVA: 0x004449C9 File Offset: 0x00442BC9
		public static implicit operator Half(char value)
		{
			return new Half((float)value);
		}

		// Token: 0x0600B8D3 RID: 47315 RVA: 0x004449C9 File Offset: 0x00442BC9
		public static implicit operator Half(int value)
		{
			return new Half((float)value);
		}

		// Token: 0x0600B8D4 RID: 47316 RVA: 0x004449C9 File Offset: 0x00442BC9
		public static implicit operator Half(long value)
		{
			return new Half((float)value);
		}

		// Token: 0x0600B8D5 RID: 47317 RVA: 0x004449C9 File Offset: 0x00442BC9
		public static explicit operator Half(float value)
		{
			return new Half(value);
		}

		// Token: 0x0600B8D6 RID: 47318 RVA: 0x004449C9 File Offset: 0x00442BC9
		public static explicit operator Half(double value)
		{
			return new Half((float)value);
		}

		// Token: 0x0600B8D7 RID: 47319 RVA: 0x004449D2 File Offset: 0x00442BD2
		public static explicit operator Half(decimal value)
		{
			return new Half((float)value);
		}

		// Token: 0x0600B8D8 RID: 47320 RVA: 0x004449E0 File Offset: 0x00442BE0
		public static explicit operator byte(Half value)
		{
			return (byte)value;
		}

		// Token: 0x0600B8D9 RID: 47321 RVA: 0x004449EA File Offset: 0x00442BEA
		public static explicit operator char(Half value)
		{
			return (char)value;
		}

		// Token: 0x0600B8DA RID: 47322 RVA: 0x004449F4 File Offset: 0x00442BF4
		public static explicit operator short(Half value)
		{
			return (short)value;
		}

		// Token: 0x0600B8DB RID: 47323 RVA: 0x004449FE File Offset: 0x00442BFE
		public static explicit operator int(Half value)
		{
			return (int)value;
		}

		// Token: 0x0600B8DC RID: 47324 RVA: 0x00444A08 File Offset: 0x00442C08
		public static explicit operator long(Half value)
		{
			return (long)value;
		}

		// Token: 0x0600B8DD RID: 47325 RVA: 0x00444A12 File Offset: 0x00442C12
		public static implicit operator float(Half value)
		{
			return HalfHelper.HalfToSingle(value);
		}

		// Token: 0x0600B8DE RID: 47326 RVA: 0x00444A1B File Offset: 0x00442C1B
		public static implicit operator double(Half value)
		{
			return (double)value;
		}

		// Token: 0x0600B8DF RID: 47327 RVA: 0x00444A25 File Offset: 0x00442C25
		public static explicit operator decimal(Half value)
		{
			return (decimal)value;
		}

		// Token: 0x0600B8E0 RID: 47328 RVA: 0x004449C9 File Offset: 0x00442BC9
		public static implicit operator Half(sbyte value)
		{
			return new Half((float)value);
		}

		// Token: 0x0600B8E1 RID: 47329 RVA: 0x004449C9 File Offset: 0x00442BC9
		public static implicit operator Half(ushort value)
		{
			return new Half((float)value);
		}

		// Token: 0x0600B8E2 RID: 47330 RVA: 0x00444A33 File Offset: 0x00442C33
		public static implicit operator Half(uint value)
		{
			return new Half(value);
		}

		// Token: 0x0600B8E3 RID: 47331 RVA: 0x00444A33 File Offset: 0x00442C33
		public static implicit operator Half(ulong value)
		{
			return new Half(value);
		}

		// Token: 0x0600B8E4 RID: 47332 RVA: 0x00444A3D File Offset: 0x00442C3D
		public static explicit operator sbyte(Half value)
		{
			return (sbyte)value;
		}

		// Token: 0x0600B8E5 RID: 47333 RVA: 0x004449EA File Offset: 0x00442BEA
		public static explicit operator ushort(Half value)
		{
			return (ushort)value;
		}

		// Token: 0x0600B8E6 RID: 47334 RVA: 0x00444A47 File Offset: 0x00442C47
		public static explicit operator uint(Half value)
		{
			return (uint)value;
		}

		// Token: 0x0600B8E7 RID: 47335 RVA: 0x00444A51 File Offset: 0x00442C51
		public static explicit operator ulong(Half value)
		{
			return (ulong)value;
		}

		// Token: 0x0600B8E8 RID: 47336 RVA: 0x00444A5C File Offset: 0x00442C5C
		public int CompareTo(Half other)
		{
			int num = 0;
			if (this < other)
			{
				num = -1;
			}
			else if (this > other)
			{
				num = 1;
			}
			else if (this != other)
			{
				if (!Half.IsNaN(this))
				{
					num = 1;
				}
				else if (!Half.IsNaN(other))
				{
					num = -1;
				}
			}
			return num;
		}

		// Token: 0x0600B8E9 RID: 47337 RVA: 0x00444ABC File Offset: 0x00442CBC
		public int CompareTo(object obj)
		{
			int num;
			if (obj == null)
			{
				num = 1;
			}
			else
			{
				if (!(obj is Half))
				{
					throw new ArgumentException("Object must be of type Half.");
				}
				num = this.CompareTo((Half)obj);
			}
			return num;
		}

		// Token: 0x0600B8EA RID: 47338 RVA: 0x00444AF5 File Offset: 0x00442CF5
		public bool Equals(Half other)
		{
			return other == this || (Half.IsNaN(other) && Half.IsNaN(this));
		}

		// Token: 0x0600B8EB RID: 47339 RVA: 0x00444B1C File Offset: 0x00442D1C
		public override bool Equals(object obj)
		{
			bool flag = false;
			if (obj is Half)
			{
				Half half = (Half)obj;
				if (half == this || (Half.IsNaN(half) && Half.IsNaN(this)))
				{
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x0600B8EC RID: 47340 RVA: 0x00444B60 File Offset: 0x00442D60
		public override int GetHashCode()
		{
			return this.value.GetHashCode();
		}

		// Token: 0x0600B8ED RID: 47341 RVA: 0x0004E48A File Offset: 0x0004C68A
		public TypeCode GetTypeCode()
		{
			return (TypeCode)255;
		}

		// Token: 0x0600B8EE RID: 47342 RVA: 0x00444B6D File Offset: 0x00442D6D
		public static byte[] GetBytes(Half value)
		{
			return BitConverter.GetBytes(value.value);
		}

		// Token: 0x0600B8EF RID: 47343 RVA: 0x00444B7A File Offset: 0x00442D7A
		public static ushort GetBits(Half value)
		{
			return value.value;
		}

		// Token: 0x0600B8F0 RID: 47344 RVA: 0x00444B82 File Offset: 0x00442D82
		public static Half ToHalf(byte[] value, int startIndex)
		{
			return Half.ToHalf((ushort)BitConverter.ToInt16(value, startIndex));
		}

		// Token: 0x0600B8F1 RID: 47345 RVA: 0x00444B94 File Offset: 0x00442D94
		public static Half ToHalf(ushort bits)
		{
			return new Half
			{
				value = bits
			};
		}

		// Token: 0x0600B8F2 RID: 47346 RVA: 0x00444BB2 File Offset: 0x00442DB2
		public static int Sign(Half value)
		{
			if (value < 0)
			{
				return -1;
			}
			if (value > 0)
			{
				return 1;
			}
			if (value != 0)
			{
				throw new ArithmeticException("Function does not accept floating point Not-a-Number values.");
			}
			return 0;
		}

		// Token: 0x0600B8F3 RID: 47347 RVA: 0x00444BEE File Offset: 0x00442DEE
		public static Half Abs(Half value)
		{
			return HalfHelper.Abs(value);
		}

		// Token: 0x0600B8F4 RID: 47348 RVA: 0x00444BF6 File Offset: 0x00442DF6
		public static Half Max(Half value1, Half value2)
		{
			if (!(value1 < value2))
			{
				return value1;
			}
			return value2;
		}

		// Token: 0x0600B8F5 RID: 47349 RVA: 0x00444C04 File Offset: 0x00442E04
		public static Half Min(Half value1, Half value2)
		{
			if (!(value1 < value2))
			{
				return value2;
			}
			return value1;
		}

		// Token: 0x0600B8F6 RID: 47350 RVA: 0x00444C12 File Offset: 0x00442E12
		public static bool IsNaN(Half half)
		{
			return HalfHelper.IsNaN(half);
		}

		// Token: 0x0600B8F7 RID: 47351 RVA: 0x00444C1A File Offset: 0x00442E1A
		public static bool IsInfinity(Half half)
		{
			return HalfHelper.IsInfinity(half);
		}

		// Token: 0x0600B8F8 RID: 47352 RVA: 0x00444C22 File Offset: 0x00442E22
		public static bool IsNegativeInfinity(Half half)
		{
			return HalfHelper.IsNegativeInfinity(half);
		}

		// Token: 0x0600B8F9 RID: 47353 RVA: 0x00444C2A File Offset: 0x00442E2A
		public static bool IsPositiveInfinity(Half half)
		{
			return HalfHelper.IsPositiveInfinity(half);
		}

		// Token: 0x0600B8FA RID: 47354 RVA: 0x00444C32 File Offset: 0x00442E32
		public static Half Parse(string value)
		{
			return (Half)float.Parse(value, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600B8FB RID: 47355 RVA: 0x00444C44 File Offset: 0x00442E44
		public static Half Parse(string value, IFormatProvider provider)
		{
			return (Half)float.Parse(value, provider);
		}

		// Token: 0x0600B8FC RID: 47356 RVA: 0x00444C52 File Offset: 0x00442E52
		public static Half Parse(string value, NumberStyles style)
		{
			return (Half)float.Parse(value, style, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600B8FD RID: 47357 RVA: 0x00444C65 File Offset: 0x00442E65
		public static Half Parse(string value, NumberStyles style, IFormatProvider provider)
		{
			return (Half)float.Parse(value, style, provider);
		}

		// Token: 0x0600B8FE RID: 47358 RVA: 0x00444C74 File Offset: 0x00442E74
		public static bool TryParse(string value, out Half result)
		{
			float num;
			if (float.TryParse(value, out num))
			{
				result = (Half)num;
				return true;
			}
			result = default(Half);
			return false;
		}

		// Token: 0x0600B8FF RID: 47359 RVA: 0x00444CA4 File Offset: 0x00442EA4
		public static bool TryParse(string value, NumberStyles style, IFormatProvider provider, out Half result)
		{
			bool flag = false;
			float num;
			if (float.TryParse(value, style, provider, out num))
			{
				result = (Half)num;
				flag = true;
			}
			else
			{
				result = default(Half);
			}
			return flag;
		}

		// Token: 0x0600B900 RID: 47360 RVA: 0x00444CD8 File Offset: 0x00442ED8
		public override string ToString()
		{
			return this.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x0600B901 RID: 47361 RVA: 0x00444D00 File Offset: 0x00442F00
		public string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(formatProvider);
		}

		// Token: 0x0600B902 RID: 47362 RVA: 0x00444D24 File Offset: 0x00442F24
		public string ToString(string format)
		{
			return this.ToString(format, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600B903 RID: 47363 RVA: 0x00444D4C File Offset: 0x00442F4C
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return this.ToString(format, formatProvider);
		}

		// Token: 0x0600B904 RID: 47364 RVA: 0x00444D6F File Offset: 0x00442F6F
		[PublicizedFrom(EAccessModifier.Private)]
		public float System.IConvertible.ToSingle(IFormatProvider provider)
		{
			return this;
		}

		// Token: 0x0600B905 RID: 47365 RVA: 0x00444D7D File Offset: 0x00442F7D
		[PublicizedFrom(EAccessModifier.Private)]
		public TypeCode System.IConvertible.GetTypeCode()
		{
			return this.GetTypeCode();
		}

		// Token: 0x0600B906 RID: 47366 RVA: 0x00444D85 File Offset: 0x00442F85
		[PublicizedFrom(EAccessModifier.Private)]
		public bool System.IConvertible.ToBoolean(IFormatProvider provider)
		{
			return Convert.ToBoolean(this);
		}

		// Token: 0x0600B907 RID: 47367 RVA: 0x00444D98 File Offset: 0x00442F98
		[PublicizedFrom(EAccessModifier.Private)]
		public byte System.IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this);
		}

		// Token: 0x0600B908 RID: 47368 RVA: 0x00444DAB File Offset: 0x00442FAB
		[PublicizedFrom(EAccessModifier.Private)]
		public char System.IConvertible.ToChar(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, "Invalid cast from '{0}' to '{1}'.", "Half", "Char"));
		}

		// Token: 0x0600B909 RID: 47369 RVA: 0x00444DCB File Offset: 0x00442FCB
		[PublicizedFrom(EAccessModifier.Private)]
		public DateTime System.IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, "Invalid cast from '{0}' to '{1}'.", "Half", "DateTime"));
		}

		// Token: 0x0600B90A RID: 47370 RVA: 0x00444DEB File Offset: 0x00442FEB
		[PublicizedFrom(EAccessModifier.Private)]
		public decimal System.IConvertible.ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(this);
		}

		// Token: 0x0600B90B RID: 47371 RVA: 0x00444DFE File Offset: 0x00442FFE
		[PublicizedFrom(EAccessModifier.Private)]
		public double System.IConvertible.ToDouble(IFormatProvider provider)
		{
			return Convert.ToDouble(this);
		}

		// Token: 0x0600B90C RID: 47372 RVA: 0x00444E11 File Offset: 0x00443011
		[PublicizedFrom(EAccessModifier.Private)]
		public short System.IConvertible.ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this);
		}

		// Token: 0x0600B90D RID: 47373 RVA: 0x00444E24 File Offset: 0x00443024
		[PublicizedFrom(EAccessModifier.Private)]
		public int System.IConvertible.ToInt32(IFormatProvider provider)
		{
			return Convert.ToInt32(this);
		}

		// Token: 0x0600B90E RID: 47374 RVA: 0x00444E37 File Offset: 0x00443037
		[PublicizedFrom(EAccessModifier.Private)]
		public long System.IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this);
		}

		// Token: 0x0600B90F RID: 47375 RVA: 0x00444E4A File Offset: 0x0044304A
		[PublicizedFrom(EAccessModifier.Private)]
		public sbyte System.IConvertible.ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(this);
		}

		// Token: 0x0600B910 RID: 47376 RVA: 0x00444E5D File Offset: 0x0044305D
		[PublicizedFrom(EAccessModifier.Private)]
		public string System.IConvertible.ToString(IFormatProvider provider)
		{
			return Convert.ToString(this, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600B911 RID: 47377 RVA: 0x00444E75 File Offset: 0x00443075
		[PublicizedFrom(EAccessModifier.Private)]
		public object System.IConvertible.ToType(Type conversionType, IFormatProvider provider)
		{
			return ((IConvertible)this).ToType(conversionType, provider);
		}

		// Token: 0x0600B912 RID: 47378 RVA: 0x00444E8F File Offset: 0x0044308F
		[PublicizedFrom(EAccessModifier.Private)]
		public ushort System.IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this);
		}

		// Token: 0x0600B913 RID: 47379 RVA: 0x00444EA2 File Offset: 0x004430A2
		[PublicizedFrom(EAccessModifier.Private)]
		public uint System.IConvertible.ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this);
		}

		// Token: 0x0600B914 RID: 47380 RVA: 0x00444EB5 File Offset: 0x004430B5
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong System.IConvertible.ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this);
		}

		// Token: 0x040089F5 RID: 35317
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[PublicizedFrom(EAccessModifier.Internal)]
		[NonSerialized]
		public ushort value;

		// Token: 0x040089F6 RID: 35318
		public static readonly Half Epsilon = Half.ToHalf(1);

		// Token: 0x040089F7 RID: 35319
		public static readonly Half MaxValue = Half.ToHalf(31743);

		// Token: 0x040089F8 RID: 35320
		public static readonly Half MinValue = Half.ToHalf(64511);

		// Token: 0x040089F9 RID: 35321
		public static readonly Half NaN = Half.ToHalf(65024);

		// Token: 0x040089FA RID: 35322
		public static readonly Half NegativeInfinity = Half.ToHalf(64512);

		// Token: 0x040089FB RID: 35323
		public static readonly Half PositiveInfinity = Half.ToHalf(31744);
	}
}
