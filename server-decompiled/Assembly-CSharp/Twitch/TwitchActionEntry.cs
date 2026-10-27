using System;

namespace Twitch
{
	// Token: 0x0200185A RID: 6234
	public class TwitchActionEntry
	{
		// Token: 0x17001801 RID: 6145
		// (get) Token: 0x0600C08D RID: 49293 RVA: 0x0046D671 File Offset: 0x0046B871
		public int ActionCost
		{
			get
			{
				return this.Action.CurrentCost;
			}
		}

		// Token: 0x0600C08E RID: 49294 RVA: 0x0046D680 File Offset: 0x0046B880
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

		// Token: 0x040090AE RID: 37038
		public string UserName;

		// Token: 0x040090AF RID: 37039
		public EntityPlayer Target;

		// Token: 0x040090B0 RID: 37040
		public bool ReadyForRemove;

		// Token: 0x040090B1 RID: 37041
		public TwitchVoteEntry VoteEntry;

		// Token: 0x040090B2 RID: 37042
		public TwitchAction Action;

		// Token: 0x040090B3 RID: 37043
		public bool IsSent;

		// Token: 0x040090B4 RID: 37044
		public bool ChannelNotify = true;

		// Token: 0x040090B5 RID: 37045
		public bool IsBitAction;

		// Token: 0x040090B6 RID: 37046
		public bool IsReRun;

		// Token: 0x040090B7 RID: 37047
		public bool IsRespawn;

		// Token: 0x040090B8 RID: 37048
		public int SpecialPointsUsed;

		// Token: 0x040090B9 RID: 37049
		public int StandardPointsUsed;

		// Token: 0x040090BA RID: 37050
		public int BitsUsed;

		// Token: 0x040090BB RID: 37051
		public int CreditsUsed;

		// Token: 0x040090BC RID: 37052
		public TwitchActionHistoryEntry HistoryEntry;
	}
}
