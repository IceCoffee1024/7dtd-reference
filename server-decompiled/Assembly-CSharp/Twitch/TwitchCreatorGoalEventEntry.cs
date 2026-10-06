using System;

namespace Twitch
{
	// Token: 0x02001862 RID: 6242
	public class TwitchCreatorGoalEventEntry : BaseTwitchEventEntry
	{
		// Token: 0x0600C08D RID: 49293 RVA: 0x0046DBB4 File Offset: 0x0046BDB4
		public override bool IsValid(int amount = -1, string name = "", TwitchSubEventEntry.SubTierTypes subTier = TwitchSubEventEntry.SubTierTypes.Any)
		{
			return name == this.GoalType;
		}

		// Token: 0x04009078 RID: 36984
		public string GoalType = "Subs";

		// Token: 0x04009079 RID: 36985
		public int RewardAmount = 100;

		// Token: 0x0400907A RID: 36986
		public TwitchAction.PointTypes RewardType;
	}
}
