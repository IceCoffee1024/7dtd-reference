using System;
using System.Diagnostics;
using Unity.Profiling;

// Token: 0x02000CD6 RID: 3286
public static class WaterStatsProfiler
{
	// Token: 0x060064A1 RID: 25761 RVA: 0x000027FC File Offset: 0x000009FC
	public static void SampleTick(WaterStats stats)
	{
	}

	// Token: 0x04004E16 RID: 19990
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerCounter<int> NumChunksProcessed = new ProfilerCounter<int>(ProfilerCategory.Scripts, "Num Chunks Processed", ProfilerMarkerDataUnit.Count);

	// Token: 0x04004E17 RID: 19991
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerCounter<int> NumChunksActive = new ProfilerCounter<int>(ProfilerCategory.Scripts, "Num Chunks Active", ProfilerMarkerDataUnit.Count);

	// Token: 0x04004E18 RID: 19992
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerCounter<int> NumFlowEvents = new ProfilerCounter<int>(ProfilerCategory.Scripts, "Num Flow Events", ProfilerMarkerDataUnit.Count);

	// Token: 0x04004E19 RID: 19993
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerCounter<int> NumVoxelProcessed = new ProfilerCounter<int>(ProfilerCategory.Scripts, "Num Voxels Processed", ProfilerMarkerDataUnit.Count);

	// Token: 0x04004E1A RID: 19994
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerCounter<int> NumVoxelsPutToSleep = new ProfilerCounter<int>(ProfilerCategory.Scripts, "Num Voxels Put To Sleep", ProfilerMarkerDataUnit.Count);

	// Token: 0x04004E1B RID: 19995
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerCounter<int> NumVoxelsWokeUp = new ProfilerCounter<int>(ProfilerCategory.Scripts, "Num Voxels Woke Up", ProfilerMarkerDataUnit.Count);

	// Token: 0x02000CD7 RID: 3287
	public struct Timer
	{
		// Token: 0x060064A3 RID: 25763 RVA: 0x00275CD3 File Offset: 0x00273ED3
		public Timer(string name)
		{
			this.stopwatch = new Stopwatch();
			this.counterValue = new ProfilerCounter<double>(ProfilerCategory.Scripts, name, ProfilerMarkerDataUnit.TimeNanoseconds);
		}

		// Token: 0x060064A4 RID: 25764 RVA: 0x00275CF2 File Offset: 0x00273EF2
		public void Start()
		{
			this.stopwatch.Start();
		}

		// Token: 0x060064A5 RID: 25765 RVA: 0x00275CFF File Offset: 0x00273EFF
		public void Stop()
		{
			this.stopwatch.Stop();
		}

		// Token: 0x060064A6 RID: 25766 RVA: 0x00275D0C File Offset: 0x00273F0C
		public void Sample()
		{
			this.stopwatch.Reset();
		}

		// Token: 0x04004E1C RID: 19996
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly double tickToNanos = 1000000000.0 / (double)Stopwatch.Frequency;

		// Token: 0x04004E1D RID: 19997
		[PublicizedFrom(EAccessModifier.Private)]
		public Stopwatch stopwatch;

		// Token: 0x04004E1E RID: 19998
		public ProfilerCounter<double> counterValue;
	}
}
