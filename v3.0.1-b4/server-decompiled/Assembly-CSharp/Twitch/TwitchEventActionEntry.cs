using System;

namespace Twitch
{
	// Token: 0x0200182D RID: 6189
	public class TwitchEventActionEntry
	{
		// Token: 0x0600BEFB RID: 48891 RVA: 0x0046E751 File Offset: 0x0046C951
		public bool HandleEvent(TwitchManager tm)
		{
			if (this.Event.HandleEvent(this.UserName, tm))
			{
				this.IsSent = true;
				return true;
			}
			return false;
		}

		// Token: 0x04008FF1 RID: 36849
		public string UserName;

		// Token: 0x04008FF2 RID: 36850
		public byte Tier;

		// Token: 0x04008FF3 RID: 36851
		public short Count;

		// Token: 0x04008FF4 RID: 36852
		public bool IsSent;

		// Token: 0x04008FF5 RID: 36853
		public bool IsRetry;

		// Token: 0x04008FF6 RID: 36854
		public bool ReadyForRemove;

		// Token: 0x04008FF7 RID: 36855
		public TwitchActionHistoryEntry HistoryEntry;

		// Token: 0x04008FF8 RID: 36856
		public BaseTwitchEventEntry Event;
	}
}
