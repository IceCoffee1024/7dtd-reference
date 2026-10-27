using System;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E2A RID: 3626
[Preserve]
public class XUiC_LoginBase : XUiController
{
	// Token: 0x17000C69 RID: 3177
	// (get) Token: 0x06006E82 RID: 28290 RVA: 0x002B1210 File Offset: 0x002AF410
	public virtual string MsgTitleKey
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return "xuiSteamLogin";
		}
	}

	// Token: 0x17000C6A RID: 3178
	// (get) Token: 0x06006E83 RID: 28291 RVA: 0x002B1217 File Offset: 0x002AF417
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

	// Token: 0x17000C6B RID: 3179
	// (get) Token: 0x06006E84 RID: 28292 RVA: 0x002B1244 File Offset: 0x002AF444
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

	// Token: 0x17000C6C RID: 3180
	// (get) Token: 0x06006E85 RID: 28293 RVA: 0x002B1270 File Offset: 0x002AF470
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

	// Token: 0x06006E86 RID: 28294 RVA: 0x002B12BD File Offset: 0x002AF4BD
	public override void Init()
	{
		base.Init();
		XUiC_LoginBase.ID = base.WindowGroup.Id;
	}

	// Token: 0x06006E87 RID: 28295 RVA: 0x002B12D8 File Offset: 0x002AF4D8
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

	// Token: 0x06006E88 RID: 28296 RVA: 0x002B137C File Offset: 0x002AF57C
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

	// Token: 0x06006E89 RID: 28297 RVA: 0x0007EBCB File Offset: 0x0007CDCB
	[XuiBindEvent("OnPress", "btnExit")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnExit_OnPressed(XUiController _sender, int _mouseButton)
	{
		Application.Quit();
	}

	// Token: 0x06006E8A RID: 28298 RVA: 0x002B13F4 File Offset: 0x002AF5F4
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

	// Token: 0x06006E8B RID: 28299 RVA: 0x002B14E9 File Offset: 0x002AF6E9
	[PublicizedFrom(EAccessModifier.Private)]
	public static void Open(XUi _xuiInstance, IPlatform _platform, EApiStatusReason _statusReason, string _statusReasonAdditionalText, Action _onLoginComplete)
	{
		XUiC_LoginBase childByType = _xuiInstance.FindWindowGroupByName(XUiC_LoginBase.ID).GetChildByType<XUiC_LoginBase>();
		childByType.onLoginComplete = _onLoginComplete;
		childByType.updateState(_platform, _statusReason, _statusReasonAdditionalText);
	}

	// Token: 0x06006E8C RID: 28300 RVA: 0x002B150C File Offset: 0x002AF70C
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

	// Token: 0x04005447 RID: 21575
	[PublicizedFrom(EAccessModifier.Private)]
	public static string ID = "";

	// Token: 0x04005448 RID: 21576
	[XuiBindComponent("btnRetry", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnRetry;

	// Token: 0x04005449 RID: 21577
	[XuiBindComponent("btnOffline", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnOffline;

	// Token: 0x0400544A RID: 21578
	[XuiBindComponent("btnExit", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnExit;

	// Token: 0x0400544B RID: 21579
	[PublicizedFrom(EAccessModifier.Private)]
	public IPlatform offendingPlatform;

	// Token: 0x0400544C RID: 21580
	[PublicizedFrom(EAccessModifier.Private)]
	public EApiStatusReason statusReason;

	// Token: 0x0400544D RID: 21581
	[PublicizedFrom(EAccessModifier.Private)]
	public string statusReasonAdditionalText;

	// Token: 0x0400544E RID: 21582
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wantOffline;

	// Token: 0x0400544F RID: 21583
	[PublicizedFrom(EAccessModifier.Private)]
	public Action onLoginComplete;
}
