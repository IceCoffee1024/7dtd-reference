using System;
using UnityEngine.Scripting;

// Token: 0x020002E0 RID: 736
[Preserve]
public class DialogActionAddQuest : BaseDialogAction
{
	// Token: 0x1700025B RID: 603
	// (get) Token: 0x06001545 RID: 5445 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override BaseDialogAction.ActionTypes ActionType
	{
		get
		{
			return BaseDialogAction.ActionTypes.AddQuest;
		}
	}

	// Token: 0x06001546 RID: 5446 RVA: 0x0007FCE0 File Offset: 0x0007DEE0
	public override void PerformAction(EntityPlayer player)
	{
		if (this.Quest != null)
		{
			QuestClass questClass = this.Quest.QuestClass;
			if (questClass != null)
			{
				Quest quest = player.QuestJournal.FindNonSharedQuest(this.Quest.QuestCode);
				if (quest == null || (questClass.Repeatable && !quest.Active))
				{
					LocalPlayerUI playerUI = LocalPlayerUI.GetUIForPlayer(player as EntityPlayerLocal);
					XUiC_QuestOfferWindow.OpenQuestOfferWindow(playerUI.xui, this.Quest, this.ListIndex, XUiC_QuestOfferWindow.OfferTypes.Dialog, playerUI.xui.Dialog.Respondent.entityId, delegate(EntityNPC npc)
					{
						playerUI.xui.Dialog.Respondent = npc;
						playerUI.xui.Dialog.ReturnStatement = ((DialogResponseQuest)this.Owner).LastStatementID;
					});
					return;
				}
				GameManager.ShowTooltip((EntityPlayerLocal)player, Localization.Get("questunavailable", false, null), false, false, 0f);
			}
		}
	}

	// Token: 0x04000E46 RID: 3654
	[PublicizedFrom(EAccessModifier.Private)]
	public string name = "";

	// Token: 0x04000E47 RID: 3655
	public Quest Quest;

	// Token: 0x04000E48 RID: 3656
	public int ListIndex;
}
