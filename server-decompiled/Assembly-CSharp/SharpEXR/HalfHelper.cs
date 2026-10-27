using System;
using System.Runtime.InteropServices;

namespace SharpEXR
{
	// Token: 0x0200171C RID: 5916
	[ComVisible(false)]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static class HalfHelper
	{
		// Token: 0x0600B916 RID: 47382 RVA: 0x00444F2C File Offset: 0x0044312C
		[PublicizedFrom(EAccessModifier.Private)]
		public static uint ConvertMantissa(int i)
		{
			uint num = (uint)((uint)i << 13);
			uint num2 = 0U;
			while ((num & 8388608U) == 0U)
			{
				num2 -= 8388608U;
				num <<= 1;
			}
			num &= 4286578687U;
			num2 += 947912704U;
			return num | num2;
		}

		// Token: 0x0600B917 RID: 47383 RVA: 0x00444F6C File Offset: 0x0044316C
		[PublicizedFrom(EAccessModifier.Private)]
		public static uint[] GenerateMantissaTable()
		{
			uint[] array = new uint[2048];
			array[0] = 0U;
			for (int i = 1; i < 1024; i++)
			{
				array[i] = HalfHelper.ConvertMantissa(i);
			}
			for (int j = 1024; j < 2048; j++)
			{
				array[j] = (uint)(939524096 + (j - 1024 << 13));
			}
			return array;
		}

		// Token: 0x0600B918 RID: 47384 RVA: 0x00444FCC File Offset: 0x004431CC
		[PublicizedFrom(EAccessModifier.Private)]
		public static uint[] GenerateExponentTable()
		{
			uint[] array = new uint[64];
			array[0] = 0U;
			for (int i = 1; i < 31; i++)
			{
				array[i] = (uint)((uint)i << 23);
			}
			array[31] = 1199570944U;
			array[32] = 2147483648U;
			for (int j = 33; j < 63; j++)
			{
				array[j] = (uint)((ulong)int.MinValue + (ulong)((long)((long)(j - 32) << 23)));
			}
			array[63] = 3347054592U;
			return array;
		}

		// Token: 0x0600B919 RID: 47385 RVA: 0x00445038 File Offset: 0x00443238
		[PublicizedFrom(EAccessModifier.Private)]
		public static ushort[] GenerateOffsetTable()
		{
			ushort[] array = new ushort[64];
			array[0] = 0;
			for (int i = 1; i < 32; i++)
			{
				array[i] = 1024;
			}
			array[32] = 0;
			for (int j = 33; j < 64; j++)
			{
				array[j] = 1024;
			}
			return array;
		}

		// Token: 0x0600B91A RID: 47386 RVA: 0x00445084 File Offset: 0x00443284
		[PublicizedFrom(EAccessModifier.Private)]
		public static ushort[] GenerateBaseTable()
		{
			ushort[] array = new ushort[512];
			for (int i = 0; i < 256; i++)
			{
				sbyte b = (sbyte)(127 - i);
				if (b > 24)
				{
					array[i | 0] = 0;
					array[i | 256] = 32768;
				}
				else if (b > 14)
				{
					array[i | 0] = (ushort)(1024 >> (int)(18 + b));
					array[i | 256] = (ushort)((1024 >> (int)(18 + b)) | 32768);
				}
				else if (b >= -15)
				{
					array[i | 0] = (ushort)(15 - b << 10);
					array[i | 256] = (ushort)(((int)(15 - b) << 10) | 32768);
				}
				else if (b > -128)
				{
					array[i | 0] = 31744;
					array[i | 256] = 64512;
				}
				else
				{
					array[i | 0] = 31744;
					array[i | 256] = 64512;
				}
			}
			return array;
		}

		// Token: 0x0600B91B RID: 47387 RVA: 0x00445170 File Offset: 0x00443370
		[PublicizedFrom(EAccessModifier.Private)]
		public static sbyte[] GenerateShiftTable()
		{
			sbyte[] array = new sbyte[512];
			for (int i = 0; i < 256; i++)
			{
				sbyte b = (sbyte)(127 - i);
				if (b > 24)
				{
					array[i | 0] = 24;
					array[i | 256] = 24;
				}
				else if (b > 14)
				{
					array[i | 0] = b - 1;
					array[i | 256] = b - 1;
				}
				else if (b >= -15)
				{
					array[i | 0] = 13;
					array[i | 256] = 13;
				}
				else if (b > -128)
				{
					array[i | 0] = 24;
					array[i | 256] = 24;
				}
				else
				{
					array[i | 0] = 13;
					array[i | 256] = 13;
				}
			}
			return array;
		}

		// Token: 0x0600B91C RID: 47388 RVA: 0x00445220 File Offset: 0x00443420
		public unsafe static float HalfToSingle(Half half)
		{
			uint num = HalfHelper.mantissaTable[(int)(HalfHelper.offsetTable[half.value >> 10] + (half.value & 1023))] + HalfHelper.exponentTable[half.value >> 10];
			return *(float*)(&num);
		}

		// Token: 0x0600B91D RID: 47389 RVA: 0x00445264 File Offset: 0x00443464
		public unsafe static Half SingleToHalf(float single)
		{
			uint num = *(uint*)(&single);
			return Half.ToHalf((ushort)((uint)HalfHelper.baseTable[(int)((num >> 23) & 511U)] + ((num & 8388607U) >> (int)HalfHelper.shiftTable[(int)(num >> 23)])));
		}

		// Token: 0x0600B91E RID: 47390 RVA: 0x004452A2 File Offset: 0x004434A2
		public static Half Negate(Half half)
		{
			return Half.ToHalf(half.value ^ 32768);
		}

		// Token: 0x0600B91F RID: 47391 RVA: 0x004452B6 File Offset: 0x004434B6
		public static Half Abs(Half half)
		{
			return Half.ToHalf(half.value & 32767);
		}

		// Token: 0x0600B920 RID: 47392 RVA: 0x004452CA File Offset: 0x004434CA
		public static bool IsNaN(Half half)
		{
			return (half.value & 32767) > 31744;
		}

		// Token: 0x0600B921 RID: 47393 RVA: 0x004452DF File Offset: 0x004434DF
		public static bool IsInfinity(Half half)
		{
			return (half.value & 32767) == 31744;
		}

		// Token: 0x0600B922 RID: 47394 RVA: 0x004452F4 File Offset: 0x004434F4
		public static bool IsPositiveInfinity(Half half)
		{
			return half.value == 31744;
		}

		// Token: 0x0600B923 RID: 47395 RVA: 0x00445303 File Offset: 0x00443503
		public static bool IsNegativeInfinity(Half half)
		{
			return half.value == 64512;
		}

		// Token: 0x040089FC RID: 35324
		[PublicizedFrom(EAccessModifier.Private)]
		public static uint[] mantissaTable = HalfHelper.GenerateMantissaTable();

		// Token: 0x040089FD RID: 35325
		[PublicizedFrom(EAccessModifier.Private)]
		public static uint[] exponentTable = HalfHelper.GenerateExponentTable();

		// Token: 0x040089FE RID: 35326
		[PublicizedFrom(EAccessModifier.Private)]
		public static ushort[] offsetTable = HalfHelper.GenerateOffsetTable();

		// Token: 0x040089FF RID: 35327
		[PublicizedFrom(EAccessModifier.Private)]
		public static ushort[] baseTable = HalfHelper.GenerateBaseTable();

		// Token: 0x04008A00 RID: 35328
		[PublicizedFrom(EAccessModifier.Private)]
		public static sbyte[] shiftTable = HalfHelper.GenerateShiftTable();
	}
}
