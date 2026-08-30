using System;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x020009E6 RID: 2534
[Preserve]
public class RewardSkillPoints : BaseReward
{
	// Token: 0x06004B9C RID: 19356 RVA: 0x001D58A4 File Offset: 0x001D3AA4
	public override void SetupReward()
	{
		string text = Localization.Get("RewardSkillPoint_keyword", false, null);
		base.Description = text;
		base.ValueText = string.Format("+{0}", base.Value);
		base.Icon = "ui_game_symbol_skills";
	}

	// Token: 0x06004B9D RID: 19357 RVA: 0x001D58E6 File Offset: 0x001D3AE6
	public override void GiveReward(EntityPlayer player)
	{
		player.Progression.SkillPoints += StringParsers.ParseSInt32(base.Value, 0, -1, NumberStyles.Integer);
	}

	// Token: 0x06004B9E RID: 19358 RVA: 0x001D5908 File Offset: 0x001D3B08
	public override BaseReward Clone()
	{
		RewardSkillPoints rewardSkillPoints = new RewardSkillPoints();
		base.CopyValues(rewardSkillPoints);
		return rewardSkillPoints;
	}

	// Token: 0x06004B9F RID: 19359 RVA: 0x001D5923 File Offset: 0x001D3B23
	public override string GetRewardText()
	{
		return string.Format("{0} {1}", base.Description, base.ValueText);
	}
}
