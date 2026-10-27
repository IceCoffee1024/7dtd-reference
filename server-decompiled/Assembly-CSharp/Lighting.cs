using System;
using UnityEngine;

// Token: 0x02000B6B RID: 2923
public struct Lighting
{
	// Token: 0x06005810 RID: 22544 RVA: 0x0021CF8B File Offset: 0x0021B18B
	public Lighting(byte _sun, byte _block, byte _stability)
	{
		this.sun = _sun;
		this.block = _block;
		this.stability = _stability;
	}

	// Token: 0x06005811 RID: 22545 RVA: 0x0021CFA2 File Offset: 0x0021B1A2
	public Color ToColor()
	{
		return new Color((float)this.sun * 0.06666667f, 0f, (float)this.stability * 0.06666667f, (float)this.block * 0.06666667f);
	}

	// Token: 0x06005812 RID: 22546 RVA: 0x0021CFD8 File Offset: 0x0021B1D8
	public static Color ToColor(int _sunLight, int _blockLight)
	{
		Color color;
		color.r = (float)_sunLight * 0.06666667f;
		color.g = 0f;
		color.b = 0f;
		color.a = (float)_blockLight * 0.06666667f;
		return color;
	}

	// Token: 0x06005813 RID: 22547 RVA: 0x0021D01C File Offset: 0x0021B21C
	public static Color ToColor(int _sunLight, int _blockLight, float _sideFactor)
	{
		Color color;
		color.r = (float)_sunLight * 0.06666667f * _sideFactor;
		color.g = 0f;
		color.b = 0f;
		color.a = (float)_blockLight * 0.06666667f;
		return color;
	}

	// Token: 0x0400442D RID: 17453
	public static Lighting one = new Lighting(15, 15, 0);

	// Token: 0x0400442E RID: 17454
	public byte sun;

	// Token: 0x0400442F RID: 17455
	public byte block;

	// Token: 0x04004430 RID: 17456
	public byte stability;

	// Token: 0x04004431 RID: 17457
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cToPer = 0.06666667f;
}
