using System;
using UnityEngine.Scripting;

// Token: 0x02000F1B RID: 3867
[Preserve]
public class XUiC_ConfirmationPrompt : XUiController
{
	// Token: 0x17000E37 RID: 3639
	// (get) Token: 0x0600788F RID: 30863 RVA: 0x002DC633 File Offset: 0x002DA833
	// (set) Token: 0x06007890 RID: 30864 RVA: 0x002DC63B File Offset: 0x002DA83B
	[XuiXmlBinding("headertext")]
	public string HeaderText
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "";

	// Token: 0x17000E38 RID: 3640
	// (get) Token: 0x06007891 RID: 30865 RVA: 0x002DC644 File Offset: 0x002DA844
	// (set) Token: 0x06007892 RID: 30866 RVA: 0x002DC64C File Offset: 0x002DA84C
	[XuiXmlBinding("bodytext")]
	public string BodyText
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "";

	// Token: 0x17000E39 RID: 3641
	// (get) Token: 0x06007893 RID: 30867 RVA: 0x002DC655 File Offset: 0x002DA855
	// (set) Token: 0x06007894 RID: 30868 RVA: 0x002DC65D File Offset: 0x002DA85D
	[XuiXmlBinding("canceltext")]
	public string CancelText
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "";

	// Token: 0x17000E3A RID: 3642
	// (get) Token: 0x06007895 RID: 30869 RVA: 0x002DC666 File Offset: 0x002DA866
	// (set) Token: 0x06007896 RID: 30870 RVA: 0x002DC66E File Offset: 0x002DA86E
	[XuiXmlBinding("confirmtext")]
	public string ConfirmText
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = "";

	// Token: 0x17000E3B RID: 3643
	// (get) Token: 0x06007897 RID: 30871 RVA: 0x002DC677 File Offset: 0x002DA877
	[XuiXmlBinding("cancelvisible")]
	public bool CancelVisible
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return !string.IsNullOrWhiteSpace(this.CancelText);
		}
	}

	// Token: 0x17000E3C RID: 3644
	// (get) Token: 0x06007898 RID: 30872 RVA: 0x002DC687 File Offset: 0x002DA887
	[XuiXmlBinding("confirmvisible")]
	public bool ConfirmVisible
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return !string.IsNullOrWhiteSpace(this.ConfirmText);
		}
	}

	// Token: 0x17000E3D RID: 3645
	// (get) Token: 0x06007899 RID: 30873 RVA: 0x002DC697 File Offset: 0x002DA897
	public bool IsVisible
	{
		get
		{
			return this.viewComponent.IsVisible;
		}
	}

	// Token: 0x0600789A RID: 30874 RVA: 0x002DC6A4 File Offset: 0x002DA8A4
	public override void Init()
	{
		base.Init();
		this.viewComponent.IsVisible = false;
	}

	// Token: 0x0600789B RID: 30875 RVA: 0x002DC6B8 File Offset: 0x002DA8B8
	public override void OnOpen()
	{
		base.OnOpen();
		this.viewComponent.IsVisible = false;
	}

	// Token: 0x0600789C RID: 30876 RVA: 0x002DC6CC File Offset: 0x002DA8CC
	[XuiBindEvent("OnPress", "btnConfirm")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnConfirm_OnPress(XUiController _sender, int _mouseButton)
	{
		this.Confirm();
	}

	// Token: 0x0600789D RID: 30877 RVA: 0x002DC6D4 File Offset: 0x002DA8D4
	[XuiBindEvent("OnPress", "btnCancel")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancel_OnPress(XUiController _sender, int _mouseButton)
	{
		this.Cancel();
	}

	// Token: 0x0600789E RID: 30878 RVA: 0x002DC6DC File Offset: 0x002DA8DC
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (XUiUtils.HotkeysAllowedFor(this.viewComponent, false) && this.xui.playerUI.playerInput.PermanentActions.Cancel.WasReleased)
		{
			ThreadManager.RunTaskAfterFrames(new Action(this.Cancel), 1);
		}
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x0600789F RID: 30879 RVA: 0x002DC738 File Offset: 0x002DA938
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

	// Token: 0x060078A0 RID: 30880 RVA: 0x002DC7C6 File Offset: 0x002DA9C6
	[PublicizedFrom(EAccessModifier.Private)]
	public void closePrompt()
	{
		this.xui.playerUI.CursorController.SetNavigationLockView(null, null);
		this.viewComponent.IsVisible = false;
	}

	// Token: 0x060078A1 RID: 30881 RVA: 0x002DC7EB File Offset: 0x002DA9EB
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

	// Token: 0x060078A2 RID: 30882 RVA: 0x002DC80D File Offset: 0x002DAA0D
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

	// Token: 0x04005973 RID: 22899
	[XuiBindComponent("btnPromptCancel", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCancel;

	// Token: 0x04005974 RID: 22900
	[XuiBindComponent("btnPromptConfirm", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnConfirm;

	// Token: 0x04005975 RID: 22901
	[PublicizedFrom(EAccessModifier.Private)]
	public Action<XUiC_ConfirmationPrompt.Result> resultHandler;

	// Token: 0x02000F1C RID: 3868
	public enum Result
	{
		// Token: 0x0400597B RID: 22907
		Cancelled,
		// Token: 0x0400597C RID: 22908
		Confirmed
	}
}
