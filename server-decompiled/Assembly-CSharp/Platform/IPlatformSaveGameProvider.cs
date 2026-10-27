using System;

namespace Platform
{
	// Token: 0x02001BED RID: 7149
	public interface IPlatformSaveGameProvider : IPlatformSaveGameIOProvider
	{
		// Token: 0x17001B02 RID: 6914
		// (get) Token: 0x0600D55D RID: 54621
		ESaveGameProviderStatus Status { get; }

		// Token: 0x17001B03 RID: 6915
		// (get) Token: 0x0600D55E RID: 54622
		IPlatformSaveGameIOProvider Cache { get; }

		// Token: 0x14000124 RID: 292
		// (add) Token: 0x0600D55F RID: 54623
		// (remove) Token: 0x0600D560 RID: 54624
		event Action Initialized;

		// Token: 0x0600D561 RID: 54625
		void Init(IPlatform _owner);

		// Token: 0x0600D562 RID: 54626
		void Destroy();

		// Token: 0x0600D563 RID: 54627
		bool ShouldBackup();

		// Token: 0x0600D564 RID: 54628
		bool ShouldCommit();

		// Token: 0x0600D565 RID: 54629
		double GetCommitProgress();

		// Token: 0x0600D566 RID: 54630
		void Flush(bool waitForFlush);

		// Token: 0x0600D567 RID: 54631
		bool ShouldLimitSize();

		// Token: 0x0600D568 RID: 54632
		void UpdateSizes();

		// Token: 0x0600D569 RID: 54633
		SaveDataSizes GetSizes();
	}
}
