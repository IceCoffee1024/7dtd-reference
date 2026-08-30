using System;
using System.Collections.Generic;

// Token: 0x02000AC7 RID: 2759
public class BiomeSpawnEntityGroupList
{
	// Token: 0x06005297 RID: 21143 RVA: 0x001F969C File Offset: 0x001F789C
	public BiomeSpawnEntityGroupData Find(int _idHash)
	{
		for (int i = 0; i < this.list.Count; i++)
		{
			if (this.list[i].idHash == _idHash)
			{
				return this.list[i];
			}
		}
		return null;
	}

	// Token: 0x04004030 RID: 16432
	public List<BiomeSpawnEntityGroupData> list = new List<BiomeSpawnEntityGroupData>();
}
