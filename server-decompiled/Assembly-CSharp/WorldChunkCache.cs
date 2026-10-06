using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

// Token: 0x02000C1A RID: 3098
public class WorldChunkCache : IChunkAccess
{
	// Token: 0x06005DFB RID: 24059 RVA: 0x0004DB9E File Offset: 0x0004BD9E
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static long MakeChunkKey(int x, int y)
	{
		return (((long)y & 16777215L) << 24) | ((long)x & 16777215L);
	}

	// Token: 0x06005DFC RID: 24060 RVA: 0x0024AF5E File Offset: 0x0024915E
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int extractX(long key)
	{
		return (int)((int)key << 8) >> 8;
	}

	// Token: 0x06005DFD RID: 24061 RVA: 0x0024AF66 File Offset: 0x00249166
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int extractZ(long key)
	{
		return (int)(key >> 16) >> 8;
	}

	// Token: 0x06005DFE RID: 24062 RVA: 0x0024AF70 File Offset: 0x00249170
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2i extractXZ(long key)
	{
		Vector2i vector2i;
		vector2i.x = WorldChunkCache.extractX(key);
		vector2i.y = WorldChunkCache.extractZ(key);
		return vector2i;
	}

	// Token: 0x06005DFF RID: 24063 RVA: 0x0024AF98 File Offset: 0x00249198
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Chunk GetChunkSync(int _x, int _y)
	{
		return this.GetChunkSync(WorldChunkCache.MakeChunkKey(_x, _y));
	}

	// Token: 0x06005E00 RID: 24064 RVA: 0x0024AFA8 File Offset: 0x002491A8
	public virtual Chunk GetChunkSync(long _key)
	{
		this.sync.EnterReadLock();
		Chunk chunk;
		this.chunks.dict.TryGetValue(_key & 72057594037927935L, out chunk);
		this.sync.ExitReadLock();
		return chunk;
	}

	// Token: 0x06005E01 RID: 24065 RVA: 0x0024AFEA File Offset: 0x002491EA
	[PublicizedFrom(EAccessModifier.Private)]
	public IChunk IChunkAccess.GetChunkSync(int chunkX, int chunkZ)
	{
		return this.GetChunkSync(chunkX, chunkZ);
	}

	// Token: 0x06005E02 RID: 24066 RVA: 0x0024AFF4 File Offset: 0x002491F4
	[PublicizedFrom(EAccessModifier.Private)]
	public IChunk IChunkAccess.GetChunkSync(long chunkKey)
	{
		return this.GetChunkSync(chunkKey);
	}

	// Token: 0x06005E03 RID: 24067 RVA: 0x001B1448 File Offset: 0x001AF648
	public IChunk GetChunkSync(int chunkX, int chunkY, int chunkZ)
	{
		return IChunkAccess.DefaultGetChunkSync(this, chunkX, chunkY, chunkZ);
	}

	// Token: 0x06005E04 RID: 24068 RVA: 0x001B1453 File Offset: 0x001AF653
	public IChunk GetChunkSync(Vector2i chunkPos)
	{
		return IChunkAccess.DefaultGetChunkSync(this, chunkPos);
	}

	// Token: 0x06005E05 RID: 24069 RVA: 0x001B145C File Offset: 0x001AF65C
	public IChunk GetChunkSync(PropRef propRef)
	{
		return IChunkAccess.DefaultGetChunkSync(this, propRef);
	}

	// Token: 0x06005E06 RID: 24070 RVA: 0x001B1465 File Offset: 0x001AF665
	public IChunk GetChunkSync(BlockValueRef bvRef)
	{
		return IChunkAccess.DefaultGetChunkSync(this, bvRef);
	}

	// Token: 0x06005E07 RID: 24071 RVA: 0x001B146E File Offset: 0x001AF66E
	public IChunk GetChunkFromWorldPos(int x, int z)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, x, z);
	}

	// Token: 0x06005E08 RID: 24072 RVA: 0x001B1478 File Offset: 0x001AF678
	public IChunk GetChunkFromWorldPos(int x, int y, int z)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, x, y, z);
	}

	// Token: 0x06005E09 RID: 24073 RVA: 0x001B1483 File Offset: 0x001AF683
	public IChunk GetChunkFromWorldPos(Vector3i blockPos)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, blockPos);
	}

	// Token: 0x06005E0A RID: 24074 RVA: 0x001B148C File Offset: 0x001AF68C
	public bool GetChunkFromWorldPos(int x, int z, ref IChunk chunk)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, x, z, ref chunk);
	}

	// Token: 0x06005E0B RID: 24075 RVA: 0x001B1497 File Offset: 0x001AF697
	public bool GetChunkFromWorldPos(Vector3i blockPos, ref IChunk chunk)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, blockPos, ref chunk);
	}

	// Token: 0x06005E0C RID: 24076 RVA: 0x0024B000 File Offset: 0x00249200
	public virtual void RemoveChunkSync(long _key)
	{
		this.sync.EnterWriteLock();
		Chunk chunk;
		if (this.chunks.dict.TryGetValue(_key & 72057594037927935L, out chunk))
		{
			this.chunks.Remove(_key & 72057594037927935L);
			this.isChunkArrayDirty = true;
		}
		this.chunkKeys.Remove(_key);
		this.isChunkKeysDirty = true;
		this.sync.ExitWriteLock();
		if (chunk != null)
		{
			for (int i = 0; i < this.chunkCallbacks.Count; i++)
			{
				this.chunkCallbacks[i].OnChunkBeforeRemove(chunk);
			}
		}
	}

	// Token: 0x06005E0D RID: 24077 RVA: 0x0024B0A4 File Offset: 0x002492A4
	public void NotifyOnChunkBeforeSave(Chunk _c)
	{
		for (int i = 0; i < this.chunkCallbacks.Count; i++)
		{
			this.chunkCallbacks[i].OnChunkBeforeSave(_c);
		}
	}

	// Token: 0x06005E0E RID: 24078 RVA: 0x0024B0DC File Offset: 0x002492DC
	public virtual bool AddChunkSync(Chunk _chunk, bool _bOmitCallbacks = false)
	{
		this.ChunkMinPos.x = Utils.FastMin(this.ChunkMinPos.x, _chunk.X);
		this.ChunkMinPos.y = Utils.FastMin(this.ChunkMinPos.y, _chunk.Z);
		this.ChunkMaxPos.x = Utils.FastMax(this.ChunkMaxPos.x, _chunk.X);
		this.ChunkMaxPos.y = Utils.FastMax(this.ChunkMaxPos.y, _chunk.Z);
		this.sync.EnterWriteLock();
		long key = _chunk.Key;
		if (this.chunkKeys.Contains(key))
		{
			this.sync.ExitWriteLock();
			return false;
		}
		this.chunks.Add(key & 72057594037927935L, _chunk);
		this.chunkKeys.Add(key);
		this.isChunkArrayDirty = true;
		this.isChunkKeysDirty = true;
		this.sync.ExitWriteLock();
		int num = 0;
		while (!_bOmitCallbacks && num < this.chunkCallbacks.Count)
		{
			this.chunkCallbacks[num].OnChunkAdded(_chunk);
			num++;
		}
		return true;
	}

	// Token: 0x06005E0F RID: 24079 RVA: 0x0024B208 File Offset: 0x00249408
	public List<Chunk> GetChunkArrayCopySync()
	{
		this.sync.EnterReadLock();
		if (this.isChunkArrayDirty)
		{
			this.chunkArrayCopy = new List<Chunk>(this.chunks.list.Count);
			this.chunkArrayCopy.AddRange(this.chunks.list);
			this.isChunkArrayDirty = false;
		}
		this.sync.ExitReadLock();
		return this.chunkArrayCopy;
	}

	// Token: 0x06005E10 RID: 24080 RVA: 0x0024B275 File Offset: 0x00249475
	public LinkedList<Chunk> GetChunkArray()
	{
		return this.chunks.list;
	}

	// Token: 0x06005E11 RID: 24081 RVA: 0x0024B284 File Offset: 0x00249484
	public HashSetLong GetChunkKeysCopySync()
	{
		if (this.isChunkKeysDirty)
		{
			this.sync.EnterReadLock();
			this.chunkKeysCopy.Clear();
			foreach (long num in this.chunkKeys)
			{
				this.chunkKeysCopy.Add(num);
			}
			this.sync.ExitReadLock();
			this.isChunkKeysDirty = false;
		}
		return this.chunkKeysCopy;
	}

	// Token: 0x06005E12 RID: 24082 RVA: 0x0024B318 File Offset: 0x00249518
	public int Count()
	{
		return this.chunks.list.Count;
	}

	// Token: 0x06005E13 RID: 24083 RVA: 0x0024B32A File Offset: 0x0024952A
	public ReaderWriterLockSlim GetSyncRoot()
	{
		return this.sync;
	}

	// Token: 0x06005E14 RID: 24084 RVA: 0x0024B332 File Offset: 0x00249532
	public virtual bool ContainsChunkSync(long key)
	{
		this.sync.EnterReadLock();
		bool flag = this.chunks.dict.ContainsKey(key);
		this.sync.ExitReadLock();
		return flag;
	}

	// Token: 0x06005E15 RID: 24085 RVA: 0x0024B35B File Offset: 0x0024955B
	public void AddChunkCallback(IChunkCallback _callback)
	{
		this.chunkCallbacks.Add(_callback);
	}

	// Token: 0x06005E16 RID: 24086 RVA: 0x0024B369 File Offset: 0x00249569
	public void RemoveChunkCallback(IChunkCallback _callback)
	{
		this.chunkCallbacks.Remove(_callback);
	}

	// Token: 0x06005E17 RID: 24087 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Update()
	{
	}

	// Token: 0x06005E18 RID: 24088 RVA: 0x0024B378 File Offset: 0x00249578
	public virtual void Clear()
	{
		this.sync.EnterWriteLock();
		List<Chunk> list = new List<Chunk>();
		list.AddRange(this.GetChunkArray());
		this.chunks.Clear();
		this.chunkKeys.Clear();
		this.isChunkArrayDirty = true;
		this.isChunkKeysDirty = true;
		this.sync.ExitWriteLock();
		MemoryPools.PoolChunks.FreeSync(list);
	}

	// Token: 0x06005E19 RID: 24089 RVA: 0x0024B3E0 File Offset: 0x002495E0
	public void GetNeighborChunkKeys(long _chunkKey, bool _includeDiagonals, ref long[] _neighbours)
	{
		if (_includeDiagonals && _neighbours.Length != 8)
		{
			_neighbours = new long[8];
		}
		else if (_neighbours.Length != 8)
		{
			_neighbours = new long[4];
		}
		int num = WorldChunkCache.extractX(_chunkKey);
		int num2 = WorldChunkCache.extractZ(_chunkKey);
		_neighbours[0] = WorldChunkCache.MakeChunkKey(num, num2 - 1);
		_neighbours[1] = WorldChunkCache.MakeChunkKey(num, num2 + 1);
		_neighbours[2] = WorldChunkCache.MakeChunkKey(num - 1, num2);
		_neighbours[3] = WorldChunkCache.MakeChunkKey(num + 1, num2);
		if (_includeDiagonals)
		{
			_neighbours[4] = WorldChunkCache.MakeChunkKey(num - 1, num2 - 1);
			_neighbours[5] = WorldChunkCache.MakeChunkKey(num - 1, num2 + 1);
			_neighbours[6] = WorldChunkCache.MakeChunkKey(num + 1, num2 - 1);
			_neighbours[7] = WorldChunkCache.MakeChunkKey(num + 1, num2 + 1);
		}
	}

	// Token: 0x06005E1A RID: 24090 RVA: 0x0024B494 File Offset: 0x00249694
	public bool GetNeighborChunks(Chunk _chunk, Chunk[] neighbours)
	{
		this.sync.EnterReadLock();
		int x = _chunk.X;
		int z = _chunk.Z;
		Chunk chunk = this.GetChunk(x + 1, z);
		if (chunk != null)
		{
			neighbours[0] = chunk;
			chunk = this.GetChunk(x - 1, z);
			if (chunk != null)
			{
				neighbours[1] = chunk;
				chunk = this.GetChunk(x, z + 1);
				if (chunk != null)
				{
					neighbours[2] = chunk;
					chunk = this.GetChunk(x, z - 1);
					if (chunk != null)
					{
						neighbours[3] = chunk;
						chunk = this.GetChunk(x + 1, z + 1);
						if (chunk != null)
						{
							neighbours[4] = chunk;
							chunk = this.GetChunk(x - 1, z - 1);
							if (chunk != null)
							{
								neighbours[5] = chunk;
								chunk = this.GetChunk(x - 1, z + 1);
								if (chunk != null)
								{
									neighbours[6] = chunk;
									chunk = this.GetChunk(x + 1, z - 1);
									if (chunk != null)
									{
										neighbours[7] = chunk;
										this.sync.ExitReadLock();
										return true;
									}
								}
							}
						}
					}
				}
			}
		}
		this.sync.ExitReadLock();
		return false;
	}

	// Token: 0x06005E1B RID: 24091 RVA: 0x0024B574 File Offset: 0x00249774
	public bool HasNeighborChunks(Chunk _chunk)
	{
		this.sync.EnterReadLock();
		int x = _chunk.X;
		int z = _chunk.Z;
		bool flag = this.GetChunk(x + 1, z) != null && this.GetChunk(x - 1, z) != null && this.GetChunk(x, z + 1) != null && this.GetChunk(x, z - 1) != null && this.GetChunk(x + 1, z + 1) != null && this.GetChunk(x - 1, z - 1) != null && this.GetChunk(x - 1, z + 1) != null && this.GetChunk(x + 1, z - 1) != null;
		this.sync.ExitReadLock();
		return flag;
	}

	// Token: 0x06005E1C RID: 24092 RVA: 0x0024B614 File Offset: 0x00249814
	[PublicizedFrom(EAccessModifier.Private)]
	public Chunk GetChunk(int _x, int _y)
	{
		long num = WorldChunkCache.MakeChunkKey(_x, _y);
		Chunk chunk;
		this.chunks.dict.TryGetValue(num, out chunk);
		return chunk;
	}

	// Token: 0x040048E7 RID: 18663
	public const long InvalidChunk = 9223372036854775807L;

	// Token: 0x040048E8 RID: 18664
	public Vector2i ChunkMinPos = Vector2i.zero;

	// Token: 0x040048E9 RID: 18665
	public Vector2i ChunkMaxPos = Vector2i.zero;

	// Token: 0x040048EA RID: 18666
	public DictionaryLinkedList<long, Chunk> chunks = new DictionaryLinkedList<long, Chunk>();

	// Token: 0x040048EB RID: 18667
	[PublicizedFrom(EAccessModifier.Private)]
	public List<IChunkCallback> chunkCallbacks = new List<IChunkCallback>();

	// Token: 0x040048EC RID: 18668
	public HashSetLong chunkKeys = new HashSetLong();

	// Token: 0x040048ED RID: 18669
	[PublicizedFrom(EAccessModifier.Private)]
	public volatile bool isChunkKeysDirty = true;

	// Token: 0x040048EE RID: 18670
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSetLong chunkKeysCopy = new HashSetLong();

	// Token: 0x040048EF RID: 18671
	[PublicizedFrom(EAccessModifier.Private)]
	public volatile bool isChunkArrayDirty = true;

	// Token: 0x040048F0 RID: 18672
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Chunk> chunkArrayCopy = new List<Chunk>();

	// Token: 0x040048F1 RID: 18673
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ReaderWriterLockSlim sync = new ReaderWriterLockSlim();
}
