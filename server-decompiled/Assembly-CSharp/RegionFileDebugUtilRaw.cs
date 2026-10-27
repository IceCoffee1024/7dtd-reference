using System;

// Token: 0x02000BB6 RID: 2998
public class RegionFileDebugUtilRaw : IRegionFileDebugUtil
{
	// Token: 0x06005A00 RID: 23040 RVA: 0x00229480 File Offset: 0x00227680
	public string GetLocationString(int chunkX, int chunkZ)
	{
		int num = (int)Math.Floor((double)chunkX / 8.0);
		int num2 = (int)Math.Floor((double)chunkZ / 8.0);
		return string.Format("XZ: {0}/{1}  Region: r.{2}.{3}", new object[] { chunkX, chunkZ, num, num2 });
	}
}
