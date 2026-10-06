using System;
using UnityEngine.Scripting;

// Token: 0x02000E39 RID: 3641
[Preserve]
public class XUiC_PlayerProfileCreate : XUiController
{
	// Token: 0x17000CA3 RID: 3235
	// (get) Token: 0x06006F7F RID: 28543 RVA: 0x002B42F3 File Offset: 0x002B24F3
	// (set) Token: 0x06006F80 RID: 28544 RVA: 0x002B42FB File Offset: 0x002B24FB
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

	// Token: 0x06006F81 RID: 28545 RVA: 0x002B4315 File Offset: 0x002B2515
	public override void OnOpen()
	{
		base.OnOpen();
		this.createProfileName.Text = "";
		this.createProfileName.SelectOrVirtualKeyboard(false);
	}

	// Token: 0x06006F82 RID: 28546 RVA: 0x002B433C File Offset: 0x002B253C
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

	// Token: 0x06006F83 RID: 28547 RVA: 0x002B43A9 File Offset: 0x002B25A9
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

	// Token: 0x06006F84 RID: 28548 RVA: 0x002B43D8 File Offset: 0x002B25D8
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

	// Token: 0x06006F85 RID: 28549 RVA: 0x002B442A File Offset: 0x002B262A
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

	// Token: 0x06006F86 RID: 28550 RVA: 0x002B4440 File Offset: 0x002B2640
	[XuiBindEvent("OnChangeHandler", "createProfileName")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void CreateProfileName_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		string text = _text.Trim();
		this.CreateNameValid = text.Length > 0 && text.IndexOf('.') < 0 && !ProfileSDF.ProfileExists(text);
	}

	// Token: 0x06006F87 RID: 28551 RVA: 0x002B447C File Offset: 0x002B267C
	public static void Open(XUi _xui, Action<string> _onClosed)
	{
		XUiC_PlayerProfileCreate childByType = _xui.GetChildByType<XUiC_PlayerProfileCreate>();
		childByType.onClosed = _onClosed;
		_xui.playerUI.windowManager.Open(childByType.windowGroup, false);
	}

	// Token: 0x040054A6 RID: 21670
	[XuiBindComponent("createProfileName", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput createProfileName;

	// Token: 0x040054A7 RID: 21671
	[XuiBindComponent("btnConfirm", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button createProfileConfirm;

	// Token: 0x040054A8 RID: 21672
	[XuiBindComponent("btnCancel", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button createProfileCancel;

	// Token: 0x040054A9 RID: 21673
	[PublicizedFrom(EAccessModifier.Private)]
	public bool createNameValid;

	// Token: 0x040054AA RID: 21674
	[PublicizedFrom(EAccessModifier.Private)]
	public Action<string> onClosed;
}
