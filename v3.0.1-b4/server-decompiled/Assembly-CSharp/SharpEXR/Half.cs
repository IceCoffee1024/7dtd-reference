using System;
using System.Diagnostics;
using System.Globalization;

namespace SharpEXR
{
	// Token: 0x020016AA RID: 5802
	[Serializable]
	public struct Half : IComparable, IFormattable, IConvertible, IComparable<Half>, IEquatable<Half>
	{
		// Token: 0x0600B508 RID: 46344 RVA: 0x00437B30 File Offset: 0x00435D30
		public Half(float value)
		{
			this = HalfHelper.SingleToHalf(value);
		}

		// Token: 0x0600B509 RID: 46345 RVA: 0x00437B3E File Offset: 0x00435D3E
		public Half(int value)
		{
			this = new Half((float)value);
		}

		// Token: 0x0600B50A RID: 46346 RVA: 0x00437B3E File Offset: 0x00435D3E
		public Half(long value)
		{
			this = new Half((float)value);
		}

		// Token: 0x0600B50B RID: 46347 RVA: 0x00437B3E File Offset: 0x00435D3E
		public Half(double value)
		{
			this = new Half((float)value);
		}

		// Token: 0x0600B50C RID: 46348 RVA: 0x00437B48 File Offset: 0x00435D48
		public Half(decimal value)
		{
			this = new Half((float)value);
		}

		// Token: 0x0600B50D RID: 46349 RVA: 0x00437B57 File Offset: 0x00435D57
		public Half(uint value)
		{
			this = new Half(value);
		}

		// Token: 0x0600B50E RID: 46350 RVA: 0x00437B57 File Offset: 0x00435D57
		public Half(ulong value)
		{
			this = new Half(value);
		}

		// Token: 0x0600B50F RID: 46351 RVA: 0x00437B62 File Offset: 0x00435D62
		public static Half Negate(Half half)
		{
			return -half;
		}

		// Token: 0x0600B510 RID: 46352 RVA: 0x00437B6A File Offset: 0x00435D6A
		public static Half Add(Half half1, Half half2)
		{
			return half1 + half2;
		}

		// Token: 0x0600B511 RID: 46353 RVA: 0x00437B73 File Offset: 0x00435D73
		public static Half Subtract(Half half1, Half half2)
		{
			return half1 - half2;
		}

		// Token: 0x0600B512 RID: 46354 RVA: 0x00437B7C File Offset: 0x00435D7C
		public static Half Multiply(Half half1, Half half2)
		{
			return half1 * half2;
		}

		// Token: 0x0600B513 RID: 46355 RVA: 0x00437B85 File Offset: 0x00435D85
		public static Half Divide(Half half1, Half half2)
		{
			return half1 / half2;
		}

		// Token: 0x0600B514 RID: 46356 RVA: 0x001290F5 File Offset: 0x001272F5
		public static Half operator +(Half half)
		{
			return half;
		}

		// Token: 0x0600B515 RID: 46357 RVA: 0x00437B8E File Offset: 0x00435D8E
		public static Half operator -(Half half)
		{
			return HalfHelper.Negate(half);
		}

		// Token: 0x0600B516 RID: 46358 RVA: 0x00437B96 File Offset: 0x00435D96
		public static Half operator ++(Half half)
		{
			return (Half)(half + 1f);
		}

		// Token: 0x0600B517 RID: 46359 RVA: 0x00437BA9 File Offset: 0x00435DA9
		public static Half operator --(Half half)
		{
			return (Half)(half - 1f);
		}

		// Token: 0x0600B518 RID: 46360 RVA: 0x00437BBC File Offset: 0x00435DBC
		public static Half operator +(Half half1, Half half2)
		{
			return (Half)(half1 + half2);
		}

		// Token: 0x0600B519 RID: 46361 RVA: 0x00437BD2 File Offset: 0x00435DD2
		public static Half operator -(Half half1, Half half2)
		{
			return (Half)(half1 - half2);
		}

		// Token: 0x0600B51A RID: 46362 RVA: 0x00437BE8 File Offset: 0x00435DE8
		public static Half operator *(Half half1, Half half2)
		{
			return (Half)(half1 * half2);
		}

		// Token: 0x0600B51B RID: 46363 RVA: 0x00437BFE File Offset: 0x00435DFE
		public static Half operator /(Half half1, Half half2)
		{
			return (Half)(half1 / half2);
		}

		// Token: 0x0600B51C RID: 46364 RVA: 0x00437C14 File Offset: 0x00435E14
		public static bool operator ==(Half half1, Half half2)
		{
			return !Half.IsNaN(half1) && half1.value == half2.value;
		}

		// Token: 0x0600B51D RID: 46365 RVA: 0x00437C2E File Offset: 0x00435E2E
		public static bool operator !=(Half half1, Half half2)
		{
			return half1.value != half2.value;
		}

		// Token: 0x0600B51E RID: 46366 RVA: 0x00437C41 File Offset: 0x00435E41
		public static bool operator <(Half half1, Half half2)
		{
			return half1 < half2;
		}

		// Token: 0x0600B51F RID: 46367 RVA: 0x00437C53 File Offset: 0x00435E53
		public static bool operator >(Half half1, Half half2)
		{
			return half1 > half2;
		}

		// Token: 0x0600B520 RID: 46368 RVA: 0x00437C65 File Offset: 0x00435E65
		public static bool operator <=(Half half1, Half half2)
		{
			return half1 == half2 || half1 < half2;
		}

		// Token: 0x0600B521 RID: 46369 RVA: 0x00437C79 File Offset: 0x00435E79
		public static bool operator >=(Half half1, Half half2)
		{
			return half1 == half2 || half1 > half2;
		}

		// Token: 0x0600B522 RID: 46370 RVA: 0x00437C8D File Offset: 0x00435E8D
		public static implicit operator Half(byte value)
		{
			return new Half((float)value);
		}

		// Token: 0x0600B523 RID: 46371 RVA: 0x00437C8D File Offset: 0x00435E8D
		public static implicit operator Half(short value)
		{
			return new Half((float)value);
		}

		// Token: 0x0600B524 RID: 46372 RVA: 0x00437C8D File Offset: 0x00435E8D
		public static implicit operator Half(char value)
		{
			return new Half((float)value);
		}

		// Token: 0x0600B525 RID: 46373 RVA: 0x00437C8D File Offset: 0x00435E8D
		public static implicit operator Half(int value)
		{
			return new Half((float)value);
		}

		// Token: 0x0600B526 RID: 46374 RVA: 0x00437C8D File Offset: 0x00435E8D
		public static implicit operator Half(long value)
		{
			return new Half((float)value);
		}

		// Token: 0x0600B527 RID: 46375 RVA: 0x00437C8D File Offset: 0x00435E8D
		public static explicit operator Half(float value)
		{
			return new Half(value);
		}

		// Token: 0x0600B528 RID: 46376 RVA: 0x00437C8D File Offset: 0x00435E8D
		public static explicit operator Half(double value)
		{
			return new Half((float)value);
		}

		// Token: 0x0600B529 RID: 46377 RVA: 0x00437C96 File Offset: 0x00435E96
		public static explicit operator Half(decimal value)
		{
			return new Half((float)value);
		}

		// Token: 0x0600B52A RID: 46378 RVA: 0x00437CA4 File Offset: 0x00435EA4
		public static explicit operator byte(Half value)
		{
			return (byte)value;
		}

		// Token: 0x0600B52B RID: 46379 RVA: 0x00437CAE File Offset: 0x00435EAE
		public static explicit operator char(Half value)
		{
			return (char)value;
		}

		// Token: 0x0600B52C RID: 46380 RVA: 0x00437CB8 File Offset: 0x00435EB8
		public static explicit operator short(Half value)
		{
			return (short)value;
		}

		// Token: 0x0600B52D RID: 46381 RVA: 0x00437CC2 File Offset: 0x00435EC2
		public static explicit operator int(Half value)
		{
			return (int)value;
		}

		// Token: 0x0600B52E RID: 46382 RVA: 0x00437CCC File Offset: 0x00435ECC
		public static explicit operator long(Half value)
		{
			return (long)value;
		}

		// Token: 0x0600B52F RID: 46383 RVA: 0x00437CD6 File Offset: 0x00435ED6
		public static implicit operator float(Half value)
		{
			return HalfHelper.HalfToSingle(value);
		}

		// Token: 0x0600B530 RID: 46384 RVA: 0x00437CDF File Offset: 0x00435EDF
		public static implicit operator double(Half value)
		{
			return (double)value;
		}

		// Token: 0x0600B531 RID: 46385 RVA: 0x00437CE9 File Offset: 0x00435EE9
		public static explicit operator decimal(Half value)
		{
			return (decimal)value;
		}

		// Token: 0x0600B532 RID: 46386 RVA: 0x00437C8D File Offset: 0x00435E8D
		public static implicit operator Half(sbyte value)
		{
			return new Half((float)value);
		}

		// Token: 0x0600B533 RID: 46387 RVA: 0x00437C8D File Offset: 0x00435E8D
		public static implicit operator Half(ushort value)
		{
			return new Half((float)value);
		}

		// Token: 0x0600B534 RID: 46388 RVA: 0x00437CF7 File Offset: 0x00435EF7
		public static implicit operator Half(uint value)
		{
			return new Half(value);
		}

		// Token: 0x0600B535 RID: 46389 RVA: 0x00437CF7 File Offset: 0x00435EF7
		public static implicit operator Half(ulong value)
		{
			return new Half(value);
		}

		// Token: 0x0600B536 RID: 46390 RVA: 0x00437D01 File Offset: 0x00435F01
		public static explicit operator sbyte(Half value)
		{
			return (sbyte)value;
		}

		// Token: 0x0600B537 RID: 46391 RVA: 0x00437CAE File Offset: 0x00435EAE
		public static explicit operator ushort(Half value)
		{
			return (ushort)value;
		}

		// Token: 0x0600B538 RID: 46392 RVA: 0x00437D0B File Offset: 0x00435F0B
		public static explicit operator uint(Half value)
		{
			return (uint)value;
		}

		// Token: 0x0600B539 RID: 46393 RVA: 0x00437D15 File Offset: 0x00435F15
		public static explicit operator ulong(Half value)
		{
			return (ulong)value;
		}

		// Token: 0x0600B53A RID: 46394 RVA: 0x00437D20 File Offset: 0x00435F20
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

		// Token: 0x0600B53B RID: 46395 RVA: 0x00437D80 File Offset: 0x00435F80
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

		// Token: 0x0600B53C RID: 46396 RVA: 0x00437DB9 File Offset: 0x00435FB9
		public bool Equals(Half other)
		{
			return other == this || (Half.IsNaN(other) && Half.IsNaN(this));
		}

		// Token: 0x0600B53D RID: 46397 RVA: 0x00437DE0 File Offset: 0x00435FE0
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

		// Token: 0x0600B53E RID: 46398 RVA: 0x00437E24 File Offset: 0x00436024
		public override int GetHashCode()
		{
			return this.value.GetHashCode();
		}

		// Token: 0x0600B53F RID: 46399 RVA: 0x0004E0AE File Offset: 0x0004C2AE
		public TypeCode GetTypeCode()
		{
			return (TypeCode)255;
		}

		// Token: 0x0600B540 RID: 46400 RVA: 0x00437E31 File Offset: 0x00436031
		public static byte[] GetBytes(Half value)
		{
			return BitConverter.GetBytes(value.value);
		}

		// Token: 0x0600B541 RID: 46401 RVA: 0x00437E3E File Offset: 0x0043603E
		public static ushort GetBits(Half value)
		{
			return value.value;
		}

		// Token: 0x0600B542 RID: 46402 RVA: 0x00437E46 File Offset: 0x00436046
		public static Half ToHalf(byte[] value, int startIndex)
		{
			return Half.ToHalf((ushort)BitConverter.ToInt16(value, startIndex));
		}

		// Token: 0x0600B543 RID: 46403 RVA: 0x00437E58 File Offset: 0x00436058
		public static Half ToHalf(ushort bits)
		{
			return new Half
			{
				value = bits
			};
		}

		// Token: 0x0600B544 RID: 46404 RVA: 0x00437E76 File Offset: 0x00436076
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

		// Token: 0x0600B545 RID: 46405 RVA: 0x00437EB2 File Offset: 0x004360B2
		public static Half Abs(Half value)
		{
			return HalfHelper.Abs(value);
		}

		// Token: 0x0600B546 RID: 46406 RVA: 0x00437EBA File Offset: 0x004360BA
		public static Half Max(Half value1, Half value2)
		{
			if (!(value1 < value2))
			{
				return value1;
			}
			return value2;
		}

		// Token: 0x0600B547 RID: 46407 RVA: 0x00437EC8 File Offset: 0x004360C8
		public static Half Min(Half value1, Half value2)
		{
			if (!(value1 < value2))
			{
				return value2;
			}
			return value1;
		}

		// Token: 0x0600B548 RID: 46408 RVA: 0x00437ED6 File Offset: 0x004360D6
		public static bool IsNaN(Half half)
		{
			return HalfHelper.IsNaN(half);
		}

		// Token: 0x0600B549 RID: 46409 RVA: 0x00437EDE File Offset: 0x004360DE
		public static bool IsInfinity(Half half)
		{
			return HalfHelper.IsInfinity(half);
		}

		// Token: 0x0600B54A RID: 46410 RVA: 0x00437EE6 File Offset: 0x004360E6
		public static bool IsNegativeInfinity(Half half)
		{
			return HalfHelper.IsNegativeInfinity(half);
		}

		// Token: 0x0600B54B RID: 46411 RVA: 0x00437EEE File Offset: 0x004360EE
		public static bool IsPositiveInfinity(Half half)
		{
			return HalfHelper.IsPositiveInfinity(half);
		}

		// Token: 0x0600B54C RID: 46412 RVA: 0x00437EF6 File Offset: 0x004360F6
		public static Half Parse(string value)
		{
			return (Half)float.Parse(value, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600B54D RID: 46413 RVA: 0x00437F08 File Offset: 0x00436108
		public static Half Parse(string value, IFormatProvider provider)
		{
			return (Half)float.Parse(value, provider);
		}

		// Token: 0x0600B54E RID: 46414 RVA: 0x00437F16 File Offset: 0x00436116
		public static Half Parse(string value, NumberStyles style)
		{
			return (Half)float.Parse(value, style, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600B54F RID: 46415 RVA: 0x00437F29 File Offset: 0x00436129
		public static Half Parse(string value, NumberStyles style, IFormatProvider provider)
		{
			return (Half)float.Parse(value, style, provider);
		}

		// Token: 0x0600B550 RID: 46416 RVA: 0x00437F38 File Offset: 0x00436138
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

		// Token: 0x0600B551 RID: 46417 RVA: 0x00437F68 File Offset: 0x00436168
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

		// Token: 0x0600B552 RID: 46418 RVA: 0x00437F9C File Offset: 0x0043619C
		public override string ToString()
		{
			return this.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x0600B553 RID: 46419 RVA: 0x00437FC4 File Offset: 0x004361C4
		public string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(formatProvider);
		}

		// Token: 0x0600B554 RID: 46420 RVA: 0x00437FE8 File Offset: 0x004361E8
		public string ToString(string format)
		{
			return this.ToString(format, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600B555 RID: 46421 RVA: 0x00438010 File Offset: 0x00436210
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return this.ToString(format, formatProvider);
		}

		// Token: 0x0600B556 RID: 46422 RVA: 0x00438033 File Offset: 0x00436233
		[PublicizedFrom(EAccessModifier.Private)]
		public float System.IConvertible.ToSingle(IFormatProvider provider)
		{
			return this;
		}

		// Token: 0x0600B557 RID: 46423 RVA: 0x00438041 File Offset: 0x00436241
		[PublicizedFrom(EAccessModifier.Private)]
		public TypeCode System.IConvertible.GetTypeCode()
		{
			return this.GetTypeCode();
		}

		// Token: 0x0600B558 RID: 46424 RVA: 0x00438049 File Offset: 0x00436249
		[PublicizedFrom(EAccessModifier.Private)]
		public bool System.IConvertible.ToBoolean(IFormatProvider provider)
		{
			return Convert.ToBoolean(this);
		}

		// Token: 0x0600B559 RID: 46425 RVA: 0x0043805C File Offset: 0x0043625C
		[PublicizedFrom(EAccessModifier.Private)]
		public byte System.IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this);
		}

		// Token: 0x0600B55A RID: 46426 RVA: 0x0043806F File Offset: 0x0043626F
		[PublicizedFrom(EAccessModifier.Private)]
		public char System.IConvertible.ToChar(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, "Invalid cast from '{0}' to '{1}'.", "Half", "Char"));
		}

		// Token: 0x0600B55B RID: 46427 RVA: 0x0043808F File Offset: 0x0043628F
		[PublicizedFrom(EAccessModifier.Private)]
		public DateTime System.IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, "Invalid cast from '{0}' to '{1}'.", "Half", "DateTime"));
		}

		// Token: 0x0600B55C RID: 46428 RVA: 0x004380AF File Offset: 0x004362AF
		[PublicizedFrom(EAccessModifier.Private)]
		public decimal System.IConvertible.ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(this);
		}

		// Token: 0x0600B55D RID: 46429 RVA: 0x004380C2 File Offset: 0x004362C2
		[PublicizedFrom(EAccessModifier.Private)]
		public double System.IConvertible.ToDouble(IFormatProvider provider)
		{
			return Convert.ToDouble(this);
		}

		// Token: 0x0600B55E RID: 46430 RVA: 0x004380D5 File Offset: 0x004362D5
		[PublicizedFrom(EAccessModifier.Private)]
		public short System.IConvertible.ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this);
		}

		// Token: 0x0600B55F RID: 46431 RVA: 0x004380E8 File Offset: 0x004362E8
		[PublicizedFrom(EAccessModifier.Private)]
		public int System.IConvertible.ToInt32(IFormatProvider provider)
		{
			return Convert.ToInt32(this);
		}

		// Token: 0x0600B560 RID: 46432 RVA: 0x004380FB File Offset: 0x004362FB
		[PublicizedFrom(EAccessModifier.Private)]
		public long System.IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this);
		}

		// Token: 0x0600B561 RID: 46433 RVA: 0x0043810E File Offset: 0x0043630E
		[PublicizedFrom(EAccessModifier.Private)]
		public sbyte System.IConvertible.ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(this);
		}

		// Token: 0x0600B562 RID: 46434 RVA: 0x00438121 File Offset: 0x00436321
		[PublicizedFrom(EAccessModifier.Private)]
		public string System.IConvertible.ToString(IFormatProvider provider)
		{
			return Convert.ToString(this, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600B563 RID: 46435 RVA: 0x00438139 File Offset: 0x00436339
		[PublicizedFrom(EAccessModifier.Private)]
		public object System.IConvertible.ToType(Type conversionType, IFormatProvider provider)
		{
			return ((IConvertible)this).ToType(conversionType, provider);
		}

		// Token: 0x0600B564 RID: 46436 RVA: 0x00438153 File Offset: 0x00436353
		[PublicizedFrom(EAccessModifier.Private)]
		public ushort System.IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this);
		}

		// Token: 0x0600B565 RID: 46437 RVA: 0x00438166 File Offset: 0x00436366
		[PublicizedFrom(EAccessModifier.Private)]
		public uint System.IConvertible.ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this);
		}

		// Token: 0x0600B566 RID: 46438 RVA: 0x00438179 File Offset: 0x00436379
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong System.IConvertible.ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this);
		}

		// Token: 0x04008761 RID: 34657
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[PublicizedFrom(EAccessModifier.Internal)]
		[NonSerialized]
		public ushort value;

		// Token: 0x04008762 RID: 34658
		public static readonly Half Epsilon = Half.ToHalf(1);

		// Token: 0x04008763 RID: 34659
		public static readonly Half MaxValue = Half.ToHalf(31743);

		// Token: 0x04008764 RID: 34660
		public static readonly Half MinValue = Half.ToHalf(64511);

		// Token: 0x04008765 RID: 34661
		public static readonly Half NaN = Half.ToHalf(65024);

		// Token: 0x04008766 RID: 34662
		public static readonly Half NegativeInfinity = Half.ToHalf(64512);

		// Token: 0x04008767 RID: 34663
		public static readonly Half PositiveInfinity = Half.ToHalf(31744);
	}
}
