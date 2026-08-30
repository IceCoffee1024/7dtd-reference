using System;

namespace Twitch
{
	// Token: 0x020017CA RID: 6090
	public class TwitchActionHistoryEntry
	{
		// Token: 0x0600BC60 RID: 48224 RVA: 0x0045DE48 File Offset: 0x0045C048
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

		// Token: 0x1700169B RID: 5787
		// (get) Token: 0x0600BC61 RID: 48225 RVA: 0x0045DEE8 File Offset: 0x0045C0E8
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

		// Token: 0x1700169C RID: 5788
		// (get) Token: 0x0600BC62 RID: 48226 RVA: 0x0045DF58 File Offset: 0x0045C158
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

		// Token: 0x1700169D RID: 5789
		// (get) Token: 0x0600BC63 RID: 48227 RVA: 0x0045DFAC File Offset: 0x0045C1AC
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

		// Token: 0x1700169E RID: 5790
		// (get) Token: 0x0600BC64 RID: 48228 RVA: 0x0045DFFF File Offset: 0x0045C1FF
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

		// Token: 0x1700169F RID: 5791
		// (get) Token: 0x0600BC65 RID: 48229 RVA: 0x0045E030 File Offset: 0x0045C230
		public bool IsRefunded
		{
			get
			{
				return this.EntryState == TwitchActionHistoryEntry.EntryStates.Refunded;
			}
		}

		// Token: 0x0600BC66 RID: 48230 RVA: 0x0045E03B File Offset: 0x0045C23B
		[PublicizedFrom(EAccessModifier.Internal)]
		public bool IsValid()
		{
			return this.UserName != null && ((this.Action != null && this.Action.Command != null) || this.Vote != null || this.EventEntry != null);
		}

		// Token: 0x0600BC67 RID: 48231 RVA: 0x0045E070 File Offset: 0x0045C270
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

		// Token: 0x0600BC68 RID: 48232 RVA: 0x0045E0C8 File Offset: 0x0045C2C8
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

		// Token: 0x0600BC69 RID: 48233 RVA: 0x0045E154 File Offset: 0x0045C354
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

		// Token: 0x0600BC6A RID: 48234 RVA: 0x0045E24E File Offset: 0x0045C44E
		public bool CanRefund()
		{
			return this.PointsSpent > 0 && this.EntryState != TwitchActionHistoryEntry.EntryStates.Refunded && this.EntryState != TwitchActionHistoryEntry.EntryStates.Reimbursed && this.Action != null;
		}

		// Token: 0x04008D37 RID: 36151
		public string UserName;

		// Token: 0x04008D38 RID: 36152
		public string UserColor;

		// Token: 0x04008D39 RID: 36153
		public string Target;

		// Token: 0x04008D3A RID: 36154
		public int UserID;

		// Token: 0x04008D3B RID: 36155
		public TwitchAction Action;

		// Token: 0x04008D3C RID: 36156
		public TwitchVote Vote;

		// Token: 0x04008D3D RID: 36157
		public TwitchActionEntry ActionEntry;

		// Token: 0x04008D3E RID: 36158
		public TwitchEventActionEntry EventEntry;

		// Token: 0x04008D3F RID: 36159
		public int PointsSpent;

		// Token: 0x04008D40 RID: 36160
		public bool HasRetried;

		// Token: 0x04008D41 RID: 36161
		public string ActionTime;

		// Token: 0x04008D42 RID: 36162
		public TwitchActionHistoryEntry.EntryStates EntryState;

		// Token: 0x020017CB RID: 6091
		public enum EntryStates
		{
			// Token: 0x04008D44 RID: 36164
			Waiting,
			// Token: 0x04008D45 RID: 36165
			Completed,
			// Token: 0x04008D46 RID: 36166
			Reimbursed,
			// Token: 0x04008D47 RID: 36167
			Despawned,
			// Token: 0x04008D48 RID: 36168
			Refunded
		}
	}
}
