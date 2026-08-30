using System;

namespace SharpEXR.ColorSpace
{
	// Token: 0x020016DB RID: 5851
	public static class Gamma
	{
		// Token: 0x0600B703 RID: 46851 RVA: 0x0043E4B9 File Offset: 0x0043C6B9
		public static float Expand(float nonlinear)
		{
			return (float)Math.Pow((double)nonlinear, 2.2);
		}

		// Token: 0x0600B704 RID: 46852 RVA: 0x0043E4CC File Offset: 0x0043C6CC
		public static float Compress(float linear)
		{
			return (float)Math.Pow((double)linear, 0.45454545454545453);
		}

		// Token: 0x0600B705 RID: 46853 RVA: 0x0043E4DF File Offset: 0x0043C6DF
		public static void Expand(ref tVec3 pColor)
		{
			pColor.X = Gamma.Expand(pColor.X);
			pColor.Y = Gamma.Expand(pColor.Y);
			pColor.Z = Gamma.Expand(pColor.Z);
		}

		// Token: 0x0600B706 RID: 46854 RVA: 0x0043E514 File Offset: 0x0043C714
		public static void Compress(ref tVec3 pColor)
		{
			pColor.X = Gamma.Compress(pColor.X);
			pColor.Y = Gamma.Compress(pColor.Y);
			pColor.Z = Gamma.Compress(pColor.Z);
		}

		// Token: 0x0600B707 RID: 46855 RVA: 0x0043E549 File Offset: 0x0043C749
		public static void Expand(ref float r, ref float g, ref float b)
		{
			r = Gamma.Expand(r);
			g = Gamma.Expand(g);
			b = Gamma.Expand(b);
		}

		// Token: 0x0600B708 RID: 46856 RVA: 0x0043E566 File Offset: 0x0043C766
		public static void Compress(ref float r, ref float g, ref float b)
		{
			r = Gamma.Compress(r);
			g = Gamma.Compress(g);
			b = Gamma.Compress(b);
		}

		// Token: 0x0600B709 RID: 46857 RVA: 0x0043E584 File Offset: 0x0043C784
		public static tVec3 Expand(float r, float g, float b)
		{
			tVec3 tVec = new tVec3(r, g, b);
			Gamma.Expand(ref tVec);
			return tVec;
		}

		// Token: 0x0600B70A RID: 46858 RVA: 0x0043E5A4 File Offset: 0x0043C7A4
		public static tVec3 Compress(float r, float g, float b)
		{
			tVec3 tVec = new tVec3(r, g, b);
			Gamma.Compress(ref tVec);
			return tVec;
		}

		// Token: 0x0600B70B RID: 46859 RVA: 0x0043E5C3 File Offset: 0x0043C7C3
		public static float Expand_sRGB(float nonlinear)
		{
			if (nonlinear > 0.04045f)
			{
				return (float)Math.Pow((double)((nonlinear + 0.055f) / 1.055f), 2.4000000953674316);
			}
			return nonlinear / 12.92f;
		}

		// Token: 0x0600B70C RID: 46860 RVA: 0x0043E5F2 File Offset: 0x0043C7F2
		public static float Compress_sRGB(float linear)
		{
			if (linear > 0.0031308f)
			{
				return 1.055f * (float)Math.Pow((double)linear, 0.4166666567325592) - 0.055f;
			}
			return 12.92f * linear;
		}

		// Token: 0x0600B70D RID: 46861 RVA: 0x0043E621 File Offset: 0x0043C821
		public static void Expand_sRGB(ref tVec3 pColor)
		{
			pColor.X = Gamma.Expand_sRGB(pColor.X);
			pColor.Y = Gamma.Expand_sRGB(pColor.Y);
			pColor.Z = Gamma.Expand_sRGB(pColor.Z);
		}

		// Token: 0x0600B70E RID: 46862 RVA: 0x0043E656 File Offset: 0x0043C856
		public static void Compress_sRGB(ref tVec3 pColor)
		{
			pColor.X = Gamma.Compress_sRGB(pColor.X);
			pColor.Y = Gamma.Compress_sRGB(pColor.Y);
			pColor.Z = Gamma.Compress_sRGB(pColor.Z);
		}

		// Token: 0x0600B70F RID: 46863 RVA: 0x0043E68B File Offset: 0x0043C88B
		public static void Expand_sRGB(ref float r, ref float g, ref float b)
		{
			r = Gamma.Expand_sRGB(r);
			g = Gamma.Expand_sRGB(g);
			b = Gamma.Expand_sRGB(b);
		}

		// Token: 0x0600B710 RID: 46864 RVA: 0x0043E6A8 File Offset: 0x0043C8A8
		public static void Compress_sRGB(ref float r, ref float g, ref float b)
		{
			r = Gamma.Compress_sRGB(r);
			g = Gamma.Compress_sRGB(g);
			b = Gamma.Compress_sRGB(b);
		}

		// Token: 0x0600B711 RID: 46865 RVA: 0x0043E6C8 File Offset: 0x0043C8C8
		public static tVec3 Expand_sRGB(float r, float g, float b)
		{
			tVec3 tVec = new tVec3(r, g, b);
			Gamma.Expand_sRGB(ref tVec);
			return tVec;
		}

		// Token: 0x0600B712 RID: 46866 RVA: 0x0043E6E8 File Offset: 0x0043C8E8
		public static tVec3 Compress_sRGB(float r, float g, float b)
		{
			tVec3 tVec = new tVec3(r, g, b);
			Gamma.Compress_sRGB(ref tVec);
			return tVec;
		}
	}
}
