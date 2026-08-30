using System;
using UnityEngine;

// Token: 0x02000C45 RID: 3141
public interface ITerrainGenerator
{
	// Token: 0x06005FA6 RID: 24486
	void Init(World _world, IBiomeProvider _biomeProvider, int _seed);

	// Token: 0x06005FA7 RID: 24487
	void GenerateTerrain(World worldData, Chunk chunk, GameRandom _random);

	// Token: 0x06005FA8 RID: 24488
	void GenerateTerrain(World worldData, Chunk chunk, GameRandom _random, Vector3i _areaStart, Vector3i _areaSize, bool _bFillEmptyBlocks, bool _isReset);

	// Token: 0x06005FA9 RID: 24489
	byte GetTerrainHeightByteAt(int _xWorld, int _zWorld);

	// Token: 0x06005FAA RID: 24490
	sbyte GetDensityAt(int _xWorld, int _yWorld, int _zWorld, BiomeDefinition _bd, float _biomeIntensity);

	// Token: 0x06005FAB RID: 24491
	Vector3 GetTerrainNormalAt(int _xWorld, int _zWorld, BiomeDefinition _bd, float _biomeIntensity);

	// Token: 0x06005FAC RID: 24492
	float GetTerrainHeightAt(int _xWorld, int _zWorld);
}
