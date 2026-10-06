using System;
using UnityEngine.Scripting;

// Token: 0x02000DF5 RID: 3573
[Preserve]
public class XUiC_ServerPasswordWindow : XUiController
{
	// Token: 0x17000C22 RID: 3106
	// (get) Token: 0x06006CDC RID: 27868 RVA: 0x002A93D1 File Offset: 0x002A75D1
	// (set) Token: 0x06006CDD RID: 27869 RVA: 0x002A93D9 File Offset: 0x002A75D9
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

	// Token: 0x06006CDE RID: 27870 RVA: 0x002A93E9 File Offset: 0x002A75E9
	[XuiBindEvent("OnSubmitHandler", "txtPassword")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void TxtPassword_OnSubmitHandler(XUiController _sender, string _text)
	{
		this.BtnSubmit_OnPressed(_sender, -1);
	}

	// Token: 0x06006CDF RID: 27871 RVA: 0x002A93F4 File Offset: 0x002A75F4
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

	// Token: 0x06006CE0 RID: 27872 RVA: 0x002A943A File Offset: 0x002A763A
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

	// Token: 0x06006CE1 RID: 27873 RVA: 0x002A9468 File Offset: 0x002A7668
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

	// Token: 0x06006CE2 RID: 27874 RVA: 0x002A94E3 File Offset: 0x002A76E3
	public override void OnOpen()
	{
		base.OnOpen();
		this.txtPassword.SetSelected(true, true);
	}

	// Token: 0x06006CE3 RID: 27875 RVA: 0x002A94F8 File Offset: 0x002A76F8
	public override void OnClose()
	{
		base.OnClose();
		this.onCancel = null;
		this.onSubmit = null;
		this.txtPassword.Text = "";
	}

	// Token: 0x06006CE4 RID: 27876 RVA: 0x002A9520 File Offset: 0x002A7720
	public static void OpenPasswordWindow(XUi _xuiInstance, bool _badPassword, string _currentPwd, bool _modal, Action<string> _onSubmitDelegate, Action _onCancelDelegate)
	{
		XUiC_ServerPasswordWindow childByType = _xuiInstance.GetChildByType<XUiC_ServerPasswordWindow>();
		childByType.txtPassword.Text = _currentPwd;
		childByType.onSubmit = _onSubmitDelegate;
		childByType.onCancel = _onCancelDelegate;
		childByType.IsInvalidPassword = _badPassword;
		_xuiInstance.playerUI.windowManager.Open(childByType.windowGroup, _modal, true);
	}

	// Token: 0x0400533E RID: 21310
	[XuiBindComponent("txtPassword", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtPassword;

	// Token: 0x0400533F RID: 21311
	[XuiBindComponent("btnSubmit", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnSubmit;

	// Token: 0x04005340 RID: 21312
	[XuiBindComponent("btnCancel", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCancel;

	// Token: 0x04005341 RID: 21313
	[PublicizedFrom(EAccessModifier.Private)]
	public Action<string> onSubmit;

	// Token: 0x04005342 RID: 21314
	[PublicizedFrom(EAccessModifier.Private)]
	public Action onCancel;

	// Token: 0x04005343 RID: 21315
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isInvalidPassword;
}
