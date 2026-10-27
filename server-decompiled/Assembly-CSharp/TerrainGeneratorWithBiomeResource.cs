using System;
using UnityEngine;

// Token: 0x02000C66 RID: 3174
public abstract class TerrainGeneratorWithBiomeResource : ITerrainGenerator
{
	// Token: 0x06005FFA RID: 24570 RVA: 0x0026175F File Offset: 0x0025F95F
	public virtual void Init(World _world, IBiomeProvider _biomeProvider, int _seed)
	{
		this.biomeProvider = _biomeProvider;
		this.perlinNoise = new PerlinNoise(_seed);
	}

	// Token: 0x06005FFB RID: 24571
	public abstract byte GetTerrainHeightByteAt(int _xWorld, int _zWorld);

	// Token: 0x06005FFC RID: 24572 RVA: 0x0003D372 File Offset: 0x0003B572
	public virtual float GetTerrainHeightAt(int x, int z)
	{
		return 0f;
	}

	// Token: 0x06005FFD RID: 24573 RVA: 0x00108406 File Offset: 0x00106606
	public virtual Vector3 GetTerrainNormalAt(int _xWorld, int _zWorld, BiomeDefinition _bd, float _biomeIntensity)
	{
		return Vector3.up;
	}

	// Token: 0x06005FFE RID: 24574
	public abstract sbyte GetDensityAt(int _xWorld, int _yWorld, int _zWorld, BiomeDefinition _bd, float _biomeIntensity);

	// Token: 0x06005FFF RID: 24575 RVA: 0x00261774 File Offset: 0x0025F974
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void fillDensityInBlock(Chunk _chunk, int _x, int _y, int _z, BlockValue _bv)
	{
		sbyte b = (_bv.Block.shape.IsTerrain() ? MarchingCubes.DensityTerrain : MarchingCubes.DensityAir);
		_chunk.SetDensity(_x, _y, _z, b);
	}

	// Token: 0x06006000 RID: 24576 RVA: 0x002617AC File Offset: 0x0025F9AC
	public virtual void GenerateTerrain(World _world, Chunk _chunk, GameRandom _random)
	{
		this.GenerateTerrain(_world, _chunk, _random, Vector3i.zero, Vector3i.zero, false, false);
	}

	// Token: 0x06006001 RID: 24577 RVA: 0x002617C4 File Offset: 0x0025F9C4
	public virtual void GenerateTerrain(World _world, Chunk _chunk, GameRandom _random, Vector3i _areaStart, Vector3i _areaSize, bool _bFillEmptyBlocks, bool _isReset)
	{
		int num = 0;
		int num2 = 16;
		int num3 = 0;
		int num4 = 16;
		if (_areaSize.x > 0 && _areaSize.z > 0)
		{
			Vector3i vector3i = _chunk.ToWorldPos(Vector3i.zero);
			Vector3i vector3i2 = vector3i + new Vector3i(16, 0, 16);
			if (vector3i2.x <= _areaStart.x || vector3i.x > _areaStart.x + _areaSize.x || vector3i2.z <= _areaStart.z || vector3i.z > _areaStart.z + _areaSize.z)
			{
				return;
			}
			num = _areaStart.x - vector3i.x;
			num2 = vector3i2.x - (_areaStart.x + _areaSize.x);
			num3 = _areaStart.z - vector3i.z;
			num4 = vector3i2.z - (_areaStart.z + _areaSize.z);
			if (num < 0)
			{
				num = 0;
			}
			if (num3 < 0)
			{
				num3 = 0;
			}
			if (num2 < 0)
			{
				num2 = 16;
			}
			else
			{
				num2 = 16 - num2;
			}
			if (num4 < 0)
			{
				num4 = 16;
			}
			else
			{
				num4 = 16 - num4;
			}
		}
		for (int i = num3; i < num4; i++)
		{
			int blockWorldPosZ = _chunk.GetBlockWorldPosZ(i);
			for (int j = num; j < num2; j++)
			{
				int blockWorldPosX = _chunk.GetBlockWorldPosX(j);
				BiomeDefinition biomeDefinition = this.biomeProvider.GetBiomeAt(blockWorldPosX, blockWorldPosZ);
				if (biomeDefinition != null)
				{
					_chunk.SetBiomeId(j, i, biomeDefinition.m_Id);
					byte terrainHeightByteAt = this.GetTerrainHeightByteAt(blockWorldPosX, blockWorldPosZ);
					_chunk.SetTerrainHeight(j, i, terrainHeightByteAt);
					_chunk.SetHeight(j, i, terrainHeightByteAt);
					int subBiomeIdxAt = this.biomeProvider.GetSubBiomeIdxAt(biomeDefinition, blockWorldPosX, (int)terrainHeightByteAt, blockWorldPosZ);
					if (subBiomeIdxAt >= 0 && subBiomeIdxAt < biomeDefinition.subbiomes.Count)
					{
						biomeDefinition = biomeDefinition.subbiomes[subBiomeIdxAt];
					}
					int num5 = (int)((terrainHeightByteAt + 1) & byte.MaxValue);
					if (_bFillEmptyBlocks)
					{
						for (int k = 255; k >= num5; k--)
						{
							_chunk.SetBlockRaw(j, k, i, BlockValue.Air);
							_chunk.SetDensity(j, k, i, MarchingCubes.DensityAir);
						}
					}
					this.fillDensityInBlock(_chunk, j, num5, i, BlockValue.Air);
					num5--;
					if (num5 >= 0)
					{
						int count = biomeDefinition.m_Layers.Count;
						int num6 = count - 1;
						int depth = biomeDefinition.m_Layers[num6].m_Depth;
						int num7 = 0;
						while (num7 < count && num5 >= 0)
						{
							BiomeLayer biomeLayer = biomeDefinition.m_Layers[num7];
							int num8 = biomeLayer.m_Depth;
							if (num8 < 0)
							{
								num8 = (int)terrainHeightByteAt - biomeDefinition.TotalLayerDepth;
							}
							int num9 = 0;
							while (num9 < num8 && num5 >= 0)
							{
								if (num5 < depth && num7 != num6)
								{
									num7 = num6 - 1;
									break;
								}
								BlockValue blockValue = BlockValue.Air;
								int count2 = biomeLayer.m_Resources.Count;
								if (count2 > 0 && GameUtils.GetOreNoiseAt(this.perlinNoise, blockWorldPosX, num5, blockWorldPosZ) > 0f)
								{
									float randomFloat = _random.RandomFloat;
									for (int l = 0; l < count2; l++)
									{
										if (randomFloat < biomeLayer.SumResourceProbs[l])
										{
											blockValue = biomeLayer.m_Resources[l].blockValues[0];
											break;
										}
									}
								}
								if (blockValue.isair)
								{
									int num10 = biomeLayer.m_Block.blockValues.Length - 1;
									if (num10 > 0)
									{
										num10 = _random.RandomRange(0, num10 + 1);
									}
									blockValue = biomeLayer.m_Block.blockValues[num10];
								}
								int blockId = _chunk.GetBlockId(j, num5, i);
								if (blockId >= 256 && Block.list[blockId].shape is BlockShapeModelEntity)
								{
									_chunk.SetBlock(_world, j, num5, i, blockValue, true, true, _isReset, _isReset, -1);
								}
								else
								{
									_chunk.SetBlockRaw(j, num5, i, blockValue);
								}
								this.fillDensityInBlock(_chunk, j, num5, i, blockValue);
								num9++;
								num5--;
							}
							num7++;
						}
					}
				}
			}
		}
	}

	// Token: 0x06006002 RID: 24578 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public TerrainGeneratorWithBiomeResource()
	{
	}

	// Token: 0x04004B62 RID: 19298
	[PublicizedFrom(EAccessModifier.Private)]
	public IBiomeProvider biomeProvider;

	// Token: 0x04004B63 RID: 19299
	[PublicizedFrom(EAccessModifier.Private)]
	public PerlinNoise perlinNoise;
}
