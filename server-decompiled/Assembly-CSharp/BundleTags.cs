using System;

// Token: 0x020011B3 RID: 4531
public static class BundleTags
{
	// Token: 0x1700118E RID: 4494
	// (get) Token: 0x060090A8 RID: 37032 RVA: 0x00366FF4 File Offset: 0x003651F4
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

	// Token: 0x040069F8 RID: 27128
	public const string TagHalfRes = "_halfres";
}
