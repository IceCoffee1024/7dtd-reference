using System;

namespace Platform.XBL.Save.Storage
{
	// Token: 0x02001C26 RID: 7206
	public interface ISaveStorageProvider : IDisposable
	{
		// Token: 0x17001A27 RID: 6695
		// (get) Token: 0x0600D533 RID: 54579
		SizeTracker SizeTracker { get; }

		// Token: 0x17001A28 RID: 6696
		// (get) Token: 0x0600D534 RID: 54580
		ISaveStorageContainer RootSaveStorageContainer { get; }

		// Token: 0x0600D535 RID: 54581
		void InitializeAsync(IPlatform owner, long maxSizeBytes, SingleThreadTaskScheduler taskScheduler, OnGameSaveProviderStatusChanged statusChanged);

		// Token: 0x0600D536 RID: 54582
		void Flush(bool waitForFlush);
	}
}
