using System;

// Token: 0x02000AEB RID: 2795
public class BiomeSpawnEntityGroupData
{
	// Token: 0x06005338 RID: 21304 RVA: 0x001FF3EC File Offset: 0x001FD5EC
	public BiomeSpawnEntityGroupData(int _idHash, int _maxCount, int[] _respawndelay, EDaytime _daytime, BiomeSpawnEntityGroupData.eType _type)
	{
		this.idHash = _idHash;
		this.maxCount = _maxCount;
		this.daytime = _daytime;
		this.respawnDelayInWorldTime = _respawndelay;
		this.type = _type;
	}

	// Token: 0x040040FF RID: 16639
	public int idHash;

	// Token: 0x04004100 RID: 16640
	public string entityGroupName;

	// Token: 0x04004101 RID: 16641
	public int maxCount;

	// Token: 0x04004102 RID: 16642
	public int[] respawnDelayInWorldTime;

	// Token: 0x04004103 RID: 16643
	public EDaytime daytime;

	// Token: 0x04004104 RID: 16644
	public FastTags<TagGroup.Poi> POITags;

	// Token: 0x04004105 RID: 16645
	public FastTags<TagGroup.Poi> noPOITags;

	// Token: 0x04004106 RID: 16646
	public BiomeSpawnEntityGroupData.eType type;

	// Token: 0x02000AEC RID: 2796
	public enum eType
	{
		// Token: 0x04004108 RID: 16648
		Normal,
		// Token: 0x04004109 RID: 16649
		Animal,
		// Token: 0x0400410A RID: 16650
		Rare
	}
}
