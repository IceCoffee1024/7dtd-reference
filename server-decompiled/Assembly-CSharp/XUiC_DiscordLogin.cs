using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

// Token: 0x0200034F RID: 847
[Preserve]
public class XUiC_DiscordLogin : XUiController
{
	// Token: 0x170002DE RID: 734
	// (get) Token: 0x06001857 RID: 6231 RVA: 0x0008AE90 File Offset: 0x00089090
	// (set) Token: 0x06001858 RID: 6232 RVA: 0x0008AEA1 File Offset: 0x000890A1
	[XuiXmlBinding("status_text")]
	public string StatusText
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.statusText ?? "";
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.statusText = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x170002DF RID: 735
	// (get) Token: 0x06001859 RID: 6233 RVA: 0x0008AEB1 File Offset: 0x000890B1
	// (set) Token: 0x0600185A RID: 6234 RVA: 0x0008AEB9 File Offset: 0x000890B9
	[XuiXmlBinding("show_settings_button")]
	public bool ShowSettingsButton
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.showSettingsButton;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.showSettingsButton = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x170002E0 RID: 736
	// (get) Token: 0x0600185B RID: 6235 RVA: 0x0008AEC9 File Offset: 0x000890C9
	// (set) Token: 0x0600185C RID: 6236 RVA: 0x0008AED1 File Offset: 0x000890D1
	[XuiXmlBinding("done")]
	public bool IsDone
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.isDone;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.isDone = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x170002E1 RID: 737
	// (get) Token: 0x0600185D RID: 6237 RVA: 0x0008AEE1 File Offset: 0x000890E1
	// (set) Token: 0x0600185E RID: 6238 RVA: 0x0008AEE9 File Offset: 0x000890E9
	[XuiXmlBinding("cancellable")]
	public bool Cancellable
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.cancellable;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.cancellable = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x170002E2 RID: 738
	// (get) Token: 0x0600185F RID: 6239 RVA: 0x0008AEF9 File Offset: 0x000890F9
	// (set) Token: 0x06001860 RID: 6240 RVA: 0x0008AF01 File Offset: 0x00089101
	[XuiXmlBinding("account_login_result")]
	public DiscordManager.EFullAccountLoginResult AccountLoginResult
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.accountLoginResult;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.accountLoginResult = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x170002E3 RID: 739
	// (get) Token: 0x06001861 RID: 6241 RVA: 0x0008AF11 File Offset: 0x00089111
	// (set) Token: 0x06001862 RID: 6242 RVA: 0x0008AF19 File Offset: 0x00089119
	[XuiXmlBinding("provisional_login_result")]
	public DiscordManager.EProvisionalAccountLoginResult ProvisionalLoginResult
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.provisionalLoginResult;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.provisionalLoginResult = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x170002E4 RID: 740
	// (get) Token: 0x06001863 RID: 6243 RVA: 0x0008A889 File Offset: 0x00088A89
	[XuiXmlBinding("discord_supports_provisional_accounts")]
	public bool DiscordSupportsProvisionalAccounts
	{
		get
		{
			return DiscordManager.SupportsProvisionalAccounts;
		}
	}

	// Token: 0x06001864 RID: 6244 RVA: 0x0008AF29 File Offset: 0x00089129
	public override void Init()
	{
		base.Init();
		XUiC_DiscordLogin.ID = base.WindowGroup.Id;
	}

	// Token: 0x06001865 RID: 6245 RVA: 0x0008AF41 File Offset: 0x00089141
	[XuiBindEvent("OnPress", "btnOk")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnOk_OnPress(XUiController _sender, int _mouseButton)
	{
		this.closeAndOpenNextWindow();
	}

	// Token: 0x06001866 RID: 6246 RVA: 0x0008AF49 File Offset: 0x00089149
	[XuiBindEvent("OnPress", "btnCancel")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnCancel_OnPress(XUiController _sender, int _mouseButton)
	{
		DiscordManager.Instance.AuthManager.AbortAuth();
	}

	// Token: 0x06001867 RID: 6247 RVA: 0x0008AF5A File Offset: 0x0008915A
	[XuiBindEvent("OnPress", "btnRequestAuth")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnRequestAuth_OnPress(XUiController _sender, int _mouseButton)
	{
		this.resetStates();
		DiscordManager.Instance.AuthManager.LoginDiscordUser();
	}

	// Token: 0x06001868 RID: 6248 RVA: 0x0008AF71 File Offset: 0x00089171
	[XuiBindEvent("OnPress", "btnUnlink")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnUnlink_OnPress(XUiController _sender, int _mouseButton)
	{
		this.resetStates();
		DiscordManager.Instance.AuthManager.UnmergeAccount();
	}

	// Token: 0x06001869 RID: 6249 RVA: 0x0008A7AF File Offset: 0x000889AF
	[XuiBindEvent("OnPress", "btnSettings")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnSettings_OnPress(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		XUiC_OptionsAudio childByType = this.xui.GetChildByType<XUiC_OptionsAudio>();
		if (childByType == null)
		{
			return;
		}
		childByType.OpenAtTab("xuiOptionsAudioDiscord");
	}

	// Token: 0x0600186A RID: 6250 RVA: 0x0008AF88 File Offset: 0x00089188
	[PublicizedFrom(EAccessModifier.Private)]
	public void resetStates()
	{
		this.StatusText = Localization.Get("xuiDiscordLoginLoggingIn", false, null);
		this.IsDone = false;
		this.AccountLoginResult = DiscordManager.EFullAccountLoginResult.None;
		this.ProvisionalLoginResult = DiscordManager.EProvisionalAccountLoginResult.None;
	}

	// Token: 0x0600186B RID: 6251 RVA: 0x0008AFB4 File Offset: 0x000891B4
	[PublicizedFrom(EAccessModifier.Private)]
	public void closeAndOpenNextWindow()
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		if (this.onCloseWithOk == null)
		{
			this.xui.playerUI.windowManager.Open(XUiC_MainMenu.ID, true);
			return;
		}
		this.onCloseWithOk();
	}

	// Token: 0x0600186C RID: 6252 RVA: 0x0008B00C File Offset: 0x0008920C
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.playerUI.nguiWindowManager.Show(EnumNGUIWindow.Loading, false);
		this.windowGroup.isEscClosable = false;
	}

	// Token: 0x0600186D RID: 6253 RVA: 0x0008B037 File Offset: 0x00089237
	public override void OnClose()
	{
		base.OnClose();
		DiscordManager.Instance.UserAuthorizationResult -= this.authResult;
	}

	// Token: 0x0600186E RID: 6254 RVA: 0x0008B055 File Offset: 0x00089255
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.updateInput();
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x0600186F RID: 6255 RVA: 0x0008B06C File Offset: 0x0008926C
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateInput()
	{
		if (!this.IsDone)
		{
			return;
		}
		PlayerActionsGUI guiactions = this.xui.playerUI.playerInput.GUIActions;
		if (guiactions.Apply.WasReleased || guiactions.Cancel.WasReleased)
		{
			this.closeAndOpenNextWindow();
		}
	}

	// Token: 0x06001870 RID: 6256 RVA: 0x0008B0B8 File Offset: 0x000892B8
	[PublicizedFrom(EAccessModifier.Private)]
	public void authResult(bool _isDone, DiscordManager.EFullAccountLoginResult _fullAccResult, DiscordManager.EProvisionalAccountLoginResult _provisionalAccResult, bool _isExpectedSuccess)
	{
		LocalPlayerUI playerUI = this.xui.playerUI;
		playerUI.nguiWindowManager.Show(EnumNGUIWindow.Loading, false);
		if (!this.windowGroup.isShowing && (_isDone || _fullAccResult == DiscordManager.EFullAccountLoginResult.RequestingAuth))
		{
			if (this.skipOnSuccess && _isExpectedSuccess)
			{
				DiscordManager.Instance.UserAuthorizationResult -= this.authResult;
				this.closeAndOpenNextWindow();
				return;
			}
			XUiC_DiscordLogin.openNow(playerUI, this.openModal);
		}
		this.updateUi(_isDone, _fullAccResult, _provisionalAccResult, _isExpectedSuccess);
	}

	// Token: 0x06001871 RID: 6257 RVA: 0x0008B134 File Offset: 0x00089334
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateUi(bool _isDone, DiscordManager.EFullAccountLoginResult _fullAccResult, DiscordManager.EProvisionalAccountLoginResult _provisionalAccResult, bool _isExpectedSuccess)
	{
		XUiC_DiscordLogin.<>c__DisplayClass48_0 CS$<>8__locals1;
		CS$<>8__locals1._isDone = _isDone;
		CS$<>8__locals1._fullAccResult = _fullAccResult;
		CS$<>8__locals1._provisionalAccResult = _provisionalAccResult;
		this.IsDone = CS$<>8__locals1._isDone;
		this.Cancellable = CS$<>8__locals1._fullAccResult == DiscordManager.EFullAccountLoginResult.RequestingAuth;
		this.StatusText = XUiC_DiscordLogin.<updateUi>g__BuildText|48_0(ref CS$<>8__locals1);
		this.AccountLoginResult = CS$<>8__locals1._fullAccResult;
		this.ProvisionalLoginResult = CS$<>8__locals1._provisionalAccResult;
		base.RefreshBindings();
	}

	// Token: 0x06001872 RID: 6258 RVA: 0x0008B1A0 File Offset: 0x000893A0
	public static void Open(Action _onCloseWithOk = null, bool _showSettingsButton = true, bool _waitForResultToShow = false, bool _skipOnSuccess = false, bool _modal = true, bool _cancellable = false)
	{
		XUi xui = LocalPlayerUI.primaryUI.xui;
		LocalPlayerUI playerUI = xui.playerUI;
		XUiC_DiscordLogin childByType = xui.GetWindowGroupById(XUiC_DiscordLogin.ID).Controller.GetChildByType<XUiC_DiscordLogin>();
		DiscordManager.Instance.UserAuthorizationResult += childByType.authResult;
		childByType.resetStates();
		childByType.Cancellable = _cancellable;
		childByType.onCloseWithOk = _onCloseWithOk;
		childByType.ShowSettingsButton = _showSettingsButton;
		childByType.skipOnSuccess = _skipOnSuccess;
		childByType.openModal = _modal;
		if (_waitForResultToShow)
		{
			playerUI.nguiWindowManager.SetLabelText(EnumNGUIWindow.Loading, Localization.Get("xuiDiscordLoginProgress", false, null) + "...", false);
			return;
		}
		XUiC_DiscordLogin.openNow(playerUI, _modal);
	}

	// Token: 0x06001873 RID: 6259 RVA: 0x0008B243 File Offset: 0x00089443
	[PublicizedFrom(EAccessModifier.Private)]
	public static void openNow(LocalPlayerUI _playerUi, bool _modal = true)
	{
		_playerUi.windowManager.Open(XUiC_DiscordLogin.ID, _modal, true);
	}

	// Token: 0x06001876 RID: 6262 RVA: 0x0008B264 File Offset: 0x00089464
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static string <updateUi>g__BuildText|48_0(ref XUiC_DiscordLogin.<>c__DisplayClass48_0 A_0)
	{
		if (!A_0._isDone)
		{
			switch (A_0._fullAccResult)
			{
			case DiscordManager.EFullAccountLoginResult.RequestingAuth:
				return Localization.Get("xuiDiscordLoginFullAccountRequestingAuth", false, null);
			case DiscordManager.EFullAccountLoginResult.AuthAccepted:
				return Localization.Get("xuiDiscordLoginFullAccountAuthAccepted", false, null);
			case DiscordManager.EFullAccountLoginResult.AuthCancelled:
				return Localization.Get("xuiDiscordLoginFullAccountAuthCancelledTryingProvisional", false, null);
			case DiscordManager.EFullAccountLoginResult.AuthFailed:
				return Localization.Get("xuiDiscordLoginFullAccountAuthFailedTryingProvisional", false, null);
			}
		}
		if (A_0._fullAccResult == DiscordManager.EFullAccountLoginResult.None)
		{
			return XUiC_DiscordLogin.<updateUi>g__BuildProvisionalOnlyText|48_3(ref A_0);
		}
		if (A_0._fullAccResult == DiscordManager.EFullAccountLoginResult.Success)
		{
			return Localization.Get("xuiDiscordLoginFullAccountSuccess", false, null);
		}
		return XUiC_DiscordLogin.<updateUi>g__BuildFullAccFailureText|48_1(ref A_0);
	}

	// Token: 0x06001877 RID: 6263 RVA: 0x0008B2FC File Offset: 0x000894FC
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static string <updateUi>g__BuildFullAccFailureText|48_1(ref XUiC_DiscordLogin.<>c__DisplayClass48_0 A_0)
	{
		string text = Localization.Get("xuiDiscordLoginFullAccount" + A_0._fullAccResult.ToStringCached<DiscordManager.EFullAccountLoginResult>(), false, null);
		if (A_0._provisionalAccResult == DiscordManager.EProvisionalAccountLoginResult.None || A_0._provisionalAccResult == DiscordManager.EProvisionalAccountLoginResult.NotSupported)
		{
			return text;
		}
		return XUiC_DiscordLogin.<updateUi>g__BuildProvisionalFallbackText|48_2(text, ref A_0);
	}

	// Token: 0x06001878 RID: 6264 RVA: 0x0008B340 File Offset: 0x00089540
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static string <updateUi>g__BuildProvisionalFallbackText|48_2(string _fullAccFailureText, ref XUiC_DiscordLogin.<>c__DisplayClass48_0 A_1)
	{
		string text = Localization.Get("xuiDiscordLoginFallback" + A_1._provisionalAccResult.ToStringCached<DiscordManager.EProvisionalAccountLoginResult>(), false, null);
		return _fullAccFailureText + "\n\n" + text;
	}

	// Token: 0x06001879 RID: 6265 RVA: 0x0008B376 File Offset: 0x00089576
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static string <updateUi>g__BuildProvisionalOnlyText|48_3(ref XUiC_DiscordLogin.<>c__DisplayClass48_0 A_0)
	{
		return Localization.Get("xuiDiscordLoginProvisional" + A_0._provisionalAccResult.ToStringCached<DiscordManager.EProvisionalAccountLoginResult>(), false, null);
	}

	// Token: 0x04000F9F RID: 3999
	[PublicizedFrom(EAccessModifier.Private)]
	public static string ID = "";

	// Token: 0x04000FA0 RID: 4000
	[XuiBindComponent("btnOk", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnOk;

	// Token: 0x04000FA1 RID: 4001
	[XuiBindComponent("btnCancel", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCancel;

	// Token: 0x04000FA2 RID: 4002
	[XuiBindComponent("btnRequestAuth", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnRequestAuth;

	// Token: 0x04000FA3 RID: 4003
	[XuiBindComponent("btnUnlink", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnUnlink;

	// Token: 0x04000FA4 RID: 4004
	[XuiBindComponent("btnSettings", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnSettings;

	// Token: 0x04000FA5 RID: 4005
	[PublicizedFrom(EAccessModifier.Private)]
	public Action onCloseWithOk;

	// Token: 0x04000FA6 RID: 4006
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showSettingsButton;

	// Token: 0x04000FA7 RID: 4007
	[PublicizedFrom(EAccessModifier.Private)]
	public bool skipOnSuccess;

	// Token: 0x04000FA8 RID: 4008
	[PublicizedFrom(EAccessModifier.Private)]
	public bool openModal;

	// Token: 0x04000FA9 RID: 4009
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isDone;

	// Token: 0x04000FAA RID: 4010
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cancellable;

	// Token: 0x04000FAB RID: 4011
	[PublicizedFrom(EAccessModifier.Private)]
	public string statusText;

	// Token: 0x04000FAC RID: 4012
	[PublicizedFrom(EAccessModifier.Private)]
	public DiscordManager.EFullAccountLoginResult accountLoginResult;

	// Token: 0x04000FAD RID: 4013
	[PublicizedFrom(EAccessModifier.Private)]
	public DiscordManager.EProvisionalAccountLoginResult provisionalLoginResult;
}
