using System;
using UnityEngine.Scripting;

// Token: 0x02000F01 RID: 3841
[Preserve]
public class XUiC_ConfirmationPrompt : XUiController
{
	// Token: 0x17000E16 RID: 3606
	// (get) Token: 0x0600786E RID: 30830 RVA: 0x002D7D47 File Offset: 0x002D5F47
	// (set) Token: 0x0600786F RID: 30831 RVA: 0x002D7D4F File Offset: 0x002D5F4F
	[XuiXmlBinding("headertext")]
	public string HeaderText
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "";

	// Token: 0x17000E17 RID: 3607
	// (get) Token: 0x06007870 RID: 30832 RVA: 0x002D7D58 File Offset: 0x002D5F58
	// (set) Token: 0x06007871 RID: 30833 RVA: 0x002D7D60 File Offset: 0x002D5F60
	[XuiXmlBinding("bodytext")]
	public string BodyText
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "";

	// Token: 0x17000E18 RID: 3608
	// (get) Token: 0x06007872 RID: 30834 RVA: 0x002D7D69 File Offset: 0x002D5F69
	// (set) Token: 0x06007873 RID: 30835 RVA: 0x002D7D71 File Offset: 0x002D5F71
	[XuiXmlBinding("canceltext")]
	public string CancelText
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "";

	// Token: 0x17000E19 RID: 3609
	// (get) Token: 0x06007874 RID: 30836 RVA: 0x002D7D7A File Offset: 0x002D5F7A
	// (set) Token: 0x06007875 RID: 30837 RVA: 0x002D7D82 File Offset: 0x002D5F82
	[XuiXmlBinding("confirmtext")]
	public string ConfirmText
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "";

	// Token: 0x17000E1A RID: 3610
	// (get) Token: 0x06007876 RID: 30838 RVA: 0x002D7D8B File Offset: 0x002D5F8B
	[XuiXmlBinding("cancelvisible")]
	public bool CancelVisible
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return !string.IsNullOrWhiteSpace(this.CancelText);
		}
	}

	// Token: 0x17000E1B RID: 3611
	// (get) Token: 0x06007877 RID: 30839 RVA: 0x002D7D9B File Offset: 0x002D5F9B
	[XuiXmlBinding("confirmvisible")]
	public bool ConfirmVisible
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return !string.IsNullOrWhiteSpace(this.ConfirmText);
		}
	}

	// Token: 0x17000E1C RID: 3612
	// (get) Token: 0x06007878 RID: 30840 RVA: 0x002D7DAB File Offset: 0x002D5FAB
	public bool IsVisible
	{
		get
		{
			return this.viewComponent.IsVisible;
		}
	}

	// Token: 0x06007879 RID: 30841 RVA: 0x002D7DB8 File Offset: 0x002D5FB8
	public override void Init()
	{
		base.Init();
		this.viewComponent.IsVisible = false;
	}

	// Token: 0x0600787A RID: 30842 RVA: 0x002D7DCC File Offset: 0x002D5FCC
	public override void OnOpen()
	{
		base.OnOpen();
		this.viewComponent.IsVisible = false;
	}

	// Token: 0x0600787B RID: 30843 RVA: 0x002D7DE0 File Offset: 0x002D5FE0
	[XuiBindEvent("OnPress", "btnConfirm")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnConfirm_OnPress(XUiController _sender, int _mouseButton)
	{
		this.Confirm();
	}

	// Token: 0x0600787C RID: 30844 RVA: 0x002D7DE8 File Offset: 0x002D5FE8
	[XuiBindEvent("OnPress", "btnCancel")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancel_OnPress(XUiController _sender, int _mouseButton)
	{
		this.Cancel();
	}

	// Token: 0x0600787D RID: 30845 RVA: 0x002D7DF0 File Offset: 0x002D5FF0
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (XUiUtils.HotkeysAllowedFor(this.viewComponent, false) && this.xui.playerUI.playerInput.PermanentActions.Cancel.WasReleased)
		{
			ThreadManager.RunTaskAfterFrames(new Action(this.Cancel), 1);
		}
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x0600787E RID: 30846 RVA: 0x002D7E4C File Offset: 0x002D604C
	public void ShowPrompt(string _headerText, string _bodyText, string _cancelText, string _confirmText, Action<XUiC_ConfirmationPrompt.Result> _callback)
	{
		this.HeaderText = _headerText;
		this.BodyText = _bodyText;
		this.CancelText = _cancelText;
		this.ConfirmText = _confirmText;
		this.resultHandler = _callback;
		this.viewComponent.IsVisible = true;
		XUiView xuiView = ((!string.IsNullOrWhiteSpace(_cancelText)) ? this.btnCancel.ViewComponent : ((!string.IsNullOrWhiteSpace(_confirmText)) ? this.btnConfirm.ViewComponent : null));
		this.xui.playerUI.CursorController.SetNavigationLockView(this.viewComponent, xuiView);
		this.IsDirty = true;
	}

	// Token: 0x0600787F RID: 30847 RVA: 0x002D7EDA File Offset: 0x002D60DA
	[PublicizedFrom(EAccessModifier.Private)]
	public void closePrompt()
	{
		this.xui.playerUI.CursorController.SetNavigationLockView(null, null);
		this.viewComponent.IsVisible = false;
	}

	// Token: 0x06007880 RID: 30848 RVA: 0x002D7EFF File Offset: 0x002D60FF
	public void Confirm()
	{
		if (!this.IsVisible)
		{
			return;
		}
		this.closePrompt();
		Action<XUiC_ConfirmationPrompt.Result> action = this.resultHandler;
		if (action == null)
		{
			return;
		}
		action(XUiC_ConfirmationPrompt.Result.Confirmed);
	}

	// Token: 0x06007881 RID: 30849 RVA: 0x002D7F21 File Offset: 0x002D6121
	public void Cancel()
	{
		if (!this.IsVisible)
		{
			return;
		}
		this.closePrompt();
		Action<XUiC_ConfirmationPrompt.Result> action = this.resultHandler;
		if (action == null)
		{
			return;
		}
		action(XUiC_ConfirmationPrompt.Result.Cancelled);
	}

	// Token: 0x04005916 RID: 22806
	[XuiBindComponent("btnPromptCancel", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCancel;

	// Token: 0x04005917 RID: 22807
	[XuiBindComponent("btnPromptConfirm", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnConfirm;

	// Token: 0x04005918 RID: 22808
	[PublicizedFrom(EAccessModifier.Private)]
	public Action<XUiC_ConfirmationPrompt.Result> resultHandler;

	// Token: 0x02000F02 RID: 3842
	public enum Result
	{
		// Token: 0x0400591E RID: 22814
		Cancelled,
		// Token: 0x0400591F RID: 22815
		Confirmed
	}
}
