using System;

// Token: 0x02000BB5 RID: 2997
public static class RegionFilePlatform
{
	// Token: 0x06005A37 RID: 23095 RVA: 0x0022C744 File Offset: 0x0022A944
	public static IRegionFilePlatformFactory CreateFactory()
	{
		return new RegionFileFactorySectorBased();
	}
}
