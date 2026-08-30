using System;
using UnityEngine.Scripting;

// Token: 0x02000E2A RID: 3626
[Preserve]
public class XUiC_PlayerProfileCreate : XUiController
{
	// Token: 0x17000C88 RID: 3208
	// (get) Token: 0x06006F87 RID: 28551 RVA: 0x002B0F43 File Offset: 0x002AF143
	// (set) Token: 0x06006F88 RID: 28552 RVA: 0x002B0F4B File Offset: 0x002AF14B
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

	// Token: 0x06006F89 RID: 28553 RVA: 0x002B0F65 File Offset: 0x002AF165
	public override void OnOpen()
	{
		base.OnOpen();
		this.createProfileName.Text = "";
		this.createProfileName.SelectOrVirtualKeyboard(false);
	}

	// Token: 0x06006F8A RID: 28554 RVA: 0x002B0F8C File Offset: 0x002AF18C
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

	// Token: 0x06006F8B RID: 28555 RVA: 0x002B0FF9 File Offset: 0x002AF1F9
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

	// Token: 0x06006F8C RID: 28556 RVA: 0x002B1028 File Offset: 0x002AF228
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

	// Token: 0x06006F8D RID: 28557 RVA: 0x002B107A File Offset: 0x002AF27A
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

	// Token: 0x06006F8E RID: 28558 RVA: 0x002B1090 File Offset: 0x002AF290
	[XuiBindEvent("OnChangeHandler", "createProfileName")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void CreateProfileName_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		string text = _text.Trim();
		this.CreateNameValid = text.Length > 0 && text.IndexOf('.') < 0 && !ProfileSDF.ProfileExists(text);
	}

	// Token: 0x06006F8F RID: 28559 RVA: 0x002B10CC File Offset: 0x002AF2CC
	public static void Open(XUi _xui, Action<string> _onClosed)
	{
		XUiC_PlayerProfileCreate childByType = _xui.GetChildByType<XUiC_PlayerProfileCreate>();
		childByType.onClosed = _onClosed;
		_xui.playerUI.windowManager.Open(childByType.windowGroup, false);
	}

	// Token: 0x0400546F RID: 21615
	[XuiBindComponent("createProfileName", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput createProfileName;

	// Token: 0x04005470 RID: 21616
	[XuiBindComponent("btnConfirm", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button createProfileConfirm;

	// Token: 0x04005471 RID: 21617
	[XuiBindComponent("btnCancel", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button createProfileCancel;

	// Token: 0x04005472 RID: 21618
	[PublicizedFrom(EAccessModifier.Private)]
	public bool createNameValid;

	// Token: 0x04005473 RID: 21619
	[PublicizedFrom(EAccessModifier.Private)]
	public Action<string> onClosed;
}
