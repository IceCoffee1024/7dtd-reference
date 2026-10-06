using System;
using UnityEngine.Scripting;

// Token: 0x020002E7 RID: 743
[Preserve]
public class DialogActionAddQuest : BaseDialogAction
{
	// Token: 0x17000254 RID: 596
	// (get) Token: 0x06001555 RID: 5461 RVA: 0x0002F260 File Offset: 0x0002D460
	public override BaseDialogAction.ActionTypes ActionType
	{
		get
		{
			return BaseDialogAction.ActionTypes.AddQuest;
		}
	}

	// Token: 0x06001556 RID: 5462 RVA: 0x00080650 File Offset: 0x0007E850
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

	// Token: 0x04000E50 RID: 3664
	[PublicizedFrom(EAccessModifier.Private)]
	public string name = "";

	// Token: 0x04000E51 RID: 3665
	public Quest Quest;

	// Token: 0x04000E52 RID: 3666
	public int ListIndex;
}
