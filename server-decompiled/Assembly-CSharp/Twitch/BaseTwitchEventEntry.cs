using System;

namespace Twitch
{
	// Token: 0x02001805 RID: 6149
	public class BaseTwitchEventEntry
	{
		// Token: 0x0600BD90 RID: 48528 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool IsValid(int amount = -1, string name = "", TwitchSubEventEntry.SubTierTypes subTier = TwitchSubEventEntry.SubTierTypes.Any)
		{
			return false;
		}

		// Token: 0x0600BD91 RID: 48529 RVA: 0x004630E2 File Offset: 0x004612E2
		public virtual string Description(TwitchEventActionEntry entry)
		{
			return string.Format("{0}({1})", this.EventTitle, entry.Count);
		}

		// Token: 0x0600BD92 RID: 48530 RVA: 0x00463100 File Offset: 0x00461300
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

		// Token: 0x0600BD93 RID: 48531 RVA: 0x004631B8 File Offset: 0x004613B8
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

		// Token: 0x04008E5B RID: 36443
		public string EventName = "";

		// Token: 0x04008E5C RID: 36444
		public string EventTitle = "";

		// Token: 0x04008E5D RID: 36445
		public bool SafeAllowed = true;

		// Token: 0x04008E5E RID: 36446
		public bool StartingCooldownAllowed;

		// Token: 0x04008E5F RID: 36447
		public bool CooldownAllowed = true;

		// Token: 0x04008E60 RID: 36448
		public bool VoteEventAllowed = true;

		// Token: 0x04008E61 RID: 36449
		public bool RewardsBitPot;

		// Token: 0x04008E62 RID: 36450
		public int PPAmount;

		// Token: 0x04008E63 RID: 36451
		public int SPAmount;

		// Token: 0x04008E64 RID: 36452
		public int PimpPotAdd;

		// Token: 0x04008E65 RID: 36453
		public int BitPotAdd;

		// Token: 0x04008E66 RID: 36454
		public int CooldownAdd;

		// Token: 0x04008E67 RID: 36455
		public BaseTwitchEventEntry.EventTypes EventType;

		// Token: 0x02001806 RID: 6150
		public enum EventTypes
		{
			// Token: 0x04008E69 RID: 36457
			Bits,
			// Token: 0x04008E6A RID: 36458
			Subs,
			// Token: 0x04008E6B RID: 36459
			GiftSubs,
			// Token: 0x04008E6C RID: 36460
			Raid,
			// Token: 0x04008E6D RID: 36461
			Charity,
			// Token: 0x04008E6E RID: 36462
			ChannelPoints,
			// Token: 0x04008E6F RID: 36463
			HypeTrain,
			// Token: 0x04008E70 RID: 36464
			CreatorGoal
		}
	}
}
