using System;
using System.Runtime.CompilerServices;

// Token: 0x0200150D RID: 5389
public static class OpenSimplex2S
{
	// Token: 0x0600A980 RID: 43392 RVA: 0x003F11C0 File Offset: 0x003EF3C0
	public static float Noise2(long seed, double x, double y)
	{
		double num = 0.366025403784439 * (x + y);
		double num2 = x + num;
		double num3 = y + num;
		return OpenSimplex2S.Noise2_UnskewedBase(seed, num2, num3);
	}

	// Token: 0x0600A981 RID: 43393 RVA: 0x003F11EC File Offset: 0x003EF3EC
	public static float Noise2_ImproveX(long seed, double x, double y)
	{
		double num = x * 0.7071067811865476;
		double num2 = y * 1.2247448713915896;
		return OpenSimplex2S.Noise2_UnskewedBase(seed, num2 + num, num2 - num);
	}

	// Token: 0x0600A982 RID: 43394 RVA: 0x003F1220 File Offset: 0x003EF420
	[PublicizedFrom(EAccessModifier.Private)]
	public static float Noise2_UnskewedBase(long seed, double xs, double ys)
	{
		int num = OpenSimplex2S.FastFloor(xs);
		int num2 = OpenSimplex2S.FastFloor(ys);
		float num3 = (float)(xs - (double)num);
		float num4 = (float)(ys - (double)num2);
		long num5 = (long)num * 5910200641878280303L;
		long num6 = (long)num2 * 6452764530575939509L;
		float num7 = (num3 + num4) * -0.21132487f;
		float num8 = num3 + num7;
		float num9 = num4 + num7;
		float num10 = 0.6666667f - num8 * num8 - num9 * num9;
		float num11 = num10 * num10 * (num10 * num10) * OpenSimplex2S.Grad(seed, num5, num6, num8, num9);
		float num12 = -3.1547005f * num7 + (-0.6666667f + num10);
		float num13 = num8 - 0.57735026f;
		float num14 = num9 - 0.57735026f;
		num11 += num12 * num12 * (num12 * num12) * OpenSimplex2S.Grad(seed, num5 + 5910200641878280303L, num6 + 6452764530575939509L, num13, num14);
		float num15 = num3 - num4;
		if ((double)num7 < -0.21132486540518713)
		{
			if (num3 + num15 > 1f)
			{
				float num16 = num8 - 1.3660254f;
				float num17 = num9 - 0.36602542f;
				float num18 = 0.6666667f - num16 * num16 - num17 * num17;
				if (num18 > 0f)
				{
					num11 += num18 * num18 * (num18 * num18) * OpenSimplex2S.Grad(seed, num5 + -6626342789952991010L, num6 + 6452764530575939509L, num16, num17);
				}
			}
			else
			{
				float num19 = num8 - -0.21132487f;
				float num20 = num9 - 0.7886751f;
				float num21 = 0.6666667f - num19 * num19 - num20 * num20;
				if (num21 > 0f)
				{
					num11 += num21 * num21 * (num21 * num21) * OpenSimplex2S.Grad(seed, num5, num6 + 6452764530575939509L, num19, num20);
				}
			}
			if (num4 - num15 > 1f)
			{
				float num22 = num8 - 0.36602542f;
				float num23 = num9 - 1.3660254f;
				float num24 = 0.6666667f - num22 * num22 - num23 * num23;
				if (num24 > 0f)
				{
					num11 += num24 * num24 * (num24 * num24) * OpenSimplex2S.Grad(seed, num5 + 5910200641878280303L, num6 + -5541215012557672598L, num22, num23);
				}
			}
			else
			{
				float num25 = num8 - 0.7886751f;
				float num26 = num9 - -0.21132487f;
				float num27 = 0.6666667f - num25 * num25 - num26 * num26;
				if (num27 > 0f)
				{
					num11 += num27 * num27 * (num27 * num27) * OpenSimplex2S.Grad(seed, num5 + 5910200641878280303L, num6, num25, num26);
				}
			}
		}
		else
		{
			if (num3 + num15 < 0f)
			{
				float num28 = num8 + 0.7886751f;
				float num29 = num9 + -0.21132487f;
				float num30 = 0.6666667f - num28 * num28 - num29 * num29;
				if (num30 > 0f)
				{
					num11 += num30 * num30 * (num30 * num30) * OpenSimplex2S.Grad(seed, num5 - 5910200641878280303L, num6, num28, num29);
				}
			}
			else
			{
				float num31 = num8 - 0.7886751f;
				float num32 = num9 - -0.21132487f;
				float num33 = 0.6666667f - num31 * num31 - num32 * num32;
				if (num33 > 0f)
				{
					num11 += num33 * num33 * (num33 * num33) * OpenSimplex2S.Grad(seed, num5 + 5910200641878280303L, num6, num31, num32);
				}
			}
			if (num4 < num15)
			{
				float num34 = num8 + -0.21132487f;
				float num35 = num9 + 0.7886751f;
				float num36 = 0.6666667f - num34 * num34 - num35 * num35;
				if (num36 > 0f)
				{
					num11 += num36 * num36 * (num36 * num36) * OpenSimplex2S.Grad(seed, num5, num6 - 6452764530575939509L, num34, num35);
				}
			}
			else
			{
				float num37 = num8 - -0.21132487f;
				float num38 = num9 - 0.7886751f;
				float num39 = 0.6666667f - num37 * num37 - num38 * num38;
				if (num39 > 0f)
				{
					num11 += num39 * num39 * (num39 * num39) * OpenSimplex2S.Grad(seed, num5, num6 + 6452764530575939509L, num37, num38);
				}
			}
		}
		return num11;
	}

	// Token: 0x0600A983 RID: 43395 RVA: 0x003F163C File Offset: 0x003EF83C
	public static float Noise3_ImproveXY(long seed, double x, double y, double z)
	{
		double num = x + y;
		double num2 = num * -0.21132486540518713;
		double num3 = z * 0.577350269189626;
		double num4 = x + num2 + num3;
		double num5 = y + num2 + num3;
		double num6 = num * -0.577350269189626 + num3;
		return OpenSimplex2S.Noise3_UnrotatedBase(seed, num4, num5, num6);
	}

	// Token: 0x0600A984 RID: 43396 RVA: 0x003F1688 File Offset: 0x003EF888
	public static float Noise3_ImproveXZ(long seed, double x, double y, double z)
	{
		double num = x + z;
		double num2 = num * -0.211324865405187;
		double num3 = y * 0.577350269189626;
		double num4 = x + num2 + num3;
		double num5 = z + num2 + num3;
		double num6 = num * -0.577350269189626 + num3;
		return OpenSimplex2S.Noise3_UnrotatedBase(seed, num4, num6, num5);
	}

	// Token: 0x0600A985 RID: 43397 RVA: 0x003F16D4 File Offset: 0x003EF8D4
	public static float Noise3_Fallback(long seed, double x, double y, double z)
	{
		double num = 0.6666666666666666 * (x + y + z);
		double num2 = num - x;
		double num3 = num - y;
		double num4 = num - z;
		return OpenSimplex2S.Noise3_UnrotatedBase(seed, num2, num3, num4);
	}

	// Token: 0x0600A986 RID: 43398 RVA: 0x003F1704 File Offset: 0x003EF904
	[PublicizedFrom(EAccessModifier.Private)]
	public static float Noise3_UnrotatedBase(long seed, double xr, double yr, double zr)
	{
		int num = OpenSimplex2S.FastFloor(xr);
		int num2 = OpenSimplex2S.FastFloor(yr);
		int num3 = OpenSimplex2S.FastFloor(zr);
		float num4 = (float)(xr - (double)num);
		float num5 = (float)(yr - (double)num2);
		float num6 = (float)(zr - (double)num3);
		long num7 = (long)num * 5910200641878280303L;
		long num8 = (long)num2 * 6452764530575939509L;
		long num9 = (long)num3 * 6614699811220273867L;
		long num10 = seed ^ -5968755714895566377L;
		int num11 = (int)(-0.5f - num4);
		int num12 = (int)(-0.5f - num5);
		int num13 = (int)(-0.5f - num6);
		float num14 = num4 + (float)num11;
		float num15 = num5 + (float)num12;
		float num16 = num6 + (float)num13;
		float num17 = 0.75f - num14 * num14 - num15 * num15 - num16 * num16;
		float num18 = num17 * num17 * (num17 * num17) * OpenSimplex2S.Grad(seed, num7 + ((long)num11 & 5910200641878280303L), num8 + ((long)num12 & 6452764530575939509L), num9 + ((long)num13 & 6614699811220273867L), num14, num15, num16);
		float num19 = num4 - 0.5f;
		float num20 = num5 - 0.5f;
		float num21 = num6 - 0.5f;
		float num22 = 0.75f - num19 * num19 - num20 * num20 - num21 * num21;
		num18 += num22 * num22 * (num22 * num22) * OpenSimplex2S.Grad(num10, num7 + 5910200641878280303L, num8 + 6452764530575939509L, num9 + 6614699811220273867L, num19, num20, num21);
		float num23 = (float)((num11 | 1) << 1) * num19;
		float num24 = (float)((num12 | 1) << 1) * num20;
		float num25 = (float)((num13 | 1) << 1) * num21;
		float num26 = (float)(-2 - (num11 << 2)) * num19 - 1f;
		float num27 = (float)(-2 - (num12 << 2)) * num20 - 1f;
		float num28 = (float)(-2 - (num13 << 2)) * num21 - 1f;
		bool flag = false;
		float num29 = num23 + num17;
		if (num29 > 0f)
		{
			float num30 = num14 - (float)(num11 | 1);
			float num31 = num15;
			float num32 = num16;
			num18 += num29 * num29 * (num29 * num29) * OpenSimplex2S.Grad(seed, num7 + ((long)(~(long)num11) & 5910200641878280303L), num8 + ((long)num12 & 6452764530575939509L), num9 + ((long)num13 & 6614699811220273867L), num30, num31, num32);
		}
		else
		{
			float num33 = num24 + num25 + num17;
			if (num33 > 0f)
			{
				float num34 = num14;
				float num35 = num15 - (float)(num12 | 1);
				float num36 = num16 - (float)(num13 | 1);
				num18 += num33 * num33 * (num33 * num33) * OpenSimplex2S.Grad(seed, num7 + ((long)num11 & 5910200641878280303L), num8 + ((long)(~(long)num12) & 6452764530575939509L), num9 + ((long)(~(long)num13) & 6614699811220273867L), num34, num35, num36);
			}
			float num37 = num26 + num22;
			if (num37 > 0f)
			{
				float num38 = (float)(num11 | 1) + num19;
				float num39 = num20;
				float num40 = num21;
				num18 += num37 * num37 * (num37 * num37) * OpenSimplex2S.Grad(num10, num7 + ((long)num11 & -6626342789952991010L), num8 + 6452764530575939509L, num9 + 6614699811220273867L, num38, num39, num40);
				flag = true;
			}
		}
		bool flag2 = false;
		float num41 = num24 + num17;
		if (num41 > 0f)
		{
			float num42 = num14;
			float num43 = num15 - (float)(num12 | 1);
			float num44 = num16;
			num18 += num41 * num41 * (num41 * num41) * OpenSimplex2S.Grad(seed, num7 + ((long)num11 & 5910200641878280303L), num8 + ((long)(~(long)num12) & 6452764530575939509L), num9 + ((long)num13 & 6614699811220273867L), num42, num43, num44);
		}
		else
		{
			float num45 = num23 + num25 + num17;
			if (num45 > 0f)
			{
				float num46 = num14 - (float)(num11 | 1);
				float num47 = num15;
				float num48 = num16 - (float)(num13 | 1);
				num18 += num45 * num45 * (num45 * num45) * OpenSimplex2S.Grad(seed, num7 + ((long)(~(long)num11) & 5910200641878280303L), num8 + ((long)num12 & 6452764530575939509L), num9 + ((long)(~(long)num13) & 6614699811220273867L), num46, num47, num48);
			}
			float num49 = num27 + num22;
			if (num49 > 0f)
			{
				float num50 = num19;
				float num51 = (float)(num12 | 1) + num20;
				float num52 = num21;
				num18 += num49 * num49 * (num49 * num49) * OpenSimplex2S.Grad(num10, num7 + 5910200641878280303L, num8 + ((long)num12 & -5541215012557672598L), num9 + 6614699811220273867L, num50, num51, num52);
				flag2 = true;
			}
		}
		bool flag3 = false;
		float num53 = num25 + num17;
		if (num53 > 0f)
		{
			float num54 = num14;
			float num55 = num15;
			float num56 = num16 - (float)(num13 | 1);
			num18 += num53 * num53 * (num53 * num53) * OpenSimplex2S.Grad(seed, num7 + ((long)num11 & 5910200641878280303L), num8 + ((long)num12 & 6452764530575939509L), num9 + ((long)(~(long)num13) & 6614699811220273867L), num54, num55, num56);
		}
		else
		{
			float num57 = num23 + num24 + num17;
			if (num57 > 0f)
			{
				float num58 = num14 - (float)(num11 | 1);
				float num59 = num15 - (float)(num12 | 1);
				float num60 = num16;
				num18 += num57 * num57 * (num57 * num57) * OpenSimplex2S.Grad(seed, num7 + ((long)(~(long)num11) & 5910200641878280303L), num8 + ((long)(~(long)num12) & 6452764530575939509L), num9 + ((long)num13 & 6614699811220273867L), num58, num59, num60);
			}
			float num61 = num28 + num22;
			if (num61 > 0f)
			{
				float num62 = num19;
				float num63 = num20;
				float num64 = (float)(num13 | 1) + num21;
				num18 += num61 * num61 * (num61 * num61) * OpenSimplex2S.Grad(num10, num7 + 5910200641878280303L, num8 + 6452764530575939509L, num9 + ((long)num13 & -5217344451269003882L), num62, num63, num64);
				flag3 = true;
			}
		}
		if (!flag)
		{
			float num65 = num27 + num28 + num22;
			if (num65 > 0f)
			{
				float num66 = num19;
				float num67 = (float)(num12 | 1) + num20;
				float num68 = (float)(num13 | 1) + num21;
				num18 += num65 * num65 * (num65 * num65) * OpenSimplex2S.Grad(num10, num7 + 5910200641878280303L, num8 + ((long)num12 & -5541215012557672598L), num9 + ((long)num13 & -5217344451269003882L), num66, num67, num68);
			}
		}
		if (!flag2)
		{
			float num69 = num26 + num28 + num22;
			if (num69 > 0f)
			{
				float num70 = (float)(num11 | 1) + num19;
				float num71 = num20;
				float num72 = (float)(num13 | 1) + num21;
				num18 += num69 * num69 * (num69 * num69) * OpenSimplex2S.Grad(num10, num7 + ((long)num11 & -6626342789952991010L), num8 + 6452764530575939509L, num9 + ((long)num13 & -5217344451269003882L), num70, num71, num72);
			}
		}
		if (!flag3)
		{
			float num73 = num26 + num27 + num22;
			if (num73 > 0f)
			{
				float num74 = (float)(num11 | 1) + num19;
				float num75 = (float)(num12 | 1) + num20;
				float num76 = num21;
				num18 += num73 * num73 * (num73 * num73) * OpenSimplex2S.Grad(num10, num7 + ((long)num11 & -6626342789952991010L), num8 + ((long)num12 & -5541215012557672598L), num9 + 6614699811220273867L, num74, num75, num76);
			}
		}
		return num18;
	}

	// Token: 0x0600A987 RID: 43399 RVA: 0x003F1E64 File Offset: 0x003F0064
	public static float Noise4_ImproveXYZ_ImproveXY(long seed, double x, double y, double z, double w)
	{
		double num = x + y;
		double num2 = num * -0.211324865405187;
		double num3 = z * 0.2886751345948129;
		double num4 = w * 1.118033988749894;
		double num5 = x + (num3 + num4 + num2);
		double num6 = y + (num3 + num4 + num2);
		double num7 = num * -0.577350269189626 + (num3 + num4);
		double num8 = z * -0.866025403784439 + num4;
		return OpenSimplex2S.Noise4_UnskewedBase(seed, num5, num6, num7, num8);
	}

	// Token: 0x0600A988 RID: 43400 RVA: 0x003F1ED8 File Offset: 0x003F00D8
	public static float Noise4_ImproveXYZ_ImproveXZ(long seed, double x, double y, double z, double w)
	{
		double num = x + z;
		double num2 = num * -0.211324865405187;
		double num3 = y * 0.2886751345948129;
		double num4 = w * 1.118033988749894;
		double num5 = x + (num3 + num4 + num2);
		double num6 = z + (num3 + num4 + num2);
		double num7 = num * -0.577350269189626 + (num3 + num4);
		double num8 = y * -0.866025403784439 + num4;
		return OpenSimplex2S.Noise4_UnskewedBase(seed, num5, num7, num6, num8);
	}

	// Token: 0x0600A989 RID: 43401 RVA: 0x003F1F4C File Offset: 0x003F014C
	public static float Noise4_ImproveXYZ(long seed, double x, double y, double z, double w)
	{
		double num = x + y + z;
		double num2 = w * 1.118033988749894;
		double num3 = num * -0.16666666666666666 + num2;
		double num4 = x + num3;
		double num5 = y + num3;
		double num6 = z + num3;
		double num7 = -0.5 * num + num2;
		return OpenSimplex2S.Noise4_UnskewedBase(seed, num4, num5, num6, num7);
	}

	// Token: 0x0600A98A RID: 43402 RVA: 0x003F1FA4 File Offset: 0x003F01A4
	public static float Noise4_Fallback(long seed, double x, double y, double z, double w)
	{
		double num = 0.30901700258255005 * (x + y + z + w);
		double num2 = x + num;
		double num3 = y + num;
		double num4 = z + num;
		double num5 = w + num;
		return OpenSimplex2S.Noise4_UnskewedBase(seed, num2, num3, num4, num5);
	}

	// Token: 0x0600A98B RID: 43403 RVA: 0x003F1FE4 File Offset: 0x003F01E4
	[PublicizedFrom(EAccessModifier.Private)]
	public static float Noise4_UnskewedBase(long seed, double xs, double ys, double zs, double ws)
	{
		int num = OpenSimplex2S.FastFloor(xs);
		int num2 = OpenSimplex2S.FastFloor(ys);
		int num3 = OpenSimplex2S.FastFloor(zs);
		int num4 = OpenSimplex2S.FastFloor(ws);
		float num5 = (float)(xs - (double)num);
		float num6 = (float)(ys - (double)num2);
		float num7 = (float)(zs - (double)num3);
		float num8 = (float)(ws - (double)num4);
		float num9 = (num5 + num6 + num7 + num8) * -0.1381966f;
		float num10 = num5 + num9;
		float num11 = num6 + num9;
		float num12 = num7 + num9;
		float num13 = num8 + num9;
		long num14 = (long)num * 5910200641878280303L;
		long num15 = (long)num2 * 6452764530575939509L;
		long num16 = (long)num3 * 6614699811220273867L;
		long num17 = (long)num4 * 6254464313819354443L;
		int num18 = (OpenSimplex2S.FastFloor(xs * 4.0) & 3) | ((OpenSimplex2S.FastFloor(ys * 4.0) & 3) << 2) | ((OpenSimplex2S.FastFloor(zs * 4.0) & 3) << 4) | ((OpenSimplex2S.FastFloor(ws * 4.0) & 3) << 6);
		float num19 = 0f;
		ValueTuple<short, short> valueTuple = OpenSimplex2S.LOOKUP_4D_A[num18];
		int item = (int)valueTuple.Item1;
		int item2 = (int)valueTuple.Item2;
		int num20 = item;
		int num21 = item2;
		for (int i = num20; i < num21; i++)
		{
			OpenSimplex2S.LatticeVertex4D latticeVertex4D = OpenSimplex2S.LOOKUP_4D_B[i];
			float num22 = num10 + latticeVertex4D.dx;
			float num23 = num11 + latticeVertex4D.dy;
			float num24 = num12 + latticeVertex4D.dz;
			float num25 = num13 + latticeVertex4D.dw;
			float num26 = num22 * num22 + num23 * num23 + (num24 * num24 + num25 * num25);
			if (num26 < 0.8f)
			{
				num26 -= 0.8f;
				num26 *= num26;
				num19 += num26 * num26 * OpenSimplex2S.Grad(seed, num14 + latticeVertex4D.xsvp, num15 + latticeVertex4D.ysvp, num16 + latticeVertex4D.zsvp, num17 + latticeVertex4D.wsvp, num22, num23, num24, num25);
			}
		}
		return num19;
	}

	// Token: 0x0600A98C RID: 43404 RVA: 0x003F21D0 File Offset: 0x003F03D0
	[PublicizedFrom(EAccessModifier.Private)]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float Grad(long seed, long xsvp, long ysvp, float dx, float dy)
	{
		long num = (seed ^ xsvp ^ ysvp) * 6026932503003350773L;
		int num2 = (int)(num ^ (num >> 58)) & 254;
		return OpenSimplex2S.GRADIENTS_2D[num2 | 0] * dx + OpenSimplex2S.GRADIENTS_2D[num2 | 1] * dy;
	}

	// Token: 0x0600A98D RID: 43405 RVA: 0x003F2214 File Offset: 0x003F0414
	[PublicizedFrom(EAccessModifier.Private)]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float Grad(long seed, long xrvp, long yrvp, long zrvp, float dx, float dy, float dz)
	{
		long num = (seed ^ xrvp ^ (yrvp ^ zrvp)) * 6026932503003350773L;
		int num2 = (int)(num ^ (num >> 58)) & 1020;
		return OpenSimplex2S.GRADIENTS_3D[num2 | 0] * dx + OpenSimplex2S.GRADIENTS_3D[num2 | 1] * dy + OpenSimplex2S.GRADIENTS_3D[num2 | 2] * dz;
	}

	// Token: 0x0600A98E RID: 43406 RVA: 0x003F2268 File Offset: 0x003F0468
	[PublicizedFrom(EAccessModifier.Private)]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float Grad(long seed, long xsvp, long ysvp, long zsvp, long wsvp, float dx, float dy, float dz, float dw)
	{
		long num = (seed ^ (xsvp ^ ysvp) ^ (zsvp ^ wsvp)) * 6026932503003350773L;
		int num2 = (int)(num ^ (num >> 57)) & 2044;
		return OpenSimplex2S.GRADIENTS_4D[num2 | 0] * dx + OpenSimplex2S.GRADIENTS_4D[num2 | 1] * dy + (OpenSimplex2S.GRADIENTS_4D[num2 | 2] * dz + OpenSimplex2S.GRADIENTS_4D[num2 | 3] * dw);
	}

	// Token: 0x0600A98F RID: 43407 RVA: 0x003F22CC File Offset: 0x003F04CC
	[PublicizedFrom(EAccessModifier.Private)]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int FastFloor(double x)
	{
		int num = (int)x;
		if (x >= (double)num)
		{
			return num;
		}
		return num - 1;
	}

	// Token: 0x0600A990 RID: 43408 RVA: 0x003F22E8 File Offset: 0x003F04E8
	[PublicizedFrom(EAccessModifier.Private)]
	static OpenSimplex2S()
	{
		float[] array = new float[]
		{
			0.38268343f, 0.9238795f, 0.9238795f, 0.38268343f, 0.9238795f, -0.38268343f, 0.38268343f, -0.9238795f, -0.38268343f, -0.9238795f,
			-0.9238795f, -0.38268343f, -0.9238795f, 0.38268343f, -0.38268343f, 0.9238795f, 0.13052619f, 0.9914449f, 0.6087614f, 0.7933533f,
			0.7933533f, 0.6087614f, 0.9914449f, 0.13052619f, 0.9914449f, -0.13052619f, 0.7933533f, -0.6087614f, 0.6087614f, -0.7933533f,
			0.13052619f, -0.9914449f, -0.13052619f, -0.9914449f, -0.6087614f, -0.7933533f, -0.7933533f, -0.6087614f, -0.9914449f, -0.13052619f,
			-0.9914449f, 0.13052619f, -0.7933533f, 0.6087614f, -0.6087614f, 0.7933533f, -0.13052619f, 0.9914449f
		};
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = (float)((double)array[i] / 0.05481866495625118);
		}
		int j = 0;
		int num = 0;
		while (j < OpenSimplex2S.GRADIENTS_2D.Length)
		{
			if (num == array.Length)
			{
				num = 0;
			}
			OpenSimplex2S.GRADIENTS_2D[j] = array[num];
			j++;
			num++;
		}
		OpenSimplex2S.GRADIENTS_3D = new float[1024];
		float[] array2 = new float[]
		{
			2.2247448f, 2.2247448f, -1f, 0f, 2.2247448f, 2.2247448f, 1f, 0f, 3.0862665f, 1.1721513f,
			0f, 0f, 1.1721513f, 3.0862665f, 0f, 0f, -2.2247448f, 2.2247448f, -1f, 0f,
			-2.2247448f, 2.2247448f, 1f, 0f, -1.1721513f, 3.0862665f, 0f, 0f, -3.0862665f, 1.1721513f,
			0f, 0f, -1f, -2.2247448f, -2.2247448f, 0f, 1f, -2.2247448f, -2.2247448f, 0f,
			0f, -3.0862665f, -1.1721513f, 0f, 0f, -1.1721513f, -3.0862665f, 0f, -1f, -2.2247448f,
			2.2247448f, 0f, 1f, -2.2247448f, 2.2247448f, 0f, 0f, -1.1721513f, 3.0862665f, 0f,
			0f, -3.0862665f, 1.1721513f, 0f, -2.2247448f, -2.2247448f, -1f, 0f, -2.2247448f, -2.2247448f,
			1f, 0f, -3.0862665f, -1.1721513f, 0f, 0f, -1.1721513f, -3.0862665f, 0f, 0f,
			-2.2247448f, -1f, -2.2247448f, 0f, -2.2247448f, 1f, -2.2247448f, 0f, -1.1721513f, 0f,
			-3.0862665f, 0f, -3.0862665f, 0f, -1.1721513f, 0f, -2.2247448f, -1f, 2.2247448f, 0f,
			-2.2247448f, 1f, 2.2247448f, 0f, -3.0862665f, 0f, 1.1721513f, 0f, -1.1721513f, 0f,
			3.0862665f, 0f, -1f, 2.2247448f, -2.2247448f, 0f, 1f, 2.2247448f, -2.2247448f, 0f,
			0f, 1.1721513f, -3.0862665f, 0f, 0f, 3.0862665f, -1.1721513f, 0f, -1f, 2.2247448f,
			2.2247448f, 0f, 1f, 2.2247448f, 2.2247448f, 0f, 0f, 3.0862665f, 1.1721513f, 0f,
			0f, 1.1721513f, 3.0862665f, 0f, 2.2247448f, -2.2247448f, -1f, 0f, 2.2247448f, -2.2247448f,
			1f, 0f, 1.1721513f, -3.0862665f, 0f, 0f, 3.0862665f, -1.1721513f, 0f, 0f,
			2.2247448f, -1f, -2.2247448f, 0f, 2.2247448f, 1f, -2.2247448f, 0f, 3.0862665f, 0f,
			-1.1721513f, 0f, 1.1721513f, 0f, -3.0862665f, 0f, 2.2247448f, -1f, 2.2247448f, 0f,
			2.2247448f, 1f, 2.2247448f, 0f, 1.1721513f, 0f, 3.0862665f, 0f, 3.0862665f, 0f,
			1.1721513f, 0f
		};
		for (int k = 0; k < array2.Length; k++)
		{
			array2[k] = (float)((double)array2[k] / 0.2781926117527186);
		}
		int l = 0;
		int num2 = 0;
		while (l < OpenSimplex2S.GRADIENTS_3D.Length)
		{
			if (num2 == array2.Length)
			{
				num2 = 0;
			}
			OpenSimplex2S.GRADIENTS_3D[l] = array2[num2];
			l++;
			num2++;
		}
		OpenSimplex2S.GRADIENTS_4D = new float[2048];
		float[] array3 = new float[]
		{
			-0.6740059f, -0.32398477f, -0.32398477f, 0.5794685f, -0.7504884f, -0.40046722f, 0.15296486f, 0.502986f, -0.7504884f, 0.15296486f,
			-0.40046722f, 0.502986f, -0.8828162f, 0.08164729f, 0.08164729f, 0.4553054f, -0.4553054f, -0.08164729f, -0.08164729f, 0.8828162f,
			-0.502986f, -0.15296486f, 0.40046722f, 0.7504884f, -0.502986f, 0.40046722f, -0.15296486f, 0.7504884f, -0.5794685f, 0.32398477f,
			0.32398477f, 0.6740059f, -0.6740059f, -0.32398477f, 0.5794685f, -0.32398477f, -0.7504884f, -0.40046722f, 0.502986f, 0.15296486f,
			-0.7504884f, 0.15296486f, 0.502986f, -0.40046722f, -0.8828162f, 0.08164729f, 0.4553054f, 0.08164729f, -0.4553054f, -0.08164729f,
			0.8828162f, -0.08164729f, -0.502986f, -0.15296486f, 0.7504884f, 0.40046722f, -0.502986f, 0.40046722f, 0.7504884f, -0.15296486f,
			-0.5794685f, 0.32398477f, 0.6740059f, 0.32398477f, -0.6740059f, 0.5794685f, -0.32398477f, -0.32398477f, -0.7504884f, 0.502986f,
			-0.40046722f, 0.15296486f, -0.7504884f, 0.502986f, 0.15296486f, -0.40046722f, -0.8828162f, 0.4553054f, 0.08164729f, 0.08164729f,
			-0.4553054f, 0.8828162f, -0.08164729f, -0.08164729f, -0.502986f, 0.7504884f, -0.15296486f, 0.40046722f, -0.502986f, 0.7504884f,
			0.40046722f, -0.15296486f, -0.5794685f, 0.6740059f, 0.32398477f, 0.32398477f, 0.5794685f, -0.6740059f, -0.32398477f, -0.32398477f,
			0.502986f, -0.7504884f, -0.40046722f, 0.15296486f, 0.502986f, -0.7504884f, 0.15296486f, -0.40046722f, 0.4553054f, -0.8828162f,
			0.08164729f, 0.08164729f, 0.8828162f, -0.4553054f, -0.08164729f, -0.08164729f, 0.7504884f, -0.502986f, -0.15296486f, 0.40046722f,
			0.7504884f, -0.502986f, 0.40046722f, -0.15296486f, 0.6740059f, -0.5794685f, 0.32398477f, 0.32398477f, -0.753341f, -0.3796829f,
			-0.3796829f, -0.3796829f, -0.78216845f, -0.43214726f, -0.43214726f, 0.121284805f, -0.78216845f, -0.43214726f, 0.121284805f, -0.43214726f,
			-0.78216845f, 0.121284805f, -0.43214726f, -0.43214726f, -0.85865086f, -0.5086297f, 0.04480237f, 0.04480237f, -0.85865086f, 0.04480237f,
			-0.5086297f, 0.04480237f, -0.85865086f, 0.04480237f, 0.04480237f, -0.5086297f, -0.9982829f, -0.033819415f, -0.033819415f, -0.033819415f,
			-0.3796829f, -0.753341f, -0.3796829f, -0.3796829f, -0.43214726f, -0.78216845f, -0.43214726f, 0.121284805f, -0.43214726f, -0.78216845f,
			0.121284805f, -0.43214726f, 0.121284805f, -0.78216845f, -0.43214726f, -0.43214726f, -0.5086297f, -0.85865086f, 0.04480237f, 0.04480237f,
			0.04480237f, -0.85865086f, -0.5086297f, 0.04480237f, 0.04480237f, -0.85865086f, 0.04480237f, -0.5086297f, -0.033819415f, -0.9982829f,
			-0.033819415f, -0.033819415f, -0.3796829f, -0.3796829f, -0.753341f, -0.3796829f, -0.43214726f, -0.43214726f, -0.78216845f, 0.121284805f,
			-0.43214726f, 0.121284805f, -0.78216845f, -0.43214726f, 0.121284805f, -0.43214726f, -0.78216845f, -0.43214726f, -0.5086297f, 0.04480237f,
			-0.85865086f, 0.04480237f, 0.04480237f, -0.5086297f, -0.85865086f, 0.04480237f, 0.04480237f, 0.04480237f, -0.85865086f, -0.5086297f,
			-0.033819415f, -0.033819415f, -0.9982829f, -0.033819415f, -0.3796829f, -0.3796829f, -0.3796829f, -0.753341f, -0.43214726f, -0.43214726f,
			0.121284805f, -0.78216845f, -0.43214726f, 0.121284805f, -0.43214726f, -0.78216845f, 0.121284805f, -0.43214726f, -0.43214726f, -0.78216845f,
			-0.5086297f, 0.04480237f, 0.04480237f, -0.85865086f, 0.04480237f, -0.5086297f, 0.04480237f, -0.85865086f, 0.04480237f, 0.04480237f,
			-0.5086297f, -0.85865086f, -0.033819415f, -0.033819415f, -0.033819415f, -0.9982829f, -0.32398477f, -0.6740059f, -0.32398477f, 0.5794685f,
			-0.40046722f, -0.7504884f, 0.15296486f, 0.502986f, 0.15296486f, -0.7504884f, -0.40046722f, 0.502986f, 0.08164729f, -0.8828162f,
			0.08164729f, 0.4553054f, -0.08164729f, -0.4553054f, -0.08164729f, 0.8828162f, -0.15296486f, -0.502986f, 0.40046722f, 0.7504884f,
			0.40046722f, -0.502986f, -0.15296486f, 0.7504884f, 0.32398477f, -0.5794685f, 0.32398477f, 0.6740059f, -0.32398477f, -0.32398477f,
			-0.6740059f, 0.5794685f, -0.40046722f, 0.15296486f, -0.7504884f, 0.502986f, 0.15296486f, -0.40046722f, -0.7504884f, 0.502986f,
			0.08164729f, 0.08164729f, -0.8828162f, 0.4553054f, -0.08164729f, -0.08164729f, -0.4553054f, 0.8828162f, -0.15296486f, 0.40046722f,
			-0.502986f, 0.7504884f, 0.40046722f, -0.15296486f, -0.502986f, 0.7504884f, 0.32398477f, 0.32398477f, -0.5794685f, 0.6740059f,
			-0.32398477f, -0.6740059f, 0.5794685f, -0.32398477f, -0.40046722f, -0.7504884f, 0.502986f, 0.15296486f, 0.15296486f, -0.7504884f,
			0.502986f, -0.40046722f, 0.08164729f, -0.8828162f, 0.4553054f, 0.08164729f, -0.08164729f, -0.4553054f, 0.8828162f, -0.08164729f,
			-0.15296486f, -0.502986f, 0.7504884f, 0.40046722f, 0.40046722f, -0.502986f, 0.7504884f, -0.15296486f, 0.32398477f, -0.5794685f,
			0.6740059f, 0.32398477f, -0.32398477f, -0.32398477f, 0.5794685f, -0.6740059f, -0.40046722f, 0.15296486f, 0.502986f, -0.7504884f,
			0.15296486f, -0.40046722f, 0.502986f, -0.7504884f, 0.08164729f, 0.08164729f, 0.4553054f, -0.8828162f, -0.08164729f, -0.08164729f,
			0.8828162f, -0.4553054f, -0.15296486f, 0.40046722f, 0.7504884f, -0.502986f, 0.40046722f, -0.15296486f, 0.7504884f, -0.502986f,
			0.32398477f, 0.32398477f, 0.6740059f, -0.5794685f, -0.32398477f, 0.5794685f, -0.6740059f, -0.32398477f, -0.40046722f, 0.502986f,
			-0.7504884f, 0.15296486f, 0.15296486f, 0.502986f, -0.7504884f, -0.40046722f, 0.08164729f, 0.4553054f, -0.8828162f, 0.08164729f,
			-0.08164729f, 0.8828162f, -0.4553054f, -0.08164729f, -0.15296486f, 0.7504884f, -0.502986f, 0.40046722f, 0.40046722f, 0.7504884f,
			-0.502986f, -0.15296486f, 0.32398477f, 0.6740059f, -0.5794685f, 0.32398477f, -0.32398477f, 0.5794685f, -0.32398477f, -0.6740059f,
			-0.40046722f, 0.502986f, 0.15296486f, -0.7504884f, 0.15296486f, 0.502986f, -0.40046722f, -0.7504884f, 0.08164729f, 0.4553054f,
			0.08164729f, -0.8828162f, -0.08164729f, 0.8828162f, -0.08164729f, -0.4553054f, -0.15296486f, 0.7504884f, 0.40046722f, -0.502986f,
			0.40046722f, 0.7504884f, -0.15296486f, -0.502986f, 0.32398477f, 0.6740059f, 0.32398477f, -0.5794685f, 0.5794685f, -0.32398477f,
			-0.6740059f, -0.32398477f, 0.502986f, -0.40046722f, -0.7504884f, 0.15296486f, 0.502986f, 0.15296486f, -0.7504884f, -0.40046722f,
			0.4553054f, 0.08164729f, -0.8828162f, 0.08164729f, 0.8828162f, -0.08164729f, -0.4553054f, -0.08164729f, 0.7504884f, -0.15296486f,
			-0.502986f, 0.40046722f, 0.7504884f, 0.40046722f, -0.502986f, -0.15296486f, 0.6740059f, 0.32398477f, -0.5794685f, 0.32398477f,
			0.5794685f, -0.32398477f, -0.32398477f, -0.6740059f, 0.502986f, -0.40046722f, 0.15296486f, -0.7504884f, 0.502986f, 0.15296486f,
			-0.40046722f, -0.7504884f, 0.4553054f, 0.08164729f, 0.08164729f, -0.8828162f, 0.8828162f, -0.08164729f, -0.08164729f, -0.4553054f,
			0.7504884f, -0.15296486f, 0.40046722f, -0.502986f, 0.7504884f, 0.40046722f, -0.15296486f, -0.502986f, 0.6740059f, 0.32398477f,
			0.32398477f, -0.5794685f, 0.033819415f, 0.033819415f, 0.033819415f, 0.9982829f, -0.04480237f, -0.04480237f, 0.5086297f, 0.85865086f,
			-0.04480237f, 0.5086297f, -0.04480237f, 0.85865086f, -0.121284805f, 0.43214726f, 0.43214726f, 0.78216845f, 0.5086297f, -0.04480237f,
			-0.04480237f, 0.85865086f, 0.43214726f, -0.121284805f, 0.43214726f, 0.78216845f, 0.43214726f, 0.43214726f, -0.121284805f, 0.78216845f,
			0.3796829f, 0.3796829f, 0.3796829f, 0.753341f, 0.033819415f, 0.033819415f, 0.9982829f, 0.033819415f, -0.04480237f, 0.04480237f,
			0.85865086f, 0.5086297f, -0.04480237f, 0.5086297f, 0.85865086f, -0.04480237f, -0.121284805f, 0.43214726f, 0.78216845f, 0.43214726f,
			0.5086297f, -0.04480237f, 0.85865086f, -0.04480237f, 0.43214726f, -0.121284805f, 0.78216845f, 0.43214726f, 0.43214726f, 0.43214726f,
			0.78216845f, -0.121284805f, 0.3796829f, 0.3796829f, 0.753341f, 0.3796829f, 0.033819415f, 0.9982829f, 0.033819415f, 0.033819415f,
			-0.04480237f, 0.85865086f, -0.04480237f, 0.5086297f, -0.04480237f, 0.85865086f, 0.5086297f, -0.04480237f, -0.121284805f, 0.78216845f,
			0.43214726f, 0.43214726f, 0.5086297f, 0.85865086f, -0.04480237f, -0.04480237f, 0.43214726f, 0.78216845f, -0.121284805f, 0.43214726f,
			0.43214726f, 0.78216845f, 0.43214726f, -0.121284805f, 0.3796829f, 0.753341f, 0.3796829f, 0.3796829f, 0.9982829f, 0.033819415f,
			0.033819415f, 0.033819415f, 0.85865086f, -0.04480237f, -0.04480237f, 0.5086297f, 0.85865086f, -0.04480237f, 0.5086297f, -0.04480237f,
			0.78216845f, -0.121284805f, 0.43214726f, 0.43214726f, 0.85865086f, 0.5086297f, -0.04480237f, -0.04480237f, 0.78216845f, 0.43214726f,
			-0.121284805f, 0.43214726f, 0.78216845f, 0.43214726f, 0.43214726f, -0.121284805f, 0.753341f, 0.3796829f, 0.3796829f, 0.3796829f
		};
		for (int m = 0; m < array3.Length; m++)
		{
			array3[m] = (float)((double)array3[m] / 0.11127401889945551);
		}
		int n = 0;
		int num3 = 0;
		while (n < OpenSimplex2S.GRADIENTS_4D.Length)
		{
			if (num3 == array3.Length)
			{
				num3 = 0;
			}
			OpenSimplex2S.GRADIENTS_4D[n] = array3[num3];
			n++;
			num3++;
		}
		int[][] array4 = new int[][]
		{
			new int[]
			{
				21, 69, 81, 84, 85, 86, 89, 90, 101, 102,
				105, 106, 149, 150, 153, 154, 165, 166, 169, 170
			},
			new int[]
			{
				21, 69, 81, 85, 86, 89, 90, 101, 102, 106,
				149, 150, 154, 166, 170
			},
			new int[]
			{
				1, 5, 17, 21, 65, 69, 81, 85, 86, 90,
				102, 106, 150, 154, 166, 170
			},
			new int[]
			{
				1, 21, 22, 69, 70, 81, 82, 85, 86, 90,
				102, 106, 150, 154, 166, 170, 171
			},
			new int[]
			{
				21, 69, 84, 85, 86, 89, 90, 101, 105, 106,
				149, 153, 154, 169, 170
			},
			new int[]
			{
				5, 21, 69, 85, 86, 89, 90, 101, 102, 105,
				106, 149, 150, 153, 154, 170
			},
			new int[]
			{
				5, 21, 69, 85, 86, 89, 90, 102, 106, 150,
				154, 170
			},
			new int[]
			{
				5, 21, 22, 69, 70, 85, 86, 89, 90, 102,
				106, 150, 154, 170, 171
			},
			new int[]
			{
				4, 5, 20, 21, 68, 69, 84, 85, 89, 90,
				105, 106, 153, 154, 169, 170
			},
			new int[]
			{
				5, 21, 69, 85, 86, 89, 90, 105, 106, 153,
				154, 170
			},
			new int[] { 5, 21, 69, 85, 86, 89, 90, 106, 154, 170 },
			new int[]
			{
				5, 21, 22, 69, 70, 85, 86, 89, 90, 91,
				106, 154, 170, 171
			},
			new int[]
			{
				4, 21, 25, 69, 73, 84, 85, 88, 89, 90,
				105, 106, 153, 154, 169, 170, 174
			},
			new int[]
			{
				5, 21, 25, 69, 73, 85, 86, 89, 90, 105,
				106, 153, 154, 170, 174
			},
			new int[]
			{
				5, 21, 25, 69, 73, 85, 86, 89, 90, 94,
				106, 154, 170, 174
			},
			new int[]
			{
				5, 21, 26, 69, 74, 85, 86, 89, 90, 91,
				94, 106, 154, 170, 171, 174, 175
			},
			new int[]
			{
				21, 81, 84, 85, 86, 89, 101, 102, 105, 106,
				149, 165, 166, 169, 170
			},
			new int[]
			{
				17, 21, 81, 85, 86, 89, 90, 101, 102, 105,
				106, 149, 150, 165, 166, 170
			},
			new int[]
			{
				17, 21, 81, 85, 86, 90, 101, 102, 106, 150,
				166, 170
			},
			new int[]
			{
				17, 21, 22, 81, 82, 85, 86, 90, 101, 102,
				106, 150, 166, 170, 171
			},
			new int[]
			{
				20, 21, 84, 85, 86, 89, 90, 101, 102, 105,
				106, 149, 153, 165, 169, 170
			},
			new int[]
			{
				21, 85, 86, 89, 90, 101, 102, 105, 106, 149,
				154, 166, 169, 170
			},
			new int[]
			{
				21, 85, 86, 89, 90, 101, 102, 105, 106, 150,
				154, 166, 170, 171
			},
			new int[]
			{
				21, 22, 85, 86, 90, 102, 106, 107, 150, 154,
				166, 170, 171
			},
			new int[]
			{
				20, 21, 84, 85, 89, 90, 101, 105, 106, 153,
				169, 170
			},
			new int[]
			{
				21, 85, 86, 89, 90, 101, 102, 105, 106, 153,
				154, 169, 170, 174
			},
			new int[]
			{
				21, 85, 86, 89, 90, 101, 102, 105, 106, 154,
				170
			},
			new int[]
			{
				21, 22, 85, 86, 89, 90, 102, 106, 107, 154,
				170, 171
			},
			new int[]
			{
				20, 21, 25, 84, 85, 88, 89, 90, 101, 105,
				106, 153, 169, 170, 174
			},
			new int[]
			{
				21, 25, 85, 89, 90, 105, 106, 110, 153, 154,
				169, 170, 174
			},
			new int[]
			{
				21, 25, 85, 86, 89, 90, 105, 106, 110, 154,
				170, 174
			},
			new int[]
			{
				21, 26, 85, 86, 89, 90, 106, 107, 110, 154,
				170, 171, 174, 175
			},
			new int[]
			{
				16, 17, 20, 21, 80, 81, 84, 85, 101, 102,
				105, 106, 165, 166, 169, 170
			},
			new int[]
			{
				17, 21, 81, 85, 86, 101, 102, 105, 106, 165,
				166, 170
			},
			new int[] { 17, 21, 81, 85, 86, 101, 102, 106, 166, 170 },
			new int[]
			{
				17, 21, 22, 81, 82, 85, 86, 101, 102, 103,
				106, 166, 170, 171
			},
			new int[]
			{
				20, 21, 84, 85, 89, 101, 102, 105, 106, 165,
				169, 170
			},
			new int[]
			{
				21, 85, 86, 89, 90, 101, 102, 105, 106, 165,
				166, 169, 170, 186
			},
			new int[]
			{
				21, 85, 86, 89, 90, 101, 102, 105, 106, 166,
				170
			},
			new int[]
			{
				21, 22, 85, 86, 90, 101, 102, 106, 107, 166,
				170, 171
			},
			new int[] { 20, 21, 84, 85, 89, 101, 105, 106, 169, 170 },
			new int[]
			{
				21, 85, 86, 89, 90, 101, 102, 105, 106, 169,
				170
			},
			new int[] { 21, 85, 86, 89, 90, 101, 102, 105, 106, 170 },
			new int[]
			{
				21, 22, 85, 86, 89, 90, 101, 102, 105, 106,
				107, 170, 171
			},
			new int[]
			{
				20, 21, 25, 84, 85, 88, 89, 101, 105, 106,
				109, 169, 170, 174
			},
			new int[]
			{
				21, 25, 85, 89, 90, 101, 105, 106, 110, 169,
				170, 174
			},
			new int[]
			{
				21, 25, 85, 86, 89, 90, 101, 102, 105, 106,
				110, 170, 174
			},
			new int[]
			{
				21, 85, 86, 89, 90, 102, 105, 106, 107, 110,
				154, 170, 171, 174, 175
			},
			new int[]
			{
				16, 21, 37, 81, 84, 85, 97, 100, 101, 102,
				105, 106, 165, 166, 169, 170, 186
			},
			new int[]
			{
				17, 21, 37, 81, 85, 86, 97, 101, 102, 105,
				106, 165, 166, 170, 186
			},
			new int[]
			{
				17, 21, 37, 81, 85, 86, 97, 101, 102, 106,
				118, 166, 170, 186
			},
			new int[]
			{
				17, 21, 38, 81, 85, 86, 98, 101, 102, 103,
				106, 118, 166, 170, 171, 186, 187
			},
			new int[]
			{
				20, 21, 37, 84, 85, 89, 100, 101, 102, 105,
				106, 165, 169, 170, 186
			},
			new int[]
			{
				21, 37, 85, 101, 102, 105, 106, 122, 165, 166,
				169, 170, 186
			},
			new int[]
			{
				21, 37, 85, 86, 101, 102, 105, 106, 122, 166,
				170, 186
			},
			new int[]
			{
				21, 38, 85, 86, 101, 102, 106, 107, 122, 166,
				170, 171, 186, 187
			},
			new int[]
			{
				20, 21, 37, 84, 85, 89, 100, 101, 105, 106,
				121, 169, 170, 186
			},
			new int[]
			{
				21, 37, 85, 89, 101, 102, 105, 106, 122, 169,
				170, 186
			},
			new int[]
			{
				21, 37, 85, 86, 89, 90, 101, 102, 105, 106,
				122, 170, 186
			},
			new int[]
			{
				21, 85, 86, 90, 101, 102, 105, 106, 107, 122,
				166, 170, 171, 186, 187
			},
			new int[]
			{
				20, 21, 41, 84, 85, 89, 101, 104, 105, 106,
				109, 121, 169, 170, 174, 186, 190
			},
			new int[]
			{
				21, 41, 85, 89, 101, 105, 106, 110, 122, 169,
				170, 174, 186, 190
			},
			new int[]
			{
				21, 85, 89, 90, 101, 102, 105, 106, 110, 122,
				169, 170, 174, 186, 190
			},
			new int[]
			{
				21, 85, 86, 89, 90, 101, 102, 105, 106, 107,
				110, 122, 170, 171, 174, 186, 191
			},
			new int[]
			{
				69, 81, 84, 85, 86, 89, 101, 149, 150, 153,
				154, 165, 166, 169, 170
			},
			new int[]
			{
				65, 69, 81, 85, 86, 89, 90, 101, 102, 149,
				150, 153, 154, 165, 166, 170
			},
			new int[]
			{
				65, 69, 81, 85, 86, 90, 102, 149, 150, 154,
				166, 170
			},
			new int[]
			{
				65, 69, 70, 81, 82, 85, 86, 90, 102, 149,
				150, 154, 166, 170, 171
			},
			new int[]
			{
				68, 69, 84, 85, 86, 89, 90, 101, 105, 149,
				150, 153, 154, 165, 169, 170
			},
			new int[]
			{
				69, 85, 86, 89, 90, 101, 106, 149, 150, 153,
				154, 166, 169, 170
			},
			new int[]
			{
				69, 85, 86, 89, 90, 102, 106, 149, 150, 153,
				154, 166, 170, 171
			},
			new int[]
			{
				69, 70, 85, 86, 90, 102, 106, 150, 154, 155,
				166, 170, 171
			},
			new int[]
			{
				68, 69, 84, 85, 89, 90, 105, 149, 153, 154,
				169, 170
			},
			new int[]
			{
				69, 85, 86, 89, 90, 105, 106, 149, 150, 153,
				154, 169, 170, 174
			},
			new int[]
			{
				69, 85, 86, 89, 90, 106, 149, 150, 153, 154,
				170
			},
			new int[]
			{
				69, 70, 85, 86, 89, 90, 106, 150, 154, 155,
				170, 171
			},
			new int[]
			{
				68, 69, 73, 84, 85, 88, 89, 90, 105, 149,
				153, 154, 169, 170, 174
			},
			new int[]
			{
				69, 73, 85, 89, 90, 105, 106, 153, 154, 158,
				169, 170, 174
			},
			new int[]
			{
				69, 73, 85, 86, 89, 90, 106, 153, 154, 158,
				170, 174
			},
			new int[]
			{
				69, 74, 85, 86, 89, 90, 106, 154, 155, 158,
				170, 171, 174, 175
			},
			new int[]
			{
				80, 81, 84, 85, 86, 89, 101, 102, 105, 149,
				150, 153, 165, 166, 169, 170
			},
			new int[]
			{
				81, 85, 86, 89, 101, 102, 106, 149, 150, 154,
				165, 166, 169, 170
			},
			new int[]
			{
				81, 85, 86, 90, 101, 102, 106, 149, 150, 154,
				165, 166, 170, 171
			},
			new int[]
			{
				81, 82, 85, 86, 90, 102, 106, 150, 154, 166,
				167, 170, 171
			},
			new int[]
			{
				84, 85, 86, 89, 101, 105, 106, 149, 153, 154,
				165, 166, 169, 170
			},
			new int[]
			{
				85, 86, 89, 90, 101, 102, 105, 106, 149, 150,
				153, 154, 165, 166, 169, 170
			},
			new int[]
			{
				21, 69, 81, 85, 86, 89, 90, 101, 102, 106,
				149, 150, 154, 166, 170, 171
			},
			new int[] { 85, 86, 90, 102, 106, 150, 154, 166, 170, 171 },
			new int[]
			{
				84, 85, 89, 90, 101, 105, 106, 149, 153, 154,
				165, 169, 170, 174
			},
			new int[]
			{
				21, 69, 84, 85, 86, 89, 90, 101, 105, 106,
				149, 153, 154, 169, 170, 174
			},
			new int[]
			{
				21, 69, 85, 86, 89, 90, 101, 102, 105, 106,
				149, 150, 153, 154, 166, 169, 170, 171, 174
			},
			new int[]
			{
				85, 86, 89, 90, 102, 106, 150, 154, 166, 170,
				171
			},
			new int[]
			{
				84, 85, 88, 89, 90, 105, 106, 153, 154, 169,
				170, 173, 174
			},
			new int[] { 85, 89, 90, 105, 106, 153, 154, 169, 170, 174 },
			new int[]
			{
				85, 86, 89, 90, 105, 106, 153, 154, 169, 170,
				174
			},
			new int[] { 85, 86, 89, 90, 106, 154, 170, 171, 174, 175 },
			new int[]
			{
				80, 81, 84, 85, 101, 102, 105, 149, 165, 166,
				169, 170
			},
			new int[]
			{
				81, 85, 86, 101, 102, 105, 106, 149, 150, 165,
				166, 169, 170, 186
			},
			new int[]
			{
				81, 85, 86, 101, 102, 106, 149, 150, 165, 166,
				170
			},
			new int[]
			{
				81, 82, 85, 86, 101, 102, 106, 150, 166, 167,
				170, 171
			},
			new int[]
			{
				84, 85, 89, 101, 102, 105, 106, 149, 153, 165,
				166, 169, 170, 186
			},
			new int[]
			{
				21, 81, 84, 85, 86, 89, 101, 102, 105, 106,
				149, 165, 166, 169, 170, 186
			},
			new int[]
			{
				21, 81, 85, 86, 89, 90, 101, 102, 105, 106,
				149, 150, 154, 165, 166, 169, 170, 171, 186
			},
			new int[]
			{
				85, 86, 90, 101, 102, 106, 150, 154, 166, 170,
				171
			},
			new int[]
			{
				84, 85, 89, 101, 105, 106, 149, 153, 165, 169,
				170
			},
			new int[]
			{
				21, 84, 85, 86, 89, 90, 101, 102, 105, 106,
				149, 153, 154, 165, 166, 169, 170, 174, 186
			},
			new int[]
			{
				21, 85, 86, 89, 90, 101, 102, 105, 106, 154,
				166, 169, 170
			},
			new int[]
			{
				21, 85, 86, 89, 90, 101, 102, 105, 106, 150,
				154, 166, 170, 171
			},
			new int[]
			{
				84, 85, 88, 89, 101, 105, 106, 153, 169, 170,
				173, 174
			},
			new int[]
			{
				85, 89, 90, 101, 105, 106, 153, 154, 169, 170,
				174
			},
			new int[]
			{
				21, 85, 86, 89, 90, 101, 102, 105, 106, 153,
				154, 169, 170, 174
			},
			new int[]
			{
				21, 85, 86, 89, 90, 102, 105, 106, 154, 170,
				171, 174, 175
			},
			new int[]
			{
				80, 81, 84, 85, 97, 100, 101, 102, 105, 149,
				165, 166, 169, 170, 186
			},
			new int[]
			{
				81, 85, 97, 101, 102, 105, 106, 165, 166, 169,
				170, 182, 186
			},
			new int[]
			{
				81, 85, 86, 97, 101, 102, 106, 165, 166, 170,
				182, 186
			},
			new int[]
			{
				81, 85, 86, 98, 101, 102, 106, 166, 167, 170,
				171, 182, 186, 187
			},
			new int[]
			{
				84, 85, 100, 101, 102, 105, 106, 165, 166, 169,
				170, 185, 186
			},
			new int[] { 85, 101, 102, 105, 106, 165, 166, 169, 170, 186 },
			new int[]
			{
				85, 86, 101, 102, 105, 106, 165, 166, 169, 170,
				186
			},
			new int[] { 85, 86, 101, 102, 106, 166, 170, 171, 186, 187 },
			new int[]
			{
				84, 85, 89, 100, 101, 105, 106, 165, 169, 170,
				185, 186
			},
			new int[]
			{
				85, 89, 101, 102, 105, 106, 165, 166, 169, 170,
				186
			},
			new int[]
			{
				21, 85, 86, 89, 90, 101, 102, 105, 106, 165,
				166, 169, 170, 186
			},
			new int[]
			{
				21, 85, 86, 90, 101, 102, 105, 106, 166, 170,
				171, 186, 187
			},
			new int[]
			{
				84, 85, 89, 101, 104, 105, 106, 169, 170, 173,
				174, 185, 186, 190
			},
			new int[] { 85, 89, 101, 105, 106, 169, 170, 174, 186, 190 },
			new int[]
			{
				21, 85, 89, 90, 101, 102, 105, 106, 169, 170,
				174, 186, 190
			},
			new int[]
			{
				85, 86, 89, 90, 101, 102, 105, 106, 170, 171,
				174, 186, 191
			},
			new int[]
			{
				64, 65, 68, 69, 80, 81, 84, 85, 149, 150,
				153, 154, 165, 166, 169, 170
			},
			new int[]
			{
				65, 69, 81, 85, 86, 149, 150, 153, 154, 165,
				166, 170
			},
			new int[] { 65, 69, 81, 85, 86, 149, 150, 154, 166, 170 },
			new int[]
			{
				65, 69, 70, 81, 82, 85, 86, 149, 150, 151,
				154, 166, 170, 171
			},
			new int[]
			{
				68, 69, 84, 85, 89, 149, 150, 153, 154, 165,
				169, 170
			},
			new int[]
			{
				69, 85, 86, 89, 90, 149, 150, 153, 154, 165,
				166, 169, 170, 234
			},
			new int[]
			{
				69, 85, 86, 89, 90, 149, 150, 153, 154, 166,
				170
			},
			new int[]
			{
				69, 70, 85, 86, 90, 149, 150, 154, 155, 166,
				170, 171
			},
			new int[] { 68, 69, 84, 85, 89, 149, 153, 154, 169, 170 },
			new int[]
			{
				69, 85, 86, 89, 90, 149, 150, 153, 154, 169,
				170
			},
			new int[] { 69, 85, 86, 89, 90, 149, 150, 153, 154, 170 },
			new int[]
			{
				69, 70, 85, 86, 89, 90, 149, 150, 153, 154,
				155, 170, 171
			},
			new int[]
			{
				68, 69, 73, 84, 85, 88, 89, 149, 153, 154,
				157, 169, 170, 174
			},
			new int[]
			{
				69, 73, 85, 89, 90, 149, 153, 154, 158, 169,
				170, 174
			},
			new int[]
			{
				69, 73, 85, 86, 89, 90, 149, 150, 153, 154,
				158, 170, 174
			},
			new int[]
			{
				69, 85, 86, 89, 90, 106, 150, 153, 154, 155,
				158, 170, 171, 174, 175
			},
			new int[]
			{
				80, 81, 84, 85, 101, 149, 150, 153, 165, 166,
				169, 170
			},
			new int[]
			{
				81, 85, 86, 101, 102, 149, 150, 153, 154, 165,
				166, 169, 170, 234
			},
			new int[]
			{
				81, 85, 86, 101, 102, 149, 150, 154, 165, 166,
				170
			},
			new int[]
			{
				81, 82, 85, 86, 102, 149, 150, 154, 166, 167,
				170, 171
			},
			new int[]
			{
				84, 85, 89, 101, 105, 149, 150, 153, 154, 165,
				166, 169, 170, 234
			},
			new int[]
			{
				69, 81, 84, 85, 86, 89, 101, 149, 150, 153,
				154, 165, 166, 169, 170, 234
			},
			new int[]
			{
				69, 81, 85, 86, 89, 90, 101, 102, 106, 149,
				150, 153, 154, 165, 166, 169, 170, 171, 234
			},
			new int[]
			{
				85, 86, 90, 102, 106, 149, 150, 154, 166, 170,
				171
			},
			new int[]
			{
				84, 85, 89, 101, 105, 149, 153, 154, 165, 169,
				170
			},
			new int[]
			{
				69, 84, 85, 86, 89, 90, 101, 105, 106, 149,
				150, 153, 154, 165, 166, 169, 170, 174, 234
			},
			new int[]
			{
				69, 85, 86, 89, 90, 106, 149, 150, 153, 154,
				166, 169, 170
			},
			new int[]
			{
				69, 85, 86, 89, 90, 102, 106, 149, 150, 153,
				154, 166, 170, 171
			},
			new int[]
			{
				84, 85, 88, 89, 105, 149, 153, 154, 169, 170,
				173, 174
			},
			new int[]
			{
				85, 89, 90, 105, 106, 149, 153, 154, 169, 170,
				174
			},
			new int[]
			{
				69, 85, 86, 89, 90, 105, 106, 149, 150, 153,
				154, 169, 170, 174
			},
			new int[]
			{
				69, 85, 86, 89, 90, 106, 150, 153, 154, 170,
				171, 174, 175
			},
			new int[] { 80, 81, 84, 85, 101, 149, 165, 166, 169, 170 },
			new int[]
			{
				81, 85, 86, 101, 102, 149, 150, 165, 166, 169,
				170
			},
			new int[] { 81, 85, 86, 101, 102, 149, 150, 165, 166, 170 },
			new int[]
			{
				81, 82, 85, 86, 101, 102, 149, 150, 165, 166,
				167, 170, 171
			},
			new int[]
			{
				84, 85, 89, 101, 105, 149, 153, 165, 166, 169,
				170
			},
			new int[]
			{
				81, 84, 85, 86, 89, 101, 102, 105, 106, 149,
				150, 153, 154, 165, 166, 169, 170, 186, 234
			},
			new int[]
			{
				81, 85, 86, 101, 102, 106, 149, 150, 154, 165,
				166, 169, 170
			},
			new int[]
			{
				81, 85, 86, 90, 101, 102, 106, 149, 150, 154,
				165, 166, 170, 171
			},
			new int[] { 84, 85, 89, 101, 105, 149, 153, 165, 169, 170 },
			new int[]
			{
				84, 85, 89, 101, 105, 106, 149, 153, 154, 165,
				166, 169, 170
			},
			new int[]
			{
				85, 86, 89, 90, 101, 102, 105, 106, 149, 150,
				153, 154, 165, 166, 169, 170
			},
			new int[]
			{
				85, 86, 89, 90, 101, 102, 106, 149, 150, 154,
				166, 169, 170, 171
			},
			new int[]
			{
				84, 85, 88, 89, 101, 105, 149, 153, 165, 169,
				170, 173, 174
			},
			new int[]
			{
				84, 85, 89, 90, 101, 105, 106, 149, 153, 154,
				165, 169, 170, 174
			},
			new int[]
			{
				85, 86, 89, 90, 101, 105, 106, 149, 153, 154,
				166, 169, 170, 174
			},
			new int[]
			{
				85, 86, 89, 90, 102, 105, 106, 150, 153, 154,
				166, 169, 170, 171, 174, 175
			},
			new int[]
			{
				80, 81, 84, 85, 97, 100, 101, 149, 165, 166,
				169, 170, 181, 186
			},
			new int[]
			{
				81, 85, 97, 101, 102, 149, 165, 166, 169, 170,
				182, 186
			},
			new int[]
			{
				81, 85, 86, 97, 101, 102, 149, 150, 165, 166,
				170, 182, 186
			},
			new int[]
			{
				81, 85, 86, 101, 102, 106, 150, 165, 166, 167,
				170, 171, 182, 186, 187
			},
			new int[]
			{
				84, 85, 100, 101, 105, 149, 165, 166, 169, 170,
				185, 186
			},
			new int[]
			{
				85, 101, 102, 105, 106, 149, 165, 166, 169, 170,
				186
			},
			new int[]
			{
				81, 85, 86, 101, 102, 105, 106, 149, 150, 165,
				166, 169, 170, 186
			},
			new int[]
			{
				81, 85, 86, 101, 102, 106, 150, 165, 166, 170,
				171, 186, 187
			},
			new int[]
			{
				84, 85, 89, 100, 101, 105, 149, 153, 165, 169,
				170, 185, 186
			},
			new int[]
			{
				84, 85, 89, 101, 102, 105, 106, 149, 153, 165,
				166, 169, 170, 186
			},
			new int[]
			{
				85, 86, 89, 101, 102, 105, 106, 149, 154, 165,
				166, 169, 170, 186
			},
			new int[]
			{
				85, 86, 90, 101, 102, 105, 106, 150, 154, 165,
				166, 169, 170, 171, 186, 187
			},
			new int[]
			{
				84, 85, 89, 101, 105, 106, 153, 165, 169, 170,
				173, 174, 185, 186, 190
			},
			new int[]
			{
				84, 85, 89, 101, 105, 106, 153, 165, 169, 170,
				174, 186, 190
			},
			new int[]
			{
				85, 89, 90, 101, 102, 105, 106, 153, 154, 165,
				166, 169, 170, 174, 186, 190
			},
			new int[]
			{
				85, 86, 89, 90, 101, 102, 105, 106, 154, 166,
				169, 170, 171, 174, 186
			},
			new int[]
			{
				64, 69, 81, 84, 85, 133, 145, 148, 149, 150,
				153, 154, 165, 166, 169, 170, 234
			},
			new int[]
			{
				65, 69, 81, 85, 86, 133, 145, 149, 150, 153,
				154, 165, 166, 170, 234
			},
			new int[]
			{
				65, 69, 81, 85, 86, 133, 145, 149, 150, 154,
				166, 170, 214, 234
			},
			new int[]
			{
				65, 69, 81, 85, 86, 134, 146, 149, 150, 151,
				154, 166, 170, 171, 214, 234, 235
			},
			new int[]
			{
				68, 69, 84, 85, 89, 133, 148, 149, 150, 153,
				154, 165, 169, 170, 234
			},
			new int[]
			{
				69, 85, 133, 149, 150, 153, 154, 165, 166, 169,
				170, 218, 234
			},
			new int[]
			{
				69, 85, 86, 133, 149, 150, 153, 154, 166, 170,
				218, 234
			},
			new int[]
			{
				69, 85, 86, 134, 149, 150, 154, 155, 166, 170,
				171, 218, 234, 235
			},
			new int[]
			{
				68, 69, 84, 85, 89, 133, 148, 149, 153, 154,
				169, 170, 217, 234
			},
			new int[]
			{
				69, 85, 89, 133, 149, 150, 153, 154, 169, 170,
				218, 234
			},
			new int[]
			{
				69, 85, 86, 89, 90, 133, 149, 150, 153, 154,
				170, 218, 234
			},
			new int[]
			{
				69, 85, 86, 90, 149, 150, 153, 154, 155, 166,
				170, 171, 218, 234, 235
			},
			new int[]
			{
				68, 69, 84, 85, 89, 137, 149, 152, 153, 154,
				157, 169, 170, 174, 217, 234, 238
			},
			new int[]
			{
				69, 85, 89, 137, 149, 153, 154, 158, 169, 170,
				174, 218, 234, 238
			},
			new int[]
			{
				69, 85, 89, 90, 149, 150, 153, 154, 158, 169,
				170, 174, 218, 234, 238
			},
			new int[]
			{
				69, 85, 86, 89, 90, 149, 150, 153, 154, 155,
				158, 170, 171, 174, 218, 234, 239
			},
			new int[]
			{
				80, 81, 84, 85, 101, 145, 148, 149, 150, 153,
				165, 166, 169, 170, 234
			},
			new int[]
			{
				81, 85, 145, 149, 150, 153, 154, 165, 166, 169,
				170, 230, 234
			},
			new int[]
			{
				81, 85, 86, 145, 149, 150, 154, 165, 166, 170,
				230, 234
			},
			new int[]
			{
				81, 85, 86, 146, 149, 150, 154, 166, 167, 170,
				171, 230, 234, 235
			},
			new int[]
			{
				84, 85, 148, 149, 150, 153, 154, 165, 166, 169,
				170, 233, 234
			},
			new int[] { 85, 149, 150, 153, 154, 165, 166, 169, 170, 234 },
			new int[]
			{
				85, 86, 149, 150, 153, 154, 165, 166, 169, 170,
				234
			},
			new int[] { 85, 86, 149, 150, 154, 166, 170, 171, 234, 235 },
			new int[]
			{
				84, 85, 89, 148, 149, 153, 154, 165, 169, 170,
				233, 234
			},
			new int[]
			{
				85, 89, 149, 150, 153, 154, 165, 166, 169, 170,
				234
			},
			new int[]
			{
				69, 85, 86, 89, 90, 149, 150, 153, 154, 165,
				166, 169, 170, 234
			},
			new int[]
			{
				69, 85, 86, 90, 149, 150, 153, 154, 166, 170,
				171, 234, 235
			},
			new int[]
			{
				84, 85, 89, 149, 152, 153, 154, 169, 170, 173,
				174, 233, 234, 238
			},
			new int[] { 85, 89, 149, 153, 154, 169, 170, 174, 234, 238 },
			new int[]
			{
				69, 85, 89, 90, 149, 150, 153, 154, 169, 170,
				174, 234, 238
			},
			new int[]
			{
				85, 86, 89, 90, 149, 150, 153, 154, 170, 171,
				174, 234, 239
			},
			new int[]
			{
				80, 81, 84, 85, 101, 145, 148, 149, 165, 166,
				169, 170, 229, 234
			},
			new int[]
			{
				81, 85, 101, 145, 149, 150, 165, 166, 169, 170,
				230, 234
			},
			new int[]
			{
				81, 85, 86, 101, 102, 145, 149, 150, 165, 166,
				170, 230, 234
			},
			new int[]
			{
				81, 85, 86, 102, 149, 150, 154, 165, 166, 167,
				170, 171, 230, 234, 235
			},
			new int[]
			{
				84, 85, 101, 148, 149, 153, 165, 166, 169, 170,
				233, 234
			},
			new int[]
			{
				85, 101, 149, 150, 153, 154, 165, 166, 169, 170,
				234
			},
			new int[]
			{
				81, 85, 86, 101, 102, 149, 150, 153, 154, 165,
				166, 169, 170, 234
			},
			new int[]
			{
				81, 85, 86, 102, 149, 150, 154, 165, 166, 170,
				171, 234, 235
			},
			new int[]
			{
				84, 85, 89, 101, 105, 148, 149, 153, 165, 169,
				170, 233, 234
			},
			new int[]
			{
				84, 85, 89, 101, 105, 149, 150, 153, 154, 165,
				166, 169, 170, 234
			},
			new int[]
			{
				85, 86, 89, 101, 106, 149, 150, 153, 154, 165,
				166, 169, 170, 234
			},
			new int[]
			{
				85, 86, 90, 102, 106, 149, 150, 153, 154, 165,
				166, 169, 170, 171, 234, 235
			},
			new int[]
			{
				84, 85, 89, 105, 149, 153, 154, 165, 169, 170,
				173, 174, 233, 234, 238
			},
			new int[]
			{
				84, 85, 89, 105, 149, 153, 154, 165, 169, 170,
				174, 234, 238
			},
			new int[]
			{
				85, 89, 90, 105, 106, 149, 150, 153, 154, 165,
				166, 169, 170, 174, 234, 238
			},
			new int[]
			{
				85, 86, 89, 90, 106, 149, 150, 153, 154, 166,
				169, 170, 171, 174, 234
			},
			new int[]
			{
				80, 81, 84, 85, 101, 149, 161, 164, 165, 166,
				169, 170, 181, 186, 229, 234, 250
			},
			new int[]
			{
				81, 85, 101, 149, 161, 165, 166, 169, 170, 182,
				186, 230, 234, 250
			},
			new int[]
			{
				81, 85, 101, 102, 149, 150, 165, 166, 169, 170,
				182, 186, 230, 234, 250
			},
			new int[]
			{
				81, 85, 86, 101, 102, 149, 150, 165, 166, 167,
				170, 171, 182, 186, 230, 234, 251
			},
			new int[]
			{
				84, 85, 101, 149, 164, 165, 166, 169, 170, 185,
				186, 233, 234, 250
			},
			new int[] { 85, 101, 149, 165, 166, 169, 170, 186, 234, 250 },
			new int[]
			{
				81, 85, 101, 102, 149, 150, 165, 166, 169, 170,
				186, 234, 250
			},
			new int[]
			{
				85, 86, 101, 102, 149, 150, 165, 166, 170, 171,
				186, 234, 251
			},
			new int[]
			{
				84, 85, 101, 105, 149, 153, 165, 166, 169, 170,
				185, 186, 233, 234, 250
			},
			new int[]
			{
				84, 85, 101, 105, 149, 153, 165, 166, 169, 170,
				186, 234, 250
			},
			new int[]
			{
				85, 101, 102, 105, 106, 149, 150, 153, 154, 165,
				166, 169, 170, 186, 234, 250
			},
			new int[]
			{
				85, 86, 101, 102, 106, 149, 150, 154, 165, 166,
				169, 170, 171, 186, 234
			},
			new int[]
			{
				84, 85, 89, 101, 105, 149, 153, 165, 169, 170,
				173, 174, 185, 186, 233, 234, 254
			},
			new int[]
			{
				85, 89, 101, 105, 149, 153, 165, 169, 170, 174,
				186, 234, 254
			},
			new int[]
			{
				85, 89, 101, 105, 106, 149, 153, 154, 165, 166,
				169, 170, 174, 186, 234
			},
			new int[]
			{
				85, 86, 89, 90, 101, 102, 105, 106, 149, 150,
				153, 154, 165, 166, 169, 170, 171, 174, 186, 234
			}
		};
		OpenSimplex2S.LatticeVertex4D[] array5 = new OpenSimplex2S.LatticeVertex4D[256];
		for (int num4 = 0; num4 < 256; num4++)
		{
			int num5 = (num4 & 3) - 1;
			int num6 = ((num4 >> 2) & 3) - 1;
			int num7 = ((num4 >> 4) & 3) - 1;
			int num8 = ((num4 >> 6) & 3) - 1;
			array5[num4] = new OpenSimplex2S.LatticeVertex4D(num5, num6, num7, num8);
		}
		int num9 = 0;
		for (int num10 = 0; num10 < 256; num10++)
		{
			num9 += array4[num10].Length;
		}
		OpenSimplex2S.LOOKUP_4D_A = new ValueTuple<short, short>[256];
		OpenSimplex2S.LOOKUP_4D_B = new OpenSimplex2S.LatticeVertex4D[num9];
		int num11 = 0;
		int num12 = 0;
		while (num11 < 256)
		{
			OpenSimplex2S.LOOKUP_4D_A[num11] = new ValueTuple<short, short>((short)num12, (short)(num12 + array4[num11].Length));
			for (int num13 = 0; num13 < array4[num11].Length; num13++)
			{
				OpenSimplex2S.LOOKUP_4D_B[num12++] = array5[array4[num11][num13]];
			}
			num11++;
		}
	}

	// Token: 0x04007D76 RID: 32118
	[PublicizedFrom(EAccessModifier.Private)]
	public const long PRIME_X = 5910200641878280303L;

	// Token: 0x04007D77 RID: 32119
	[PublicizedFrom(EAccessModifier.Private)]
	public const long PRIME_Y = 6452764530575939509L;

	// Token: 0x04007D78 RID: 32120
	[PublicizedFrom(EAccessModifier.Private)]
	public const long PRIME_Z = 6614699811220273867L;

	// Token: 0x04007D79 RID: 32121
	[PublicizedFrom(EAccessModifier.Private)]
	public const long PRIME_W = 6254464313819354443L;

	// Token: 0x04007D7A RID: 32122
	[PublicizedFrom(EAccessModifier.Private)]
	public const long HASH_MULTIPLIER = 6026932503003350773L;

	// Token: 0x04007D7B RID: 32123
	[PublicizedFrom(EAccessModifier.Private)]
	public const long SEED_FLIP_3D = -5968755714895566377L;

	// Token: 0x04007D7C RID: 32124
	[PublicizedFrom(EAccessModifier.Private)]
	public const double ROOT2OVER2 = 0.7071067811865476;

	// Token: 0x04007D7D RID: 32125
	[PublicizedFrom(EAccessModifier.Private)]
	public const double SKEW_2D = 0.366025403784439;

	// Token: 0x04007D7E RID: 32126
	[PublicizedFrom(EAccessModifier.Private)]
	public const double UNSKEW_2D = -0.21132486540518713;

	// Token: 0x04007D7F RID: 32127
	[PublicizedFrom(EAccessModifier.Private)]
	public const double ROOT3OVER3 = 0.577350269189626;

	// Token: 0x04007D80 RID: 32128
	[PublicizedFrom(EAccessModifier.Private)]
	public const double FALLBACK_ROTATE3 = 0.6666666666666666;

	// Token: 0x04007D81 RID: 32129
	[PublicizedFrom(EAccessModifier.Private)]
	public const double ROTATE3_ORTHOGONALIZER = -0.21132486540518713;

	// Token: 0x04007D82 RID: 32130
	[PublicizedFrom(EAccessModifier.Private)]
	public const float SKEW_4D = 0.309017f;

	// Token: 0x04007D83 RID: 32131
	[PublicizedFrom(EAccessModifier.Private)]
	public const float UNSKEW_4D = -0.1381966f;

	// Token: 0x04007D84 RID: 32132
	[PublicizedFrom(EAccessModifier.Private)]
	public const int N_GRADS_2D_EXPONENT = 7;

	// Token: 0x04007D85 RID: 32133
	[PublicizedFrom(EAccessModifier.Private)]
	public const int N_GRADS_3D_EXPONENT = 8;

	// Token: 0x04007D86 RID: 32134
	[PublicizedFrom(EAccessModifier.Private)]
	public const int N_GRADS_4D_EXPONENT = 9;

	// Token: 0x04007D87 RID: 32135
	[PublicizedFrom(EAccessModifier.Private)]
	public const int N_GRADS_2D = 128;

	// Token: 0x04007D88 RID: 32136
	[PublicizedFrom(EAccessModifier.Private)]
	public const int N_GRADS_3D = 256;

	// Token: 0x04007D89 RID: 32137
	[PublicizedFrom(EAccessModifier.Private)]
	public const int N_GRADS_4D = 512;

	// Token: 0x04007D8A RID: 32138
	[PublicizedFrom(EAccessModifier.Private)]
	public const double NORMALIZER_2D = 0.05481866495625118;

	// Token: 0x04007D8B RID: 32139
	[PublicizedFrom(EAccessModifier.Private)]
	public const double NORMALIZER_3D = 0.2781926117527186;

	// Token: 0x04007D8C RID: 32140
	[PublicizedFrom(EAccessModifier.Private)]
	public const double NORMALIZER_4D = 0.11127401889945551;

	// Token: 0x04007D8D RID: 32141
	[PublicizedFrom(EAccessModifier.Private)]
	public const float RSQUARED_2D = 0.6666667f;

	// Token: 0x04007D8E RID: 32142
	[PublicizedFrom(EAccessModifier.Private)]
	public const float RSQUARED_3D = 0.75f;

	// Token: 0x04007D8F RID: 32143
	[PublicizedFrom(EAccessModifier.Private)]
	public const float RSQUARED_4D = 0.8f;

	// Token: 0x04007D90 RID: 32144
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly float[] GRADIENTS_2D = new float[256];

	// Token: 0x04007D91 RID: 32145
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly float[] GRADIENTS_3D;

	// Token: 0x04007D92 RID: 32146
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly float[] GRADIENTS_4D;

	// Token: 0x04007D93 RID: 32147
	[TupleElementNames(new string[] { "SecondaryIndexStart", "SecondaryIndexStop" })]
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ValueTuple<short, short>[] LOOKUP_4D_A;

	// Token: 0x04007D94 RID: 32148
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly OpenSimplex2S.LatticeVertex4D[] LOOKUP_4D_B;

	// Token: 0x0200150E RID: 5390
	[PublicizedFrom(EAccessModifier.Private)]
	public class LatticeVertex4D
	{
		// Token: 0x0600A991 RID: 43409 RVA: 0x003F3CEC File Offset: 0x003F1EEC
		public LatticeVertex4D(int xsv, int ysv, int zsv, int wsv)
		{
			this.xsvp = (long)xsv * 5910200641878280303L;
			this.ysvp = (long)ysv * 6452764530575939509L;
			this.zsvp = (long)zsv * 6614699811220273867L;
			this.wsvp = (long)wsv * 6254464313819354443L;
			float num = (float)(xsv + ysv + zsv + wsv) * -0.1381966f;
			this.dx = (float)(-(float)xsv) - num;
			this.dy = (float)(-(float)ysv) - num;
			this.dz = (float)(-(float)zsv) - num;
			this.dw = (float)(-(float)wsv) - num;
		}

		// Token: 0x04007D95 RID: 32149
		public readonly float dx;

		// Token: 0x04007D96 RID: 32150
		public readonly float dy;

		// Token: 0x04007D97 RID: 32151
		public readonly float dz;

		// Token: 0x04007D98 RID: 32152
		public readonly float dw;

		// Token: 0x04007D99 RID: 32153
		public readonly long xsvp;

		// Token: 0x04007D9A RID: 32154
		public readonly long ysvp;

		// Token: 0x04007D9B RID: 32155
		public readonly long zsvp;

		// Token: 0x04007D9C RID: 32156
		public readonly long wsvp;
	}
}
