using System;

namespace Platform
{
	// Token: 0x02001B85 RID: 7045
	public static class AchievementUtils
	{
		// Token: 0x0600D321 RID: 54049 RVA: 0x004CB13C File Offset: 0x004C933C
		public static bool IsCreativeModeActive()
		{
			return GamePrefs.GetString(EnumGamePrefs.GameMode).Equals(GameModeCreative.TypeName) || GameStats.GetBool(EnumGameStats.IsCreativeMenuEnabled) || GamePrefs.GetBool(EnumGamePrefs.CreativeMenuEnabled) || GamePrefs.GetBool(EnumGamePrefs.DebugMenuEnabled);
		}
	}
}
