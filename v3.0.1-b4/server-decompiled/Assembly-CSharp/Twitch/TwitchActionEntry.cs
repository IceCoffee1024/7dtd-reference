using System;

namespace Twitch
{
	// Token: 0x020017D7 RID: 6103
	public class TwitchActionEntry
	{
		// Token: 0x170016A5 RID: 5797
		// (get) Token: 0x0600BC9C RID: 48284 RVA: 0x0045F9ED File Offset: 0x0045DBED
		public int ActionCost
		{
			get
			{
				return this.Action.CurrentCost;
			}
		}

		// Token: 0x0600BC9D RID: 48285 RVA: 0x0045F9FC File Offset: 0x0045DBFC
		public TwitchActionHistoryEntry SetupHistoryEntry(ViewerEntry viewerEntry)
		{
			string text = ((this.Target != null) ? this.Target.EntityName : "");
			this.HistoryEntry = new TwitchActionHistoryEntry(this.UserName, viewerEntry.UserColor, this.Action, null, null)
			{
				UserID = viewerEntry.UserID,
				PointsSpent = this.Action.CurrentCost,
				Target = text
			};
			this.HistoryEntry.ActionEntry = this;
			return this.HistoryEntry;
		}

		// Token: 0x04008DE3 RID: 36323
		public string UserName;

		// Token: 0x04008DE4 RID: 36324
		public EntityPlayer Target;

		// Token: 0x04008DE5 RID: 36325
		public bool ReadyForRemove;

		// Token: 0x04008DE6 RID: 36326
		public TwitchVoteEntry VoteEntry;

		// Token: 0x04008DE7 RID: 36327
		public TwitchAction Action;

		// Token: 0x04008DE8 RID: 36328
		public bool IsSent;

		// Token: 0x04008DE9 RID: 36329
		public bool ChannelNotify = true;

		// Token: 0x04008DEA RID: 36330
		public bool IsBitAction;

		// Token: 0x04008DEB RID: 36331
		public bool IsReRun;

		// Token: 0x04008DEC RID: 36332
		public bool IsRespawn;

		// Token: 0x04008DED RID: 36333
		public int SpecialPointsUsed;

		// Token: 0x04008DEE RID: 36334
		public int StandardPointsUsed;

		// Token: 0x04008DEF RID: 36335
		public int BitsUsed;

		// Token: 0x04008DF0 RID: 36336
		public int CreditsUsed;

		// Token: 0x04008DF1 RID: 36337
		public TwitchActionHistoryEntry HistoryEntry;
	}
}
