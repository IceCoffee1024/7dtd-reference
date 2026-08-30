using System;
using System.Collections.Generic;

namespace Services.Analytics
{
	// Token: 0x0200167E RID: 5758
	public static class EventTypes
	{
		// Token: 0x04008759 RID: 34649
		public static readonly Dictionary<string, int> VersionDictionary = new Dictionary<string, int>
		{
			{ "login", 3 },
			{ "heartbeat", 5 },
			{ "hardware_info", 2 },
			{ "server_start", 3 },
			{ "player_join_server", 2 },
			{ "challenge_claimed", 1 },
			{ "challenge_completed", 1 },
			{ "dlc_ownership", 0 },
			{ "cosmetic_usage", 0 }
		};
	}
}
