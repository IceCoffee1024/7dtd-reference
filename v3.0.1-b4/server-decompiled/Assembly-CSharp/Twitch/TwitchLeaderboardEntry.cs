using System;

namespace Twitch
{
	// Token: 0x0200181A RID: 6170
	public class TwitchLeaderboardEntry
	{
		// Token: 0x0600BDEC RID: 48620 RVA: 0x00464876 File Offset: 0x00462A76
		public TwitchLeaderboardEntry(string username, string usercolor, int kills)
		{
			this.UserName = username;
			this.UserColor = ((usercolor == null) ? "FFFFFF" : usercolor);
			this.Kills = kills;
		}

		// Token: 0x04008ED8 RID: 36568
		public string UserName;

		// Token: 0x04008ED9 RID: 36569
		public string UserColor;

		// Token: 0x04008EDA RID: 36570
		public int Kills;
	}
}
