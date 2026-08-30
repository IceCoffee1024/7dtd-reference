using System;

namespace Twitch
{
	// Token: 0x02001853 RID: 6227
	public class TwitchEventActionEntry
	{
		// Token: 0x0600C080 RID: 49280 RVA: 0x004744ED File Offset: 0x004726ED
		public bool HandleEvent(TwitchManager tm)
		{
			if (this.Event.HandleEvent(this.UserName, tm))
			{
				this.IsSent = true;
				return true;
			}
			return false;
		}

		// Token: 0x0400910D RID: 37133
		public string UserName;

		// Token: 0x0400910E RID: 37134
		public byte Tier;

		// Token: 0x0400910F RID: 37135
		public short Count;

		// Token: 0x04009110 RID: 37136
		public bool IsSent;

		// Token: 0x04009111 RID: 37137
		public bool IsRetry;

		// Token: 0x04009112 RID: 37138
		public bool ReadyForRemove;

		// Token: 0x04009113 RID: 37139
		public TwitchActionHistoryEntry HistoryEntry;

		// Token: 0x04009114 RID: 37140
		public BaseTwitchEventEntry Event;
	}
}
