using System;

// Token: 0x02000B98 RID: 2968
public class RegionFileDebugUtilSectorBased : IRegionFileDebugUtil
{
	// Token: 0x060059BF RID: 22975 RVA: 0x00224C2C File Offset: 0x00222E2C
	public string GetLocationString(int chunkX, int chunkZ)
	{
		int num = (int)Math.Floor((double)chunkX / 32.0);
		int num2 = (int)Math.Floor((double)chunkZ / 32.0);
		return string.Format("XZ: {0}/{1}  Region: r.{2}.{3}", new object[] { chunkX, chunkZ, num, num2 });
	}
}
