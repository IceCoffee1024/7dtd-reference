using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AF0 RID: 2800
public class SpawnManagerBiomes : SpawnManagerAbstract
{
	// Token: 0x06005376 RID: 21366 RVA: 0x001FDE1D File Offset: 0x001FC01D
	public SpawnManagerBiomes(World _world)
		: base(_world)
	{
		_world.EntityUnloadedDelegates += this.OnEntityUnloaded;
	}

	// Token: 0x06005377 RID: 21367 RVA: 0x001FDE4E File Offset: 0x001FC04E
	public void Cleanup()
	{
		this.world.EntityUnloadedDelegates -= this.OnEntityUnloaded;
		this.world = null;
	}

	// Token: 0x06005378 RID: 21368 RVA: 0x001FDE70 File Offset: 0x001FC070
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEntityUnloaded(Entity entity, EnumRemoveEntityReason _reason)
	{
		if (_reason == EnumRemoveEntityReason.Undef || _reason == EnumRemoveEntityReason.Unloaded)
		{
			return;
		}
		if (entity.GetSpawnerSource() != EnumSpawnerSource.Biome)
		{
			return;
		}
		Chunk chunk = (Chunk)this.world.GetChunkSync(entity.GetSpawnerSourceChunkKey());
		if (chunk == null)
		{
			return;
		}
		ChunkAreaBiomeSpawnData chunkBiomeSpawnData = chunk.GetChunkBiomeSpawnData();
		if (chunkBiomeSpawnData == null)
		{
			return;
		}
		int spawnerSourceBiomeIdHash = entity.GetSpawnerSourceBiomeIdHash();
		if (_reason == EnumRemoveEntityReason.Despawned)
		{
			chunkBiomeSpawnData.DecCount(spawnerSourceBiomeIdHash, false);
			return;
		}
		if (_reason == EnumRemoveEntityReason.Killed)
		{
			EntityHuman entityHuman = entity as EntityHuman;
			if (entityHuman && this.world.worldTime >= entityHuman.timeToDie)
			{
				chunkBiomeSpawnData.DecCount(spawnerSourceBiomeIdHash, false);
				return;
			}
			chunkBiomeSpawnData.DecCount(spawnerSourceBiomeIdHash, true);
		}
	}

	// Token: 0x06005379 RID: 21369 RVA: 0x001FDF00 File Offset: 0x001FC100
	public override void Update(string _spawnerName, bool _bSpawnEnemyEntities, object _userData)
	{
		if (!GameUtils.IsPlaytesting())
		{
			this.SpawnUpdate(_spawnerName, _bSpawnEnemyEntities, _userData as ChunkAreaBiomeSpawnData);
		}
	}

	// Token: 0x0600537A RID: 21370 RVA: 0x001FDF18 File Offset: 0x001FC118
	[PublicizedFrom(EAccessModifier.Private)]
	public void SpawnUpdate(string _spawnerName, bool _isSpawnEnemy, ChunkAreaBiomeSpawnData _spawnData)
	{
		if (_spawnData == null)
		{
			return;
		}
		if (_isSpawnEnemy)
		{
			if (!AIDirector.CanSpawn(1f))
			{
				_isSpawnEnemy = false;
			}
			else if (this.world.aiDirector.BloodMoonComponent.BloodMoonActive)
			{
				_isSpawnEnemy = false;
			}
		}
		if (!_isSpawnEnemy && GameStats.GetInt(EnumGameStats.AnimalCount) >= GamePrefs.GetInt(EnumGamePrefs.MaxSpawnedAnimals))
		{
			return;
		}
		bool flag = false;
		List<EntityPlayer> players = this.world.GetPlayers();
		for (int i = 0; i < players.Count; i++)
		{
			EntityPlayer entityPlayer = players[i];
			if (entityPlayer.Spawned)
			{
				Rect rect = new Rect(entityPlayer.position.x - 40f, entityPlayer.position.z - 40f, 80f, 80f);
				if (rect.Overlaps(_spawnData.area))
				{
					flag = true;
					break;
				}
			}
		}
		if (!flag)
		{
			return;
		}
		int num = (_isSpawnEnemy ? 28 : 48);
		int num2 = (_isSpawnEnemy ? 54 : 70);
		Vector3 vector;
		Chunk chunk;
		if (!this.world.GetRandomSpawnPositionInAreaMinMaxToPlayers(_spawnData.area, num, num2, true, out vector, out chunk))
		{
			return;
		}
		BiomeDefinition biome = this.world.Biomes.GetBiome(_spawnData.biomeId);
		if (biome == null)
		{
			return;
		}
		BiomeSpawnEntityGroupList biomeSpawnEntityGroupList = BiomeSpawningClass.list[biome.m_sBiomeName];
		if (biomeSpawnEntityGroupList == null)
		{
			return;
		}
		EDaytime edaytime = (this.world.IsDaytime() ? EDaytime.Day : EDaytime.Night);
		GameRandom gameRandom = this.world.GetGameRandom();
		if (!_spawnData.checkedPOITags)
		{
			_spawnData.checkedPOITags = true;
			FastTags<TagGroup.Poi> fastTags = FastTags<TagGroup.Poi>.none;
			Vector3i worldPos = _spawnData.chunk.GetWorldPos();
			this.world.GetPOIsAtXZ(worldPos.x + 16, worldPos.x + 80 - 16, worldPos.z + 16, worldPos.z + 80 - 16, this.spawnPIs);
			for (int j = 0; j < this.spawnPIs.Count; j++)
			{
				PrefabInstance prefabInstance = this.spawnPIs[j];
				fastTags |= prefabInstance.prefab.Tags;
			}
			_spawnData.poiTags = fastTags;
			bool isEmpty = fastTags.IsEmpty;
			for (int k = 0; k < biomeSpawnEntityGroupList.list.Count; k++)
			{
				BiomeSpawnEntityGroupData biomeSpawnEntityGroupData = biomeSpawnEntityGroupList.list[k];
				if ((biomeSpawnEntityGroupData.POITags.IsEmpty || biomeSpawnEntityGroupData.POITags.Test_AnySet(fastTags)) && (isEmpty || biomeSpawnEntityGroupData.noPOITags.IsEmpty || !biomeSpawnEntityGroupData.noPOITags.Test_AnySet(fastTags)))
				{
					_spawnData.groupsEnabledFlags |= 1 << k;
				}
			}
		}
		int idHash = 0;
		int num3 = -1;
		int num4 = gameRandom.RandomRange(biomeSpawnEntityGroupList.list.Count);
		int num5 = Utils.FastMin(5, biomeSpawnEntityGroupList.list.Count);
		int l = 0;
		while (l < num5)
		{
			BiomeSpawnEntityGroupData biomeSpawnEntityGroupData2 = biomeSpawnEntityGroupList.list[num4];
			if ((_spawnData.groupsEnabledFlags & (1 << num4)) != 0 && (biomeSpawnEntityGroupData2.daytime == EDaytime.Any || biomeSpawnEntityGroupData2.daytime == edaytime))
			{
				bool flag2 = EntityGroups.IsEnemyGroup(biomeSpawnEntityGroupData2.entityGroupName);
				if (!flag2 || _isSpawnEnemy)
				{
					idHash = biomeSpawnEntityGroupData2.idHash;
					ulong delayWorldTime = _spawnData.GetDelayWorldTime(idHash);
					if (this.world.worldTime > delayWorldTime)
					{
						int num6 = biomeSpawnEntityGroupData2.maxCount;
						if (flag2)
						{
							num6 = EntitySpawner.ModifySpawnCountByGameDifficulty(num6);
						}
						_spawnData.ResetRespawn(idHash, this.world, num6);
					}
					if (_spawnData.CanSpawn(idHash))
					{
						num3 = num4;
						break;
					}
				}
			}
			l++;
			num4 = (num4 + 1) % biomeSpawnEntityGroupList.list.Count;
		}
		if (num3 < 0)
		{
			return;
		}
		Bounds bounds = new Bounds(vector, new Vector3(4f, 2.5f, 4f));
		this.world.GetEntitiesInBounds(typeof(Entity), bounds, this.spawnNearList);
		int count = this.spawnNearList.Count;
		this.spawnNearList.Clear();
		if (count > 0)
		{
			return;
		}
		int randomFromGroup = EntityGroups.GetRandomFromGroup(biomeSpawnEntityGroupList.list[num3].entityGroupName, ref this.lastClassId, null);
		if (randomFromGroup == 0)
		{
			_spawnData.DecMaxCount(idHash);
			return;
		}
		EntityClass entityClass;
		if (!EntityClass.list.TryGetValue(randomFromGroup, out entityClass))
		{
			Log.Error(string.Format("SpawnManagerBiomes invalid entity id {0}", randomFromGroup));
			return;
		}
		_spawnData.IncCount(idHash);
		EntityCreationData entityCreationData = EntityFactory.SetupEntityCreationData(randomFromGroup, vector);
		chunk.SpawnEntityAsync(this.world, entityCreationData, delegate(Entity entity)
		{
			this.OnEntitySpawned(entity, _spawnData.chunk.Key, idHash);
		});
	}

	// Token: 0x0600537B RID: 21371 RVA: 0x001FE402 File Offset: 0x001FC602
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEntitySpawned(Entity _entity, long _masterChunkKey, int _biomeIdHash)
	{
		_entity.SetSpawnerSource(EnumSpawnerSource.Biome, _masterChunkKey, _biomeIdHash);
		this.world.DebugAddSpawnedEntity(_entity);
	}

	// Token: 0x0600537C RID: 21372 RVA: 0x001FE419 File Offset: 0x001FC619
	[PublicizedFrom(EAccessModifier.Private)]
	public void LogSpawn(string format, params object[] args)
	{
		format = string.Format("{0} SpawnManagerBiomes {1}", GameManager.frameCount, format);
		Log.Warning(format, args);
	}

	// Token: 0x0600537D RID: 21373 RVA: 0x001FE439 File Offset: 0x001FC639
	[PublicizedFrom(EAccessModifier.Private)]
	public void LogSpawn(Entity _entity, string format, params object[] args)
	{
		format = string.Format("{0} SpawnManagerBiomes {1}, {2}", GameManager.frameCount, _entity ? _entity.ToString() : "", format);
		Log.Warning(format, args);
	}

	// Token: 0x0600537E RID: 21374 RVA: 0x001FE470 File Offset: 0x001FC670
	[PublicizedFrom(EAccessModifier.Private)]
	public void LogSpawn(EntityCreationData _ecd, string format, params object[] args)
	{
		format = string.Format("{0} SpawnManagerBiomes {1} {2}, {3}", new object[]
		{
			GameManager.frameCount,
			_ecd.id,
			EntityClass.GetEntityClass(_ecd.entityClass).entityClassName,
			format
		});
		Log.Warning(format, args);
	}

	// Token: 0x04004139 RID: 16697
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cAnimalMinDistance = 48;

	// Token: 0x0400413A RID: 16698
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cAnimalMaxDistance = 70;

	// Token: 0x0400413B RID: 16699
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cEnemyMinDistance = 28;

	// Token: 0x0400413C RID: 16700
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cEnemyMaxDistance = 54;

	// Token: 0x0400413D RID: 16701
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Entity> spawnNearList = new List<Entity>();

	// Token: 0x0400413E RID: 16702
	[PublicizedFrom(EAccessModifier.Private)]
	public int lastClassId;

	// Token: 0x0400413F RID: 16703
	[PublicizedFrom(EAccessModifier.Private)]
	public List<PrefabInstance> spawnPIs = new List<PrefabInstance>();
}
