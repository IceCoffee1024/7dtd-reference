using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Token: 0x02000BCF RID: 3023
public class TileAreaCache<[IsUnmanaged] T> : ITileArea<T[,]> where T : struct, ValueType
{
	// Token: 0x06005B1C RID: 23324 RVA: 0x0023039A File Offset: 0x0022E59A
	public TileAreaCache(TileAreaConfig _config, TileFile<T> _tileFile, int _cacheMax)
	{
		this.tilesDatabase = _tileFile;
		this.config = _config;
		this.cacheMax = _cacheMax;
	}

	// Token: 0x17000986 RID: 2438
	// (get) Token: 0x06005B1D RID: 23325 RVA: 0x002303CD File Offset: 0x0022E5CD
	public TileAreaConfig Config
	{
		get
		{
			return this.config;
		}
	}

	// Token: 0x17000987 RID: 2439
	public T[,] this[uint _key]
	{
		get
		{
			T[,] array;
			if (this.cache.TryGetValue(_key, out array))
			{
				this.PromoteEntry(_key);
				return array;
			}
			int tileXPos = TileAreaUtils.GetTileXPos(_key);
			int tileZPos = TileAreaUtils.GetTileZPos(_key);
			return this.Cache(_key, tileXPos, tileZPos);
		}
	}

	// Token: 0x17000988 RID: 2440
	public T[,] this[int _tileX, int _tileZ]
	{
		get
		{
			this.config.checkCoordinates(ref _tileX, ref _tileZ);
			uint num = TileAreaUtils.MakeKey(_tileX, _tileZ);
			T[,] array;
			if (this.cache.TryGetValue(num, out array))
			{
				this.PromoteEntry(num);
				return array;
			}
			return this.Cache(num, _tileX, _tileZ);
		}
	}

	// Token: 0x06005B20 RID: 23328 RVA: 0x00230460 File Offset: 0x0022E660
	[PublicizedFrom(EAccessModifier.Private)]
	public void PromoteEntry(uint _key)
	{
		for (LinkedListNode<uint> linkedListNode = this.cacheQueue.First; linkedListNode != this.cacheQueue.Last; linkedListNode = linkedListNode.Next)
		{
			if (linkedListNode.Value == _key)
			{
				this.cacheQueue.Remove(linkedListNode);
				this.cacheQueue.AddFirst(linkedListNode);
				return;
			}
		}
	}

	// Token: 0x06005B21 RID: 23329 RVA: 0x002304B4 File Offset: 0x0022E6B4
	[PublicizedFrom(EAccessModifier.Private)]
	public T[,] Cache(uint _key, int _tileX, int _tileZ)
	{
		int num = _tileX - this.config.tileStart.x;
		int num2 = _tileZ - this.config.tileStart.y;
		if (!this.tilesDatabase.IsInDatabase(num, num2))
		{
			return null;
		}
		LinkedListNode<uint> linkedListNode = null;
		T[,] array = null;
		if (this.cacheQueue.Count >= this.cacheMax)
		{
			linkedListNode = this.cacheQueue.Last;
			this.cacheQueue.Remove(linkedListNode);
			array = this.cache[linkedListNode.Value];
			this.cache.Remove(linkedListNode.Value);
		}
		this.tilesDatabase.LoadTile(num, num2, ref array);
		this.cache.Add(_key, array);
		if (linkedListNode != null)
		{
			linkedListNode.Value = _key;
			this.cacheQueue.AddFirst(linkedListNode);
		}
		else
		{
			this.cacheQueue.AddFirst(_key);
		}
		return array;
	}

	// Token: 0x06005B22 RID: 23330 RVA: 0x0023058D File Offset: 0x0022E78D
	public void Cleanup()
	{
		if (this.tilesDatabase != null)
		{
			this.tilesDatabase.Dispose();
			this.tilesDatabase = null;
		}
	}

	// Token: 0x040046B9 RID: 18105
	[PublicizedFrom(EAccessModifier.Private)]
	public TileFile<T> tilesDatabase;

	// Token: 0x040046BA RID: 18106
	[PublicizedFrom(EAccessModifier.Private)]
	public TileAreaConfig config;

	// Token: 0x040046BB RID: 18107
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<uint, T[,]> cache = new Dictionary<uint, T[,]>();

	// Token: 0x040046BC RID: 18108
	[PublicizedFrom(EAccessModifier.Private)]
	public LinkedList<uint> cacheQueue = new LinkedList<uint>();

	// Token: 0x040046BD RID: 18109
	[PublicizedFrom(EAccessModifier.Private)]
	public int cacheMax;
}
