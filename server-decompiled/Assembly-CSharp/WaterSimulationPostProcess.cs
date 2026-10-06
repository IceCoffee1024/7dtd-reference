using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

// Token: 0x02000CE1 RID: 3297
[BurstCompile(CompileSynchronously = true)]
public struct WaterSimulationPostProcess : IJob
{
	// Token: 0x060064C2 RID: 25794 RVA: 0x0027A2C8 File Offset: 0x002784C8
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

	// Token: 0x04004E98 RID: 20120
	public NativeArray<ChunkKey> processingChunks;

	// Token: 0x04004E99 RID: 20121
	public NativeList<ChunkKey> nonFlowingChunks;

	// Token: 0x04004E9A RID: 20122
	public UnsafeParallelHashSet<ChunkKey> activeChunks;

	// Token: 0x04004E9B RID: 20123
	public UnsafeParallelHashMap<ChunkKey, WaterDataHandle> waterDataHandles;
}
