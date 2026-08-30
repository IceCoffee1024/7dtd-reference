using System;

// Token: 0x020014F5 RID: 5365
public class SimplexNoise
{
	// Token: 0x0600A89E RID: 43166 RVA: 0x003EEF44 File Offset: 0x003ED144
	public static float noise(float x, float y, float z)
	{
		SimplexNoise.s = (x + y + z) * 0.33333334f;
		SimplexNoise.i = SimplexNoise.fastfloor(x + SimplexNoise.s);
		SimplexNoise.j = SimplexNoise.fastfloor(y + SimplexNoise.s);
		SimplexNoise.k = SimplexNoise.fastfloor(z + SimplexNoise.s);
		SimplexNoise.s = (float)(SimplexNoise.i + SimplexNoise.j + SimplexNoise.k) * 0.16666667f;
		SimplexNoise.u = x - (float)SimplexNoise.i + SimplexNoise.s;
		SimplexNoise.v = y - (float)SimplexNoise.j + SimplexNoise.s;
		SimplexNoise.w = z - (float)SimplexNoise.k + SimplexNoise.s;
		SimplexNoise.A[0] = (SimplexNoise.A[1] = (SimplexNoise.A[2] = 0));
		int num = ((SimplexNoise.u >= SimplexNoise.w) ? ((SimplexNoise.u >= SimplexNoise.v) ? 0 : 1) : ((SimplexNoise.v >= SimplexNoise.w) ? 1 : 2));
		int num2 = ((SimplexNoise.u < SimplexNoise.w) ? ((SimplexNoise.u < SimplexNoise.v) ? 0 : 1) : ((SimplexNoise.v < SimplexNoise.w) ? 1 : 2));
		return SimplexNoise.K(num) + SimplexNoise.K(3 - num - num2) + SimplexNoise.K(num2) + SimplexNoise.K(0);
	}

	// Token: 0x0600A89F RID: 43167 RVA: 0x003EF083 File Offset: 0x003ED283
	[PublicizedFrom(EAccessModifier.Private)]
	public static int fastfloor(float n)
	{
		if (n <= 0f)
		{
			return (int)n - 1;
		}
		return (int)n;
	}

	// Token: 0x0600A8A0 RID: 43168 RVA: 0x003EF094 File Offset: 0x003ED294
	[PublicizedFrom(EAccessModifier.Private)]
	public static float K(int a)
	{
		SimplexNoise.s = (float)(SimplexNoise.A[0] + SimplexNoise.A[1] + SimplexNoise.A[2]) * 0.16666667f;
		float num = SimplexNoise.u - (float)SimplexNoise.A[0] + SimplexNoise.s;
		float num2 = SimplexNoise.v - (float)SimplexNoise.A[1] + SimplexNoise.s;
		float num3 = SimplexNoise.w - (float)SimplexNoise.A[2] + SimplexNoise.s;
		float num4 = 0.6f - num * num - num2 * num2 - num3 * num3;
		int num5 = SimplexNoise.shuffle(SimplexNoise.i + SimplexNoise.A[0], SimplexNoise.j + SimplexNoise.A[1], SimplexNoise.k + SimplexNoise.A[2]);
		SimplexNoise.A[a]++;
		if (num4 < 0f)
		{
			return 0f;
		}
		int num6 = (num5 >> 5) & 1;
		int num7 = (num5 >> 4) & 1;
		int num8 = (num5 >> 3) & 1;
		int num9 = (num5 >> 2) & 1;
		int num10 = num5 & 3;
		float num11 = ((num10 == 1) ? num : ((num10 == 2) ? num2 : num3));
		float num12 = ((num10 == 1) ? num2 : ((num10 == 2) ? num3 : num));
		float num13 = ((num10 == 1) ? num3 : ((num10 == 2) ? num : num2));
		num11 = ((num6 == num8) ? (-num11) : num11);
		num12 = ((num6 == num7) ? (-num12) : num12);
		num13 = ((num6 != (num7 ^ num8)) ? (-num13) : num13);
		num4 *= num4;
		return 8f * num4 * num4 * (num11 + ((num10 == 0) ? (num12 + num13) : ((num9 == 0) ? num12 : num13)));
	}

	// Token: 0x0600A8A1 RID: 43169 RVA: 0x003EF218 File Offset: 0x003ED418
	[PublicizedFrom(EAccessModifier.Private)]
	public static int shuffle(int i, int j, int k)
	{
		return SimplexNoise.b(i, j, k, 0) + SimplexNoise.b(j, k, i, 1) + SimplexNoise.b(k, i, j, 2) + SimplexNoise.b(i, j, k, 3) + SimplexNoise.b(j, k, i, 4) + SimplexNoise.b(k, i, j, 5) + SimplexNoise.b(i, j, k, 6) + SimplexNoise.b(j, k, i, 7);
	}

	// Token: 0x0600A8A2 RID: 43170 RVA: 0x003EF274 File Offset: 0x003ED474
	[PublicizedFrom(EAccessModifier.Private)]
	public static int b(int i, int j, int k, int B)
	{
		return SimplexNoise.T[(SimplexNoise.b(i, B) << 2) | (SimplexNoise.b(j, B) << 1) | SimplexNoise.b(k, B)];
	}

	// Token: 0x0600A8A3 RID: 43171 RVA: 0x003EF297 File Offset: 0x003ED497
	[PublicizedFrom(EAccessModifier.Private)]
	public static int b(int N, int B)
	{
		return (N >> B) & 1;
	}

	// Token: 0x04007CC8 RID: 31944
	[PublicizedFrom(EAccessModifier.Private)]
	public static int i;

	// Token: 0x04007CC9 RID: 31945
	[PublicizedFrom(EAccessModifier.Private)]
	public static int j;

	// Token: 0x04007CCA RID: 31946
	[PublicizedFrom(EAccessModifier.Private)]
	public static int k;

	// Token: 0x04007CCB RID: 31947
	[PublicizedFrom(EAccessModifier.Private)]
	public static int[] A = new int[3];

	// Token: 0x04007CCC RID: 31948
	[PublicizedFrom(EAccessModifier.Private)]
	public static float u;

	// Token: 0x04007CCD RID: 31949
	[PublicizedFrom(EAccessModifier.Private)]
	public static float v;

	// Token: 0x04007CCE RID: 31950
	[PublicizedFrom(EAccessModifier.Private)]
	public static float w;

	// Token: 0x04007CCF RID: 31951
	[PublicizedFrom(EAccessModifier.Private)]
	public static float s;

	// Token: 0x04007CD0 RID: 31952
	[PublicizedFrom(EAccessModifier.Private)]
	public const float onethird = 0.33333334f;

	// Token: 0x04007CD1 RID: 31953
	[PublicizedFrom(EAccessModifier.Private)]
	public const float onesixth = 0.16666667f;

	// Token: 0x04007CD2 RID: 31954
	[PublicizedFrom(EAccessModifier.Private)]
	public static int[] T = new int[] { 21, 56, 50, 44, 13, 19, 7, 42 };
}
