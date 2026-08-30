using System;
using SandboxOptions;
using UnityEngine.Scripting;

// Token: 0x02000E43 RID: 3651
[Preserve]
public class XUiC_SandboxSettingsSaveAsPreset : XUiController
{
	// Token: 0x17000CB0 RID: 3248
	// (get) Token: 0x0600704B RID: 28747 RVA: 0x002B3835 File Offset: 0x002B1A35
	// (set) Token: 0x0600704C RID: 28748 RVA: 0x002B3846 File Offset: 0x002B1A46
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

	// Token: 0x17000CB1 RID: 3249
	// (get) Token: 0x0600704D RID: 28749 RVA: 0x002B3865 File Offset: 0x002B1A65
	// (set) Token: 0x0600704E RID: 28750 RVA: 0x002B386D File Offset: 0x002B1A6D
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

	// Token: 0x17000CB2 RID: 3250
	// (get) Token: 0x0600704F RID: 28751 RVA: 0x002B3887 File Offset: 0x002B1A87
	// (set) Token: 0x06007050 RID: 28752 RVA: 0x002B3898 File Offset: 0x002B1A98
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

	// Token: 0x06007051 RID: 28753 RVA: 0x002B38B7 File Offset: 0x002B1AB7
	public override void Init()
	{
		base.Init();
		this.txtPresetName.UIInput.onValidate = new UIInput.OnValidate(GameUtils.ValidateGameNameInput);
	}

	// Token: 0x06007052 RID: 28754 RVA: 0x002B38DB File Offset: 0x002B1ADB
	[XuiBindEvent("OnPress", "btnCancel")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancelCreate_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.onConfirm = null;
	}

	// Token: 0x06007053 RID: 28755 RVA: 0x002B3900 File Offset: 0x002B1B00
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

	// Token: 0x06007054 RID: 28756 RVA: 0x002B39B6 File Offset: 0x002B1BB6
	[XuiBindEvent("OnSubmitHandler", "txtPresetName")]
	[XuiBindEvent("OnSubmitHandler", "txtDescription")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void CreateProfileName_OnSubmitHandler(XUiController _sender, string _text)
	{
		this.BtnConfirmCreate_OnPressed(this, -1);
	}

	// Token: 0x06007055 RID: 28757 RVA: 0x002B39C0 File Offset: 0x002B1BC0
	[XuiBindEvent("OnChangeHandler", "txtPresetName")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void CreateProfileName_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		string text = _text.Trim();
		this.NameValid = text.Length > 0 && text.IndexOf('.') < 0 && this.sandboxManager.GetPreset(text) == null;
	}

	// Token: 0x06007056 RID: 28758 RVA: 0x002B3A00 File Offset: 0x002B1C00
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

	// Token: 0x06007057 RID: 28759 RVA: 0x002B3A8C File Offset: 0x002B1C8C
	public override void OnOpen()
	{
		base.OnOpen();
		this.NameValid = false;
		this.txtPresetName.Text = "";
		this.txtDescription.Text = this.Description ?? "";
		this.txtPresetName.SelectOrVirtualKeyboard(true);
	}

	// Token: 0x06007058 RID: 28760 RVA: 0x002B3ADC File Offset: 0x002B1CDC
	public override void OnClose()
	{
		base.OnClose();
		this.Description = "";
	}

	// Token: 0x06007059 RID: 28761 RVA: 0x002B3AF0 File Offset: 0x002B1CF0
	public static void Open(XUi _xui, string _sandboxCode, string _description, Action<string> _onConfirm = null)
	{
		XUiC_SandboxSettingsSaveAsPreset childByType = _xui.GetChildByType<XUiC_SandboxSettingsSaveAsPreset>();
		childByType.SandboxCode = _sandboxCode;
		childByType.Description = _description;
		childByType.onConfirm = _onConfirm;
		_xui.playerUI.windowManager.Open(childByType.windowGroup, false);
	}

	// Token: 0x040054D3 RID: 21715
	[XuiBindComponent("presetName", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtPresetName;

	// Token: 0x040054D4 RID: 21716
	[XuiBindComponent("description", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtDescription;

	// Token: 0x040054D5 RID: 21717
	[XuiBindComponent("btnConfirm", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnConfirm;

	// Token: 0x040054D6 RID: 21718
	[XuiBindComponent("btnCancel", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCancel;

	// Token: 0x040054D7 RID: 21719
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SandboxOptionManager sandboxManager = SandboxOptionManager.Current;

	// Token: 0x040054D8 RID: 21720
	[PublicizedFrom(EAccessModifier.Private)]
	public Action<string> onConfirm;

	// Token: 0x040054D9 RID: 21721
	[PublicizedFrom(EAccessModifier.Private)]
	public string sandboxCode;

	// Token: 0x040054DA RID: 21722
	[PublicizedFrom(EAccessModifier.Private)]
	public bool nameValid;

	// Token: 0x040054DB RID: 21723
	[PublicizedFrom(EAccessModifier.Private)]
	public string description;
}
