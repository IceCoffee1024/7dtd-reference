using System;
using System.Diagnostics;
using System.Globalization;

namespace SharpEXR
{
	// Token: 0x020016FC RID: 5884
	[Serializable]
	public struct Half : IComparable, IFormattable, IConvertible, IComparable<Half>, IEquatable<Half>
	{
		// Token: 0x0600B7E2 RID: 47074 RVA: 0x00441DC8 File Offset: 0x0043FFC8
		public Half(float value)
		{
			this = HalfHelper.SingleToHalf(value);
		}

		// Token: 0x0600B7E3 RID: 47075 RVA: 0x00441DD6 File Offset: 0x0043FFD6
		public Half(int value)
		{
			this = new Half((float)value);
		}

		// Token: 0x0600B7E4 RID: 47076 RVA: 0x00441DD6 File Offset: 0x0043FFD6
		public Half(long value)
		{
			this = new Half((float)value);
		}

		// Token: 0x0600B7E5 RID: 47077 RVA: 0x00441DD6 File Offset: 0x0043FFD6
		public Half(double value)
		{
			this = new Half((float)value);
		}

		// Token: 0x0600B7E6 RID: 47078 RVA: 0x00441DE0 File Offset: 0x0043FFE0
		public Half(decimal value)
		{
			this = new Half((float)value);
		}

		// Token: 0x0600B7E7 RID: 47079 RVA: 0x00441DEF File Offset: 0x0043FFEF
		public Half(uint value)
		{
			this = new Half(value);
		}

		// Token: 0x0600B7E8 RID: 47080 RVA: 0x00441DEF File Offset: 0x0043FFEF
		public Half(ulong value)
		{
			this = new Half(value);
		}

		// Token: 0x0600B7E9 RID: 47081 RVA: 0x00441DFA File Offset: 0x0043FFFA
		public static Half Negate(Half half)
		{
			return -half;
		}

		// Token: 0x0600B7EA RID: 47082 RVA: 0x00441E02 File Offset: 0x00440002
		public static Half Add(Half half1, Half half2)
		{
			return half1 + half2;
		}

		// Token: 0x0600B7EB RID: 47083 RVA: 0x00441E0B File Offset: 0x0044000B
		public static Half Subtract(Half half1, Half half2)
		{
			return half1 - half2;
		}

		// Token: 0x0600B7EC RID: 47084 RVA: 0x00441E14 File Offset: 0x00440014
		public static Half Multiply(Half half1, Half half2)
		{
			return half1 * half2;
		}

		// Token: 0x0600B7ED RID: 47085 RVA: 0x00441E1D File Offset: 0x0044001D
		public static Half Divide(Half half1, Half half2)
		{
			return half1 / half2;
		}

		// Token: 0x0600B7EE RID: 47086 RVA: 0x0012AC85 File Offset: 0x00128E85
		public static Half operator +(Half half)
		{
			return half;
		}

		// Token: 0x0600B7EF RID: 47087 RVA: 0x00441E26 File Offset: 0x00440026
		public static Half operator -(Half half)
		{
			return HalfHelper.Negate(half);
		}

		// Token: 0x0600B7F0 RID: 47088 RVA: 0x00441E2E File Offset: 0x0044002E
		public static Half operator ++(Half half)
		{
			return (Half)(half + 1f);
		}

		// Token: 0x0600B7F1 RID: 47089 RVA: 0x00441E41 File Offset: 0x00440041
		public static Half operator --(Half half)
		{
			return (Half)(half - 1f);
		}

		// Token: 0x0600B7F2 RID: 47090 RVA: 0x00441E54 File Offset: 0x00440054
		public static Half operator +(Half half1, Half half2)
		{
			return (Half)(half1 + half2);
		}

		// Token: 0x0600B7F3 RID: 47091 RVA: 0x00441E6A File Offset: 0x0044006A
		public static Half operator -(Half half1, Half half2)
		{
			return (Half)(half1 - half2);
		}

		// Token: 0x0600B7F4 RID: 47092 RVA: 0x00441E80 File Offset: 0x00440080
		public static Half operator *(Half half1, Half half2)
		{
			return (Half)(half1 * half2);
		}

		// Token: 0x0600B7F5 RID: 47093 RVA: 0x00441E96 File Offset: 0x00440096
		public static Half operator /(Half half1, Half half2)
		{
			return (Half)(half1 / half2);
		}

		// Token: 0x0600B7F6 RID: 47094 RVA: 0x00441EAC File Offset: 0x004400AC
		public static bool operator ==(Half half1, Half half2)
		{
			return !Half.IsNaN(half1) && half1.value == half2.value;
		}

		// Token: 0x0600B7F7 RID: 47095 RVA: 0x00441EC6 File Offset: 0x004400C6
		public static bool operator !=(Half half1, Half half2)
		{
			return half1.value != half2.value;
		}

		// Token: 0x0600B7F8 RID: 47096 RVA: 0x00441ED9 File Offset: 0x004400D9
		public static bool operator <(Half half1, Half half2)
		{
			return half1 < half2;
		}

		// Token: 0x0600B7F9 RID: 47097 RVA: 0x00441EEB File Offset: 0x004400EB
		public static bool operator >(Half half1, Half half2)
		{
			return half1 > half2;
		}

		// Token: 0x0600B7FA RID: 47098 RVA: 0x00441EFD File Offset: 0x004400FD
		public static bool operator <=(Half half1, Half half2)
		{
			return half1 == half2 || half1 < half2;
		}

		// Token: 0x0600B7FB RID: 47099 RVA: 0x00441F11 File Offset: 0x00440111
		public static bool operator >=(Half half1, Half half2)
		{
			return half1 == half2 || half1 > half2;
		}

		// Token: 0x0600B7FC RID: 47100 RVA: 0x00441F25 File Offset: 0x00440125
		public static implicit operator Half(byte value)
		{
			return new Half((float)value);
		}

		// Token: 0x0600B7FD RID: 47101 RVA: 0x00441F25 File Offset: 0x00440125
		public static implicit operator Half(short value)
		{
			return new Half((float)value);
		}

		// Token: 0x0600B7FE RID: 47102 RVA: 0x00441F25 File Offset: 0x00440125
		public static implicit operator Half(char value)
		{
			return new Half((float)value);
		}

		// Token: 0x0600B7FF RID: 47103 RVA: 0x00441F25 File Offset: 0x00440125
		public static implicit operator Half(int value)
		{
			return new Half((float)value);
		}

		// Token: 0x0600B800 RID: 47104 RVA: 0x00441F25 File Offset: 0x00440125
		public static implicit operator Half(long value)
		{
			return new Half((float)value);
		}

		// Token: 0x0600B801 RID: 47105 RVA: 0x00441F25 File Offset: 0x00440125
		public static explicit operator Half(float value)
		{
			return new Half(value);
		}

		// Token: 0x0600B802 RID: 47106 RVA: 0x00441F25 File Offset: 0x00440125
		public static explicit operator Half(double value)
		{
			return new Half((float)value);
		}

		// Token: 0x0600B803 RID: 47107 RVA: 0x00441F2E File Offset: 0x0044012E
		public static explicit operator Half(decimal value)
		{
			return new Half((float)value);
		}

		// Token: 0x0600B804 RID: 47108 RVA: 0x00441F3C File Offset: 0x0044013C
		public static explicit operator byte(Half value)
		{
			return (byte)value;
		}

		// Token: 0x0600B805 RID: 47109 RVA: 0x00441F46 File Offset: 0x00440146
		public static explicit operator char(Half value)
		{
			return (char)value;
		}

		// Token: 0x0600B806 RID: 47110 RVA: 0x00441F50 File Offset: 0x00440150
		public static explicit operator short(Half value)
		{
			return (short)value;
		}

		// Token: 0x0600B807 RID: 47111 RVA: 0x00441F5A File Offset: 0x0044015A
		public static explicit operator int(Half value)
		{
			return (int)value;
		}

		// Token: 0x0600B808 RID: 47112 RVA: 0x00441F64 File Offset: 0x00440164
		public static explicit operator long(Half value)
		{
			return (long)value;
		}

		// Token: 0x0600B809 RID: 47113 RVA: 0x00441F6E File Offset: 0x0044016E
		public static implicit operator float(Half value)
		{
			return HalfHelper.HalfToSingle(value);
		}

		// Token: 0x0600B80A RID: 47114 RVA: 0x00441F77 File Offset: 0x00440177
		public static implicit operator double(Half value)
		{
			return (double)value;
		}

		// Token: 0x0600B80B RID: 47115 RVA: 0x00441F81 File Offset: 0x00440181
		public static explicit operator decimal(Half value)
		{
			return (decimal)value;
		}

		// Token: 0x0600B80C RID: 47116 RVA: 0x00441F25 File Offset: 0x00440125
		public static implicit operator Half(sbyte value)
		{
			return new Half((float)value);
		}

		// Token: 0x0600B80D RID: 47117 RVA: 0x00441F25 File Offset: 0x00440125
		public static implicit operator Half(ushort value)
		{
			return new Half((float)value);
		}

		// Token: 0x0600B80E RID: 47118 RVA: 0x00441F8F File Offset: 0x0044018F
		public static implicit operator Half(uint value)
		{
			return new Half(value);
		}

		// Token: 0x0600B80F RID: 47119 RVA: 0x00441F8F File Offset: 0x0044018F
		public static implicit operator Half(ulong value)
		{
			return new Half(value);
		}

		// Token: 0x0600B810 RID: 47120 RVA: 0x00441F99 File Offset: 0x00440199
		public static explicit operator sbyte(Half value)
		{
			return (sbyte)value;
		}

		// Token: 0x0600B811 RID: 47121 RVA: 0x00441F46 File Offset: 0x00440146
		public static explicit operator ushort(Half value)
		{
			return (ushort)value;
		}

		// Token: 0x0600B812 RID: 47122 RVA: 0x00441FA3 File Offset: 0x004401A3
		public static explicit operator uint(Half value)
		{
			return (uint)value;
		}

		// Token: 0x0600B813 RID: 47123 RVA: 0x00441FAD File Offset: 0x004401AD
		public static explicit operator ulong(Half value)
		{
			return (ulong)value;
		}

		// Token: 0x0600B814 RID: 47124 RVA: 0x00441FB8 File Offset: 0x004401B8
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

		// Token: 0x0600B815 RID: 47125 RVA: 0x00442018 File Offset: 0x00440218
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

		// Token: 0x0600B816 RID: 47126 RVA: 0x00442051 File Offset: 0x00440251
		public bool Equals(Half other)
		{
			return other == this || (Half.IsNaN(other) && Half.IsNaN(this));
		}

		// Token: 0x0600B817 RID: 47127 RVA: 0x00442078 File Offset: 0x00440278
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

		// Token: 0x0600B818 RID: 47128 RVA: 0x004420BC File Offset: 0x004402BC
		public override int GetHashCode()
		{
			return this.value.GetHashCode();
		}

		// Token: 0x0600B819 RID: 47129 RVA: 0x0004E4CA File Offset: 0x0004C6CA
		public TypeCode GetTypeCode()
		{
			return (TypeCode)255;
		}

		// Token: 0x0600B81A RID: 47130 RVA: 0x004420C9 File Offset: 0x004402C9
		public static byte[] GetBytes(Half value)
		{
			return BitConverter.GetBytes(value.value);
		}

		// Token: 0x0600B81B RID: 47131 RVA: 0x004420D6 File Offset: 0x004402D6
		public static ushort GetBits(Half value)
		{
			return value.value;
		}

		// Token: 0x0600B81C RID: 47132 RVA: 0x004420DE File Offset: 0x004402DE
		public static Half ToHalf(byte[] value, int startIndex)
		{
			return Half.ToHalf((ushort)BitConverter.ToInt16(value, startIndex));
		}

		// Token: 0x0600B81D RID: 47133 RVA: 0x004420F0 File Offset: 0x004402F0
		public static Half ToHalf(ushort bits)
		{
			return new Half
			{
				value = bits
			};
		}

		// Token: 0x0600B81E RID: 47134 RVA: 0x0044210E File Offset: 0x0044030E
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

		// Token: 0x0600B81F RID: 47135 RVA: 0x0044214A File Offset: 0x0044034A
		public static Half Abs(Half value)
		{
			return HalfHelper.Abs(value);
		}

		// Token: 0x0600B820 RID: 47136 RVA: 0x00442152 File Offset: 0x00440352
		public static Half Max(Half value1, Half value2)
		{
			if (!(value1 < value2))
			{
				return value1;
			}
			return value2;
		}

		// Token: 0x0600B821 RID: 47137 RVA: 0x00442160 File Offset: 0x00440360
		public static Half Min(Half value1, Half value2)
		{
			if (!(value1 < value2))
			{
				return value2;
			}
			return value1;
		}

		// Token: 0x0600B822 RID: 47138 RVA: 0x0044216E File Offset: 0x0044036E
		public static bool IsNaN(Half half)
		{
			return HalfHelper.IsNaN(half);
		}

		// Token: 0x0600B823 RID: 47139 RVA: 0x00442176 File Offset: 0x00440376
		public static bool IsInfinity(Half half)
		{
			return HalfHelper.IsInfinity(half);
		}

		// Token: 0x0600B824 RID: 47140 RVA: 0x0044217E File Offset: 0x0044037E
		public static bool IsNegativeInfinity(Half half)
		{
			return HalfHelper.IsNegativeInfinity(half);
		}

		// Token: 0x0600B825 RID: 47141 RVA: 0x00442186 File Offset: 0x00440386
		public static bool IsPositiveInfinity(Half half)
		{
			return HalfHelper.IsPositiveInfinity(half);
		}

		// Token: 0x0600B826 RID: 47142 RVA: 0x0044218E File Offset: 0x0044038E
		public static Half Parse(string value)
		{
			return (Half)float.Parse(value, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600B827 RID: 47143 RVA: 0x004421A0 File Offset: 0x004403A0
		public static Half Parse(string value, IFormatProvider provider)
		{
			return (Half)float.Parse(value, provider);
		}

		// Token: 0x0600B828 RID: 47144 RVA: 0x004421AE File Offset: 0x004403AE
		public static Half Parse(string value, NumberStyles style)
		{
			return (Half)float.Parse(value, style, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600B829 RID: 47145 RVA: 0x004421C1 File Offset: 0x004403C1
		public static Half Parse(string value, NumberStyles style, IFormatProvider provider)
		{
			return (Half)float.Parse(value, style, provider);
		}

		// Token: 0x0600B82A RID: 47146 RVA: 0x004421D0 File Offset: 0x004403D0
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

		// Token: 0x0600B82B RID: 47147 RVA: 0x00442200 File Offset: 0x00440400
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

		// Token: 0x0600B82C RID: 47148 RVA: 0x00442234 File Offset: 0x00440434
		public override string ToString()
		{
			return this.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x0600B82D RID: 47149 RVA: 0x0044225C File Offset: 0x0044045C
		public string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(formatProvider);
		}

		// Token: 0x0600B82E RID: 47150 RVA: 0x00442280 File Offset: 0x00440480
		public string ToString(string format)
		{
			return this.ToString(format, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600B82F RID: 47151 RVA: 0x004422A8 File Offset: 0x004404A8
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return this.ToString(format, formatProvider);
		}

		// Token: 0x0600B830 RID: 47152 RVA: 0x004422CB File Offset: 0x004404CB
		[PublicizedFrom(EAccessModifier.Private)]
		public float System.IConvertible.ToSingle(IFormatProvider provider)
		{
			return this;
		}

		// Token: 0x0600B831 RID: 47153 RVA: 0x004422D9 File Offset: 0x004404D9
		[PublicizedFrom(EAccessModifier.Private)]
		public TypeCode System.IConvertible.GetTypeCode()
		{
			return this.GetTypeCode();
		}

		// Token: 0x0600B832 RID: 47154 RVA: 0x004422E1 File Offset: 0x004404E1
		[PublicizedFrom(EAccessModifier.Private)]
		public bool System.IConvertible.ToBoolean(IFormatProvider provider)
		{
			return Convert.ToBoolean(this);
		}

		// Token: 0x0600B833 RID: 47155 RVA: 0x004422F4 File Offset: 0x004404F4
		[PublicizedFrom(EAccessModifier.Private)]
		public byte System.IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this);
		}

		// Token: 0x0600B834 RID: 47156 RVA: 0x00442307 File Offset: 0x00440507
		[PublicizedFrom(EAccessModifier.Private)]
		public char System.IConvertible.ToChar(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, "Invalid cast from '{0}' to '{1}'.", "Half", "Char"));
		}

		// Token: 0x0600B835 RID: 47157 RVA: 0x00442327 File Offset: 0x00440527
		[PublicizedFrom(EAccessModifier.Private)]
		public DateTime System.IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, "Invalid cast from '{0}' to '{1}'.", "Half", "DateTime"));
		}

		// Token: 0x0600B836 RID: 47158 RVA: 0x00442347 File Offset: 0x00440547
		[PublicizedFrom(EAccessModifier.Private)]
		public decimal System.IConvertible.ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(this);
		}

		// Token: 0x0600B837 RID: 47159 RVA: 0x0044235A File Offset: 0x0044055A
		[PublicizedFrom(EAccessModifier.Private)]
		public double System.IConvertible.ToDouble(IFormatProvider provider)
		{
			return Convert.ToDouble(this);
		}

		// Token: 0x0600B838 RID: 47160 RVA: 0x0044236D File Offset: 0x0044056D
		[PublicizedFrom(EAccessModifier.Private)]
		public short System.IConvertible.ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this);
		}

		// Token: 0x0600B839 RID: 47161 RVA: 0x00442380 File Offset: 0x00440580
		[PublicizedFrom(EAccessModifier.Private)]
		public int System.IConvertible.ToInt32(IFormatProvider provider)
		{
			return Convert.ToInt32(this);
		}

		// Token: 0x0600B83A RID: 47162 RVA: 0x00442393 File Offset: 0x00440593
		[PublicizedFrom(EAccessModifier.Private)]
		public long System.IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this);
		}

		// Token: 0x0600B83B RID: 47163 RVA: 0x004423A6 File Offset: 0x004405A6
		[PublicizedFrom(EAccessModifier.Private)]
		public sbyte System.IConvertible.ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(this);
		}

		// Token: 0x0600B83C RID: 47164 RVA: 0x004423B9 File Offset: 0x004405B9
		[PublicizedFrom(EAccessModifier.Private)]
		public string System.IConvertible.ToString(IFormatProvider provider)
		{
			return Convert.ToString(this, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600B83D RID: 47165 RVA: 0x004423D1 File Offset: 0x004405D1
		[PublicizedFrom(EAccessModifier.Private)]
		public object System.IConvertible.ToType(Type conversionType, IFormatProvider provider)
		{
			return ((IConvertible)this).ToType(conversionType, provider);
		}

		// Token: 0x0600B83E RID: 47166 RVA: 0x004423EB File Offset: 0x004405EB
		[PublicizedFrom(EAccessModifier.Private)]
		public ushort System.IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this);
		}

		// Token: 0x0600B83F RID: 47167 RVA: 0x004423FE File Offset: 0x004405FE
		[PublicizedFrom(EAccessModifier.Private)]
		public uint System.IConvertible.ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this);
		}

		// Token: 0x0600B840 RID: 47168 RVA: 0x00442411 File Offset: 0x00440611
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong System.IConvertible.ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this);
		}

		// Token: 0x04008942 RID: 35138
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[PublicizedFrom(EAccessModifier.Internal)]
		[NonSerialized]
		public ushort value;

		// Token: 0x04008943 RID: 35139
		public static readonly Half Epsilon = Half.ToHalf(1);

		// Token: 0x04008944 RID: 35140
		public static readonly Half MaxValue = Half.ToHalf(31743);

		// Token: 0x04008945 RID: 35141
		public static readonly Half MinValue = Half.ToHalf(64511);

		// Token: 0x04008946 RID: 35142
		public static readonly Half NaN = Half.ToHalf(65024);

		// Token: 0x04008947 RID: 35143
		public static readonly Half NegativeInfinity = Half.ToHalf(64512);

		// Token: 0x04008948 RID: 35144
		public static readonly Half PositiveInfinity = Half.ToHalf(31744);
	}
}
