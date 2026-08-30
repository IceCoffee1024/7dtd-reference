using System;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000DFB RID: 3579
[Preserve]
public class XUiC_LoginBase : XUiController
{
	// Token: 0x17000C2F RID: 3119
	// (get) Token: 0x06006DC5 RID: 28101 RVA: 0x002A93E8 File Offset: 0x002A75E8
	public virtual string MsgTitleKey
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return "xuiSteamLogin";
		}
	}

	// Token: 0x17000C30 RID: 3120
	// (get) Token: 0x06006DC6 RID: 28102 RVA: 0x002A93EF File Offset: 0x002A75EF
	[XuiXmlBinding("title")]
	public string MsgTitle
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.offendingPlatform != null)
			{
				return string.Format(Localization.Get(this.MsgTitleKey, false, null), this.offendingPlatform.PlatformDisplayName);
			}
			return "";
		}
	}

	// Token: 0x17000C31 RID: 3121
	// (get) Token: 0x06006DC7 RID: 28103 RVA: 0x002A941C File Offset: 0x002A761C
	[XuiXmlBinding("caption")]
	public string MsgCaption
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.offendingPlatform != null)
			{
				return string.Format(Localization.Get("xuiSteamLoginFailure", false, null), this.offendingPlatform.PlatformDisplayName);
			}
			return "";
		}
	}

	// Token: 0x17000C32 RID: 3122
	// (get) Token: 0x06006DC8 RID: 28104 RVA: 0x002A9448 File Offset: 0x002A7648
	[XuiXmlBinding("reason")]
	public string MsgReason
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.offendingPlatform != null)
			{
				return string.Format(Localization.Get("xuiSteamLoginReason" + this.statusReason.ToStringCached<EApiStatusReason>(), false, null), this.offendingPlatform.PlatformDisplayName, this.statusReasonAdditionalText);
			}
			return "";
		}
	}

	// Token: 0x06006DC9 RID: 28105 RVA: 0x002A9495 File Offset: 0x002A7695
	public override void Init()
	{
		base.Init();
		XUiC_LoginBase.ID = base.WindowGroup.Id;
	}

	// Token: 0x06006DCA RID: 28106 RVA: 0x002A94B0 File Offset: 0x002A76B0
	[XuiBindEvent("OnPress", "btnRetry")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnRetry_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.btnOffline.ViewComponent.Enabled = false;
		this.btnRetry.ViewComponent.Enabled = false;
		if (this.btnExit != null)
		{
			this.btnExit.ViewComponent.Enabled = false;
		}
		this.offendingPlatform = null;
		this.wantOffline = false;
		base.RefreshBindings();
		_sender.xui.playerUI.nguiWindowManager.SetLabelText(EnumNGUIWindow.Loading, Localization.Get("xuiSteamLoginProgressSignIn", false, null) + "...", false);
		PlatformManager.MultiPlatform.User.Login(new LoginUserCallback(this.updateState));
	}

	// Token: 0x06006DCB RID: 28107 RVA: 0x002A9554 File Offset: 0x002A7754
	[XuiBindEvent("OnPress", "btnOffline")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnOffline_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.btnOffline.ViewComponent.Enabled = false;
		this.btnRetry.ViewComponent.Enabled = false;
		if (this.btnExit != null)
		{
			this.btnExit.ViewComponent.Enabled = false;
		}
		this.offendingPlatform = null;
		base.RefreshBindings();
		this.wantOffline = true;
		PlatformManager.MultiPlatform.User.PlayOffline(new LoginUserCallback(this.updateState));
	}

	// Token: 0x06006DCC RID: 28108 RVA: 0x0007E0EF File Offset: 0x0007C2EF
	[XuiBindEvent("OnPress", "btnExit")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnExit_OnPressed(XUiController _sender, int _mouseButton)
	{
		Application.Quit();
	}

	// Token: 0x06006DCD RID: 28109 RVA: 0x002A95CC File Offset: 0x002A77CC
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateState(IPlatform _platform, EApiStatusReason _statusReason, string _statusReasonAdditionalText)
	{
		if (_platform.User.UserStatus == EUserStatus.LoggedIn || (this.wantOffline && _platform.User.UserStatus == EUserStatus.OfflineMode))
		{
			this.xui.playerUI.windowManager.Close(this.windowGroup, false);
			Action action = this.onLoginComplete;
			if (action != null)
			{
				action();
			}
			this.onLoginComplete = null;
			return;
		}
		this.xui.playerUI.windowManager.Open(this.windowGroup, true, true);
		this.btnRetry.ViewComponent.Enabled = _platform.Api.ClientApiStatus != EApiStatus.PermanentError;
		this.btnOffline.ViewComponent.Enabled = _platform.User.UserStatus == EUserStatus.OfflineMode;
		if (this.btnExit != null)
		{
			this.btnExit.ViewComponent.Enabled = true;
		}
		this.offendingPlatform = _platform;
		this.statusReason = _statusReason;
		this.statusReasonAdditionalText = _statusReasonAdditionalText;
		base.RefreshBindings();
	}

	// Token: 0x06006DCE RID: 28110 RVA: 0x002A96C1 File Offset: 0x002A78C1
	[PublicizedFrom(EAccessModifier.Private)]
	public static void Open(XUi _xuiInstance, IPlatform _platform, EApiStatusReason _statusReason, string _statusReasonAdditionalText, Action _onLoginComplete)
	{
		XUiC_LoginBase childByType = _xuiInstance.FindWindowGroupByName(XUiC_LoginBase.ID).GetChildByType<XUiC_LoginBase>();
		childByType.onLoginComplete = _onLoginComplete;
		childByType.updateState(_platform, _statusReason, _statusReasonAdditionalText);
	}

	// Token: 0x06006DCF RID: 28111 RVA: 0x002A96E4 File Offset: 0x002A78E4
	public static void Login(XUi _xuiInstance, Action _onLoginComplete)
	{
		_xuiInstance.playerUI.nguiWindowManager.SetLabelText(EnumNGUIWindow.Loading, Localization.Get("xuiSteamLoginProgressSignIn", false, null) + "...", false);
		PlatformManager.MultiPlatform.User.Login(delegate(IPlatform _platform, EApiStatusReason _statusReason, string _statusReasonAdditionalText)
		{
			if (_platform.Api.ClientApiStatus != EApiStatus.Ok)
			{
				XUiC_LoginBase.Open(_xuiInstance, _platform, _statusReason, _statusReasonAdditionalText, _onLoginComplete);
				return;
			}
			if (_platform.User.UserStatus != EUserStatus.LoggedIn || _statusReason != EApiStatusReason.Ok)
			{
				XUiC_LoginBase.Open(_xuiInstance, _platform, _statusReason, _statusReasonAdditionalText, _onLoginComplete);
				return;
			}
			_onLoginComplete();
		});
	}

	// Token: 0x04005348 RID: 21320
	[PublicizedFrom(EAccessModifier.Private)]
	public static string ID = "";

	// Token: 0x04005349 RID: 21321
	[XuiBindComponent("btnRetry", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnRetry;

	// Token: 0x0400534A RID: 21322
	[XuiBindComponent("btnOffline", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnOffline;

	// Token: 0x0400534B RID: 21323
	[XuiBindComponent("btnExit", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnExit;

	// Token: 0x0400534C RID: 21324
	[PublicizedFrom(EAccessModifier.Private)]
	public IPlatform offendingPlatform;

	// Token: 0x0400534D RID: 21325
	[PublicizedFrom(EAccessModifier.Private)]
	public EApiStatusReason statusReason;

	// Token: 0x0400534E RID: 21326
	[PublicizedFrom(EAccessModifier.Private)]
	public string statusReasonAdditionalText;

	// Token: 0x0400534F RID: 21327
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wantOffline;

	// Token: 0x04005350 RID: 21328
	[PublicizedFrom(EAccessModifier.Private)]
	public Action onLoginComplete;
}
