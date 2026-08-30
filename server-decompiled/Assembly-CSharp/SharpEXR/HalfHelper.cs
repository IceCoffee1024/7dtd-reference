using System;
using System.Runtime.InteropServices;

namespace SharpEXR
{
	// Token: 0x020016D1 RID: 5841
	[ComVisible(false)]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static class HalfHelper
	{
		// Token: 0x0600B6EB RID: 46827 RVA: 0x0043DF68 File Offset: 0x0043C168
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

		// Token: 0x0600B6EC RID: 46828 RVA: 0x0043DFA8 File Offset: 0x0043C1A8
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

		// Token: 0x0600B6ED RID: 46829 RVA: 0x0043E008 File Offset: 0x0043C208
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

		// Token: 0x0600B6EE RID: 46830 RVA: 0x0043E074 File Offset: 0x0043C274
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

		// Token: 0x0600B6EF RID: 46831 RVA: 0x0043E0C0 File Offset: 0x0043C2C0
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

		// Token: 0x0600B6F0 RID: 46832 RVA: 0x0043E1AC File Offset: 0x0043C3AC
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

		// Token: 0x0600B6F1 RID: 46833 RVA: 0x0043E25C File Offset: 0x0043C45C
		public unsafe static float HalfToSingle(Half half)
		{
			uint num = HalfHelper.mantissaTable[(int)(HalfHelper.offsetTable[half.value >> 10] + (half.value & 1023))] + HalfHelper.exponentTable[half.value >> 10];
			return *(float*)(&num);
		}

		// Token: 0x0600B6F2 RID: 46834 RVA: 0x0043E2A0 File Offset: 0x0043C4A0
		public unsafe static Half SingleToHalf(float single)
		{
			uint num = *(uint*)(&single);
			return Half.ToHalf((ushort)((uint)HalfHelper.baseTable[(int)((num >> 23) & 511U)] + ((num & 8388607U) >> (int)HalfHelper.shiftTable[(int)(num >> 23)])));
		}

		// Token: 0x0600B6F3 RID: 46835 RVA: 0x0043E2DE File Offset: 0x0043C4DE
		public static Half Negate(Half half)
		{
			return Half.ToHalf(half.value ^ 32768);
		}

		// Token: 0x0600B6F4 RID: 46836 RVA: 0x0043E2F2 File Offset: 0x0043C4F2
		public static Half Abs(Half half)
		{
			return Half.ToHalf(half.value & 32767);
		}

		// Token: 0x0600B6F5 RID: 46837 RVA: 0x0043E306 File Offset: 0x0043C506
		public static bool IsNaN(Half half)
		{
			return (half.value & 32767) > 31744;
		}

		// Token: 0x0600B6F6 RID: 46838 RVA: 0x0043E31B File Offset: 0x0043C51B
		public static bool IsInfinity(Half half)
		{
			return (half.value & 32767) == 31744;
		}

		// Token: 0x0600B6F7 RID: 46839 RVA: 0x0043E330 File Offset: 0x0043C530
		public static bool IsPositiveInfinity(Half half)
		{
			return half.value == 31744;
		}

		// Token: 0x0600B6F8 RID: 46840 RVA: 0x0043E33F File Offset: 0x0043C53F
		public static bool IsNegativeInfinity(Half half)
		{
			return half.value == 64512;
		}

		// Token: 0x04008884 RID: 34948
		[PublicizedFrom(EAccessModifier.Private)]
		public static uint[] mantissaTable = HalfHelper.GenerateMantissaTable();

		// Token: 0x04008885 RID: 34949
		[PublicizedFrom(EAccessModifier.Private)]
		public static uint[] exponentTable = HalfHelper.GenerateExponentTable();

		// Token: 0x04008886 RID: 34950
		[PublicizedFrom(EAccessModifier.Private)]
		public static ushort[] offsetTable = HalfHelper.GenerateOffsetTable();

		// Token: 0x04008887 RID: 34951
		[PublicizedFrom(EAccessModifier.Private)]
		public static ushort[] baseTable = HalfHelper.GenerateBaseTable();

		// Token: 0x04008888 RID: 34952
		[PublicizedFrom(EAccessModifier.Private)]
		public static sbyte[] shiftTable = HalfHelper.GenerateShiftTable();
	}
}
