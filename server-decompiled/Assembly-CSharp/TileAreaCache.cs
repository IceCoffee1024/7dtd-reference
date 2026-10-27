using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Token: 0x02000BEC RID: 3052
public class TileAreaCache<[IsUnmanaged] T> : ITileArea<T[,]> where T : struct, ValueType
{
	// Token: 0x06005B5A RID: 23386 RVA: 0x00234BF2 File Offset: 0x00232DF2
	public TileAreaCache(TileAreaConfig _config, TileFile<T> _tileFile, int _cacheMax)
	{
		this.tilesDatabase = _tileFile;
		this.config = _config;
		this.cacheMax = _cacheMax;
	}

	// Token: 0x170009AE RID: 2478
	// (get) Token: 0x06005B5B RID: 23387 RVA: 0x00234C25 File Offset: 0x00232E25
	public TileAreaConfig Config
	{
		get
		{
			return this.config;
		}
	}

	// Token: 0x170009AF RID: 2479
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

	// Token: 0x170009B0 RID: 2480
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

	// Token: 0x06005B5E RID: 23390 RVA: 0x00234CB8 File Offset: 0x00232EB8
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

	// Token: 0x06005B5F RID: 23391 RVA: 0x00234D0C File Offset: 0x00232F0C
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

	// Token: 0x06005B60 RID: 23392 RVA: 0x00234DE5 File Offset: 0x00232FE5
	public void Cleanup()
	{
		if (this.tilesDatabase != null)
		{
			this.tilesDatabase.Dispose();
			this.tilesDatabase = null;
		}
	}

	// Token: 0x0400471D RID: 18205
	[PublicizedFrom(EAccessModifier.Private)]
	public TileFile<T> tilesDatabase;

	// Token: 0x0400471E RID: 18206
	[PublicizedFrom(EAccessModifier.Private)]
	public TileAreaConfig config;

	// Token: 0x0400471F RID: 18207
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<uint, T[,]> cache = new Dictionary<uint, T[,]>();

	// Token: 0x04004720 RID: 18208
	[PublicizedFrom(EAccessModifier.Private)]
	public LinkedList<uint> cacheQueue = new LinkedList<uint>();

	// Token: 0x04004721 RID: 18209
	[PublicizedFrom(EAccessModifier.Private)]
	public int cacheMax;
}
