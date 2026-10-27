using System;

namespace Platform.XBL.Save.Storage
{
	// Token: 0x02001CB0 RID: 7344
	public interface ISaveStorageProvider : IDisposable
	{
		// Token: 0x17001B87 RID: 7047
		// (get) Token: 0x0600D950 RID: 55632
		SizeTracker SizeTracker { get; }

		// Token: 0x17001B88 RID: 7048
		// (get) Token: 0x0600D951 RID: 55633
		ISaveStorageContainer RootSaveStorageContainer { get; }

		// Token: 0x0600D952 RID: 55634
		void InitializeAsync(IPlatform owner, long maxSizeBytes, SingleThreadTaskScheduler taskScheduler, OnGameSaveProviderStatusChanged statusChanged);

		// Token: 0x0600D953 RID: 55635
		void Flush(bool waitForFlush);
	}
}
