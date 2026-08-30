using System;
using SandboxOptions;
using UnityEngine.Scripting;

// Token: 0x02000E2E RID: 3630
[Preserve]
public class XUiC_SandboxSettingsSaveAsPreset : XUiController
{
	// Token: 0x17000C97 RID: 3223
	// (get) Token: 0x06006FAB RID: 28587 RVA: 0x002B0311 File Offset: 0x002AE511
	// (set) Token: 0x06006FAC RID: 28588 RVA: 0x002B0322 File Offset: 0x002AE522
	[XuiXmlAttribute("sandboxcode", false)]
	[XuiXmlBinding("sandboxcode")]
	public string SandboxCode
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.sandboxCode ?? "";
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (string.Equals(value, this.sandboxCode))
			{
				return;
			}
			this.sandboxCode = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000C98 RID: 3224
	// (get) Token: 0x06006FAD RID: 28589 RVA: 0x002B0341 File Offset: 0x002AE541
	// (set) Token: 0x06006FAE RID: 28590 RVA: 0x002B0349 File Offset: 0x002AE549
	[XuiXmlBinding("name_valid")]
	public bool NameValid
	{
		get
		{
			return this.nameValid;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (this.nameValid == value)
			{
				return;
			}
			this.nameValid = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000C99 RID: 3225
	// (get) Token: 0x06006FAF RID: 28591 RVA: 0x002B0363 File Offset: 0x002AE563
	// (set) Token: 0x06006FB0 RID: 28592 RVA: 0x002B0374 File Offset: 0x002AE574
	public string Description
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.description ?? "";
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (string.Equals(value, this.description))
			{
				return;
			}
			this.description = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x06006FB1 RID: 28593 RVA: 0x002B0393 File Offset: 0x002AE593
	public override void Init()
	{
		base.Init();
		this.txtPresetName.UIInput.onValidate = new UIInput.OnValidate(GameUtils.ValidateGameNameInput);
	}

	// Token: 0x06006FB2 RID: 28594 RVA: 0x002B03B7 File Offset: 0x002AE5B7
	[XuiBindEvent("OnPress", "btnCancel")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancelCreate_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.onConfirm = null;
	}

	// Token: 0x06006FB3 RID: 28595 RVA: 0x002B03DC File Offset: 0x002AE5DC
	[XuiBindEvent("OnPress", "btnConfirm")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnConfirmCreate_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (!this.NameValid)
		{
			return;
		}
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		SandboxOptionPreset sandboxOptionPreset = new SandboxOptionPreset
		{
			Name = this.txtPresetName.Text,
			Description = this.txtDescription.Text,
			IsUserPreset = true,
			Group = "User",
			Icon = "Data/Sandbox/icons/user_custom"
		};
		this.sandboxManager.LoadOptionsFromCode(this.SandboxCode, sandboxOptionPreset);
		this.sandboxManager.SavePreset(sandboxOptionPreset, true, true);
		Action<string> action = this.onConfirm;
		if (action != null)
		{
			action(sandboxOptionPreset.Name);
		}
		this.onConfirm = null;
	}

	// Token: 0x06006FB4 RID: 28596 RVA: 0x002B0492 File Offset: 0x002AE692
	[XuiBindEvent("OnSubmitHandler", "txtPresetName")]
	[XuiBindEvent("OnSubmitHandler", "txtDescription")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void CreateProfileName_OnSubmitHandler(XUiController _sender, string _text)
	{
		this.BtnConfirmCreate_OnPressed(this, -1);
	}

	// Token: 0x06006FB5 RID: 28597 RVA: 0x002B049C File Offset: 0x002AE69C
	[XuiBindEvent("OnChangeHandler", "txtPresetName")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void CreateProfileName_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		string text = _text.Trim();
		this.NameValid = text.Length > 0 && text.IndexOf('.') < 0 && this.sandboxManager.GetPreset(text) == null;
	}

	// Token: 0x06006FB6 RID: 28598 RVA: 0x002B04DC File Offset: 0x002AE6DC
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (XUiUtils.HotkeysAllowedFor(this.viewComponent, false))
		{
			if (this.xui.playerUI.playerInput.PermanentActions.Cancel.WasReleased)
			{
				this.BtnCancelCreate_OnPressed(this, -1);
			}
			if (this.xui.playerUI.playerInput.GUIActions.Apply.WasReleased)
			{
				this.BtnConfirmCreate_OnPressed(null, 0);
			}
		}
		if (this.IsDirty)
		{
			this.IsDirty = false;
			base.RefreshBindings();
		}
	}

	// Token: 0x06006FB7 RID: 28599 RVA: 0x002B0568 File Offset: 0x002AE768
	public override void OnOpen()
	{
		base.OnOpen();
		this.NameValid = false;
		this.txtPresetName.Text = "";
		this.txtDescription.Text = this.Description ?? "";
		this.txtPresetName.SelectOrVirtualKeyboard(true);
	}

	// Token: 0x06006FB8 RID: 28600 RVA: 0x002B05B8 File Offset: 0x002AE7B8
	public override void OnClose()
	{
		base.OnClose();
		this.Description = "";
	}

	// Token: 0x06006FB9 RID: 28601 RVA: 0x002B05CC File Offset: 0x002AE7CC
	public static void Open(XUi _xui, string _sandboxCode, string _description, Action<string> _onConfirm = null)
	{
		XUiC_SandboxSettingsSaveAsPreset childByType = _xui.GetChildByType<XUiC_SandboxSettingsSaveAsPreset>();
		childByType.SandboxCode = _sandboxCode;
		childByType.Description = _description;
		childByType.onConfirm = _onConfirm;
		_xui.playerUI.windowManager.Open(childByType.windowGroup, false);
	}

	// Token: 0x0400542F RID: 21551
	[XuiBindComponent("presetName", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtPresetName;

	// Token: 0x04005430 RID: 21552
	[XuiBindComponent("description", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtDescription;

	// Token: 0x04005431 RID: 21553
	[XuiBindComponent("btnConfirm", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnConfirm;

	// Token: 0x04005432 RID: 21554
	[XuiBindComponent("btnCancel", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCancel;

	// Token: 0x04005433 RID: 21555
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SandboxOptionManager sandboxManager = SandboxOptionManager.Current;

	// Token: 0x04005434 RID: 21556
	[PublicizedFrom(EAccessModifier.Private)]
	public Action<string> onConfirm;

	// Token: 0x04005435 RID: 21557
	[PublicizedFrom(EAccessModifier.Private)]
	public string sandboxCode;

	// Token: 0x04005436 RID: 21558
	[PublicizedFrom(EAccessModifier.Private)]
	public bool nameValid;

	// Token: 0x04005437 RID: 21559
	[PublicizedFrom(EAccessModifier.Private)]
	public string description;
}
