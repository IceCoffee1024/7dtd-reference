using System;

namespace Twitch
{
	// Token: 0x0200183B RID: 6203
	public class TwitchSubEventEntry : TwitchEventEntry
	{
		// Token: 0x0600BF5C RID: 48988 RVA: 0x00469D84 File Offset: 0x00467F84
		public TwitchSubEventEntry()
		{
			this.RewardsBitPot = true;
		}

		// Token: 0x0600BF5D RID: 48989 RVA: 0x00469D93 File Offset: 0x00467F93
		public override bool IsValid(int amount = -1, string name = "", TwitchSubEventEntry.SubTierTypes subTier = TwitchSubEventEntry.SubTierTypes.Any)
		{
			return (this.StartAmount == -1 || amount >= this.StartAmount) && (this.EndAmount == -1 || amount <= this.EndAmount) && (this.SubTier == TwitchSubEventEntry.SubTierTypes.Any || this.SubTier == subTier);
		}

		// Token: 0x0600BF5E RID: 48990 RVA: 0x00469DCE File Offset: 0x00467FCE
		public override string Description(TwitchEventActionEntry entry)
		{
			return this.EventTitle;
		}

		// Token: 0x0600BF5F RID: 48991 RVA: 0x00469DD6 File Offset: 0x00467FD6
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

		// Token: 0x04008FCB RID: 36811
		public TwitchSubEventEntry.SubTierTypes SubTier;

		// Token: 0x0200183C RID: 6204
		public enum SubTierTypes
		{
			// Token: 0x04008FCD RID: 36813
			Any,
			// Token: 0x04008FCE RID: 36814
			Prime,
			// Token: 0x04008FCF RID: 36815
			Tier1,
			// Token: 0x04008FD0 RID: 36816
			Tier2,
			// Token: 0x04008FD1 RID: 36817
			Tier3
		}
	}
}
