using System;
using UnityEngine.Scripting;

// Token: 0x02000391 RID: 913
[Preserve]
public class RegionItemData
{
	// Token: 0x06001B2C RID: 6956 RVA: 0x000A1319 File Offset: 0x0009F519
	public RegionItemData(int x, int z, int updateTime)
	{
		this.X = x;
		this.Z = z;
		this.UpdateTime = updateTime;
	}

	// Token: 0x06001B2D RID: 6957 RVA: 0x000A1336 File Offset: 0x0009F536
	public void Update(int x, int z, int updateTime)
	{
		this.X = x;
		this.Z = z;
		this.UpdateTime = updateTime;
	}

	// Token: 0x06001B2E RID: 6958 RVA: 0x000A134D File Offset: 0x0009F54D
	public void Update(DynamicMeshItem item)
	{
		this.X = item.WorldPosition.x;
		this.Z = item.WorldPosition.z;
		this.UpdateTime = item.UpdateTime;
	}

	// Token: 0x0400117E RID: 4478
	public int X;

	// Token: 0x0400117F RID: 4479
	public int Z;

	// Token: 0x04001180 RID: 4480
	public int UpdateTime;
}
