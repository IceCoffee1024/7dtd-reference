using System;
using UnityEngine.Scripting;

// Token: 0x020009EF RID: 2543
[Preserve]
public class RewardExp : BaseReward
{
	// Token: 0x06004BEC RID: 19436 RVA: 0x001D6F5D File Offset: 0x001D515D
	public override void SetupReward()
	{
		base.Description = Localization.Get("RewardExp_keyword", false, null);
		this.SetupValueText();
		base.Icon = "ui_game_symbol_trophy";
	}

	// Token: 0x06004BED RID: 19437 RVA: 0x001D6F84 File Offset: 0x001D5184
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

	// Token: 0x06004BEE RID: 19438 RVA: 0x001D6FC4 File Offset: 0x001D51C4
	public override void GiveReward(EntityPlayer player)
	{
		int num = Convert.ToInt32(base.Value);
		player.Progression.AddLevelExp(num, "_xpFromQuest", Progression.XPTypes.Quest, true, true, -1, null);
	}

	// Token: 0x06004BEF RID: 19439 RVA: 0x001D6FF4 File Offset: 0x001D51F4
	public override BaseReward Clone()
	{
		RewardExp rewardExp = new RewardExp();
		base.CopyValues(rewardExp);
		return rewardExp;
	}

	// Token: 0x06004BF0 RID: 19440 RVA: 0x001D700F File Offset: 0x001D520F
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		if (properties.Values.ContainsKey(RewardExp.PropExp))
		{
			base.Value = properties.Values[RewardExp.PropExp];
		}
	}

	// Token: 0x04003BC4 RID: 15300
	public static string PropExp = "xp";

	// Token: 0x04003BC5 RID: 15301
	[PublicizedFrom(EAccessModifier.Private)]
	public static FastTags<TagGroup.Global> questTag = FastTags<TagGroup.Global>.Parse("quest");
}
