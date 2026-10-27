using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

// Token: 0x02000C21 RID: 3105
public class WorldBlockTicker
{
	// Token: 0x06005E04 RID: 24068 RVA: 0x0024B8B0 File Offset: 0x00249AB0
	public WorldBlockTicker(World _world)
	{
		this.world = _world;
	}

	// Token: 0x06005E05 RID: 24069 RVA: 0x0024B908 File Offset: 0x00249B08
	public void Cleanup()
	{
		object obj = this.lockObject;
		lock (obj)
		{
			this.scheduledTicksSorted.Clear();
			this.scheduledTicksDict.Clear();
			this.chunkToScheduledTicks.Clear();
		}
	}

	// Token: 0x06005E06 RID: 24070 RVA: 0x0024B964 File Offset: 0x00249B64
	public void Tick(ArraySegment<long> _activeChunks, EntityPlayer _thePlayer, GameRandom _rnd)
	{
		if (!GameManager.bTickingActive)
		{
			return;
		}
		if (!this.world.IsRemote())
		{
			this.tickScheduled(_rnd);
		}
		if (!this.world.IsRemote())
		{
			this.tickRandom(_activeChunks, _rnd);
		}
	}

	// Token: 0x06005E07 RID: 24071 RVA: 0x0024B998 File Offset: 0x00249B98
	public void AddScheduledBlockUpdate(Vector3i _pos, int _blockId, ulong _ticks)
	{
		if (_blockId == 0)
		{
			return;
		}
		WorldBlockTickerEntry worldBlockTickerEntry = new WorldBlockTickerEntry(_pos, _blockId, _ticks + GameTimer.Instance.ticks);
		object obj = this.lockObject;
		lock (obj)
		{
			if (this.scheduledTicksDict.ContainsKey(worldBlockTickerEntry.GetHashCode()))
			{
				this.remove(_pos, _blockId);
			}
			this.add(worldBlockTickerEntry);
		}
	}

	// Token: 0x06005E08 RID: 24072 RVA: 0x0024BA0C File Offset: 0x00249C0C
	[PublicizedFrom(EAccessModifier.Private)]
	public void add(WorldBlockTickerEntry _wbte)
	{
		this.scheduledTicksDict.Add(_wbte.GetHashCode(), _wbte);
		this.scheduledTicksSorted.Add(_wbte, null);
		HashSet<WorldBlockTickerEntry> hashSet = this.chunkToScheduledTicks[_wbte.GetChunkKey()];
		if (hashSet == null)
		{
			hashSet = new HashSet<WorldBlockTickerEntry>();
			this.chunkToScheduledTicks[_wbte.GetChunkKey()] = hashSet;
		}
		hashSet.Add(_wbte);
	}

	// Token: 0x06005E09 RID: 24073 RVA: 0x0024BA70 File Offset: 0x00249C70
	[PublicizedFrom(EAccessModifier.Private)]
	public void execute(WorldBlockTickerEntry _wbte, GameRandom _rnd, ulong _ticksIfLoaded)
	{
		BlockValue block = this.world.GetBlock(_wbte.worldPos);
		if (block.type == _wbte.blockID)
		{
			block.Block.UpdateTick(this.world, _wbte.worldPos, block, false, _ticksIfLoaded, _rnd);
		}
	}

	// Token: 0x06005E0A RID: 24074 RVA: 0x0024BABC File Offset: 0x00249CBC
	[PublicizedFrom(EAccessModifier.Private)]
	public bool tickScheduled(GameRandom _rnd)
	{
		object obj = this.lockObject;
		int num;
		lock (obj)
		{
			num = this.scheduledTicksSorted.Count;
			if (num != this.scheduledTicksDict.Count)
			{
				throw new Exception("WBT: Invalid dict state");
			}
		}
		if (num > 100)
		{
			num = 100;
		}
		for (int i = 0; i < num; i++)
		{
			obj = this.lockObject;
			WorldBlockTickerEntry worldBlockTickerEntry;
			lock (obj)
			{
				if (this.scheduledTicksSorted.Count == 0)
				{
					Log.Warning("WorldBlockTicker tickScheduled count 0");
					break;
				}
				worldBlockTickerEntry = (WorldBlockTickerEntry)this.scheduledTicksSorted.GetKey(0);
				if (worldBlockTickerEntry.scheduledTime > GameTimer.Instance.ticks)
				{
					break;
				}
				this.scheduledTicksSorted.Remove(worldBlockTickerEntry);
				this.scheduledTicksDict.Remove(worldBlockTickerEntry.GetHashCode());
				HashSet<WorldBlockTickerEntry> hashSet = this.chunkToScheduledTicks[worldBlockTickerEntry.GetChunkKey()];
				if (hashSet != null)
				{
					hashSet.Remove(worldBlockTickerEntry);
				}
			}
			if (!this.world.IsChunkAreaLoaded(worldBlockTickerEntry.worldPos.x, worldBlockTickerEntry.worldPos.y, worldBlockTickerEntry.worldPos.z))
			{
				int num2 = World.toChunkXZ(worldBlockTickerEntry.worldPos.x);
				int num3 = World.toChunkXZ(worldBlockTickerEntry.worldPos.z);
				if (this.world.GetChunkSync(num2, num3) != null)
				{
					this.AddScheduledBlockUpdate(worldBlockTickerEntry.worldPos, worldBlockTickerEntry.blockID, (ulong)(30 + _rnd.RandomRange(0, 15)));
				}
			}
			else
			{
				this.execute(worldBlockTickerEntry, _rnd, 0UL);
			}
		}
		return this.scheduledTicksSorted.Count != 0;
	}

	// Token: 0x06005E0B RID: 24075 RVA: 0x0024BC8C File Offset: 0x00249E8C
	[PublicizedFrom(EAccessModifier.Private)]
	public void tickRandom(ArraySegment<long> _activeChunkSet, GameRandom _rnd)
	{
		if (this.randomTickIndex >= this.randomTickChunkKeys.Count)
		{
			this.randomTickChunkKeys.Clear();
			if (_activeChunkSet.Count > this.randomTickChunkKeys.Capacity)
			{
				this.randomTickChunkKeys.Capacity = _activeChunkSet.Count * 2;
			}
			int num = _activeChunkSet.Offset + _activeChunkSet.Count;
			for (int i = _activeChunkSet.Offset; i < num; i++)
			{
				this.randomTickChunkKeys.Add(_activeChunkSet.Array[i]);
			}
			this.randomTickCountPerFrame = Math.Max(_activeChunkSet.Count / 100, 1);
			this.randomTickIndex = 0;
		}
		int num2 = 0;
		while (this.randomTickIndex < this.randomTickChunkKeys.Count && num2 < this.randomTickCountPerFrame)
		{
			long num3 = this.randomTickChunkKeys[this.randomTickIndex];
			Chunk chunkSync = this.world.ChunkCache.GetChunkSync(num3);
			this.tickChunkRandom(chunkSync, _rnd);
			this.randomTickIndex++;
			num2++;
		}
	}

	// Token: 0x06005E0C RID: 24076 RVA: 0x0024BD98 File Offset: 0x00249F98
	[PublicizedFrom(EAccessModifier.Private)]
	public void tickChunkRandom(Chunk chunk, GameRandom _rnd)
	{
		if (chunk == null)
		{
			return;
		}
		if (chunk.NeedsLightCalculation)
		{
			return;
		}
		if (GameTimer.Instance.ticks - chunk.LastTimeRandomTicked < 1200UL)
		{
			return;
		}
		ulong num = GameTimer.Instance.ticks - chunk.LastTimeRandomTicked;
		chunk.LastTimeRandomTicked = GameTimer.Instance.ticks;
		DictionaryKeyList<Vector3i, int> tickedBlocks = chunk.GetTickedBlocks();
		DictionaryKeyList<Vector3i, int> dictionaryKeyList = tickedBlocks;
		lock (dictionaryKeyList)
		{
			for (int i = tickedBlocks.list.Count - 1; i >= 0; i--)
			{
				Vector3i vector3i = tickedBlocks.list[i];
				BlockValue block = chunk.GetBlock(World.toBlockXZ(vector3i.x), vector3i.y, World.toBlockXZ(vector3i.z));
				if (this.scheduledTicksDict.Count == 0 || !this.scheduledTicksDict.ContainsKey(WorldBlockTickerEntry.ToHashCode(vector3i, block.type)))
				{
					block.Block.UpdateTick(this.world, vector3i, block, true, num, _rnd);
				}
			}
		}
	}

	// Token: 0x06005E0D RID: 24077 RVA: 0x0024BEB8 File Offset: 0x0024A0B8
	public void OnChunkAdded(WorldBase _world, Chunk _c, GameRandom _rnd)
	{
		ChunkCustomData chunkCustomData;
		if (!_c.ChunkCustomData.dict.TryGetValue("wbt.sch", out chunkCustomData) || chunkCustomData == null)
		{
			return;
		}
		_c.ChunkCustomData.Remove("wbt.sch");
		using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
		{
			pooledBinaryReader.SetBaseStream(new MemoryStream(chunkCustomData.data));
			int num = (int)pooledBinaryReader.ReadUInt16();
			int num2 = (int)pooledBinaryReader.ReadByte();
			for (int i = 0; i < num; i++)
			{
				WorldBlockTickerEntry worldBlockTickerEntry = WorldBlockTickerEntry.Read(pooledBinaryReader, _c.X, _c.Z, num2);
				object obj = this.lockObject;
				lock (obj)
				{
					if (!this.scheduledTicksDict.ContainsKey(worldBlockTickerEntry.GetHashCode()))
					{
						if (worldBlockTickerEntry.scheduledTime > GameTimer.Instance.ticks)
						{
							this.add(worldBlockTickerEntry);
						}
						else
						{
							this.execute(worldBlockTickerEntry, _rnd, GameTimer.Instance.ticks - worldBlockTickerEntry.scheduledTime);
						}
					}
				}
			}
		}
	}

	// Token: 0x06005E0E RID: 24078 RVA: 0x0024BFE0 File Offset: 0x0024A1E0
	public void OnChunkRemoved(Chunk _c)
	{
		this.addScheduleInformationToChunk(_c, true);
	}

	// Token: 0x06005E0F RID: 24079 RVA: 0x0024BFEA File Offset: 0x0024A1EA
	public void OnChunkBeforeSave(Chunk _c)
	{
		this.addScheduleInformationToChunk(_c, false);
	}

	// Token: 0x06005E10 RID: 24080 RVA: 0x0024BFF4 File Offset: 0x0024A1F4
	[PublicizedFrom(EAccessModifier.Private)]
	public void addScheduleInformationToChunk(Chunk _c, bool _bChunkIsRemoved)
	{
		HashSet<WorldBlockTickerEntry> hashSet = this.chunkToScheduledTicks[_c.Key];
		object obj = this.lockObject;
		lock (obj)
		{
			if (hashSet == null)
			{
				return;
			}
			if (_bChunkIsRemoved)
			{
				this.chunkToScheduledTicks.Remove(_c.Key);
			}
		}
		if (hashSet.Count == 0)
		{
			return;
		}
		ChunkCustomData chunkCustomData = new ChunkCustomData("wbt.sch", ulong.MaxValue, false);
		using (PooledExpandableMemoryStream pooledExpandableMemoryStream = MemoryPools.poolMemoryStream.AllocSync(true))
		{
			using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
			{
				pooledBinaryWriter.SetBaseStream(pooledExpandableMemoryStream);
				pooledBinaryWriter.Write((ushort)hashSet.Count);
				pooledBinaryWriter.Write(1);
				foreach (WorldBlockTickerEntry worldBlockTickerEntry in hashSet)
				{
					worldBlockTickerEntry.Write(pooledBinaryWriter);
					if (_bChunkIsRemoved)
					{
						obj = this.lockObject;
						lock (obj)
						{
							this.scheduledTicksSorted.Remove(worldBlockTickerEntry);
							this.scheduledTicksDict.Remove(worldBlockTickerEntry.GetHashCode());
						}
					}
				}
			}
			chunkCustomData.data = pooledExpandableMemoryStream.ToArray();
		}
		_c.ChunkCustomData.Set("wbt.sch", chunkCustomData);
		_c.isModified = true;
	}

	// Token: 0x06005E11 RID: 24081 RVA: 0x0024C190 File Offset: 0x0024A390
	public int GetCount()
	{
		object obj = this.lockObject;
		int count;
		lock (obj)
		{
			count = this.scheduledTicksDict.Count;
		}
		return count;
	}

	// Token: 0x06005E12 RID: 24082 RVA: 0x0024C1D8 File Offset: 0x0024A3D8
	public void InvalidateScheduledBlockUpdate(Vector3i _pos, int _blockID)
	{
		if (_blockID == 0)
		{
			return;
		}
		this.remove(_pos, _blockID);
	}

	// Token: 0x06005E13 RID: 24083 RVA: 0x0024C1E8 File Offset: 0x0024A3E8
	[PublicizedFrom(EAccessModifier.Private)]
	public void remove(Vector3i _pos, int _blockID)
	{
		object obj = this.lockObject;
		lock (obj)
		{
			int num = WorldBlockTickerEntry.ToHashCode(_pos, _blockID);
			WorldBlockTickerEntry worldBlockTickerEntry = null;
			if (this.scheduledTicksDict.ContainsKey(num))
			{
				worldBlockTickerEntry = this.scheduledTicksDict[num];
				this.scheduledTicksDict.Remove(num);
				if (worldBlockTickerEntry != null)
				{
					this.scheduledTicksSorted.Remove(worldBlockTickerEntry);
				}
			}
			long num2 = WorldChunkCache.MakeChunkKey(World.toChunkXZ(_pos.x), World.toChunkXZ(_pos.z));
			HashSet<WorldBlockTickerEntry> hashSet = this.chunkToScheduledTicks[num2];
			if (hashSet != null)
			{
				hashSet.Remove(worldBlockTickerEntry);
			}
		}
	}

	// Token: 0x040048FF RID: 18687
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object lockObject = new object();

	// Token: 0x04004900 RID: 18688
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SortedList scheduledTicksSorted = new SortedList(new WorldBlockTicker.EntryComparer());

	// Token: 0x04004901 RID: 18689
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<int, WorldBlockTickerEntry> scheduledTicksDict = new Dictionary<int, WorldBlockTickerEntry>();

	// Token: 0x04004902 RID: 18690
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly DictionarySave<long, HashSet<WorldBlockTickerEntry>> chunkToScheduledTicks = new DictionarySave<long, HashSet<WorldBlockTickerEntry>>();

	// Token: 0x04004903 RID: 18691
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly World world;

	// Token: 0x04004904 RID: 18692
	[PublicizedFrom(EAccessModifier.Private)]
	public int randomTickIndex;

	// Token: 0x04004905 RID: 18693
	[PublicizedFrom(EAccessModifier.Private)]
	public int randomTickCountPerFrame;

	// Token: 0x04004906 RID: 18694
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<long> randomTickChunkKeys = new List<long>();

	// Token: 0x02000C22 RID: 3106
	[PublicizedFrom(EAccessModifier.Private)]
	public class EntryComparer : IComparer
	{
		// Token: 0x06005E14 RID: 24084 RVA: 0x0024C29C File Offset: 0x0024A49C
		public int Compare(object _o1, object _o2)
		{
			WorldBlockTickerEntry worldBlockTickerEntry = (WorldBlockTickerEntry)_o1;
			WorldBlockTickerEntry worldBlockTickerEntry2 = (WorldBlockTickerEntry)_o2;
			if (worldBlockTickerEntry.scheduledTime < worldBlockTickerEntry2.scheduledTime)
			{
				return -1;
			}
			if (worldBlockTickerEntry.scheduledTime > worldBlockTickerEntry2.scheduledTime)
			{
				return 1;
			}
			if (worldBlockTickerEntry.tickEntryID < worldBlockTickerEntry2.tickEntryID)
			{
				return -1;
			}
			if (worldBlockTickerEntry.tickEntryID > worldBlockTickerEntry2.tickEntryID)
			{
				return 1;
			}
			return 0;
		}
	}
}
