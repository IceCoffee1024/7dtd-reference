using System;
using SandboxOptions;
using UnityEngine.Scripting;

// Token: 0x020002FB RID: 763
[Preserve]
public class DialogRequirementQuestsAvailable : BaseDialogRequirement
{
	// Token: 0x1700026B RID: 619
	// (get) Token: 0x060015AE RID: 5550 RVA: 0x0002F260 File Offset: 0x0002D460
	public override BaseDialogRequirement.RequirementTypes RequirementType
	{
		get
		{
			return BaseDialogRequirement.RequirementTypes.QuestsAvailable;
		}
	}

	// Token: 0x060015AF RID: 5551 RVA: 0x0003223F File Offset: 0x0003043F
	public override string GetRequiredDescription(EntityPlayer player)
	{
		return "";
	}

	// Token: 0x060015B0 RID: 5552 RVA: 0x0008158C File Offset: 0x0007F78C
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

	// Token: 0x04000E8F RID: 3727
	[PublicizedFrom(EAccessModifier.Private)]
	public static FastTags<TagGroup.Global> introtag = FastTags<TagGroup.Global>.Parse("introquest");
}
