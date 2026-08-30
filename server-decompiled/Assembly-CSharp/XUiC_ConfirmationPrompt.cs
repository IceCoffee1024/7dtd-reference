using System;
using UnityEngine.Scripting;

// Token: 0x02000EEA RID: 3818
[Preserve]
public class XUiC_ConfirmationPrompt : XUiController
{
	// Token: 0x17000DFC RID: 3580
	// (get) Token: 0x060077B6 RID: 30646 RVA: 0x002D3DBB File Offset: 0x002D1FBB
	// (set) Token: 0x060077B7 RID: 30647 RVA: 0x002D3DC3 File Offset: 0x002D1FC3
	[XuiXmlBinding("headertext")]
	public string HeaderText
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "";

	// Token: 0x17000DFD RID: 3581
	// (get) Token: 0x060077B8 RID: 30648 RVA: 0x002D3DCC File Offset: 0x002D1FCC
	// (set) Token: 0x060077B9 RID: 30649 RVA: 0x002D3DD4 File Offset: 0x002D1FD4
	[XuiXmlBinding("bodytext")]
	public string BodyText
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "";

	// Token: 0x17000DFE RID: 3582
	// (get) Token: 0x060077BA RID: 30650 RVA: 0x002D3DDD File Offset: 0x002D1FDD
	// (set) Token: 0x060077BB RID: 30651 RVA: 0x002D3DE5 File Offset: 0x002D1FE5
	[XuiXmlBinding("canceltext")]
	public string CancelText
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "";

	// Token: 0x17000DFF RID: 3583
	// (get) Token: 0x060077BC RID: 30652 RVA: 0x002D3DEE File Offset: 0x002D1FEE
	// (set) Token: 0x060077BD RID: 30653 RVA: 0x002D3DF6 File Offset: 0x002D1FF6
	[XuiXmlBinding("confirmtext")]
	public string ConfirmText
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "";

	// Token: 0x17000E00 RID: 3584
	// (get) Token: 0x060077BE RID: 30654 RVA: 0x002D3DFF File Offset: 0x002D1FFF
	[XuiXmlBinding("cancelvisible")]
	public bool CancelVisible
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return !string.IsNullOrWhiteSpace(this.CancelText);
		}
	}

	// Token: 0x17000E01 RID: 3585
	// (get) Token: 0x060077BF RID: 30655 RVA: 0x002D3E0F File Offset: 0x002D200F
	[XuiXmlBinding("confirmvisible")]
	public bool ConfirmVisible
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return !string.IsNullOrWhiteSpace(this.ConfirmText);
		}
	}

	// Token: 0x17000E02 RID: 3586
	// (get) Token: 0x060077C0 RID: 30656 RVA: 0x002D3E1F File Offset: 0x002D201F
	public bool IsVisible
	{
		get
		{
			return this.viewComponent.IsVisible;
		}
	}

	// Token: 0x060077C1 RID: 30657 RVA: 0x002D3E2C File Offset: 0x002D202C
	public override void Init()
	{
		base.Init();
		this.viewComponent.IsVisible = false;
	}

	// Token: 0x060077C2 RID: 30658 RVA: 0x002D3E40 File Offset: 0x002D2040
	public override void OnOpen()
	{
		base.OnOpen();
		this.viewComponent.IsVisible = false;
	}

	// Token: 0x060077C3 RID: 30659 RVA: 0x002D3E54 File Offset: 0x002D2054
	[XuiBindEvent("OnPress", "btnConfirm")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnConfirm_OnPress(XUiController _sender, int _mouseButton)
	{
		this.Confirm();
	}

	// Token: 0x060077C4 RID: 30660 RVA: 0x002D3E5C File Offset: 0x002D205C
	[XuiBindEvent("OnPress", "btnCancel")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancel_OnPress(XUiController _sender, int _mouseButton)
	{
		this.Cancel();
	}

	// Token: 0x060077C5 RID: 30661 RVA: 0x002D3E64 File Offset: 0x002D2064
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (XUiUtils.HotkeysAllowedFor(this.viewComponent, false) && this.xui.playerUI.playerInput.PermanentActions.Cancel.WasReleased)
		{
			ThreadManager.RunTaskAfterFrames(new Action(this.Cancel), 1);
		}
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x060077C6 RID: 30662 RVA: 0x002D3EC0 File Offset: 0x002D20C0
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

	// Token: 0x060077C7 RID: 30663 RVA: 0x002D3F4E File Offset: 0x002D214E
	[PublicizedFrom(EAccessModifier.Private)]
	public void closePrompt()
	{
		this.xui.playerUI.CursorController.SetNavigationLockView(null, null);
		this.viewComponent.IsVisible = false;
	}

	// Token: 0x060077C8 RID: 30664 RVA: 0x002D3F73 File Offset: 0x002D2173
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

	// Token: 0x060077C9 RID: 30665 RVA: 0x002D3F95 File Offset: 0x002D2195
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

	// Token: 0x04005864 RID: 22628
	[XuiBindComponent("btnPromptCancel", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCancel;

	// Token: 0x04005865 RID: 22629
	[XuiBindComponent("btnPromptConfirm", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnConfirm;

	// Token: 0x04005866 RID: 22630
	[PublicizedFrom(EAccessModifier.Private)]
	public Action<XUiC_ConfirmationPrompt.Result> resultHandler;

	// Token: 0x02000EEB RID: 3819
	public enum Result
	{
		// Token: 0x0400586C RID: 22636
		Cancelled,
		// Token: 0x0400586D RID: 22637
		Confirmed
	}
}
