using System;

namespace Twitch
{
	// Token: 0x02001863 RID: 6243
	public class TwitchEventEntry : BaseTwitchEventEntry
	{
		// Token: 0x0600C08F RID: 49295 RVA: 0x0046DBDD File Offset: 0x0046BDDD
		public override bool IsValid(int amount = -1, string name = "", TwitchSubEventEntry.SubTierTypes subTier = TwitchSubEventEntry.SubTierTypes.Any)
		{
			return (this.StartAmount == -1 || amount >= this.StartAmount) && (this.EndAmount == -1 || amount <= this.EndAmount);
		}

		// Token: 0x0400907B RID: 36987
		public int StartAmount = -1;

		// Token: 0x0400907C RID: 36988
		public int EndAmount = -1;
	}
}
