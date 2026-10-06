using System;

// Token: 0x02000BAA RID: 2986
public class RegionFileDebugUtilSectorBased : IRegionFileDebugUtil
{
	// Token: 0x060059D9 RID: 23001 RVA: 0x00227EBC File Offset: 0x002260BC
	public string GetLocationString(int chunkX, int chunkZ)
	{
		int num = (int)Math.Floor((double)chunkX / 32.0);
		int num2 = (int)Math.Floor((double)chunkZ / 32.0);
		return string.Format("XZ: {0}/{1}  Region: r.{2}.{3}", new object[] { chunkX, chunkZ, num, num2 });
	}
}
