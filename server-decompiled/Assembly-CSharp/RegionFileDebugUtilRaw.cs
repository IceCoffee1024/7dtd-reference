using System;

// Token: 0x02000B99 RID: 2969
public class RegionFileDebugUtilRaw : IRegionFileDebugUtil
{
	// Token: 0x060059C1 RID: 22977 RVA: 0x00224C94 File Offset: 0x00222E94
	public string GetLocationString(int chunkX, int chunkZ)
	{
		int num = (int)Math.Floor((double)chunkX / 8.0);
		int num2 = (int)Math.Floor((double)chunkZ / 8.0);
		return string.Format("XZ: {0}/{1}  Region: r.{2}.{3}", new object[] { chunkX, chunkZ, num, num2 });
	}
}
