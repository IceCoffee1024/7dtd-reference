using System;
using UnityEngine.Scripting;

// Token: 0x02000F35 RID: 3893
[Preserve]
public class XUiC_DialogWindowGroup : XUiController
{
	// Token: 0x17000E56 RID: 3670
	// (get) Token: 0x060079B1 RID: 31153 RVA: 0x002E0F76 File Offset: 0x002DF176
	// (set) Token: 0x060079B2 RID: 31154 RVA: 0x002E0F7E File Offset: 0x002DF17E
	public Dialog CurrentDialog
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x060079B3 RID: 31155 RVA: 0x002E0F88 File Offset: 0x002DF188
	public static void Open(XUi _xui, Action _onDialogClosed = null)
	{
		_xui.playerUI.windowManager.Open("dialog", true);
		if (_onDialogClosed != null)
		{
			GUIWindow window = _xui.playerUI.windowManager.GetWindow("dialog");
			window.OnWindowClose = (Action)Delegate.Combine(window.OnWindowClose, _onDialogClosed);
		}
	}

	// Token: 0x060079B4 RID: 31156 RVA: 0x002E0FD9 File Offset: 0x002DF1D9
	public override void Init()
	{
		base.Init();
		this.responseWindow = base.GetChildByType<XUiC_DialogResponseList>();
	}

	// Token: 0x060079B5 RID: 31157 RVA: 0x002D5E2F File Offset: 0x002D402F
	public override void Update(float _dt)
	{
		base.Update(_dt);
	}

	// Token: 0x060079B6 RID: 31158 RVA: 0x002E0FF0 File Offset: 0x002DF1F0
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.Dialog.DialogWindowGroup = this;
		this.xui.playerUI.entityPlayer.OverrideFOV = 30f;
		this.xui.playerUI.entityPlayer.OverrideLookAt = this.xui.Dialog.Respondent.getHeadPosition();
		this.xui.playerUI.windowManager.Close("windowpaging");
		this.xui.playerUI.windowManager.Close("toolbelt");
		this.CurrentDialog = Dialog.DialogList[this.xui.Dialog.Respondent.NPCInfo.DialogID];
		this.CurrentDialog.CurrentOwner = this.xui.Dialog.Respondent;
		if (this.xui.Dialog.ReturnStatement == "" || this.CurrentDialog.CurrentStatement == null)
		{
			this.CurrentDialog.RestartDialog(this.xui.playerUI.entityPlayer);
		}
		else if (this.xui.Dialog.ReturnStatement != "")
		{
			this.CurrentDialog.CurrentStatement = this.CurrentDialog.GetStatement(this.xui.Dialog.ReturnStatement);
			this.CurrentDialog.ChildDialog = null;
		}
		this.xui.Dialog.ReturnStatement = "";
		this.responseWindow.CurrentDialog = this.CurrentDialog;
		GameManager.Instance.SetToolTipPause(this.xui.playerUI.nguiWindowManager, true);
	}

	// Token: 0x060079B7 RID: 31159 RVA: 0x002E11A8 File Offset: 0x002DF3A8
	public override void OnClose()
	{
		base.OnClose();
		this.xui.playerUI.windowManager.Close("questOffer");
		this.xui.playerUI.windowManager.Open("toolbelt", false);
		this.xui.Dialog.Respondent = null;
		GameManager.Instance.SetToolTipPause(this.xui.playerUI.nguiWindowManager, false);
		if (!this.xui.Dialog.KeepZoomOnClose)
		{
			this.xui.playerUI.entityPlayer.OverrideFOV = -1f;
		}
	}

	// Token: 0x060079B8 RID: 31160 RVA: 0x002E1248 File Offset: 0x002DF448
	public void RefreshDialog()
	{
		if (this.CurrentDialog.CurrentStatement != null)
		{
			this.responseWindow.Refresh();
			return;
		}
		this.xui.playerUI.windowManager.Close("dialog");
	}

	// Token: 0x060079B9 RID: 31161 RVA: 0x002E127D File Offset: 0x002DF47D
	public void ShowResponseWindow(bool isVisible)
	{
		this.responseWindow.ViewComponent.IsVisible = isVisible;
	}

	// Token: 0x04005A36 RID: 23094
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DialogResponseList responseWindow;
}
