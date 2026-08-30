using System;

namespace Platform
{
	// Token: 0x02001B2F RID: 6959
	public static class EnumAchievementDataStatExtensions
	{
		// Token: 0x0600D0C9 RID: 53449 RVA: 0x004C2D8B File Offset: 0x004C0F8B
		public static bool IsSupported(this EnumAchievementDataStat _stat)
		{
			IAchievementManager achievementManager = PlatformManager.NativePlatform.AchievementManager;
			return achievementManager == null || achievementManager.IsAchievementStatSupported(_stat);
		}
	}
}
