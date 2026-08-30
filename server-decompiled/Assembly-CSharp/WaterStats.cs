using System;
using Unity.Collections;

// Token: 0x02000CD5 RID: 3285
public struct WaterStats
{
	// Token: 0x0600649E RID: 25758 RVA: 0x00275B54 File Offset: 0x00273D54
	public static WaterStats Sum(NativeArray<WaterStats> array)
	{
		WaterStats waterStats = default(WaterStats);
		for (int i = 0; i < array.Length; i++)
		{
			waterStats += array[i];
		}
		return waterStats;
	}

	// Token: 0x0600649F RID: 25759 RVA: 0x00275B8C File Offset: 0x00273D8C
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

	// Token: 0x060064A0 RID: 25760 RVA: 0x00275C1A File Offset: 0x00273E1A
	public void ResetFrame()
	{
		this.NumChunksProcessed = 0;
		this.NumChunksActive = 0;
		this.NumFlowEvents = 0;
		this.NumVoxelsProcessed = 0;
		this.NumVoxelsPutToSleep = 0;
		this.NumVoxelsWokeUp = 0;
	}

	// Token: 0x04004E10 RID: 19984
	public int NumChunksProcessed;

	// Token: 0x04004E11 RID: 19985
	public int NumChunksActive;

	// Token: 0x04004E12 RID: 19986
	public int NumFlowEvents;

	// Token: 0x04004E13 RID: 19987
	public int NumVoxelsProcessed;

	// Token: 0x04004E14 RID: 19988
	public int NumVoxelsPutToSleep;

	// Token: 0x04004E15 RID: 19989
	public int NumVoxelsWokeUp;
}
