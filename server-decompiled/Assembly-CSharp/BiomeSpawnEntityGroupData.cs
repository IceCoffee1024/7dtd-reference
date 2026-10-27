using System;

// Token: 0x02000AF6 RID: 2806
public class BiomeSpawnEntityGroupData
{
	// Token: 0x0600535D RID: 21341 RVA: 0x00200970 File Offset: 0x001FEB70
	public BiomeSpawnEntityGroupData(int _idHash, int _maxCount, int[] _respawndelay, EDaytime _daytime, BiomeSpawnEntityGroupData.eType _type)
	{
		this.idHash = _idHash;
		this.maxCount = _maxCount;
		this.daytime = _daytime;
		this.respawnDelayInWorldTime = _respawndelay;
		this.type = _type;
	}

	// Token: 0x04004124 RID: 16676
	public int idHash;

	// Token: 0x04004125 RID: 16677
	public string entityGroupName;

	// Token: 0x04004126 RID: 16678
	public int maxCount;

	// Token: 0x04004127 RID: 16679
	public int[] respawnDelayInWorldTime;

	// Token: 0x04004128 RID: 16680
	public EDaytime daytime;

	// Token: 0x04004129 RID: 16681
	public FastTags<TagGroup.Poi> POITags;

	// Token: 0x0400412A RID: 16682
	public FastTags<TagGroup.Poi> noPOITags;

	// Token: 0x0400412B RID: 16683
	public BiomeSpawnEntityGroupData.eType type;

	// Token: 0x02000AF7 RID: 2807
	public enum eType
	{
		// Token: 0x0400412D RID: 16685
		Normal,
		// Token: 0x0400412E RID: 16686
		Animal,
		// Token: 0x0400412F RID: 16687
		Rare
	}
}
