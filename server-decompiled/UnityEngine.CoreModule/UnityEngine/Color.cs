using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x020001E8 RID: 488
	[NativeClass("ColorRGBAf")]
	[NativeHeader("Runtime/Math/Color.h")]
	[RequiredByNativeCode(Optional = true, GenerateProxy = true)]
	public struct Color : IEquatable<Color>, IFormattable
	{
		// Token: 0x060014EA RID: 5354 RVA: 0x0001E8CF File Offset: 0x0001CACF
		public Color(float r, float g, float b, float a)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = a;
		}

		// Token: 0x060014EB RID: 5355 RVA: 0x0001E8EF File Offset: 0x0001CAEF
		public Color(float r, float g, float b)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = 1f;
		}

		// Token: 0x060014EC RID: 5356 RVA: 0x0001E914 File Offset: 0x0001CB14
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x060014ED RID: 5357 RVA: 0x0001E930 File Offset: 0x0001CB30
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x060014EE RID: 5358 RVA: 0x0001E94C File Offset: 0x0001CB4C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			bool flag = string.IsNullOrEmpty(format);
			if (flag)
			{
				format = "F3";
			}
			bool flag2 = formatProvider == null;
			if (flag2)
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

		// Token: 0x060014EF RID: 5359 RVA: 0x0001E9D4 File Offset: 0x0001CBD4
		public override int GetHashCode()
		{
			return this.GetHashCode();
		}

		// Token: 0x060014F0 RID: 5360 RVA: 0x0001EA00 File Offset: 0x0001CC00
		public override bool Equals(object other)
		{
			bool flag = !(other is Color);
			return !flag && this.Equals((Color)other);
		}

		// Token: 0x060014F1 RID: 5361 RVA: 0x0001EA34 File Offset: 0x0001CC34
		public bool Equals(Color other)
		{
			return this.r.Equals(other.r) && this.g.Equals(other.g) && this.b.Equals(other.b) && this.a.Equals(other.a);
		}

		// Token: 0x060014F2 RID: 5362 RVA: 0x0001EA94 File Offset: 0x0001CC94
		public static Color operator +(Color a, Color b)
		{
			return new Color(a.r + b.r, a.g + b.g, a.b + b.b, a.a + b.a);
		}

		// Token: 0x060014F3 RID: 5363 RVA: 0x0001EAE0 File Offset: 0x0001CCE0
		public static Color operator -(Color a, Color b)
		{
			return new Color(a.r - b.r, a.g - b.g, a.b - b.b, a.a - b.a);
		}

		// Token: 0x060014F4 RID: 5364 RVA: 0x0001EB2C File Offset: 0x0001CD2C
		public static Color operator *(Color a, Color b)
		{
			return new Color(a.r * b.r, a.g * b.g, a.b * b.b, a.a * b.a);
		}

		// Token: 0x060014F5 RID: 5365 RVA: 0x0001EB78 File Offset: 0x0001CD78
		public static Color operator *(Color a, float b)
		{
			return new Color(a.r * b, a.g * b, a.b * b, a.a * b);
		}

		// Token: 0x060014F6 RID: 5366 RVA: 0x0001EBB0 File Offset: 0x0001CDB0
		public static Color operator *(float b, Color a)
		{
			return new Color(a.r * b, a.g * b, a.b * b, a.a * b);
		}

		// Token: 0x060014F7 RID: 5367 RVA: 0x0001EBE8 File Offset: 0x0001CDE8
		public static Color operator /(Color a, float b)
		{
			return new Color(a.r / b, a.g / b, a.b / b, a.a / b);
		}

		// Token: 0x060014F8 RID: 5368 RVA: 0x0001EC20 File Offset: 0x0001CE20
		public static bool operator ==(Color lhs, Color rhs)
		{
			return lhs == rhs;
		}

		// Token: 0x060014F9 RID: 5369 RVA: 0x0001EC44 File Offset: 0x0001CE44
		public static bool operator !=(Color lhs, Color rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x060014FA RID: 5370 RVA: 0x0001EC60 File Offset: 0x0001CE60
		public static Color Lerp(Color a, Color b, float t)
		{
			t = Mathf.Clamp01(t);
			return new Color(a.r + (b.r - a.r) * t, a.g + (b.g - a.g) * t, a.b + (b.b - a.b) * t, a.a + (b.a - a.a) * t);
		}

		// Token: 0x060014FB RID: 5371 RVA: 0x0001ECD8 File Offset: 0x0001CED8
		public static Color LerpUnclamped(Color a, Color b, float t)
		{
			return new Color(a.r + (b.r - a.r) * t, a.g + (b.g - a.g) * t, a.b + (b.b - a.b) * t, a.a + (b.a - a.a) * t);
		}

		// Token: 0x060014FC RID: 5372 RVA: 0x0001ED48 File Offset: 0x0001CF48
		internal Color RGBMultiplied(float multiplier)
		{
			return new Color(this.r * multiplier, this.g * multiplier, this.b * multiplier, this.a);
		}

		// Token: 0x060014FD RID: 5373 RVA: 0x0001ED80 File Offset: 0x0001CF80
		internal Color AlphaMultiplied(float multiplier)
		{
			return new Color(this.r, this.g, this.b, this.a * multiplier);
		}

		// Token: 0x060014FE RID: 5374 RVA: 0x0001EDB4 File Offset: 0x0001CFB4
		internal Color RGBMultiplied(Color multiplier)
		{
			return new Color(this.r * multiplier.r, this.g * multiplier.g, this.b * multiplier.b, this.a);
		}

		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x060014FF RID: 5375 RVA: 0x0001EDF8 File Offset: 0x0001CFF8
		public static Color red
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new Color(1f, 0f, 0f, 1f);
			}
		}

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x06001500 RID: 5376 RVA: 0x0001EE24 File Offset: 0x0001D024
		public static Color green
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new Color(0f, 1f, 0f, 1f);
			}
		}

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x06001501 RID: 5377 RVA: 0x0001EE50 File Offset: 0x0001D050
		public static Color blue
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new Color(0f, 0f, 1f, 1f);
			}
		}

		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x06001502 RID: 5378 RVA: 0x0001EE7C File Offset: 0x0001D07C
		public static Color white
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new Color(1f, 1f, 1f, 1f);
			}
		}

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x06001503 RID: 5379 RVA: 0x0001EEA8 File Offset: 0x0001D0A8
		public static Color black
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new Color(0f, 0f, 0f, 1f);
			}
		}

		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x06001504 RID: 5380 RVA: 0x0001EED4 File Offset: 0x0001D0D4
		public static Color yellow
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new Color(1f, 0.92156863f, 0.015686275f, 1f);
			}
		}

		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x06001505 RID: 5381 RVA: 0x0001EF00 File Offset: 0x0001D100
		public static Color cyan
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new Color(0f, 1f, 1f, 1f);
			}
		}

		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x06001506 RID: 5382 RVA: 0x0001EF2C File Offset: 0x0001D12C
		public static Color magenta
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new Color(1f, 0f, 1f, 1f);
			}
		}

		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x06001507 RID: 5383 RVA: 0x0001EF58 File Offset: 0x0001D158
		public static Color gray
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new Color(0.5f, 0.5f, 0.5f, 1f);
			}
		}

		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x06001508 RID: 5384 RVA: 0x0001EF84 File Offset: 0x0001D184
		public static Color grey
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new Color(0.5f, 0.5f, 0.5f, 1f);
			}
		}

		// Token: 0x17000446 RID: 1094
		// (get) Token: 0x06001509 RID: 5385 RVA: 0x0001EFB0 File Offset: 0x0001D1B0
		public static Color clear
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new Color(0f, 0f, 0f, 0f);
			}
		}

		// Token: 0x17000447 RID: 1095
		// (get) Token: 0x0600150A RID: 5386 RVA: 0x0001EFDC File Offset: 0x0001D1DC
		public float grayscale
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return 0.299f * this.r + 0.587f * this.g + 0.114f * this.b;
			}
		}

		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x0600150B RID: 5387 RVA: 0x0001F014 File Offset: 0x0001D214
		public Color linear
		{
			get
			{
				return new Color(Mathf.GammaToLinearSpace(this.r), Mathf.GammaToLinearSpace(this.g), Mathf.GammaToLinearSpace(this.b), this.a);
			}
		}

		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x0600150C RID: 5388 RVA: 0x0001F054 File Offset: 0x0001D254
		public Color gamma
		{
			get
			{
				return new Color(Mathf.LinearToGammaSpace(this.r), Mathf.LinearToGammaSpace(this.g), Mathf.LinearToGammaSpace(this.b), this.a);
			}
		}

		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x0600150D RID: 5389 RVA: 0x0001F094 File Offset: 0x0001D294
		public float maxColorComponent
		{
			get
			{
				return Mathf.Max(Mathf.Max(this.r, this.g), this.b);
			}
		}

		// Token: 0x0600150E RID: 5390 RVA: 0x0001F0C4 File Offset: 0x0001D2C4
		public static implicit operator Vector4(Color c)
		{
			return new Vector4(c.r, c.g, c.b, c.a);
		}

		// Token: 0x0600150F RID: 5391 RVA: 0x0001F0F4 File Offset: 0x0001D2F4
		public static implicit operator Color(Vector4 v)
		{
			return new Color(v.x, v.y, v.z, v.w);
		}

		// Token: 0x1700044B RID: 1099
		public float this[int index]
		{
			get
			{
				float num;
				switch (index)
				{
				case 0:
					num = this.r;
					break;
				case 1:
					num = this.g;
					break;
				case 2:
					num = this.b;
					break;
				case 3:
					num = this.a;
					break;
				default:
					throw new IndexOutOfRangeException("Invalid Color index(" + index.ToString() + ")!");
				}
				return num;
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
					throw new IndexOutOfRangeException("Invalid Color index(" + index.ToString() + ")!");
				}
			}
		}

		// Token: 0x06001512 RID: 5394 RVA: 0x0001F1FC File Offset: 0x0001D3FC
		public static void RGBToHSV(Color rgbColor, out float H, out float S, out float V)
		{
			bool flag = rgbColor.b > rgbColor.g && rgbColor.b > rgbColor.r;
			if (flag)
			{
				Color.RGBToHSVHelper(4f, rgbColor.b, rgbColor.r, rgbColor.g, out H, out S, out V);
			}
			else
			{
				bool flag2 = rgbColor.g > rgbColor.r;
				if (flag2)
				{
					Color.RGBToHSVHelper(2f, rgbColor.g, rgbColor.b, rgbColor.r, out H, out S, out V);
				}
				else
				{
					Color.RGBToHSVHelper(0f, rgbColor.r, rgbColor.g, rgbColor.b, out H, out S, out V);
				}
			}
		}

		// Token: 0x06001513 RID: 5395 RVA: 0x0001F2A4 File Offset: 0x0001D4A4
		private static void RGBToHSVHelper(float offset, float dominantcolor, float colorone, float colortwo, out float H, out float S, out float V)
		{
			V = dominantcolor;
			bool flag = V != 0f;
			if (flag)
			{
				bool flag2 = colorone > colortwo;
				float num;
				if (flag2)
				{
					num = colortwo;
				}
				else
				{
					num = colorone;
				}
				float num2 = V - num;
				bool flag3 = num2 != 0f;
				if (flag3)
				{
					S = num2 / V;
					H = offset + (colorone - colortwo) / num2;
				}
				else
				{
					S = 0f;
					H = offset + (colorone - colortwo);
				}
				H /= 6f;
				bool flag4 = H < 0f;
				if (flag4)
				{
					H += 1f;
				}
			}
			else
			{
				S = 0f;
				H = 0f;
			}
		}

		// Token: 0x06001514 RID: 5396 RVA: 0x0001F358 File Offset: 0x0001D558
		public static Color HSVToRGB(float H, float S, float V)
		{
			return Color.HSVToRGB(H, S, V, true);
		}

		// Token: 0x06001515 RID: 5397 RVA: 0x0001F374 File Offset: 0x0001D574
		public static Color HSVToRGB(float H, float S, float V, bool hdr)
		{
			Color white = Color.white;
			bool flag = S == 0f;
			if (flag)
			{
				white.r = V;
				white.g = V;
				white.b = V;
			}
			else
			{
				bool flag2 = V == 0f;
				if (flag2)
				{
					white.r = 0f;
					white.g = 0f;
					white.b = 0f;
				}
				else
				{
					white.r = 0f;
					white.g = 0f;
					white.b = 0f;
					float num = H * 6f;
					int num2 = (int)Mathf.Floor(num);
					float num3 = num - (float)num2;
					float num4 = V * (1f - S);
					float num5 = V * (1f - S * num3);
					float num6 = V * (1f - S * (1f - num3));
					switch (num2)
					{
					case -1:
						white.r = V;
						white.g = num4;
						white.b = num5;
						break;
					case 0:
						white.r = V;
						white.g = num6;
						white.b = num4;
						break;
					case 1:
						white.r = num5;
						white.g = V;
						white.b = num4;
						break;
					case 2:
						white.r = num4;
						white.g = V;
						white.b = num6;
						break;
					case 3:
						white.r = num4;
						white.g = num5;
						white.b = V;
						break;
					case 4:
						white.r = num6;
						white.g = num4;
						white.b = V;
						break;
					case 5:
						white.r = V;
						white.g = num4;
						white.b = num5;
						break;
					case 6:
						white.r = V;
						white.g = num6;
						white.b = num4;
						break;
					}
					bool flag3 = !hdr;
					if (flag3)
					{
						white.r = Mathf.Clamp(white.r, 0f, 1f);
						white.g = Mathf.Clamp(white.g, 0f, 1f);
						white.b = Mathf.Clamp(white.b, 0f, 1f);
					}
				}
			}
			return white;
		}

		// Token: 0x040007DB RID: 2011
		public float r;

		// Token: 0x040007DC RID: 2012
		public float g;

		// Token: 0x040007DD RID: 2013
		public float b;

		// Token: 0x040007DE RID: 2014
		public float a;
	}
}
