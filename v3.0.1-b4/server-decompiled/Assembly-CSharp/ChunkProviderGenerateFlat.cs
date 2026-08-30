using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

// Token: 0x02000B1B RID: 2843
public class ChunkProviderGenerateFlat : ChunkProviderAbstract
{
	// Token: 0x0600561B RID: 22043 RVA: 0x00210687 File Offset: 0x0020E887
	public ChunkProviderGenerateFlat(ChunkCluster _cc, string _worldPath)
	{
		this.cc = _cc;
		this.worldPath = _worldPath;
	}

	// Token: 0x0600561C RID: 22044 RVA: 0x0021069D File Offset: 0x0020E89D
	public override IEnumerator Init(World _world)
	{
		this.world = _world;
		GamePrefs.OnGamePrefChanged += this.GamePrefChanged;
		bool flag = false;
		bool createGroundTerrain = false;
		bool createQuestContainerAndTrader = false;
		Prefab prefabToPlace = null;
		Vector3i playerPos = Vector3i.zero;
		if (GameUtils.IsPlaytesting())
		{
			flag = true;
			createGroundTerrain = true;
			createQuestContainerAndTrader = true;
			prefabToPlace = new Prefab();
			prefabToPlace.Load(GamePrefs.GetString(EnumGamePrefs.GameName), true, true, false, false);
			playerPos = new Vector3i(1f, WorldConstants.WaterLevel, (float)(-(float)prefabToPlace.size.z / 2 - 4 - 10));
		}
		MultiBlockManager.Instance.Initialize(null);
		this.prefabDecorator = new DynamicPrefabDecorator(_world.m_PrefabCache);
		if (flag)
		{
			if (SdDirectory.Exists(GameIO.GetSaveGameRegionDir()))
			{
				SdDirectory.Delete(GameIO.GetSaveGameRegionDir(), true);
			}
			if (SdFile.Exists(GameIO.GetSaveGameDir() + "/decoration.7dt"))
			{
				SdFile.Delete(GameIO.GetSaveGameDir() + "/decoration.7dt");
			}
		}
		bool flag2 = _world.IsEditor() || !SdDirectory.Exists(GameIO.GetSaveGameRegionDir());
		this.cc.IsFixedSize = true;
		if (flag2)
		{
			yield return this.prefabDecorator.Load(this.worldPath, false);
			this.prefabDecorator.CopyAllPrefabsIntoWorld(this.world, false);
		}
		this.spawnPointManager = new SpawnPointManager(true);
		this.spawnPointManager.Load(this.worldPath);
		BoundsInt boundsInt;
		if (prefabToPlace != null)
		{
			int num = -1 * prefabToPlace.size.x / 2;
			int num2 = -1 * prefabToPlace.size.z / 2;
			int num3 = num + prefabToPlace.size.x;
			int num4 = num2 + prefabToPlace.size.z;
			Vector3Int vector3Int = new Vector3Int((num - 1) / 16 - 1, 0, (num2 - 1) / 16 - 1);
			vector3Int -= new Vector3Int(ChunkProviderGenerateFlat.prefabChunkBorderSize, 0, ChunkProviderGenerateFlat.prefabChunkBorderSize + 3);
			Vector3Int vector3Int2 = new Vector3Int((num3 + 1) / 16 + 1, 0, (num4 + 1) / 16 + 1);
			vector3Int2 += new Vector3Int(ChunkProviderGenerateFlat.prefabChunkBorderSize, 0, ChunkProviderGenerateFlat.prefabChunkBorderSize);
			boundsInt = new BoundsInt(vector3Int, vector3Int2 - vector3Int);
		}
		else
		{
			boundsInt = new BoundsInt(new Vector3Int(-ChunkProviderGenerateFlat.defaultWorldAreaChunkDim / 2, 0, -ChunkProviderGenerateFlat.defaultWorldAreaChunkDim / 2), new Vector3Int(ChunkProviderGenerateFlat.defaultWorldAreaChunkDim, 0, ChunkProviderGenerateFlat.defaultWorldAreaChunkDim));
		}
		this.world.m_ChunkManager.RemoveAllChunks();
		BiomeDefinition.BiomeType biomeType = (BiomeDefinition.BiomeType)GamePrefs.GetInt(EnumGamePrefs.PlaytestBiome);
		if (biomeType <= BiomeDefinition.BiomeType.Any || biomeType > BiomeDefinition.BiomeType.burnt_forest)
		{
			biomeType = BiomeDefinition.BiomeType.PineForest;
		}
		byte b = (byte)biomeType;
		for (int i = boundsInt.xMin; i <= boundsInt.xMax; i++)
		{
			for (int j = boundsInt.zMin; j <= boundsInt.zMax; j++)
			{
				Chunk chunk = MemoryPools.PoolChunks.AllocSync(true);
				chunk.X = i;
				chunk.Z = j;
				chunk.FillBiomeId(b);
				if (createGroundTerrain)
				{
					chunk.FillBlockRaw((int)WorldConstants.WaterLevel - 1, Block.GetBlockValue("terrainFiller", false));
				}
				chunk.SetFullSunlight();
				chunk.NeedsLightCalculation = false;
				chunk.NeedsDecoration = false;
				chunk.NeedsRegeneration = false;
				this.cc.AddChunkSync(chunk, true);
			}
		}
		List<Chunk> chunkArrayCopySync = this.world.ChunkCache.GetChunkArrayCopySync();
		if (prefabToPlace != null)
		{
			Vector3i vector3i = new Vector3i((float)(-(float)prefabToPlace.size.x / 2), WorldConstants.WaterLevel + (float)prefabToPlace.yOffset, (float)(-(float)prefabToPlace.size.z / 2));
			PrefabInstance prefabInstance = new PrefabInstance(1, prefabToPlace.location, vector3i, 0, prefabToPlace, 0);
			this.prefabDecorator.AddWorldPrefab(prefabInstance, true);
			this.prefabDecorator.CopyAllPrefabsIntoWorld(this.world, false);
			foreach (Chunk chunk2 in chunkArrayCopySync)
			{
				WaterSimulationNative.Instance.InitializeChunk(chunk2);
			}
		}
		if (playerPos == Vector3i.zero)
		{
			playerPos = new Vector3i(0f, createGroundTerrain ? (WorldConstants.WaterLevel + 1f) : 16f, -10f);
		}
		if (createQuestContainerAndTrader)
		{
			this.world.SetBlock(playerPos + new Vector3i(1, 0, 3), Block.GetBlockValue("cntQuestTestLoot", false), false, false);
			Vector3i vector3i2 = playerPos + new Vector3i(-3, 0, 3);
			if (this.world.GetChunkFromWorldPos(vector3i2) != null)
			{
				_world.SpawnEntityInWorld(EntityFactory.CreateEntity(new EntityCreationData
				{
					pos = vector3i2,
					rot = new Vector3(0f, 180f, 0f),
					entityClass = EntityClass.FromString("npcTraderTest")
				}));
			}
		}
		foreach (Chunk chunk3 in chunkArrayCopySync)
		{
			chunk3.ResetStability();
			chunk3.ResetLights(0);
			chunk3.RefreshSunlight();
			chunk3.NeedsLightCalculation = true;
		}
		this.spawnPointManager.spawnPointList = new SpawnPointList
		{
			new SpawnPoint(playerPos)
		};
		yield return null;
		yield break;
	}

	// Token: 0x0600561D RID: 22045 RVA: 0x002106B4 File Offset: 0x0020E8B4
	[PublicizedFrom(EAccessModifier.Private)]
	public void GamePrefChanged(EnumGamePrefs _pref)
	{
		if (_pref == EnumGamePrefs.PlaytestBiome)
		{
			BiomeDefinition.BiomeType biomeType = (BiomeDefinition.BiomeType)GamePrefs.GetInt(EnumGamePrefs.PlaytestBiome);
			if (biomeType <= BiomeDefinition.BiomeType.Any || biomeType > BiomeDefinition.BiomeType.burnt_forest)
			{
				biomeType = BiomeDefinition.BiomeType.PineForest;
			}
			byte b = (byte)biomeType;
			foreach (Chunk chunk in this.world.ChunkCache.GetChunkArrayCopySync())
			{
				chunk.FillBiomeId(b);
				chunk.ResetStability();
				chunk.ResetLights(0);
				chunk.RefreshSunlight();
				chunk.NeedsLightCalculation = true;
			}
		}
	}

	// Token: 0x0600561E RID: 22046 RVA: 0x0021074C File Offset: 0x0020E94C
	public override DynamicPrefabDecorator GetDynamicPrefabDecorator()
	{
		return this.prefabDecorator;
	}

	// Token: 0x0600561F RID: 22047 RVA: 0x00210754 File Offset: 0x0020E954
	public override SpawnPointList GetSpawnPointList()
	{
		return this.spawnPointManager.spawnPointList;
	}

	// Token: 0x06005620 RID: 22048 RVA: 0x00210761 File Offset: 0x0020E961
	public override void SetSpawnPointList(SpawnPointList _spawnPointList)
	{
		this.spawnPointManager.spawnPointList = _spawnPointList;
	}

	// Token: 0x06005621 RID: 22049 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool GetOverviewMap(Vector2i _startPos, Vector2i _size, Color[] _mapColors)
	{
		return false;
	}

	// Token: 0x06005622 RID: 22050 RVA: 0x0005FC95 File Offset: 0x0005DE95
	public override EnumChunkProviderId GetProviderId()
	{
		return EnumChunkProviderId.FlatWorld;
	}

	// Token: 0x06005623 RID: 22051 RVA: 0x0021076F File Offset: 0x0020E96F
	public override void Cleanup()
	{
		GamePrefs.OnGamePrefChanged -= this.GamePrefChanged;
		Thread.Sleep(200);
		MultiBlockManager.Instance.Cleanup();
	}

	// Token: 0x06005624 RID: 22052 RVA: 0x00210798 File Offset: 0x0020E998
	public override bool GetWorldExtent(out Vector3i _minSize, out Vector3i _maxSize)
	{
		_minSize = new Vector3i(this.world.ChunkCache.ChunkMinPos.x * 16, 0, this.world.ChunkCache.ChunkMinPos.y * 16);
		_maxSize = new Vector3i(this.world.ChunkCache.ChunkMaxPos.x * 16, 255, this.world.ChunkCache.ChunkMaxPos.y * 16);
		return true;
	}

	// Token: 0x06005625 RID: 22053 RVA: 0x00210824 File Offset: 0x0020EA24
	public override void RebuildTerrain(HashSetLong _chunks, Vector3i _areaStart, Vector3i _areaSize, bool _bStopStabilityUpdate, bool _bRegenerateChunk, bool _bFillEmptyBlocks, bool _isReset)
	{
		if (_bStopStabilityUpdate)
		{
			foreach (long num in _chunks)
			{
				Chunk chunkSync = this.cc.GetChunkSync(num);
				if (chunkSync != null)
				{
					chunkSync.StopStabilityCalculation = true;
				}
			}
		}
		ThreadManager.RunCoroutineSync(this.prefabDecorator.Load(this.worldPath, false));
		this.prefabDecorator.CopyAllPrefabsIntoWorld(this.world, true);
		Chunk[] array = new Chunk[8];
		foreach (Chunk chunk in this.world.ChunkCache.GetChunkArrayCopySync())
		{
			chunk.ResetStability();
			chunk.ResetLights(0);
			chunk.RefreshSunlight();
			if (this.cc.GetNeighborChunks(chunk, array))
			{
				this.cc.LightChunk(chunk, array);
			}
			List<TileEntity> list = chunk.GetTileEntities().list;
			for (int i = 0; i < list.Count; i++)
			{
				list[i].Reset(FastTags<TagGroup.Global>.none);
			}
		}
		List<EntityPlayerLocal> localPlayers = this.world.GetLocalPlayers();
		foreach (long num2 in _chunks)
		{
			Chunk chunkSync2 = this.cc.GetChunkSync(num2);
			if (chunkSync2 != null)
			{
				for (int j = 0; j < this.world.m_ChunkManager.m_ObservedEntities.Count; j++)
				{
					if (!this.world.m_ChunkManager.m_ObservedEntities[j].bBuildVisualMeshAround)
					{
						bool flag = false;
						int num3 = 0;
						while (!flag && num3 < localPlayers.Count)
						{
							if (this.world.m_ChunkManager.m_ObservedEntities[j].entityIdToSendChunksTo == localPlayers[num3].entityId)
							{
								flag = true;
							}
							num3++;
						}
						if (!flag && this.world.m_ChunkManager.m_ObservedEntities[j].chunksLoaded.Contains(num2))
						{
							SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageChunk>().Setup(chunkSync2, true), false, this.world.m_ChunkManager.m_ObservedEntities[j].entityIdToSendChunksTo, -1, -1, null, 192, false);
						}
					}
				}
			}
		}
	}

	// Token: 0x0400427D RID: 17021
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly int defaultWorldAreaChunkDim = 32;

	// Token: 0x0400427E RID: 17022
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly int prefabChunkBorderSize = 3;

	// Token: 0x0400427F RID: 17023
	[PublicizedFrom(EAccessModifier.Private)]
	public World world;

	// Token: 0x04004280 RID: 17024
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string worldPath;

	// Token: 0x04004281 RID: 17025
	[PublicizedFrom(EAccessModifier.Private)]
	public DynamicPrefabDecorator prefabDecorator;

	// Token: 0x04004282 RID: 17026
	[PublicizedFrom(EAccessModifier.Private)]
	public IChunkProviderIndicator chunkManager;

	// Token: 0x04004283 RID: 17027
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ChunkCluster cc;

	// Token: 0x04004284 RID: 17028
	[PublicizedFrom(EAccessModifier.Private)]
	public SpawnPointManager spawnPointManager;
}
