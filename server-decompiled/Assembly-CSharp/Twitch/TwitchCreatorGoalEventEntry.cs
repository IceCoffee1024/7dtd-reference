using System;

namespace Twitch
{
	// Token: 0x02001836 RID: 6198
	public class TwitchCreatorGoalEventEntry : BaseTwitchEventEntry
	{
		// Token: 0x0600BF35 RID: 48949 RVA: 0x00469678 File Offset: 0x00467878
		public override bool IsValid(int amount = -1, string name = "", TwitchSubEventEntry.SubTierTypes subTier = TwitchSubEventEntry.SubTierTypes.Any)
		{
			return name == this.GoalType;
		}

		// Token: 0x04008FB2 RID: 36786
		public string GoalType = "Subs";

		// Token: 0x04008FB3 RID: 36787
		public int RewardAmount = 100;

		// Token: 0x04008FB4 RID: 36788
		public TwitchAction.PointTypes RewardType;
	}
}
