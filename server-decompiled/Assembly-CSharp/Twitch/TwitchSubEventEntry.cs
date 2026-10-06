using System;

namespace Twitch
{
	// Token: 0x02001867 RID: 6247
	public class TwitchSubEventEntry : TwitchEventEntry
	{
		// Token: 0x0600C0B4 RID: 49332 RVA: 0x0046E2C0 File Offset: 0x0046C4C0
		public TwitchSubEventEntry()
		{
			this.RewardsBitPot = true;
		}

		// Token: 0x0600C0B5 RID: 49333 RVA: 0x0046E2CF File Offset: 0x0046C4CF
		public override bool IsValid(int amount = -1, string name = "", TwitchSubEventEntry.SubTierTypes subTier = TwitchSubEventEntry.SubTierTypes.Any)
		{
			return (this.StartAmount == -1 || amount >= this.StartAmount) && (this.EndAmount == -1 || amount <= this.EndAmount) && (this.SubTier == TwitchSubEventEntry.SubTierTypes.Any || this.SubTier == subTier);
		}

		// Token: 0x0600C0B6 RID: 49334 RVA: 0x0046E30A File Offset: 0x0046C50A
		public override string Description(TwitchEventActionEntry entry)
		{
			return this.EventTitle;
		}

		// Token: 0x0600C0B7 RID: 49335 RVA: 0x0046E312 File Offset: 0x0046C512
		public static TwitchSubEventEntry.SubTierTypes GetSubTier(string subPlan)
		{
			if (subPlan == "1000")
			{
				return TwitchSubEventEntry.SubTierTypes.Tier1;
			}
			if (subPlan == "2000")
			{
				return TwitchSubEventEntry.SubTierTypes.Tier2;
			}
			if (!(subPlan == "3000"))
			{
				return TwitchSubEventEntry.SubTierTypes.Prime;
			}
			return TwitchSubEventEntry.SubTierTypes.Tier3;
		}

		// Token: 0x04009091 RID: 37009
		public TwitchSubEventEntry.SubTierTypes SubTier;

		// Token: 0x02001868 RID: 6248
		public enum SubTierTypes
		{
			// Token: 0x04009093 RID: 37011
			Any,
			// Token: 0x04009094 RID: 37012
			Prime,
			// Token: 0x04009095 RID: 37013
			Tier1,
			// Token: 0x04009096 RID: 37014
			Tier2,
			// Token: 0x04009097 RID: 37015
			Tier3
		}
	}
}
