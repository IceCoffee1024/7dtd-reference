using System;

// Token: 0x02000D03 RID: 3331
public static class WaterUtils
{
	// Token: 0x0600654B RID: 25931 RVA: 0x0027D4F8 File Offset: 0x0027B6F8
	public static int GetVoxelKey2D(int _x, int _z)
	{
		return _x * 8976890 + _z * 981131;
	}

	// Token: 0x0600654C RID: 25932 RVA: 0x0027D509 File Offset: 0x0027B709
	public static int GetVoxelKey(int _x, int _y, int _z = 0)
	{
		return _x * 8976890 + _y * 981131 + _z;
	}

	// Token: 0x0600654D RID: 25933 RVA: 0x0027D51C File Offset: 0x0027B71C
	public static bool IsChunkSafeToUpdate(Chunk chunk)
	{
		return chunk != null && !chunk.NeedsDecoration && !chunk.NeedsCopying && !chunk.IsLocked;
	}

	// Token: 0x0600654E RID: 25934 RVA: 0x0027D540 File Offset: 0x0027B740
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

	// Token: 0x0600654F RID: 25935 RVA: 0x0027D59C File Offset: 0x0027B79C
	public static bool CanWaterFlowThrough(BlockValue _bv)
	{
		Block block = _bv.Block;
		return block != null && block.WaterFlowMask != BlockFaceFlag.All;
	}

	// Token: 0x06006550 RID: 25936 RVA: 0x0027D5C4 File Offset: 0x0027B7C4
	public static bool CanWaterFlowThrough(int _blockId)
	{
		Block block = Block.list[_blockId];
		return block != null && block.WaterFlowMask != BlockFaceFlag.All;
	}

	// Token: 0x06006551 RID: 25937 RVA: 0x0027D5EB File Offset: 0x0027B7EB
	public static int GetWaterLevel(WaterValue waterValue)
	{
		if (waterValue.GetMass() > 195)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x06006552 RID: 25938 RVA: 0x0027D5FE File Offset: 0x0027B7FE
	public static bool IsVoxelOutsideChunk(int _neighborX, int _neighborZ)
	{
		return _neighborX < 0 || _neighborX > 15 || _neighborZ < 0 || _neighborZ > 15;
	}
}
