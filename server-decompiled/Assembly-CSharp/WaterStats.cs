using System;
using Unity.Collections;

// Token: 0x02000CE7 RID: 3303
public struct WaterStats
{
	// Token: 0x0600652A RID: 25898 RVA: 0x00278A50 File Offset: 0x00276C50
	public static WaterStats Sum(NativeArray<WaterStats> array)
	{
		WaterStats waterStats = default(WaterStats);
		for (int i = 0; i < array.Length; i++)
		{
			waterStats += array[i];
		}
		return waterStats;
	}

	// Token: 0x0600652B RID: 25899 RVA: 0x00278A88 File Offset: 0x00276C88
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

	// Token: 0x0600652C RID: 25900 RVA: 0x00278B16 File Offset: 0x00276D16
	public void ResetFrame()
	{
		this.NumChunksProcessed = 0;
		this.NumChunksActive = 0;
		this.NumFlowEvents = 0;
		this.NumVoxelsProcessed = 0;
		this.NumVoxelsPutToSleep = 0;
		this.NumVoxelsWokeUp = 0;
	}

	// Token: 0x04004EAE RID: 20142
	public int NumChunksProcessed;

	// Token: 0x04004EAF RID: 20143
	public int NumChunksActive;

	// Token: 0x04004EB0 RID: 20144
	public int NumFlowEvents;

	// Token: 0x04004EB1 RID: 20145
	public int NumVoxelsProcessed;

	// Token: 0x04004EB2 RID: 20146
	public int NumVoxelsPutToSleep;

	// Token: 0x04004EB3 RID: 20147
	public int NumVoxelsWokeUp;
}
