using System;
using UnityEngine.Scripting;

// Token: 0x020009F3 RID: 2547
[Preserve]
public class RewardQuest : BaseReward
{
	// Token: 0x06004C0D RID: 19469 RVA: 0x001D7C9E File Offset: 0x001D5E9E
	public override void SetupReward()
	{
		base.Description = Localization.Get("RewardQuest_keyword", false, null);
		base.ValueText = QuestClass.GetQuest(base.ID).Name;
		base.Icon = "ui_game_symbol_quest";
	}

	// Token: 0x06004C0E RID: 19470 RVA: 0x001D7CD4 File Offset: 0x001D5ED4
	public override void GiveReward(EntityPlayer player)
	{
		Quest quest = QuestClass.CreateQuest(base.ID);
		if (base.OwnerQuest != null)
		{
			quest.PreviousQuest = QuestClass.GetQuest(base.OwnerQuest.ID).Name;
		}
		player.QuestJournal.AddQuest(quest, true);
	}

	// Token: 0x06004C0F RID: 19471 RVA: 0x001D7D20 File Offset: 0x001D5F20
	public override BaseReward Clone()
	{
		RewardQuest rewardQuest = new RewardQuest();
		base.CopyValues(rewardQuest);
		rewardQuest.IsChainQuest = this.IsChainQuest;
		return rewardQuest;
	}

	// Token: 0x06004C10 RID: 19472 RVA: 0x001D7D48 File Offset: 0x001D5F48
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

	// Token: 0x04003BCA RID: 15306
	public static string PropQuest = "quest";

	// Token: 0x04003BCB RID: 15307
	public static string PropChainQuest = "chainquest";

	// Token: 0x04003BCC RID: 15308
	public bool IsChainQuest = true;
}
