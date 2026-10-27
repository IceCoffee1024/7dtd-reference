using System;
using UnityEngine.Scripting;

// Token: 0x020002E9 RID: 745
[Preserve]
public class DialogActionAddQuest : BaseDialogAction
{
	// Token: 0x17000254 RID: 596
	// (get) Token: 0x06001559 RID: 5465 RVA: 0x0002F260 File Offset: 0x0002D460
	public override BaseDialogAction.ActionTypes ActionType
	{
		get
		{
			return BaseDialogAction.ActionTypes.AddQuest;
		}
	}

	// Token: 0x0600155A RID: 5466 RVA: 0x00080820 File Offset: 0x0007EA20
	public override void PerformAction(EntityPlayer player)
	{
		if (this.Quest != null && this.Quest.QuestClass != null)
		{
			Quest quest = player.QuestJournal.FindNonSharedQuest(this.Quest.QuestCode);
			if (quest == null || !quest.Active)
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

	// Token: 0x04000E51 RID: 3665
	[PublicizedFrom(EAccessModifier.Private)]
	public string name = "";

	// Token: 0x04000E52 RID: 3666
	public Quest Quest;

	// Token: 0x04000E53 RID: 3667
	public int ListIndex;
}
