using System;

namespace Twitch
{
	// Token: 0x020018B0 RID: 6320
	public class TwitchEventActionEntry
	{
		// Token: 0x0600C2EC RID: 49900 RVA: 0x0047C3D5 File Offset: 0x0047A5D5
		public bool HandleEvent(TwitchManager tm)
		{
			if (this.Event.HandleEvent(this.UserName, tm))
			{
				this.IsSent = true;
				return true;
			}
			return false;
		}

		// Token: 0x040092BC RID: 37564
		public string UserName;

		// Token: 0x040092BD RID: 37565
		public byte Tier;

		// Token: 0x040092BE RID: 37566
		public short Count;

		// Token: 0x040092BF RID: 37567
		public bool IsSent;

		// Token: 0x040092C0 RID: 37568
		public bool IsRetry;

		// Token: 0x040092C1 RID: 37569
		public bool ReadyForRemove;

		// Token: 0x040092C2 RID: 37570
		public TwitchActionHistoryEntry HistoryEntry;

		// Token: 0x040092C3 RID: 37571
		public BaseTwitchEventEntry Event;
	}
}
