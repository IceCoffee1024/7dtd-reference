using System;

namespace Twitch
{
	// Token: 0x0200186C RID: 6252
	public class TwitchLeaderboardEntry
	{
		// Token: 0x0600C0C9 RID: 49353 RVA: 0x0046EB4E File Offset: 0x0046CD4E
		public TwitchLeaderboardEntry(string username, string usercolor, int kills)
		{
			this.UserName = username;
			this.UserColor = ((usercolor == null) ? "FFFFFF" : usercolor);
			this.Kills = kills;
		}

		// Token: 0x040090BA RID: 37050
		public string UserName;

		// Token: 0x040090BB RID: 37051
		public string UserColor;

		// Token: 0x040090BC RID: 37052
		public int Kills;
	}
}
