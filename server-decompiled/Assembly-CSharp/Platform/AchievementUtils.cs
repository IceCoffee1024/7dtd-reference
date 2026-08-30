using System;

namespace Platform
{
	// Token: 0x02001B2C RID: 6956
	public static class AchievementUtils
	{
		// Token: 0x0600D021 RID: 53281 RVA: 0x004C0260 File Offset: 0x004BE460
		public static bool IsCreativeModeActive()
		{
			return GamePrefs.GetString(EnumGamePrefs.GameMode).Equals(GameModeCreative.TypeName) || GameStats.GetBool(EnumGameStats.IsCreativeMenuEnabled) || GamePrefs.GetBool(EnumGamePrefs.CreativeMenuEnabled) || GamePrefs.GetBool(EnumGamePrefs.DebugMenuEnabled);
		}
	}
}
