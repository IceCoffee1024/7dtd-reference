using System;
using UnityEngine.Scripting;

// Token: 0x02000394 RID: 916
[Preserve]
public class RegionItemData
{
	// Token: 0x06001B26 RID: 6950 RVA: 0x000A156D File Offset: 0x0009F76D
	public RegionItemData(int x, int z, int updateTime)
	{
		this.X = x;
		this.Z = z;
		this.UpdateTime = updateTime;
	}

	// Token: 0x06001B27 RID: 6951 RVA: 0x000A158A File Offset: 0x0009F78A
	public void Update(int x, int z, int updateTime)
	{
		this.X = x;
		this.Z = z;
		this.UpdateTime = updateTime;
	}

	// Token: 0x06001B28 RID: 6952 RVA: 0x000A15A1 File Offset: 0x0009F7A1
	public void Update(DynamicMeshItem item)
	{
		this.X = item.WorldPosition.x;
		this.Z = item.WorldPosition.z;
		this.UpdateTime = item.UpdateTime;
	}

	// Token: 0x04001177 RID: 4471
	public int X;

	// Token: 0x04001178 RID: 4472
	public int Z;

	// Token: 0x04001179 RID: 4473
	public int UpdateTime;
}
