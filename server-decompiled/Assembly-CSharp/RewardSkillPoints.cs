using System;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x02000A01 RID: 2561
[Preserve]
public class RewardSkillPoints : BaseReward
{
	// Token: 0x06004BF1 RID: 19441 RVA: 0x001D933C File Offset: 0x001D753C
	public override void SetupReward()
	{
		string text = Localization.Get("RewardSkillPoint_keyword", false, null);
		base.Description = text;
		base.ValueText = string.Format("+{0}", base.Value);
		base.Icon = "ui_game_symbol_skills";
	}

	// Token: 0x06004BF2 RID: 19442 RVA: 0x001D937E File Offset: 0x001D757E
	public override void GiveReward(EntityPlayer player)
	{
		player.Progression.SkillPoints += StringParsers.ParseSInt32(base.Value, 0, -1, NumberStyles.Integer);
	}

	// Token: 0x06004BF3 RID: 19443 RVA: 0x001D93A0 File Offset: 0x001D75A0
	public override BaseReward Clone()
	{
		RewardSkillPoints rewardSkillPoints = new RewardSkillPoints();
		base.CopyValues(rewardSkillPoints);
		return rewardSkillPoints;
	}

	// Token: 0x06004BF4 RID: 19444 RVA: 0x001D93BB File Offset: 0x001D75BB
	public override string GetRewardText()
	{
		return string.Format("{0} {1}", base.Description, base.ValueText);
	}
}
