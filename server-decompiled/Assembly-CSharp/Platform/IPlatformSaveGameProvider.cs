using System;

namespace Platform
{
	// Token: 0x02001B8A RID: 7050
	public interface IPlatformSaveGameProvider : IPlatformSaveGameIOProvider
	{
		// Token: 0x17001A14 RID: 6676
		// (get) Token: 0x0600D2D1 RID: 53969
		ESaveGameProviderStatus Status { get; }

		// Token: 0x17001A15 RID: 6677
		// (get) Token: 0x0600D2D2 RID: 53970
		IPlatformSaveGameIOProvider Cache { get; }

		// Token: 0x14000125 RID: 293
		// (add) Token: 0x0600D2D3 RID: 53971
		// (remove) Token: 0x0600D2D4 RID: 53972
		event Action Initialized;

		// Token: 0x0600D2D5 RID: 53973
		void Init(IPlatform _owner);

		// Token: 0x0600D2D6 RID: 53974
		void Destroy();

		// Token: 0x0600D2D7 RID: 53975
		bool ShouldBackup();

		// Token: 0x0600D2D8 RID: 53976
		bool ShouldCommit();

		// Token: 0x0600D2D9 RID: 53977
		double GetCommitProgress();

		// Token: 0x0600D2DA RID: 53978
		void Flush(bool waitForFlush);

		// Token: 0x0600D2DB RID: 53979
		bool ShouldLimitSize();

		// Token: 0x0600D2DC RID: 53980
		void UpdateSizes();

		// Token: 0x0600D2DD RID: 53981
		SaveDataSizes GetSizes();
	}
}
