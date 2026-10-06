using System;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

// Token: 0x02000CDB RID: 3291
public struct WaterNeighborCacheNative
{
	// Token: 0x0600649A RID: 25754 RVA: 0x00278BB8 File Offset: 0x00276DB8
	public static WaterNeighborCacheNative InitializeCache(UnsafeParallelHashMap<ChunkKey, WaterDataHandle> _handles)
	{
		return new WaterNeighborCacheNative
		{
			waterDataHandles = _handles
		};
	}

	// Token: 0x0600649B RID: 25755 RVA: 0x00278BD6 File Offset: 0x00276DD6
	public void SetChunk(ChunkKey _chunk)
	{
		this.chunkKey = _chunk;
		this.center = this.waterDataHandles[_chunk];
	}

	// Token: 0x0600649C RID: 25756 RVA: 0x00278BF1 File Offset: 0x00276DF1
	public void SetVoxel(int _x, int _y, int _z)
	{
		this.voxelX = _x;
		this.voxelY = _y;
		this.voxelZ = _z;
	}

	// Token: 0x0600649D RID: 25757 RVA: 0x00278C08 File Offset: 0x00276E08
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

	// Token: 0x04004E71 RID: 20081
	public static readonly int2 X_NEG = new int2(-1, 0);

	// Token: 0x04004E72 RID: 20082
	public static readonly int2 X_POS = new int2(1, 0);

	// Token: 0x04004E73 RID: 20083
	public static readonly int2 Z_NEG = new int2(0, -1);

	// Token: 0x04004E74 RID: 20084
	public static readonly int2 Z_POS = new int2(0, 1);

	// Token: 0x04004E75 RID: 20085
	[PublicizedFrom(EAccessModifier.Private)]
	public UnsafeParallelHashMap<ChunkKey, WaterDataHandle> waterDataHandles;

	// Token: 0x04004E76 RID: 20086
	public ChunkKey chunkKey;

	// Token: 0x04004E77 RID: 20087
	public int voxelX;

	// Token: 0x04004E78 RID: 20088
	public int voxelY;

	// Token: 0x04004E79 RID: 20089
	public int voxelZ;

	// Token: 0x04004E7A RID: 20090
	public WaterDataHandle center;
}
