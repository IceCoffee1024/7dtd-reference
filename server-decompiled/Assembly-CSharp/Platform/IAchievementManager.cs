using System;

namespace Platform
{
	// Token: 0x02001B52 RID: 6994
	public interface IAchievementManager
	{
		// Token: 0x0600D1A2 RID: 53666
		void Init(IPlatform _owner);

		// Token: 0x0600D1A3 RID: 53667
		void ShowAchievementsUi();

		// Token: 0x0600D1A4 RID: 53668
		bool IsAchievementStatSupported(EnumAchievementDataStat _stat);

		// Token: 0x0600D1A5 RID: 53669
		void SetAchievementStat(EnumAchievementDataStat _stat, int _value);

		// Token: 0x0600D1A6 RID: 53670
		void SetAchievementStat(EnumAchievementDataStat _stat, float _value);

		// Token: 0x0600D1A7 RID: 53671
		void ResetStats(bool _andAchievements);

		// Token: 0x0600D1A8 RID: 53672
		void UnlockAllAchievements();

		// Token: 0x0600D1A9 RID: 53673
		void Destroy();
	}
}
