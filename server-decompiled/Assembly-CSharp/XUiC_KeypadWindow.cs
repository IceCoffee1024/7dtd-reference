using System;
using Audio;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000F83 RID: 3971
[Preserve]
public class XUiC_KeypadWindow : XUiController
{
	// Token: 0x06007C8B RID: 31883 RVA: 0x002F3194 File Offset: 0x002F1394
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

	// Token: 0x06007C8C RID: 31884 RVA: 0x002AC36D File Offset: 0x002AA56D
	[PublicizedFrom(EAccessModifier.Private)]
	public void TextInput_OnInputAbortedHandler(XUiController _sender)
	{
		this.xui.playerUI.windowManager.Close(base.WindowGroup, false);
	}

	// Token: 0x06007C8D RID: 31885 RVA: 0x002F326A File Offset: 0x002F146A
	[PublicizedFrom(EAccessModifier.Private)]
	public void TxtPassword_OnSubmitHandler(XUiController _sender, string _text)
	{
		this.BtnOk_OnPressed(_sender, -1);
	}

	// Token: 0x06007C8E RID: 31886 RVA: 0x002F3274 File Offset: 0x002F1474
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

	// Token: 0x06007C8F RID: 31887 RVA: 0x002AC36D File Offset: 0x002AA56D
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancel_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(base.WindowGroup, false);
	}

	// Token: 0x06007C90 RID: 31888 RVA: 0x002F33D4 File Offset: 0x002F15D4
	public static void Open(LocalPlayerUI _playerUi, ILockable _lockedItem, Action _onClose = null, Func<bool> _interactionCheck = null)
	{
		XUiC_KeypadWindow childByType = _playerUi.xui.FindWindowGroupByName(XUiC_KeypadWindow.ID).GetChildByType<XUiC_KeypadWindow>();
		childByType.LockedItem = _lockedItem;
		childByType.onCloseCallback = _onClose;
		childByType.checkInteraction = _interactionCheck;
		_playerUi.windowManager.Open(XUiC_KeypadWindow.ID, true);
	}

	// Token: 0x06007C91 RID: 31889 RVA: 0x002F3410 File Offset: 0x002F1610
	public override void Update(float _dt)
	{
		if (this.checkInteraction != null && !this.checkInteraction())
		{
			this.xui.playerUI.windowManager.Close(XUiC_KeypadWindow.ID);
		}
		base.Update(_dt);
	}

	// Token: 0x06007C92 RID: 31890 RVA: 0x002F3448 File Offset: 0x002F1648
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.playerUI.entityPlayer.PlayOneShot("open_sign", false, false, false, null, 1f);
	}

	// Token: 0x06007C93 RID: 31891 RVA: 0x002F3474 File Offset: 0x002F1674
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

	// Token: 0x04005C3B RID: 23611
	public static string ID = "";

	// Token: 0x04005C3C RID: 23612
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtPassword;

	// Token: 0x04005C3D RID: 23613
	[PublicizedFrom(EAccessModifier.Private)]
	public ILockable LockedItem;

	// Token: 0x04005C3E RID: 23614
	[PublicizedFrom(EAccessModifier.Private)]
	public Action onCloseCallback;

	// Token: 0x04005C3F RID: 23615
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<bool> checkInteraction;
}
