using System;

// Token: 0x02000BC0 RID: 3008
public static class RegionFilePlatform
{
	// Token: 0x06005A5C RID: 23132 RVA: 0x0022DCA0 File Offset: 0x0022BEA0
	public static IRegionFilePlatformFactory CreateFactory()
	{
		return new RegionFileFactorySectorBased();
	}
}
