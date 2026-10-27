using System;
using System.Diagnostics;

// Token: 0x020014B6 RID: 5302
public static class ProfilerExt
{
	// Token: 0x0600A601 RID: 42497 RVA: 0x003DDACD File Offset: 0x003DBCCD
	[Conditional("ENABLE_PROFILER")]
	public static void BeginSampleThreadSafe(string _caption)
	{
		ThreadManager.IsMainThread();
	}

	// Token: 0x0600A602 RID: 42498 RVA: 0x003DDACD File Offset: 0x003DBCCD
	[Conditional("ENABLE_PROFILER")]
	public static void EndSampleThreadSafe()
	{
		ThreadManager.IsMainThread();
	}
}
