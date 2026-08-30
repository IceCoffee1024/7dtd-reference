using System;
using UnityEngine.Scripting;

// Token: 0x020009E0 RID: 2528
[Preserve]
public class RewardLevel : BaseReward
{
	// Token: 0x06004B77 RID: 19319 RVA: 0x001D4E11 File Offset: 0x001D3011
	public override void SetupReward()
	{
		base.Description = string.Format("{0}", Localization.Get("RewardLevel_keyword", false, null));
		base.ValueText = base.Value;
		base.Icon = "ui_game_symbol_trophy";
	}

	// Token: 0x06004B78 RID: 19320 RVA: 0x001D4E48 File Offset: 0x001D3048
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

	// Token: 0x06004B79 RID: 19321 RVA: 0x001D4EC0 File Offset: 0x001D30C0
	public override BaseReward Clone()
	{
		RewardLevel rewardLevel = new RewardLevel();
		base.CopyValues(rewardLevel);
		return rewardLevel;
	}
}
