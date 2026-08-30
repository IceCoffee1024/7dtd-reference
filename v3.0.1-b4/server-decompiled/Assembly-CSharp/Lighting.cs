using System;
using UnityEngine;

// Token: 0x02000B3C RID: 2876
public struct Lighting
{
	// Token: 0x06005749 RID: 22345 RVA: 0x00215A87 File Offset: 0x00213C87
	public Lighting(byte _sun, byte _block, byte _stability)
	{
		this.sun = _sun;
		this.block = _block;
		this.stability = _stability;
	}

	// Token: 0x0600574A RID: 22346 RVA: 0x00215A9E File Offset: 0x00213C9E
	public Color ToColor()
	{
		return new Color((float)this.sun * 0.06666667f, 0f, (float)this.stability * 0.06666667f, (float)this.block * 0.06666667f);
	}

	// Token: 0x0600574B RID: 22347 RVA: 0x00215AD4 File Offset: 0x00213CD4
	public static Color ToColor(int _sunLight, int _blockLight)
	{
		Color color;
		color.r = (float)_sunLight * 0.06666667f;
		color.g = 0f;
		color.b = 0f;
		color.a = (float)_blockLight * 0.06666667f;
		return color;
	}

	// Token: 0x0600574C RID: 22348 RVA: 0x00215B18 File Offset: 0x00213D18
	public static Color ToColor(int _sunLight, int _blockLight, float _sideFactor)
	{
		Color color;
		color.r = (float)_sunLight * 0.06666667f * _sideFactor;
		color.g = 0f;
		color.b = 0f;
		color.a = (float)_blockLight * 0.06666667f;
		return color;
	}

	// Token: 0x0400432F RID: 17199
	public static Lighting one = new Lighting(15, 15, 0);

	// Token: 0x04004330 RID: 17200
	public byte sun;

	// Token: 0x04004331 RID: 17201
	public byte block;

	// Token: 0x04004332 RID: 17202
	public byte stability;

	// Token: 0x04004333 RID: 17203
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cToPer = 0.06666667f;
}
