using System;

namespace Twitch
{
	// Token: 0x0200189D RID: 6301
	public class TwitchLeaderboardEntry
	{
		// Token: 0x0600C1DD RID: 49629 RVA: 0x004724FA File Offset: 0x004706FA
		public TwitchLeaderboardEntry(string username, string usercolor, int kills)
		{
			this.UserName = username;
			this.UserColor = ((usercolor == null) ? "FFFFFF" : usercolor);
			this.Kills = kills;
		}

		// Token: 0x040091A3 RID: 37283
		public string UserName;

		// Token: 0x040091A4 RID: 37284
		public string UserColor;

		// Token: 0x040091A5 RID: 37285
		public int Kills;
	}
}
