using System;

namespace Platform
{
	// Token: 0x02001B05 RID: 6917
	public static class EnumAchievementManagerAchievementExtensions
	{
		// Token: 0x0600CF3F RID: 53055 RVA: 0x004BC98A File Offset: 0x004BAB8A
		public static bool IsSupported(this EnumAchievementManagerAchievement _achievement)
		{
			return AchievementData.GetStat(_achievement).IsSupported();
		}
	}
}
