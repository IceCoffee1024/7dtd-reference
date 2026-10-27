using System;

namespace Twitch
{
	// Token: 0x02001898 RID: 6296
	public class TwitchSubEventEntry : TwitchEventEntry
	{
		// Token: 0x0600C1C8 RID: 49608 RVA: 0x00471C6C File Offset: 0x0046FE6C
		public TwitchSubEventEntry()
		{
			this.RewardsBitPot = true;
		}

		// Token: 0x0600C1C9 RID: 49609 RVA: 0x00471C7B File Offset: 0x0046FE7B
		public override bool IsValid(int amount = -1, string name = "", TwitchSubEventEntry.SubTierTypes subTier = TwitchSubEventEntry.SubTierTypes.Any)
		{
			return (this.StartAmount == -1 || amount >= this.StartAmount) && (this.EndAmount == -1 || amount <= this.EndAmount) && (this.SubTier == TwitchSubEventEntry.SubTierTypes.Any || this.SubTier == subTier);
		}

		// Token: 0x0600C1CA RID: 49610 RVA: 0x00471CB6 File Offset: 0x0046FEB6
		public override string Description(TwitchEventActionEntry entry)
		{
			return this.EventTitle;
		}

		// Token: 0x0600C1CB RID: 49611 RVA: 0x00471CBE File Offset: 0x0046FEBE
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

		// Token: 0x0400917A RID: 37242
		public TwitchSubEventEntry.SubTierTypes SubTier;

		// Token: 0x02001899 RID: 6297
		public enum SubTierTypes
		{
			// Token: 0x0400917C RID: 37244
			Any,
			// Token: 0x0400917D RID: 37245
			Prime,
			// Token: 0x0400917E RID: 37246
			Tier1,
			// Token: 0x0400917F RID: 37247
			Tier2,
			// Token: 0x04009180 RID: 37248
			Tier3
		}
	}
}
