using System;
using System.Runtime.InteropServices;

namespace SharpEXR
{
	// Token: 0x020016AB RID: 5803
	[ComVisible(false)]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static class HalfHelper
	{
		// Token: 0x0600B568 RID: 46440 RVA: 0x004381F0 File Offset: 0x004363F0
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

		// Token: 0x0600B569 RID: 46441 RVA: 0x00438230 File Offset: 0x00436430
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

		// Token: 0x0600B56A RID: 46442 RVA: 0x00438290 File Offset: 0x00436490
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

		// Token: 0x0600B56B RID: 46443 RVA: 0x004382FC File Offset: 0x004364FC
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

		// Token: 0x0600B56C RID: 46444 RVA: 0x00438348 File Offset: 0x00436548
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

		// Token: 0x0600B56D RID: 46445 RVA: 0x00438434 File Offset: 0x00436634
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

		// Token: 0x0600B56E RID: 46446 RVA: 0x004384E4 File Offset: 0x004366E4
		public unsafe static float HalfToSingle(Half half)
		{
			uint num = HalfHelper.mantissaTable[(int)(HalfHelper.offsetTable[half.value >> 10] + (half.value & 1023))] + HalfHelper.exponentTable[half.value >> 10];
			return *(float*)(&num);
		}

		// Token: 0x0600B56F RID: 46447 RVA: 0x00438528 File Offset: 0x00436728
		public unsafe static Half SingleToHalf(float single)
		{
			uint num = *(uint*)(&single);
			return Half.ToHalf((ushort)((uint)HalfHelper.baseTable[(int)((num >> 23) & 511U)] + ((num & 8388607U) >> (int)HalfHelper.shiftTable[(int)(num >> 23)])));
		}

		// Token: 0x0600B570 RID: 46448 RVA: 0x00438566 File Offset: 0x00436766
		public static Half Negate(Half half)
		{
			return Half.ToHalf(half.value ^ 32768);
		}

		// Token: 0x0600B571 RID: 46449 RVA: 0x0043857A File Offset: 0x0043677A
		public static Half Abs(Half half)
		{
			return Half.ToHalf(half.value & 32767);
		}

		// Token: 0x0600B572 RID: 46450 RVA: 0x0043858E File Offset: 0x0043678E
		public static bool IsNaN(Half half)
		{
			return (half.value & 32767) > 31744;
		}

		// Token: 0x0600B573 RID: 46451 RVA: 0x004385A3 File Offset: 0x004367A3
		public static bool IsInfinity(Half half)
		{
			return (half.value & 32767) == 31744;
		}

		// Token: 0x0600B574 RID: 46452 RVA: 0x004385B8 File Offset: 0x004367B8
		public static bool IsPositiveInfinity(Half half)
		{
			return half.value == 31744;
		}

		// Token: 0x0600B575 RID: 46453 RVA: 0x004385C7 File Offset: 0x004367C7
		public static bool IsNegativeInfinity(Half half)
		{
			return half.value == 64512;
		}

		// Token: 0x04008768 RID: 34664
		[PublicizedFrom(EAccessModifier.Private)]
		public static uint[] mantissaTable = HalfHelper.GenerateMantissaTable();

		// Token: 0x04008769 RID: 34665
		[PublicizedFrom(EAccessModifier.Private)]
		public static uint[] exponentTable = HalfHelper.GenerateExponentTable();

		// Token: 0x0400876A RID: 34666
		[PublicizedFrom(EAccessModifier.Private)]
		public static ushort[] offsetTable = HalfHelper.GenerateOffsetTable();

		// Token: 0x0400876B RID: 34667
		[PublicizedFrom(EAccessModifier.Private)]
		public static ushort[] baseTable = HalfHelper.GenerateBaseTable();

		// Token: 0x0400876C RID: 34668
		[PublicizedFrom(EAccessModifier.Private)]
		public static sbyte[] shiftTable = HalfHelper.GenerateShiftTable();
	}
}
