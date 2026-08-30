using System;

// Token: 0x02000CEA RID: 3306
public static class WaterUtils
{
	// Token: 0x06006534 RID: 25908 RVA: 0x00278C2C File Offset: 0x00276E2C
	public static int GetVoxelKey2D(int _x, int _z)
	{
		return _x * 8976890 + _z * 981131;
	}

	// Token: 0x06006535 RID: 25909 RVA: 0x00278C3D File Offset: 0x00276E3D
	public static int GetVoxelKey(int _x, int _y, int _z = 0)
	{
		return _x * 8976890 + _y * 981131 + _z;
	}

	// Token: 0x06006536 RID: 25910 RVA: 0x00278C50 File Offset: 0x00276E50
	public static bool IsChunkSafeToUpdate(Chunk chunk)
	{
		return chunk != null && !chunk.NeedsDecoration && !chunk.NeedsCopying && !chunk.IsLocked;
	}

	// Token: 0x06006537 RID: 25911 RVA: 0x00278C74 File Offset: 0x00276E74
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

	// Token: 0x06006538 RID: 25912 RVA: 0x00278CD0 File Offset: 0x00276ED0
	public static bool CanWaterFlowThrough(BlockValue _bv)
	{
		Block block = _bv.Block;
		return block != null && block.WaterFlowMask != BlockFaceFlag.All;
	}

	// Token: 0x06006539 RID: 25913 RVA: 0x00278CF8 File Offset: 0x00276EF8
	public static bool CanWaterFlowThrough(int _blockId)
	{
		Block block = Block.list[_blockId];
		return block != null && block.WaterFlowMask != BlockFaceFlag.All;
	}

	// Token: 0x0600653A RID: 25914 RVA: 0x00278D1F File Offset: 0x00276F1F
	public static int GetWaterLevel(WaterValue waterValue)
	{
		if (waterValue.GetMass() > 195)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x0600653B RID: 25915 RVA: 0x00278D32 File Offset: 0x00276F32
	public static bool IsVoxelOutsideChunk(int _neighborX, int _neighborZ)
	{
		return _neighborX < 0 || _neighborX > 15 || _neighborZ < 0 || _neighborZ > 15;
	}
}
