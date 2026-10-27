using System;

namespace Twitch
{
	// Token: 0x02001893 RID: 6291
	public class TwitchCreatorGoalEventEntry : BaseTwitchEventEntry
	{
		// Token: 0x0600C1A1 RID: 49569 RVA: 0x00471560 File Offset: 0x0046F760
		public override bool IsValid(int amount = -1, string name = "", TwitchSubEventEntry.SubTierTypes subTier = TwitchSubEventEntry.SubTierTypes.Any)
		{
			return name == this.GoalType;
		}

		// Token: 0x04009161 RID: 37217
		public string GoalType = "Subs";

		// Token: 0x04009162 RID: 37218
		public int RewardAmount = 100;

		// Token: 0x04009163 RID: 37219
		public TwitchAction.PointTypes RewardType;
	}
}
