using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200101B RID: 4123
[Preserve]
public class XUiC_QuestTurnInWindowGroup : XUiController
{
	// Token: 0x06008190 RID: 33168 RVA: 0x003149C0 File Offset: 0x00312BC0
	public static void Open(XUi xui, Action _OnCloseAction = null)
	{
		xui.playerUI.windowManager.Open("questTurnIn", true);
		if (_OnCloseAction != null)
		{
			GUIWindow window = xui.playerUI.windowManager.GetWindow("questTurnIn");
			window.OnWindowClose = (Action)Delegate.Combine(window.OnWindowClose, _OnCloseAction);
		}
	}

	// Token: 0x06008191 RID: 33169 RVA: 0x00314A11 File Offset: 0x00312C11
	public override void Init()
	{
		base.Init();
		this.nonPagingHeaderWindow = base.GetChildByType<XUiC_WindowNonPagingHeader>();
		this.detailsWindow = base.GetChildByType<XUiC_QuestTurnInDetailsWindow>();
		this.rewardsWindow = base.GetChildByType<XUiC_QuestTurnInRewardsWindow>();
	}

	// Token: 0x06008192 RID: 33170 RVA: 0x00314A40 File Offset: 0x00312C40
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

	// Token: 0x06008193 RID: 33171 RVA: 0x00314B78 File Offset: 0x00312D78
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

	// Token: 0x06008194 RID: 33172 RVA: 0x00314C38 File Offset: 0x00312E38
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

	// Token: 0x0400602F RID: 24623
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestTurnInDetailsWindow detailsWindow;

	// Token: 0x04006030 RID: 24624
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestTurnInRewardsWindow rewardsWindow;

	// Token: 0x04006031 RID: 24625
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeaderWindow;

	// Token: 0x04006032 RID: 24626
	public static string ID = "questTurnIn";

	// Token: 0x04006033 RID: 24627
	public EntityNPC NPC;
}
