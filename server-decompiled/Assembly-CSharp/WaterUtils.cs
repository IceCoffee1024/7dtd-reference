using System;

// Token: 0x02000CF8 RID: 3320
public static class WaterUtils
{
	// Token: 0x06006526 RID: 25894 RVA: 0x0027BFD0 File Offset: 0x0027A1D0
	public static int GetVoxelKey2D(int _x, int _z)
	{
		return _x * 8976890 + _z * 981131;
	}

	// Token: 0x06006527 RID: 25895 RVA: 0x0027BFE1 File Offset: 0x0027A1E1
	public static int GetVoxelKey(int _x, int _y, int _z = 0)
	{
		return _x * 8976890 + _y * 981131 + _z;
	}

	// Token: 0x06006528 RID: 25896 RVA: 0x0027BFF4 File Offset: 0x0027A1F4
	public static bool IsChunkSafeToUpdate(Chunk chunk)
	{
		return chunk != null && !chunk.NeedsDecoration && !chunk.NeedsCopying && !chunk.IsLocked;
	}

	// Token: 0x06006529 RID: 25897 RVA: 0x0027C018 File Offset: 0x0027A218
	public static bool TryOpenChunkForUpdate(ChunkCluster _chunks, long _key, out Chunk _chunk)
	{
		bool flag;
		using (ScopedChunkWriteAccess chunkWriteAccess = ScopedChunkAccess.GetChunkWriteAccess(_chunks, _key))
		{
			Chunk chunk = chunkWriteAccess.Chunk;
			if (!WaterUtils.IsChunkSafeToUpdate(chunk))
			{
				_chunk = null;
				flag = false;
			}
			else
			{
				_chunk = chunk;
				_chunk.InProgressWaterSim = true;
				flag = true;
			}
		}
		return flag;
	}

	// Token: 0x0600652A RID: 25898 RVA: 0x0027C074 File Offset: 0x0027A274
	public static bool CanWaterFlowThrough(BlockValue _bv)
	{
		Block block = _bv.Block;
		return block != null && block.WaterFlowMask != BlockFaceFlag.All;
	}

	// Token: 0x0600652B RID: 25899 RVA: 0x0027C09C File Offset: 0x0027A29C
	public static bool CanWaterFlowThrough(int _blockId)
	{
		Block block = Block.list[_blockId];
		return block != null && block.WaterFlowMask != BlockFaceFlag.All;
	}

	// Token: 0x0600652C RID: 25900 RVA: 0x0027C0C3 File Offset: 0x0027A2C3
	public static int GetWaterLevel(WaterValue waterValue)
	{
		if (waterValue.GetMass() > 195)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x0600652D RID: 25901 RVA: 0x0027C0D6 File Offset: 0x0027A2D6
	public static bool IsVoxelOutsideChunk(int _neighborX, int _neighborZ)
	{
		return _neighborX < 0 || _neighborX > 15 || _neighborZ < 0 || _neighborZ > 15;
	}
}
