using System;
using UnityEngine.Scripting;

// Token: 0x02000A07 RID: 2567
[Preserve]
public class RewardQuest : BaseReward
{
	// Token: 0x06004C00 RID: 19456 RVA: 0x001DA036 File Offset: 0x001D8236
	public override void SetupReward()
	{
		base.Description = Localization.Get("RewardQuest_keyword", false, null);
		base.ValueText = QuestClass.GetQuest(base.ID).Name;
		base.Icon = "ui_game_symbol_quest";
	}

	// Token: 0x06004C01 RID: 19457 RVA: 0x001DA06C File Offset: 0x001D826C
	public override void GiveReward(EntityPlayer player)
	{
		Quest quest = QuestClass.CreateQuest(base.ID);
		if (base.OwnerQuest != null)
		{
			quest.PreviousQuest = QuestClass.GetQuest(base.OwnerQuest.ID).Name;
		}
		player.QuestJournal.AddQuest(quest, Quest.QuestSource.QuestSystem, true);
	}

	// Token: 0x06004C02 RID: 19458 RVA: 0x001DA0B8 File Offset: 0x001D82B8
	public override BaseReward Clone()
	{
		RewardQuest rewardQuest = new RewardQuest();
		base.CopyValues(rewardQuest);
		rewardQuest.IsChainQuest = this.IsChainQuest;
		return rewardQuest;
	}

	// Token: 0x06004C03 RID: 19459 RVA: 0x001DA0E0 File Offset: 0x001D82E0
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		if (properties.Values.ContainsKey(RewardQuest.PropQuest))
		{
			base.ID = properties.Values[RewardQuest.PropQuest];
		}
		if (properties.Values.ContainsKey(RewardQuest.PropChainQuest))
		{
			this.IsChainQuest = Convert.ToBoolean(properties.Values[RewardQuest.PropChainQuest]);
		}
	}

	// Token: 0x04003BF8 RID: 15352
	public static string PropQuest = "quest";

	// Token: 0x04003BF9 RID: 15353
	public static string PropChainQuest = "chainquest";

	// Token: 0x04003BFA RID: 15354
	public bool IsChainQuest = true;
}
