using System;
using Unity.Collections;

// Token: 0x02000CF5 RID: 3317
public struct WaterStats
{
	// Token: 0x0600651C RID: 25884 RVA: 0x0027BDF4 File Offset: 0x00279FF4
	public static WaterStats Sum(NativeArray<WaterStats> array)
	{
		WaterStats waterStats = default(WaterStats);
		for (int i = 0; i < array.Length; i++)
		{
			waterStats += array[i];
		}
		return waterStats;
	}

	// Token: 0x0600651D RID: 25885 RVA: 0x0027BE2C File Offset: 0x0027A02C
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

	// Token: 0x0600651E RID: 25886 RVA: 0x0027BEBA File Offset: 0x0027A0BA
	public void ResetFrame()
	{
		this.NumChunksProcessed = 0;
		this.NumChunksActive = 0;
		this.NumFlowEvents = 0;
		this.NumVoxelsProcessed = 0;
		this.NumVoxelsPutToSleep = 0;
		this.NumVoxelsWokeUp = 0;
	}

	// Token: 0x04004EE4 RID: 20196
	public int NumChunksProcessed;

	// Token: 0x04004EE5 RID: 20197
	public int NumChunksActive;

	// Token: 0x04004EE6 RID: 20198
	public int NumFlowEvents;

	// Token: 0x04004EE7 RID: 20199
	public int NumVoxelsProcessed;

	// Token: 0x04004EE8 RID: 20200
	public int NumVoxelsPutToSleep;

	// Token: 0x04004EE9 RID: 20201
	public int NumVoxelsWokeUp;
}
