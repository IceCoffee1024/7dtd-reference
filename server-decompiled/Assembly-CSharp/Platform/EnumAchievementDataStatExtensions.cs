using System;

namespace Platform
{
	// Token: 0x02001B92 RID: 7058
	public static class EnumAchievementDataStatExtensions
	{
		// Token: 0x0600D354 RID: 54100 RVA: 0x004CB39B File Offset: 0x004C959B
		public static bool IsSupported(this EnumAchievementDataStat _stat)
		{
			IAchievementManager achievementManager = PlatformManager.NativePlatform.AchievementManager;
			return achievementManager == null || achievementManager.IsAchievementStatSupported(_stat);
		}
	}
}
