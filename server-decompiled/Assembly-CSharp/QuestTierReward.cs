using System;
using System.Collections.Generic;

// Token: 0x020009F4 RID: 2548
public class QuestTierReward
{
	// Token: 0x06004B86 RID: 19334 RVA: 0x001D7978 File Offset: 0x001D5B78
	public void GiveRewards(EntityPlayer player)
	{
		for (int i = 0; i < this.Rewards.Count; i++)
		{
			this.Rewards[i].GiveReward(player);
		}
	}

	// Token: 0x04003BA4 RID: 15268
	public int Tier;

	// Token: 0x04003BA5 RID: 15269
	public List<BaseReward> Rewards = new List<BaseReward>();
}
