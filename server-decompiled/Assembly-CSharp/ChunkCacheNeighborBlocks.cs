using System;

// Token: 0x02000B00 RID: 2816
public class ChunkCacheNeighborBlocks : INeighborBlockCache
{
	// Token: 0x060054FB RID: 21755 RVA: 0x00207E10 File Offset: 0x00206010
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

	// Token: 0x060054FC RID: 21756 RVA: 0x00207E6C File Offset: 0x0020606C
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

	// Token: 0x060054FD RID: 21757 RVA: 0x00208068 File Offset: 0x00206268
	public void Clear()
	{
		this.centerBX = int.MaxValue;
		this.centerBZ = int.MaxValue;
		this.curChunkX = int.MaxValue;
		this.curChunkZ = int.MaxValue;
		Array.Clear(this.chunks, 0, this.chunks.Length);
	}

	// Token: 0x060054FE RID: 21758 RVA: 0x002080B5 File Offset: 0x002062B5
	public IChunk GetChunk(int x, int z)
	{
		return this.chunks[x + 1 + (z + 1) * 3];
	}

	// Token: 0x060054FF RID: 21759 RVA: 0x002080C7 File Offset: 0x002062C7
	public IChunk GetNeighborChunk(int x, int z)
	{
		return this.nChunks[x, z];
	}

	// Token: 0x06005500 RID: 21760 RVA: 0x002080D8 File Offset: 0x002062D8
	public bool IsBlockInCache(int relx, int absy, int relz)
	{
		if (absy < 0 || absy > 255)
		{
			return false;
		}
		int num = relx + 1 + (relz + 1) * 3;
		return num <= 8 && this.chunks[num] != null;
	}

	// Token: 0x06005501 RID: 21761 RVA: 0x0020810E File Offset: 0x0020630E
	public Vector3i GetChunkPos(int relx, int absy, int relz)
	{
		return new Vector3i((this.centerBX + relx) & 15, absy, (this.centerBZ + relz) & 15);
	}

	// Token: 0x06005502 RID: 21762 RVA: 0x0020812C File Offset: 0x0020632C
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
				if (blockCompositeTileEntity != null && blockCompositeTileEntity.CompositeData.HasFeature<ITileEntityLootable>())
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

	// Token: 0x06005503 RID: 21763 RVA: 0x002081F8 File Offset: 0x002063F8
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
				if (blockCompositeTileEntity != null && blockCompositeTileEntity.CompositeData.HasFeature<ITileEntityLootable>())
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

	// Token: 0x06005504 RID: 21764 RVA: 0x00208308 File Offset: 0x00206508
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

	// Token: 0x06005505 RID: 21765 RVA: 0x002083B4 File Offset: 0x002065B4
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

	// Token: 0x06005506 RID: 21766 RVA: 0x0020846C File Offset: 0x0020666C
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

	// Token: 0x06005507 RID: 21767 RVA: 0x0020854C File Offset: 0x0020674C
	public override string ToString()
	{
		return string.Format("BlockCache -- Chunk Pos ({0}, {1}) -- Block Pos ({2}, {3})", new object[] { this.curChunkX, this.curChunkZ, this.centerBX, this.centerBZ });
	}

	// Token: 0x040041B0 RID: 16816
	[PublicizedFrom(EAccessModifier.Private)]
	public IChunk[] chunks;

	// Token: 0x040041B1 RID: 16817
	[PublicizedFrom(EAccessModifier.Private)]
	public ChunkCacheNeighborChunks nChunks;

	// Token: 0x040041B2 RID: 16818
	[PublicizedFrom(EAccessModifier.Private)]
	public int centerBX;

	// Token: 0x040041B3 RID: 16819
	[PublicizedFrom(EAccessModifier.Private)]
	public int centerBZ;

	// Token: 0x040041B4 RID: 16820
	[PublicizedFrom(EAccessModifier.Private)]
	public int curChunkX;

	// Token: 0x040041B5 RID: 16821
	[PublicizedFrom(EAccessModifier.Private)]
	public int curChunkZ;

	// Token: 0x040041B6 RID: 16822
	[PublicizedFrom(EAccessModifier.Private)]
	public bool firstException;
}
