using System;
using UnityEngine.Scripting;

// Token: 0x020009DE RID: 2526
[Preserve]
public class RewardExp : BaseReward
{
	// Token: 0x06004B66 RID: 19302 RVA: 0x001D4775 File Offset: 0x001D2975
	public override void SetupReward()
	{
		base.Description = Localization.Get("RewardExp_keyword", false, null);
		this.SetupValueText();
		base.Icon = "ui_game_symbol_trophy";
	}

	// Token: 0x06004B67 RID: 19303 RVA: 0x001D479C File Offset: 0x001D299C
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetupValueText()
	{
		float num = Convert.ToSingle(base.Value) * Progression.XPGain;
		if (num > 214748370f)
		{
			num = 214748370f;
		}
		base.ValueText = num.ToString("{0}");
	}

	// Token: 0x06004B68 RID: 19304 RVA: 0x001D47DC File Offset: 0x001D29DC
	public override void GiveReward(EntityPlayer player)
	{
		int num = Convert.ToInt32(base.Value);
		player.Progression.AddLevelExp(num, "_xpFromQuest", Progression.XPTypes.Quest, true, true, -1, null);
	}

	// Token: 0x06004B69 RID: 19305 RVA: 0x001D480C File Offset: 0x001D2A0C
	public override BaseReward Clone()
	{
		RewardExp rewardExp = new RewardExp();
		base.CopyValues(rewardExp);
		return rewardExp;
	}

	// Token: 0x06004B6A RID: 19306 RVA: 0x001D4827 File Offset: 0x001D2A27
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		if (properties.Values.ContainsKey(RewardExp.PropExp))
		{
			base.Value = properties.Values[RewardExp.PropExp];
		}
	}

	// Token: 0x04003B33 RID: 15155
	public static string PropExp = "xp";

	// Token: 0x04003B34 RID: 15156
	[PublicizedFrom(EAccessModifier.Private)]
	public static FastTags<TagGroup.Global> questTag = FastTags<TagGroup.Global>.Parse("quest");
}
