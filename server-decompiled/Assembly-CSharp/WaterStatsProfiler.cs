using System;
using System.Diagnostics;
using Unity.Profiling;

// Token: 0x02000CE8 RID: 3304
public static class WaterStatsProfiler
{
	// Token: 0x0600652D RID: 25901 RVA: 0x000027FC File Offset: 0x000009FC
	public static void SampleTick(WaterStats stats)
	{
	}

	// Token: 0x04004EB4 RID: 20148
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerCounter<int> NumChunksProcessed = new ProfilerCounter<int>(ProfilerCategory.Scripts, "Num Chunks Processed", ProfilerMarkerDataUnit.Count);

	// Token: 0x04004EB5 RID: 20149
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerCounter<int> NumChunksActive = new ProfilerCounter<int>(ProfilerCategory.Scripts, "Num Chunks Active", ProfilerMarkerDataUnit.Count);

	// Token: 0x04004EB6 RID: 20150
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerCounter<int> NumFlowEvents = new ProfilerCounter<int>(ProfilerCategory.Scripts, "Num Flow Events", ProfilerMarkerDataUnit.Count);

	// Token: 0x04004EB7 RID: 20151
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerCounter<int> NumVoxelProcessed = new ProfilerCounter<int>(ProfilerCategory.Scripts, "Num Voxels Processed", ProfilerMarkerDataUnit.Count);

	// Token: 0x04004EB8 RID: 20152
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerCounter<int> NumVoxelsPutToSleep = new ProfilerCounter<int>(ProfilerCategory.Scripts, "Num Voxels Put To Sleep", ProfilerMarkerDataUnit.Count);

	// Token: 0x04004EB9 RID: 20153
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerCounter<int> NumVoxelsWokeUp = new ProfilerCounter<int>(ProfilerCategory.Scripts, "Num Voxels Woke Up", ProfilerMarkerDataUnit.Count);

	// Token: 0x02000CE9 RID: 3305
	public struct Timer
	{
		// Token: 0x0600652F RID: 25903 RVA: 0x00278BCF File Offset: 0x00276DCF
		public Timer(string name)
		{
			this.stopwatch = new Stopwatch();
			this.counterValue = new ProfilerCounter<double>(ProfilerCategory.Scripts, name, ProfilerMarkerDataUnit.TimeNanoseconds);
		}

		// Token: 0x06006530 RID: 25904 RVA: 0x00278BEE File Offset: 0x00276DEE
		public void Start()
		{
			this.stopwatch.Start();
		}

		// Token: 0x06006531 RID: 25905 RVA: 0x00278BFB File Offset: 0x00276DFB
		public void Stop()
		{
			this.stopwatch.Stop();
		}

		// Token: 0x06006532 RID: 25906 RVA: 0x00278C08 File Offset: 0x00276E08
		public void Sample()
		{
			this.stopwatch.Reset();
		}

		// Token: 0x04004EBA RID: 20154
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly double tickToNanos = 1000000000.0 / (double)Stopwatch.Frequency;

		// Token: 0x04004EBB RID: 20155
		[PublicizedFrom(EAccessModifier.Private)]
		public Stopwatch stopwatch;

		// Token: 0x04004EBC RID: 20156
		public ProfilerCounter<double> counterValue;
	}
}
