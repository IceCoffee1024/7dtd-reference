using System;

namespace Twitch
{
	// Token: 0x0200184D RID: 6221
	public class TwitchActionHistoryEntry
	{
		// Token: 0x0600C051 RID: 49233 RVA: 0x0046BACC File Offset: 0x00469CCC
		public TwitchActionHistoryEntry(string username, string usercolor, TwitchAction action, TwitchVote vote, TwitchEventActionEntry eventEntry)
		{
			this.UserName = username;
			this.Action = action;
			this.Vote = vote;
			this.EventEntry = eventEntry;
			this.UserColor = usercolor;
			ValueTuple<int, int, int> valueTuple = GameUtils.WorldTimeToElements(GameManager.Instance.World.worldTime);
			int item = valueTuple.Item1;
			int item2 = valueTuple.Item2;
			int item3 = valueTuple.Item3;
			this.ActionTime = string.Format("{0} {1}, {2:00}:{3:00}", new object[]
			{
				Localization.Get("xuiDay", false, null),
				item,
				item2,
				item3
			});
		}

		// Token: 0x170017F7 RID: 6135
		// (get) Token: 0x0600C052 RID: 49234 RVA: 0x0046BB6C File Offset: 0x00469D6C
		public string Command
		{
			get
			{
				if (this.Action != null)
				{
					return string.Format("{0}({1})", this.Action.Command, this.PointsSpent);
				}
				if (this.Vote != null)
				{
					return this.Vote.VoteDescription;
				}
				if (this.EventEntry != null)
				{
					return this.EventEntry.Event.Description(this.EventEntry);
				}
				return "";
			}
		}

		// Token: 0x170017F8 RID: 6136
		// (get) Token: 0x0600C053 RID: 49235 RVA: 0x0046BBDC File Offset: 0x00469DDC
		public string Title
		{
			get
			{
				if (this.Action != null)
				{
					return this.Action.Title;
				}
				if (this.Vote != null)
				{
					return this.Vote.VoteDescription;
				}
				if (this.EventEntry != null)
				{
					return this.EventEntry.Event.EventTitle;
				}
				return "";
			}
		}

		// Token: 0x170017F9 RID: 6137
		// (get) Token: 0x0600C054 RID: 49236 RVA: 0x0046BC30 File Offset: 0x00469E30
		public string Description
		{
			get
			{
				if (this.Action != null)
				{
					return this.Action.Description;
				}
				if (this.Vote != null)
				{
					return this.Vote.Description;
				}
				if (this.EventEntry != null)
				{
					return this.EventEntry.Event.EventTitle;
				}
				return "";
			}
		}

		// Token: 0x170017FA RID: 6138
		// (get) Token: 0x0600C055 RID: 49237 RVA: 0x0046BC83 File Offset: 0x00469E83
		public string HistoryType
		{
			get
			{
				if (this.Action != null)
				{
					return "action";
				}
				if (this.Vote != null)
				{
					return "vote";
				}
				if (this.EventEntry != null)
				{
					return "event";
				}
				return "";
			}
		}

		// Token: 0x170017FB RID: 6139
		// (get) Token: 0x0600C056 RID: 49238 RVA: 0x0046BCB4 File Offset: 0x00469EB4
		public bool IsRefunded
		{
			get
			{
				return this.EntryState == TwitchActionHistoryEntry.EntryStates.Refunded;
			}
		}

		// Token: 0x0600C057 RID: 49239 RVA: 0x0046BCBF File Offset: 0x00469EBF
		[PublicizedFrom(EAccessModifier.Internal)]
		public bool IsValid()
		{
			return this.UserName != null && ((this.Action != null && this.Action.Command != null) || this.Vote != null || this.EventEntry != null);
		}

		// Token: 0x0600C058 RID: 49240 RVA: 0x0046BCF4 File Offset: 0x00469EF4
		[PublicizedFrom(EAccessModifier.Internal)]
		public void Refund()
		{
			if (this.EntryState != TwitchActionHistoryEntry.EntryStates.Refunded)
			{
				TwitchManager twitchManager = TwitchManager.Current;
				twitchManager.ViewerData.ReimburseAction(this.UserName, this.PointsSpent, this.Action);
				twitchManager.LocalPlayer.PlayOneShot("ui_vending_purchase", false, false, false, null, 1f);
				this.EntryState = TwitchActionHistoryEntry.EntryStates.Refunded;
			}
		}

		// Token: 0x0600C059 RID: 49241 RVA: 0x0046BD4C File Offset: 0x00469F4C
		public void Retry()
		{
			if (!this.HasRetried)
			{
				if (this.Action != null)
				{
					TwitchManager.Current.HandleExtensionMessage(this.UserID, string.Format("{0} {1}", this.Action.Command, this.Target), true, 0, 0);
				}
				else if (this.EventEntry != null)
				{
					this.EventEntry.IsSent = false;
					this.EventEntry.IsRetry = true;
					TwitchManager.Current.EventQueue.Add(this.EventEntry);
				}
				this.HasRetried = true;
			}
		}

		// Token: 0x0600C05A RID: 49242 RVA: 0x0046BDD8 File Offset: 0x00469FD8
		public bool CanRetry()
		{
			if (this.HasRetried)
			{
				return false;
			}
			TwitchManager.CooldownTypes cooldownType = TwitchManager.Current.CooldownType;
			if (this.Action != null)
			{
				if (TwitchManager.Current.VotingManager.VotingIsActive)
				{
					return false;
				}
				if (!this.Action.IgnoreCooldown)
				{
					return cooldownType != TwitchManager.CooldownTypes.BloodMoonDisabled && cooldownType != TwitchManager.CooldownTypes.Time && cooldownType != TwitchManager.CooldownTypes.QuestDisabled && ((cooldownType != TwitchManager.CooldownTypes.MaxReachedWaiting && cooldownType != TwitchManager.CooldownTypes.SafeCooldown) || !this.Action.WaitingBlocked) && !this.HasRetried;
				}
				return !this.HasRetried;
			}
			else
			{
				if (this.EventEntry != null)
				{
					if (!this.EventEntry.Event.CooldownAllowed)
					{
						if (cooldownType == TwitchManager.CooldownTypes.BloodMoonDisabled || cooldownType == TwitchManager.CooldownTypes.Time || cooldownType == TwitchManager.CooldownTypes.QuestDisabled)
						{
							return false;
						}
						if (cooldownType == TwitchManager.CooldownTypes.MaxReachedWaiting)
						{
							return false;
						}
					}
					return (this.EventEntry.Event.StartingCooldownAllowed || cooldownType != TwitchManager.CooldownTypes.Startup) && (this.EventEntry.Event.VoteEventAllowed || !TwitchManager.Current.VotingManager.VotingIsActive) && !this.HasRetried;
				}
				return false;
			}
		}

		// Token: 0x0600C05B RID: 49243 RVA: 0x0046BED2 File Offset: 0x0046A0D2
		public bool CanRefund()
		{
			return this.PointsSpent > 0 && this.EntryState != TwitchActionHistoryEntry.EntryStates.Refunded && this.EntryState != TwitchActionHistoryEntry.EntryStates.Reimbursed && this.Action != null;
		}

		// Token: 0x04009002 RID: 36866
		public string UserName;

		// Token: 0x04009003 RID: 36867
		public string UserColor;

		// Token: 0x04009004 RID: 36868
		public string Target;

		// Token: 0x04009005 RID: 36869
		public int UserID;

		// Token: 0x04009006 RID: 36870
		public TwitchAction Action;

		// Token: 0x04009007 RID: 36871
		public TwitchVote Vote;

		// Token: 0x04009008 RID: 36872
		public TwitchActionEntry ActionEntry;

		// Token: 0x04009009 RID: 36873
		public TwitchEventActionEntry EventEntry;

		// Token: 0x0400900A RID: 36874
		public int PointsSpent;

		// Token: 0x0400900B RID: 36875
		public bool HasRetried;

		// Token: 0x0400900C RID: 36876
		public string ActionTime;

		// Token: 0x0400900D RID: 36877
		public TwitchActionHistoryEntry.EntryStates EntryState;

		// Token: 0x0200184E RID: 6222
		public enum EntryStates
		{
			// Token: 0x0400900F RID: 36879
			Waiting,
			// Token: 0x04009010 RID: 36880
			Completed,
			// Token: 0x04009011 RID: 36881
			Reimbursed,
			// Token: 0x04009012 RID: 36882
			Despawned,
			// Token: 0x04009013 RID: 36883
			Refunded
		}
	}
}
