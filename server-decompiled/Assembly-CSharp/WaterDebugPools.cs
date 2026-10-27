using System;

// Token: 0x02000CF6 RID: 3318
public static class WaterDebugPools
{
	// Token: 0x06006506 RID: 25862 RVA: 0x0027BE3A File Offset: 0x0027A03A
	public static void CreatePools()
	{
		WaterDebugPools.rendererPool = new MemoryPooledObject<WaterDebugRenderer>(250);
		WaterDebugPools.layerPool = new MemoryPooledObject<WaterDebugRendererLayer>(4000);
	}

	// Token: 0x06006507 RID: 25863 RVA: 0x0027BE5A File Offset: 0x0027A05A
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

	// Token: 0x04004ED4 RID: 20180
	[PublicizedFrom(EAccessModifier.Private)]
	public const int maxActiveChunks = 250;

	// Token: 0x04004ED5 RID: 20181
	[PublicizedFrom(EAccessModifier.Private)]
	public const int numLayers = 16;

	// Token: 0x04004ED6 RID: 20182
	public static MemoryPooledObject<WaterDebugRenderer> rendererPool;

	// Token: 0x04004ED7 RID: 20183
	public static MemoryPooledObject<WaterDebugRendererLayer> layerPool;
}
