using System;

namespace Platform
{
	// Token: 0x02001B2B RID: 6955
	public interface IAchievementManager
	{
		// Token: 0x0600D019 RID: 53273
		void Init(IPlatform _owner);

		// Token: 0x0600D01A RID: 53274
		void ShowAchievementsUi();

		// Token: 0x0600D01B RID: 53275
		bool IsAchievementStatSupported(EnumAchievementDataStat _stat);

		// Token: 0x0600D01C RID: 53276
		void SetAchievementStat(EnumAchievementDataStat _stat, int _value);

		// Token: 0x0600D01D RID: 53277
		void SetAchievementStat(EnumAchievementDataStat _stat, float _value);

		// Token: 0x0600D01E RID: 53278
		void ResetStats(bool _andAchievements);

		// Token: 0x0600D01F RID: 53279
		void UnlockAllAchievements();

		// Token: 0x0600D020 RID: 53280
		void Destroy();
	}
}
