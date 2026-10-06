using System;
using UnityEngine.Scripting;

// Token: 0x02000F10 RID: 3856
[Preserve]
public class XUiC_ConfirmationPrompt : XUiController
{
	// Token: 0x17000E33 RID: 3635
	// (get) Token: 0x06007869 RID: 30825 RVA: 0x002DB0DB File Offset: 0x002D92DB
	// (set) Token: 0x0600786A RID: 30826 RVA: 0x002DB0E3 File Offset: 0x002D92E3
	[XuiXmlBinding("headertext")]
	public string HeaderText
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "";

	// Token: 0x17000E34 RID: 3636
	// (get) Token: 0x0600786B RID: 30827 RVA: 0x002DB0EC File Offset: 0x002D92EC
	// (set) Token: 0x0600786C RID: 30828 RVA: 0x002DB0F4 File Offset: 0x002D92F4
	[XuiXmlBinding("bodytext")]
	public string BodyText
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "";

	// Token: 0x17000E35 RID: 3637
	// (get) Token: 0x0600786D RID: 30829 RVA: 0x002DB0FD File Offset: 0x002D92FD
	// (set) Token: 0x0600786E RID: 30830 RVA: 0x002DB105 File Offset: 0x002D9305
	[XuiXmlBinding("canceltext")]
	public string CancelText
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "";

	// Token: 0x17000E36 RID: 3638
	// (get) Token: 0x0600786F RID: 30831 RVA: 0x002DB10E File Offset: 0x002D930E
	// (set) Token: 0x06007870 RID: 30832 RVA: 0x002DB116 File Offset: 0x002D9316
	[XuiXmlBinding("confirmtext")]
	public string ConfirmText
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "";

	// Token: 0x17000E37 RID: 3639
	// (get) Token: 0x06007871 RID: 30833 RVA: 0x002DB11F File Offset: 0x002D931F
	[XuiXmlBinding("cancelvisible")]
	public bool CancelVisible
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return !string.IsNullOrWhiteSpace(this.CancelText);
		}
	}

	// Token: 0x17000E38 RID: 3640
	// (get) Token: 0x06007872 RID: 30834 RVA: 0x002DB12F File Offset: 0x002D932F
	[XuiXmlBinding("confirmvisible")]
	public bool ConfirmVisible
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return !string.IsNullOrWhiteSpace(this.ConfirmText);
		}
	}

	// Token: 0x17000E39 RID: 3641
	// (get) Token: 0x06007873 RID: 30835 RVA: 0x002DB13F File Offset: 0x002D933F
	public bool IsVisible
	{
		get
		{
			return this.viewComponent.IsVisible;
		}
	}

	// Token: 0x06007874 RID: 30836 RVA: 0x002DB14C File Offset: 0x002D934C
	public override void Init()
	{
		base.Init();
		this.viewComponent.IsVisible = false;
	}

	// Token: 0x06007875 RID: 30837 RVA: 0x002DB160 File Offset: 0x002D9360
	public override void OnOpen()
	{
		base.OnOpen();
		this.viewComponent.IsVisible = false;
	}

	// Token: 0x06007876 RID: 30838 RVA: 0x002DB174 File Offset: 0x002D9374
	[XuiBindEvent("OnPress", "btnConfirm")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnConfirm_OnPress(XUiController _sender, int _mouseButton)
	{
		this.Confirm();
	}

	// Token: 0x06007877 RID: 30839 RVA: 0x002DB17C File Offset: 0x002D937C
	[XuiBindEvent("OnPress", "btnCancel")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancel_OnPress(XUiController _sender, int _mouseButton)
	{
		this.Cancel();
	}

	// Token: 0x06007878 RID: 30840 RVA: 0x002DB184 File Offset: 0x002D9384
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (XUiUtils.HotkeysAllowedFor(this.viewComponent, false) && this.xui.playerUI.playerInput.PermanentActions.Cancel.WasReleased)
		{
			ThreadManager.RunTaskAfterFrames(new Action(this.Cancel), 1);
		}
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x06007879 RID: 30841 RVA: 0x002DB1E0 File Offset: 0x002D93E0
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

	// Token: 0x0600787A RID: 30842 RVA: 0x002DB26E File Offset: 0x002D946E
	[PublicizedFrom(EAccessModifier.Private)]
	public void closePrompt()
	{
		this.xui.playerUI.CursorController.SetNavigationLockView(null, null);
		this.viewComponent.IsVisible = false;
	}

	// Token: 0x0600787B RID: 30843 RVA: 0x002DB293 File Offset: 0x002D9493
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

	// Token: 0x0600787C RID: 30844 RVA: 0x002DB2B5 File Offset: 0x002D94B5
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

	// Token: 0x0400594E RID: 22862
	[XuiBindComponent("btnPromptCancel", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCancel;

	// Token: 0x0400594F RID: 22863
	[XuiBindComponent("btnPromptConfirm", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnConfirm;

	// Token: 0x04005950 RID: 22864
	[PublicizedFrom(EAccessModifier.Private)]
	public Action<XUiC_ConfirmationPrompt.Result> resultHandler;

	// Token: 0x02000F11 RID: 3857
	public enum Result
	{
		// Token: 0x04005956 RID: 22870
		Cancelled,
		// Token: 0x04005957 RID: 22871
		Confirmed
	}
}
