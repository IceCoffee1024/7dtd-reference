using System;

// Token: 0x02000CCB RID: 3275
public static class WaterDebugPools
{
	// Token: 0x06006463 RID: 25699 RVA: 0x00274672 File Offset: 0x00272872
	public static void CreatePools()
	{
		WaterDebugPools.rendererPool = new MemoryPooledObject<WaterDebugRenderer>(250);
		WaterDebugPools.layerPool = new MemoryPooledObject<WaterDebugRendererLayer>(4000);
	}

	// Token: 0x06006464 RID: 25700 RVA: 0x00274692 File Offset: 0x00272892
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

	// Token: 0x04004DDB RID: 19931
	[PublicizedFrom(EAccessModifier.Private)]
	public const int maxActiveChunks = 250;

	// Token: 0x04004DDC RID: 19932
	[PublicizedFrom(EAccessModifier.Private)]
	public const int numLayers = 16;

	// Token: 0x04004DDD RID: 19933
	public static MemoryPooledObject<WaterDebugRenderer> rendererPool;

	// Token: 0x04004DDE RID: 19934
	public static MemoryPooledObject<WaterDebugRendererLayer> layerPool;
}
