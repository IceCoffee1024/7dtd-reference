using System;
using UnityEngine;

// Token: 0x0200155D RID: 5469
public class vp_FractalNoise
{
	// Token: 0x0600AB06 RID: 43782 RVA: 0x003FBE6E File Offset: 0x003FA06E
	public vp_FractalNoise(float inH, float inLacunarity, float inOctaves)
		: this(inH, inLacunarity, inOctaves, null)
	{
	}

	// Token: 0x0600AB07 RID: 43783 RVA: 0x003FBE7C File Offset: 0x003FA07C
	public vp_FractalNoise(float inH, float inLacunarity, float inOctaves, vp_Perlin noise)
	{
		this.m_Lacunarity = inLacunarity;
		this.m_Octaves = inOctaves;
		this.m_IntOctaves = (int)inOctaves;
		this.m_Exponent = new float[this.m_IntOctaves + 1];
		float num = 1f;
		for (int i = 0; i < this.m_IntOctaves + 1; i++)
		{
			this.m_Exponent[i] = (float)Math.Pow((double)this.m_Lacunarity, (double)(-(double)inH));
			num *= this.m_Lacunarity;
		}
		if (noise == null)
		{
			this.m_Noise = new vp_Perlin();
			return;
		}
		this.m_Noise = noise;
	}

	// Token: 0x0600AB08 RID: 43784 RVA: 0x003FBF0C File Offset: 0x003FA10C
	public float HybridMultifractal(float x, float y, float offset)
	{
		float num = (this.m_Noise.Noise(x, y) + offset) * this.m_Exponent[0];
		float num2 = num;
		x *= this.m_Lacunarity;
		y *= this.m_Lacunarity;
		int i;
		for (i = 1; i < this.m_IntOctaves; i++)
		{
			if (num2 > 1f)
			{
				num2 = 1f;
			}
			float num3 = (this.m_Noise.Noise(x, y) + offset) * this.m_Exponent[i];
			num += num2 * num3;
			num2 *= num3;
			x *= this.m_Lacunarity;
			y *= this.m_Lacunarity;
		}
		float num4 = this.m_Octaves - (float)this.m_IntOctaves;
		return num + num4 * this.m_Noise.Noise(x, y) * this.m_Exponent[i];
	}

	// Token: 0x0600AB09 RID: 43785 RVA: 0x003FBFD0 File Offset: 0x003FA1D0
	public float RidgedMultifractal(float x, float y, float offset, float gain)
	{
		float num = Mathf.Abs(this.m_Noise.Noise(x, y));
		num = offset - num;
		num *= num;
		float num2 = num;
		for (int i = 1; i < this.m_IntOctaves; i++)
		{
			x *= this.m_Lacunarity;
			y *= this.m_Lacunarity;
			float num3 = num * gain;
			num3 = Mathf.Clamp01(num3);
			num = Mathf.Abs(this.m_Noise.Noise(x, y));
			num = offset - num;
			num *= num;
			num *= num3;
			num2 += num * this.m_Exponent[i];
		}
		return num2;
	}

	// Token: 0x0600AB0A RID: 43786 RVA: 0x003FC060 File Offset: 0x003FA260
	public float BrownianMotion(float x, float y)
	{
		float num = 0f;
		long num2;
		for (num2 = 0L; num2 < (long)this.m_IntOctaves; num2 += 1L)
		{
			num = this.m_Noise.Noise(x, y) * this.m_Exponent[(int)(checked((IntPtr)num2))];
			x *= this.m_Lacunarity;
			y *= this.m_Lacunarity;
		}
		float num3 = this.m_Octaves - (float)this.m_IntOctaves;
		return num + num3 * this.m_Noise.Noise(x, y) * this.m_Exponent[(int)(checked((IntPtr)num2))];
	}

	// Token: 0x04007E6A RID: 32362
	[PublicizedFrom(EAccessModifier.Private)]
	public vp_Perlin m_Noise;

	// Token: 0x04007E6B RID: 32363
	[PublicizedFrom(EAccessModifier.Private)]
	public float[] m_Exponent;

	// Token: 0x04007E6C RID: 32364
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_IntOctaves;

	// Token: 0x04007E6D RID: 32365
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_Octaves;

	// Token: 0x04007E6E RID: 32366
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_Lacunarity;
}
