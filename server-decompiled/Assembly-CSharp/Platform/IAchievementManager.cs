using System;

namespace Platform
{
	// Token: 0x02001B84 RID: 7044
	public interface IAchievementManager
	{
		// Token: 0x0600D319 RID: 54041
		void Init(IPlatform _owner);

		// Token: 0x0600D31A RID: 54042
		void ShowAchievementsUi();

		// Token: 0x0600D31B RID: 54043
		bool IsAchievementStatSupported(EnumAchievementDataStat _stat);

		// Token: 0x0600D31C RID: 54044
		void SetAchievementStat(EnumAchievementDataStat _stat, int _value);

		// Token: 0x0600D31D RID: 54045
		void SetAchievementStat(EnumAchievementDataStat _stat, float _value);

		// Token: 0x0600D31E RID: 54046
		void ResetStats(bool _andAchievements);

		// Token: 0x0600D31F RID: 54047
		void UnlockAllAchievements();

		// Token: 0x0600D320 RID: 54048
		void Destroy();
	}
}
