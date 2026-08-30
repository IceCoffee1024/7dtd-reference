using System;

namespace Platform
{
	// Token: 0x02001B53 RID: 6995
	public static class AchievementUtils
	{
		// Token: 0x0600D1AA RID: 53674 RVA: 0x004C6654 File Offset: 0x004C4854
		public static bool IsCreativeModeActive()
		{
			return GamePrefs.GetString(EnumGamePrefs.GameMode).Equals(GameModeCreative.TypeName) || GameStats.GetBool(EnumGameStats.IsCreativeMenuEnabled) || GamePrefs.GetBool(EnumGamePrefs.CreativeMenuEnabled) || GamePrefs.GetBool(EnumGamePrefs.DebugMenuEnabled);
		}
	}
}
