using System;

// Token: 0x02000B2D RID: 2861
public class ChunkCache : IBlockAccess
{
	// Token: 0x060055AC RID: 21932 RVA: 0x0020F13C File Offset: 0x0020D33C
	public ChunkCache(int _dim)
	{
		this.chunkArray = new Chunk[_dim, _dim];
	}

	// Token: 0x060055AD RID: 21933 RVA: 0x0020F154 File Offset: 0x0020D354
	public void Init(World world, int x, int y, int z, int sx, int sy, int sz)
	{
		this.worldObj = world;
		this.chunkX = x >> 4;
		this.chunkZ = z >> 4;
		int num = sx >> 4;
		int num2 = sz >> 4;
		for (int i = this.chunkX; i <= num; i++)
		{
			for (int j = this.chunkZ; j <= num2; j++)
			{
				Chunk chunkSync = world.ChunkCache.GetChunkSync(i, j);
				if (chunkSync != null)
				{
					this.chunkArray[i - this.chunkX, j - this.chunkZ] = chunkSync;
				}
			}
		}
	}

	// Token: 0x060055AE RID: 21934 RVA: 0x0020F1D7 File Offset: 0x0020D3D7
	public void Clear()
	{
		Array.Clear(this.chunkArray, 0, this.chunkArray.GetLength(0) * this.chunkArray.GetLength(1));
	}

	// Token: 0x060055AF RID: 21935 RVA: 0x0020F1FE File Offset: 0x0020D3FE
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsChunkValid(Chunk _chunk)
	{
		return _chunk != null && _chunk.IsInitialized;
	}

	// Token: 0x060055B0 RID: 21936 RVA: 0x0020F20C File Offset: 0x0020D40C
	[PublicizedFrom(EAccessModifier.Private)]
	public bool TryGetChunk(int _cX, int _cY, out Chunk _chunk)
	{
		int num = _cX - this.chunkX;
		int num2 = _cY - this.chunkZ;
		Chunk chunk;
		if (num < 0 || num >= this.chunkArray.GetLength(0) || num2 < 0 || num2 >= this.chunkArray.GetLength(1))
		{
			chunk = this.worldObj.ChunkCache.GetChunkSync(_cX, _cY);
		}
		else
		{
			chunk = this.chunkArray[num, num2];
		}
		if (!this.IsChunkValid(chunk))
		{
			_chunk = null;
			return false;
		}
		_chunk = chunk;
		return true;
	}

	// Token: 0x060055B1 RID: 21937 RVA: 0x0020F288 File Offset: 0x0020D488
	public BlockValue GetBlock(int _x, int _y, int _z)
	{
		if (_y < 0)
		{
			return BlockValue.Air;
		}
		if (_y >= 256)
		{
			return BlockValue.Air;
		}
		Chunk chunk;
		if (!this.TryGetChunk(World.toChunkXZ(_x), World.toChunkXZ(_z), out chunk))
		{
			return BlockValue.Air;
		}
		return chunk.GetBlock(_x & 15, _y, _z & 15);
	}

	// Token: 0x060055B2 RID: 21938 RVA: 0x001B3814 File Offset: 0x001B1A14
	public BlockValue GetBlock(Vector3i pos)
	{
		return IBlockAccess.DefaultGetBlock(this, pos);
	}

	// Token: 0x060055B3 RID: 21939 RVA: 0x001B381D File Offset: 0x001B1A1D
	public BlockValue GetBlock(BlockValueRef bvRef)
	{
		return IBlockAccess.DefaultGetBlock(this, bvRef);
	}

	// Token: 0x060055B4 RID: 21940 RVA: 0x0020F2D8 File Offset: 0x0020D4D8
	public PropValue GetProp(int chunkX, int chunkZ, int propId)
	{
		Chunk chunk;
		if (!this.TryGetChunk(chunkX, chunkZ, out chunk))
		{
			return PropValue.AIR;
		}
		return chunk.GetProp(chunkX, chunkZ, propId);
	}

	// Token: 0x060055B5 RID: 21941 RVA: 0x0020F300 File Offset: 0x0020D500
	public PropValue GetProp(long chunkKey, int propId)
	{
		return this.GetProp(WorldChunkCache.extractX(chunkKey), WorldChunkCache.extractZ(chunkKey), propId);
	}

	// Token: 0x060055B6 RID: 21942 RVA: 0x001B39A9 File Offset: 0x001B1BA9
	public PropValue GetProp(Vector2i chunkPos, int propId)
	{
		return IBlockAccess.DefaultGetProp(this, chunkPos, propId);
	}

	// Token: 0x060055B7 RID: 21943 RVA: 0x001B39B3 File Offset: 0x001B1BB3
	public PropValue GetProp(PropRef propRef)
	{
		return IBlockAccess.DefaultGetProp(this, propRef);
	}

	// Token: 0x040042A9 RID: 17065
	[PublicizedFrom(EAccessModifier.Private)]
	public int chunkX;

	// Token: 0x040042AA RID: 17066
	[PublicizedFrom(EAccessModifier.Private)]
	public int chunkZ;

	// Token: 0x040042AB RID: 17067
	[PublicizedFrom(EAccessModifier.Private)]
	public Chunk[,] chunkArray;

	// Token: 0x040042AC RID: 17068
	[PublicizedFrom(EAccessModifier.Private)]
	public World worldObj;
}
