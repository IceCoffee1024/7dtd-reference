using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FFC RID: 4092
[Preserve]
public class XUiC_QuestTurnInWindowGroup : XUiController
{
	// Token: 0x0600812F RID: 33071 RVA: 0x0030FCD4 File Offset: 0x0030DED4
	public static void Open(XUi xui, Action _OnCloseAction = null)
	{
		xui.playerUI.windowManager.Open("questTurnIn", true);
		if (_OnCloseAction != null)
		{
			GUIWindow window = xui.playerUI.windowManager.GetWindow("questTurnIn");
			window.OnWindowClose = (Action)Delegate.Combine(window.OnWindowClose, _OnCloseAction);
		}
	}

	// Token: 0x06008130 RID: 33072 RVA: 0x0030FD25 File Offset: 0x0030DF25
	public override void Init()
	{
		base.Init();
		this.nonPagingHeaderWindow = base.GetChildByType<XUiC_WindowNonPagingHeader>();
		this.detailsWindow = base.GetChildByType<XUiC_QuestTurnInDetailsWindow>();
		this.rewardsWindow = base.GetChildByType<XUiC_QuestTurnInRewardsWindow>();
	}

	// Token: 0x06008131 RID: 33073 RVA: 0x0030FD54 File Offset: 0x0030DF54
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

	// Token: 0x06008132 RID: 33074 RVA: 0x0030FE8C File Offset: 0x0030E08C
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

	// Token: 0x06008133 RID: 33075 RVA: 0x0030FF4C File Offset: 0x0030E14C
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

	// Token: 0x04005FC8 RID: 24520
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestTurnInDetailsWindow detailsWindow;

	// Token: 0x04005FC9 RID: 24521
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestTurnInRewardsWindow rewardsWindow;

	// Token: 0x04005FCA RID: 24522
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeaderWindow;

	// Token: 0x04005FCB RID: 24523
	public static string ID = "questTurnIn";

	// Token: 0x04005FCC RID: 24524
	public EntityNPC NPC;
}
