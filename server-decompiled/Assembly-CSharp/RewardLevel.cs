using System;
using UnityEngine.Scripting;

// Token: 0x020009F1 RID: 2545
[Preserve]
public class RewardLevel : BaseReward
{
	// Token: 0x06004BFD RID: 19453 RVA: 0x001D75F9 File Offset: 0x001D57F9
	public override void SetupReward()
	{
		base.Description = string.Format("{0}", Localization.Get("RewardLevel_keyword", false, null));
		base.ValueText = base.Value;
		base.Icon = "ui_game_symbol_trophy";
	}

	// Token: 0x06004BFE RID: 19454 RVA: 0x001D7630 File Offset: 0x001D5830
	public override void GiveReward(EntityPlayer player)
	{
		int num = Convert.ToInt32(base.Value);
		float levelProgressPercentage = player.Progression.GetLevelProgressPercentage();
		for (int i = 0; i < num; i++)
		{
			player.Progression.AddLevelExp(player.Progression.ExpToNextLevel, "_xpOther", Progression.XPTypes.Other, true, true, -1, null);
		}
		player.Progression.AddLevelExp((int)(levelProgressPercentage * (float)player.Progression.GetExpForNextLevel()), "_xpOther", Progression.XPTypes.Other, true, true, -1, null);
	}

	// Token: 0x06004BFF RID: 19455 RVA: 0x001D76A8 File Offset: 0x001D58A8
	public override BaseReward Clone()
	{
		RewardLevel rewardLevel = new RewardLevel();
		base.CopyValues(rewardLevel);
		return rewardLevel;
	}
}
