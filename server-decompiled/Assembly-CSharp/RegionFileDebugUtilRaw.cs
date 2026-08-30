using System;

// Token: 0x02000B87 RID: 2951
public class RegionFileDebugUtilRaw : IRegionFileDebugUtil
{
	// Token: 0x06005939 RID: 22841 RVA: 0x00221F78 File Offset: 0x00220178
	public string GetLocationString(int chunkX, int chunkZ)
	{
		int num = (int)Math.Floor((double)chunkX / 8.0);
		int num2 = (int)Math.Floor((double)chunkZ / 8.0);
		return string.Format("XZ: {0}/{1}  Region: r.{2}.{3}", new object[] { chunkX, chunkZ, num, num2 });
	}
}
