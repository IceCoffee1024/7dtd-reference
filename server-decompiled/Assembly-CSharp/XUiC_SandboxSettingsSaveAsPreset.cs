using System;
using SandboxOptions;
using UnityEngine.Scripting;

// Token: 0x02000E5D RID: 3677
[Preserve]
public class XUiC_SandboxSettingsSaveAsPreset : XUiController
{
	// Token: 0x17000CD1 RID: 3281
	// (get) Token: 0x06007068 RID: 28776 RVA: 0x002B8151 File Offset: 0x002B6351
	// (set) Token: 0x06007069 RID: 28777 RVA: 0x002B8162 File Offset: 0x002B6362
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

	// Token: 0x17000CD2 RID: 3282
	// (get) Token: 0x0600706A RID: 28778 RVA: 0x002B8181 File Offset: 0x002B6381
	// (set) Token: 0x0600706B RID: 28779 RVA: 0x002B8189 File Offset: 0x002B6389
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

	// Token: 0x17000CD3 RID: 3283
	// (get) Token: 0x0600706C RID: 28780 RVA: 0x002B81A3 File Offset: 0x002B63A3
	// (set) Token: 0x0600706D RID: 28781 RVA: 0x002B81B4 File Offset: 0x002B63B4
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

	// Token: 0x0600706E RID: 28782 RVA: 0x002B81D3 File Offset: 0x002B63D3
	public override void Init()
	{
		base.Init();
		this.txtPresetName.UIInput.onValidate = new UIInput.OnValidate(GameUtils.ValidateGameNameInput);
	}

	// Token: 0x0600706F RID: 28783 RVA: 0x002B81F7 File Offset: 0x002B63F7
	[XuiBindEvent("OnPress", "btnCancel")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancelCreate_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.onConfirm = null;
	}

	// Token: 0x06007070 RID: 28784 RVA: 0x002B821C File Offset: 0x002B641C
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

	// Token: 0x06007071 RID: 28785 RVA: 0x002B82D2 File Offset: 0x002B64D2
	[XuiBindEvent("OnSubmitHandler", "txtPresetName")]
	[XuiBindEvent("OnSubmitHandler", "txtDescription")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void CreateProfileName_OnSubmitHandler(XUiController _sender, string _text)
	{
		this.BtnConfirmCreate_OnPressed(this, -1);
	}

	// Token: 0x06007072 RID: 28786 RVA: 0x002B82DC File Offset: 0x002B64DC
	[XuiBindEvent("OnChangeHandler", "txtPresetName")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void CreateProfileName_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		string text = _text.Trim();
		this.NameValid = text.Length > 0 && text.IndexOf('.') < 0 && this.sandboxManager.GetPreset(text) == null;
	}

	// Token: 0x06007073 RID: 28787 RVA: 0x002B831C File Offset: 0x002B651C
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

	// Token: 0x06007074 RID: 28788 RVA: 0x002B83A8 File Offset: 0x002B65A8
	public override void OnOpen()
	{
		base.OnOpen();
		this.NameValid = false;
		this.txtPresetName.Text = "";
		this.txtDescription.Text = this.Description ?? "";
		this.txtPresetName.SelectOrVirtualKeyboard(true);
	}

	// Token: 0x06007075 RID: 28789 RVA: 0x002B83F8 File Offset: 0x002B65F8
	public override void OnClose()
	{
		base.OnClose();
		this.Description = "";
	}

	// Token: 0x06007076 RID: 28790 RVA: 0x002B840C File Offset: 0x002B660C
	public static void Open(XUi _xui, string _sandboxCode, string _description, Action<string> _onConfirm = null)
	{
		XUiC_SandboxSettingsSaveAsPreset childByType = _xui.GetChildByType<XUiC_SandboxSettingsSaveAsPreset>();
		childByType.SandboxCode = _sandboxCode;
		childByType.Description = _description;
		childByType.onConfirm = _onConfirm;
		_xui.playerUI.windowManager.Open(childByType.windowGroup, false);
	}

	// Token: 0x0400552F RID: 21807
	[XuiBindComponent("presetName", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtPresetName;

	// Token: 0x04005530 RID: 21808
	[XuiBindComponent("description", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtDescription;

	// Token: 0x04005531 RID: 21809
	[XuiBindComponent("btnConfirm", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnConfirm;

	// Token: 0x04005532 RID: 21810
	[XuiBindComponent("btnCancel", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCancel;

	// Token: 0x04005533 RID: 21811
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SandboxOptionManager sandboxManager = SandboxOptionManager.Current;

	// Token: 0x04005534 RID: 21812
	[PublicizedFrom(EAccessModifier.Private)]
	public Action<string> onConfirm;

	// Token: 0x04005535 RID: 21813
	[PublicizedFrom(EAccessModifier.Private)]
	public string sandboxCode;

	// Token: 0x04005536 RID: 21814
	[PublicizedFrom(EAccessModifier.Private)]
	public bool nameValid;

	// Token: 0x04005537 RID: 21815
	[PublicizedFrom(EAccessModifier.Private)]
	public string description;
}
