using System;

namespace Twitch
{
	// Token: 0x02001837 RID: 6199
	public class TwitchEventEntry : BaseTwitchEventEntry
	{
		// Token: 0x0600BF37 RID: 48951 RVA: 0x004696A1 File Offset: 0x004678A1
		public override bool IsValid(int amount = -1, string name = "", TwitchSubEventEntry.SubTierTypes subTier = TwitchSubEventEntry.SubTierTypes.Any)
		{
			return (this.StartAmount == -1 || amount >= this.StartAmount) && (this.EndAmount == -1 || amount <= this.EndAmount);
		}

		// Token: 0x04008FB5 RID: 36789
		public int StartAmount = -1;

		// Token: 0x04008FB6 RID: 36790
		public int EndAmount = -1;
	}
}
