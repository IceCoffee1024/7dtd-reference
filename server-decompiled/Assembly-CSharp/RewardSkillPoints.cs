using System;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x02000A0B RID: 2571
[Preserve]
public class RewardSkillPoints : BaseReward
{
	// Token: 0x06004C15 RID: 19477 RVA: 0x001DA424 File Offset: 0x001D8624
	public override void SetupReward()
	{
		string text = Localization.Get("RewardSkillPoint_keyword", false, null);
		base.Description = text;
		base.ValueText = string.Format("+{0}", base.Value);
		base.Icon = "ui_game_symbol_skills";
	}

	// Token: 0x06004C16 RID: 19478 RVA: 0x001DA466 File Offset: 0x001D8666
	public override void GiveReward(EntityPlayer player)
	{
		player.Progression.SkillPoints += StringParsers.ParseSInt32(base.Value, 0, -1, NumberStyles.Integer);
	}

	// Token: 0x06004C17 RID: 19479 RVA: 0x001DA488 File Offset: 0x001D8688
	public override BaseReward Clone()
	{
		RewardSkillPoints rewardSkillPoints = new RewardSkillPoints();
		base.CopyValues(rewardSkillPoints);
		return rewardSkillPoints;
	}

	// Token: 0x06004C18 RID: 19480 RVA: 0x001DA4A3 File Offset: 0x001D86A3
	public override string GetRewardText()
	{
		return string.Format("{0} {1}", base.Description, base.ValueText);
	}
}
