using System;
using System.Collections.Generic;

// Token: 0x020009FE RID: 2558
public class QuestTierReward
{
	// Token: 0x06004BAA RID: 19370 RVA: 0x001D8A60 File Offset: 0x001D6C60
	public void GiveRewards(EntityPlayer player)
	{
		for (int i = 0; i < this.Rewards.Count; i++)
		{
			this.Rewards[i].GiveReward(player);
		}
	}

	// Token: 0x04003BC5 RID: 15301
	public int Tier;

	// Token: 0x04003BC6 RID: 15302
	public List<BaseReward> Rewards = new List<BaseReward>();
}
