using System;
using UnityEngine;

// Token: 0x0200144D RID: 5197
[Serializable]
public struct HSBColor
{
	// Token: 0x0600A2C6 RID: 41670 RVA: 0x003D20AC File Offset: 0x003D02AC
	public HSBColor(float h, float s, float b, float a)
	{
		this.h = h;
		this.s = s;
		this.b = b;
		this.a = a;
	}

	// Token: 0x0600A2C7 RID: 41671 RVA: 0x003D20CB File Offset: 0x003D02CB
	public HSBColor(float h, float s, float b)
	{
		this.h = h;
		this.s = s;
		this.b = b;
		this.a = 1f;
	}

	// Token: 0x0600A2C8 RID: 41672 RVA: 0x003D20F0 File Offset: 0x003D02F0
	public HSBColor(Color col)
	{
		HSBColor hsbcolor = HSBColor.FromColor(col);
		this.h = hsbcolor.h;
		this.s = hsbcolor.s;
		this.b = hsbcolor.b;
		this.a = hsbcolor.a;
	}

	// Token: 0x0600A2C9 RID: 41673 RVA: 0x003D2134 File Offset: 0x003D0334
	public static HSBColor FromColor(Color color)
	{
		HSBColor hsbcolor = new HSBColor(0f, 0f, 0f, color.a);
		float r = color.r;
		float g = color.g;
		float num = color.b;
		float num2 = Mathf.Max(r, Mathf.Max(g, num));
		if (num2 <= 0f)
		{
			return hsbcolor;
		}
		float num3 = Mathf.Min(r, Mathf.Min(g, num));
		float num4 = num2 - num3;
		if (num2 > num3)
		{
			if (g == num2)
			{
				hsbcolor.h = (num - r) / num4 * 60f + 120f;
			}
			else if (num == num2)
			{
				hsbcolor.h = (r - g) / num4 * 60f + 240f;
			}
			else if (num > g)
			{
				hsbcolor.h = (g - num) / num4 * 60f + 360f;
			}
			else
			{
				hsbcolor.h = (g - num) / num4 * 60f;
			}
			if (hsbcolor.h < 0f)
			{
				hsbcolor.h += 360f;
			}
		}
		else
		{
			hsbcolor.h = 0f;
		}
		hsbcolor.h *= 0.0027777778f;
		hsbcolor.s = num4 / num2 * 1f;
		hsbcolor.b = num2;
		return hsbcolor;
	}

	// Token: 0x0600A2CA RID: 41674 RVA: 0x003D2278 File Offset: 0x003D0478
	public static Color ToColor(HSBColor hsbColor)
	{
		float num = hsbColor.b;
		float num2 = hsbColor.b;
		float num3 = hsbColor.b;
		if (hsbColor.s != 0f)
		{
			float num4 = hsbColor.b;
			float num5 = hsbColor.b * hsbColor.s;
			float num6 = hsbColor.b - num5;
			float num7 = hsbColor.h * 360f;
			if (num7 < 60f)
			{
				num = num4;
				num2 = num7 * num5 / 60f + num6;
				num3 = num6;
			}
			else if (num7 < 120f)
			{
				num = -(num7 - 120f) * num5 / 60f + num6;
				num2 = num4;
				num3 = num6;
			}
			else if (num7 < 180f)
			{
				num = num6;
				num2 = num4;
				num3 = (num7 - 120f) * num5 / 60f + num6;
			}
			else if (num7 < 240f)
			{
				num = num6;
				num2 = -(num7 - 240f) * num5 / 60f + num6;
				num3 = num4;
			}
			else if (num7 < 300f)
			{
				num = (num7 - 240f) * num5 / 60f + num6;
				num2 = num6;
				num3 = num4;
			}
			else if (num7 <= 360f)
			{
				num = num4;
				num2 = num6;
				num3 = -(num7 - 360f) * num5 / 60f + num6;
			}
			else
			{
				num = 0f;
				num2 = 0f;
				num3 = 0f;
			}
		}
		return new Color(Mathf.Clamp01(num), Mathf.Clamp01(num2), Mathf.Clamp01(num3), hsbColor.a);
	}

	// Token: 0x0600A2CB RID: 41675 RVA: 0x003D23EC File Offset: 0x003D05EC
	public Color ToColor()
	{
		return HSBColor.ToColor(this);
	}

	// Token: 0x0600A2CC RID: 41676 RVA: 0x003D23FC File Offset: 0x003D05FC
	public override string ToString()
	{
		return string.Concat(new string[]
		{
			"H:",
			this.h.ToCultureInvariantString(),
			" S:",
			this.s.ToCultureInvariantString(),
			" B:",
			this.b.ToCultureInvariantString()
		});
	}

	// Token: 0x0600A2CD RID: 41677 RVA: 0x003D2458 File Offset: 0x003D0658
	public static HSBColor Lerp(HSBColor a, HSBColor b, float t)
	{
		float num;
		float num2;
		if (a.b == 0f)
		{
			num = b.h;
			num2 = b.s;
		}
		else if (b.b == 0f)
		{
			num = a.h;
			num2 = a.s;
		}
		else
		{
			if (a.s == 0f)
			{
				num = b.h;
			}
			else if (b.s == 0f)
			{
				num = a.h;
			}
			else
			{
				float num3;
				for (num3 = Mathf.LerpAngle(a.h * 360f, b.h * 360f, t); num3 < 0f; num3 += 360f)
				{
				}
				while (num3 > 360f)
				{
					num3 -= 360f;
				}
				num = num3 / 360f;
			}
			num2 = Mathf.Lerp(a.s, b.s, t);
		}
		return new HSBColor(num, num2, Mathf.Lerp(a.b, b.b, t), Mathf.Lerp(a.a, b.a, t));
	}

	// Token: 0x04007A64 RID: 31332
	public float h;

	// Token: 0x04007A65 RID: 31333
	public float s;

	// Token: 0x04007A66 RID: 31334
	public float b;

	// Token: 0x04007A67 RID: 31335
	public float a;
}
