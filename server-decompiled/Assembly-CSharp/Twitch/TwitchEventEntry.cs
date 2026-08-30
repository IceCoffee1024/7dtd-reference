using System;

namespace Twitch
{
	// Token: 0x02001811 RID: 6161
	public class TwitchEventEntry : BaseTwitchEventEntry
	{
		// Token: 0x0600BDB2 RID: 48562 RVA: 0x00463905 File Offset: 0x00461B05
		public override bool IsValid(int amount = -1, string name = "", TwitchSubEventEntry.SubTierTypes subTier = TwitchSubEventEntry.SubTierTypes.Any)
		{
			return (this.StartAmount == -1 || amount >= this.StartAmount) && (this.EndAmount == -1 || amount <= this.EndAmount);
		}

		// Token: 0x04008E99 RID: 36505
		public int StartAmount = -1;

		// Token: 0x04008E9A RID: 36506
		public int EndAmount = -1;
	}
}
