using System;
using UnityEngine.Scripting;

// Token: 0x02000F44 RID: 3908
[Preserve]
public class XUiC_DialogWindowGroup : XUiController
{
	// Token: 0x17000E60 RID: 3680
	// (get) Token: 0x060079F4 RID: 31220 RVA: 0x002E2866 File Offset: 0x002E0A66
	// (set) Token: 0x060079F5 RID: 31221 RVA: 0x002E286E File Offset: 0x002E0A6E
	public Dialog CurrentDialog
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x060079F6 RID: 31222 RVA: 0x002E2878 File Offset: 0x002E0A78
	public static void Open(XUi _xui, Action _onDialogClosed = null)
	{
		_xui.playerUI.windowManager.Open("dialog", true);
		if (_onDialogClosed != null)
		{
			GUIWindow window = _xui.playerUI.windowManager.GetWindow("dialog");
			window.OnWindowClose = (Action)Delegate.Combine(window.OnWindowClose, _onDialogClosed);
		}
	}

	// Token: 0x060079F7 RID: 31223 RVA: 0x002E28C9 File Offset: 0x002E0AC9
	public override void Init()
	{
		base.Init();
		this.responseWindow = base.GetChildByType<XUiC_DialogResponseList>();
	}

	// Token: 0x060079F8 RID: 31224 RVA: 0x002D7387 File Offset: 0x002D5587
	public override void Update(float _dt)
	{
		base.Update(_dt);
	}

	// Token: 0x060079F9 RID: 31225 RVA: 0x002E28E0 File Offset: 0x002E0AE0
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

	// Token: 0x060079FA RID: 31226 RVA: 0x002E2A98 File Offset: 0x002E0C98
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

	// Token: 0x060079FB RID: 31227 RVA: 0x002E2B38 File Offset: 0x002E0D38
	public void RefreshDialog()
	{
		if (this.CurrentDialog.CurrentStatement != null)
		{
			this.responseWindow.Refresh();
			return;
		}
		this.xui.playerUI.windowManager.Close("dialog");
	}

	// Token: 0x060079FC RID: 31228 RVA: 0x002E2B6D File Offset: 0x002E0D6D
	public void ShowResponseWindow(bool isVisible)
	{
		this.responseWindow.ViewComponent.IsVisible = isVisible;
	}

	// Token: 0x04005A69 RID: 23145
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DialogResponseList responseWindow;
}
