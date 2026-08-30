using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

// Token: 0x02000CC2 RID: 3266
[BurstCompile(CompileSynchronously = true)]
public struct WaterSimulationPreProcess : IJob
{
	// Token: 0x06006444 RID: 25668 RVA: 0x0027407C File Offset: 0x0027227C
	public void Execute()
	{
		this.neighborCache = WaterNeighborCacheNative.InitializeCache(this.waterDataHandles);
		using (UnsafeParallelHashSet<ChunkKey>.Enumerator enumerator = this.modifiedChunks.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ChunkKey chunkKey = enumerator.Current;
				WaterDataHandle waterDataHandle;
				if (this.waterDataHandles.TryGetValue(chunkKey, out waterDataHandle))
				{
					this.neighborCache.SetChunk(chunkKey);
					int num = 0;
					using (UnsafeParallelHashSet<int>.Enumerator enumerator2 = waterDataHandle.voxelsToWakeup.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if (++num > 65536)
							{
								Debug.LogError(string.Format("[WaterSimulationPreProcess] Number of wakeups for chunk ({0}, {1}) has exceeded the volume of a chunk {2}.", chunkKey.x, chunkKey.z, 65536));
								break;
							}
							int3 voxelCoords = WaterDataHandle.GetVoxelCoords(enumerator2.Current);
							waterDataHandle.SetVoxelActive(voxelCoords.x, voxelCoords.y, voxelCoords.z);
							this.neighborCache.SetVoxel(voxelCoords.x, voxelCoords.y, voxelCoords.z);
							this.WakeNeighbor(WaterNeighborCacheNative.X_NEG);
							this.WakeNeighbor(WaterNeighborCacheNative.X_POS);
							this.WakeNeighbor(1);
							this.WakeNeighbor(-1);
							this.WakeNeighbor(WaterNeighborCacheNative.Z_NEG);
							this.WakeNeighbor(WaterNeighborCacheNative.Z_POS);
						}
						waterDataHandle.voxelsToWakeup.Clear();
						this.activeChunks.Add(chunkKey);
					}
				}
			}
			this.modifiedChunks.Clear();
			using (NativeArray<ChunkKey> nativeArray = this.activeChunks.ToNativeArray(Allocator.Temp))
			{
				for (int i = 0; i < nativeArray.Length; i++)
				{
					ChunkKey chunkKey2 = nativeArray[i];
					this.TryTrackChunk(chunkKey2.x + 1, chunkKey2.z);
					this.TryTrackChunk(chunkKey2.x - 1, chunkKey2.z);
					this.TryTrackChunk(chunkKey2.x, chunkKey2.z + 1);
					this.TryTrackChunk(chunkKey2.x, chunkKey2.z - 1);
				}
			}
		}
	}

	// Token: 0x06006445 RID: 25669 RVA: 0x002742F0 File Offset: 0x002724F0
	[PublicizedFrom(EAccessModifier.Private)]
	public void WakeNeighbor(int _yOffset)
	{
		int num = this.neighborCache.voxelY + _yOffset;
		if (num < 0 || num > 255)
		{
			return;
		}
		this.neighborCache.center.SetVoxelActive(this.neighborCache.voxelX, num, this.neighborCache.voxelZ);
	}

	// Token: 0x06006446 RID: 25670 RVA: 0x00274340 File Offset: 0x00272540
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
			waterDataHandle.SetVoxelActive(num, num2, num3);
		}
	}

	// Token: 0x06006447 RID: 25671 RVA: 0x00274370 File Offset: 0x00272570
	[PublicizedFrom(EAccessModifier.Private)]
	public void TryTrackChunk(int _chunkX, int _chunkZ)
	{
		ChunkKey chunkKey = new ChunkKey(_chunkX, _chunkZ);
		if (this.waterDataHandles.ContainsKey(chunkKey))
		{
			this.activeChunks.Add(chunkKey);
		}
	}

	// Token: 0x04004DC8 RID: 19912
	public UnsafeParallelHashSet<ChunkKey> activeChunks;

	// Token: 0x04004DC9 RID: 19913
	public UnsafeParallelHashMap<ChunkKey, WaterDataHandle> waterDataHandles;

	// Token: 0x04004DCA RID: 19914
	public UnsafeParallelHashSet<ChunkKey> modifiedChunks;

	// Token: 0x04004DCB RID: 19915
	[PublicizedFrom(EAccessModifier.Private)]
	public WaterNeighborCacheNative neighborCache;
}
