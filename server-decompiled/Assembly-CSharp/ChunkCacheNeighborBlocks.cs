using System;

// Token: 0x02000B12 RID: 2834
public class ChunkCacheNeighborBlocks : INeighborBlockCache
{
	// Token: 0x06005583 RID: 21891 RVA: 0x0020AB2C File Offset: 0x00208D2C
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

	// Token: 0x06005584 RID: 21892 RVA: 0x0020AB88 File Offset: 0x00208D88
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

	// Token: 0x06005585 RID: 21893 RVA: 0x0020AD84 File Offset: 0x00208F84
	public void Clear()
	{
		this.centerBX = int.MaxValue;
		this.centerBZ = int.MaxValue;
		this.curChunkX = int.MaxValue;
		this.curChunkZ = int.MaxValue;
		Array.Clear(this.chunks, 0, this.chunks.Length);
	}

	// Token: 0x06005586 RID: 21894 RVA: 0x0020ADD1 File Offset: 0x00208FD1
	public IChunk GetChunk(int x, int z)
	{
		return this.chunks[x + 1 + (z + 1) * 3];
	}

	// Token: 0x06005587 RID: 21895 RVA: 0x0020ADE3 File Offset: 0x00208FE3
	public IChunk GetNeighborChunk(int x, int z)
	{
		return this.nChunks[x, z];
	}

	// Token: 0x06005588 RID: 21896 RVA: 0x0020ADF4 File Offset: 0x00208FF4
	public bool IsBlockInCache(int relx, int absy, int relz)
	{
		if (absy < 0 || absy > 255)
		{
			return false;
		}
		int num = relx + 1 + (relz + 1) * 3;
		return num <= 8 && this.chunks[num] != null;
	}

	// Token: 0x06005589 RID: 21897 RVA: 0x0020AE2A File Offset: 0x0020902A
	public Vector3i GetChunkPos(int relx, int absy, int relz)
	{
		return new Vector3i((this.centerBX + relx) & 15, absy, (this.centerBZ + relz) & 15);
	}

	// Token: 0x0600558A RID: 21898 RVA: 0x0020AE48 File Offset: 0x00209048
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

	// Token: 0x0600558B RID: 21899 RVA: 0x0020AF14 File Offset: 0x00209114
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

	// Token: 0x0600558C RID: 21900 RVA: 0x0020B024 File Offset: 0x00209224
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

	// Token: 0x0600558D RID: 21901 RVA: 0x0020B0D0 File Offset: 0x002092D0
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

	// Token: 0x0600558E RID: 21902 RVA: 0x0020B188 File Offset: 0x00209388
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

	// Token: 0x0600558F RID: 21903 RVA: 0x0020B268 File Offset: 0x00209468
	public override string ToString()
	{
		return string.Format("BlockCache -- Chunk Pos ({0}, {1}) -- Block Pos ({2}, {3})", new object[] { this.curChunkX, this.curChunkZ, this.centerBX, this.centerBZ });
	}

	// Token: 0x0400424B RID: 16971
	[PublicizedFrom(EAccessModifier.Private)]
	public IChunk[] chunks;

	// Token: 0x0400424C RID: 16972
	[PublicizedFrom(EAccessModifier.Private)]
	public ChunkCacheNeighborChunks nChunks;

	// Token: 0x0400424D RID: 16973
	[PublicizedFrom(EAccessModifier.Private)]
	public int centerBX;

	// Token: 0x0400424E RID: 16974
	[PublicizedFrom(EAccessModifier.Private)]
	public int centerBZ;

	// Token: 0x0400424F RID: 16975
	[PublicizedFrom(EAccessModifier.Private)]
	public int curChunkX;

	// Token: 0x04004250 RID: 16976
	[PublicizedFrom(EAccessModifier.Private)]
	public int curChunkZ;

	// Token: 0x04004251 RID: 16977
	[PublicizedFrom(EAccessModifier.Private)]
	public bool firstException;
}
