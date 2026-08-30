using System;
using Audio;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000F64 RID: 3940
[Preserve]
public class XUiC_KeypadWindow : XUiController
{
	// Token: 0x06007C2C RID: 31788 RVA: 0x002EE3FC File Offset: 0x002EC5FC
	public override void Init()
	{
		XUiC_KeypadWindow.ID = this.windowGroup.Id;
		base.Init();
		this.txtPassword = (XUiC_TextInput)base.GetChildById("txtPassword");
		this.txtPassword.OnSubmitHandler += this.TxtPassword_OnSubmitHandler;
		XUiC_SimpleButton xuiC_SimpleButton = (XUiC_SimpleButton)base.GetChildById("btnCancel");
		xuiC_SimpleButton.OnPressed += this.BtnCancel_OnPressed;
		xuiC_SimpleButton.ViewComponent.NavUpTarget = this.txtPassword.ViewComponent;
		XUiC_SimpleButton xuiC_SimpleButton2 = (XUiC_SimpleButton)base.GetChildById("btnOk");
		xuiC_SimpleButton2.OnPressed += this.BtnOk_OnPressed;
		xuiC_SimpleButton2.ViewComponent.NavUpTarget = this.txtPassword.ViewComponent;
		this.txtPassword.ViewComponent.NavDownTarget = xuiC_SimpleButton.ViewComponent;
	}

	// Token: 0x06007C2D RID: 31789 RVA: 0x002A7AB5 File Offset: 0x002A5CB5
	[PublicizedFrom(EAccessModifier.Private)]
	public void TextInput_OnInputAbortedHandler(XUiController _sender)
	{
		this.xui.playerUI.windowManager.Close(base.WindowGroup, false);
	}

	// Token: 0x06007C2E RID: 31790 RVA: 0x002EE4D2 File Offset: 0x002EC6D2
	[PublicizedFrom(EAccessModifier.Private)]
	public void TxtPassword_OnSubmitHandler(XUiController _sender, string _text)
	{
		this.BtnOk_OnPressed(_sender, -1);
	}

	// Token: 0x06007C2F RID: 31791 RVA: 0x002EE4DC File Offset: 0x002EC6DC
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnOk_OnPressed(XUiController _sender, int _mouseButton)
	{
		string text = this.txtPassword.Text;
		string hashForPassword = this.LockedItem.GetHashForPassword(text);
		if (this.LockedItem.IsOwner(PlatformManager.InternalLocalUserIdentifier) && hashForPassword != this.LockedItem.GetPasswordHash())
		{
			this.LockedItem.SetPasswordHash(hashForPassword, PlatformManager.InternalLocalUserIdentifier);
			if (text.Length == 0)
			{
				GameManager.ShowTooltip(this.xui.playerUI.entityPlayer, "passcodeRemoved", false, false, 0f);
			}
			else
			{
				GameManager.ShowTooltip(this.xui.playerUI.entityPlayer, "passcodeSet", false, false, 0f);
			}
		}
		if (this.LockedItem.CheckPasswordHash(hashForPassword, PlatformManager.InternalLocalUserIdentifier))
		{
			if (this.LockedItem.LocalPlayerIsOwner())
			{
				Manager.PlayInsidePlayerHead("Misc/password_set", -1, 0f, false, false);
			}
			else
			{
				GameManager.ShowTooltip(this.xui.playerUI.entityPlayer, "passcodeAccepted", false, false, 0f);
				Manager.PlayInsidePlayerHead("Misc/password_pass", -1, 0f, false, false);
			}
			this.xui.playerUI.windowManager.Close(base.WindowGroup, false);
			return;
		}
		Manager.PlayInsidePlayerHead("Misc/password_fail", -1, 0f, false, false);
		GameManager.ShowTooltip(this.xui.playerUI.entityPlayer, "passcodeRejected", false, false, 0f);
	}

	// Token: 0x06007C30 RID: 31792 RVA: 0x002A7AB5 File Offset: 0x002A5CB5
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancel_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(base.WindowGroup, false);
	}

	// Token: 0x06007C31 RID: 31793 RVA: 0x002EE63C File Offset: 0x002EC83C
	public static void Open(LocalPlayerUI _playerUi, ILockable _lockedItem, Action _onClose = null, Func<bool> _interactionCheck = null)
	{
		XUiC_KeypadWindow childByType = _playerUi.xui.FindWindowGroupByName(XUiC_KeypadWindow.ID).GetChildByType<XUiC_KeypadWindow>();
		childByType.LockedItem = _lockedItem;
		childByType.onCloseCallback = _onClose;
		childByType.checkInteraction = _interactionCheck;
		_playerUi.windowManager.Open(XUiC_KeypadWindow.ID, true);
	}

	// Token: 0x06007C32 RID: 31794 RVA: 0x002EE678 File Offset: 0x002EC878
	public override void Update(float _dt)
	{
		if (this.checkInteraction != null && !this.checkInteraction())
		{
			this.xui.playerUI.windowManager.Close(XUiC_KeypadWindow.ID);
		}
		base.Update(_dt);
	}

	// Token: 0x06007C33 RID: 31795 RVA: 0x002EE6B0 File Offset: 0x002EC8B0
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.playerUI.entityPlayer.PlayOneShot("open_sign", false, false, false, null, 1f);
	}

	// Token: 0x06007C34 RID: 31796 RVA: 0x002EE6DC File Offset: 0x002EC8DC
	public override void OnClose()
	{
		base.OnClose();
		this.xui.playerUI.entityPlayer.PlayOneShot("close_sign", false, false, false, null, 1f);
		Action action = this.onCloseCallback;
		if (action != null)
		{
			action();
		}
		this.LockedItem = null;
		this.onCloseCallback = null;
		this.checkInteraction = null;
	}

	// Token: 0x04005BD8 RID: 23512
	public static string ID = "";

	// Token: 0x04005BD9 RID: 23513
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtPassword;

	// Token: 0x04005BDA RID: 23514
	[PublicizedFrom(EAccessModifier.Private)]
	public ILockable LockedItem;

	// Token: 0x04005BDB RID: 23515
	[PublicizedFrom(EAccessModifier.Private)]
	public Action onCloseCallback;

	// Token: 0x04005BDC RID: 23516
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<bool> checkInteraction;
}
