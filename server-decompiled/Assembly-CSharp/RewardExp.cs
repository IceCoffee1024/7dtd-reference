using System;
using UnityEngine.Scripting;

// Token: 0x02000A03 RID: 2563
[Preserve]
public class RewardExp : BaseReward
{
	// Token: 0x06004BDF RID: 19423 RVA: 0x001D92E9 File Offset: 0x001D74E9
	public override void SetupReward()
	{
		base.Description = Localization.Get("RewardExp_keyword", false, null);
		this.SetupValueText();
		base.Icon = "ui_game_symbol_trophy";
	}

	// Token: 0x06004BE0 RID: 19424 RVA: 0x001D9310 File Offset: 0x001D7510
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

	// Token: 0x06004BE1 RID: 19425 RVA: 0x001D9350 File Offset: 0x001D7550
	public override void GiveReward(EntityPlayer player)
	{
		int num = Convert.ToInt32(base.Value);
		player.Progression.AddLevelExp(num, "_xpFromQuest", Progression.XPTypes.Quest, true, true, -1, null);
	}

	// Token: 0x06004BE2 RID: 19426 RVA: 0x001D9380 File Offset: 0x001D7580
	public override BaseReward Clone()
	{
		RewardExp rewardExp = new RewardExp();
		base.CopyValues(rewardExp);
		return rewardExp;
	}

	// Token: 0x06004BE3 RID: 19427 RVA: 0x001D939B File Offset: 0x001D759B
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		if (properties.Values.ContainsKey(RewardExp.PropExp))
		{
			base.Value = properties.Values[RewardExp.PropExp];
		}
	}

	// Token: 0x04003BF2 RID: 15346
	public static string PropExp = "xp";

	// Token: 0x04003BF3 RID: 15347
	[PublicizedFrom(EAccessModifier.Private)]
	public static FastTags<TagGroup.Global> questTag = FastTags<TagGroup.Global>.Parse("quest");
}
