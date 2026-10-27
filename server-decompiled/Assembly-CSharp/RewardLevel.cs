using System;
using UnityEngine.Scripting;

// Token: 0x02000A05 RID: 2565
[Preserve]
public class RewardLevel : BaseReward
{
	// Token: 0x06004BF0 RID: 19440 RVA: 0x001D9993 File Offset: 0x001D7B93
	public override void SetupReward()
	{
		base.Description = string.Format("{0}", Localization.Get("RewardLevel_keyword", false, null));
		base.ValueText = base.Value;
		base.Icon = "ui_game_symbol_trophy";
	}

	// Token: 0x06004BF1 RID: 19441 RVA: 0x001D99C8 File Offset: 0x001D7BC8
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

	// Token: 0x06004BF2 RID: 19442 RVA: 0x001D9A40 File Offset: 0x001D7C40
	public override BaseReward Clone()
	{
		RewardLevel rewardLevel = new RewardLevel();
		base.CopyValues(rewardLevel);
		return rewardLevel;
	}
}
