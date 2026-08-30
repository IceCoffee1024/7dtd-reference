using System;

namespace Twitch
{
	// Token: 0x02001815 RID: 6165
	public class TwitchSubEventEntry : TwitchEventEntry
	{
		// Token: 0x0600BDD7 RID: 48599 RVA: 0x00463FE8 File Offset: 0x004621E8
		public TwitchSubEventEntry()
		{
			this.RewardsBitPot = true;
		}

		// Token: 0x0600BDD8 RID: 48600 RVA: 0x00463FF7 File Offset: 0x004621F7
		public override bool IsValid(int amount = -1, string name = "", TwitchSubEventEntry.SubTierTypes subTier = TwitchSubEventEntry.SubTierTypes.Any)
		{
			return (this.StartAmount == -1 || amount >= this.StartAmount) && (this.EndAmount == -1 || amount <= this.EndAmount) && (this.SubTier == TwitchSubEventEntry.SubTierTypes.Any || this.SubTier == subTier);
		}

		// Token: 0x0600BDD9 RID: 48601 RVA: 0x00464032 File Offset: 0x00462232
		public override string Description(TwitchEventActionEntry entry)
		{
			return this.EventTitle;
		}

		// Token: 0x0600BDDA RID: 48602 RVA: 0x0046403A File Offset: 0x0046223A
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

		// Token: 0x04008EAF RID: 36527
		public TwitchSubEventEntry.SubTierTypes SubTier;

		// Token: 0x02001816 RID: 6166
		public enum SubTierTypes
		{
			// Token: 0x04008EB1 RID: 36529
			Any,
			// Token: 0x04008EB2 RID: 36530
			Prime,
			// Token: 0x04008EB3 RID: 36531
			Tier1,
			// Token: 0x04008EB4 RID: 36532
			Tier2,
			// Token: 0x04008EB5 RID: 36533
			Tier3
		}
	}
}
