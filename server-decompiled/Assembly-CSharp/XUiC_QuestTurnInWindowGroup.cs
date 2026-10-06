using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200100B RID: 4107
[Preserve]
public class XUiC_QuestTurnInWindowGroup : XUiController
{
	// Token: 0x0600813E RID: 33086 RVA: 0x00312CBC File Offset: 0x00310EBC
	public static void Open(XUi xui, Action _OnCloseAction = null)
	{
		xui.playerUI.windowManager.Open("questTurnIn", true);
		if (_OnCloseAction != null)
		{
			GUIWindow window = xui.playerUI.windowManager.GetWindow("questTurnIn");
			window.OnWindowClose = (Action)Delegate.Combine(window.OnWindowClose, _OnCloseAction);
		}
	}

	// Token: 0x0600813F RID: 33087 RVA: 0x00312D0D File Offset: 0x00310F0D
	public override void Init()
	{
		base.Init();
		this.nonPagingHeaderWindow = base.GetChildByType<XUiC_WindowNonPagingHeader>();
		this.detailsWindow = base.GetChildByType<XUiC_QuestTurnInDetailsWindow>();
		this.rewardsWindow = base.GetChildByType<XUiC_QuestTurnInRewardsWindow>();
	}

	// Token: 0x06008140 RID: 33088 RVA: 0x00312D3C File Offset: 0x00310F3C
	public override void OnOpen()
	{
		if (this.xui.Dialog.Respondent != null)
		{
			this.NPC = this.xui.Dialog.Respondent;
		}
		else
		{
			this.NPC = this.xui.Trader.Trader as EntityTrader;
		}
		this.detailsWindow.NPC = (this.rewardsWindow.NPC = this.NPC);
		XUiC_ItemInfoWindow childByType = this.xui.GetChildByType<XUiC_ItemInfoWindow>();
		this.rewardsWindow.InfoWindow = childByType;
		base.OnOpen();
		this.xui.playerUI.entityPlayer.OverrideFOV = 30f;
		this.xui.playerUI.entityPlayer.OverrideLookAt = this.NPC.getHeadPosition();
		GUIWindowManager windowManager = this.xui.playerUI.windowManager;
		if (this.nonPagingHeaderWindow != null)
		{
			this.nonPagingHeaderWindow.SetHeader("QUEST COMPLETE");
		}
		windowManager.Close("windowpaging");
		this.xui.DragAndDropWindow.InMenu = true;
		EntityTrader entityTrader = this.xui.Dialog.Respondent as EntityTrader;
		if (entityTrader != null)
		{
			entityTrader.SetNextTraderWindow(EntityTrader.TraderWindowState.Dialog);
		}
	}

	// Token: 0x06008141 RID: 33089 RVA: 0x00312E74 File Offset: 0x00311074
	public override void OnClose()
	{
		EntityTrader entityTrader = this.xui.Dialog.Respondent as EntityTrader;
		base.OnClose();
		EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
		if (GameManager.Instance.World == null)
		{
			return;
		}
		if (entityTrader == null)
		{
			if (Vector3.Distance(this.xui.Dialog.Respondent.position, entityPlayer.position) > 5f)
			{
				this.xui.Dialog.Respondent = null;
				this.xui.playerUI.entityPlayer.OverrideFOV = -1f;
				return;
			}
			this.xui.playerUI.windowManager.Open("dialog", true);
		}
	}

	// Token: 0x06008142 RID: 33090 RVA: 0x00312F34 File Offset: 0x00311134
	public void TryNextComplete()
	{
		Quest nextCompletedQuest = this.xui.playerUI.entityPlayer.QuestJournal.GetNextCompletedQuest(this.xui.Dialog.QuestTurnIn, this.NPC.entityId);
		if (nextCompletedQuest == null)
		{
			this.xui.playerUI.windowManager.CloseAllOpenModalWindows(null, false);
			return;
		}
		this.xui.Dialog.QuestTurnIn = nextCompletedQuest;
		this.detailsWindow.CurrentQuest = (this.rewardsWindow.CurrentQuest = nextCompletedQuest);
		base.RefreshBindings();
	}

	// Token: 0x04005FF1 RID: 24561
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestTurnInDetailsWindow detailsWindow;

	// Token: 0x04005FF2 RID: 24562
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestTurnInRewardsWindow rewardsWindow;

	// Token: 0x04005FF3 RID: 24563
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeaderWindow;

	// Token: 0x04005FF4 RID: 24564
	public static string ID = "questTurnIn";

	// Token: 0x04005FF5 RID: 24565
	public EntityNPC NPC;
}
