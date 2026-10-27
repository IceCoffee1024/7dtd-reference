using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

// Token: 0x02000CEC RID: 3308
[BurstCompile(CompileSynchronously = true)]
public struct WaterSimulationPostProcess : IJob
{
	// Token: 0x060064E7 RID: 25831 RVA: 0x0027B7F0 File Offset: 0x002799F0
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

	// Token: 0x04004EBD RID: 20157
	public NativeArray<ChunkKey> processingChunks;

	// Token: 0x04004EBE RID: 20158
	public NativeList<ChunkKey> nonFlowingChunks;

	// Token: 0x04004EBF RID: 20159
	public UnsafeParallelHashSet<ChunkKey> activeChunks;

	// Token: 0x04004EC0 RID: 20160
	public UnsafeParallelHashMap<ChunkKey, WaterDataHandle> waterDataHandles;
}
