using System;

namespace Platform
{
	// Token: 0x02001B2C RID: 6956
	public static class EnumAchievementManagerAchievementExtensions
	{
		// Token: 0x0600D0C8 RID: 53448 RVA: 0x004C2D7E File Offset: 0x004C0F7E
		public static bool IsSupported(this EnumAchievementManagerAchievement _achievement)
		{
			return AchievementData.GetStat(_achievement).IsSupported();
		}
	}
}
