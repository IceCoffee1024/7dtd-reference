using System;

namespace SharpEXR.ColorSpace
{
	// Token: 0x02001726 RID: 5926
	public static class Gamma
	{
		// Token: 0x0600B92E RID: 47406 RVA: 0x0044547D File Offset: 0x0044367D
		public static float Expand(float nonlinear)
		{
			return (float)Math.Pow((double)nonlinear, 2.2);
		}

		// Token: 0x0600B92F RID: 47407 RVA: 0x00445490 File Offset: 0x00443690
		public static float Compress(float linear)
		{
			return (float)Math.Pow((double)linear, 0.45454545454545453);
		}

		// Token: 0x0600B930 RID: 47408 RVA: 0x004454A3 File Offset: 0x004436A3
		public static void Expand(ref tVec3 pColor)
		{
			pColor.X = Gamma.Expand(pColor.X);
			pColor.Y = Gamma.Expand(pColor.Y);
			pColor.Z = Gamma.Expand(pColor.Z);
		}

		// Token: 0x0600B931 RID: 47409 RVA: 0x004454D8 File Offset: 0x004436D8
		public static void Compress(ref tVec3 pColor)
		{
			pColor.X = Gamma.Compress(pColor.X);
			pColor.Y = Gamma.Compress(pColor.Y);
			pColor.Z = Gamma.Compress(pColor.Z);
		}

		// Token: 0x0600B932 RID: 47410 RVA: 0x0044550D File Offset: 0x0044370D
		public static void Expand(ref float r, ref float g, ref float b)
		{
			r = Gamma.Expand(r);
			g = Gamma.Expand(g);
			b = Gamma.Expand(b);
		}

		// Token: 0x0600B933 RID: 47411 RVA: 0x0044552A File Offset: 0x0044372A
		public static void Compress(ref float r, ref float g, ref float b)
		{
			r = Gamma.Compress(r);
			g = Gamma.Compress(g);
			b = Gamma.Compress(b);
		}

		// Token: 0x0600B934 RID: 47412 RVA: 0x00445548 File Offset: 0x00443748
		public static tVec3 Expand(float r, float g, float b)
		{
			tVec3 tVec = new tVec3(r, g, b);
			Gamma.Expand(ref tVec);
			return tVec;
		}

		// Token: 0x0600B935 RID: 47413 RVA: 0x00445568 File Offset: 0x00443768
		public static tVec3 Compress(float r, float g, float b)
		{
			tVec3 tVec = new tVec3(r, g, b);
			Gamma.Compress(ref tVec);
			return tVec;
		}

		// Token: 0x0600B936 RID: 47414 RVA: 0x00445587 File Offset: 0x00443787
		public static float Expand_sRGB(float nonlinear)
		{
			if (nonlinear > 0.04045f)
			{
				return (float)Math.Pow((double)((nonlinear + 0.055f) / 1.055f), 2.4000000953674316);
			}
			return nonlinear / 12.92f;
		}

		// Token: 0x0600B937 RID: 47415 RVA: 0x004455B6 File Offset: 0x004437B6
		public static float Compress_sRGB(float linear)
		{
			if (linear > 0.0031308f)
			{
				return 1.055f * (float)Math.Pow((double)linear, 0.4166666567325592) - 0.055f;
			}
			return 12.92f * linear;
		}

		// Token: 0x0600B938 RID: 47416 RVA: 0x004455E5 File Offset: 0x004437E5
		public static void Expand_sRGB(ref tVec3 pColor)
		{
			pColor.X = Gamma.Expand_sRGB(pColor.X);
			pColor.Y = Gamma.Expand_sRGB(pColor.Y);
			pColor.Z = Gamma.Expand_sRGB(pColor.Z);
		}

		// Token: 0x0600B939 RID: 47417 RVA: 0x0044561A File Offset: 0x0044381A
		public static void Compress_sRGB(ref tVec3 pColor)
		{
			pColor.X = Gamma.Compress_sRGB(pColor.X);
			pColor.Y = Gamma.Compress_sRGB(pColor.Y);
			pColor.Z = Gamma.Compress_sRGB(pColor.Z);
		}

		// Token: 0x0600B93A RID: 47418 RVA: 0x0044564F File Offset: 0x0044384F
		public static void Expand_sRGB(ref float r, ref float g, ref float b)
		{
			r = Gamma.Expand_sRGB(r);
			g = Gamma.Expand_sRGB(g);
			b = Gamma.Expand_sRGB(b);
		}

		// Token: 0x0600B93B RID: 47419 RVA: 0x0044566C File Offset: 0x0044386C
		public static void Compress_sRGB(ref float r, ref float g, ref float b)
		{
			r = Gamma.Compress_sRGB(r);
			g = Gamma.Compress_sRGB(g);
			b = Gamma.Compress_sRGB(b);
		}

		// Token: 0x0600B93C RID: 47420 RVA: 0x0044568C File Offset: 0x0044388C
		public static tVec3 Expand_sRGB(float r, float g, float b)
		{
			tVec3 tVec = new tVec3(r, g, b);
			Gamma.Expand_sRGB(ref tVec);
			return tVec;
		}

		// Token: 0x0600B93D RID: 47421 RVA: 0x004456AC File Offset: 0x004438AC
		public static tVec3 Compress_sRGB(float r, float g, float b)
		{
			tVec3 tVec = new tVec3(r, g, b);
			Gamma.Compress_sRGB(ref tVec);
			return tVec;
		}
	}
}
