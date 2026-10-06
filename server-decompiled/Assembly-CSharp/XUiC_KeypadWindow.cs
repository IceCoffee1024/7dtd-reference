using System;
using Audio;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000F73 RID: 3955
[Preserve]
public class XUiC_KeypadWindow : XUiController
{
	// Token: 0x06007C3C RID: 31804 RVA: 0x002F150C File Offset: 0x002EF70C
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

	// Token: 0x06007C3D RID: 31805 RVA: 0x002AAE45 File Offset: 0x002A9045
	[PublicizedFrom(EAccessModifier.Private)]
	public void TextInput_OnInputAbortedHandler(XUiController _sender)
	{
		this.xui.playerUI.windowManager.Close(base.WindowGroup, false);
	}

	// Token: 0x06007C3E RID: 31806 RVA: 0x002F15E2 File Offset: 0x002EF7E2
	[PublicizedFrom(EAccessModifier.Private)]
	public void TxtPassword_OnSubmitHandler(XUiController _sender, string _text)
	{
		this.BtnOk_OnPressed(_sender, -1);
	}

	// Token: 0x06007C3F RID: 31807 RVA: 0x002F15EC File Offset: 0x002EF7EC
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

	// Token: 0x06007C40 RID: 31808 RVA: 0x002AAE45 File Offset: 0x002A9045
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancel_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(base.WindowGroup, false);
	}

	// Token: 0x06007C41 RID: 31809 RVA: 0x002F174C File Offset: 0x002EF94C
	public static void Open(LocalPlayerUI _playerUi, ILockable _lockedItem, Action _onClose = null, Func<bool> _interactionCheck = null)
	{
		XUiC_KeypadWindow childByType = _playerUi.xui.FindWindowGroupByName(XUiC_KeypadWindow.ID).GetChildByType<XUiC_KeypadWindow>();
		childByType.LockedItem = _lockedItem;
		childByType.onCloseCallback = _onClose;
		childByType.checkInteraction = _interactionCheck;
		_playerUi.windowManager.Open(XUiC_KeypadWindow.ID, true);
	}

	// Token: 0x06007C42 RID: 31810 RVA: 0x002F1788 File Offset: 0x002EF988
	public override void Update(float _dt)
	{
		if (this.checkInteraction != null && !this.checkInteraction())
		{
			this.xui.playerUI.windowManager.Close(XUiC_KeypadWindow.ID);
		}
		base.Update(_dt);
	}

	// Token: 0x06007C43 RID: 31811 RVA: 0x002F17C0 File Offset: 0x002EF9C0
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.playerUI.entityPlayer.PlayOneShot("open_sign", false, false, false, null, 1f);
	}

	// Token: 0x06007C44 RID: 31812 RVA: 0x002F17EC File Offset: 0x002EF9EC
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

	// Token: 0x04005C01 RID: 23553
	public static string ID = "";

	// Token: 0x04005C02 RID: 23554
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtPassword;

	// Token: 0x04005C03 RID: 23555
	[PublicizedFrom(EAccessModifier.Private)]
	public ILockable LockedItem;

	// Token: 0x04005C04 RID: 23556
	[PublicizedFrom(EAccessModifier.Private)]
	public Action onCloseCallback;

	// Token: 0x04005C05 RID: 23557
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<bool> checkInteraction;
}
