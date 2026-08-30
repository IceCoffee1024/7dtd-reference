using System;
using SandboxOptions;
using UnityEngine.Scripting;

// Token: 0x020002F4 RID: 756
[Preserve]
public class DialogRequirementQuestsAvailable : BaseDialogRequirement
{
	// Token: 0x17000272 RID: 626
	// (get) Token: 0x0600159E RID: 5534 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override BaseDialogRequirement.RequirementTypes RequirementType
	{
		get
		{
			return BaseDialogRequirement.RequirementTypes.QuestsAvailable;
		}
	}

	// Token: 0x0600159F RID: 5535 RVA: 0x00031E8B File Offset: 0x0003008B
	public override string GetRequiredDescription(EntityPlayer player)
	{
		return "";
	}

	// Token: 0x060015A0 RID: 5536 RVA: 0x00080C24 File Offset: 0x0007EE24
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

	// Token: 0x04000E85 RID: 3717
	[PublicizedFrom(EAccessModifier.Private)]
	public static FastTags<TagGroup.Global> introtag = FastTags<TagGroup.Global>.Parse("introquest");
}
