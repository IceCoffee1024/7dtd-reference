using System;

namespace Platform.XBL.Save.Storage
{
	// Token: 0x02001C7F RID: 7295
	public interface ISaveStorageProvider : IDisposable
	{
		// Token: 0x17001B3F RID: 6975
		// (get) Token: 0x0600D83C RID: 55356
		SizeTracker SizeTracker { get; }

		// Token: 0x17001B40 RID: 6976
		// (get) Token: 0x0600D83D RID: 55357
		ISaveStorageContainer RootSaveStorageContainer { get; }

		// Token: 0x0600D83E RID: 55358
		void InitializeAsync(IPlatform owner, long maxSizeBytes, SingleThreadTaskScheduler taskScheduler, OnGameSaveProviderStatusChanged statusChanged);

		// Token: 0x0600D83F RID: 55359
		void Flush(bool waitForFlush);
	}
}
