using System;

namespace Twitch
{
	// Token: 0x02001894 RID: 6292
	public class TwitchEventEntry : BaseTwitchEventEntry
	{
		// Token: 0x0600C1A3 RID: 49571 RVA: 0x00471589 File Offset: 0x0046F789
		public override bool IsValid(int amount = -1, string name = "", TwitchSubEventEntry.SubTierTypes subTier = TwitchSubEventEntry.SubTierTypes.Any)
		{
			return (this.StartAmount == -1 || amount >= this.StartAmount) && (this.EndAmount == -1 || amount <= this.EndAmount);
		}

		// Token: 0x04009164 RID: 37220
		public int StartAmount = -1;

		// Token: 0x04009165 RID: 37221
		public int EndAmount = -1;
	}
}
