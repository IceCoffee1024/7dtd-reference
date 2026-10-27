using System;

// Token: 0x02000BB5 RID: 2997
public class RegionFileDebugUtilSectorBased : IRegionFileDebugUtil
{
	// Token: 0x060059FE RID: 23038 RVA: 0x00229418 File Offset: 0x00227618
	public string GetLocationString(int chunkX, int chunkZ)
	{
		int num = (int)Math.Floor((double)chunkX / 32.0);
		int num2 = (int)Math.Floor((double)chunkZ / 32.0);
		return string.Format("XZ: {0}/{1}  Region: r.{2}.{3}", new object[] { chunkX, chunkZ, num, num2 });
	}
}
