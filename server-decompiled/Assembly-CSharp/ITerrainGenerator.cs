using System;
using UnityEngine;

// Token: 0x02000C33 RID: 3123
public interface ITerrainGenerator
{
	// Token: 0x06005F1C RID: 24348
	void Init(World _world, IBiomeProvider _biomeProvider, int _seed);

	// Token: 0x06005F1D RID: 24349
	void GenerateTerrain(World worldData, Chunk chunk, GameRandom _random);

	// Token: 0x06005F1E RID: 24350
	void GenerateTerrain(World worldData, Chunk chunk, GameRandom _random, Vector3i _areaStart, Vector3i _areaSize, bool _bFillEmptyBlocks, bool _isReset);

	// Token: 0x06005F1F RID: 24351
	byte GetTerrainHeightByteAt(int _xWorld, int _zWorld);

	// Token: 0x06005F20 RID: 24352
	sbyte GetDensityAt(int _xWorld, int _yWorld, int _zWorld, BiomeDefinition _bd, float _biomeIntensity);

	// Token: 0x06005F21 RID: 24353
	Vector3 GetTerrainNormalAt(int _xWorld, int _zWorld, BiomeDefinition _bd, float _biomeIntensity);

	// Token: 0x06005F22 RID: 24354
	float GetTerrainHeightAt(int _xWorld, int _zWorld);
}
