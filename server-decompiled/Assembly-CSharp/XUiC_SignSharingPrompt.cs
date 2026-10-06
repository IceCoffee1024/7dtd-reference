using System;
using UnityEngine.Scripting;

// Token: 0x0200105C RID: 4188
[Preserve]
public class XUiC_SignSharingPrompt : XUiController
{
	// Token: 0x17000F95 RID: 3989
	// (get) Token: 0x060084AB RID: 33963 RVA: 0x002DB13F File Offset: 0x002D933F
	public bool IsVisible
	{
		get
		{
			return this.viewComponent.IsVisible;
		}
	}

	// Token: 0x060084AC RID: 33964 RVA: 0x0032A2F8 File Offset: 0x003284F8
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

	// Token: 0x060084AD RID: 33965 RVA: 0x002DB160 File Offset: 0x002D9360
	public override void OnOpen()
	{
		base.OnOpen();
		this.viewComponent.IsVisible = false;
	}

	// Token: 0x060084AE RID: 33966 RVA: 0x0032A3A0 File Offset: 0x003285A0
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (XUiUtils.HotkeysAllowedFor(this.viewComponent, false) && this.xui.playerUI.playerInput.PermanentActions.Cancel.WasReleased)
		{
			ThreadManager.RunTaskAfterFrames(new Action(this.Cancel), 1);
		}
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x060084AF RID: 33967 RVA: 0x0032A3FC File Offset: 0x003285FC
	public void ShowPrompt(SignData.SignSharing _currentSharing, Action<XUiC_SignSharingPrompt.Result, SignData.SignSharing> _callback)
	{
		this.previewedSharing = _currentSharing;
		this.cbxSharing.Value = _currentSharing;
		this.resultHandler = _callback;
		this.viewComponent.IsVisible = true;
		this.xui.playerUI.CursorController.SetNavigationLockView(this.viewComponent, this.btnCancel.ViewComponent);
		this.IsDirty = true;
	}

	// Token: 0x060084B0 RID: 33968 RVA: 0x0032A45C File Offset: 0x0032865C
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

	// Token: 0x060084B1 RID: 33969 RVA: 0x0032A484 File Offset: 0x00328684
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

	// Token: 0x060084B2 RID: 33970 RVA: 0x0032A4AC File Offset: 0x003286AC
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnSave_OnPress(XUiController _sender, int _mouseButton)
	{
		this.Save();
	}

	// Token: 0x060084B3 RID: 33971 RVA: 0x0032A4B4 File Offset: 0x003286B4
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancel_OnPress(XUiController _sender, int _mouseButton)
	{
		this.Cancel();
	}

	// Token: 0x060084B4 RID: 33972 RVA: 0x0032A4BC File Offset: 0x003286BC
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxSharing_OnValueChanged(XUiController _sender, SignData.SignSharing _oldValue, SignData.SignSharing _newValue)
	{
		this.previewedSharing = _newValue;
		this.IsDirty = true;
	}

	// Token: 0x060084B5 RID: 33973 RVA: 0x002DB26E File Offset: 0x002D946E
	[PublicizedFrom(EAccessModifier.Private)]
	public void ClosePrompt()
	{
		this.xui.playerUI.CursorController.SetNavigationLockView(null, null);
		this.viewComponent.IsVisible = false;
	}

	// Token: 0x060084B6 RID: 33974 RVA: 0x0032A4CC File Offset: 0x003286CC
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

	// Token: 0x04006274 RID: 25204
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxEnum<SignData.SignSharing> cbxSharing;

	// Token: 0x04006275 RID: 25205
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Button btnCancel;

	// Token: 0x04006276 RID: 25206
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Button btnSave;

	// Token: 0x04006277 RID: 25207
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData.SignSharing previewedSharing;

	// Token: 0x04006278 RID: 25208
	[PublicizedFrom(EAccessModifier.Private)]
	public Action<XUiC_SignSharingPrompt.Result, SignData.SignSharing> resultHandler;

	// Token: 0x0200105D RID: 4189
	public enum Result
	{
		// Token: 0x0400627A RID: 25210
		Cancelled,
		// Token: 0x0400627B RID: 25211
		Saved
	}
}
