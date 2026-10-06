using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;

// Token: 0x02000CDC RID: 3292
[BurstCompile(CompileSynchronously = true)]
public struct WaterSimulationApplyFlows : IJobParallelFor
{
	// Token: 0x0600649F RID: 25759 RVA: 0x00278D0C File Offset: 0x00276F0C
	public unsafe void Execute(int chunkIndex)
	{
		this.neighborCache = WaterNeighborCacheNative.InitializeCache(this.waterDataHandles);
		ChunkKey chunkKey = this.processingChunks[chunkIndex];
		this.stats = this.waterStats[chunkIndex];
		WaterDataHandle waterDataHandle;
		if (this.waterDataHandles.TryGetValue(chunkKey, out waterDataHandle))
		{
			waterDataHandle.ApplyEnqueuedFlows();
			if (waterDataHandle.flowVoxels.IsEmpty)
			{
				this.nonFlowingChunks.AddNoResize(chunkKey);
			}
			else
			{
				this.neighborCache.SetChunk(chunkKey);
				foreach (KeyValue<int, int> keyValue in waterDataHandle.flowVoxels)
				{
					int key = keyValue.Key;
					UnsafeParallelHashMap<int, int>.Enumerator enumerator;
					keyValue = enumerator.Current;
					int num = *keyValue.Value;
					int3 voxelCoords = WaterDataHandle.GetVoxelCoords(key);
					int num2 = waterDataHandle.voxelData.Get(key);
					if (waterDataHandle.IsInGroundWater(voxelCoords.x, voxelCoords.y, voxelCoords.z))
					{
						num2 = math.min(num, 19500);
					}
					else
					{
						if (num == 0)
						{
							continue;
						}
						num2 += num;
					}
					waterDataHandle.voxelData.Set(key, num2);
					waterDataHandle.SetVoxelActive(key);
					this.neighborCache.SetVoxel(voxelCoords.x, voxelCoords.y, voxelCoords.z);
					this.WakeNeighbor(WaterNeighborCacheNative.X_NEG);
					this.WakeNeighbor(WaterNeighborCacheNative.X_POS);
					this.WakeNeighbor(1);
					this.WakeNeighbor(-1);
					this.WakeNeighbor(WaterNeighborCacheNative.Z_NEG);
					this.WakeNeighbor(WaterNeighborCacheNative.Z_POS);
				}
				this.activeChunkSet.Add(chunkKey);
			}
		}
		this.waterStats[chunkIndex] = this.stats;
	}

	// Token: 0x060064A0 RID: 25760 RVA: 0x00278EB0 File Offset: 0x002770B0
	[PublicizedFrom(EAccessModifier.Private)]
	public void WakeNeighbor(int _yOffset)
	{
		int num = this.neighborCache.voxelY + _yOffset;
		if (num < 0 || num > 255)
		{
			return;
		}
		this.neighborCache.center.SetVoxelActive(this.neighborCache.voxelX, num, this.neighborCache.voxelZ);
		this.stats.NumVoxelsWokeUp = this.stats.NumVoxelsWokeUp + 1;
	}

	// Token: 0x060064A1 RID: 25761 RVA: 0x00278F10 File Offset: 0x00277110
	[PublicizedFrom(EAccessModifier.Private)]
	public void WakeNeighbor(int2 _xzOffset)
	{
		ChunkKey chunkKey;
		WaterDataHandle waterDataHandle;
		int num;
		int num2;
		int num3;
		if (this.neighborCache.TryGetNeighbor(_xzOffset, out chunkKey, out waterDataHandle, out num, out num2, out num3))
		{
			if (chunkKey.Equals(this.neighborCache.chunkKey))
			{
				waterDataHandle.SetVoxelActive(num, num2, num3);
			}
			else
			{
				waterDataHandle.EnqueueVoxelActive(num, num2, num3);
				this.activeChunkSet.Add(chunkKey);
			}
			this.stats.NumVoxelsWokeUp = this.stats.NumVoxelsWokeUp + 1;
		}
	}

	// Token: 0x04004E7B RID: 20091
	public NativeArray<ChunkKey> processingChunks;

	// Token: 0x04004E7C RID: 20092
	public NativeList<ChunkKey>.ParallelWriter nonFlowingChunks;

	// Token: 0x04004E7D RID: 20093
	public UnsafeParallelHashSet<ChunkKey>.ParallelWriter activeChunkSet;

	// Token: 0x04004E7E RID: 20094
	public UnsafeParallelHashMap<ChunkKey, WaterDataHandle> waterDataHandles;

	// Token: 0x04004E7F RID: 20095
	public NativeArray<WaterStats> waterStats;

	// Token: 0x04004E80 RID: 20096
	[PublicizedFrom(EAccessModifier.Private)]
	public WaterNeighborCacheNative neighborCache;

	// Token: 0x04004E81 RID: 20097
	[PublicizedFrom(EAccessModifier.Private)]
	public WaterStats stats;
}
