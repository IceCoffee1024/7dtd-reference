using System;

// Token: 0x02000B24 RID: 2852
public class ChunkCacheNeighborBlocks : INeighborBlockCache
{
	// Token: 0x0600559C RID: 21916 RVA: 0x0020DD9C File Offset: 0x0020BF9C
	public ChunkCacheNeighborBlocks(ChunkCacheNeighborChunks _nChunks)
	{
		IChunk[] array = new Chunk[9];
		this.chunks = array;
		this.centerBX = int.MaxValue;
		this.centerBZ = int.MaxValue;
		this.curChunkX = int.MaxValue;
		this.curChunkZ = int.MaxValue;
		this.firstException = true;
		base..ctor();
		this.nChunks = _nChunks;
	}

	// Token: 0x0600559D RID: 21917 RVA: 0x0020DDF8 File Offset: 0x0020BFF8
	public void Init(int _bX, int _bZ)
	{
		IChunk chunk = this.nChunks[0, 0];
		if (_bX == this.centerBX && _bZ == this.centerBZ && chunk.X == this.curChunkX && chunk.Z == this.curChunkZ)
		{
			return;
		}
		this.centerBX = _bX;
		this.centerBZ = _bZ;
		this.curChunkX = chunk.X;
		this.curChunkZ = chunk.Z;
		this.chunks[0] = ((_bX > 0) ? ((_bZ > 0) ? chunk : this.nChunks[0, -1]) : ((_bZ > 0) ? this.nChunks[-1, 0] : this.nChunks[-1, -1]));
		this.chunks[1] = ((_bZ > 0) ? chunk : this.nChunks[0, -1]);
		this.chunks[2] = ((_bX < 15) ? ((_bZ > 0) ? chunk : this.nChunks[0, -1]) : ((_bZ > 0) ? this.nChunks[1, 0] : this.nChunks[1, -1]));
		this.chunks[3] = ((_bX > 0) ? chunk : this.nChunks[-1, 0]);
		this.chunks[4] = chunk;
		this.chunks[5] = ((_bX < 15) ? chunk : this.nChunks[1, 0]);
		this.chunks[6] = ((_bX > 0) ? ((_bZ < 15) ? chunk : this.nChunks[0, 1]) : ((_bZ < 15) ? this.nChunks[-1, 0] : this.nChunks[-1, 1]));
		this.chunks[7] = ((_bZ < 15) ? chunk : this.nChunks[0, 1]);
		this.chunks[8] = ((_bX < 15) ? ((_bZ < 15) ? chunk : this.nChunks[0, 1]) : ((_bZ < 15) ? this.nChunks[1, 0] : this.nChunks[1, 1]));
	}

	// Token: 0x0600559E RID: 21918 RVA: 0x0020DFF4 File Offset: 0x0020C1F4
	public void Clear()
	{
		this.centerBX = int.MaxValue;
		this.centerBZ = int.MaxValue;
		this.curChunkX = int.MaxValue;
		this.curChunkZ = int.MaxValue;
		Array.Clear(this.chunks, 0, this.chunks.Length);
	}

	// Token: 0x0600559F RID: 21919 RVA: 0x0020E041 File Offset: 0x0020C241
	public IChunk GetChunk(int x, int z)
	{
		return this.chunks[x + 1 + (z + 1) * 3];
	}

	// Token: 0x060055A0 RID: 21920 RVA: 0x0020E053 File Offset: 0x0020C253
	public IChunk GetNeighborChunk(int x, int z)
	{
		return this.nChunks[x, z];
	}

	// Token: 0x060055A1 RID: 21921 RVA: 0x0020E064 File Offset: 0x0020C264
	public bool IsBlockInCache(int relx, int absy, int relz)
	{
		if (absy < 0 || absy > 255)
		{
			return false;
		}
		int num = relx + 1 + (relz + 1) * 3;
		return num <= 8 && this.chunks[num] != null;
	}

	// Token: 0x060055A2 RID: 21922 RVA: 0x0020E09A File Offset: 0x0020C29A
	public Vector3i GetChunkPos(int relx, int absy, int relz)
	{
		return new Vector3i((this.centerBX + relx) & 15, absy, (this.centerBZ + relz) & 15);
	}

	// Token: 0x060055A3 RID: 21923 RVA: 0x0020E0B8 File Offset: 0x0020C2B8
	public BlockValue Get(int absy)
	{
		if (absy < 0)
		{
			return BlockValue.Air;
		}
		BlockValue blockValue2;
		try
		{
			BlockValue blockValue = this.chunks[4].GetBlock(this.centerBX & 15, absy, this.centerBZ & 15);
			if (!GameManager.bShowDecorBlocks && blockValue.Block.IsDecoration)
			{
				blockValue = BlockValue.Air;
			}
			if (!GameManager.bShowLootBlocks)
			{
				BlockCompositeTileEntity blockCompositeTileEntity = blockValue.Block as BlockCompositeTileEntity;
				if (blockCompositeTileEntity != null && blockCompositeTileEntity.CompositeData.HasFeature<TEFeatureStorage>())
				{
					blockValue = BlockValue.Air;
				}
			}
			blockValue2 = blockValue;
		}
		catch (Exception)
		{
			if (this.firstException)
			{
				Log.Error("ChunkCacheNeighborBlocks.Get: len=" + this.chunks.Length.ToString());
				this.firstException = false;
			}
			blockValue2 = BlockValue.Air;
		}
		return blockValue2;
	}

	// Token: 0x060055A4 RID: 21924 RVA: 0x0020E184 File Offset: 0x0020C384
	public BlockValue Get(int relx, int absy, int relz)
	{
		int num = relx + 1 + (relz + 1) * 3;
		if (absy < 0 || num >= 9)
		{
			return BlockValue.Air;
		}
		BlockValue blockValue2;
		try
		{
			BlockValue blockValue = this.chunks[num].GetBlock((this.centerBX + relx) & 15, absy, (this.centerBZ + relz) & 15);
			if (!GameManager.bShowDecorBlocks && blockValue.Block.IsDecoration)
			{
				blockValue = BlockValue.Air;
			}
			if (!GameManager.bShowLootBlocks)
			{
				BlockCompositeTileEntity blockCompositeTileEntity = blockValue.Block as BlockCompositeTileEntity;
				if (blockCompositeTileEntity != null && blockCompositeTileEntity.CompositeData.HasFeature<TEFeatureStorage>())
				{
					blockValue = BlockValue.Air;
				}
			}
			blockValue2 = blockValue;
		}
		catch (Exception)
		{
			if (this.firstException)
			{
				Log.Error(string.Concat(new string[]
				{
					"ChunkCacheNeighborBlocks.Get: relX=",
					relx.ToString(),
					" relz=",
					relz.ToString(),
					" len=",
					this.chunks.Length.ToString()
				}));
				this.firstException = false;
			}
			blockValue2 = BlockValue.Air;
		}
		return blockValue2;
	}

	// Token: 0x060055A5 RID: 21925 RVA: 0x0020E294 File Offset: 0x0020C494
	public byte GetStab(int relx, int absy, int relz)
	{
		byte b = 0;
		try
		{
			b = this.chunks[relx + 1 + (relz + 1) * 3].GetStability((this.centerBX + relx) & 15, absy, (this.centerBZ + relz) & 15);
		}
		catch (Exception ex)
		{
			Log.Out(string.Concat(new string[]
			{
				"Bad ChunkCacheNeighborBlocks index (",
				relx.ToString(),
				", ",
				absy.ToString(),
				", ",
				relz.ToString(),
				"), \nException: ",
				ex.ToString()
			}));
		}
		return b;
	}

	// Token: 0x060055A6 RID: 21926 RVA: 0x0020E340 File Offset: 0x0020C540
	public bool IsWater(int relx, int absy, int relz)
	{
		int num = relx + 1 + (relz + 1) * 3;
		if (absy < 0 || num >= 9)
		{
			return false;
		}
		bool flag = false;
		try
		{
			flag = this.chunks[num].IsWater((this.centerBX + relx) & 15, absy, (this.centerBZ + relz) & 15);
		}
		catch (Exception ex)
		{
			Log.Out(string.Concat(new string[]
			{
				"Bad ChunkCacheNeighborBlocks index (",
				relx.ToString(),
				", ",
				absy.ToString(),
				", ",
				relz.ToString(),
				"), \nException: ",
				ex.ToString()
			}));
		}
		return flag;
	}

	// Token: 0x060055A7 RID: 21927 RVA: 0x0020E3F8 File Offset: 0x0020C5F8
	public bool IsAir(int relx, int absy, int relz)
	{
		int num = relx + 1 + (relz + 1) * 3;
		if (absy < 0 || num >= 9)
		{
			return false;
		}
		int num2 = (this.centerBX + relx) & 15;
		int num3 = (this.centerBZ + relz) & 15;
		bool flag = false;
		try
		{
			IChunk chunk = this.chunks[num];
			return chunk.GetBlock(num2, absy, num3).isair && !chunk.IsWater(num2, absy, num3);
		}
		catch (Exception ex)
		{
			Log.Out(string.Concat(new string[]
			{
				"Bad ChunkCacheNeighborBlocks index (",
				relx.ToString(),
				", ",
				absy.ToString(),
				", ",
				relz.ToString(),
				"), \nException: ",
				ex.ToString()
			}));
		}
		return flag;
	}

	// Token: 0x060055A8 RID: 21928 RVA: 0x0020E4D8 File Offset: 0x0020C6D8
	public override string ToString()
	{
		return string.Format("BlockCache -- Chunk Pos ({0}, {1}) -- Block Pos ({2}, {3})", new object[] { this.curChunkX, this.curChunkZ, this.centerBX, this.centerBZ });
	}

	// Token: 0x04004288 RID: 17032
	[PublicizedFrom(EAccessModifier.Private)]
	public IChunk[] chunks;

	// Token: 0x04004289 RID: 17033
	[PublicizedFrom(EAccessModifier.Private)]
	public ChunkCacheNeighborChunks nChunks;

	// Token: 0x0400428A RID: 17034
	[PublicizedFrom(EAccessModifier.Private)]
	public int centerBX;

	// Token: 0x0400428B RID: 17035
	[PublicizedFrom(EAccessModifier.Private)]
	public int centerBZ;

	// Token: 0x0400428C RID: 17036
	[PublicizedFrom(EAccessModifier.Private)]
	public int curChunkX;

	// Token: 0x0400428D RID: 17037
	[PublicizedFrom(EAccessModifier.Private)]
	public int curChunkZ;

	// Token: 0x0400428E RID: 17038
	[PublicizedFrom(EAccessModifier.Private)]
	public bool firstException;
}
