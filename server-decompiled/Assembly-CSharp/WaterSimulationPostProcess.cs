using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

// Token: 0x02000CD3 RID: 3283
[BurstCompile(CompileSynchronously = true)]
public struct WaterSimulationPostProcess : IJob
{
	// Token: 0x060064CF RID: 25807 RVA: 0x00276EC4 File Offset: 0x002750C4
	public void Execute()
	{
		for (int i = 0; i < this.nonFlowingChunks.Length; i++)
		{
			ChunkKey chunkKey = this.nonFlowingChunks[i];
			WaterDataHandle waterDataHandle;
			if (this.waterDataHandles.TryGetValue(chunkKey, out waterDataHandle))
			{
				this.activeChunks.Remove(this.nonFlowingChunks[i]);
			}
		}
		for (int j = 0; j < this.processingChunks.Length; j++)
		{
			ChunkKey chunkKey2 = this.processingChunks[j];
			WaterDataHandle waterDataHandle2;
			if (this.waterDataHandles.TryGetValue(chunkKey2, out waterDataHandle2) && waterDataHandle2.activationsFromOtherChunks.Count > 0)
			{
				waterDataHandle2.ApplyEnqueuedActivations();
				this.activeChunks.Add(chunkKey2);
			}
		}
	}

	// Token: 0x04004E62 RID: 20066
	public NativeArray<ChunkKey> processingChunks;

	// Token: 0x04004E63 RID: 20067
	public NativeList<ChunkKey> nonFlowingChunks;

	// Token: 0x04004E64 RID: 20068
	public UnsafeParallelHashSet<ChunkKey> activeChunks;

	// Token: 0x04004E65 RID: 20069
	public UnsafeParallelHashMap<ChunkKey, WaterDataHandle> waterDataHandles;
}
