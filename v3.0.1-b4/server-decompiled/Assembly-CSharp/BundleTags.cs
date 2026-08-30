using System;

// Token: 0x02001171 RID: 4465
public static class BundleTags
{
	// Token: 0x17001106 RID: 4358
	// (get) Token: 0x06008ED0 RID: 36560 RVA: 0x0035BC9C File Offset: 0x00359E9C
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

	// Token: 0x04006883 RID: 26755
	public const string TagHalfRes = "_halfres";
}
