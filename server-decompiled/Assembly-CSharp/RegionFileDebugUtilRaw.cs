using System;

// Token: 0x02000BAB RID: 2987
public class RegionFileDebugUtilRaw : IRegionFileDebugUtil
{
	// Token: 0x060059DB RID: 23003 RVA: 0x00227F24 File Offset: 0x00226124
	public string GetLocationString(int chunkX, int chunkZ)
	{
		int num = (int)Math.Floor((double)chunkX / 8.0);
		int num2 = (int)Math.Floor((double)chunkZ / 8.0);
		return string.Format("XZ: {0}/{1}  Region: r.{2}.{3}", new object[] { chunkX, chunkZ, num, num2 });
	}
}
