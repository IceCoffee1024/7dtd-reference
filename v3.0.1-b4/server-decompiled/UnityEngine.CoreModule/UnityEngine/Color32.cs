using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x020001E9 RID: 489
	[UsedByNativeCode]
	[StructLayout(LayoutKind.Explicit)]
	public struct Color32 : IFormattable
	{
		// Token: 0x06001516 RID: 5398 RVA: 0x0001F5EE File Offset: 0x0001D7EE
		public Color32(byte r, byte g, byte b, byte a)
		{
			this.rgba = 0;
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = a;
		}

		// Token: 0x06001517 RID: 5399 RVA: 0x0001F618 File Offset: 0x0001D818
		public static implicit operator Color32(Color c)
		{
			return new Color32((byte)Mathf.Round(Mathf.Clamp01(c.r) * 255f), (byte)Mathf.Round(Mathf.Clamp01(c.g) * 255f), (byte)Mathf.Round(Mathf.Clamp01(c.b) * 255f), (byte)Mathf.Round(Mathf.Clamp01(c.a) * 255f));
		}

		// Token: 0x06001518 RID: 5400 RVA: 0x0001F68C File Offset: 0x0001D88C
		public static implicit operator Color(Color32 c)
		{
			return new Color((float)c.r / 255f, (float)c.g / 255f, (float)c.b / 255f, (float)c.a / 255f);
		}

		// Token: 0x06001519 RID: 5401 RVA: 0x0001F6D8 File Offset: 0x0001D8D8
		public static Color32 Lerp(Color32 a, Color32 b, float t)
		{
			t = Mathf.Clamp01(t);
			return new Color32((byte)((float)a.r + (float)(b.r - a.r) * t), (byte)((float)a.g + (float)(b.g - a.g) * t), (byte)((float)a.b + (float)(b.b - a.b) * t), (byte)((float)a.a + (float)(b.a - a.a) * t));
		}

		// Token: 0x0600151A RID: 5402 RVA: 0x0001F75C File Offset: 0x0001D95C
		public static Color32 LerpUnclamped(Color32 a, Color32 b, float t)
		{
			return new Color32((byte)((float)a.r + (float)(b.r - a.r) * t), (byte)((float)a.g + (float)(b.g - a.g) * t), (byte)((float)a.b + (float)(b.b - a.b) * t), (byte)((float)a.a + (float)(b.a - a.a) * t));
		}

		// Token: 0x1700044C RID: 1100
		public byte this[int index]
		{
			get
			{
				byte b;
				switch (index)
				{
				case 0:
					b = this.r;
					break;
				case 1:
					b = this.g;
					break;
				case 2:
					b = this.b;
					break;
				case 3:
					b = this.a;
					break;
				default:
					throw new IndexOutOfRangeException("Invalid Color32 index(" + index.ToString() + ")!");
				}
				return b;
			}
			set
			{
				switch (index)
				{
				case 0:
					this.r = value;
					break;
				case 1:
					this.g = value;
					break;
				case 2:
					this.b = value;
					break;
				case 3:
					this.a = value;
					break;
				default:
					throw new IndexOutOfRangeException("Invalid Color32 index(" + index.ToString() + ")!");
				}
			}
		}

		// Token: 0x0600151D RID: 5405 RVA: 0x0001F8B0 File Offset: 0x0001DAB0
		[VisibleToOtherModules]
		internal bool InternalEquals(Color32 other)
		{
			return this.rgba == other.rgba;
		}

		// Token: 0x0600151E RID: 5406 RVA: 0x0001F8D0 File Offset: 0x0001DAD0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x0600151F RID: 5407 RVA: 0x0001F8EC File Offset: 0x0001DAEC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x06001520 RID: 5408 RVA: 0x0001F908 File Offset: 0x0001DB08
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			bool flag = formatProvider == null;
			if (flag)
			{
				formatProvider = CultureInfo.InvariantCulture.NumberFormat;
			}
			return UnityString.Format("RGBA({0}, {1}, {2}, {3})", new object[]
			{
				this.r.ToString(format, formatProvider),
				this.g.ToString(format, formatProvider),
				this.b.ToString(format, formatProvider),
				this.a.ToString(format, formatProvider)
			});
		}

		// Token: 0x040007DF RID: 2015
		[Ignore(DoesNotContributeToSize = true)]
		[FieldOffset(0)]
		private int rgba;

		// Token: 0x040007E0 RID: 2016
		[FieldOffset(0)]
		public byte r;

		// Token: 0x040007E1 RID: 2017
		[FieldOffset(1)]
		public byte g;

		// Token: 0x040007E2 RID: 2018
		[FieldOffset(2)]
		public byte b;

		// Token: 0x040007E3 RID: 2019
		[FieldOffset(3)]
		public byte a;
	}
}
