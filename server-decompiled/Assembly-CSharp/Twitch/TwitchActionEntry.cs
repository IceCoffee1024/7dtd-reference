using System;

namespace Twitch
{
	// Token: 0x02001829 RID: 6185
	public class TwitchActionEntry
	{
		// Token: 0x170017B9 RID: 6073
		// (get) Token: 0x0600BF79 RID: 49017 RVA: 0x00469CC5 File Offset: 0x00467EC5
		public int ActionCost
		{
			get
			{
				return this.Action.CurrentCost;
			}
		}

		// Token: 0x0600BF7A RID: 49018 RVA: 0x00469CD4 File Offset: 0x00467ED4
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

		// Token: 0x04008FC5 RID: 36805
		public string UserName;

		// Token: 0x04008FC6 RID: 36806
		public EntityPlayer Target;

		// Token: 0x04008FC7 RID: 36807
		public bool ReadyForRemove;

		// Token: 0x04008FC8 RID: 36808
		public TwitchVoteEntry VoteEntry;

		// Token: 0x04008FC9 RID: 36809
		public TwitchAction Action;

		// Token: 0x04008FCA RID: 36810
		public bool IsSent;

		// Token: 0x04008FCB RID: 36811
		public bool ChannelNotify = true;

		// Token: 0x04008FCC RID: 36812
		public bool IsBitAction;

		// Token: 0x04008FCD RID: 36813
		public bool IsReRun;

		// Token: 0x04008FCE RID: 36814
		public bool IsRespawn;

		// Token: 0x04008FCF RID: 36815
		public int SpecialPointsUsed;

		// Token: 0x04008FD0 RID: 36816
		public int StandardPointsUsed;

		// Token: 0x04008FD1 RID: 36817
		public int BitsUsed;

		// Token: 0x04008FD2 RID: 36818
		public int CreditsUsed;

		// Token: 0x04008FD3 RID: 36819
		public TwitchActionHistoryEntry HistoryEntry;
	}
}
