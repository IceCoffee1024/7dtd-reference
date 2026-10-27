using System;
using System.Collections.Generic;

namespace Services.Analytics
{
	// Token: 0x020016AF RID: 5807
	public static class EventTypes
	{
		// Token: 0x040087F0 RID: 34800
		public static readonly Dictionary<string, int> VersionDictionary = new Dictionary<string, int>
		{
			{ "login", 3 },
			{ "heartbeat", 5 },
			{ "hardware_info", 2 },
			{ "server_start", 3 },
			{ "player_join_server", 3 },
			{ "challenge_claimed", 2 },
			{ "challenge_completed", 2 },
			{ "dlc_ownership", 0 },
			{ "cosmetic_usage", 0 },
			{ "quest_added", 3 },
			{ "player_death", 3 },
			{ "quest_removed", 2 },
			{ "equipment_usage", 0 },
			{ "quest_progression", 4 },
			{ "player_skill_points", 1 },
			{ "player_crafting_skill_progression", 1 },
			{ "camera_toggle", 0 },
			{ "player_perk_progression", 1 },
			{ "quest_reward", 0 },
			{ "toolbelt_usage", 0 },
			{ "player_state", 0 }
		};
	}
}
