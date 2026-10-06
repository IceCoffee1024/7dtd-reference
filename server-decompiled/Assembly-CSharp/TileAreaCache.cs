using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Token: 0x02000BE1 RID: 3041
public class TileAreaCache<[IsUnmanaged] T> : ITileArea<T[,]> where T : struct, ValueType
{
	// Token: 0x06005B35 RID: 23349 RVA: 0x00233696 File Offset: 0x00231896
	public TileAreaCache(TileAreaConfig _config, TileFile<T> _tileFile, int _cacheMax)
	{
		this.tilesDatabase = _tileFile;
		this.config = _config;
		this.cacheMax = _cacheMax;
	}

	// Token: 0x170009A8 RID: 2472
	// (get) Token: 0x06005B36 RID: 23350 RVA: 0x002336C9 File Offset: 0x002318C9
	public TileAreaConfig Config
	{
		get
		{
			return this.config;
		}
	}

	// Token: 0x170009A9 RID: 2473
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

	// Token: 0x170009AA RID: 2474
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

	// Token: 0x06005B39 RID: 23353 RVA: 0x0023375C File Offset: 0x0023195C
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

	// Token: 0x06005B3A RID: 23354 RVA: 0x002337B0 File Offset: 0x002319B0
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

	// Token: 0x06005B3B RID: 23355 RVA: 0x00233889 File Offset: 0x00231A89
	public void Cleanup()
	{
		if (this.tilesDatabase != null)
		{
			this.tilesDatabase.Dispose();
			this.tilesDatabase = null;
		}
	}

	// Token: 0x040046F8 RID: 18168
	[PublicizedFrom(EAccessModifier.Private)]
	public TileFile<T> tilesDatabase;

	// Token: 0x040046F9 RID: 18169
	[PublicizedFrom(EAccessModifier.Private)]
	public TileAreaConfig config;

	// Token: 0x040046FA RID: 18170
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<uint, T[,]> cache = new Dictionary<uint, T[,]>();

	// Token: 0x040046FB RID: 18171
	[PublicizedFrom(EAccessModifier.Private)]
	public LinkedList<uint> cacheQueue = new LinkedList<uint>();

	// Token: 0x040046FC RID: 18172
	[PublicizedFrom(EAccessModifier.Private)]
	public int cacheMax;
}
