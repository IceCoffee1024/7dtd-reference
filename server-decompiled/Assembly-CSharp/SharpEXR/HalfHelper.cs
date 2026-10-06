using System;
using System.Runtime.InteropServices;

namespace SharpEXR
{
	// Token: 0x020016FD RID: 5885
	[ComVisible(false)]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static class HalfHelper
	{
		// Token: 0x0600B842 RID: 47170 RVA: 0x00442488 File Offset: 0x00440688
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

		// Token: 0x0600B843 RID: 47171 RVA: 0x004424C8 File Offset: 0x004406C8
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

		// Token: 0x0600B844 RID: 47172 RVA: 0x00442528 File Offset: 0x00440728
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

		// Token: 0x0600B845 RID: 47173 RVA: 0x00442594 File Offset: 0x00440794
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

		// Token: 0x0600B846 RID: 47174 RVA: 0x004425E0 File Offset: 0x004407E0
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

		// Token: 0x0600B847 RID: 47175 RVA: 0x004426CC File Offset: 0x004408CC
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

		// Token: 0x0600B848 RID: 47176 RVA: 0x0044277C File Offset: 0x0044097C
		public unsafe static float HalfToSingle(Half half)
		{
			uint num = HalfHelper.mantissaTable[(int)(HalfHelper.offsetTable[half.value >> 10] + (half.value & 1023))] + HalfHelper.exponentTable[half.value >> 10];
			return *(float*)(&num);
		}

		// Token: 0x0600B849 RID: 47177 RVA: 0x004427C0 File Offset: 0x004409C0
		public unsafe static Half SingleToHalf(float single)
		{
			uint num = *(uint*)(&single);
			return Half.ToHalf((ushort)((uint)HalfHelper.baseTable[(int)((num >> 23) & 511U)] + ((num & 8388607U) >> (int)HalfHelper.shiftTable[(int)(num >> 23)])));
		}

		// Token: 0x0600B84A RID: 47178 RVA: 0x004427FE File Offset: 0x004409FE
		public static Half Negate(Half half)
		{
			return Half.ToHalf(half.value ^ 32768);
		}

		// Token: 0x0600B84B RID: 47179 RVA: 0x00442812 File Offset: 0x00440A12
		public static Half Abs(Half half)
		{
			return Half.ToHalf(half.value & 32767);
		}

		// Token: 0x0600B84C RID: 47180 RVA: 0x00442826 File Offset: 0x00440A26
		public static bool IsNaN(Half half)
		{
			return (half.value & 32767) > 31744;
		}

		// Token: 0x0600B84D RID: 47181 RVA: 0x0044283B File Offset: 0x00440A3B
		public static bool IsInfinity(Half half)
		{
			return (half.value & 32767) == 31744;
		}

		// Token: 0x0600B84E RID: 47182 RVA: 0x00442850 File Offset: 0x00440A50
		public static bool IsPositiveInfinity(Half half)
		{
			return half.value == 31744;
		}

		// Token: 0x0600B84F RID: 47183 RVA: 0x0044285F File Offset: 0x00440A5F
		public static bool IsNegativeInfinity(Half half)
		{
			return half.value == 64512;
		}

		// Token: 0x04008949 RID: 35145
		[PublicizedFrom(EAccessModifier.Private)]
		public static uint[] mantissaTable = HalfHelper.GenerateMantissaTable();

		// Token: 0x0400894A RID: 35146
		[PublicizedFrom(EAccessModifier.Private)]
		public static uint[] exponentTable = HalfHelper.GenerateExponentTable();

		// Token: 0x0400894B RID: 35147
		[PublicizedFrom(EAccessModifier.Private)]
		public static ushort[] offsetTable = HalfHelper.GenerateOffsetTable();

		// Token: 0x0400894C RID: 35148
		[PublicizedFrom(EAccessModifier.Private)]
		public static ushort[] baseTable = HalfHelper.GenerateBaseTable();

		// Token: 0x0400894D RID: 35149
		[PublicizedFrom(EAccessModifier.Private)]
		public static sbyte[] shiftTable = HalfHelper.GenerateShiftTable();
	}
}
