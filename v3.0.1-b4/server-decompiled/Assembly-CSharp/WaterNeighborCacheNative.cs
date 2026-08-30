using System;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

// Token: 0x02000CBB RID: 3259
public struct WaterNeighborCacheNative
{
	// Token: 0x0600641B RID: 25627 RVA: 0x002728B8 File Offset: 0x00270AB8
	public static WaterNeighborCacheNative InitializeCache(UnsafeParallelHashMap<ChunkKey, WaterDataHandle> _handles)
	{
		return new WaterNeighborCacheNative
		{
			waterDataHandles = _handles
		};
	}

	// Token: 0x0600641C RID: 25628 RVA: 0x002728D6 File Offset: 0x00270AD6
	public void SetChunk(ChunkKey _chunk)
	{
		this.chunkKey = _chunk;
		this.center = this.waterDataHandles[_chunk];
	}

	// Token: 0x0600641D RID: 25629 RVA: 0x002728F1 File Offset: 0x00270AF1
	public void SetVoxel(int _x, int _y, int _z)
	{
		this.voxelX = _x;
		this.voxelY = _y;
		this.voxelZ = _z;
	}

	// Token: 0x0600641E RID: 25630 RVA: 0x00272908 File Offset: 0x00270B08
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

	// Token: 0x04004D9D RID: 19869
	public static readonly int2 X_NEG = new int2(-1, 0);

	// Token: 0x04004D9E RID: 19870
	public static readonly int2 X_POS = new int2(1, 0);

	// Token: 0x04004D9F RID: 19871
	public static readonly int2 Z_NEG = new int2(0, -1);

	// Token: 0x04004DA0 RID: 19872
	public static readonly int2 Z_POS = new int2(0, 1);

	// Token: 0x04004DA1 RID: 19873
	[PublicizedFrom(EAccessModifier.Private)]
	public UnsafeParallelHashMap<ChunkKey, WaterDataHandle> waterDataHandles;

	// Token: 0x04004DA2 RID: 19874
	public ChunkKey chunkKey;

	// Token: 0x04004DA3 RID: 19875
	public int voxelX;

	// Token: 0x04004DA4 RID: 19876
	public int voxelY;

	// Token: 0x04004DA5 RID: 19877
	public int voxelZ;

	// Token: 0x04004DA6 RID: 19878
	public WaterDataHandle center;
}
