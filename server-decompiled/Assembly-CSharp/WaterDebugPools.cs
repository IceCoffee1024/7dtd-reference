using System;

// Token: 0x02000CDD RID: 3293
public static class WaterDebugPools
{
	// Token: 0x060064EF RID: 25839 RVA: 0x0027756E File Offset: 0x0027576E
	public static void CreatePools()
	{
		WaterDebugPools.rendererPool = new MemoryPooledObject<WaterDebugRenderer>(250);
		WaterDebugPools.layerPool = new MemoryPooledObject<WaterDebugRendererLayer>(4000);
	}

	// Token: 0x060064F0 RID: 25840 RVA: 0x0027758E File Offset: 0x0027578E
	public static void Cleanup()
	{
		MemoryPooledObject<WaterDebugRenderer> memoryPooledObject = WaterDebugPools.rendererPool;
		if (memoryPooledObject != null)
		{
			memoryPooledObject.Cleanup();
		}
		WaterDebugPools.rendererPool = null;
		MemoryPooledObject<WaterDebugRendererLayer> memoryPooledObject2 = WaterDebugPools.layerPool;
		if (memoryPooledObject2 != null)
		{
			memoryPooledObject2.Cleanup();
		}
		WaterDebugPools.layerPool = null;
	}

	// Token: 0x04004E79 RID: 20089
	[PublicizedFrom(EAccessModifier.Private)]
	public const int maxActiveChunks = 250;

	// Token: 0x04004E7A RID: 20090
	[PublicizedFrom(EAccessModifier.Private)]
	public const int numLayers = 16;

	// Token: 0x04004E7B RID: 20091
	public static MemoryPooledObject<WaterDebugRenderer> rendererPool;

	// Token: 0x04004E7C RID: 20092
	public static MemoryPooledObject<WaterDebugRendererLayer> layerPool;
}
