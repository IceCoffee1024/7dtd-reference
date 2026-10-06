using System;
using System.Collections;

// Token: 0x02000C56 RID: 3158
public interface IBiomeProvider
{
	// Token: 0x06005FB3 RID: 24499
	IEnumerator InitData();

	// Token: 0x06005FB4 RID: 24500
	void Init(int _seed, string _worldName, WorldBiomes _biomes, string _params1, string _params2);

	// Token: 0x06005FB5 RID: 24501
	int GetSubBiomeIdxAt(BiomeDefinition bd, int _x, int _y, int _z);

	// Token: 0x06005FB6 RID: 24502
	BiomeDefinition GetBiomeAt(int _x, int _z);

	// Token: 0x06005FB7 RID: 24503
	BiomeDefinition GetBiomeAt(int _x, int _z, out float _intensity);

	// Token: 0x06005FB8 RID: 24504 RVA: 0x000200DA File Offset: 0x0001E2DA
	BiomeDefinition GetBiomeOrSubAt(int x, int z)
	{
		return null;
	}

	// Token: 0x06005FB9 RID: 24505
	float GetHumidityAt(int x, int z);

	// Token: 0x06005FBA RID: 24506
	float GetTemperatureAt(int x, int z);

	// Token: 0x06005FBB RID: 24507
	float GetRadiationAt(int x, int z);

	// Token: 0x06005FBC RID: 24508
	string GetWorldName();

	// Token: 0x06005FBD RID: 24509
	BlockValue GetTopmostBlockValue(int xWorld, int zWorld);

	// Token: 0x06005FBE RID: 24510 RVA: 0x000027FC File Offset: 0x000009FC
	void Cleanup()
	{
	}
}
