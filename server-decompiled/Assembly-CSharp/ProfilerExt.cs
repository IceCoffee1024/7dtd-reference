using System;
using System.Diagnostics;

// Token: 0x02001469 RID: 5225
public static class ProfilerExt
{
	// Token: 0x0600A3FE RID: 41982 RVA: 0x003D16F9 File Offset: 0x003CF8F9
	[Conditional("ENABLE_PROFILER")]
	public static void BeginSampleThreadSafe(string _caption)
	{
		ThreadManager.IsMainThread();
	}

	// Token: 0x0600A3FF RID: 41983 RVA: 0x003D16F9 File Offset: 0x003CF8F9
	[Conditional("ENABLE_PROFILER")]
	public static void EndSampleThreadSafe()
	{
		ThreadManager.IsMainThread();
	}
}
