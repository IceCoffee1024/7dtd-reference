using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

// Token: 0x02000AF6 RID: 2806
public class ChunkAreaBiomeSpawnData
{
	// Token: 0x06005479 RID: 21625 RVA: 0x00205114 File Offset: 0x00203314
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

	// Token: 0x0600547A RID: 21626 RVA: 0x002051DC File Offset: 0x002033DC
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

	// Token: 0x0600547B RID: 21627 RVA: 0x00205268 File Offset: 0x00203468
	public bool CanSpawn(int _idHash)
	{
		ChunkAreaBiomeSpawnData.CountsAndTime countsAndTime;
		return this.entitesSpawned.TryGetValue(_idHash, out countsAndTime) && countsAndTime.count < countsAndTime.maxCount;
	}

	// Token: 0x0600547C RID: 21628 RVA: 0x00205298 File Offset: 0x00203498
	public void SetCounts(int _idHash, int _count, int _maxCount)
	{
		ChunkAreaBiomeSpawnData.CountsAndTime countsAndTime;
		this.entitesSpawned.TryGetValue(_idHash, out countsAndTime);
		countsAndTime.count = _count;
		countsAndTime.maxCount = _maxCount;
		this.entitesSpawned[_idHash] = countsAndTime;
	}

	// Token: 0x0600547D RID: 21629 RVA: 0x002052D4 File Offset: 0x002034D4
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

	// Token: 0x0600547E RID: 21630 RVA: 0x00205320 File Offset: 0x00203520
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

	// Token: 0x0600547F RID: 21631 RVA: 0x00205384 File Offset: 0x00203584
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

	// Token: 0x06005480 RID: 21632 RVA: 0x002053D0 File Offset: 0x002035D0
	public ulong GetDelayWorldTime(int _idHash)
	{
		ChunkAreaBiomeSpawnData.CountsAndTime countsAndTime;
		this.entitesSpawned.TryGetValue(_idHash, out countsAndTime);
		return countsAndTime.delayWorldTime;
	}

	// Token: 0x06005481 RID: 21633 RVA: 0x002053F4 File Offset: 0x002035F4
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
		int num = ((biomeSpawnEntityGroupData.respawnDelayInWorldTime.Length == 1) ? 0 : (biomeSpawnEntityGroupData.isAnimal ? ChunkAreaBiomeSpawnData.RespawnDelayIndexAnimals : ChunkAreaBiomeSpawnData.RespawnDelayIndexEnemies));
		if (_maxCount > 0 && ChunkAreaBiomeSpawnData.RespawnCountOverride > 0 && EntityGroups.IsEnemyGroup(biomeSpawnEntityGroupData.entityGroupName))
		{
			_maxCount = ChunkAreaBiomeSpawnData.RespawnCountOverride;
		}
		countsAndTime.delayWorldTime = _world.worldTime + (ulong)((float)biomeSpawnEntityGroupData.respawnDelayInWorldTime[num] * _world.RandomRange(0.9f, 1.1f));
		countsAndTime.maxCount = _maxCount;
		this.entitesSpawned[_idHash] = countsAndTime;
		this.chunk.isModified = true;
	}

	// Token: 0x06005482 RID: 21634 RVA: 0x002054D8 File Offset: 0x002036D8
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

	// Token: 0x06005483 RID: 21635 RVA: 0x0020567C File Offset: 0x0020387C
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

	// Token: 0x06005484 RID: 21636 RVA: 0x00205710 File Offset: 0x00203910
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

	// Token: 0x06005485 RID: 21637 RVA: 0x00205788 File Offset: 0x00203988
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

	// Token: 0x06005486 RID: 21638 RVA: 0x00205848 File Offset: 0x00203A48
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

	// Token: 0x0400417B RID: 16763
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cCurrentSaveVersion = 2;

	// Token: 0x0400417C RID: 16764
	public byte biomeId;

	// Token: 0x0400417D RID: 16765
	public Rect area;

	// Token: 0x0400417E RID: 16766
	public Chunk chunk;

	// Token: 0x0400417F RID: 16767
	public bool checkedPOITags;

	// Token: 0x04004180 RID: 16768
	public FastTags<TagGroup.Poi> poiTags;

	// Token: 0x04004181 RID: 16769
	public int groupsEnabledFlags;

	// Token: 0x04004182 RID: 16770
	public static int RespawnDelayIndexEnemies;

	// Token: 0x04004183 RID: 16771
	public static int RespawnDelayIndexAnimals;

	// Token: 0x04004184 RID: 16772
	public static int RespawnCountOverride;

	// Token: 0x04004185 RID: 16773
	[PublicizedFrom(EAccessModifier.Private)]
	public ChunkCustomData ccd;

	// Token: 0x04004186 RID: 16774
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<int, ChunkAreaBiomeSpawnData.CountsAndTime> entitesSpawned = new Dictionary<int, ChunkAreaBiomeSpawnData.CountsAndTime>();

	// Token: 0x02000AF7 RID: 2807
	[PublicizedFrom(EAccessModifier.Private)]
	public struct CountsAndTime
	{
		// Token: 0x06005487 RID: 21639 RVA: 0x002059F0 File Offset: 0x00203BF0
		public CountsAndTime(int _count, int _maxCount, ulong _delayWorldTime)
		{
			this.count = _count;
			this.maxCount = _maxCount;
			this.delayWorldTime = _delayWorldTime;
		}

		// Token: 0x06005488 RID: 21640 RVA: 0x00205A07 File Offset: 0x00203C07
		public override string ToString()
		{
			return string.Format("cnt {0}, maxCnt {1}, wtime {2}", this.count, this.maxCount, this.delayWorldTime);
		}

		// Token: 0x04004187 RID: 16775
		public int count;

		// Token: 0x04004188 RID: 16776
		public int maxCount;

		// Token: 0x04004189 RID: 16777
		public ulong delayWorldTime;
	}
}
