using System;
using UnityEngine.Scripting;

// Token: 0x020002EB RID: 747
[Preserve]
public class DialogActionCompleteQuest : BaseDialogAction
{
	// Token: 0x17000255 RID: 597
	// (get) Token: 0x0600155E RID: 5470 RVA: 0x00047076 File Offset: 0x00045276
	public override BaseDialogAction.ActionTypes ActionType
	{
		get
		{
			return BaseDialogAction.ActionTypes.CompleteQuest;
		}
	}

	// Token: 0x0600155F RID: 5471 RVA: 0x00080950 File Offset: 0x0007EB50
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
