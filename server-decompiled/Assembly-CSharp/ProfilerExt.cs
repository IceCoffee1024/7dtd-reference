using System;
using System.Diagnostics;

// Token: 0x02001484 RID: 5252
public static class ProfilerExt
{
	// Token: 0x0600A4FB RID: 42235 RVA: 0x003D6A3D File Offset: 0x003D4C3D
	[Conditional("ENABLE_PROFILER")]
	public static void BeginSampleThreadSafe(string _caption)
	{
		ThreadManager.IsMainThread();
	}

	// Token: 0x0600A4FC RID: 42236 RVA: 0x003D6A3D File Offset: 0x003D4C3D
	[Conditional("ENABLE_PROFILER")]
	public static void EndSampleThreadSafe()
	{
		ThreadManager.IsMainThread();
	}
}
