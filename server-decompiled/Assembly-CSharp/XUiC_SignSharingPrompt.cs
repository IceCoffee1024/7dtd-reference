using System;
using UnityEngine.Scripting;

// Token: 0x0200106C RID: 4204
[Preserve]
public class XUiC_SignSharingPrompt : XUiController
{
	// Token: 0x17000FA0 RID: 4000
	// (get) Token: 0x060084FD RID: 34045 RVA: 0x002DC697 File Offset: 0x002DA897
	public bool IsVisible
	{
		get
		{
			return this.viewComponent.IsVisible;
		}
	}

	// Token: 0x060084FE RID: 34046 RVA: 0x0032BFFC File Offset: 0x0032A1FC
	public override void Init()
	{
		base.Init();
		this.cbxSharing = base.GetChildById("cbxSharing").GetChildByType<XUiC_ComboBoxEnum<SignData.SignSharing>>();
		this.cbxSharing.OnValueChanged += this.CbxSharing_OnValueChanged;
		this.btnCancel = (XUiC_Button)base.GetChildById("btnPromptCancel");
		this.btnSave = (XUiC_Button)base.GetChildById("btnPromptSave");
		this.btnCancel.OnPress += this.BtnCancel_OnPress;
		this.btnSave.OnPress += this.BtnSave_OnPress;
		this.viewComponent.IsVisible = false;
	}

	// Token: 0x060084FF RID: 34047 RVA: 0x002DC6B8 File Offset: 0x002DA8B8
	public override void OnOpen()
	{
		base.OnOpen();
		this.viewComponent.IsVisible = false;
	}

	// Token: 0x06008500 RID: 34048 RVA: 0x0032C0A4 File Offset: 0x0032A2A4
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (XUiUtils.HotkeysAllowedFor(this.viewComponent, false) && this.xui.playerUI.playerInput.PermanentActions.Cancel.WasReleased)
		{
			ThreadManager.RunTaskAfterFrames(new Action(this.Cancel), 1);
		}
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x06008501 RID: 34049 RVA: 0x0032C100 File Offset: 0x0032A300
	public void ShowPrompt(SignData.SignSharing _currentSharing, Action<XUiC_SignSharingPrompt.Result, SignData.SignSharing> _callback)
	{
		this.previewedSharing = _currentSharing;
		this.cbxSharing.Value = _currentSharing;
		this.resultHandler = _callback;
		this.viewComponent.IsVisible = true;
		this.xui.playerUI.CursorController.SetNavigationLockView(this.viewComponent, this.btnCancel.ViewComponent);
		this.IsDirty = true;
	}

	// Token: 0x06008502 RID: 34050 RVA: 0x0032C160 File Offset: 0x0032A360
	public void Save()
	{
		if (!this.IsVisible)
		{
			return;
		}
		this.ClosePrompt();
		Action<XUiC_SignSharingPrompt.Result, SignData.SignSharing> action = this.resultHandler;
		if (action == null)
		{
			return;
		}
		action(XUiC_SignSharingPrompt.Result.Saved, this.previewedSharing);
	}

	// Token: 0x06008503 RID: 34051 RVA: 0x0032C188 File Offset: 0x0032A388
	public void Cancel()
	{
		if (!this.IsVisible)
		{
			return;
		}
		this.ClosePrompt();
		Action<XUiC_SignSharingPrompt.Result, SignData.SignSharing> action = this.resultHandler;
		if (action == null)
		{
			return;
		}
		action(XUiC_SignSharingPrompt.Result.Cancelled, this.previewedSharing);
	}

	// Token: 0x06008504 RID: 34052 RVA: 0x0032C1B0 File Offset: 0x0032A3B0
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnSave_OnPress(XUiController _sender, int _mouseButton)
	{
		this.Save();
	}

	// Token: 0x06008505 RID: 34053 RVA: 0x0032C1B8 File Offset: 0x0032A3B8
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancel_OnPress(XUiController _sender, int _mouseButton)
	{
		this.Cancel();
	}

	// Token: 0x06008506 RID: 34054 RVA: 0x0032C1C0 File Offset: 0x0032A3C0
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxSharing_OnValueChanged(XUiController _sender, SignData.SignSharing _oldValue, SignData.SignSharing _newValue)
	{
		this.previewedSharing = _newValue;
		this.IsDirty = true;
	}

	// Token: 0x06008507 RID: 34055 RVA: 0x002DC7C6 File Offset: 0x002DA9C6
	[PublicizedFrom(EAccessModifier.Private)]
	public void ClosePrompt()
	{
		this.xui.playerUI.CursorController.SetNavigationLockView(null, null);
		this.viewComponent.IsVisible = false;
	}

	// Token: 0x06008508 RID: 34056 RVA: 0x0032C1D0 File Offset: 0x0032A3D0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		if (bindingName == "sharingdescription")
		{
			value = Localization.Get("lblSignSharingDesc" + this.previewedSharing.ToStringCached<SignData.SignSharing>(), false, null);
			return true;
		}
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x040062B2 RID: 25266
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxEnum<SignData.SignSharing> cbxSharing;

	// Token: 0x040062B3 RID: 25267
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Button btnCancel;

	// Token: 0x040062B4 RID: 25268
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Button btnSave;

	// Token: 0x040062B5 RID: 25269
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData.SignSharing previewedSharing;

	// Token: 0x040062B6 RID: 25270
	[PublicizedFrom(EAccessModifier.Private)]
	public Action<XUiC_SignSharingPrompt.Result, SignData.SignSharing> resultHandler;

	// Token: 0x0200106D RID: 4205
	public enum Result
	{
		// Token: 0x040062B8 RID: 25272
		Cancelled,
		// Token: 0x040062B9 RID: 25273
		Saved
	}
}
