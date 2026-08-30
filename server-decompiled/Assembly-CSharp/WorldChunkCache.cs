using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

// Token: 0x02000BF6 RID: 3062
public class WorldChunkCache : IChunkAccess
{
	// Token: 0x06005D58 RID: 23896 RVA: 0x0004D782 File Offset: 0x0004B982
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static long MakeChunkKey(int x, int y)
	{
		return (((long)y & 16777215L) << 24) | ((long)x & 16777215L);
	}

	// Token: 0x06005D59 RID: 23897 RVA: 0x00244EBE File Offset: 0x002430BE
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int extractX(long key)
	{
		return (int)((int)key << 8) >> 8;
	}

	// Token: 0x06005D5A RID: 23898 RVA: 0x00244EC6 File Offset: 0x002430C6
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int extractZ(long key)
	{
		return (int)(key >> 16) >> 8;
	}

	// Token: 0x06005D5B RID: 23899 RVA: 0x00244ED0 File Offset: 0x002430D0
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2i extractXZ(long key)
	{
		Vector2i vector2i;
		vector2i.x = WorldChunkCache.extractX(key);
		vector2i.y = WorldChunkCache.extractZ(key);
		return vector2i;
	}

	// Token: 0x06005D5C RID: 23900 RVA: 0x00244EF8 File Offset: 0x002430F8
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Chunk GetChunkSync(int _x, int _y)
	{
		return this.GetChunkSync(WorldChunkCache.MakeChunkKey(_x, _y));
	}

	// Token: 0x06005D5D RID: 23901 RVA: 0x00244F08 File Offset: 0x00243108
	public virtual Chunk GetChunkSync(long _key)
	{
		this.sync.EnterReadLock();
		Chunk chunk;
		this.chunks.dict.TryGetValue(_key & 72057594037927935L, out chunk);
		this.sync.ExitReadLock();
		return chunk;
	}

	// Token: 0x06005D5E RID: 23902 RVA: 0x00244F4A File Offset: 0x0024314A
	[PublicizedFrom(EAccessModifier.Private)]
	public IChunk IChunkAccess.GetChunkSync(int chunkX, int chunkZ)
	{
		return this.GetChunkSync(chunkX, chunkZ);
	}

	// Token: 0x06005D5F RID: 23903 RVA: 0x00244F54 File Offset: 0x00243154
	[PublicizedFrom(EAccessModifier.Private)]
	public IChunk IChunkAccess.GetChunkSync(long chunkKey)
	{
		return this.GetChunkSync(chunkKey);
	}

	// Token: 0x06005D60 RID: 23904 RVA: 0x001AEC68 File Offset: 0x001ACE68
	public IChunk GetChunkSync(int chunkX, int chunkY, int chunkZ)
	{
		return IChunkAccess.DefaultGetChunkSync(this, chunkX, chunkY, chunkZ);
	}

	// Token: 0x06005D61 RID: 23905 RVA: 0x001AEC73 File Offset: 0x001ACE73
	public IChunk GetChunkSync(Vector2i chunkPos)
	{
		return IChunkAccess.DefaultGetChunkSync(this, chunkPos);
	}

	// Token: 0x06005D62 RID: 23906 RVA: 0x001AEC7C File Offset: 0x001ACE7C
	public IChunk GetChunkSync(PropRef propRef)
	{
		return IChunkAccess.DefaultGetChunkSync(this, propRef);
	}

	// Token: 0x06005D63 RID: 23907 RVA: 0x001AEC85 File Offset: 0x001ACE85
	public IChunk GetChunkSync(BlockValueRef bvRef)
	{
		return IChunkAccess.DefaultGetChunkSync(this, bvRef);
	}

	// Token: 0x06005D64 RID: 23908 RVA: 0x001AEC8E File Offset: 0x001ACE8E
	public IChunk GetChunkFromWorldPos(int x, int z)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, x, z);
	}

	// Token: 0x06005D65 RID: 23909 RVA: 0x001AEC98 File Offset: 0x001ACE98
	public IChunk GetChunkFromWorldPos(int x, int y, int z)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, x, y, z);
	}

	// Token: 0x06005D66 RID: 23910 RVA: 0x001AECA3 File Offset: 0x001ACEA3
	public IChunk GetChunkFromWorldPos(Vector3i blockPos)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, blockPos);
	}

	// Token: 0x06005D67 RID: 23911 RVA: 0x001AECAC File Offset: 0x001ACEAC
	public bool GetChunkFromWorldPos(int x, int z, ref IChunk chunk)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, x, z, ref chunk);
	}

	// Token: 0x06005D68 RID: 23912 RVA: 0x001AECB7 File Offset: 0x001ACEB7
	public bool GetChunkFromWorldPos(Vector3i blockPos, ref IChunk chunk)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, blockPos, ref chunk);
	}

	// Token: 0x06005D69 RID: 23913 RVA: 0x00244F60 File Offset: 0x00243160
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

	// Token: 0x06005D6A RID: 23914 RVA: 0x00245004 File Offset: 0x00243204
	public void NotifyOnChunkBeforeSave(Chunk _c)
	{
		for (int i = 0; i < this.chunkCallbacks.Count; i++)
		{
			this.chunkCallbacks[i].OnChunkBeforeSave(_c);
		}
	}

	// Token: 0x06005D6B RID: 23915 RVA: 0x0024503C File Offset: 0x0024323C
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

	// Token: 0x06005D6C RID: 23916 RVA: 0x00245168 File Offset: 0x00243368
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

	// Token: 0x06005D6D RID: 23917 RVA: 0x002451D5 File Offset: 0x002433D5
	public LinkedList<Chunk> GetChunkArray()
	{
		return this.chunks.list;
	}

	// Token: 0x06005D6E RID: 23918 RVA: 0x002451E4 File Offset: 0x002433E4
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

	// Token: 0x06005D6F RID: 23919 RVA: 0x00245278 File Offset: 0x00243478
	public int Count()
	{
		return this.chunks.list.Count;
	}

	// Token: 0x06005D70 RID: 23920 RVA: 0x0024528A File Offset: 0x0024348A
	public ReaderWriterLockSlim GetSyncRoot()
	{
		return this.sync;
	}

	// Token: 0x06005D71 RID: 23921 RVA: 0x00245292 File Offset: 0x00243492
	public virtual bool ContainsChunkSync(long key)
	{
		this.sync.EnterReadLock();
		bool flag = this.chunks.dict.ContainsKey(key);
		this.sync.ExitReadLock();
		return flag;
	}

	// Token: 0x06005D72 RID: 23922 RVA: 0x002452BB File Offset: 0x002434BB
	public void AddChunkCallback(IChunkCallback _callback)
	{
		this.chunkCallbacks.Add(_callback);
	}

	// Token: 0x06005D73 RID: 23923 RVA: 0x002452C9 File Offset: 0x002434C9
	public void RemoveChunkCallback(IChunkCallback _callback)
	{
		this.chunkCallbacks.Remove(_callback);
	}

	// Token: 0x06005D74 RID: 23924 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Update()
	{
	}

	// Token: 0x06005D75 RID: 23925 RVA: 0x002452D8 File Offset: 0x002434D8
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

	// Token: 0x06005D76 RID: 23926 RVA: 0x00245340 File Offset: 0x00243540
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

	// Token: 0x06005D77 RID: 23927 RVA: 0x002453F4 File Offset: 0x002435F4
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

	// Token: 0x06005D78 RID: 23928 RVA: 0x002454D4 File Offset: 0x002436D4
	public bool HasNeighborChunks(Chunk _chunk)
	{
		this.sync.EnterReadLock();
		int x = _chunk.X;
		int z = _chunk.Z;
		bool flag = this.GetChunk(x + 1, z) != null && this.GetChunk(x - 1, z) != null && this.GetChunk(x, z + 1) != null && this.GetChunk(x, z - 1) != null && this.GetChunk(x + 1, z + 1) != null && this.GetChunk(x - 1, z - 1) != null && this.GetChunk(x - 1, z + 1) != null && this.GetChunk(x + 1, z - 1) != null;
		this.sync.ExitReadLock();
		return flag;
	}

	// Token: 0x06005D79 RID: 23929 RVA: 0x00245574 File Offset: 0x00243774
	[PublicizedFrom(EAccessModifier.Private)]
	public Chunk GetChunk(int _x, int _y)
	{
		long num = WorldChunkCache.MakeChunkKey(_x, _y);
		Chunk chunk;
		this.chunks.dict.TryGetValue(num, out chunk);
		return chunk;
	}

	// Token: 0x0400480C RID: 18444
	public const long InvalidChunk = 9223372036854775807L;

	// Token: 0x0400480D RID: 18445
	public Vector2i ChunkMinPos = Vector2i.zero;

	// Token: 0x0400480E RID: 18446
	public Vector2i ChunkMaxPos = Vector2i.zero;

	// Token: 0x0400480F RID: 18447
	public DictionaryLinkedList<long, Chunk> chunks = new DictionaryLinkedList<long, Chunk>();

	// Token: 0x04004810 RID: 18448
	[PublicizedFrom(EAccessModifier.Private)]
	public List<IChunkCallback> chunkCallbacks = new List<IChunkCallback>();

	// Token: 0x04004811 RID: 18449
	public HashSetLong chunkKeys = new HashSetLong();

	// Token: 0x04004812 RID: 18450
	[PublicizedFrom(EAccessModifier.Private)]
	public volatile bool isChunkKeysDirty = true;

	// Token: 0x04004813 RID: 18451
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSetLong chunkKeysCopy = new HashSetLong();

	// Token: 0x04004814 RID: 18452
	[PublicizedFrom(EAccessModifier.Private)]
	public volatile bool isChunkArrayDirty = true;

	// Token: 0x04004815 RID: 18453
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Chunk> chunkArrayCopy = new List<Chunk>();

	// Token: 0x04004816 RID: 18454
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ReaderWriterLockSlim sync = new ReaderWriterLockSlim();
}
