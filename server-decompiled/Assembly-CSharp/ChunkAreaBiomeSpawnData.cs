using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

// Token: 0x02000B1A RID: 2842
public class ChunkAreaBiomeSpawnData
{
	// Token: 0x0600551A RID: 21786 RVA: 0x0020AFF4 File Offset: 0x002091F4
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

	// Token: 0x0600551B RID: 21787 RVA: 0x0020B0BC File Offset: 0x002092BC
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

	// Token: 0x0600551C RID: 21788 RVA: 0x0020B148 File Offset: 0x00209348
	public bool CanSpawn(int _idHash)
	{
		ChunkAreaBiomeSpawnData.CountsAndTime countsAndTime;
		return this.entitesSpawned.TryGetValue(_idHash, out countsAndTime) && countsAndTime.count < countsAndTime.maxCount;
	}

	// Token: 0x0600551D RID: 21789 RVA: 0x0020B178 File Offset: 0x00209378
	public void SetCounts(int _idHash, int _count, int _maxCount)
	{
		ChunkAreaBiomeSpawnData.CountsAndTime countsAndTime;
		this.entitesSpawned.TryGetValue(_idHash, out countsAndTime);
		countsAndTime.count = _count;
		countsAndTime.maxCount = _maxCount;
		this.entitesSpawned[_idHash] = countsAndTime;
	}

	// Token: 0x0600551E RID: 21790 RVA: 0x0020B1B4 File Offset: 0x002093B4
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

	// Token: 0x0600551F RID: 21791 RVA: 0x0020B200 File Offset: 0x00209400
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

	// Token: 0x06005520 RID: 21792 RVA: 0x0020B264 File Offset: 0x00209464
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

	// Token: 0x06005521 RID: 21793 RVA: 0x0020B2B0 File Offset: 0x002094B0
	public ulong GetDelayWorldTime(int _idHash)
	{
		ChunkAreaBiomeSpawnData.CountsAndTime countsAndTime;
		this.entitesSpawned.TryGetValue(_idHash, out countsAndTime);
		return countsAndTime.delayWorldTime;
	}

	// Token: 0x06005522 RID: 21794 RVA: 0x0020B2D4 File Offset: 0x002094D4
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

	// Token: 0x06005523 RID: 21795 RVA: 0x0020B464 File Offset: 0x00209664
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

	// Token: 0x06005524 RID: 21796 RVA: 0x0020B608 File Offset: 0x00209808
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

	// Token: 0x06005525 RID: 21797 RVA: 0x0020B69C File Offset: 0x0020989C
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

	// Token: 0x06005526 RID: 21798 RVA: 0x0020B714 File Offset: 0x00209914
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

	// Token: 0x06005527 RID: 21799 RVA: 0x0020B7D4 File Offset: 0x002099D4
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

	// Token: 0x0400424E RID: 16974
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cCurrentSaveVersion = 2;

	// Token: 0x0400424F RID: 16975
	public byte biomeId;

	// Token: 0x04004250 RID: 16976
	public Rect area;

	// Token: 0x04004251 RID: 16977
	public Chunk chunk;

	// Token: 0x04004252 RID: 16978
	public bool checkedPOITags;

	// Token: 0x04004253 RID: 16979
	public FastTags<TagGroup.Poi> poiTags;

	// Token: 0x04004254 RID: 16980
	public int groupsEnabledFlags;

	// Token: 0x04004255 RID: 16981
	public static int RespawnDayDelayIndexEnemies;

	// Token: 0x04004256 RID: 16982
	public static int RespawnDayDelayIndexAnimals;

	// Token: 0x04004257 RID: 16983
	public static int RespawnDayEnemyCountOverride;

	// Token: 0x04004258 RID: 16984
	public static int RespawnDayAnimalCountOverride;

	// Token: 0x04004259 RID: 16985
	public static int RespawnNightDelayIndexEnemies;

	// Token: 0x0400425A RID: 16986
	public static int RespawnNightDelayIndexAnimals;

	// Token: 0x0400425B RID: 16987
	public static int RespawnNightEnemyCountOverride;

	// Token: 0x0400425C RID: 16988
	public static int RespawnNightAnimalCountOverride;

	// Token: 0x0400425D RID: 16989
	[PublicizedFrom(EAccessModifier.Private)]
	public ChunkCustomData ccd;

	// Token: 0x0400425E RID: 16990
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<int, ChunkAreaBiomeSpawnData.CountsAndTime> entitesSpawned = new Dictionary<int, ChunkAreaBiomeSpawnData.CountsAndTime>();

	// Token: 0x02000B1B RID: 2843
	[PublicizedFrom(EAccessModifier.Private)]
	public struct CountsAndTime
	{
		// Token: 0x06005528 RID: 21800 RVA: 0x0020B97C File Offset: 0x00209B7C
		public CountsAndTime(int _count, int _maxCount, ulong _delayWorldTime)
		{
			this.count = _count;
			this.maxCount = _maxCount;
			this.delayWorldTime = _delayWorldTime;
		}

		// Token: 0x06005529 RID: 21801 RVA: 0x0020B993 File Offset: 0x00209B93
		public override string ToString()
		{
			return string.Format("cnt {0}, maxCnt {1}, wtime {2}", this.count, this.maxCount, this.delayWorldTime);
		}

		// Token: 0x0400425F RID: 16991
		public int count;

		// Token: 0x04004260 RID: 16992
		public int maxCount;

		// Token: 0x04004261 RID: 16993
		public ulong delayWorldTime;
	}
}
