using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Token: 0x02000BBD RID: 3005
public class TileAreaCache<[IsUnmanaged] T> : ITileArea<T[,]> where T : struct, ValueType
{
	// Token: 0x06005A94 RID: 23188 RVA: 0x0022D632 File Offset: 0x0022B832
	public TileAreaCache(TileAreaConfig _config, TileFile<T> _tileFile, int _cacheMax)
	{
		this.tilesDatabase = _tileFile;
		this.config = _config;
		this.cacheMax = _cacheMax;
	}

	// Token: 0x17000974 RID: 2420
	// (get) Token: 0x06005A95 RID: 23189 RVA: 0x0022D665 File Offset: 0x0022B865
	public TileAreaConfig Config
	{
		get
		{
			return this.config;
		}
	}

	// Token: 0x17000975 RID: 2421
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

	// Token: 0x17000976 RID: 2422
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

	// Token: 0x06005A98 RID: 23192 RVA: 0x0022D6F8 File Offset: 0x0022B8F8
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

	// Token: 0x06005A99 RID: 23193 RVA: 0x0022D74C File Offset: 0x0022B94C
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

	// Token: 0x06005A9A RID: 23194 RVA: 0x0022D825 File Offset: 0x0022BA25
	public void Cleanup()
	{
		if (this.tilesDatabase != null)
		{
			this.tilesDatabase.Dispose();
			this.tilesDatabase = null;
		}
	}

	// Token: 0x0400461D RID: 17949
	[PublicizedFrom(EAccessModifier.Private)]
	public TileFile<T> tilesDatabase;

	// Token: 0x0400461E RID: 17950
	[PublicizedFrom(EAccessModifier.Private)]
	public TileAreaConfig config;

	// Token: 0x0400461F RID: 17951
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<uint, T[,]> cache = new Dictionary<uint, T[,]>();

	// Token: 0x04004620 RID: 17952
	[PublicizedFrom(EAccessModifier.Private)]
	public LinkedList<uint> cacheQueue = new LinkedList<uint>();

	// Token: 0x04004621 RID: 17953
	[PublicizedFrom(EAccessModifier.Private)]
	public int cacheMax;
}
