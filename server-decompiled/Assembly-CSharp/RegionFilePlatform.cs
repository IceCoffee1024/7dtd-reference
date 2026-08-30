using System;

// Token: 0x02000BA3 RID: 2979
public static class RegionFilePlatform
{
	// Token: 0x06005A1E RID: 23070 RVA: 0x00229448 File Offset: 0x00227648
	public static IRegionFilePlatformFactory CreateFactory()
	{
		return new RegionFileFactorySectorBased();
	}
}
