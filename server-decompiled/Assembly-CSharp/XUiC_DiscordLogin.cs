using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

// Token: 0x0200034C RID: 844
[Preserve]
public class XUiC_DiscordLogin : XUiController
{
	// Token: 0x170002E6 RID: 742
	// (get) Token: 0x0600185C RID: 6236 RVA: 0x0008AC10 File Offset: 0x00088E10
	// (set) Token: 0x0600185D RID: 6237 RVA: 0x0008AC21 File Offset: 0x00088E21
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

	// Token: 0x170002E7 RID: 743
	// (get) Token: 0x0600185E RID: 6238 RVA: 0x0008AC31 File Offset: 0x00088E31
	// (set) Token: 0x0600185F RID: 6239 RVA: 0x0008AC39 File Offset: 0x00088E39
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

	// Token: 0x170002E8 RID: 744
	// (get) Token: 0x06001860 RID: 6240 RVA: 0x0008AC49 File Offset: 0x00088E49
	// (set) Token: 0x06001861 RID: 6241 RVA: 0x0008AC51 File Offset: 0x00088E51
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

	// Token: 0x170002E9 RID: 745
	// (get) Token: 0x06001862 RID: 6242 RVA: 0x0008AC61 File Offset: 0x00088E61
	// (set) Token: 0x06001863 RID: 6243 RVA: 0x0008AC69 File Offset: 0x00088E69
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

	// Token: 0x170002EA RID: 746
	// (get) Token: 0x06001864 RID: 6244 RVA: 0x0008AC79 File Offset: 0x00088E79
	// (set) Token: 0x06001865 RID: 6245 RVA: 0x0008AC81 File Offset: 0x00088E81
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

	// Token: 0x170002EB RID: 747
	// (get) Token: 0x06001866 RID: 6246 RVA: 0x0008AC91 File Offset: 0x00088E91
	// (set) Token: 0x06001867 RID: 6247 RVA: 0x0008AC99 File Offset: 0x00088E99
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

	// Token: 0x170002EC RID: 748
	// (get) Token: 0x06001868 RID: 6248 RVA: 0x0008A609 File Offset: 0x00088809
	[XuiXmlBinding("discord_supports_provisional_accounts")]
	public bool DiscordSupportsProvisionalAccounts
	{
		get
		{
			return DiscordManager.SupportsProvisionalAccounts;
		}
	}

	// Token: 0x06001869 RID: 6249 RVA: 0x0008ACA9 File Offset: 0x00088EA9
	public override void Init()
	{
		base.Init();
		XUiC_DiscordLogin.ID = base.WindowGroup.Id;
	}

	// Token: 0x0600186A RID: 6250 RVA: 0x0008ACC1 File Offset: 0x00088EC1
	[XuiBindEvent("OnPress", "btnOk")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnOk_OnPress(XUiController _sender, int _mouseButton)
	{
		this.closeAndOpenNextWindow();
	}

	// Token: 0x0600186B RID: 6251 RVA: 0x0008ACC9 File Offset: 0x00088EC9
	[XuiBindEvent("OnPress", "btnCancel")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnCancel_OnPress(XUiController _sender, int _mouseButton)
	{
		DiscordManager.Instance.AuthManager.AbortAuth();
	}

	// Token: 0x0600186C RID: 6252 RVA: 0x0008ACDA File Offset: 0x00088EDA
	[XuiBindEvent("OnPress", "btnRequestAuth")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnRequestAuth_OnPress(XUiController _sender, int _mouseButton)
	{
		this.resetStates();
		DiscordManager.Instance.AuthManager.LoginDiscordUser();
	}

	// Token: 0x0600186D RID: 6253 RVA: 0x0008ACF1 File Offset: 0x00088EF1
	[XuiBindEvent("OnPress", "btnUnlink")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnUnlink_OnPress(XUiController _sender, int _mouseButton)
	{
		this.resetStates();
		DiscordManager.Instance.AuthManager.UnmergeAccount();
	}

	// Token: 0x0600186E RID: 6254 RVA: 0x0008A52F File Offset: 0x0008872F
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

	// Token: 0x0600186F RID: 6255 RVA: 0x0008AD08 File Offset: 0x00088F08
	[PublicizedFrom(EAccessModifier.Private)]
	public void resetStates()
	{
		this.StatusText = Localization.Get("xuiDiscordLoginLoggingIn", false, null);
		this.IsDone = false;
		this.AccountLoginResult = DiscordManager.EFullAccountLoginResult.None;
		this.ProvisionalLoginResult = DiscordManager.EProvisionalAccountLoginResult.None;
	}

	// Token: 0x06001870 RID: 6256 RVA: 0x0008AD34 File Offset: 0x00088F34
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

	// Token: 0x06001871 RID: 6257 RVA: 0x0008AD8C File Offset: 0x00088F8C
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.playerUI.nguiWindowManager.Show(EnumNGUIWindow.Loading, false);
		this.windowGroup.isEscClosable = false;
	}

	// Token: 0x06001872 RID: 6258 RVA: 0x0008ADB7 File Offset: 0x00088FB7
	public override void OnClose()
	{
		base.OnClose();
		DiscordManager.Instance.UserAuthorizationResult -= this.authResult;
	}

	// Token: 0x06001873 RID: 6259 RVA: 0x0008ADD5 File Offset: 0x00088FD5
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.updateInput();
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x06001874 RID: 6260 RVA: 0x0008ADEC File Offset: 0x00088FEC
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

	// Token: 0x06001875 RID: 6261 RVA: 0x0008AE38 File Offset: 0x00089038
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

	// Token: 0x06001876 RID: 6262 RVA: 0x0008AEB4 File Offset: 0x000890B4
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

	// Token: 0x06001877 RID: 6263 RVA: 0x0008AF20 File Offset: 0x00089120
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

	// Token: 0x06001878 RID: 6264 RVA: 0x0008AFC3 File Offset: 0x000891C3
	[PublicizedFrom(EAccessModifier.Private)]
	public static void openNow(LocalPlayerUI _playerUi, bool _modal = true)
	{
		_playerUi.windowManager.Open(XUiC_DiscordLogin.ID, _modal, true);
	}

	// Token: 0x0600187B RID: 6267 RVA: 0x0008AFE4 File Offset: 0x000891E4
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

	// Token: 0x0600187C RID: 6268 RVA: 0x0008B07C File Offset: 0x0008927C
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

	// Token: 0x0600187D RID: 6269 RVA: 0x0008B0C0 File Offset: 0x000892C0
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static string <updateUi>g__BuildProvisionalFallbackText|48_2(string _fullAccFailureText, ref XUiC_DiscordLogin.<>c__DisplayClass48_0 A_1)
	{
		string text = Localization.Get("xuiDiscordLoginFallback" + A_1._provisionalAccResult.ToStringCached<DiscordManager.EProvisionalAccountLoginResult>(), false, null);
		return _fullAccFailureText + "\n\n" + text;
	}

	// Token: 0x0600187E RID: 6270 RVA: 0x0008B0F6 File Offset: 0x000892F6
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static string <updateUi>g__BuildProvisionalOnlyText|48_3(ref XUiC_DiscordLogin.<>c__DisplayClass48_0 A_0)
	{
		return Localization.Get("xuiDiscordLoginProvisional" + A_0._provisionalAccResult.ToStringCached<DiscordManager.EProvisionalAccountLoginResult>(), false, null);
	}

	// Token: 0x04000FA6 RID: 4006
	[PublicizedFrom(EAccessModifier.Private)]
	public static string ID = "";

	// Token: 0x04000FA7 RID: 4007
	[XuiBindComponent("btnOk", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnOk;

	// Token: 0x04000FA8 RID: 4008
	[XuiBindComponent("btnCancel", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCancel;

	// Token: 0x04000FA9 RID: 4009
	[XuiBindComponent("btnRequestAuth", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnRequestAuth;

	// Token: 0x04000FAA RID: 4010
	[XuiBindComponent("btnUnlink", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnUnlink;

	// Token: 0x04000FAB RID: 4011
	[XuiBindComponent("btnSettings", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnSettings;

	// Token: 0x04000FAC RID: 4012
	[PublicizedFrom(EAccessModifier.Private)]
	public Action onCloseWithOk;

	// Token: 0x04000FAD RID: 4013
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showSettingsButton;

	// Token: 0x04000FAE RID: 4014
	[PublicizedFrom(EAccessModifier.Private)]
	public bool skipOnSuccess;

	// Token: 0x04000FAF RID: 4015
	[PublicizedFrom(EAccessModifier.Private)]
	public bool openModal;

	// Token: 0x04000FB0 RID: 4016
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isDone;

	// Token: 0x04000FB1 RID: 4017
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cancellable;

	// Token: 0x04000FB2 RID: 4018
	[PublicizedFrom(EAccessModifier.Private)]
	public string statusText;

	// Token: 0x04000FB3 RID: 4019
	[PublicizedFrom(EAccessModifier.Private)]
	public DiscordManager.EFullAccountLoginResult accountLoginResult;

	// Token: 0x04000FB4 RID: 4020
	[PublicizedFrom(EAccessModifier.Private)]
	public DiscordManager.EProvisionalAccountLoginResult provisionalLoginResult;
}
