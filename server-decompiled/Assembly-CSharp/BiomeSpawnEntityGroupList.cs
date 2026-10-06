using System;
using System.Collections.Generic;

// Token: 0x02000AEA RID: 2794
public class BiomeSpawnEntityGroupList
{
	// Token: 0x06005336 RID: 21302 RVA: 0x001FF394 File Offset: 0x001FD594
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

	// Token: 0x040040FE RID: 16638
	public List<BiomeSpawnEntityGroupData> list = new List<BiomeSpawnEntityGroupData>();
}
