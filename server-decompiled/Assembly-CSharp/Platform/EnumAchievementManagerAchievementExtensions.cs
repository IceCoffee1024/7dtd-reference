using System;

namespace Platform
{
	// Token: 0x02001B8F RID: 7055
	public static class EnumAchievementManagerAchievementExtensions
	{
		// Token: 0x0600D353 RID: 54099 RVA: 0x004CB38E File Offset: 0x004C958E
		public static bool IsSupported(this EnumAchievementManagerAchievement _achievement)
		{
			return AchievementData.GetStat(_achievement).IsSupported();
		}
	}
}
