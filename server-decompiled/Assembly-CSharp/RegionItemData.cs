using System;
using UnityEngine.Scripting;

// Token: 0x02000396 RID: 918
[Preserve]
public class RegionItemData
{
	// Token: 0x06001B2A RID: 6954 RVA: 0x000A177D File Offset: 0x0009F97D
	public RegionItemData(int x, int z, int updateTime)
	{
		this.X = x;
		this.Z = z;
		this.UpdateTime = updateTime;
	}

	// Token: 0x06001B2B RID: 6955 RVA: 0x000A179A File Offset: 0x0009F99A
	public void Update(int x, int z, int updateTime)
	{
		this.X = x;
		this.Z = z;
		this.UpdateTime = updateTime;
	}

	// Token: 0x06001B2C RID: 6956 RVA: 0x000A17B1 File Offset: 0x0009F9B1
	public void Update(DynamicMeshItem item)
	{
		this.X = item.WorldPosition.x;
		this.Z = item.WorldPosition.z;
		this.UpdateTime = item.UpdateTime;
	}

	// Token: 0x04001187 RID: 4487
	public int X;

	// Token: 0x04001188 RID: 4488
	public int Z;

	// Token: 0x04001189 RID: 4489
	public int UpdateTime;
}
