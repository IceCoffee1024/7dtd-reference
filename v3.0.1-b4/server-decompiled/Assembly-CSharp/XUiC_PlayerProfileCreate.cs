using System;
using UnityEngine.Scripting;

// Token: 0x02000E15 RID: 3605
[Preserve]
public class XUiC_PlayerProfileCreate : XUiController
{
	// Token: 0x17000C6F RID: 3183
	// (get) Token: 0x06006EE7 RID: 28391 RVA: 0x002ADA2F File Offset: 0x002ABC2F
	// (set) Token: 0x06006EE8 RID: 28392 RVA: 0x002ADA37 File Offset: 0x002ABC37
	[XuiXmlBinding("create_name_valid")]
	public bool CreateNameValid
	{
		get
		{
			return this.createNameValid;
		}
		set
		{
			if (this.createNameValid == value)
			{
				return;
			}
			this.createNameValid = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x06006EE9 RID: 28393 RVA: 0x002ADA51 File Offset: 0x002ABC51
	public override void OnOpen()
	{
		base.OnOpen();
		this.createProfileName.Text = "";
		this.createProfileName.SelectOrVirtualKeyboard(false);
	}

	// Token: 0x06006EEA RID: 28394 RVA: 0x002ADA78 File Offset: 0x002ABC78
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
		if (this.xui.playerUI.playerInput.PermanentActions.Cancel.WasReleased)
		{
			this.BtnCancelCreate_OnPressed(this, -1);
		}
		if (this.xui.playerUI.playerInput.GUIActions.Apply.WasReleased)
		{
			this.BtnConfirmCreate_OnPressed(this, -1);
		}
	}

	// Token: 0x06006EEB RID: 28395 RVA: 0x002ADAE5 File Offset: 0x002ABCE5
	[XuiBindEvent("OnPress", "createProfileCancel")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancelCreate_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		Action<string> action = this.onClosed;
		if (action == null)
		{
			return;
		}
		action(null);
	}

	// Token: 0x06006EEC RID: 28396 RVA: 0x002ADB14 File Offset: 0x002ABD14
	[XuiBindEvent("OnPress", "createProfileConfirm")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnConfirmCreate_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (!this.CreateNameValid)
		{
			return;
		}
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		Action<string> action = this.onClosed;
		if (action == null)
		{
			return;
		}
		action(this.createProfileName.Text.Trim());
	}

	// Token: 0x06006EED RID: 28397 RVA: 0x002ADB66 File Offset: 0x002ABD66
	[XuiBindEvent("OnSubmitHandler", "createProfileName")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void CreateProfileName_OnSubmitHandler(XUiController _sender, string _text)
	{
		if (!this.CreateNameValid)
		{
			return;
		}
		this.BtnConfirmCreate_OnPressed(this, -1);
	}

	// Token: 0x06006EEE RID: 28398 RVA: 0x002ADB7C File Offset: 0x002ABD7C
	[XuiBindEvent("OnChangeHandler", "createProfileName")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void CreateProfileName_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		string text = _text.Trim();
		this.CreateNameValid = text.Length > 0 && text.IndexOf('.') < 0 && !ProfileSDF.ProfileExists(text);
	}

	// Token: 0x06006EEF RID: 28399 RVA: 0x002ADBB8 File Offset: 0x002ABDB8
	public static void Open(XUi _xui, Action<string> _onClosed)
	{
		XUiC_PlayerProfileCreate childByType = _xui.GetChildByType<XUiC_PlayerProfileCreate>();
		childByType.onClosed = _onClosed;
		_xui.playerUI.windowManager.Open(childByType.windowGroup, false);
	}

	// Token: 0x040053CB RID: 21451
	[XuiBindComponent("createProfileName", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput createProfileName;

	// Token: 0x040053CC RID: 21452
	[XuiBindComponent("btnConfirm", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button createProfileConfirm;

	// Token: 0x040053CD RID: 21453
	[XuiBindComponent("btnCancel", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button createProfileCancel;

	// Token: 0x040053CE RID: 21454
	[PublicizedFrom(EAccessModifier.Private)]
	public bool createNameValid;

	// Token: 0x040053CF RID: 21455
	[PublicizedFrom(EAccessModifier.Private)]
	public Action<string> onClosed;
}
