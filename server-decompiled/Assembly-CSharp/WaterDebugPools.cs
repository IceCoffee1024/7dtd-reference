using System;

// Token: 0x02000CEB RID: 3307
public static class WaterDebugPools
{
	// Token: 0x060064E1 RID: 25825 RVA: 0x0027A912 File Offset: 0x00278B12
	public static void CreatePools()
	{
		WaterDebugPools.rendererPool = new MemoryPooledObject<WaterDebugRenderer>(250);
		WaterDebugPools.layerPool = new MemoryPooledObject<WaterDebugRendererLayer>(4000);
	}

	// Token: 0x060064E2 RID: 25826 RVA: 0x0027A932 File Offset: 0x00278B32
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

	// Token: 0x04004EAF RID: 20143
	[PublicizedFrom(EAccessModifier.Private)]
	public const int maxActiveChunks = 250;

	// Token: 0x04004EB0 RID: 20144
	[PublicizedFrom(EAccessModifier.Private)]
	public const int numLayers = 16;

	// Token: 0x04004EB1 RID: 20145
	public static MemoryPooledObject<WaterDebugRenderer> rendererPool;

	// Token: 0x04004EB2 RID: 20146
	public static MemoryPooledObject<WaterDebugRendererLayer> layerPool;
}
