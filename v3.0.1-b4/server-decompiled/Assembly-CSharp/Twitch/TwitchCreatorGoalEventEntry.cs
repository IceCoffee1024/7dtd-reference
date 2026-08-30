using System;

namespace Twitch
{
	// Token: 0x02001810 RID: 6160
	public class TwitchCreatorGoalEventEntry : BaseTwitchEventEntry
	{
		// Token: 0x0600BDB0 RID: 48560 RVA: 0x004638DC File Offset: 0x00461ADC
		public override bool IsValid(int amount = -1, string name = "", TwitchSubEventEntry.SubTierTypes subTier = TwitchSubEventEntry.SubTierTypes.Any)
		{
			return name == this.GoalType;
		}

		// Token: 0x04008E96 RID: 36502
		public string GoalType = "Subs";

		// Token: 0x04008E97 RID: 36503
		public int RewardAmount = 100;

		// Token: 0x04008E98 RID: 36504
		public TwitchAction.PointTypes RewardType;
	}
}
