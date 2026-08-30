using System;

namespace Platform
{
	// Token: 0x02001B08 RID: 6920
	public static class EnumAchievementDataStatExtensions
	{
		// Token: 0x0600CF40 RID: 53056 RVA: 0x004BC997 File Offset: 0x004BAB97
		public static bool IsSupported(this EnumAchievementDataStat _stat)
		{
			IAchievementManager achievementManager = PlatformManager.NativePlatform.AchievementManager;
			return achievementManager == null || achievementManager.IsAchievementStatSupported(_stat);
		}
	}
}
