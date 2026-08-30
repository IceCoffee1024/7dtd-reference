using System;
using Audio;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000F4B RID: 3915
[Preserve]
public class XUiC_KeypadWindow : XUiController
{
	// Token: 0x06007B3A RID: 31546 RVA: 0x002E9C28 File Offset: 0x002E7E28
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

	// Token: 0x06007B3B RID: 31547 RVA: 0x002A4621 File Offset: 0x002A2821
	[PublicizedFrom(EAccessModifier.Private)]
	public void TextInput_OnInputAbortedHandler(XUiController _sender)
	{
		this.xui.playerUI.windowManager.Close(base.WindowGroup, false);
	}

	// Token: 0x06007B3C RID: 31548 RVA: 0x002E9CFE File Offset: 0x002E7EFE
	[PublicizedFrom(EAccessModifier.Private)]
	public void TxtPassword_OnSubmitHandler(XUiController _sender, string _text)
	{
		this.BtnOk_OnPressed(_sender, -1);
	}

	// Token: 0x06007B3D RID: 31549 RVA: 0x002E9D08 File Offset: 0x002E7F08
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

	// Token: 0x06007B3E RID: 31550 RVA: 0x002A4621 File Offset: 0x002A2821
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancel_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(base.WindowGroup, false);
	}

	// Token: 0x06007B3F RID: 31551 RVA: 0x002E9E68 File Offset: 0x002E8068
	public static void Open(LocalPlayerUI _playerUi, ILockable _lockedItem, Action _onClose = null, Func<bool> _interactionCheck = null)
	{
		XUiC_KeypadWindow childByType = _playerUi.xui.FindWindowGroupByName(XUiC_KeypadWindow.ID).GetChildByType<XUiC_KeypadWindow>();
		childByType.LockedItem = _lockedItem;
		childByType.onCloseCallback = _onClose;
		childByType.checkInteraction = _interactionCheck;
		_playerUi.windowManager.Open(XUiC_KeypadWindow.ID, true);
	}

	// Token: 0x06007B40 RID: 31552 RVA: 0x002E9EA4 File Offset: 0x002E80A4
	public override void Update(float _dt)
	{
		if (this.checkInteraction != null && !this.checkInteraction())
		{
			this.xui.playerUI.windowManager.Close(XUiC_KeypadWindow.ID);
		}
		base.Update(_dt);
	}

	// Token: 0x06007B41 RID: 31553 RVA: 0x002E9EDC File Offset: 0x002E80DC
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.playerUI.entityPlayer.PlayOneShot("open_sign", false, false, false, null, 1f);
	}

	// Token: 0x06007B42 RID: 31554 RVA: 0x002E9F08 File Offset: 0x002E8108
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

	// Token: 0x04005B11 RID: 23313
	public static string ID = "";

	// Token: 0x04005B12 RID: 23314
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtPassword;

	// Token: 0x04005B13 RID: 23315
	[PublicizedFrom(EAccessModifier.Private)]
	public ILockable LockedItem;

	// Token: 0x04005B14 RID: 23316
	[PublicizedFrom(EAccessModifier.Private)]
	public Action onCloseCallback;

	// Token: 0x04005B15 RID: 23317
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<bool> checkInteraction;
}
