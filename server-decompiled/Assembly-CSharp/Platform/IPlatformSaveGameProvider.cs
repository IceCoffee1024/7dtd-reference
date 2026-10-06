using System;

namespace Platform
{
	// Token: 0x02001BBC RID: 7100
	public interface IPlatformSaveGameProvider : IPlatformSaveGameIOProvider
	{
		// Token: 0x17001ABA RID: 6842
		// (get) Token: 0x0600D449 RID: 54345
		ESaveGameProviderStatus Status { get; }

		// Token: 0x17001ABB RID: 6843
		// (get) Token: 0x0600D44A RID: 54346
		IPlatformSaveGameIOProvider Cache { get; }

		// Token: 0x14000124 RID: 292
		// (add) Token: 0x0600D44B RID: 54347
		// (remove) Token: 0x0600D44C RID: 54348
		event Action Initialized;

		// Token: 0x0600D44D RID: 54349
		void Init(IPlatform _owner);

		// Token: 0x0600D44E RID: 54350
		void Destroy();

		// Token: 0x0600D44F RID: 54351
		bool ShouldBackup();

		// Token: 0x0600D450 RID: 54352
		bool ShouldCommit();

		// Token: 0x0600D451 RID: 54353
		double GetCommitProgress();

		// Token: 0x0600D452 RID: 54354
		void Flush(bool waitForFlush);

		// Token: 0x0600D453 RID: 54355
		bool ShouldLimitSize();

		// Token: 0x0600D454 RID: 54356
		void UpdateSizes();

		// Token: 0x0600D455 RID: 54357
		SaveDataSizes GetSizes();
	}
}
