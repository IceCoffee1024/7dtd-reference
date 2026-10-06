using System;
using System.Collections.Generic;

namespace Services.Analytics
{
	// Token: 0x0200169D RID: 5789
	public static class EventTypes
	{
		// Token: 0x040087AF RID: 34735
		public static readonly Dictionary<string, int> VersionDictionary = new Dictionary<string, int>
		{
			{ "login", 3 },
			{ "heartbeat", 5 },
			{ "hardware_info", 2 },
			{ "server_start", 3 },
			{ "player_join_server", 3 },
			{ "challenge_claimed", 1 },
			{ "challenge_completed", 1 },
			{ "dlc_ownership", 0 },
			{ "cosmetic_usage", 0 },
			{ "quest_added", 3 },
			{ "player_death", 2 },
			{ "quest_removed", 2 },
			{ "equipment_usage", 0 },
			{ "quest_progression", 4 }
		};
	}
}
