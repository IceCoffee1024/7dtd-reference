using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

// Token: 0x02000CC1 RID: 3265
[BurstCompile(CompileSynchronously = true)]
public struct WaterSimulationPostProcess : IJob
{
	// Token: 0x06006443 RID: 25667 RVA: 0x00273FC8 File Offset: 0x002721C8
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

	// Token: 0x04004DC4 RID: 19908
	public NativeArray<ChunkKey> processingChunks;

	// Token: 0x04004DC5 RID: 19909
	public NativeList<ChunkKey> nonFlowingChunks;

	// Token: 0x04004DC6 RID: 19910
	public UnsafeParallelHashSet<ChunkKey> activeChunks;

	// Token: 0x04004DC7 RID: 19911
	public UnsafeParallelHashMap<ChunkKey, WaterDataHandle> waterDataHandles;
}
