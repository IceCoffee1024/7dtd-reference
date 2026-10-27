using System;

namespace Platform
{
	// Token: 0x02001BB5 RID: 7093
	public interface IAchievementManager
	{
		// Token: 0x0600D42D RID: 54317
		void Init(IPlatform _owner);

		// Token: 0x0600D42E RID: 54318
		void ShowAchievementsUi();

		// Token: 0x0600D42F RID: 54319
		bool IsAchievementStatSupported(EnumAchievementDataStat _stat);

		// Token: 0x0600D430 RID: 54320
		void SetAchievementStat(EnumAchievementDataStat _stat, int _value);

		// Token: 0x0600D431 RID: 54321
		void SetAchievementStat(EnumAchievementDataStat _stat, float _value);

		// Token: 0x0600D432 RID: 54322
		void ResetStats(bool _andAchievements);

		// Token: 0x0600D433 RID: 54323
		void UnlockAllAchievements();

		// Token: 0x0600D434 RID: 54324
		void Destroy();
	}
}
