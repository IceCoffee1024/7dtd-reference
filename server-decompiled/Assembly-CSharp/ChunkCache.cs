using System;

// Token: 0x02000AFE RID: 2814
public class ChunkCache : IBlockAccess
{
	// Token: 0x060054E6 RID: 21734 RVA: 0x00207C34 File Offset: 0x00205E34
	public ChunkCache(int _dim)
	{
		this.chunkArray = new Chunk[_dim, _dim];
	}

	// Token: 0x060054E7 RID: 21735 RVA: 0x00207C4C File Offset: 0x00205E4C
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

	// Token: 0x060054E8 RID: 21736 RVA: 0x00207CCF File Offset: 0x00205ECF
	public void Clear()
	{
		Array.Clear(this.chunkArray, 0, this.chunkArray.GetLength(0) * this.chunkArray.GetLength(1));
	}

	// Token: 0x060054E9 RID: 21737 RVA: 0x00207CF6 File Offset: 0x00205EF6
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsChunkValid(Chunk _chunk)
	{
		return _chunk != null && _chunk.IsInitialized;
	}

	// Token: 0x060054EA RID: 21738 RVA: 0x00207D04 File Offset: 0x00205F04
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

	// Token: 0x060054EB RID: 21739 RVA: 0x00207D80 File Offset: 0x00205F80
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

	// Token: 0x060054EC RID: 21740 RVA: 0x001AFD64 File Offset: 0x001ADF64
	public BlockValue GetBlock(Vector3i pos)
	{
		return IBlockAccess.DefaultGetBlock(this, pos);
	}

	// Token: 0x060054ED RID: 21741 RVA: 0x001AFD6D File Offset: 0x001ADF6D
	public BlockValue GetBlock(BlockValueRef bvRef)
	{
		return IBlockAccess.DefaultGetBlock(this, bvRef);
	}

	// Token: 0x060054EE RID: 21742 RVA: 0x00207DD0 File Offset: 0x00205FD0
	public PropValue GetProp(int chunkX, int chunkZ, int propId)
	{
		Chunk chunk;
		if (!this.TryGetChunk(chunkX, chunkZ, out chunk))
		{
			return PropValue.AIR;
		}
		return chunk.GetProp(chunkX, chunkZ, propId);
	}

	// Token: 0x060054EF RID: 21743 RVA: 0x00207DF8 File Offset: 0x00205FF8
	public PropValue GetProp(long chunkKey, int propId)
	{
		return this.GetProp(WorldChunkCache.extractX(chunkKey), WorldChunkCache.extractZ(chunkKey), propId);
	}

	// Token: 0x060054F0 RID: 21744 RVA: 0x001AFEF9 File Offset: 0x001AE0F9
	public PropValue GetProp(Vector2i chunkPos, int propId)
	{
		return IBlockAccess.DefaultGetProp(this, chunkPos, propId);
	}

	// Token: 0x060054F1 RID: 21745 RVA: 0x001AFF03 File Offset: 0x001AE103
	public PropValue GetProp(PropRef propRef)
	{
		return IBlockAccess.DefaultGetProp(this, propRef);
	}

	// Token: 0x040041AC RID: 16812
	[PublicizedFrom(EAccessModifier.Private)]
	public int chunkX;

	// Token: 0x040041AD RID: 16813
	[PublicizedFrom(EAccessModifier.Private)]
	public int chunkZ;

	// Token: 0x040041AE RID: 16814
	[PublicizedFrom(EAccessModifier.Private)]
	public Chunk[,] chunkArray;

	// Token: 0x040041AF RID: 16815
	[PublicizedFrom(EAccessModifier.Private)]
	public World worldObj;
}
