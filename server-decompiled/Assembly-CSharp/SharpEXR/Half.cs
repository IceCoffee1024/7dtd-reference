using System;
using System.Diagnostics;
using System.Globalization;

namespace SharpEXR
{
	// Token: 0x020016D0 RID: 5840
	[Serializable]
	public struct Half : IComparable, IFormattable, IConvertible, IComparable<Half>, IEquatable<Half>
	{
		// Token: 0x0600B68B RID: 46731 RVA: 0x0043D8A8 File Offset: 0x0043BAA8
		public Half(float value)
		{
			this = HalfHelper.SingleToHalf(value);
		}

		// Token: 0x0600B68C RID: 46732 RVA: 0x0043D8B6 File Offset: 0x0043BAB6
		public Half(int value)
		{
			this = new Half((float)value);
		}

		// Token: 0x0600B68D RID: 46733 RVA: 0x0043D8B6 File Offset: 0x0043BAB6
		public Half(long value)
		{
			this = new Half((float)value);
		}

		// Token: 0x0600B68E RID: 46734 RVA: 0x0043D8B6 File Offset: 0x0043BAB6
		public Half(double value)
		{
			this = new Half((float)value);
		}

		// Token: 0x0600B68F RID: 46735 RVA: 0x0043D8C0 File Offset: 0x0043BAC0
		public Half(decimal value)
		{
			this = new Half((float)value);
		}

		// Token: 0x0600B690 RID: 46736 RVA: 0x0043D8CF File Offset: 0x0043BACF
		public Half(uint value)
		{
			this = new Half(value);
		}

		// Token: 0x0600B691 RID: 46737 RVA: 0x0043D8CF File Offset: 0x0043BACF
		public Half(ulong value)
		{
			this = new Half(value);
		}

		// Token: 0x0600B692 RID: 46738 RVA: 0x0043D8DA File Offset: 0x0043BADA
		public static Half Negate(Half half)
		{
			return -half;
		}

		// Token: 0x0600B693 RID: 46739 RVA: 0x0043D8E2 File Offset: 0x0043BAE2
		public static Half Add(Half half1, Half half2)
		{
			return half1 + half2;
		}

		// Token: 0x0600B694 RID: 46740 RVA: 0x0043D8EB File Offset: 0x0043BAEB
		public static Half Subtract(Half half1, Half half2)
		{
			return half1 - half2;
		}

		// Token: 0x0600B695 RID: 46741 RVA: 0x0043D8F4 File Offset: 0x0043BAF4
		public static Half Multiply(Half half1, Half half2)
		{
			return half1 * half2;
		}

		// Token: 0x0600B696 RID: 46742 RVA: 0x0043D8FD File Offset: 0x0043BAFD
		public static Half Divide(Half half1, Half half2)
		{
			return half1 / half2;
		}

		// Token: 0x0600B697 RID: 46743 RVA: 0x0012A165 File Offset: 0x00128365
		public static Half operator +(Half half)
		{
			return half;
		}

		// Token: 0x0600B698 RID: 46744 RVA: 0x0043D906 File Offset: 0x0043BB06
		public static Half operator -(Half half)
		{
			return HalfHelper.Negate(half);
		}

		// Token: 0x0600B699 RID: 46745 RVA: 0x0043D90E File Offset: 0x0043BB0E
		public static Half operator ++(Half half)
		{
			return (Half)(half + 1f);
		}

		// Token: 0x0600B69A RID: 46746 RVA: 0x0043D921 File Offset: 0x0043BB21
		public static Half operator --(Half half)
		{
			return (Half)(half - 1f);
		}

		// Token: 0x0600B69B RID: 46747 RVA: 0x0043D934 File Offset: 0x0043BB34
		public static Half operator +(Half half1, Half half2)
		{
			return (Half)(half1 + half2);
		}

		// Token: 0x0600B69C RID: 46748 RVA: 0x0043D94A File Offset: 0x0043BB4A
		public static Half operator -(Half half1, Half half2)
		{
			return (Half)(half1 - half2);
		}

		// Token: 0x0600B69D RID: 46749 RVA: 0x0043D960 File Offset: 0x0043BB60
		public static Half operator *(Half half1, Half half2)
		{
			return (Half)(half1 * half2);
		}

		// Token: 0x0600B69E RID: 46750 RVA: 0x0043D976 File Offset: 0x0043BB76
		public static Half operator /(Half half1, Half half2)
		{
			return (Half)(half1 / half2);
		}

		// Token: 0x0600B69F RID: 46751 RVA: 0x0043D98C File Offset: 0x0043BB8C
		public static bool operator ==(Half half1, Half half2)
		{
			return !Half.IsNaN(half1) && half1.value == half2.value;
		}

		// Token: 0x0600B6A0 RID: 46752 RVA: 0x0043D9A6 File Offset: 0x0043BBA6
		public static bool operator !=(Half half1, Half half2)
		{
			return half1.value != half2.value;
		}

		// Token: 0x0600B6A1 RID: 46753 RVA: 0x0043D9B9 File Offset: 0x0043BBB9
		public static bool operator <(Half half1, Half half2)
		{
			return half1 < half2;
		}

		// Token: 0x0600B6A2 RID: 46754 RVA: 0x0043D9CB File Offset: 0x0043BBCB
		public static bool operator >(Half half1, Half half2)
		{
			return half1 > half2;
		}

		// Token: 0x0600B6A3 RID: 46755 RVA: 0x0043D9DD File Offset: 0x0043BBDD
		public static bool operator <=(Half half1, Half half2)
		{
			return half1 == half2 || half1 < half2;
		}

		// Token: 0x0600B6A4 RID: 46756 RVA: 0x0043D9F1 File Offset: 0x0043BBF1
		public static bool operator >=(Half half1, Half half2)
		{
			return half1 == half2 || half1 > half2;
		}

		// Token: 0x0600B6A5 RID: 46757 RVA: 0x0043DA05 File Offset: 0x0043BC05
		public static implicit operator Half(byte value)
		{
			return new Half((float)value);
		}

		// Token: 0x0600B6A6 RID: 46758 RVA: 0x0043DA05 File Offset: 0x0043BC05
		public static implicit operator Half(short value)
		{
			return new Half((float)value);
		}

		// Token: 0x0600B6A7 RID: 46759 RVA: 0x0043DA05 File Offset: 0x0043BC05
		public static implicit operator Half(char value)
		{
			return new Half((float)value);
		}

		// Token: 0x0600B6A8 RID: 46760 RVA: 0x0043DA05 File Offset: 0x0043BC05
		public static implicit operator Half(int value)
		{
			return new Half((float)value);
		}

		// Token: 0x0600B6A9 RID: 46761 RVA: 0x0043DA05 File Offset: 0x0043BC05
		public static implicit operator Half(long value)
		{
			return new Half((float)value);
		}

		// Token: 0x0600B6AA RID: 46762 RVA: 0x0043DA05 File Offset: 0x0043BC05
		public static explicit operator Half(float value)
		{
			return new Half(value);
		}

		// Token: 0x0600B6AB RID: 46763 RVA: 0x0043DA05 File Offset: 0x0043BC05
		public static explicit operator Half(double value)
		{
			return new Half((float)value);
		}

		// Token: 0x0600B6AC RID: 46764 RVA: 0x0043DA0E File Offset: 0x0043BC0E
		public static explicit operator Half(decimal value)
		{
			return new Half((float)value);
		}

		// Token: 0x0600B6AD RID: 46765 RVA: 0x0043DA1C File Offset: 0x0043BC1C
		public static explicit operator byte(Half value)
		{
			return (byte)value;
		}

		// Token: 0x0600B6AE RID: 46766 RVA: 0x0043DA26 File Offset: 0x0043BC26
		public static explicit operator char(Half value)
		{
			return (char)value;
		}

		// Token: 0x0600B6AF RID: 46767 RVA: 0x0043DA30 File Offset: 0x0043BC30
		public static explicit operator short(Half value)
		{
			return (short)value;
		}

		// Token: 0x0600B6B0 RID: 46768 RVA: 0x0043DA3A File Offset: 0x0043BC3A
		public static explicit operator int(Half value)
		{
			return (int)value;
		}

		// Token: 0x0600B6B1 RID: 46769 RVA: 0x0043DA44 File Offset: 0x0043BC44
		public static explicit operator long(Half value)
		{
			return (long)value;
		}

		// Token: 0x0600B6B2 RID: 46770 RVA: 0x0043DA4E File Offset: 0x0043BC4E
		public static implicit operator float(Half value)
		{
			return HalfHelper.HalfToSingle(value);
		}

		// Token: 0x0600B6B3 RID: 46771 RVA: 0x0043DA57 File Offset: 0x0043BC57
		public static implicit operator double(Half value)
		{
			return (double)value;
		}

		// Token: 0x0600B6B4 RID: 46772 RVA: 0x0043DA61 File Offset: 0x0043BC61
		public static explicit operator decimal(Half value)
		{
			return (decimal)value;
		}

		// Token: 0x0600B6B5 RID: 46773 RVA: 0x0043DA05 File Offset: 0x0043BC05
		public static implicit operator Half(sbyte value)
		{
			return new Half((float)value);
		}

		// Token: 0x0600B6B6 RID: 46774 RVA: 0x0043DA05 File Offset: 0x0043BC05
		public static implicit operator Half(ushort value)
		{
			return new Half((float)value);
		}

		// Token: 0x0600B6B7 RID: 46775 RVA: 0x0043DA6F File Offset: 0x0043BC6F
		public static implicit operator Half(uint value)
		{
			return new Half(value);
		}

		// Token: 0x0600B6B8 RID: 46776 RVA: 0x0043DA6F File Offset: 0x0043BC6F
		public static implicit operator Half(ulong value)
		{
			return new Half(value);
		}

		// Token: 0x0600B6B9 RID: 46777 RVA: 0x0043DA79 File Offset: 0x0043BC79
		public static explicit operator sbyte(Half value)
		{
			return (sbyte)value;
		}

		// Token: 0x0600B6BA RID: 46778 RVA: 0x0043DA26 File Offset: 0x0043BC26
		public static explicit operator ushort(Half value)
		{
			return (ushort)value;
		}

		// Token: 0x0600B6BB RID: 46779 RVA: 0x0043DA83 File Offset: 0x0043BC83
		public static explicit operator uint(Half value)
		{
			return (uint)value;
		}

		// Token: 0x0600B6BC RID: 46780 RVA: 0x0043DA8D File Offset: 0x0043BC8D
		public static explicit operator ulong(Half value)
		{
			return (ulong)value;
		}

		// Token: 0x0600B6BD RID: 46781 RVA: 0x0043DA98 File Offset: 0x0043BC98
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

		// Token: 0x0600B6BE RID: 46782 RVA: 0x0043DAF8 File Offset: 0x0043BCF8
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

		// Token: 0x0600B6BF RID: 46783 RVA: 0x0043DB31 File Offset: 0x0043BD31
		public bool Equals(Half other)
		{
			return other == this || (Half.IsNaN(other) && Half.IsNaN(this));
		}

		// Token: 0x0600B6C0 RID: 46784 RVA: 0x0043DB58 File Offset: 0x0043BD58
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

		// Token: 0x0600B6C1 RID: 46785 RVA: 0x0043DB9C File Offset: 0x0043BD9C
		public override int GetHashCode()
		{
			return this.value.GetHashCode();
		}

		// Token: 0x0600B6C2 RID: 46786 RVA: 0x0004E4C6 File Offset: 0x0004C6C6
		public TypeCode GetTypeCode()
		{
			return (TypeCode)255;
		}

		// Token: 0x0600B6C3 RID: 46787 RVA: 0x0043DBA9 File Offset: 0x0043BDA9
		public static byte[] GetBytes(Half value)
		{
			return BitConverter.GetBytes(value.value);
		}

		// Token: 0x0600B6C4 RID: 46788 RVA: 0x0043DBB6 File Offset: 0x0043BDB6
		public static ushort GetBits(Half value)
		{
			return value.value;
		}

		// Token: 0x0600B6C5 RID: 46789 RVA: 0x0043DBBE File Offset: 0x0043BDBE
		public static Half ToHalf(byte[] value, int startIndex)
		{
			return Half.ToHalf((ushort)BitConverter.ToInt16(value, startIndex));
		}

		// Token: 0x0600B6C6 RID: 46790 RVA: 0x0043DBD0 File Offset: 0x0043BDD0
		public static Half ToHalf(ushort bits)
		{
			return new Half
			{
				value = bits
			};
		}

		// Token: 0x0600B6C7 RID: 46791 RVA: 0x0043DBEE File Offset: 0x0043BDEE
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

		// Token: 0x0600B6C8 RID: 46792 RVA: 0x0043DC2A File Offset: 0x0043BE2A
		public static Half Abs(Half value)
		{
			return HalfHelper.Abs(value);
		}

		// Token: 0x0600B6C9 RID: 46793 RVA: 0x0043DC32 File Offset: 0x0043BE32
		public static Half Max(Half value1, Half value2)
		{
			if (!(value1 < value2))
			{
				return value1;
			}
			return value2;
		}

		// Token: 0x0600B6CA RID: 46794 RVA: 0x0043DC40 File Offset: 0x0043BE40
		public static Half Min(Half value1, Half value2)
		{
			if (!(value1 < value2))
			{
				return value2;
			}
			return value1;
		}

		// Token: 0x0600B6CB RID: 46795 RVA: 0x0043DC4E File Offset: 0x0043BE4E
		public static bool IsNaN(Half half)
		{
			return HalfHelper.IsNaN(half);
		}

		// Token: 0x0600B6CC RID: 46796 RVA: 0x0043DC56 File Offset: 0x0043BE56
		public static bool IsInfinity(Half half)
		{
			return HalfHelper.IsInfinity(half);
		}

		// Token: 0x0600B6CD RID: 46797 RVA: 0x0043DC5E File Offset: 0x0043BE5E
		public static bool IsNegativeInfinity(Half half)
		{
			return HalfHelper.IsNegativeInfinity(half);
		}

		// Token: 0x0600B6CE RID: 46798 RVA: 0x0043DC66 File Offset: 0x0043BE66
		public static bool IsPositiveInfinity(Half half)
		{
			return HalfHelper.IsPositiveInfinity(half);
		}

		// Token: 0x0600B6CF RID: 46799 RVA: 0x0043DC6E File Offset: 0x0043BE6E
		public static Half Parse(string value)
		{
			return (Half)float.Parse(value, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600B6D0 RID: 46800 RVA: 0x0043DC80 File Offset: 0x0043BE80
		public static Half Parse(string value, IFormatProvider provider)
		{
			return (Half)float.Parse(value, provider);
		}

		// Token: 0x0600B6D1 RID: 46801 RVA: 0x0043DC8E File Offset: 0x0043BE8E
		public static Half Parse(string value, NumberStyles style)
		{
			return (Half)float.Parse(value, style, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600B6D2 RID: 46802 RVA: 0x0043DCA1 File Offset: 0x0043BEA1
		public static Half Parse(string value, NumberStyles style, IFormatProvider provider)
		{
			return (Half)float.Parse(value, style, provider);
		}

		// Token: 0x0600B6D3 RID: 46803 RVA: 0x0043DCB0 File Offset: 0x0043BEB0
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

		// Token: 0x0600B6D4 RID: 46804 RVA: 0x0043DCE0 File Offset: 0x0043BEE0
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

		// Token: 0x0600B6D5 RID: 46805 RVA: 0x0043DD14 File Offset: 0x0043BF14
		public override string ToString()
		{
			return this.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x0600B6D6 RID: 46806 RVA: 0x0043DD3C File Offset: 0x0043BF3C
		public string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(formatProvider);
		}

		// Token: 0x0600B6D7 RID: 46807 RVA: 0x0043DD60 File Offset: 0x0043BF60
		public string ToString(string format)
		{
			return this.ToString(format, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600B6D8 RID: 46808 RVA: 0x0043DD88 File Offset: 0x0043BF88
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return this.ToString(format, formatProvider);
		}

		// Token: 0x0600B6D9 RID: 46809 RVA: 0x0043DDAB File Offset: 0x0043BFAB
		[PublicizedFrom(EAccessModifier.Private)]
		public float System.IConvertible.ToSingle(IFormatProvider provider)
		{
			return this;
		}

		// Token: 0x0600B6DA RID: 46810 RVA: 0x0043DDB9 File Offset: 0x0043BFB9
		[PublicizedFrom(EAccessModifier.Private)]
		public TypeCode System.IConvertible.GetTypeCode()
		{
			return this.GetTypeCode();
		}

		// Token: 0x0600B6DB RID: 46811 RVA: 0x0043DDC1 File Offset: 0x0043BFC1
		[PublicizedFrom(EAccessModifier.Private)]
		public bool System.IConvertible.ToBoolean(IFormatProvider provider)
		{
			return Convert.ToBoolean(this);
		}

		// Token: 0x0600B6DC RID: 46812 RVA: 0x0043DDD4 File Offset: 0x0043BFD4
		[PublicizedFrom(EAccessModifier.Private)]
		public byte System.IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this);
		}

		// Token: 0x0600B6DD RID: 46813 RVA: 0x0043DDE7 File Offset: 0x0043BFE7
		[PublicizedFrom(EAccessModifier.Private)]
		public char System.IConvertible.ToChar(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, "Invalid cast from '{0}' to '{1}'.", "Half", "Char"));
		}

		// Token: 0x0600B6DE RID: 46814 RVA: 0x0043DE07 File Offset: 0x0043C007
		[PublicizedFrom(EAccessModifier.Private)]
		public DateTime System.IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, "Invalid cast from '{0}' to '{1}'.", "Half", "DateTime"));
		}

		// Token: 0x0600B6DF RID: 46815 RVA: 0x0043DE27 File Offset: 0x0043C027
		[PublicizedFrom(EAccessModifier.Private)]
		public decimal System.IConvertible.ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(this);
		}

		// Token: 0x0600B6E0 RID: 46816 RVA: 0x0043DE3A File Offset: 0x0043C03A
		[PublicizedFrom(EAccessModifier.Private)]
		public double System.IConvertible.ToDouble(IFormatProvider provider)
		{
			return Convert.ToDouble(this);
		}

		// Token: 0x0600B6E1 RID: 46817 RVA: 0x0043DE4D File Offset: 0x0043C04D
		[PublicizedFrom(EAccessModifier.Private)]
		public short System.IConvertible.ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this);
		}

		// Token: 0x0600B6E2 RID: 46818 RVA: 0x0043DE60 File Offset: 0x0043C060
		[PublicizedFrom(EAccessModifier.Private)]
		public int System.IConvertible.ToInt32(IFormatProvider provider)
		{
			return Convert.ToInt32(this);
		}

		// Token: 0x0600B6E3 RID: 46819 RVA: 0x0043DE73 File Offset: 0x0043C073
		[PublicizedFrom(EAccessModifier.Private)]
		public long System.IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this);
		}

		// Token: 0x0600B6E4 RID: 46820 RVA: 0x0043DE86 File Offset: 0x0043C086
		[PublicizedFrom(EAccessModifier.Private)]
		public sbyte System.IConvertible.ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(this);
		}

		// Token: 0x0600B6E5 RID: 46821 RVA: 0x0043DE99 File Offset: 0x0043C099
		[PublicizedFrom(EAccessModifier.Private)]
		public string System.IConvertible.ToString(IFormatProvider provider)
		{
			return Convert.ToString(this, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600B6E6 RID: 46822 RVA: 0x0043DEB1 File Offset: 0x0043C0B1
		[PublicizedFrom(EAccessModifier.Private)]
		public object System.IConvertible.ToType(Type conversionType, IFormatProvider provider)
		{
			return ((IConvertible)this).ToType(conversionType, provider);
		}

		// Token: 0x0600B6E7 RID: 46823 RVA: 0x0043DECB File Offset: 0x0043C0CB
		[PublicizedFrom(EAccessModifier.Private)]
		public ushort System.IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this);
		}

		// Token: 0x0600B6E8 RID: 46824 RVA: 0x0043DEDE File Offset: 0x0043C0DE
		[PublicizedFrom(EAccessModifier.Private)]
		public uint System.IConvertible.ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this);
		}

		// Token: 0x0600B6E9 RID: 46825 RVA: 0x0043DEF1 File Offset: 0x0043C0F1
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong System.IConvertible.ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this);
		}

		// Token: 0x0400887D RID: 34941
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[PublicizedFrom(EAccessModifier.Internal)]
		[NonSerialized]
		public ushort value;

		// Token: 0x0400887E RID: 34942
		public static readonly Half Epsilon = Half.ToHalf(1);

		// Token: 0x0400887F RID: 34943
		public static readonly Half MaxValue = Half.ToHalf(31743);

		// Token: 0x04008880 RID: 34944
		public static readonly Half MinValue = Half.ToHalf(64511);

		// Token: 0x04008881 RID: 34945
		public static readonly Half NaN = Half.ToHalf(65024);

		// Token: 0x04008882 RID: 34946
		public static readonly Half NegativeInfinity = Half.ToHalf(64512);

		// Token: 0x04008883 RID: 34947
		public static readonly Half PositiveInfinity = Half.ToHalf(31744);
	}
}
