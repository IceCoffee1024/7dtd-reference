using System;
using UnityEngine.Scripting;

// Token: 0x02000DE6 RID: 3558
[Preserve]
public class XUiC_ServerPasswordWindow : XUiController
{
	// Token: 0x17000C07 RID: 3079
	// (get) Token: 0x06006CE4 RID: 27876 RVA: 0x002A6041 File Offset: 0x002A4241
	// (set) Token: 0x06006CE5 RID: 27877 RVA: 0x002A6049 File Offset: 0x002A4249
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

	// Token: 0x06006CE6 RID: 27878 RVA: 0x002A6059 File Offset: 0x002A4259
	[XuiBindEvent("OnSubmitHandler", "txtPassword")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void TxtPassword_OnSubmitHandler(XUiController _sender, string _text)
	{
		this.BtnSubmit_OnPressed(_sender, -1);
	}

	// Token: 0x06006CE7 RID: 27879 RVA: 0x002A6064 File Offset: 0x002A4264
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

	// Token: 0x06006CE8 RID: 27880 RVA: 0x002A60AA File Offset: 0x002A42AA
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

	// Token: 0x06006CE9 RID: 27881 RVA: 0x002A60D8 File Offset: 0x002A42D8
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

	// Token: 0x06006CEA RID: 27882 RVA: 0x002A6153 File Offset: 0x002A4353
	public override void OnOpen()
	{
		base.OnOpen();
		this.txtPassword.SetSelected(true, true);
	}

	// Token: 0x06006CEB RID: 27883 RVA: 0x002A6168 File Offset: 0x002A4368
	public override void OnClose()
	{
		base.OnClose();
		this.onCancel = null;
		this.onSubmit = null;
		this.txtPassword.Text = "";
	}

	// Token: 0x06006CEC RID: 27884 RVA: 0x002A6190 File Offset: 0x002A4390
	public static void OpenPasswordWindow(XUi _xuiInstance, bool _badPassword, string _currentPwd, bool _modal, Action<string> _onSubmitDelegate, Action _onCancelDelegate)
	{
		XUiC_ServerPasswordWindow childByType = _xuiInstance.GetChildByType<XUiC_ServerPasswordWindow>();
		childByType.txtPassword.Text = _currentPwd;
		childByType.onSubmit = _onSubmitDelegate;
		childByType.onCancel = _onCancelDelegate;
		childByType.IsInvalidPassword = _badPassword;
		_xuiInstance.playerUI.windowManager.Open(childByType.windowGroup, _modal, true);
	}

	// Token: 0x04005307 RID: 21255
	[XuiBindComponent("txtPassword", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtPassword;

	// Token: 0x04005308 RID: 21256
	[XuiBindComponent("btnSubmit", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnSubmit;

	// Token: 0x04005309 RID: 21257
	[XuiBindComponent("btnCancel", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCancel;

	// Token: 0x0400530A RID: 21258
	[PublicizedFrom(EAccessModifier.Private)]
	public Action<string> onSubmit;

	// Token: 0x0400530B RID: 21259
	[PublicizedFrom(EAccessModifier.Private)]
	public Action onCancel;

	// Token: 0x0400530C RID: 21260
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isInvalidPassword;
}
