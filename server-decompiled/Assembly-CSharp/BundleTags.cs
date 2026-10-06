using System;

// Token: 0x020011A3 RID: 4515
public static class BundleTags
{
	// Token: 0x17001183 RID: 4483
	// (get) Token: 0x06009054 RID: 36948 RVA: 0x00364F04 File Offset: 0x00363104
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

	// Token: 0x040069B8 RID: 27064
	public const string TagHalfRes = "_halfres";
}
