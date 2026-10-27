using System;
using UnityEngine.Scripting;

// Token: 0x02000E00 RID: 3584
[Preserve]
public class XUiC_ServerPasswordWindow : XUiController
{
	// Token: 0x17000C28 RID: 3112
	// (get) Token: 0x06006D01 RID: 27905 RVA: 0x002AA8F9 File Offset: 0x002A8AF9
	// (set) Token: 0x06006D02 RID: 27906 RVA: 0x002AA901 File Offset: 0x002A8B01
	[XuiXmlBinding("isinvalidpassword")]
	public bool IsInvalidPassword
	{
		get
		{
			return this.isInvalidPassword;
		}
		set
		{
			this.isInvalidPassword = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x06006D03 RID: 27907 RVA: 0x002AA911 File Offset: 0x002A8B11
	[XuiBindEvent("OnSubmitHandler", "txtPassword")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void TxtPassword_OnSubmitHandler(XUiController _sender, string _text)
	{
		this.BtnSubmit_OnPressed(_sender, -1);
	}

	// Token: 0x06006D04 RID: 27908 RVA: 0x002AA91C File Offset: 0x002A8B1C
	[XuiBindEvent("OnPress", "btnSubmit")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnSubmit_OnPressed(XUiController _sender, int _mouseButton)
	{
		Action<string> action = this.onSubmit;
		string text = this.txtPassword.Text;
		this.xui.playerUI.windowManager.Close(base.WindowGroup, false);
		if (action == null)
		{
			return;
		}
		action(text);
	}

	// Token: 0x06006D05 RID: 27909 RVA: 0x002AA962 File Offset: 0x002A8B62
	[XuiBindEvent("OnPress", "btnCancel")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancel_OnPressed(XUiController _sender, int _mouseButton)
	{
		Action action = this.onCancel;
		this.xui.playerUI.windowManager.Close(base.WindowGroup, false);
		if (action == null)
		{
			return;
		}
		action();
	}

	// Token: 0x06006D06 RID: 27910 RVA: 0x002AA990 File Offset: 0x002A8B90
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
		if (XUiUtils.HotkeysAllowedFor(this.viewComponent, false))
		{
			if (this.xui.playerUI.playerInput.PermanentActions.Cancel.WasReleased)
			{
				this.BtnCancel_OnPressed(this, -1);
			}
			if (this.xui.playerUI.playerInput.GUIActions.Apply.WasReleased)
			{
				this.BtnSubmit_OnPressed(this, -1);
			}
		}
	}

	// Token: 0x06006D07 RID: 27911 RVA: 0x002AAA0B File Offset: 0x002A8C0B
	public override void OnOpen()
	{
		base.OnOpen();
		this.txtPassword.SetSelected(true, true);
	}

	// Token: 0x06006D08 RID: 27912 RVA: 0x002AAA20 File Offset: 0x002A8C20
	public override void OnClose()
	{
		base.OnClose();
		this.onCancel = null;
		this.onSubmit = null;
		this.txtPassword.Text = "";
	}

	// Token: 0x06006D09 RID: 27913 RVA: 0x002AAA48 File Offset: 0x002A8C48
	public static void OpenPasswordWindow(XUi _xuiInstance, bool _badPassword, string _currentPwd, bool _modal, Action<string> _onSubmitDelegate, Action _onCancelDelegate)
	{
		XUiC_ServerPasswordWindow childByType = _xuiInstance.GetChildByType<XUiC_ServerPasswordWindow>();
		childByType.txtPassword.Text = _currentPwd;
		childByType.onSubmit = _onSubmitDelegate;
		childByType.onCancel = _onCancelDelegate;
		childByType.IsInvalidPassword = _badPassword;
		_xuiInstance.playerUI.windowManager.Open(childByType.windowGroup, _modal, true);
	}

	// Token: 0x04005363 RID: 21347
	[XuiBindComponent("txtPassword", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtPassword;

	// Token: 0x04005364 RID: 21348
	[XuiBindComponent("btnSubmit", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnSubmit;

	// Token: 0x04005365 RID: 21349
	[XuiBindComponent("btnCancel", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCancel;

	// Token: 0x04005366 RID: 21350
	[PublicizedFrom(EAccessModifier.Private)]
	public Action<string> onSubmit;

	// Token: 0x04005367 RID: 21351
	[PublicizedFrom(EAccessModifier.Private)]
	public Action onCancel;

	// Token: 0x04005368 RID: 21352
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isInvalidPassword;
}
