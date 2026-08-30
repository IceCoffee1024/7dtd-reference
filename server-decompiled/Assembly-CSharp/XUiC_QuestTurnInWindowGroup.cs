using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FE3 RID: 4067
[Preserve]
public class XUiC_QuestTurnInWindowGroup : XUiController
{
	// Token: 0x0600803D RID: 32829 RVA: 0x0030B4A0 File Offset: 0x003096A0
	public static void Open(XUi xui, Action _OnCloseAction = null)
	{
		xui.playerUI.windowManager.Open("questTurnIn", true);
		if (_OnCloseAction != null)
		{
			GUIWindow window = xui.playerUI.windowManager.GetWindow("questTurnIn");
			window.OnWindowClose = (Action)Delegate.Combine(window.OnWindowClose, _OnCloseAction);
		}
	}

	// Token: 0x0600803E RID: 32830 RVA: 0x0030B4F1 File Offset: 0x003096F1
	public override void Init()
	{
		base.Init();
		this.nonPagingHeaderWindow = base.GetChildByType<XUiC_WindowNonPagingHeader>();
		this.detailsWindow = base.GetChildByType<XUiC_QuestTurnInDetailsWindow>();
		this.rewardsWindow = base.GetChildByType<XUiC_QuestTurnInRewardsWindow>();
	}

	// Token: 0x0600803F RID: 32831 RVA: 0x0030B520 File Offset: 0x00309720
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

	// Token: 0x06008040 RID: 32832 RVA: 0x0030B658 File Offset: 0x00309858
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

	// Token: 0x06008041 RID: 32833 RVA: 0x0030B718 File Offset: 0x00309918
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

	// Token: 0x04005F01 RID: 24321
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestTurnInDetailsWindow detailsWindow;

	// Token: 0x04005F02 RID: 24322
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestTurnInRewardsWindow rewardsWindow;

	// Token: 0x04005F03 RID: 24323
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeaderWindow;

	// Token: 0x04005F04 RID: 24324
	public static string ID = "questTurnIn";

	// Token: 0x04005F05 RID: 24325
	public EntityNPC NPC;
}
