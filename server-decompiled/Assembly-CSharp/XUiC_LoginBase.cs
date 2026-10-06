using System;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E1F RID: 3615
[Preserve]
public class XUiC_LoginBase : XUiController
{
	// Token: 0x17000C63 RID: 3171
	// (get) Token: 0x06006E5D RID: 28253 RVA: 0x002AFCA4 File Offset: 0x002ADEA4
	public virtual string MsgTitleKey
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return "xuiSteamLogin";
		}
	}

	// Token: 0x17000C64 RID: 3172
	// (get) Token: 0x06006E5E RID: 28254 RVA: 0x002AFCAB File Offset: 0x002ADEAB
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

	// Token: 0x17000C65 RID: 3173
	// (get) Token: 0x06006E5F RID: 28255 RVA: 0x002AFCD8 File Offset: 0x002ADED8
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

	// Token: 0x17000C66 RID: 3174
	// (get) Token: 0x06006E60 RID: 28256 RVA: 0x002AFD04 File Offset: 0x002ADF04
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

	// Token: 0x06006E61 RID: 28257 RVA: 0x002AFD51 File Offset: 0x002ADF51
	public override void Init()
	{
		base.Init();
		XUiC_LoginBase.ID = base.WindowGroup.Id;
	}

	// Token: 0x06006E62 RID: 28258 RVA: 0x002AFD6C File Offset: 0x002ADF6C
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

	// Token: 0x06006E63 RID: 28259 RVA: 0x002AFE10 File Offset: 0x002AE010
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

	// Token: 0x06006E64 RID: 28260 RVA: 0x0007E9FB File Offset: 0x0007CBFB
	[XuiBindEvent("OnPress", "btnExit")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnExit_OnPressed(XUiController _sender, int _mouseButton)
	{
		Application.Quit();
	}

	// Token: 0x06006E65 RID: 28261 RVA: 0x002AFE88 File Offset: 0x002AE088
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

	// Token: 0x06006E66 RID: 28262 RVA: 0x002AFF7D File Offset: 0x002AE17D
	[PublicizedFrom(EAccessModifier.Private)]
	public static void Open(XUi _xuiInstance, IPlatform _platform, EApiStatusReason _statusReason, string _statusReasonAdditionalText, Action _onLoginComplete)
	{
		XUiC_LoginBase childByType = _xuiInstance.FindWindowGroupByName(XUiC_LoginBase.ID).GetChildByType<XUiC_LoginBase>();
		childByType.onLoginComplete = _onLoginComplete;
		childByType.updateState(_platform, _statusReason, _statusReasonAdditionalText);
	}

	// Token: 0x06006E67 RID: 28263 RVA: 0x002AFFA0 File Offset: 0x002AE1A0
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

	// Token: 0x04005422 RID: 21538
	[PublicizedFrom(EAccessModifier.Private)]
	public static string ID = "";

	// Token: 0x04005423 RID: 21539
	[XuiBindComponent("btnRetry", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnRetry;

	// Token: 0x04005424 RID: 21540
	[XuiBindComponent("btnOffline", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnOffline;

	// Token: 0x04005425 RID: 21541
	[XuiBindComponent("btnExit", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnExit;

	// Token: 0x04005426 RID: 21542
	[PublicizedFrom(EAccessModifier.Private)]
	public IPlatform offendingPlatform;

	// Token: 0x04005427 RID: 21543
	[PublicizedFrom(EAccessModifier.Private)]
	public EApiStatusReason statusReason;

	// Token: 0x04005428 RID: 21544
	[PublicizedFrom(EAccessModifier.Private)]
	public string statusReasonAdditionalText;

	// Token: 0x04005429 RID: 21545
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wantOffline;

	// Token: 0x0400542A RID: 21546
	[PublicizedFrom(EAccessModifier.Private)]
	public Action onLoginComplete;
}
