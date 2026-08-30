using System;

// Token: 0x02000CD8 RID: 3288
public static class WaterUtils
{
	// Token: 0x060064A8 RID: 25768 RVA: 0x00275D30 File Offset: 0x00273F30
	public static int GetVoxelKey2D(int _x, int _z)
	{
		return _x * 8976890 + _z * 981131;
	}

	// Token: 0x060064A9 RID: 25769 RVA: 0x00275D41 File Offset: 0x00273F41
	public static int GetVoxelKey(int _x, int _y, int _z = 0)
	{
		return _x * 8976890 + _y * 981131 + _z;
	}

	// Token: 0x060064AA RID: 25770 RVA: 0x00275D54 File Offset: 0x00273F54
	public static bool IsChunkSafeToUpdate(Chunk chunk)
	{
		return chunk != null && !chunk.NeedsDecoration && !chunk.NeedsCopying && !chunk.IsLocked;
	}

	// Token: 0x060064AB RID: 25771 RVA: 0x00275D78 File Offset: 0x00273F78
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

	// Token: 0x060064AC RID: 25772 RVA: 0x00275DD4 File Offset: 0x00273FD4
	public static bool CanWaterFlowThrough(BlockValue _bv)
	{
		Block block = _bv.Block;
		return block != null && block.WaterFlowMask != BlockFaceFlag.All;
	}

	// Token: 0x060064AD RID: 25773 RVA: 0x00275DFC File Offset: 0x00273FFC
	public static bool CanWaterFlowThrough(int _blockId)
	{
		Block block = Block.list[_blockId];
		return block != null && block.WaterFlowMask != BlockFaceFlag.All;
	}

	// Token: 0x060064AE RID: 25774 RVA: 0x00275E23 File Offset: 0x00274023
	public static int GetWaterLevel(WaterValue waterValue)
	{
		if (waterValue.GetMass() > 195)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x060064AF RID: 25775 RVA: 0x00275E36 File Offset: 0x00274036
	public static bool IsVoxelOutsideChunk(int _neighborX, int _neighborZ)
	{
		return _neighborX < 0 || _neighborX > 15 || _neighborZ < 0 || _neighborZ > 15;
	}
}
