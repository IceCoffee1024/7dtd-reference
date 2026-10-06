using System;

namespace Twitch
{
	// Token: 0x0200181C RID: 6172
	public class TwitchActionHistoryEntry
	{
		// Token: 0x0600BF3D RID: 48957 RVA: 0x00468120 File Offset: 0x00466320
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

		// Token: 0x170017AF RID: 6063
		// (get) Token: 0x0600BF3E RID: 48958 RVA: 0x004681C0 File Offset: 0x004663C0
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

		// Token: 0x170017B0 RID: 6064
		// (get) Token: 0x0600BF3F RID: 48959 RVA: 0x00468230 File Offset: 0x00466430
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

		// Token: 0x170017B1 RID: 6065
		// (get) Token: 0x0600BF40 RID: 48960 RVA: 0x00468284 File Offset: 0x00466484
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

		// Token: 0x170017B2 RID: 6066
		// (get) Token: 0x0600BF41 RID: 48961 RVA: 0x004682D7 File Offset: 0x004664D7
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

		// Token: 0x170017B3 RID: 6067
		// (get) Token: 0x0600BF42 RID: 48962 RVA: 0x00468308 File Offset: 0x00466508
		public bool IsRefunded
		{
			get
			{
				return this.EntryState == TwitchActionHistoryEntry.EntryStates.Refunded;
			}
		}

		// Token: 0x0600BF43 RID: 48963 RVA: 0x00468313 File Offset: 0x00466513
		[PublicizedFrom(EAccessModifier.Internal)]
		public bool IsValid()
		{
			return this.UserName != null && ((this.Action != null && this.Action.Command != null) || this.Vote != null || this.EventEntry != null);
		}

		// Token: 0x0600BF44 RID: 48964 RVA: 0x00468348 File Offset: 0x00466548
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

		// Token: 0x0600BF45 RID: 48965 RVA: 0x004683A0 File Offset: 0x004665A0
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

		// Token: 0x0600BF46 RID: 48966 RVA: 0x0046842C File Offset: 0x0046662C
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

		// Token: 0x0600BF47 RID: 48967 RVA: 0x00468526 File Offset: 0x00466726
		public bool CanRefund()
		{
			return this.PointsSpent > 0 && this.EntryState != TwitchActionHistoryEntry.EntryStates.Refunded && this.EntryState != TwitchActionHistoryEntry.EntryStates.Reimbursed && this.Action != null;
		}

		// Token: 0x04008F19 RID: 36633
		public string UserName;

		// Token: 0x04008F1A RID: 36634
		public string UserColor;

		// Token: 0x04008F1B RID: 36635
		public string Target;

		// Token: 0x04008F1C RID: 36636
		public int UserID;

		// Token: 0x04008F1D RID: 36637
		public TwitchAction Action;

		// Token: 0x04008F1E RID: 36638
		public TwitchVote Vote;

		// Token: 0x04008F1F RID: 36639
		public TwitchActionEntry ActionEntry;

		// Token: 0x04008F20 RID: 36640
		public TwitchEventActionEntry EventEntry;

		// Token: 0x04008F21 RID: 36641
		public int PointsSpent;

		// Token: 0x04008F22 RID: 36642
		public bool HasRetried;

		// Token: 0x04008F23 RID: 36643
		public string ActionTime;

		// Token: 0x04008F24 RID: 36644
		public TwitchActionHistoryEntry.EntryStates EntryState;

		// Token: 0x0200181D RID: 6173
		public enum EntryStates
		{
			// Token: 0x04008F26 RID: 36646
			Waiting,
			// Token: 0x04008F27 RID: 36647
			Completed,
			// Token: 0x04008F28 RID: 36648
			Reimbursed,
			// Token: 0x04008F29 RID: 36649
			Despawned,
			// Token: 0x04008F2A RID: 36650
			Refunded
		}
	}
}
