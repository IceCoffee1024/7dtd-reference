using System;
using System.Collections.Generic;

// Token: 0x02000AF5 RID: 2805
public class BiomeSpawnEntityGroupList
{
	// Token: 0x0600535B RID: 21339 RVA: 0x00200918 File Offset: 0x001FEB18
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

	// Token: 0x04004123 RID: 16675
	public List<BiomeSpawnEntityGroupData> list = new List<BiomeSpawnEntityGroupData>();
}
