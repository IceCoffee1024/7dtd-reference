using System;
using System.Diagnostics;
using Unity.Profiling;

// Token: 0x02000CF6 RID: 3318
public static class WaterStatsProfiler
{
	// Token: 0x0600651F RID: 25887 RVA: 0x000027FC File Offset: 0x000009FC
	public static void SampleTick(WaterStats stats)
	{
	}

	// Token: 0x04004EEA RID: 20202
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerCounter<int> NumChunksProcessed = new ProfilerCounter<int>(ProfilerCategory.Scripts, "Num Chunks Processed", ProfilerMarkerDataUnit.Count);

	// Token: 0x04004EEB RID: 20203
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerCounter<int> NumChunksActive = new ProfilerCounter<int>(ProfilerCategory.Scripts, "Num Chunks Active", ProfilerMarkerDataUnit.Count);

	// Token: 0x04004EEC RID: 20204
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerCounter<int> NumFlowEvents = new ProfilerCounter<int>(ProfilerCategory.Scripts, "Num Flow Events", ProfilerMarkerDataUnit.Count);

	// Token: 0x04004EED RID: 20205
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerCounter<int> NumVoxelProcessed = new ProfilerCounter<int>(ProfilerCategory.Scripts, "Num Voxels Processed", ProfilerMarkerDataUnit.Count);

	// Token: 0x04004EEE RID: 20206
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerCounter<int> NumVoxelsPutToSleep = new ProfilerCounter<int>(ProfilerCategory.Scripts, "Num Voxels Put To Sleep", ProfilerMarkerDataUnit.Count);

	// Token: 0x04004EEF RID: 20207
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerCounter<int> NumVoxelsWokeUp = new ProfilerCounter<int>(ProfilerCategory.Scripts, "Num Voxels Woke Up", ProfilerMarkerDataUnit.Count);

	// Token: 0x02000CF7 RID: 3319
	public struct Timer
	{
		// Token: 0x06006521 RID: 25889 RVA: 0x0027BF73 File Offset: 0x0027A173
		public Timer(string name)
		{
			this.stopwatch = new Stopwatch();
			this.counterValue = new ProfilerCounter<double>(ProfilerCategory.Scripts, name, ProfilerMarkerDataUnit.TimeNanoseconds);
		}

		// Token: 0x06006522 RID: 25890 RVA: 0x0027BF92 File Offset: 0x0027A192
		public void Start()
		{
			this.stopwatch.Start();
		}

		// Token: 0x06006523 RID: 25891 RVA: 0x0027BF9F File Offset: 0x0027A19F
		public void Stop()
		{
			this.stopwatch.Stop();
		}

		// Token: 0x06006524 RID: 25892 RVA: 0x0027BFAC File Offset: 0x0027A1AC
		public void Sample()
		{
			this.stopwatch.Reset();
		}

		// Token: 0x04004EF0 RID: 20208
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly double tickToNanos = 1000000000.0 / (double)Stopwatch.Frequency;

		// Token: 0x04004EF1 RID: 20209
		[PublicizedFrom(EAccessModifier.Private)]
		public Stopwatch stopwatch;

		// Token: 0x04004EF2 RID: 20210
		public ProfilerCounter<double> counterValue;
	}
}
