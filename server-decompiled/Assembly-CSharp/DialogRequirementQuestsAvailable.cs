using System;
using SandboxOptions;
using UnityEngine.Scripting;

// Token: 0x020002FD RID: 765
[Preserve]
public class DialogRequirementQuestsAvailable : BaseDialogRequirement
{
	// Token: 0x1700026B RID: 619
	// (get) Token: 0x060015B2 RID: 5554 RVA: 0x0002F260 File Offset: 0x0002D460
	public override BaseDialogRequirement.RequirementTypes RequirementType
	{
		get
		{
			return BaseDialogRequirement.RequirementTypes.QuestsAvailable;
		}
	}

	// Token: 0x060015B3 RID: 5555 RVA: 0x000321FF File Offset: 0x000303FF
	public override string GetRequiredDescription(EntityPlayer player)
	{
		return "";
	}

	// Token: 0x060015B4 RID: 5556 RVA: 0x000817B0 File Offset: 0x0007F9B0
	public override bool CheckRequirement(EntityPlayer player, EntityNPC talkingTo)
	{
		LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(GameManager.Instance.World.GetPrimaryPlayer());
		EntityTrader entityTrader = uiforPlayer.xui.Dialog.Respondent as EntityTrader;
		if (!SandboxOptionManager.GetBool(global::SandboxOptions.SandboxOptions.QuestsEnabled))
		{
			return false;
		}
		bool flag = false;
		if (entityTrader.activeQuests != null)
		{
			int currentFactionTier = uiforPlayer.entityPlayer.QuestJournal.GetCurrentFactionTier(entityTrader.NPCInfo.QuestFaction, 0, false);
			for (int i = 0; i < entityTrader.activeQuests.Count; i++)
			{
				if (entityTrader.activeQuests[i].QuestClass.QuestType == base.Value && (!entityTrader.activeQuests[i].QuestClass.ExtraTags.Test_AnySet(DialogRequirementQuestsAvailable.introtag) || QuestJournal.IntroQuestEnabled) && (int)entityTrader.activeQuests[i].QuestClass.DifficultyTier <= currentFactionTier && (entityTrader.activeQuests[i].QuestClass.Repeatable || uiforPlayer.entityPlayer.QuestJournal.FindActiveOrCompleteQuest(entityTrader.activeQuests[i].ID, (int)entityTrader.NPCInfo.QuestFaction) == null))
				{
					flag = true;
					break;
				}
			}
		}
		return flag;
	}

	// Token: 0x04000E9F RID: 3743
	[PublicizedFrom(EAccessModifier.Private)]
	public static FastTags<TagGroup.Global> introtag = FastTags<TagGroup.Global>.Parse("introquest");
}
