using System;
using UnityEngine;

// Token: 0x02000C62 RID: 3170
public interface ITerrainGenerator
{
	// Token: 0x06005FE4 RID: 24548
	void Init(World _world, IBiomeProvider _biomeProvider, int _seed);

	// Token: 0x06005FE5 RID: 24549
	void GenerateTerrain(World worldData, Chunk chunk, GameRandom _random);

	// Token: 0x06005FE6 RID: 24550
	void GenerateTerrain(World worldData, Chunk chunk, GameRandom _random, Vector3i _areaStart, Vector3i _areaSize, bool _bFillEmptyBlocks, bool _isReset);

	// Token: 0x06005FE7 RID: 24551
	byte GetTerrainHeightByteAt(int _xWorld, int _zWorld);

	// Token: 0x06005FE8 RID: 24552
	sbyte GetDensityAt(int _xWorld, int _yWorld, int _zWorld, BiomeDefinition _bd, float _biomeIntensity);

	// Token: 0x06005FE9 RID: 24553
	Vector3 GetTerrainNormalAt(int _xWorld, int _zWorld, BiomeDefinition _bd, float _biomeIntensity);

	// Token: 0x06005FEA RID: 24554
	float GetTerrainHeightAt(int _xWorld, int _zWorld);
}
