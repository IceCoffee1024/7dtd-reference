using System;

// Token: 0x02000B2F RID: 2863
public class ChunkCacheNeighborBlocks : INeighborBlockCache
{
	// Token: 0x060055C1 RID: 21953 RVA: 0x0020F318 File Offset: 0x0020D518
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

	// Token: 0x060055C2 RID: 21954 RVA: 0x0020F374 File Offset: 0x0020D574
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

	// Token: 0x060055C3 RID: 21955 RVA: 0x0020F570 File Offset: 0x0020D770
	public void Clear()
	{
		this.centerBX = int.MaxValue;
		this.centerBZ = int.MaxValue;
		this.curChunkX = int.MaxValue;
		this.curChunkZ = int.MaxValue;
		Array.Clear(this.chunks, 0, this.chunks.Length);
	}

	// Token: 0x060055C4 RID: 21956 RVA: 0x0020F5BD File Offset: 0x0020D7BD
	public IChunk GetChunk(int x, int z)
	{
		return this.chunks[x + 1 + (z + 1) * 3];
	}

	// Token: 0x060055C5 RID: 21957 RVA: 0x0020F5CF File Offset: 0x0020D7CF
	public IChunk GetNeighborChunk(int x, int z)
	{
		return this.nChunks[x, z];
	}

	// Token: 0x060055C6 RID: 21958 RVA: 0x0020F5E0 File Offset: 0x0020D7E0
	public bool IsBlockInCache(int relx, int absy, int relz)
	{
		if (absy < 0 || absy > 255)
		{
			return false;
		}
		int num = relx + 1 + (relz + 1) * 3;
		return num <= 8 && this.chunks[num] != null;
	}

	// Token: 0x060055C7 RID: 21959 RVA: 0x0020F616 File Offset: 0x0020D816
	public Vector3i GetChunkPos(int relx, int absy, int relz)
	{
		return new Vector3i((this.centerBX + relx) & 15, absy, (this.centerBZ + relz) & 15);
	}

	// Token: 0x060055C8 RID: 21960 RVA: 0x0020F634 File Offset: 0x0020D834
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

	// Token: 0x060055C9 RID: 21961 RVA: 0x0020F700 File Offset: 0x0020D900
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

	// Token: 0x060055CA RID: 21962 RVA: 0x0020F810 File Offset: 0x0020DA10
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

	// Token: 0x060055CB RID: 21963 RVA: 0x0020F8BC File Offset: 0x0020DABC
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

	// Token: 0x060055CC RID: 21964 RVA: 0x0020F974 File Offset: 0x0020DB74
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

	// Token: 0x060055CD RID: 21965 RVA: 0x0020FA54 File Offset: 0x0020DC54
	public override string ToString()
	{
		return string.Format("BlockCache -- Chunk Pos ({0}, {1}) -- Block Pos ({2}, {3})", new object[] { this.curChunkX, this.curChunkZ, this.centerBX, this.centerBZ });
	}

	// Token: 0x040042AD RID: 17069
	[PublicizedFrom(EAccessModifier.Private)]
	public IChunk[] chunks;

	// Token: 0x040042AE RID: 17070
	[PublicizedFrom(EAccessModifier.Private)]
	public ChunkCacheNeighborChunks nChunks;

	// Token: 0x040042AF RID: 17071
	[PublicizedFrom(EAccessModifier.Private)]
	public int centerBX;

	// Token: 0x040042B0 RID: 17072
	[PublicizedFrom(EAccessModifier.Private)]
	public int centerBZ;

	// Token: 0x040042B1 RID: 17073
	[PublicizedFrom(EAccessModifier.Private)]
	public int curChunkX;

	// Token: 0x040042B2 RID: 17074
	[PublicizedFrom(EAccessModifier.Private)]
	public int curChunkZ;

	// Token: 0x040042B3 RID: 17075
	[PublicizedFrom(EAccessModifier.Private)]
	public bool firstException;
}
