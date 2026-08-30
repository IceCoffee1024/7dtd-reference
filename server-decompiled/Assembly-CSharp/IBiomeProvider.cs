using System;
using System.Collections;

// Token: 0x02000C32 RID: 3122
public interface IBiomeProvider
{
	// Token: 0x06005F10 RID: 24336
	IEnumerator InitData();

	// Token: 0x06005F11 RID: 24337
	void Init(int _seed, string _worldName, WorldBiomes _biomes, string _params1, string _params2);

	// Token: 0x06005F12 RID: 24338
	int GetSubBiomeIdxAt(BiomeDefinition bd, int _x, int _y, int _z);

	// Token: 0x06005F13 RID: 24339
	BiomeDefinition GetBiomeAt(int _x, int _z);

	// Token: 0x06005F14 RID: 24340
	BiomeDefinition GetBiomeAt(int _x, int _z, out float _intensity);

	// Token: 0x06005F15 RID: 24341 RVA: 0x0001FF1E File Offset: 0x0001E11E
	BiomeDefinition GetBiomeOrSubAt(int x, int z)
	{
		return null;
	}

	// Token: 0x06005F16 RID: 24342
	float GetHumidityAt(int x, int z);

	// Token: 0x06005F17 RID: 24343
	float GetTemperatureAt(int x, int z);

	// Token: 0x06005F18 RID: 24344
	float GetRadiationAt(int x, int z);

	// Token: 0x06005F19 RID: 24345
	string GetWorldName();

	// Token: 0x06005F1A RID: 24346
	BlockValue GetTopmostBlockValue(int xWorld, int zWorld);

	// Token: 0x06005F1B RID: 24347 RVA: 0x000027FC File Offset: 0x000009FC
	void Cleanup()
	{
	}
}
