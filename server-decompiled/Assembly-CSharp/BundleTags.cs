using System;

// Token: 0x0200118B RID: 4491
public static class BundleTags
{
	// Token: 0x1700113F RID: 4415
	// (get) Token: 0x06008FC8 RID: 36808 RVA: 0x00360738 File Offset: 0x0035E938
	public static string Tag
	{
		get
		{
			if (!PlatformOptimizations.LoadHalfResAssets)
			{
				return string.Empty;
			}
			return "_halfres";
		}
	}

	// Token: 0x04006952 RID: 26962
	public const string TagHalfRes = "_halfres";
}
