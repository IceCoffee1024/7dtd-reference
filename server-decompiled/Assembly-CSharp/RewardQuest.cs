using System;
using UnityEngine.Scripting;

// Token: 0x020009FD RID: 2557
[Preserve]
public class RewardQuest : BaseReward
{
	// Token: 0x06004BDC RID: 19420 RVA: 0x001D8F4E File Offset: 0x001D714E
	public override void SetupReward()
	{
		base.Description = Localization.Get("RewardQuest_keyword", false, null);
		base.ValueText = QuestClass.GetQuest(base.ID).Name;
		base.Icon = "ui_game_symbol_quest";
	}

	// Token: 0x06004BDD RID: 19421 RVA: 0x001D8F84 File Offset: 0x001D7184
	public override void GiveReward(EntityPlayer player)
	{
		Quest quest = QuestClass.CreateQuest(base.ID);
		if (base.OwnerQuest != null)
		{
			quest.PreviousQuest = QuestClass.GetQuest(base.OwnerQuest.ID).Name;
		}
		player.QuestJournal.AddQuest(quest, Quest.QuestSource.QuestSystem, true);
	}

	// Token: 0x06004BDE RID: 19422 RVA: 0x001D8FD0 File Offset: 0x001D71D0
	public override BaseReward Clone()
	{
		RewardQuest rewardQuest = new RewardQuest();
		base.CopyValues(rewardQuest);
		rewardQuest.IsChainQuest = this.IsChainQuest;
		return rewardQuest;
	}

	// Token: 0x06004BDF RID: 19423 RVA: 0x001D8FF8 File Offset: 0x001D71F8
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

	// Token: 0x04003BD7 RID: 15319
	public static string PropQuest = "quest";

	// Token: 0x04003BD8 RID: 15320
	public static string PropChainQuest = "chainquest";

	// Token: 0x04003BD9 RID: 15321
	public bool IsChainQuest = true;
}
