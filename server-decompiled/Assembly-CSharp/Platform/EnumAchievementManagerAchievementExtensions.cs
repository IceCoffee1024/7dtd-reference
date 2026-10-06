using System;

namespace Platform
{
	// Token: 0x02001B5E RID: 7006
	public static class EnumAchievementManagerAchievementExtensions
	{
		// Token: 0x0600D23F RID: 53823 RVA: 0x004C7866 File Offset: 0x004C5A66
		public static bool IsSupported(this EnumAchievementManagerAchievement _achievement)
		{
			return AchievementData.GetStat(_achievement).IsSupported();
		}
	}
}
