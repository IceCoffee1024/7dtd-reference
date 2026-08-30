using System;
using System.Collections.Generic;

// Token: 0x020009D9 RID: 2521
public class QuestTierReward
{
	// Token: 0x06004B31 RID: 19249 RVA: 0x001D3EEC File Offset: 0x001D20EC
	public void GiveRewards(EntityPlayer player)
	{
		for (int i = 0; i < this.Rewards.Count; i++)
		{
			this.Rewards[i].GiveReward(player);
		}
	}

	// Token: 0x04003B06 RID: 15110
	public int Tier;

	// Token: 0x04003B07 RID: 15111
	public List<BaseReward> Rewards = new List<BaseReward>();
}
