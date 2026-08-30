using System;
using UnityEngine.Scripting;

// Token: 0x020009E2 RID: 2530
[Preserve]
public class RewardQuest : BaseReward
{
	// Token: 0x06004B87 RID: 19335 RVA: 0x001D54B6 File Offset: 0x001D36B6
	public override void SetupReward()
	{
		base.Description = Localization.Get("RewardQuest_keyword", false, null);
		base.ValueText = QuestClass.GetQuest(base.ID).Name;
		base.Icon = "ui_game_symbol_quest";
	}

	// Token: 0x06004B88 RID: 19336 RVA: 0x001D54EC File Offset: 0x001D36EC
	public override void GiveReward(EntityPlayer player)
	{
		Quest quest = QuestClass.CreateQuest(base.ID);
		if (base.OwnerQuest != null)
		{
			quest.PreviousQuest = QuestClass.GetQuest(base.OwnerQuest.ID).Name;
		}
		player.QuestJournal.AddQuest(quest, true);
	}

	// Token: 0x06004B89 RID: 19337 RVA: 0x001D5538 File Offset: 0x001D3738
	public override BaseReward Clone()
	{
		RewardQuest rewardQuest = new RewardQuest();
		base.CopyValues(rewardQuest);
		rewardQuest.IsChainQuest = this.IsChainQuest;
		return rewardQuest;
	}

	// Token: 0x06004B8A RID: 19338 RVA: 0x001D5560 File Offset: 0x001D3760
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

	// Token: 0x04003B39 RID: 15161
	public static string PropQuest = "quest";

	// Token: 0x04003B3A RID: 15162
	public static string PropChainQuest = "chainquest";

	// Token: 0x04003B3B RID: 15163
	public bool IsChainQuest = true;
}
