using System;
using System.Diagnostics;
using Unity.Profiling;

// Token: 0x02000D01 RID: 3329
public static class WaterStatsProfiler
{
	// Token: 0x06006544 RID: 25924 RVA: 0x000027FC File Offset: 0x000009FC
	public static void SampleTick(WaterStats stats)
	{
	}

	// Token: 0x04004F0F RID: 20239
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerCounter<int> NumChunksProcessed = new ProfilerCounter<int>(ProfilerCategory.Scripts, "Num Chunks Processed", ProfilerMarkerDataUnit.Count);

	// Token: 0x04004F10 RID: 20240
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerCounter<int> NumChunksActive = new ProfilerCounter<int>(ProfilerCategory.Scripts, "Num Chunks Active", ProfilerMarkerDataUnit.Count);

	// Token: 0x04004F11 RID: 20241
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerCounter<int> NumFlowEvents = new ProfilerCounter<int>(ProfilerCategory.Scripts, "Num Flow Events", ProfilerMarkerDataUnit.Count);

	// Token: 0x04004F12 RID: 20242
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerCounter<int> NumVoxelProcessed = new ProfilerCounter<int>(ProfilerCategory.Scripts, "Num Voxels Processed", ProfilerMarkerDataUnit.Count);

	// Token: 0x04004F13 RID: 20243
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerCounter<int> NumVoxelsPutToSleep = new ProfilerCounter<int>(ProfilerCategory.Scripts, "Num Voxels Put To Sleep", ProfilerMarkerDataUnit.Count);

	// Token: 0x04004F14 RID: 20244
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerCounter<int> NumVoxelsWokeUp = new ProfilerCounter<int>(ProfilerCategory.Scripts, "Num Voxels Woke Up", ProfilerMarkerDataUnit.Count);

	// Token: 0x02000D02 RID: 3330
	public struct Timer
	{
		// Token: 0x06006546 RID: 25926 RVA: 0x0027D49B File Offset: 0x0027B69B
		public Timer(string name)
		{
			this.stopwatch = new Stopwatch();
			this.counterValue = new ProfilerCounter<double>(ProfilerCategory.Scripts, name, ProfilerMarkerDataUnit.TimeNanoseconds);
		}

		// Token: 0x06006547 RID: 25927 RVA: 0x0027D4BA File Offset: 0x0027B6BA
		public void Start()
		{
			this.stopwatch.Start();
		}

		// Token: 0x06006548 RID: 25928 RVA: 0x0027D4C7 File Offset: 0x0027B6C7
		public void Stop()
		{
			this.stopwatch.Stop();
		}

		// Token: 0x06006549 RID: 25929 RVA: 0x0027D4D4 File Offset: 0x0027B6D4
		public void Sample()
		{
			this.stopwatch.Reset();
		}

		// Token: 0x04004F15 RID: 20245
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly double tickToNanos = 1000000000.0 / (double)Stopwatch.Frequency;

		// Token: 0x04004F16 RID: 20246
		[PublicizedFrom(EAccessModifier.Private)]
		public Stopwatch stopwatch;

		// Token: 0x04004F17 RID: 20247
		public ProfilerCounter<double> counterValue;
	}
}
