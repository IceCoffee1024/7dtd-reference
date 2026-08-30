using System;

// Token: 0x0200155C RID: 5468
public class vp_Perlin
{
	// Token: 0x0600AAFB RID: 43771 RVA: 0x003FB6D1 File Offset: 0x003F98D1
	[PublicizedFrom(EAccessModifier.Private)]
	public float s_curve(float t)
	{
		return t * t * (3f - 2f * t);
	}

	// Token: 0x0600AAFC RID: 43772 RVA: 0x003EEF1F File Offset: 0x003ED11F
	[PublicizedFrom(EAccessModifier.Private)]
	public float lerp(float t, float a, float b)
	{
		return a + t * (b - a);
	}

	// Token: 0x0600AAFD RID: 43773 RVA: 0x003FB6E4 File Offset: 0x003F98E4
	[PublicizedFrom(EAccessModifier.Private)]
	public void setup(float value, out int b0, out int b1, out float r0, out float r1)
	{
		float num = value + 4096f;
		b0 = (int)num & 255;
		b1 = (b0 + 1) & 255;
		r0 = num - (float)((int)num);
		r1 = r0 - 1f;
	}

	// Token: 0x0600AAFE RID: 43774 RVA: 0x003FB723 File Offset: 0x003F9923
	[PublicizedFrom(EAccessModifier.Private)]
	public float at2(float rx, float ry, float x, float y)
	{
		return rx * x + ry * y;
	}

	// Token: 0x0600AAFF RID: 43775 RVA: 0x003FB72D File Offset: 0x003F992D
	[PublicizedFrom(EAccessModifier.Private)]
	public float at3(float rx, float ry, float rz, float x, float y, float z)
	{
		return rx * x + ry * y + rz * z;
	}

	// Token: 0x0600AB00 RID: 43776 RVA: 0x003FB740 File Offset: 0x003F9940
	public float Noise(float arg)
	{
		int num;
		int num2;
		float num3;
		float num4;
		this.setup(arg, out num, out num2, out num3, out num4);
		float num5 = this.s_curve(num3);
		float num6 = num3 * this.g1[this.p[num]];
		float num7 = num4 * this.g1[this.p[num2]];
		return this.lerp(num5, num6, num7);
	}

	// Token: 0x0600AB01 RID: 43777 RVA: 0x003FB798 File Offset: 0x003F9998
	public float Noise(float x, float y)
	{
		int num;
		int num2;
		float num3;
		float num4;
		this.setup(x, out num, out num2, out num3, out num4);
		int num5;
		int num6;
		float num7;
		float num8;
		this.setup(y, out num5, out num6, out num7, out num8);
		int num9 = this.p[num];
		int num10 = this.p[num2];
		int num11 = this.p[num9 + num5];
		int num12 = this.p[num10 + num5];
		int num13 = this.p[num9 + num6];
		int num14 = this.p[num10 + num6];
		float num15 = this.s_curve(num3);
		float num16 = this.s_curve(num7);
		float num17 = this.at2(num3, num7, this.g2[num11, 0], this.g2[num11, 1]);
		float num18 = this.at2(num4, num7, this.g2[num12, 0], this.g2[num12, 1]);
		float num19 = this.lerp(num15, num17, num18);
		num17 = this.at2(num3, num8, this.g2[num13, 0], this.g2[num13, 1]);
		num18 = this.at2(num4, num8, this.g2[num14, 0], this.g2[num14, 1]);
		float num20 = this.lerp(num15, num17, num18);
		return this.lerp(num16, num19, num20);
	}

	// Token: 0x0600AB02 RID: 43778 RVA: 0x003FB8E8 File Offset: 0x003F9AE8
	public float Noise(float x, float y, float z)
	{
		int num;
		int num2;
		float num3;
		float num4;
		this.setup(x, out num, out num2, out num3, out num4);
		int num5;
		int num6;
		float num7;
		float num8;
		this.setup(y, out num5, out num6, out num7, out num8);
		int num9;
		int num10;
		float num11;
		float num12;
		this.setup(z, out num9, out num10, out num11, out num12);
		int num13 = this.p[num];
		int num14 = this.p[num2];
		int num15 = this.p[num13 + num5];
		int num16 = this.p[num14 + num5];
		int num17 = this.p[num13 + num6];
		int num18 = this.p[num14 + num6];
		float num19 = this.s_curve(num3);
		float num20 = this.s_curve(num7);
		float num21 = this.s_curve(num11);
		float num22 = this.at3(num3, num7, num11, this.g3[num15 + num9, 0], this.g3[num15 + num9, 1], this.g3[num15 + num9, 2]);
		float num23 = this.at3(num4, num7, num11, this.g3[num16 + num9, 0], this.g3[num16 + num9, 1], this.g3[num16 + num9, 2]);
		float num24 = this.lerp(num19, num22, num23);
		num22 = this.at3(num3, num8, num11, this.g3[num17 + num9, 0], this.g3[num17 + num9, 1], this.g3[num17 + num9, 2]);
		num23 = this.at3(num4, num8, num11, this.g3[num18 + num9, 0], this.g3[num18 + num9, 1], this.g3[num18 + num9, 2]);
		float num25 = this.lerp(num19, num22, num23);
		float num26 = this.lerp(num20, num24, num25);
		num22 = this.at3(num3, num7, num12, this.g3[num15 + num10, 0], this.g3[num15 + num10, 2], this.g3[num15 + num10, 2]);
		num23 = this.at3(num4, num7, num12, this.g3[num16 + num10, 0], this.g3[num16 + num10, 1], this.g3[num16 + num10, 2]);
		num24 = this.lerp(num19, num22, num23);
		num22 = this.at3(num3, num8, num12, this.g3[num17 + num10, 0], this.g3[num17 + num10, 1], this.g3[num17 + num10, 2]);
		num23 = this.at3(num4, num8, num12, this.g3[num18 + num10, 0], this.g3[num18 + num10, 1], this.g3[num18 + num10, 2]);
		num25 = this.lerp(num19, num22, num23);
		float num27 = this.lerp(num20, num24, num25);
		return this.lerp(num21, num26, num27);
	}

	// Token: 0x0600AB03 RID: 43779 RVA: 0x003FBBF0 File Offset: 0x003F9DF0
	[PublicizedFrom(EAccessModifier.Private)]
	public void normalize2(ref float x, ref float y)
	{
		float num = (float)Math.Sqrt((double)(x * x + y * y));
		x = y / num;
		y /= num;
	}

	// Token: 0x0600AB04 RID: 43780 RVA: 0x003FBC1C File Offset: 0x003F9E1C
	[PublicizedFrom(EAccessModifier.Private)]
	public void normalize3(ref float x, ref float y, ref float z)
	{
		float num = (float)Math.Sqrt((double)(x * x + y * y + z * z));
		x = y / num;
		y /= num;
		z /= num;
	}

	// Token: 0x0600AB05 RID: 43781 RVA: 0x003FBC54 File Offset: 0x003F9E54
	public vp_Perlin()
	{
		Random random = new Random();
		int i;
		for (i = 0; i < 256; i++)
		{
			this.p[i] = i;
			this.g1[i] = (float)(random.Next(512) - 256) / 256f;
			for (int j = 0; j < 2; j++)
			{
				this.g2[i, j] = (float)(random.Next(512) - 256) / 256f;
			}
			this.normalize2(ref this.g2[i, 0], ref this.g2[i, 1]);
			for (int j = 0; j < 3; j++)
			{
				this.g3[i, j] = (float)(random.Next(512) - 256) / 256f;
			}
			this.normalize3(ref this.g3[i, 0], ref this.g3[i, 1], ref this.g3[i, 2]);
		}
		while (--i != 0)
		{
			int num = this.p[i];
			int j;
			this.p[i] = this.p[j = random.Next(256)];
			this.p[j] = num;
		}
		for (i = 0; i < 258; i++)
		{
			this.p[256 + i] = this.p[i];
			this.g1[256 + i] = this.g1[i];
			for (int j = 0; j < 2; j++)
			{
				this.g2[256 + i, j] = this.g2[i, j];
			}
			for (int j = 0; j < 3; j++)
			{
				this.g3[256 + i, j] = this.g3[i, j];
			}
		}
	}

	// Token: 0x04007E63 RID: 32355
	[PublicizedFrom(EAccessModifier.Private)]
	public const int B = 256;

	// Token: 0x04007E64 RID: 32356
	[PublicizedFrom(EAccessModifier.Private)]
	public const int BM = 255;

	// Token: 0x04007E65 RID: 32357
	[PublicizedFrom(EAccessModifier.Private)]
	public const int N = 4096;

	// Token: 0x04007E66 RID: 32358
	[PublicizedFrom(EAccessModifier.Private)]
	public int[] p = new int[514];

	// Token: 0x04007E67 RID: 32359
	[PublicizedFrom(EAccessModifier.Private)]
	public float[,] g3 = new float[514, 3];

	// Token: 0x04007E68 RID: 32360
	[PublicizedFrom(EAccessModifier.Private)]
	public float[,] g2 = new float[514, 2];

	// Token: 0x04007E69 RID: 32361
	[PublicizedFrom(EAccessModifier.Private)]
	public float[] g1 = new float[514];
}
