using System;

namespace Twitch
{
	// Token: 0x02001840 RID: 6208
	public class TwitchLeaderboardEntry
	{
		// Token: 0x0600BF71 RID: 49009 RVA: 0x0046A612 File Offset: 0x00468812
		public TwitchLeaderboardEntry(string username, string usercolor, int kills)
		{
			this.UserName = username;
			this.UserColor = ((usercolor == null) ? "FFFFFF" : usercolor);
			this.Kills = kills;
		}

		// Token: 0x04008FF4 RID: 36852
		public string UserName;

		// Token: 0x04008FF5 RID: 36853
		public string UserColor;

		// Token: 0x04008FF6 RID: 36854
		public int Kills;
	}
}
