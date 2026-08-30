using System;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

// Token: 0x02000CCD RID: 3277
public struct WaterNeighborCacheNative
{
	// Token: 0x060064A7 RID: 25767 RVA: 0x002757B4 File Offset: 0x002739B4
	public static WaterNeighborCacheNative InitializeCache(UnsafeParallelHashMap<ChunkKey, WaterDataHandle> _handles)
	{
		return new WaterNeighborCacheNative
		{
			waterDataHandles = _handles
		};
	}

	// Token: 0x060064A8 RID: 25768 RVA: 0x002757D2 File Offset: 0x002739D2
	public void SetChunk(ChunkKey _chunk)
	{
		this.chunkKey = _chunk;
		this.center = this.waterDataHandles[_chunk];
	}

	// Token: 0x060064A9 RID: 25769 RVA: 0x002757ED File Offset: 0x002739ED
	public void SetVoxel(int _x, int _y, int _z)
	{
		this.voxelX = _x;
		this.voxelY = _y;
		this.voxelZ = _z;
	}

	// Token: 0x060064AA RID: 25770 RVA: 0x00275804 File Offset: 0x00273A04
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

	// Token: 0x04004E3B RID: 20027
	public static readonly int2 X_NEG = new int2(-1, 0);

	// Token: 0x04004E3C RID: 20028
	public static readonly int2 X_POS = new int2(1, 0);

	// Token: 0x04004E3D RID: 20029
	public static readonly int2 Z_NEG = new int2(0, -1);

	// Token: 0x04004E3E RID: 20030
	public static readonly int2 Z_POS = new int2(0, 1);

	// Token: 0x04004E3F RID: 20031
	[PublicizedFrom(EAccessModifier.Private)]
	public UnsafeParallelHashMap<ChunkKey, WaterDataHandle> waterDataHandles;

	// Token: 0x04004E40 RID: 20032
	public ChunkKey chunkKey;

	// Token: 0x04004E41 RID: 20033
	public int voxelX;

	// Token: 0x04004E42 RID: 20034
	public int voxelY;

	// Token: 0x04004E43 RID: 20035
	public int voxelZ;

	// Token: 0x04004E44 RID: 20036
	public WaterDataHandle center;
}
