using System;

namespace Twitch
{
	// Token: 0x020017FD RID: 6141
	public class TwitchActionEntry
	{
		// Token: 0x17001717 RID: 5911
		// (get) Token: 0x0600BE21 RID: 48673 RVA: 0x00465789 File Offset: 0x00463989
		public int ActionCost
		{
			get
			{
				return this.Action.CurrentCost;
			}
		}

		// Token: 0x0600BE22 RID: 48674 RVA: 0x00465798 File Offset: 0x00463998
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

		// Token: 0x04008EFF RID: 36607
		public string UserName;

		// Token: 0x04008F00 RID: 36608
		public EntityPlayer Target;

		// Token: 0x04008F01 RID: 36609
		public bool ReadyForRemove;

		// Token: 0x04008F02 RID: 36610
		public TwitchVoteEntry VoteEntry;

		// Token: 0x04008F03 RID: 36611
		public TwitchAction Action;

		// Token: 0x04008F04 RID: 36612
		public bool IsSent;

		// Token: 0x04008F05 RID: 36613
		public bool ChannelNotify = true;

		// Token: 0x04008F06 RID: 36614
		public bool IsBitAction;

		// Token: 0x04008F07 RID: 36615
		public bool IsReRun;

		// Token: 0x04008F08 RID: 36616
		public bool IsRespawn;

		// Token: 0x04008F09 RID: 36617
		public int SpecialPointsUsed;

		// Token: 0x04008F0A RID: 36618
		public int StandardPointsUsed;

		// Token: 0x04008F0B RID: 36619
		public int BitsUsed;

		// Token: 0x04008F0C RID: 36620
		public int CreditsUsed;

		// Token: 0x04008F0D RID: 36621
		public TwitchActionHistoryEntry HistoryEntry;
	}
}
