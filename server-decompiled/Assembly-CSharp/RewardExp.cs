using System;
using UnityEngine.Scripting;

// Token: 0x020009F9 RID: 2553
[Preserve]
public class RewardExp : BaseReward
{
	// Token: 0x06004BBB RID: 19387 RVA: 0x001D8201 File Offset: 0x001D6401
	public override void SetupReward()
	{
		base.Description = Localization.Get("RewardExp_keyword", false, null);
		this.SetupValueText();
		base.Icon = "ui_game_symbol_trophy";
	}

	// Token: 0x06004BBC RID: 19388 RVA: 0x001D8228 File Offset: 0x001D6428
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

	// Token: 0x06004BBD RID: 19389 RVA: 0x001D8268 File Offset: 0x001D6468
	public override void GiveReward(EntityPlayer player)
	{
		int num = Convert.ToInt32(base.Value);
		player.Progression.AddLevelExp(num, "_xpFromQuest", Progression.XPTypes.Quest, true, true, -1, null);
	}

	// Token: 0x06004BBE RID: 19390 RVA: 0x001D8298 File Offset: 0x001D6498
	public override BaseReward Clone()
	{
		RewardExp rewardExp = new RewardExp();
		base.CopyValues(rewardExp);
		return rewardExp;
	}

	// Token: 0x06004BBF RID: 19391 RVA: 0x001D82B3 File Offset: 0x001D64B3
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		if (properties.Values.ContainsKey(RewardExp.PropExp))
		{
			base.Value = properties.Values[RewardExp.PropExp];
		}
	}

	// Token: 0x04003BD1 RID: 15313
	public static string PropExp = "xp";

	// Token: 0x04003BD2 RID: 15314
	[PublicizedFrom(EAccessModifier.Private)]
	public static FastTags<TagGroup.Global> questTag = FastTags<TagGroup.Global>.Parse("quest");
}
