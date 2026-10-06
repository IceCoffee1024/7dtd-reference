using System;

namespace Twitch
{
	// Token: 0x02001857 RID: 6231
	public class BaseTwitchEventEntry
	{
		// Token: 0x0600C06D RID: 49261 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool IsValid(int amount = -1, string name = "", TwitchSubEventEntry.SubTierTypes subTier = TwitchSubEventEntry.SubTierTypes.Any)
		{
			return false;
		}

		// Token: 0x0600C06E RID: 49262 RVA: 0x0046D3BA File Offset: 0x0046B5BA
		public virtual string Description(TwitchEventActionEntry entry)
		{
			return string.Format("{0}({1})", this.EventTitle, entry.Count);
		}

		// Token: 0x0600C06F RID: 49263 RVA: 0x0046D3D8 File Offset: 0x0046B5D8
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

		// Token: 0x0600C070 RID: 49264 RVA: 0x0046D490 File Offset: 0x0046B690
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

		// Token: 0x0400903D RID: 36925
		public string EventName = "";

		// Token: 0x0400903E RID: 36926
		public string EventTitle = "";

		// Token: 0x0400903F RID: 36927
		public bool SafeAllowed = true;

		// Token: 0x04009040 RID: 36928
		public bool StartingCooldownAllowed;

		// Token: 0x04009041 RID: 36929
		public bool CooldownAllowed = true;

		// Token: 0x04009042 RID: 36930
		public bool VoteEventAllowed = true;

		// Token: 0x04009043 RID: 36931
		public bool RewardsBitPot;

		// Token: 0x04009044 RID: 36932
		public int PPAmount;

		// Token: 0x04009045 RID: 36933
		public int SPAmount;

		// Token: 0x04009046 RID: 36934
		public int PimpPotAdd;

		// Token: 0x04009047 RID: 36935
		public int BitPotAdd;

		// Token: 0x04009048 RID: 36936
		public int CooldownAdd;

		// Token: 0x04009049 RID: 36937
		public BaseTwitchEventEntry.EventTypes EventType;

		// Token: 0x02001858 RID: 6232
		public enum EventTypes
		{
			// Token: 0x0400904B RID: 36939
			Bits,
			// Token: 0x0400904C RID: 36940
			Subs,
			// Token: 0x0400904D RID: 36941
			GiftSubs,
			// Token: 0x0400904E RID: 36942
			Raid,
			// Token: 0x0400904F RID: 36943
			Charity,
			// Token: 0x04009050 RID: 36944
			ChannelPoints,
			// Token: 0x04009051 RID: 36945
			HypeTrain,
			// Token: 0x04009052 RID: 36946
			CreatorGoal
		}
	}
}
