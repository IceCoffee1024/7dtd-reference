using System;
using UnityEngine;

// Token: 0x02000B60 RID: 2912
public struct Lighting
{
	// Token: 0x060057EB RID: 22507 RVA: 0x0021BA2F File Offset: 0x00219C2F
	public Lighting(byte _sun, byte _block, byte _stability)
	{
		this.sun = _sun;
		this.block = _block;
		this.stability = _stability;
	}

	// Token: 0x060057EC RID: 22508 RVA: 0x0021BA46 File Offset: 0x00219C46
	public Color ToColor()
	{
		return new Color((float)this.sun * 0.06666667f, 0f, (float)this.stability * 0.06666667f, (float)this.block * 0.06666667f);
	}

	// Token: 0x060057ED RID: 22509 RVA: 0x0021BA7C File Offset: 0x00219C7C
	public static Color ToColor(int _sunLight, int _blockLight)
	{
		Color color;
		color.r = (float)_sunLight * 0.06666667f;
		color.g = 0f;
		color.b = 0f;
		color.a = (float)_blockLight * 0.06666667f;
		return color;
	}

	// Token: 0x060057EE RID: 22510 RVA: 0x0021BAC0 File Offset: 0x00219CC0
	public static Color ToColor(int _sunLight, int _blockLight, float _sideFactor)
	{
		Color color;
		color.r = (float)_sunLight * 0.06666667f * _sideFactor;
		color.g = 0f;
		color.b = 0f;
		color.a = (float)_blockLight * 0.06666667f;
		return color;
	}

	// Token: 0x04004408 RID: 17416
	public static Lighting one = new Lighting(15, 15, 0);

	// Token: 0x04004409 RID: 17417
	public byte sun;

	// Token: 0x0400440A RID: 17418
	public byte block;

	// Token: 0x0400440B RID: 17419
	public byte stability;

	// Token: 0x0400440C RID: 17420
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cToPer = 0.06666667f;
}
