using System;
using Unity.Collections;

// Token: 0x02000D00 RID: 3328
public struct WaterStats
{
	// Token: 0x06006541 RID: 25921 RVA: 0x0027D31C File Offset: 0x0027B51C
	public static WaterStats Sum(NativeArray<WaterStats> array)
	{
		WaterStats waterStats = default(WaterStats);
		for (int i = 0; i < array.Length; i++)
		{
			waterStats += array[i];
		}
		return waterStats;
	}

	// Token: 0x06006542 RID: 25922 RVA: 0x0027D354 File Offset: 0x0027B554
	public static WaterStats operator +(WaterStats a, WaterStats b)
	{
		return new WaterStats
		{
			NumChunksProcessed = a.NumChunksProcessed + b.NumChunksProcessed,
			NumChunksActive = a.NumChunksActive + b.NumChunksActive,
			NumFlowEvents = a.NumFlowEvents + b.NumFlowEvents,
			NumVoxelsProcessed = a.NumVoxelsProcessed + b.NumVoxelsProcessed,
			NumVoxelsPutToSleep = a.NumVoxelsPutToSleep + b.NumVoxelsPutToSleep,
			NumVoxelsWokeUp = a.NumVoxelsWokeUp + b.NumVoxelsWokeUp
		};
	}

	// Token: 0x06006543 RID: 25923 RVA: 0x0027D3E2 File Offset: 0x0027B5E2
	public void ResetFrame()
	{
		this.NumChunksProcessed = 0;
		this.NumChunksActive = 0;
		this.NumFlowEvents = 0;
		this.NumVoxelsProcessed = 0;
		this.NumVoxelsPutToSleep = 0;
		this.NumVoxelsWokeUp = 0;
	}

	// Token: 0x04004F09 RID: 20233
	public int NumChunksProcessed;

	// Token: 0x04004F0A RID: 20234
	public int NumChunksActive;

	// Token: 0x04004F0B RID: 20235
	public int NumFlowEvents;

	// Token: 0x04004F0C RID: 20236
	public int NumVoxelsProcessed;

	// Token: 0x04004F0D RID: 20237
	public int NumVoxelsPutToSleep;

	// Token: 0x04004F0E RID: 20238
	public int NumVoxelsWokeUp;
}
