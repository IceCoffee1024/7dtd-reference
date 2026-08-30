using System;
using UnityEngine.Scripting;

// Token: 0x0200038D RID: 909
[Preserve]
public class RegionItemData
{
	// Token: 0x06001B0F RID: 6927 RVA: 0x000A0BD9 File Offset: 0x0009EDD9
	public RegionItemData(int x, int z, int updateTime)
	{
		this.X = x;
		this.Z = z;
		this.UpdateTime = updateTime;
	}

	// Token: 0x06001B10 RID: 6928 RVA: 0x000A0BF6 File Offset: 0x0009EDF6
	public void Update(int x, int z, int updateTime)
	{
		this.X = x;
		this.Z = z;
		this.UpdateTime = updateTime;
	}

	// Token: 0x06001B11 RID: 6929 RVA: 0x000A0C0D File Offset: 0x0009EE0D
	public void Update(DynamicMeshItem item)
	{
		this.X = item.WorldPosition.x;
		this.Z = item.WorldPosition.z;
		this.UpdateTime = item.UpdateTime;
	}

	// Token: 0x04001169 RID: 4457
	public int X;

	// Token: 0x0400116A RID: 4458
	public int Z;

	// Token: 0x0400116B RID: 4459
	public int UpdateTime;
}
