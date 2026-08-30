using System;
using System.Collections.Generic;

// Token: 0x020009EA RID: 2538
public class QuestTierReward
{
	// Token: 0x06004BB7 RID: 19383 RVA: 0x001D66D4 File Offset: 0x001D48D4
	public void GiveRewards(EntityPlayer player)
	{
		for (int i = 0; i < this.Rewards.Count; i++)
		{
			this.Rewards[i].GiveReward(player);
		}
	}

	// Token: 0x04003B97 RID: 15255
	public int Tier;

	// Token: 0x04003B98 RID: 15256
	public List<BaseReward> Rewards = new List<BaseReward>();
}
