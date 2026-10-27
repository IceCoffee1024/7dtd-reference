using System;

namespace Twitch
{
	// Token: 0x02001888 RID: 6280
	public class BaseTwitchEventEntry
	{
		// Token: 0x0600C181 RID: 49537 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool IsValid(int amount = -1, string name = "", TwitchSubEventEntry.SubTierTypes subTier = TwitchSubEventEntry.SubTierTypes.Any)
		{
			return false;
		}

		// Token: 0x0600C182 RID: 49538 RVA: 0x00470D66 File Offset: 0x0046EF66
		public virtual string Description(TwitchEventActionEntry entry)
		{
			return string.Format("{0}({1})", this.EventTitle, entry.Count);
		}

		// Token: 0x0600C183 RID: 49539 RVA: 0x00470D84 File Offset: 0x0046EF84
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

		// Token: 0x0600C184 RID: 49540 RVA: 0x00470E3C File Offset: 0x0046F03C
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

		// Token: 0x04009126 RID: 37158
		public string EventName = "";

		// Token: 0x04009127 RID: 37159
		public string EventTitle = "";

		// Token: 0x04009128 RID: 37160
		public bool SafeAllowed = true;

		// Token: 0x04009129 RID: 37161
		public bool StartingCooldownAllowed;

		// Token: 0x0400912A RID: 37162
		public bool CooldownAllowed = true;

		// Token: 0x0400912B RID: 37163
		public bool VoteEventAllowed = true;

		// Token: 0x0400912C RID: 37164
		public bool RewardsBitPot;

		// Token: 0x0400912D RID: 37165
		public int PPAmount;

		// Token: 0x0400912E RID: 37166
		public int SPAmount;

		// Token: 0x0400912F RID: 37167
		public int PimpPotAdd;

		// Token: 0x04009130 RID: 37168
		public int BitPotAdd;

		// Token: 0x04009131 RID: 37169
		public int CooldownAdd;

		// Token: 0x04009132 RID: 37170
		public BaseTwitchEventEntry.EventTypes EventType;

		// Token: 0x02001889 RID: 6281
		public enum EventTypes
		{
			// Token: 0x04009134 RID: 37172
			Bits,
			// Token: 0x04009135 RID: 37173
			Subs,
			// Token: 0x04009136 RID: 37174
			GiftSubs,
			// Token: 0x04009137 RID: 37175
			Raid,
			// Token: 0x04009138 RID: 37176
			Charity,
			// Token: 0x04009139 RID: 37177
			ChannelPoints,
			// Token: 0x0400913A RID: 37178
			HypeTrain,
			// Token: 0x0400913B RID: 37179
			CreatorGoal
		}
	}
}
