using System;

namespace SharpEXR.ColorSpace
{
	// Token: 0x02001707 RID: 5895
	public static class Gamma
	{
		// Token: 0x0600B85A RID: 47194 RVA: 0x004429D9 File Offset: 0x00440BD9
		public static float Expand(float nonlinear)
		{
			return (float)Math.Pow((double)nonlinear, 2.2);
		}

		// Token: 0x0600B85B RID: 47195 RVA: 0x004429EC File Offset: 0x00440BEC
		public static float Compress(float linear)
		{
			return (float)Math.Pow((double)linear, 0.45454545454545453);
		}

		// Token: 0x0600B85C RID: 47196 RVA: 0x004429FF File Offset: 0x00440BFF
		public static void Expand(ref tVec3 pColor)
		{
			pColor.X = Gamma.Expand(pColor.X);
			pColor.Y = Gamma.Expand(pColor.Y);
			pColor.Z = Gamma.Expand(pColor.Z);
		}

		// Token: 0x0600B85D RID: 47197 RVA: 0x00442A34 File Offset: 0x00440C34
		public static void Compress(ref tVec3 pColor)
		{
			pColor.X = Gamma.Compress(pColor.X);
			pColor.Y = Gamma.Compress(pColor.Y);
			pColor.Z = Gamma.Compress(pColor.Z);
		}

		// Token: 0x0600B85E RID: 47198 RVA: 0x00442A69 File Offset: 0x00440C69
		public static void Expand(ref float r, ref float g, ref float b)
		{
			r = Gamma.Expand(r);
			g = Gamma.Expand(g);
			b = Gamma.Expand(b);
		}

		// Token: 0x0600B85F RID: 47199 RVA: 0x00442A86 File Offset: 0x00440C86
		public static void Compress(ref float r, ref float g, ref float b)
		{
			r = Gamma.Compress(r);
			g = Gamma.Compress(g);
			b = Gamma.Compress(b);
		}

		// Token: 0x0600B860 RID: 47200 RVA: 0x00442AA4 File Offset: 0x00440CA4
		public static tVec3 Expand(float r, float g, float b)
		{
			tVec3 tVec = new tVec3(r, g, b);
			Gamma.Expand(ref tVec);
			return tVec;
		}

		// Token: 0x0600B861 RID: 47201 RVA: 0x00442AC4 File Offset: 0x00440CC4
		public static tVec3 Compress(float r, float g, float b)
		{
			tVec3 tVec = new tVec3(r, g, b);
			Gamma.Compress(ref tVec);
			return tVec;
		}

		// Token: 0x0600B862 RID: 47202 RVA: 0x00442AE3 File Offset: 0x00440CE3
		public static float Expand_sRGB(float nonlinear)
		{
			if (nonlinear > 0.04045f)
			{
				return (float)Math.Pow((double)((nonlinear + 0.055f) / 1.055f), 2.4000000953674316);
			}
			return nonlinear / 12.92f;
		}

		// Token: 0x0600B863 RID: 47203 RVA: 0x00442B12 File Offset: 0x00440D12
		public static float Compress_sRGB(float linear)
		{
			if (linear > 0.0031308f)
			{
				return 1.055f * (float)Math.Pow((double)linear, 0.4166666567325592) - 0.055f;
			}
			return 12.92f * linear;
		}

		// Token: 0x0600B864 RID: 47204 RVA: 0x00442B41 File Offset: 0x00440D41
		public static void Expand_sRGB(ref tVec3 pColor)
		{
			pColor.X = Gamma.Expand_sRGB(pColor.X);
			pColor.Y = Gamma.Expand_sRGB(pColor.Y);
			pColor.Z = Gamma.Expand_sRGB(pColor.Z);
		}

		// Token: 0x0600B865 RID: 47205 RVA: 0x00442B76 File Offset: 0x00440D76
		public static void Compress_sRGB(ref tVec3 pColor)
		{
			pColor.X = Gamma.Compress_sRGB(pColor.X);
			pColor.Y = Gamma.Compress_sRGB(pColor.Y);
			pColor.Z = Gamma.Compress_sRGB(pColor.Z);
		}

		// Token: 0x0600B866 RID: 47206 RVA: 0x00442BAB File Offset: 0x00440DAB
		public static void Expand_sRGB(ref float r, ref float g, ref float b)
		{
			r = Gamma.Expand_sRGB(r);
			g = Gamma.Expand_sRGB(g);
			b = Gamma.Expand_sRGB(b);
		}

		// Token: 0x0600B867 RID: 47207 RVA: 0x00442BC8 File Offset: 0x00440DC8
		public static void Compress_sRGB(ref float r, ref float g, ref float b)
		{
			r = Gamma.Compress_sRGB(r);
			g = Gamma.Compress_sRGB(g);
			b = Gamma.Compress_sRGB(b);
		}

		// Token: 0x0600B868 RID: 47208 RVA: 0x00442BE8 File Offset: 0x00440DE8
		public static tVec3 Expand_sRGB(float r, float g, float b)
		{
			tVec3 tVec = new tVec3(r, g, b);
			Gamma.Expand_sRGB(ref tVec);
			return tVec;
		}

		// Token: 0x0600B869 RID: 47209 RVA: 0x00442C08 File Offset: 0x00440E08
		public static tVec3 Compress_sRGB(float r, float g, float b)
		{
			tVec3 tVec = new tVec3(r, g, b);
			Gamma.Compress_sRGB(ref tVec);
			return tVec;
		}
	}
}
