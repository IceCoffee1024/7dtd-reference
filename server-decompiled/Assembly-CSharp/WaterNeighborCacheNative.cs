using System;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

// Token: 0x02000CE6 RID: 3302
public struct WaterNeighborCacheNative
{
	// Token: 0x060064BF RID: 25791 RVA: 0x0027A0E0 File Offset: 0x002782E0
	public static WaterNeighborCacheNative InitializeCache(UnsafeParallelHashMap<ChunkKey, WaterDataHandle> _handles)
	{
		return new WaterNeighborCacheNative
		{
			waterDataHandles = _handles
		};
	}

	// Token: 0x060064C0 RID: 25792 RVA: 0x0027A0FE File Offset: 0x002782FE
	public void SetChunk(ChunkKey _chunk)
	{
		this.chunkKey = _chunk;
		this.center = this.waterDataHandles[_chunk];
	}

	// Token: 0x060064C1 RID: 25793 RVA: 0x0027A119 File Offset: 0x00278319
	public void SetVoxel(int _x, int _y, int _z)
	{
		this.voxelX = _x;
		this.voxelY = _y;
		this.voxelZ = _z;
	}

	// Token: 0x060064C2 RID: 25794 RVA: 0x0027A130 File Offset: 0x00278330
	public bool TryGetNeighbor(int2 _xzOffset, out ChunkKey _chunkKey, out WaterDataHandle _dataHandle, out int _x, out int _y, out int _z)
	{
		_x = this.voxelX + _xzOffset.x;
		_y = this.voxelY;
		_z = this.voxelZ + _xzOffset.y;
		if (!WaterUtils.IsVoxelOutsideChunk(_x, _z))
		{
			_chunkKey = this.chunkKey;
			_dataHandle = this.center;
			return true;
		}
		int num = _x & 15;
		int num2 = _z & 15;
		int num3 = this.chunkKey.x + (_x - num) / 16;
		int num4 = this.chunkKey.z + (_z - num2) / 16;
		_chunkKey = new ChunkKey(num3, num4);
		if (this.waterDataHandles.TryGetValue(_chunkKey, out _dataHandle))
		{
			_x = num;
			_z = num2;
			return true;
		}
		_chunkKey = default(ChunkKey);
		_dataHandle = default(WaterDataHandle);
		return false;
	}

	// Token: 0x04004E96 RID: 20118
	public static readonly int2 X_NEG = new int2(-1, 0);

	// Token: 0x04004E97 RID: 20119
	public static readonly int2 X_POS = new int2(1, 0);

	// Token: 0x04004E98 RID: 20120
	public static readonly int2 Z_NEG = new int2(0, -1);

	// Token: 0x04004E99 RID: 20121
	public static readonly int2 Z_POS = new int2(0, 1);

	// Token: 0x04004E9A RID: 20122
	[PublicizedFrom(EAccessModifier.Private)]
	public UnsafeParallelHashMap<ChunkKey, WaterDataHandle> waterDataHandles;

	// Token: 0x04004E9B RID: 20123
	public ChunkKey chunkKey;

	// Token: 0x04004E9C RID: 20124
	public int voxelX;

	// Token: 0x04004E9D RID: 20125
	public int voxelY;

	// Token: 0x04004E9E RID: 20126
	public int voxelZ;

	// Token: 0x04004E9F RID: 20127
	public WaterDataHandle center;
}
