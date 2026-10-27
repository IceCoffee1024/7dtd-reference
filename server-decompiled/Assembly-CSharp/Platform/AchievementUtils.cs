using System;

namespace Platform
{
	// Token: 0x02001BB6 RID: 7094
	public static class AchievementUtils
	{
		// Token: 0x0600D435 RID: 54325 RVA: 0x004CEC64 File Offset: 0x004CCE64
		public static bool IsCreativeModeActive()
		{
			return GamePrefs.GetString(EnumGamePrefs.GameMode).Equals(GameModeCreative.TypeName) || GameStats.GetBool(EnumGameStats.IsCreativeMenuEnabled) || GamePrefs.GetBool(EnumGamePrefs.CreativeMenuEnabled) || GamePrefs.GetBool(EnumGamePrefs.DebugMenuEnabled);
		}
	}
}
