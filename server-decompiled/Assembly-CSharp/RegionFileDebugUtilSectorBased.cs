using System;

// Token: 0x02000B86 RID: 2950
public class RegionFileDebugUtilSectorBased : IRegionFileDebugUtil
{
	// Token: 0x06005937 RID: 22839 RVA: 0x00221F10 File Offset: 0x00220110
	public string GetLocationString(int chunkX, int chunkZ)
	{
		int num = (int)Math.Floor((double)chunkX / 32.0);
		int num2 = (int)Math.Floor((double)chunkZ / 32.0);
		return string.Format("XZ: {0}/{1}  Region: r.{2}.{3}", new object[] { chunkX, chunkZ, num, num2 });
	}
}
