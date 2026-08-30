using System;

namespace Platform
{
	// Token: 0x02001B63 RID: 7011
	public interface IPlatformSaveGameProvider : IPlatformSaveGameIOProvider
	{
		// Token: 0x170019A2 RID: 6562
		// (get) Token: 0x0600D146 RID: 53574
		ESaveGameProviderStatus Status { get; }

		// Token: 0x170019A3 RID: 6563
		// (get) Token: 0x0600D147 RID: 53575
		IPlatformSaveGameIOProvider Cache { get; }

		// Token: 0x14000124 RID: 292
		// (add) Token: 0x0600D148 RID: 53576
		// (remove) Token: 0x0600D149 RID: 53577
		event Action Initialized;

		// Token: 0x0600D14A RID: 53578
		void Init(IPlatform _owner);

		// Token: 0x0600D14B RID: 53579
		void Destroy();

		// Token: 0x0600D14C RID: 53580
		bool ShouldBackup();

		// Token: 0x0600D14D RID: 53581
		bool ShouldCommit();

		// Token: 0x0600D14E RID: 53582
		double GetCommitProgress();

		// Token: 0x0600D14F RID: 53583
		void Flush(bool waitForFlush);

		// Token: 0x0600D150 RID: 53584
		bool ShouldLimitSize();

		// Token: 0x0600D151 RID: 53585
		void UpdateSizes();

		// Token: 0x0600D152 RID: 53586
		SaveDataSizes GetSizes();
	}
}
