using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

// Token: 0x02000C08 RID: 3080
public class WorldChunkCache : IChunkAccess
{
	// Token: 0x06005DE2 RID: 24034 RVA: 0x0004DB9A File Offset: 0x0004BD9A
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static long MakeChunkKey(int x, int y)
	{
		return (((long)y & 16777215L) << 24) | ((long)x & 16777215L);
	}

	// Token: 0x06005DE3 RID: 24035 RVA: 0x00247C62 File Offset: 0x00245E62
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int extractX(long key)
	{
		return (int)((int)key << 8) >> 8;
	}

	// Token: 0x06005DE4 RID: 24036 RVA: 0x00247C6A File Offset: 0x00245E6A
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int extractZ(long key)
	{
		return (int)(key >> 16) >> 8;
	}

	// Token: 0x06005DE5 RID: 24037 RVA: 0x00247C74 File Offset: 0x00245E74
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2i extractXZ(long key)
	{
		Vector2i vector2i;
		vector2i.x = WorldChunkCache.extractX(key);
		vector2i.y = WorldChunkCache.extractZ(key);
		return vector2i;
	}

	// Token: 0x06005DE6 RID: 24038 RVA: 0x00247C9C File Offset: 0x00245E9C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Chunk GetChunkSync(int _x, int _y)
	{
		return this.GetChunkSync(WorldChunkCache.MakeChunkKey(_x, _y));
	}

	// Token: 0x06005DE7 RID: 24039 RVA: 0x00247CAC File Offset: 0x00245EAC
	public virtual Chunk GetChunkSync(long _key)
	{
		this.sync.EnterReadLock();
		Chunk chunk;
		this.chunks.dict.TryGetValue(_key & 72057594037927935L, out chunk);
		this.sync.ExitReadLock();
		return chunk;
	}

	// Token: 0x06005DE8 RID: 24040 RVA: 0x00247CEE File Offset: 0x00245EEE
	[PublicizedFrom(EAccessModifier.Private)]
	public IChunk IChunkAccess.GetChunkSync(int chunkX, int chunkZ)
	{
		return this.GetChunkSync(chunkX, chunkZ);
	}

	// Token: 0x06005DE9 RID: 24041 RVA: 0x00247CF8 File Offset: 0x00245EF8
	[PublicizedFrom(EAccessModifier.Private)]
	public IChunk IChunkAccess.GetChunkSync(long chunkKey)
	{
		return this.GetChunkSync(chunkKey);
	}

	// Token: 0x06005DEA RID: 24042 RVA: 0x001B1240 File Offset: 0x001AF440
	public IChunk GetChunkSync(int chunkX, int chunkY, int chunkZ)
	{
		return IChunkAccess.DefaultGetChunkSync(this, chunkX, chunkY, chunkZ);
	}

	// Token: 0x06005DEB RID: 24043 RVA: 0x001B124B File Offset: 0x001AF44B
	public IChunk GetChunkSync(Vector2i chunkPos)
	{
		return IChunkAccess.DefaultGetChunkSync(this, chunkPos);
	}

	// Token: 0x06005DEC RID: 24044 RVA: 0x001B1254 File Offset: 0x001AF454
	public IChunk GetChunkSync(PropRef propRef)
	{
		return IChunkAccess.DefaultGetChunkSync(this, propRef);
	}

	// Token: 0x06005DED RID: 24045 RVA: 0x001B125D File Offset: 0x001AF45D
	public IChunk GetChunkSync(BlockValueRef bvRef)
	{
		return IChunkAccess.DefaultGetChunkSync(this, bvRef);
	}

	// Token: 0x06005DEE RID: 24046 RVA: 0x001B1266 File Offset: 0x001AF466
	public IChunk GetChunkFromWorldPos(int x, int z)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, x, z);
	}

	// Token: 0x06005DEF RID: 24047 RVA: 0x001B1270 File Offset: 0x001AF470
	public IChunk GetChunkFromWorldPos(int x, int y, int z)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, x, y, z);
	}

	// Token: 0x06005DF0 RID: 24048 RVA: 0x001B127B File Offset: 0x001AF47B
	public IChunk GetChunkFromWorldPos(Vector3i blockPos)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, blockPos);
	}

	// Token: 0x06005DF1 RID: 24049 RVA: 0x001B1284 File Offset: 0x001AF484
	public bool GetChunkFromWorldPos(int x, int z, ref IChunk chunk)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, x, z, ref chunk);
	}

	// Token: 0x06005DF2 RID: 24050 RVA: 0x001B128F File Offset: 0x001AF48F
	public bool GetChunkFromWorldPos(Vector3i blockPos, ref IChunk chunk)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, blockPos, ref chunk);
	}

	// Token: 0x06005DF3 RID: 24051 RVA: 0x00247D04 File Offset: 0x00245F04
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

	// Token: 0x06005DF4 RID: 24052 RVA: 0x00247DA8 File Offset: 0x00245FA8
	public void NotifyOnChunkBeforeSave(Chunk _c)
	{
		for (int i = 0; i < this.chunkCallbacks.Count; i++)
		{
			this.chunkCallbacks[i].OnChunkBeforeSave(_c);
		}
	}

	// Token: 0x06005DF5 RID: 24053 RVA: 0x00247DE0 File Offset: 0x00245FE0
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

	// Token: 0x06005DF6 RID: 24054 RVA: 0x00247F0C File Offset: 0x0024610C
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

	// Token: 0x06005DF7 RID: 24055 RVA: 0x00247F79 File Offset: 0x00246179
	public LinkedList<Chunk> GetChunkArray()
	{
		return this.chunks.list;
	}

	// Token: 0x06005DF8 RID: 24056 RVA: 0x00247F88 File Offset: 0x00246188
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

	// Token: 0x06005DF9 RID: 24057 RVA: 0x0024801C File Offset: 0x0024621C
	public int Count()
	{
		return this.chunks.list.Count;
	}

	// Token: 0x06005DFA RID: 24058 RVA: 0x0024802E File Offset: 0x0024622E
	public ReaderWriterLockSlim GetSyncRoot()
	{
		return this.sync;
	}

	// Token: 0x06005DFB RID: 24059 RVA: 0x00248036 File Offset: 0x00246236
	public virtual bool ContainsChunkSync(long key)
	{
		this.sync.EnterReadLock();
		bool flag = this.chunks.dict.ContainsKey(key);
		this.sync.ExitReadLock();
		return flag;
	}

	// Token: 0x06005DFC RID: 24060 RVA: 0x0024805F File Offset: 0x0024625F
	public void AddChunkCallback(IChunkCallback _callback)
	{
		this.chunkCallbacks.Add(_callback);
	}

	// Token: 0x06005DFD RID: 24061 RVA: 0x0024806D File Offset: 0x0024626D
	public void RemoveChunkCallback(IChunkCallback _callback)
	{
		this.chunkCallbacks.Remove(_callback);
	}

	// Token: 0x06005DFE RID: 24062 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Update()
	{
	}

	// Token: 0x06005DFF RID: 24063 RVA: 0x0024807C File Offset: 0x0024627C
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

	// Token: 0x06005E00 RID: 24064 RVA: 0x002480E4 File Offset: 0x002462E4
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

	// Token: 0x06005E01 RID: 24065 RVA: 0x00248198 File Offset: 0x00246398
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

	// Token: 0x06005E02 RID: 24066 RVA: 0x00248278 File Offset: 0x00246478
	public bool HasNeighborChunks(Chunk _chunk)
	{
		this.sync.EnterReadLock();
		int x = _chunk.X;
		int z = _chunk.Z;
		bool flag = this.GetChunk(x + 1, z) != null && this.GetChunk(x - 1, z) != null && this.GetChunk(x, z + 1) != null && this.GetChunk(x, z - 1) != null && this.GetChunk(x + 1, z + 1) != null && this.GetChunk(x - 1, z - 1) != null && this.GetChunk(x - 1, z + 1) != null && this.GetChunk(x + 1, z - 1) != null;
		this.sync.ExitReadLock();
		return flag;
	}

	// Token: 0x06005E03 RID: 24067 RVA: 0x00248318 File Offset: 0x00246518
	[PublicizedFrom(EAccessModifier.Private)]
	public Chunk GetChunk(int _x, int _y)
	{
		long num = WorldChunkCache.MakeChunkKey(_x, _y);
		Chunk chunk;
		this.chunks.dict.TryGetValue(num, out chunk);
		return chunk;
	}

	// Token: 0x040048A8 RID: 18600
	public const long InvalidChunk = 9223372036854775807L;

	// Token: 0x040048A9 RID: 18601
	public Vector2i ChunkMinPos = Vector2i.zero;

	// Token: 0x040048AA RID: 18602
	public Vector2i ChunkMaxPos = Vector2i.zero;

	// Token: 0x040048AB RID: 18603
	public DictionaryLinkedList<long, Chunk> chunks = new DictionaryLinkedList<long, Chunk>();

	// Token: 0x040048AC RID: 18604
	[PublicizedFrom(EAccessModifier.Private)]
	public List<IChunkCallback> chunkCallbacks = new List<IChunkCallback>();

	// Token: 0x040048AD RID: 18605
	public HashSetLong chunkKeys = new HashSetLong();

	// Token: 0x040048AE RID: 18606
	[PublicizedFrom(EAccessModifier.Private)]
	public volatile bool isChunkKeysDirty = true;

	// Token: 0x040048AF RID: 18607
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSetLong chunkKeysCopy = new HashSetLong();

	// Token: 0x040048B0 RID: 18608
	[PublicizedFrom(EAccessModifier.Private)]
	public volatile bool isChunkArrayDirty = true;

	// Token: 0x040048B1 RID: 18609
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Chunk> chunkArrayCopy = new List<Chunk>();

	// Token: 0x040048B2 RID: 18610
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ReaderWriterLockSlim sync = new ReaderWriterLockSlim();
}
