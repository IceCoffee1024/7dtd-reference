using System;
using UnityEngine.Scripting;

// Token: 0x02000F0F RID: 3855
[Preserve]
public class XUiC_DialogWindowGroup : XUiController
{
	// Token: 0x17000E1F RID: 3615
	// (get) Token: 0x060078FE RID: 30974 RVA: 0x002D9C4E File Offset: 0x002D7E4E
	// (set) Token: 0x060078FF RID: 30975 RVA: 0x002D9C56 File Offset: 0x002D7E56
	public Dialog CurrentDialog
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06007900 RID: 30976 RVA: 0x002D9C60 File Offset: 0x002D7E60
	public static void Open(XUi _xui, Action _onDialogClosed = null)
	{
		_xui.playerUI.windowManager.Open("dialog", true);
		if (_onDialogClosed != null)
		{
			GUIWindow window = _xui.playerUI.windowManager.GetWindow("dialog");
			window.OnWindowClose = (Action)Delegate.Combine(window.OnWindowClose, _onDialogClosed);
		}
	}

	// Token: 0x06007901 RID: 30977 RVA: 0x002D9CB1 File Offset: 0x002D7EB1
	public override void Init()
	{
		base.Init();
		this.responseWindow = base.GetChildByType<XUiC_DialogResponseList>();
	}

	// Token: 0x06007902 RID: 30978 RVA: 0x002CF437 File Offset: 0x002CD637
	public override void Update(float _dt)
	{
		base.Update(_dt);
	}

	// Token: 0x06007903 RID: 30979 RVA: 0x002D9CC8 File Offset: 0x002D7EC8
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

	// Token: 0x06007904 RID: 30980 RVA: 0x002D9E80 File Offset: 0x002D8080
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

	// Token: 0x06007905 RID: 30981 RVA: 0x002D9F20 File Offset: 0x002D8120
	public void RefreshDialog()
	{
		if (this.CurrentDialog.CurrentStatement != null)
		{
			this.responseWindow.Refresh();
			return;
		}
		this.xui.playerUI.windowManager.Close("dialog");
	}

	// Token: 0x06007906 RID: 30982 RVA: 0x002D9F55 File Offset: 0x002D8155
	public void ShowResponseWindow(bool isVisible)
	{
		this.responseWindow.ViewComponent.IsVisible = isVisible;
	}

	// Token: 0x0400594C RID: 22860
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DialogResponseList responseWindow;
}
