using System;

// Token: 0x02000AC8 RID: 2760
public class BiomeSpawnEntityGroupData
{
	// Token: 0x06005299 RID: 21145 RVA: 0x001F96F4 File Offset: 0x001F78F4
	public BiomeSpawnEntityGroupData(int _idHash, int _maxCount, int[] _respawndelay, EDaytime _daytime, bool _isAnimal)
	{
		this.idHash = _idHash;
		this.maxCount = _maxCount;
		this.daytime = _daytime;
		this.respawnDelayInWorldTime = _respawndelay;
		this.isAnimal = _isAnimal;
	}

	// Token: 0x04004031 RID: 16433
	public int idHash;

	// Token: 0x04004032 RID: 16434
	public string entityGroupName;

	// Token: 0x04004033 RID: 16435
	public int maxCount;

	// Token: 0x04004034 RID: 16436
	public int[] respawnDelayInWorldTime;

	// Token: 0x04004035 RID: 16437
	public EDaytime daytime;

	// Token: 0x04004036 RID: 16438
	public FastTags<TagGroup.Poi> POITags;

	// Token: 0x04004037 RID: 16439
	public FastTags<TagGroup.Poi> noPOITags;

	// Token: 0x04004038 RID: 16440
	public bool isAnimal;
}
