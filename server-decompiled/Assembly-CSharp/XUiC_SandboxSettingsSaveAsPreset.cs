using System;
using SandboxOptions;
using UnityEngine.Scripting;

// Token: 0x02000E52 RID: 3666
[Preserve]
public class XUiC_SandboxSettingsSaveAsPreset : XUiController
{
	// Token: 0x17000CCB RID: 3275
	// (get) Token: 0x06007043 RID: 28739 RVA: 0x002B6BE5 File Offset: 0x002B4DE5
	// (set) Token: 0x06007044 RID: 28740 RVA: 0x002B6BF6 File Offset: 0x002B4DF6
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

	// Token: 0x17000CCC RID: 3276
	// (get) Token: 0x06007045 RID: 28741 RVA: 0x002B6C15 File Offset: 0x002B4E15
	// (set) Token: 0x06007046 RID: 28742 RVA: 0x002B6C1D File Offset: 0x002B4E1D
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

	// Token: 0x17000CCD RID: 3277
	// (get) Token: 0x06007047 RID: 28743 RVA: 0x002B6C37 File Offset: 0x002B4E37
	// (set) Token: 0x06007048 RID: 28744 RVA: 0x002B6C48 File Offset: 0x002B4E48
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

	// Token: 0x06007049 RID: 28745 RVA: 0x002B6C67 File Offset: 0x002B4E67
	public override void Init()
	{
		base.Init();
		this.txtPresetName.UIInput.onValidate = new UIInput.OnValidate(GameUtils.ValidateGameNameInput);
	}

	// Token: 0x0600704A RID: 28746 RVA: 0x002B6C8B File Offset: 0x002B4E8B
	[XuiBindEvent("OnPress", "btnCancel")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancelCreate_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.onConfirm = null;
	}

	// Token: 0x0600704B RID: 28747 RVA: 0x002B6CB0 File Offset: 0x002B4EB0
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

	// Token: 0x0600704C RID: 28748 RVA: 0x002B6D66 File Offset: 0x002B4F66
	[XuiBindEvent("OnSubmitHandler", "txtPresetName")]
	[XuiBindEvent("OnSubmitHandler", "txtDescription")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void CreateProfileName_OnSubmitHandler(XUiController _sender, string _text)
	{
		this.BtnConfirmCreate_OnPressed(this, -1);
	}

	// Token: 0x0600704D RID: 28749 RVA: 0x002B6D70 File Offset: 0x002B4F70
	[XuiBindEvent("OnChangeHandler", "txtPresetName")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void CreateProfileName_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		string text = _text.Trim();
		this.NameValid = text.Length > 0 && text.IndexOf('.') < 0 && this.sandboxManager.GetPreset(text) == null;
	}

	// Token: 0x0600704E RID: 28750 RVA: 0x002B6DB0 File Offset: 0x002B4FB0
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

	// Token: 0x0600704F RID: 28751 RVA: 0x002B6E3C File Offset: 0x002B503C
	public override void OnOpen()
	{
		base.OnOpen();
		this.NameValid = false;
		this.txtPresetName.Text = "";
		this.txtDescription.Text = this.Description ?? "";
		this.txtPresetName.SelectOrVirtualKeyboard(true);
	}

	// Token: 0x06007050 RID: 28752 RVA: 0x002B6E8C File Offset: 0x002B508C
	public override void OnClose()
	{
		base.OnClose();
		this.Description = "";
	}

	// Token: 0x06007051 RID: 28753 RVA: 0x002B6EA0 File Offset: 0x002B50A0
	public static void Open(XUi _xui, string _sandboxCode, string _description, Action<string> _onConfirm = null)
	{
		XUiC_SandboxSettingsSaveAsPreset childByType = _xui.GetChildByType<XUiC_SandboxSettingsSaveAsPreset>();
		childByType.SandboxCode = _sandboxCode;
		childByType.Description = _description;
		childByType.onConfirm = _onConfirm;
		_xui.playerUI.windowManager.Open(childByType.windowGroup, false);
	}

	// Token: 0x0400550A RID: 21770
	[XuiBindComponent("presetName", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtPresetName;

	// Token: 0x0400550B RID: 21771
	[XuiBindComponent("description", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtDescription;

	// Token: 0x0400550C RID: 21772
	[XuiBindComponent("btnConfirm", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnConfirm;

	// Token: 0x0400550D RID: 21773
	[XuiBindComponent("btnCancel", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCancel;

	// Token: 0x0400550E RID: 21774
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SandboxOptionManager sandboxManager = SandboxOptionManager.Current;

	// Token: 0x0400550F RID: 21775
	[PublicizedFrom(EAccessModifier.Private)]
	public Action<string> onConfirm;

	// Token: 0x04005510 RID: 21776
	[PublicizedFrom(EAccessModifier.Private)]
	public string sandboxCode;

	// Token: 0x04005511 RID: 21777
	[PublicizedFrom(EAccessModifier.Private)]
	public bool nameValid;

	// Token: 0x04005512 RID: 21778
	[PublicizedFrom(EAccessModifier.Private)]
	public string description;
}
