using System;

namespace Platform
{
	// Token: 0x02001B61 RID: 7009
	public static class EnumAchievementDataStatExtensions
	{
		// Token: 0x0600D240 RID: 53824 RVA: 0x004C7873 File Offset: 0x004C5A73
		public static bool IsSupported(this EnumAchievementDataStat _stat)
		{
			IAchievementManager achievementManager = PlatformManager.NativePlatform.AchievementManager;
			return achievementManager == null || achievementManager.IsAchievementStatSupported(_stat);
		}
	}
}
