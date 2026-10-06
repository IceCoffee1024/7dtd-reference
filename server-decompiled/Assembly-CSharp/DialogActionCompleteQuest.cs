using System;
using UnityEngine.Scripting;

// Token: 0x020002E9 RID: 745
[Preserve]
public class DialogActionCompleteQuest : BaseDialogAction
{
	// Token: 0x17000255 RID: 597
	// (get) Token: 0x0600155A RID: 5466 RVA: 0x000470B6 File Offset: 0x000452B6
	public override BaseDialogAction.ActionTypes ActionType
	{
		get
		{
			return BaseDialogAction.ActionTypes.CompleteQuest;
		}
	}

	// Token: 0x0600155B RID: 5467 RVA: 0x00080780 File Offset: 0x0007E980
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
