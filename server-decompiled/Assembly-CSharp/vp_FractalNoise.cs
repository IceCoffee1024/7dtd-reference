using System;
using UnityEngine;

// Token: 0x020015AC RID: 5548
public class vp_FractalNoise
{
	// Token: 0x0600AD0E RID: 44302 RVA: 0x00408406 File Offset: 0x00406606
	public vp_FractalNoise(float inH, float inLacunarity, float inOctaves)
		: this(inH, inLacunarity, inOctaves, null)
	{
	}

	// Token: 0x0600AD0F RID: 44303 RVA: 0x00408414 File Offset: 0x00406614
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

	// Token: 0x0600AD10 RID: 44304 RVA: 0x004084A4 File Offset: 0x004066A4
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

	// Token: 0x0600AD11 RID: 44305 RVA: 0x00408568 File Offset: 0x00406768
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

	// Token: 0x0600AD12 RID: 44306 RVA: 0x004085F8 File Offset: 0x004067F8
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

	// Token: 0x04008007 RID: 32775
	[PublicizedFrom(EAccessModifier.Private)]
	public vp_Perlin m_Noise;

	// Token: 0x04008008 RID: 32776
	[PublicizedFrom(EAccessModifier.Private)]
	public float[] m_Exponent;

	// Token: 0x04008009 RID: 32777
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_IntOctaves;

	// Token: 0x0400800A RID: 32778
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_Octaves;

	// Token: 0x0400800B RID: 32779
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_Lacunarity;
}
