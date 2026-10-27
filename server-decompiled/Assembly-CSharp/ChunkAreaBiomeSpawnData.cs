using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

// Token: 0x02000B25 RID: 2853
public class ChunkAreaBiomeSpawnData
{
	// Token: 0x0600553F RID: 21823 RVA: 0x0020C570 File Offset: 0x0020A770
	public ChunkAreaBiomeSpawnData(Chunk _chunk, byte _biomeId, ChunkCustomData _ccd)
	{
		this.biomeId = _biomeId;
		this.area = new Rect((float)(_chunk.X * 16), (float)(_chunk.Z * 16), 80f, 80f);
		this.chunk = _chunk;
		this.ccd = _ccd;
		this.ccd.TriggerWriteDataDelegate = new ChunkCustomData.TriggerWriteData(this.BeforeWrite);
		if (this.ccd.data != null)
		{
			using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
			{
				pooledBinaryReader.SetBaseStream(new MemoryStream(this.ccd.data));
				this.read(pooledBinaryReader);
			}
		}
	}

	// Token: 0x06005540 RID: 21824 RVA: 0x0020C638 File Offset: 0x0020A838
	public bool IsSpawnNeeded(WorldBiomes _worldBiomes, ulong _worldTime)
	{
		BiomeDefinition biome = _worldBiomes.GetBiome(this.biomeId);
		if (biome == null)
		{
			return false;
		}
		BiomeSpawnEntityGroupList biomeSpawnEntityGroupList = BiomeSpawningClass.list[biome.m_sBiomeName];
		if (biomeSpawnEntityGroupList == null)
		{
			return false;
		}
		for (int i = 0; i < biomeSpawnEntityGroupList.list.Count; i++)
		{
			BiomeSpawnEntityGroupData biomeSpawnEntityGroupData = biomeSpawnEntityGroupList.list[i];
			ChunkAreaBiomeSpawnData.CountsAndTime countsAndTime;
			if (!this.entitesSpawned.TryGetValue(biomeSpawnEntityGroupData.idHash, out countsAndTime))
			{
				return true;
			}
			if (countsAndTime.count < countsAndTime.maxCount || _worldTime > countsAndTime.delayWorldTime)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06005541 RID: 21825 RVA: 0x0020C6C4 File Offset: 0x0020A8C4
	public bool CanSpawn(int _idHash)
	{
		ChunkAreaBiomeSpawnData.CountsAndTime countsAndTime;
		return this.entitesSpawned.TryGetValue(_idHash, out countsAndTime) && countsAndTime.count < countsAndTime.maxCount;
	}

	// Token: 0x06005542 RID: 21826 RVA: 0x0020C6F4 File Offset: 0x0020A8F4
	public void SetCounts(int _idHash, int _count, int _maxCount)
	{
		ChunkAreaBiomeSpawnData.CountsAndTime countsAndTime;
		this.entitesSpawned.TryGetValue(_idHash, out countsAndTime);
		countsAndTime.count = _count;
		countsAndTime.maxCount = _maxCount;
		this.entitesSpawned[_idHash] = countsAndTime;
	}

	// Token: 0x06005543 RID: 21827 RVA: 0x0020C730 File Offset: 0x0020A930
	public void IncCount(int _idHash)
	{
		ChunkAreaBiomeSpawnData.CountsAndTime countsAndTime;
		if (!this.entitesSpawned.TryGetValue(_idHash, out countsAndTime))
		{
			countsAndTime.count = 1;
		}
		else
		{
			countsAndTime.count++;
		}
		this.entitesSpawned[_idHash] = countsAndTime;
		this.chunk.isModified = true;
	}

	// Token: 0x06005544 RID: 21828 RVA: 0x0020C77C File Offset: 0x0020A97C
	public void DecCount(int _idHash, bool _killed)
	{
		ChunkAreaBiomeSpawnData.CountsAndTime countsAndTime;
		if (this.entitesSpawned.TryGetValue(_idHash, out countsAndTime))
		{
			countsAndTime.count = Utils.FastMax(countsAndTime.count - 1, 0);
			if (_killed)
			{
				countsAndTime.maxCount = Utils.FastMax(0, countsAndTime.maxCount - 1);
			}
			this.entitesSpawned[_idHash] = countsAndTime;
			this.chunk.isModified = true;
		}
	}

	// Token: 0x06005545 RID: 21829 RVA: 0x0020C7E0 File Offset: 0x0020A9E0
	public void DecMaxCount(int _idHash)
	{
		ChunkAreaBiomeSpawnData.CountsAndTime countsAndTime;
		if (this.entitesSpawned.TryGetValue(_idHash, out countsAndTime))
		{
			countsAndTime.maxCount = Utils.FastMax(0, countsAndTime.maxCount - 1);
			this.entitesSpawned[_idHash] = countsAndTime;
			this.chunk.isModified = true;
		}
	}

	// Token: 0x06005546 RID: 21830 RVA: 0x0020C82C File Offset: 0x0020AA2C
	public ulong GetDelayWorldTime(int _idHash)
	{
		ChunkAreaBiomeSpawnData.CountsAndTime countsAndTime;
		this.entitesSpawned.TryGetValue(_idHash, out countsAndTime);
		return countsAndTime.delayWorldTime;
	}

	// Token: 0x06005547 RID: 21831 RVA: 0x0020C850 File Offset: 0x0020AA50
	public void ResetRespawn(int _idHash, World _world, int _maxCount)
	{
		BiomeDefinition biome = _world.Biomes.GetBiome(this.biomeId);
		if (biome == null)
		{
			return;
		}
		BiomeSpawnEntityGroupList biomeSpawnEntityGroupList = BiomeSpawningClass.list[biome.m_sBiomeName];
		if (biomeSpawnEntityGroupList == null)
		{
			return;
		}
		BiomeSpawnEntityGroupData biomeSpawnEntityGroupData = biomeSpawnEntityGroupList.Find(_idHash);
		if (biomeSpawnEntityGroupData == null)
		{
			return;
		}
		ChunkAreaBiomeSpawnData.CountsAndTime countsAndTime;
		this.entitesSpawned.TryGetValue(_idHash, out countsAndTime);
		int num = 0;
		bool flag = _world.IsDaytime();
		if (biomeSpawnEntityGroupData.respawnDelayInWorldTime.Length > 1)
		{
			if (biomeSpawnEntityGroupData.type == BiomeSpawnEntityGroupData.eType.Animal)
			{
				num = (flag ? ChunkAreaBiomeSpawnData.RespawnDayDelayIndexAnimals : ChunkAreaBiomeSpawnData.RespawnNightDelayIndexAnimals);
			}
			else
			{
				num = (flag ? ChunkAreaBiomeSpawnData.RespawnDayDelayIndexEnemies : ChunkAreaBiomeSpawnData.RespawnNightDelayIndexEnemies);
			}
		}
		if (_maxCount > 0)
		{
			if (flag)
			{
				if (ChunkAreaBiomeSpawnData.RespawnDayEnemyCountOverride == -1)
				{
					_maxCount = 0;
				}
				else if (biomeSpawnEntityGroupData.type != BiomeSpawnEntityGroupData.eType.Rare)
				{
					if (ChunkAreaBiomeSpawnData.RespawnDayEnemyCountOverride > 0 && EntityGroups.IsEnemyGroup(biomeSpawnEntityGroupData.entityGroupName))
					{
						_maxCount = ChunkAreaBiomeSpawnData.RespawnDayEnemyCountOverride;
					}
					else if (ChunkAreaBiomeSpawnData.RespawnDayAnimalCountOverride > 0 && !EntityGroups.IsEnemyGroup(biomeSpawnEntityGroupData.entityGroupName))
					{
						_maxCount = ChunkAreaBiomeSpawnData.RespawnDayAnimalCountOverride;
					}
				}
			}
			else if (ChunkAreaBiomeSpawnData.RespawnNightEnemyCountOverride == -1)
			{
				_maxCount = 0;
			}
			else if (ChunkAreaBiomeSpawnData.RespawnNightEnemyCountOverride > 0 && EntityGroups.IsEnemyGroup(biomeSpawnEntityGroupData.entityGroupName))
			{
				_maxCount = ChunkAreaBiomeSpawnData.RespawnNightEnemyCountOverride;
			}
			else if (ChunkAreaBiomeSpawnData.RespawnNightAnimalCountOverride > 0 && !EntityGroups.IsEnemyGroup(biomeSpawnEntityGroupData.entityGroupName))
			{
				_maxCount = ChunkAreaBiomeSpawnData.RespawnNightAnimalCountOverride;
			}
		}
		countsAndTime.delayWorldTime = _world.worldTime + (ulong)((float)biomeSpawnEntityGroupData.respawnDelayInWorldTime[num] * _world.RandomRange(0.9f, 1.1f));
		countsAndTime.maxCount = _maxCount;
		this.entitesSpawned[_idHash] = countsAndTime;
		this.chunk.isModified = true;
	}

	// Token: 0x06005548 RID: 21832 RVA: 0x0020C9E0 File Offset: 0x0020ABE0
	public bool DelayAllEnemySpawningUntil(ulong _worldTime, WorldBiomes _worldBiomes)
	{
		bool flag = false;
		BiomeDefinition biome = _worldBiomes.GetBiome(this.biomeId);
		if (biome == null)
		{
			return false;
		}
		BiomeSpawnEntityGroupList biomeSpawnEntityGroupList = BiomeSpawningClass.list[biome.m_sBiomeName];
		if (biomeSpawnEntityGroupList == null)
		{
			return false;
		}
		Dictionary<int, ChunkAreaBiomeSpawnData.CountsAndTime> dictionary = new Dictionary<int, ChunkAreaBiomeSpawnData.CountsAndTime>();
		foreach (KeyValuePair<int, ChunkAreaBiomeSpawnData.CountsAndTime> keyValuePair in this.entitesSpawned)
		{
			BiomeSpawnEntityGroupData biomeSpawnEntityGroupData = biomeSpawnEntityGroupList.Find(keyValuePair.Key);
			if (biomeSpawnEntityGroupData != null && EntityGroups.IsEnemyGroup(biomeSpawnEntityGroupData.entityGroupName))
			{
				ChunkAreaBiomeSpawnData.CountsAndTime value = keyValuePair.Value;
				bool flag2 = false;
				if (value.delayWorldTime < _worldTime)
				{
					value.delayWorldTime = _worldTime;
					flag2 = true;
				}
				if (value.maxCount > 0)
				{
					value.maxCount = 0;
					flag2 = true;
				}
				if (flag2)
				{
					dictionary[keyValuePair.Key] = value;
					flag = true;
				}
			}
		}
		foreach (KeyValuePair<int, ChunkAreaBiomeSpawnData.CountsAndTime> keyValuePair2 in dictionary)
		{
			this.entitesSpawned[keyValuePair2.Key] = keyValuePair2.Value;
		}
		for (int i = 0; i < biomeSpawnEntityGroupList.list.Count; i++)
		{
			BiomeSpawnEntityGroupData biomeSpawnEntityGroupData2 = biomeSpawnEntityGroupList.list[i];
			if (EntityGroups.IsEnemyGroup(biomeSpawnEntityGroupData2.entityGroupName) && !this.entitesSpawned.ContainsKey(biomeSpawnEntityGroupData2.idHash))
			{
				this.entitesSpawned[biomeSpawnEntityGroupData2.idHash] = new ChunkAreaBiomeSpawnData.CountsAndTime(0, 0, _worldTime);
				flag = true;
			}
		}
		return flag;
	}

	// Token: 0x06005549 RID: 21833 RVA: 0x0020CB84 File Offset: 0x0020AD84
	[PublicizedFrom(EAccessModifier.Private)]
	public void read(BinaryReader _br)
	{
		int num = (int)_br.ReadByte();
		this.entitesSpawned.Clear();
		int num2 = (int)_br.ReadByte();
		for (int i = 0; i < num2; i++)
		{
			if (num <= 1)
			{
				_br.ReadString();
				_br.ReadUInt16();
				_br.ReadUInt64();
			}
			else
			{
				int num3 = _br.ReadInt32();
				int num4 = (int)_br.ReadUInt16();
				ChunkAreaBiomeSpawnData.CountsAndTime countsAndTime;
				countsAndTime.count = num4 & 255;
				countsAndTime.maxCount = num4 >> 8;
				countsAndTime.delayWorldTime = _br.ReadUInt64();
				this.entitesSpawned[num3] = countsAndTime;
			}
		}
	}

	// Token: 0x0600554A RID: 21834 RVA: 0x0020CC18 File Offset: 0x0020AE18
	public void BeforeWrite()
	{
		using (PooledExpandableMemoryStream pooledExpandableMemoryStream = MemoryPools.poolMemoryStream.AllocSync(true))
		{
			using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
			{
				pooledBinaryWriter.SetBaseStream(pooledExpandableMemoryStream);
				this.write(pooledBinaryWriter);
			}
			this.ccd.data = pooledExpandableMemoryStream.ToArray();
		}
	}

	// Token: 0x0600554B RID: 21835 RVA: 0x0020CC90 File Offset: 0x0020AE90
	[PublicizedFrom(EAccessModifier.Private)]
	public void write(BinaryWriter _bw)
	{
		_bw.Write(2);
		int num = 0;
		int num2 = Utils.FastMin(this.entitesSpawned.Count, 255);
		_bw.Write((byte)num2);
		foreach (KeyValuePair<int, ChunkAreaBiomeSpawnData.CountsAndTime> keyValuePair in this.entitesSpawned)
		{
			_bw.Write(keyValuePair.Key);
			_bw.Write((ushort)((keyValuePair.Value.maxCount << 8) | keyValuePair.Value.count));
			_bw.Write(keyValuePair.Value.delayWorldTime);
			if (++num >= num2)
			{
				break;
			}
		}
	}

	// Token: 0x0600554C RID: 21836 RVA: 0x0020CD50 File Offset: 0x0020AF50
	public override string ToString()
	{
		World world = GameManager.Instance.World;
		ulong worldTime = world.worldTime;
		BiomeDefinition biome = world.Biomes.GetBiome(this.biomeId);
		if (biome == null)
		{
			return "biome? " + this.biomeId.ToString();
		}
		BiomeSpawnEntityGroupList biomeSpawnEntityGroupList = BiomeSpawningClass.list[biome.m_sBiomeName];
		StringBuilder stringBuilder = new StringBuilder();
		foreach (KeyValuePair<int, ChunkAreaBiomeSpawnData.CountsAndTime> keyValuePair in this.entitesSpawned)
		{
			string text = "?";
			if (biomeSpawnEntityGroupList != null)
			{
				BiomeSpawnEntityGroupData biomeSpawnEntityGroupData = biomeSpawnEntityGroupList.Find(keyValuePair.Key);
				if (biomeSpawnEntityGroupData != null)
				{
					text = biomeSpawnEntityGroupData.entityGroupName + " " + biomeSpawnEntityGroupData.daytime.ToString();
				}
			}
			ulong num = keyValuePair.Value.delayWorldTime - worldTime;
			if (num < 0UL)
			{
				num = 0UL;
			}
			stringBuilder.Append(string.Format("{0} #{1}/{2} {3}, ", new object[]
			{
				text,
				keyValuePair.Value.count,
				keyValuePair.Value.maxCount,
				GameUtils.WorldTimeDeltaToString(num)
			}));
		}
		return string.Format("biomeId {0}, XZ {1} {2}: {3}", new object[]
		{
			this.biomeId,
			this.area.x.ToCultureInvariantString("0"),
			this.area.y.ToCultureInvariantString("0"),
			stringBuilder.ToString()
		});
	}

	// Token: 0x04004273 RID: 17011
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cCurrentSaveVersion = 2;

	// Token: 0x04004274 RID: 17012
	public byte biomeId;

	// Token: 0x04004275 RID: 17013
	public Rect area;

	// Token: 0x04004276 RID: 17014
	public Chunk chunk;

	// Token: 0x04004277 RID: 17015
	public bool checkedPOITags;

	// Token: 0x04004278 RID: 17016
	public FastTags<TagGroup.Poi> poiTags;

	// Token: 0x04004279 RID: 17017
	public int groupsEnabledFlags;

	// Token: 0x0400427A RID: 17018
	public static int RespawnDayDelayIndexEnemies;

	// Token: 0x0400427B RID: 17019
	public static int RespawnDayDelayIndexAnimals;

	// Token: 0x0400427C RID: 17020
	public static int RespawnDayEnemyCountOverride;

	// Token: 0x0400427D RID: 17021
	public static int RespawnDayAnimalCountOverride;

	// Token: 0x0400427E RID: 17022
	public static int RespawnNightDelayIndexEnemies;

	// Token: 0x0400427F RID: 17023
	public static int RespawnNightDelayIndexAnimals;

	// Token: 0x04004280 RID: 17024
	public static int RespawnNightEnemyCountOverride;

	// Token: 0x04004281 RID: 17025
	public static int RespawnNightAnimalCountOverride;

	// Token: 0x04004282 RID: 17026
	[PublicizedFrom(EAccessModifier.Private)]
	public ChunkCustomData ccd;

	// Token: 0x04004283 RID: 17027
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<int, ChunkAreaBiomeSpawnData.CountsAndTime> entitesSpawned = new Dictionary<int, ChunkAreaBiomeSpawnData.CountsAndTime>();

	// Token: 0x02000B26 RID: 2854
	[PublicizedFrom(EAccessModifier.Private)]
	public struct CountsAndTime
	{
		// Token: 0x0600554D RID: 21837 RVA: 0x0020CEF8 File Offset: 0x0020B0F8
		public CountsAndTime(int _count, int _maxCount, ulong _delayWorldTime)
		{
			this.count = _count;
			this.maxCount = _maxCount;
			this.delayWorldTime = _delayWorldTime;
		}

		// Token: 0x0600554E RID: 21838 RVA: 0x0020CF0F File Offset: 0x0020B10F
		public override string ToString()
		{
			return string.Format("cnt {0}, maxCnt {1}, wtime {2}", this.count, this.maxCount, this.delayWorldTime);
		}

		// Token: 0x04004284 RID: 17028
		public int count;

		// Token: 0x04004285 RID: 17029
		public int maxCount;

		// Token: 0x04004286 RID: 17030
		public ulong delayWorldTime;
	}
}
