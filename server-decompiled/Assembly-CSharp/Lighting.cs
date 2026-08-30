using System;
using UnityEngine;

// Token: 0x02000B4E RID: 2894
public struct Lighting
{
	// Token: 0x060057D1 RID: 22481 RVA: 0x002187A3 File Offset: 0x002169A3
	public Lighting(byte _sun, byte _block, byte _stability)
	{
		this.sun = _sun;
		this.block = _block;
		this.stability = _stability;
	}

	// Token: 0x060057D2 RID: 22482 RVA: 0x002187BA File Offset: 0x002169BA
	public Color ToColor()
	{
		return new Color((float)this.sun * 0.06666667f, 0f, (float)this.stability * 0.06666667f, (float)this.block * 0.06666667f);
	}

	// Token: 0x060057D3 RID: 22483 RVA: 0x002187F0 File Offset: 0x002169F0
	public static Color ToColor(int _sunLight, int _blockLight)
	{
		Color color;
		color.r = (float)_sunLight * 0.06666667f;
		color.g = 0f;
		color.b = 0f;
		color.a = (float)_blockLight * 0.06666667f;
		return color;
	}

	// Token: 0x060057D4 RID: 22484 RVA: 0x00218834 File Offset: 0x00216A34
	public static Color ToColor(int _sunLight, int _blockLight, float _sideFactor)
	{
		Color color;
		color.r = (float)_sunLight * 0.06666667f * _sideFactor;
		color.g = 0f;
		color.b = 0f;
		color.a = (float)_blockLight * 0.06666667f;
		return color;
	}

	// Token: 0x040043CA RID: 17354
	public static Lighting one = new Lighting(15, 15, 0);

	// Token: 0x040043CB RID: 17355
	public byte sun;

	// Token: 0x040043CC RID: 17356
	public byte block;

	// Token: 0x040043CD RID: 17357
	public byte stability;

	// Token: 0x040043CE RID: 17358
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cToPer = 0.06666667f;
}
