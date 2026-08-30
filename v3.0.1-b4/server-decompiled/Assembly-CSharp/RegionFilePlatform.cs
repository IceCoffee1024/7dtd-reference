using System;

// Token: 0x02000B91 RID: 2961
public static class RegionFilePlatform
{
	// Token: 0x06005996 RID: 22934 RVA: 0x0022672C File Offset: 0x0022492C
	public static IRegionFilePlatformFactory CreateFactory()
	{
		return new RegionFileFactorySectorBased();
	}
}
