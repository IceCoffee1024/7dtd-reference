using System;

// Token: 0x02000B22 RID: 2850
public class ChunkCache : IBlockAccess
{
	// Token: 0x06005587 RID: 21895 RVA: 0x0020DBC0 File Offset: 0x0020BDC0
	public ChunkCache(int _dim)
	{
		this.chunkArray = new Chunk[_dim, _dim];
	}

	// Token: 0x06005588 RID: 21896 RVA: 0x0020DBD8 File Offset: 0x0020BDD8
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

	// Token: 0x06005589 RID: 21897 RVA: 0x0020DC5B File Offset: 0x0020BE5B
	public void Clear()
	{
		Array.Clear(this.chunkArray, 0, this.chunkArray.GetLength(0) * this.chunkArray.GetLength(1));
	}

	// Token: 0x0600558A RID: 21898 RVA: 0x0020DC82 File Offset: 0x0020BE82
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsChunkValid(Chunk _chunk)
	{
		return _chunk != null && _chunk.IsInitialized;
	}

	// Token: 0x0600558B RID: 21899 RVA: 0x0020DC90 File Offset: 0x0020BE90
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

	// Token: 0x0600558C RID: 21900 RVA: 0x0020DD0C File Offset: 0x0020BF0C
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

	// Token: 0x0600558D RID: 21901 RVA: 0x001B2790 File Offset: 0x001B0990
	public BlockValue GetBlock(Vector3i pos)
	{
		return IBlockAccess.DefaultGetBlock(this, pos);
	}

	// Token: 0x0600558E RID: 21902 RVA: 0x001B2799 File Offset: 0x001B0999
	public BlockValue GetBlock(BlockValueRef bvRef)
	{
		return IBlockAccess.DefaultGetBlock(this, bvRef);
	}

	// Token: 0x0600558F RID: 21903 RVA: 0x0020DD5C File Offset: 0x0020BF5C
	public PropValue GetProp(int chunkX, int chunkZ, int propId)
	{
		Chunk chunk;
		if (!this.TryGetChunk(chunkX, chunkZ, out chunk))
		{
			return PropValue.AIR;
		}
		return chunk.GetProp(chunkX, chunkZ, propId);
	}

	// Token: 0x06005590 RID: 21904 RVA: 0x0020DD84 File Offset: 0x0020BF84
	public PropValue GetProp(long chunkKey, int propId)
	{
		return this.GetProp(WorldChunkCache.extractX(chunkKey), WorldChunkCache.extractZ(chunkKey), propId);
	}

	// Token: 0x06005591 RID: 21905 RVA: 0x001B2925 File Offset: 0x001B0B25
	public PropValue GetProp(Vector2i chunkPos, int propId)
	{
		return IBlockAccess.DefaultGetProp(this, chunkPos, propId);
	}

	// Token: 0x06005592 RID: 21906 RVA: 0x001B292F File Offset: 0x001B0B2F
	public PropValue GetProp(PropRef propRef)
	{
		return IBlockAccess.DefaultGetProp(this, propRef);
	}

	// Token: 0x04004284 RID: 17028
	[PublicizedFrom(EAccessModifier.Private)]
	public int chunkX;

	// Token: 0x04004285 RID: 17029
	[PublicizedFrom(EAccessModifier.Private)]
	public int chunkZ;

	// Token: 0x04004286 RID: 17030
	[PublicizedFrom(EAccessModifier.Private)]
	public Chunk[,] chunkArray;

	// Token: 0x04004287 RID: 17031
	[PublicizedFrom(EAccessModifier.Private)]
	public World worldObj;
}
