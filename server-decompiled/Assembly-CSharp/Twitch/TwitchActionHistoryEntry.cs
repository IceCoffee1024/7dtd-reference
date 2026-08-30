using System;

namespace Twitch
{
	// Token: 0x020017F0 RID: 6128
	public class TwitchActionHistoryEntry
	{
		// Token: 0x0600BDE5 RID: 48613 RVA: 0x00463BE4 File Offset: 0x00461DE4
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

		// Token: 0x1700170D RID: 5901
		// (get) Token: 0x0600BDE6 RID: 48614 RVA: 0x00463C84 File Offset: 0x00461E84
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

		// Token: 0x1700170E RID: 5902
		// (get) Token: 0x0600BDE7 RID: 48615 RVA: 0x00463CF4 File Offset: 0x00461EF4
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

		// Token: 0x1700170F RID: 5903
		// (get) Token: 0x0600BDE8 RID: 48616 RVA: 0x00463D48 File Offset: 0x00461F48
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

		// Token: 0x17001710 RID: 5904
		// (get) Token: 0x0600BDE9 RID: 48617 RVA: 0x00463D9B File Offset: 0x00461F9B
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

		// Token: 0x17001711 RID: 5905
		// (get) Token: 0x0600BDEA RID: 48618 RVA: 0x00463DCC File Offset: 0x00461FCC
		public bool IsRefunded
		{
			get
			{
				return this.EntryState == TwitchActionHistoryEntry.EntryStates.Refunded;
			}
		}

		// Token: 0x0600BDEB RID: 48619 RVA: 0x00463DD7 File Offset: 0x00461FD7
		[PublicizedFrom(EAccessModifier.Internal)]
		public bool IsValid()
		{
			return this.UserName != null && ((this.Action != null && this.Action.Command != null) || this.Vote != null || this.EventEntry != null);
		}

		// Token: 0x0600BDEC RID: 48620 RVA: 0x00463E0C File Offset: 0x0046200C
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

		// Token: 0x0600BDED RID: 48621 RVA: 0x00463E64 File Offset: 0x00462064
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

		// Token: 0x0600BDEE RID: 48622 RVA: 0x00463EF0 File Offset: 0x004620F0
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

		// Token: 0x0600BDEF RID: 48623 RVA: 0x00463FEA File Offset: 0x004621EA
		public bool CanRefund()
		{
			return this.PointsSpent > 0 && this.EntryState != TwitchActionHistoryEntry.EntryStates.Refunded && this.EntryState != TwitchActionHistoryEntry.EntryStates.Reimbursed && this.Action != null;
		}

		// Token: 0x04008E53 RID: 36435
		public string UserName;

		// Token: 0x04008E54 RID: 36436
		public string UserColor;

		// Token: 0x04008E55 RID: 36437
		public string Target;

		// Token: 0x04008E56 RID: 36438
		public int UserID;

		// Token: 0x04008E57 RID: 36439
		public TwitchAction Action;

		// Token: 0x04008E58 RID: 36440
		public TwitchVote Vote;

		// Token: 0x04008E59 RID: 36441
		public TwitchActionEntry ActionEntry;

		// Token: 0x04008E5A RID: 36442
		public TwitchEventActionEntry EventEntry;

		// Token: 0x04008E5B RID: 36443
		public int PointsSpent;

		// Token: 0x04008E5C RID: 36444
		public bool HasRetried;

		// Token: 0x04008E5D RID: 36445
		public string ActionTime;

		// Token: 0x04008E5E RID: 36446
		public TwitchActionHistoryEntry.EntryStates EntryState;

		// Token: 0x020017F1 RID: 6129
		public enum EntryStates
		{
			// Token: 0x04008E60 RID: 36448
			Waiting,
			// Token: 0x04008E61 RID: 36449
			Completed,
			// Token: 0x04008E62 RID: 36450
			Reimbursed,
			// Token: 0x04008E63 RID: 36451
			Despawned,
			// Token: 0x04008E64 RID: 36452
			Refunded
		}
	}
}
