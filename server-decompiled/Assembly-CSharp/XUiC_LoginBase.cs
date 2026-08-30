using System;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E10 RID: 3600
[Preserve]
public class XUiC_LoginBase : XUiController
{
	// Token: 0x17000C48 RID: 3144
	// (get) Token: 0x06006E65 RID: 28261 RVA: 0x002AC8FC File Offset: 0x002AAAFC
	public virtual string MsgTitleKey
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return "xuiSteamLogin";
		}
	}

	// Token: 0x17000C49 RID: 3145
	// (get) Token: 0x06006E66 RID: 28262 RVA: 0x002AC903 File Offset: 0x002AAB03
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

	// Token: 0x17000C4A RID: 3146
	// (get) Token: 0x06006E67 RID: 28263 RVA: 0x002AC930 File Offset: 0x002AAB30
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

	// Token: 0x17000C4B RID: 3147
	// (get) Token: 0x06006E68 RID: 28264 RVA: 0x002AC95C File Offset: 0x002AAB5C
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

	// Token: 0x06006E69 RID: 28265 RVA: 0x002AC9A9 File Offset: 0x002AABA9
	public override void Init()
	{
		base.Init();
		XUiC_LoginBase.ID = base.WindowGroup.Id;
	}

	// Token: 0x06006E6A RID: 28266 RVA: 0x002AC9C4 File Offset: 0x002AABC4
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

	// Token: 0x06006E6B RID: 28267 RVA: 0x002ACA68 File Offset: 0x002AAC68
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

	// Token: 0x06006E6C RID: 28268 RVA: 0x0007E747 File Offset: 0x0007C947
	[XuiBindEvent("OnPress", "btnExit")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnExit_OnPressed(XUiController _sender, int _mouseButton)
	{
		Application.Quit();
	}

	// Token: 0x06006E6D RID: 28269 RVA: 0x002ACAE0 File Offset: 0x002AACE0
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

	// Token: 0x06006E6E RID: 28270 RVA: 0x002ACBD5 File Offset: 0x002AADD5
	[PublicizedFrom(EAccessModifier.Private)]
	public static void Open(XUi _xuiInstance, IPlatform _platform, EApiStatusReason _statusReason, string _statusReasonAdditionalText, Action _onLoginComplete)
	{
		XUiC_LoginBase childByType = _xuiInstance.FindWindowGroupByName(XUiC_LoginBase.ID).GetChildByType<XUiC_LoginBase>();
		childByType.onLoginComplete = _onLoginComplete;
		childByType.updateState(_platform, _statusReason, _statusReasonAdditionalText);
	}

	// Token: 0x06006E6F RID: 28271 RVA: 0x002ACBF8 File Offset: 0x002AADF8
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

	// Token: 0x040053EB RID: 21483
	[PublicizedFrom(EAccessModifier.Private)]
	public static string ID = "";

	// Token: 0x040053EC RID: 21484
	[XuiBindComponent("btnRetry", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnRetry;

	// Token: 0x040053ED RID: 21485
	[XuiBindComponent("btnOffline", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnOffline;

	// Token: 0x040053EE RID: 21486
	[XuiBindComponent("btnExit", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnExit;

	// Token: 0x040053EF RID: 21487
	[PublicizedFrom(EAccessModifier.Private)]
	public IPlatform offendingPlatform;

	// Token: 0x040053F0 RID: 21488
	[PublicizedFrom(EAccessModifier.Private)]
	public EApiStatusReason statusReason;

	// Token: 0x040053F1 RID: 21489
	[PublicizedFrom(EAccessModifier.Private)]
	public string statusReasonAdditionalText;

	// Token: 0x040053F2 RID: 21490
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wantOffline;

	// Token: 0x040053F3 RID: 21491
	[PublicizedFrom(EAccessModifier.Private)]
	public Action onLoginComplete;
}
