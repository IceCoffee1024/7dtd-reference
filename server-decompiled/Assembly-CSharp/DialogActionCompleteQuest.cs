using System;
using UnityEngine.Scripting;

// Token: 0x020002E2 RID: 738
[Preserve]
public class DialogActionCompleteQuest : BaseDialogAction
{
	// Token: 0x1700025C RID: 604
	// (get) Token: 0x0600154A RID: 5450 RVA: 0x00046C7A File Offset: 0x00044E7A
	public override BaseDialogAction.ActionTypes ActionType
	{
		get
		{
			return BaseDialogAction.ActionTypes.CompleteQuest;
		}
	}

	// Token: 0x0600154B RID: 5451 RVA: 0x0007FE18 File Offset: 0x0007E018
	public override void PerformAction(EntityPlayer player)
	{
		if (base.ID != "")
		{
			Quest quest = player.QuestJournal.FindQuest(base.ID, (int)base.OwnerDialog.CurrentOwner.NPCInfo.QuestFaction);
			Convert.ToInt32(base.Value);
			if (quest != null && quest.Active)
			{
				QuestEventManager.Current.NPCInteracted(base.OwnerDialog.CurrentOwner);
				quest.RefreshQuestCompletion(QuestClass.CompletionTypes.TurnIn, null, true, null);
			}
		}
	}
}
