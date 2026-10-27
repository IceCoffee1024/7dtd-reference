using System;
using System.Collections;

// Token: 0x02000C61 RID: 3169
public interface IBiomeProvider
{
	// Token: 0x06005FD8 RID: 24536
	IEnumerator InitData();

	// Token: 0x06005FD9 RID: 24537
	void Init(int _seed, string _worldName, WorldBiomes _biomes, string _params1, string _params2);

	// Token: 0x06005FDA RID: 24538
	int GetSubBiomeIdxAt(BiomeDefinition bd, int _x, int _y, int _z);

	// Token: 0x06005FDB RID: 24539
	BiomeDefinition GetBiomeAt(int _x, int _z);

	// Token: 0x06005FDC RID: 24540
	BiomeDefinition GetBiomeAt(int _x, int _z, out float _intensity);

	// Token: 0x06005FDD RID: 24541 RVA: 0x000200DA File Offset: 0x0001E2DA
	BiomeDefinition GetBiomeOrSubAt(int x, int z)
	{
		return null;
	}

	// Token: 0x06005FDE RID: 24542
	float GetHumidityAt(int x, int z);

	// Token: 0x06005FDF RID: 24543
	float GetTemperatureAt(int x, int z);

	// Token: 0x06005FE0 RID: 24544
	float GetRadiationAt(int x, int z);

	// Token: 0x06005FE1 RID: 24545
	string GetWorldName();

	// Token: 0x06005FE2 RID: 24546
	BlockValue GetTopmostBlockValue(int xWorld, int zWorld);

	// Token: 0x06005FE3 RID: 24547 RVA: 0x000027FC File Offset: 0x000009FC
	void Cleanup()
	{
	}
}
