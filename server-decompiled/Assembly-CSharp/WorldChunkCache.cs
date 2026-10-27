using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

// Token: 0x02000C25 RID: 3109
public class WorldChunkCache : IChunkAccess
{
	// Token: 0x06005E20 RID: 24096 RVA: 0x0004DB5E File Offset: 0x0004BD5E
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static long MakeChunkKey(int x, int y)
	{
		return (((long)y & 16777215L) << 24) | ((long)x & 16777215L);
	}

	// Token: 0x06005E21 RID: 24097 RVA: 0x0024C486 File Offset: 0x0024A686
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int extractX(long key)
	{
		return (int)((int)key << 8) >> 8;
	}

	// Token: 0x06005E22 RID: 24098 RVA: 0x0024C48E File Offset: 0x0024A68E
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int extractZ(long key)
	{
		return (int)(key >> 16) >> 8;
	}

	// Token: 0x06005E23 RID: 24099 RVA: 0x0024C498 File Offset: 0x0024A698
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2i extractXZ(long key)
	{
		Vector2i vector2i;
		vector2i.x = WorldChunkCache.extractX(key);
		vector2i.y = WorldChunkCache.extractZ(key);
		return vector2i;
	}

	// Token: 0x06005E24 RID: 24100 RVA: 0x0024C4C0 File Offset: 0x0024A6C0
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Chunk GetChunkSync(int _x, int _y)
	{
		return this.GetChunkSync(WorldChunkCache.MakeChunkKey(_x, _y));
	}

	// Token: 0x06005E25 RID: 24101 RVA: 0x0024C4D0 File Offset: 0x0024A6D0
	public virtual Chunk GetChunkSync(long _key)
	{
		this.sync.EnterReadLock();
		Chunk chunk;
		this.chunks.dict.TryGetValue(_key & 72057594037927935L, out chunk);
		this.sync.ExitReadLock();
		return chunk;
	}

	// Token: 0x06005E26 RID: 24102 RVA: 0x0024C512 File Offset: 0x0024A712
	[PublicizedFrom(EAccessModifier.Private)]
	public IChunk IChunkAccess.GetChunkSync(int chunkX, int chunkZ)
	{
		return this.GetChunkSync(chunkX, chunkZ);
	}

	// Token: 0x06005E27 RID: 24103 RVA: 0x0024C51C File Offset: 0x0024A71C
	[PublicizedFrom(EAccessModifier.Private)]
	public IChunk IChunkAccess.GetChunkSync(long chunkKey)
	{
		return this.GetChunkSync(chunkKey);
	}

	// Token: 0x06005E28 RID: 24104 RVA: 0x001B24CC File Offset: 0x001B06CC
	public IChunk GetChunkSync(int chunkX, int chunkY, int chunkZ)
	{
		return IChunkAccess.DefaultGetChunkSync(this, chunkX, chunkY, chunkZ);
	}

	// Token: 0x06005E29 RID: 24105 RVA: 0x001B24D7 File Offset: 0x001B06D7
	public IChunk GetChunkSync(Vector2i chunkPos)
	{
		return IChunkAccess.DefaultGetChunkSync(this, chunkPos);
	}

	// Token: 0x06005E2A RID: 24106 RVA: 0x001B24E0 File Offset: 0x001B06E0
	public IChunk GetChunkSync(PropRef propRef)
	{
		return IChunkAccess.DefaultGetChunkSync(this, propRef);
	}

	// Token: 0x06005E2B RID: 24107 RVA: 0x001B24E9 File Offset: 0x001B06E9
	public IChunk GetChunkSync(BlockValueRef bvRef)
	{
		return IChunkAccess.DefaultGetChunkSync(this, bvRef);
	}

	// Token: 0x06005E2C RID: 24108 RVA: 0x001B24F2 File Offset: 0x001B06F2
	public IChunk GetChunkFromWorldPos(int x, int z)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, x, z);
	}

	// Token: 0x06005E2D RID: 24109 RVA: 0x001B24FC File Offset: 0x001B06FC
	public IChunk GetChunkFromWorldPos(int x, int y, int z)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, x, y, z);
	}

	// Token: 0x06005E2E RID: 24110 RVA: 0x001B2507 File Offset: 0x001B0707
	public IChunk GetChunkFromWorldPos(Vector3i blockPos)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, blockPos);
	}

	// Token: 0x06005E2F RID: 24111 RVA: 0x001B2510 File Offset: 0x001B0710
	public bool GetChunkFromWorldPos(int x, int z, ref IChunk chunk)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, x, z, ref chunk);
	}

	// Token: 0x06005E30 RID: 24112 RVA: 0x001B251B File Offset: 0x001B071B
	public bool GetChunkFromWorldPos(Vector3i blockPos, ref IChunk chunk)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, blockPos, ref chunk);
	}

	// Token: 0x06005E31 RID: 24113 RVA: 0x0024C528 File Offset: 0x0024A728
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

	// Token: 0x06005E32 RID: 24114 RVA: 0x0024C5CC File Offset: 0x0024A7CC
	public void NotifyOnChunkBeforeSave(Chunk _c)
	{
		for (int i = 0; i < this.chunkCallbacks.Count; i++)
		{
			this.chunkCallbacks[i].OnChunkBeforeSave(_c);
		}
	}

	// Token: 0x06005E33 RID: 24115 RVA: 0x0024C604 File Offset: 0x0024A804
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

	// Token: 0x06005E34 RID: 24116 RVA: 0x0024C730 File Offset: 0x0024A930
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

	// Token: 0x06005E35 RID: 24117 RVA: 0x0024C79D File Offset: 0x0024A99D
	public LinkedList<Chunk> GetChunkArray()
	{
		return this.chunks.list;
	}

	// Token: 0x06005E36 RID: 24118 RVA: 0x0024C7AC File Offset: 0x0024A9AC
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

	// Token: 0x06005E37 RID: 24119 RVA: 0x0024C840 File Offset: 0x0024AA40
	public int Count()
	{
		return this.chunks.list.Count;
	}

	// Token: 0x06005E38 RID: 24120 RVA: 0x0024C852 File Offset: 0x0024AA52
	public ReaderWriterLockSlim GetSyncRoot()
	{
		return this.sync;
	}

	// Token: 0x06005E39 RID: 24121 RVA: 0x0024C85A File Offset: 0x0024AA5A
	public virtual bool ContainsChunkSync(long key)
	{
		this.sync.EnterReadLock();
		bool flag = this.chunks.dict.ContainsKey(key);
		this.sync.ExitReadLock();
		return flag;
	}

	// Token: 0x06005E3A RID: 24122 RVA: 0x0024C883 File Offset: 0x0024AA83
	public void AddChunkCallback(IChunkCallback _callback)
	{
		this.chunkCallbacks.Add(_callback);
	}

	// Token: 0x06005E3B RID: 24123 RVA: 0x0024C891 File Offset: 0x0024AA91
	public void RemoveChunkCallback(IChunkCallback _callback)
	{
		this.chunkCallbacks.Remove(_callback);
	}

	// Token: 0x06005E3C RID: 24124 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Update()
	{
	}

	// Token: 0x06005E3D RID: 24125 RVA: 0x0024C8A0 File Offset: 0x0024AAA0
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

	// Token: 0x06005E3E RID: 24126 RVA: 0x0024C908 File Offset: 0x0024AB08
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

	// Token: 0x06005E3F RID: 24127 RVA: 0x0024C9BC File Offset: 0x0024ABBC
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

	// Token: 0x06005E40 RID: 24128 RVA: 0x0024CA9C File Offset: 0x0024AC9C
	public bool HasNeighborChunks(Chunk _chunk)
	{
		this.sync.EnterReadLock();
		int x = _chunk.X;
		int z = _chunk.Z;
		bool flag = this.GetChunk(x + 1, z) != null && this.GetChunk(x - 1, z) != null && this.GetChunk(x, z + 1) != null && this.GetChunk(x, z - 1) != null && this.GetChunk(x + 1, z + 1) != null && this.GetChunk(x - 1, z - 1) != null && this.GetChunk(x - 1, z + 1) != null && this.GetChunk(x + 1, z - 1) != null;
		this.sync.ExitReadLock();
		return flag;
	}

	// Token: 0x06005E41 RID: 24129 RVA: 0x0024CB3C File Offset: 0x0024AD3C
	[PublicizedFrom(EAccessModifier.Private)]
	public Chunk GetChunk(int _x, int _y)
	{
		long num = WorldChunkCache.MakeChunkKey(_x, _y);
		Chunk chunk;
		this.chunks.dict.TryGetValue(num, out chunk);
		return chunk;
	}

	// Token: 0x0400490C RID: 18700
	public const long InvalidChunk = 9223372036854775807L;

	// Token: 0x0400490D RID: 18701
	public Vector2i ChunkMinPos = Vector2i.zero;

	// Token: 0x0400490E RID: 18702
	public Vector2i ChunkMaxPos = Vector2i.zero;

	// Token: 0x0400490F RID: 18703
	public DictionaryLinkedList<long, Chunk> chunks = new DictionaryLinkedList<long, Chunk>();

	// Token: 0x04004910 RID: 18704
	[PublicizedFrom(EAccessModifier.Private)]
	public List<IChunkCallback> chunkCallbacks = new List<IChunkCallback>();

	// Token: 0x04004911 RID: 18705
	public HashSetLong chunkKeys = new HashSetLong();

	// Token: 0x04004912 RID: 18706
	[PublicizedFrom(EAccessModifier.Private)]
	public volatile bool isChunkKeysDirty = true;

	// Token: 0x04004913 RID: 18707
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSetLong chunkKeysCopy = new HashSetLong();

	// Token: 0x04004914 RID: 18708
	[PublicizedFrom(EAccessModifier.Private)]
	public volatile bool isChunkArrayDirty = true;

	// Token: 0x04004915 RID: 18709
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Chunk> chunkArrayCopy = new List<Chunk>();

	// Token: 0x04004916 RID: 18710
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ReaderWriterLockSlim sync = new ReaderWriterLockSlim();
}
