using System;
using UnityEngine.Scripting;

// Token: 0x02000DD1 RID: 3537
[Preserve]
public class XUiC_ServerPasswordWindow : XUiController
{
	// Token: 0x17000BEE RID: 3054
	// (get) Token: 0x06006C45 RID: 27717 RVA: 0x002A2BAD File Offset: 0x002A0DAD
	// (set) Token: 0x06006C46 RID: 27718 RVA: 0x002A2BB5 File Offset: 0x002A0DB5
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

	// Token: 0x06006C47 RID: 27719 RVA: 0x002A2BC5 File Offset: 0x002A0DC5
	[XuiBindEvent("OnSubmitHandler", "txtPassword")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void TxtPassword_OnSubmitHandler(XUiController _sender, string _text)
	{
		this.BtnSubmit_OnPressed(_sender, -1);
	}

	// Token: 0x06006C48 RID: 27720 RVA: 0x002A2BD0 File Offset: 0x002A0DD0
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

	// Token: 0x06006C49 RID: 27721 RVA: 0x002A2C16 File Offset: 0x002A0E16
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

	// Token: 0x06006C4A RID: 27722 RVA: 0x002A2C44 File Offset: 0x002A0E44
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

	// Token: 0x06006C4B RID: 27723 RVA: 0x002A2CBF File Offset: 0x002A0EBF
	public override void OnOpen()
	{
		base.OnOpen();
		this.txtPassword.SetSelected(true, true);
	}

	// Token: 0x06006C4C RID: 27724 RVA: 0x002A2CD4 File Offset: 0x002A0ED4
	public override void OnClose()
	{
		base.OnClose();
		this.onCancel = null;
		this.onSubmit = null;
		this.txtPassword.Text = "";
	}

	// Token: 0x06006C4D RID: 27725 RVA: 0x002A2CFC File Offset: 0x002A0EFC
	public static void OpenPasswordWindow(XUi _xuiInstance, bool _badPassword, string _currentPwd, bool _modal, Action<string> _onSubmitDelegate, Action _onCancelDelegate)
	{
		XUiC_ServerPasswordWindow childByType = _xuiInstance.GetChildByType<XUiC_ServerPasswordWindow>();
		childByType.txtPassword.Text = _currentPwd;
		childByType.onSubmit = _onSubmitDelegate;
		childByType.onCancel = _onCancelDelegate;
		childByType.IsInvalidPassword = _badPassword;
		_xuiInstance.playerUI.windowManager.Open(childByType.windowGroup, _modal, true);
	}

	// Token: 0x04005264 RID: 21092
	[XuiBindComponent("txtPassword", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtPassword;

	// Token: 0x04005265 RID: 21093
	[XuiBindComponent("btnSubmit", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnSubmit;

	// Token: 0x04005266 RID: 21094
	[XuiBindComponent("btnCancel", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCancel;

	// Token: 0x04005267 RID: 21095
	[PublicizedFrom(EAccessModifier.Private)]
	public Action<string> onSubmit;

	// Token: 0x04005268 RID: 21096
	[PublicizedFrom(EAccessModifier.Private)]
	public Action onCancel;

	// Token: 0x04005269 RID: 21097
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isInvalidPassword;
}
