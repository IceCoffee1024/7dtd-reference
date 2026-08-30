using System;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x020009F7 RID: 2551
[Preserve]
public class RewardSkillPoints : BaseReward
{
	// Token: 0x06004C22 RID: 19490 RVA: 0x001D808C File Offset: 0x001D628C
	public override void SetupReward()
	{
		string text = Localization.Get("RewardSkillPoint_keyword", false, null);
		base.Description = text;
		base.ValueText = string.Format("+{0}", base.Value);
		base.Icon = "ui_game_symbol_skills";
	}

	// Token: 0x06004C23 RID: 19491 RVA: 0x001D80CE File Offset: 0x001D62CE
	public override void GiveReward(EntityPlayer player)
	{
		player.Progression.SkillPoints += StringParsers.ParseSInt32(base.Value, 0, -1, NumberStyles.Integer);
	}

	// Token: 0x06004C24 RID: 19492 RVA: 0x001D80F0 File Offset: 0x001D62F0
	public override BaseReward Clone()
	{
		RewardSkillPoints rewardSkillPoints = new RewardSkillPoints();
		base.CopyValues(rewardSkillPoints);
		return rewardSkillPoints;
	}

	// Token: 0x06004C25 RID: 19493 RVA: 0x001D810B File Offset: 0x001D630B
	public override string GetRewardText()
	{
		return string.Format("{0} {1}", base.Description, base.ValueText);
	}
}
