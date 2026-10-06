using System;

namespace Twitch
{
	// Token: 0x0200187F RID: 6271
	public class TwitchEventActionEntry
	{
		// Token: 0x0600C1D8 RID: 49624 RVA: 0x00478A29 File Offset: 0x00476C29
		public bool HandleEvent(TwitchManager tm)
		{
			if (this.Event.HandleEvent(this.UserName, tm))
			{
				this.IsSent = true;
				return true;
			}
			return false;
		}

		// Token: 0x040091D3 RID: 37331
		public string UserName;

		// Token: 0x040091D4 RID: 37332
		public byte Tier;

		// Token: 0x040091D5 RID: 37333
		public short Count;

		// Token: 0x040091D6 RID: 37334
		public bool IsSent;

		// Token: 0x040091D7 RID: 37335
		public bool IsRetry;

		// Token: 0x040091D8 RID: 37336
		public bool ReadyForRemove;

		// Token: 0x040091D9 RID: 37337
		public TwitchActionHistoryEntry HistoryEntry;

		// Token: 0x040091DA RID: 37338
		public BaseTwitchEventEntry Event;
	}
}
