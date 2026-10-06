using System;
using UnityEngine;

// Token: 0x02000C57 RID: 3159
public interface ITerrainGenerator
{
	// Token: 0x06005FBF RID: 24511
	void Init(World _world, IBiomeProvider _biomeProvider, int _seed);

	// Token: 0x06005FC0 RID: 24512
	void GenerateTerrain(World worldData, Chunk chunk, GameRandom _random);

	// Token: 0x06005FC1 RID: 24513
	void GenerateTerrain(World worldData, Chunk chunk, GameRandom _random, Vector3i _areaStart, Vector3i _areaSize, bool _bFillEmptyBlocks, bool _isReset);

	// Token: 0x06005FC2 RID: 24514
	byte GetTerrainHeightByteAt(int _xWorld, int _zWorld);

	// Token: 0x06005FC3 RID: 24515
	sbyte GetDensityAt(int _xWorld, int _yWorld, int _zWorld, BiomeDefinition _bd, float _biomeIntensity);

	// Token: 0x06005FC4 RID: 24516
	Vector3 GetTerrainNormalAt(int _xWorld, int _zWorld, BiomeDefinition _bd, float _biomeIntensity);

	// Token: 0x06005FC5 RID: 24517
	float GetTerrainHeightAt(int _xWorld, int _zWorld);
}
