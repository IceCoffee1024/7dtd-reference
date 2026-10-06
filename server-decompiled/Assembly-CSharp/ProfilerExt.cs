using System;
using System.Diagnostics;

// Token: 0x020014A4 RID: 5284
public static class ProfilerExt
{
	// Token: 0x0600A5A9 RID: 42409 RVA: 0x003DB6C9 File Offset: 0x003D98C9
	[Conditional("ENABLE_PROFILER")]
	public static void BeginSampleThreadSafe(string _caption)
	{
		ThreadManager.IsMainThread();
	}

	// Token: 0x0600A5AA RID: 42410 RVA: 0x003DB6C9 File Offset: 0x003D98C9
	[Conditional("ENABLE_PROFILER")]
	public static void EndSampleThreadSafe()
	{
		ThreadManager.IsMainThread();
	}
}
