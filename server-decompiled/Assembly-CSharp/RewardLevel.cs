using System;
using UnityEngine.Scripting;

// Token: 0x020009FB RID: 2555
[Preserve]
public class RewardLevel : BaseReward
{
	// Token: 0x06004BCC RID: 19404 RVA: 0x001D88AB File Offset: 0x001D6AAB
	public override void SetupReward()
	{
		base.Description = string.Format("{0}", Localization.Get("RewardLevel_keyword", false, null));
		base.ValueText = base.Value;
		base.Icon = "ui_game_symbol_trophy";
	}

	// Token: 0x06004BCD RID: 19405 RVA: 0x001D88E0 File Offset: 0x001D6AE0
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

	// Token: 0x06004BCE RID: 19406 RVA: 0x001D8958 File Offset: 0x001D6B58
	public override BaseReward Clone()
	{
		RewardLevel rewardLevel = new RewardLevel();
		base.CopyValues(rewardLevel);
		return rewardLevel;
	}
}
