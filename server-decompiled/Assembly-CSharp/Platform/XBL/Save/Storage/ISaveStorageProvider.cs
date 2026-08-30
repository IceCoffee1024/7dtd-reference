using System;

namespace Platform.XBL.Save.Storage
{
	// Token: 0x02001C4D RID: 7245
	public interface ISaveStorageProvider : IDisposable
	{
		// Token: 0x17001A99 RID: 6809
		// (get) Token: 0x0600D6C3 RID: 54979
		SizeTracker SizeTracker { get; }

		// Token: 0x17001A9A RID: 6810
		// (get) Token: 0x0600D6C4 RID: 54980
		ISaveStorageContainer RootSaveStorageContainer { get; }

		// Token: 0x0600D6C5 RID: 54981
		void InitializeAsync(IPlatform owner, long maxSizeBytes, SingleThreadTaskScheduler taskScheduler, OnGameSaveProviderStatusChanged statusChanged);

		// Token: 0x0600D6C6 RID: 54982
		void Flush(bool waitForFlush);
	}
}
