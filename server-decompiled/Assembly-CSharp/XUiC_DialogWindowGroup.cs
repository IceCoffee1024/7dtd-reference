using System;
using UnityEngine.Scripting;

// Token: 0x02000F26 RID: 3878
[Preserve]
public class XUiC_DialogWindowGroup : XUiController
{
	// Token: 0x17000E39 RID: 3641
	// (get) Token: 0x060079B6 RID: 31158 RVA: 0x002DDBDA File Offset: 0x002DBDDA
	// (set) Token: 0x060079B7 RID: 31159 RVA: 0x002DDBE2 File Offset: 0x002DBDE2
	public Dialog CurrentDialog
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x060079B8 RID: 31160 RVA: 0x002DDBEC File Offset: 0x002DBDEC
	public static void Open(XUi _xui, Action _onDialogClosed = null)
	{
		_xui.playerUI.windowManager.Open("dialog", true);
		if (_onDialogClosed != null)
		{
			GUIWindow window = _xui.playerUI.windowManager.GetWindow("dialog");
			window.OnWindowClose = (Action)Delegate.Combine(window.OnWindowClose, _onDialogClosed);
		}
	}

	// Token: 0x060079B9 RID: 31161 RVA: 0x002DDC3D File Offset: 0x002DBE3D
	public override void Init()
	{
		base.Init();
		this.responseWindow = base.GetChildByType<XUiC_DialogResponseList>();
	}

	// Token: 0x060079BA RID: 31162 RVA: 0x002D2ABB File Offset: 0x002D0CBB
	public override void Update(float _dt)
	{
		base.Update(_dt);
	}

	// Token: 0x060079BB RID: 31163 RVA: 0x002DDC54 File Offset: 0x002DBE54
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

	// Token: 0x060079BC RID: 31164 RVA: 0x002DDE0C File Offset: 0x002DC00C
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

	// Token: 0x060079BD RID: 31165 RVA: 0x002DDEAC File Offset: 0x002DC0AC
	public void RefreshDialog()
	{
		if (this.CurrentDialog.CurrentStatement != null)
		{
			this.responseWindow.Refresh();
			return;
		}
		this.xui.playerUI.windowManager.Close("dialog");
	}

	// Token: 0x060079BE RID: 31166 RVA: 0x002DDEE1 File Offset: 0x002DC0E1
	public void ShowResponseWindow(bool isVisible)
	{
		this.responseWindow.ViewComponent.IsVisible = isVisible;
	}

	// Token: 0x040059FE RID: 23038
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DialogResponseList responseWindow;
}
