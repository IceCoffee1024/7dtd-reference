using System;

namespace Twitch
{
	// Token: 0x0200182B RID: 6187
	public class BaseTwitchEventEntry
	{
		// Token: 0x0600BF15 RID: 48917 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool IsValid(int amount = -1, string name = "", TwitchSubEventEntry.SubTierTypes subTier = TwitchSubEventEntry.SubTierTypes.Any)
		{
			return false;
		}

		// Token: 0x0600BF16 RID: 48918 RVA: 0x00468E7E File Offset: 0x0046707E
		public virtual string Description(TwitchEventActionEntry entry)
		{
			return string.Format("{0}({1})", this.EventTitle, entry.Count);
		}

		// Token: 0x0600BF17 RID: 48919 RVA: 0x00468E9C File Offset: 0x0046709C
		public virtual void HandleInstant(string username, TwitchManager tm)
		{
			if (this.PimpPotAdd > 0)
			{
				tm.AddToPot(this.PimpPotAdd);
			}
			if (this.BitPotAdd > 0)
			{
				tm.AddToBitPot(this.BitPotAdd);
			}
			if (this.CooldownAdd > 0)
			{
				tm.AddCooldownAmount(this.CooldownAdd);
			}
			if (this.PPAmount > 0 || this.SPAmount > 0)
			{
				if (username == "")
				{
					tm.ViewerData.AddPointsAll(this.PPAmount, this.SPAmount, true);
					return;
				}
				ViewerEntry viewerEntry = tm.ViewerData.GetViewerEntry(username);
				viewerEntry.SpecialPoints += (float)this.SPAmount;
				viewerEntry.StandardPoints += (float)this.PPAmount;
			}
		}

		// Token: 0x0600BF18 RID: 48920 RVA: 0x00468F54 File Offset: 0x00467154
		public virtual bool HandleEvent(string username, TwitchManager tm)
		{
			if (this.EventName == "")
			{
				return true;
			}
			if (!this.SafeAllowed && tm.IsSafe)
			{
				return false;
			}
			if (TwitchManager.BossHordeActive)
			{
				return false;
			}
			TwitchManager.CooldownTypes cooldownType = tm.CooldownType;
			if (cooldownType != TwitchManager.CooldownTypes.None)
			{
				if (cooldownType == TwitchManager.CooldownTypes.Startup)
				{
					if (!this.StartingCooldownAllowed)
					{
						return false;
					}
				}
				else if (!this.CooldownAllowed)
				{
					return false;
				}
			}
			if (!this.VoteEventAllowed && tm.VotingManager.VotingIsActive)
			{
				return false;
			}
			if (GameEventManager.Current.HandleAction(this.EventName, tm.LocalPlayer, tm.LocalPlayer, false, username, "event", tm.AllowCrateSharing, false, "", null))
			{
				GameEventManager.Current.HandleGameEventApproved(this.EventName, tm.LocalPlayer.entityId, username, "event");
				return true;
			}
			return false;
		}

		// Token: 0x04008F77 RID: 36727
		public string EventName = "";

		// Token: 0x04008F78 RID: 36728
		public string EventTitle = "";

		// Token: 0x04008F79 RID: 36729
		public bool SafeAllowed = true;

		// Token: 0x04008F7A RID: 36730
		public bool StartingCooldownAllowed;

		// Token: 0x04008F7B RID: 36731
		public bool CooldownAllowed = true;

		// Token: 0x04008F7C RID: 36732
		public bool VoteEventAllowed = true;

		// Token: 0x04008F7D RID: 36733
		public bool RewardsBitPot;

		// Token: 0x04008F7E RID: 36734
		public int PPAmount;

		// Token: 0x04008F7F RID: 36735
		public int SPAmount;

		// Token: 0x04008F80 RID: 36736
		public int PimpPotAdd;

		// Token: 0x04008F81 RID: 36737
		public int BitPotAdd;

		// Token: 0x04008F82 RID: 36738
		public int CooldownAdd;

		// Token: 0x04008F83 RID: 36739
		public BaseTwitchEventEntry.EventTypes EventType;

		// Token: 0x0200182C RID: 6188
		public enum EventTypes
		{
			// Token: 0x04008F85 RID: 36741
			Bits,
			// Token: 0x04008F86 RID: 36742
			Subs,
			// Token: 0x04008F87 RID: 36743
			GiftSubs,
			// Token: 0x04008F88 RID: 36744
			Raid,
			// Token: 0x04008F89 RID: 36745
			Charity,
			// Token: 0x04008F8A RID: 36746
			ChannelPoints,
			// Token: 0x04008F8B RID: 36747
			HypeTrain,
			// Token: 0x04008F8C RID: 36748
			CreatorGoal
		}
	}
}
