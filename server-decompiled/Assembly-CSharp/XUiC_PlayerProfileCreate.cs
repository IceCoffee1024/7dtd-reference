using System;
using UnityEngine.Scripting;

// Token: 0x02000E44 RID: 3652
[Preserve]
public class XUiC_PlayerProfileCreate : XUiController
{
	// Token: 0x17000CA9 RID: 3241
	// (get) Token: 0x06006FA4 RID: 28580 RVA: 0x002B585F File Offset: 0x002B3A5F
	// (set) Token: 0x06006FA5 RID: 28581 RVA: 0x002B5867 File Offset: 0x002B3A67
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

	// Token: 0x06006FA6 RID: 28582 RVA: 0x002B5881 File Offset: 0x002B3A81
	public override void OnOpen()
	{
		base.OnOpen();
		this.createProfileName.Text = "";
		this.createProfileName.SelectOrVirtualKeyboard(false);
	}

	// Token: 0x06006FA7 RID: 28583 RVA: 0x002B58A8 File Offset: 0x002B3AA8
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

	// Token: 0x06006FA8 RID: 28584 RVA: 0x002B5915 File Offset: 0x002B3B15
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

	// Token: 0x06006FA9 RID: 28585 RVA: 0x002B5944 File Offset: 0x002B3B44
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

	// Token: 0x06006FAA RID: 28586 RVA: 0x002B5996 File Offset: 0x002B3B96
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

	// Token: 0x06006FAB RID: 28587 RVA: 0x002B59AC File Offset: 0x002B3BAC
	[XuiBindEvent("OnChangeHandler", "createProfileName")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void CreateProfileName_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		string text = _text.Trim();
		this.CreateNameValid = text.Length > 0 && text.IndexOf('.') < 0 && !ProfileSDF.ProfileExists(text);
	}

	// Token: 0x06006FAC RID: 28588 RVA: 0x002B59E8 File Offset: 0x002B3BE8
	public static void Open(XUi _xui, Action<string> _onClosed)
	{
		XUiC_PlayerProfileCreate childByType = _xui.GetChildByType<XUiC_PlayerProfileCreate>();
		childByType.onClosed = _onClosed;
		_xui.playerUI.windowManager.Open(childByType.windowGroup, false);
	}

	// Token: 0x040054CB RID: 21707
	[XuiBindComponent("createProfileName", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput createProfileName;

	// Token: 0x040054CC RID: 21708
	[XuiBindComponent("btnConfirm", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button createProfileConfirm;

	// Token: 0x040054CD RID: 21709
	[XuiBindComponent("btnCancel", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button createProfileCancel;

	// Token: 0x040054CE RID: 21710
	[PublicizedFrom(EAccessModifier.Private)]
	public bool createNameValid;

	// Token: 0x040054CF RID: 21711
	[PublicizedFrom(EAccessModifier.Private)]
	public Action<string> onClosed;
}
