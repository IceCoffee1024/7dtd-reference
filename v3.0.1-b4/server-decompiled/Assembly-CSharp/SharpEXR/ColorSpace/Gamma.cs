using System;

namespace SharpEXR.ColorSpace
{
	// Token: 0x020016B5 RID: 5813
	public static class Gamma
	{
		// Token: 0x0600B580 RID: 46464 RVA: 0x00438741 File Offset: 0x00436941
		public static float Expand(float nonlinear)
		{
			return (float)Math.Pow((double)nonlinear, 2.2);
		}

		// Token: 0x0600B581 RID: 46465 RVA: 0x00438754 File Offset: 0x00436954
		public static float Compress(float linear)
		{
			return (float)Math.Pow((double)linear, 0.45454545454545453);
		}

		// Token: 0x0600B582 RID: 46466 RVA: 0x00438767 File Offset: 0x00436967
		public static void Expand(ref tVec3 pColor)
		{
			pColor.X = Gamma.Expand(pColor.X);
			pColor.Y = Gamma.Expand(pColor.Y);
			pColor.Z = Gamma.Expand(pColor.Z);
		}

		// Token: 0x0600B583 RID: 46467 RVA: 0x0043879C File Offset: 0x0043699C
		public static void Compress(ref tVec3 pColor)
		{
			pColor.X = Gamma.Compress(pColor.X);
			pColor.Y = Gamma.Compress(pColor.Y);
			pColor.Z = Gamma.Compress(pColor.Z);
		}

		// Token: 0x0600B584 RID: 46468 RVA: 0x004387D1 File Offset: 0x004369D1
		public static void Expand(ref float r, ref float g, ref float b)
		{
			r = Gamma.Expand(r);
			g = Gamma.Expand(g);
			b = Gamma.Expand(b);
		}

		// Token: 0x0600B585 RID: 46469 RVA: 0x004387EE File Offset: 0x004369EE
		public static void Compress(ref float r, ref float g, ref float b)
		{
			r = Gamma.Compress(r);
			g = Gamma.Compress(g);
			b = Gamma.Compress(b);
		}

		// Token: 0x0600B586 RID: 46470 RVA: 0x0043880C File Offset: 0x00436A0C
		public static tVec3 Expand(float r, float g, float b)
		{
			tVec3 tVec = new tVec3(r, g, b);
			Gamma.Expand(ref tVec);
			return tVec;
		}

		// Token: 0x0600B587 RID: 46471 RVA: 0x0043882C File Offset: 0x00436A2C
		public static tVec3 Compress(float r, float g, float b)
		{
			tVec3 tVec = new tVec3(r, g, b);
			Gamma.Compress(ref tVec);
			return tVec;
		}

		// Token: 0x0600B588 RID: 46472 RVA: 0x0043884B File Offset: 0x00436A4B
		public static float Expand_sRGB(float nonlinear)
		{
			if (nonlinear > 0.04045f)
			{
				return (float)Math.Pow((double)((nonlinear + 0.055f) / 1.055f), 2.4000000953674316);
			}
			return nonlinear / 12.92f;
		}

		// Token: 0x0600B589 RID: 46473 RVA: 0x0043887A File Offset: 0x00436A7A
		public static float Compress_sRGB(float linear)
		{
			if (linear > 0.0031308f)
			{
				return 1.055f * (float)Math.Pow((double)linear, 0.4166666567325592) - 0.055f;
			}
			return 12.92f * linear;
		}

		// Token: 0x0600B58A RID: 46474 RVA: 0x004388A9 File Offset: 0x00436AA9
		public static void Expand_sRGB(ref tVec3 pColor)
		{
			pColor.X = Gamma.Expand_sRGB(pColor.X);
			pColor.Y = Gamma.Expand_sRGB(pColor.Y);
			pColor.Z = Gamma.Expand_sRGB(pColor.Z);
		}

		// Token: 0x0600B58B RID: 46475 RVA: 0x004388DE File Offset: 0x00436ADE
		public static void Compress_sRGB(ref tVec3 pColor)
		{
			pColor.X = Gamma.Compress_sRGB(pColor.X);
			pColor.Y = Gamma.Compress_sRGB(pColor.Y);
			pColor.Z = Gamma.Compress_sRGB(pColor.Z);
		}

		// Token: 0x0600B58C RID: 46476 RVA: 0x00438913 File Offset: 0x00436B13
		public static void Expand_sRGB(ref float r, ref float g, ref float b)
		{
			r = Gamma.Expand_sRGB(r);
			g = Gamma.Expand_sRGB(g);
			b = Gamma.Expand_sRGB(b);
		}

		// Token: 0x0600B58D RID: 46477 RVA: 0x00438930 File Offset: 0x00436B30
		public static void Compress_sRGB(ref float r, ref float g, ref float b)
		{
			r = Gamma.Compress_sRGB(r);
			g = Gamma.Compress_sRGB(g);
			b = Gamma.Compress_sRGB(b);
		}

		// Token: 0x0600B58E RID: 46478 RVA: 0x00438950 File Offset: 0x00436B50
		public static tVec3 Expand_sRGB(float r, float g, float b)
		{
			tVec3 tVec = new tVec3(r, g, b);
			Gamma.Expand_sRGB(ref tVec);
			return tVec;
		}

		// Token: 0x0600B58F RID: 46479 RVA: 0x00438970 File Offset: 0x00436B70
		public static tVec3 Compress_sRGB(float r, float g, float b)
		{
			tVec3 tVec = new tVec3(r, g, b);
			Gamma.Compress_sRGB(ref tVec);
			return tVec;
		}
	}
}
