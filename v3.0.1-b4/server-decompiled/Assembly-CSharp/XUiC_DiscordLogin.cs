using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

// Token: 0x02000348 RID: 840
[Preserve]
public class XUiC_DiscordLogin : XUiController
{
	// Token: 0x170002E5 RID: 741
	// (get) Token: 0x06001847 RID: 6215 RVA: 0x0008A564 File Offset: 0x00088764
	// (set) Token: 0x06001848 RID: 6216 RVA: 0x0008A575 File Offset: 0x00088775
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

	// Token: 0x170002E6 RID: 742
	// (get) Token: 0x06001849 RID: 6217 RVA: 0x0008A585 File Offset: 0x00088785
	// (set) Token: 0x0600184A RID: 6218 RVA: 0x0008A58D File Offset: 0x0008878D
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

	// Token: 0x170002E7 RID: 743
	// (get) Token: 0x0600184B RID: 6219 RVA: 0x0008A59D File Offset: 0x0008879D
	// (set) Token: 0x0600184C RID: 6220 RVA: 0x0008A5A5 File Offset: 0x000887A5
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

	// Token: 0x170002E8 RID: 744
	// (get) Token: 0x0600184D RID: 6221 RVA: 0x0008A5B5 File Offset: 0x000887B5
	// (set) Token: 0x0600184E RID: 6222 RVA: 0x0008A5BD File Offset: 0x000887BD
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

	// Token: 0x0600184F RID: 6223 RVA: 0x0008A5CD File Offset: 0x000887CD
	public override void Init()
	{
		base.Init();
		XUiC_DiscordLogin.ID = base.WindowGroup.Id;
	}

	// Token: 0x06001850 RID: 6224 RVA: 0x0008A5E5 File Offset: 0x000887E5
	[XuiBindEvent("OnPress", "btnOk")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnOk_OnPress(XUiController _sender, int _mouseButton)
	{
		this.closeAndOpenNextWindow();
	}

	// Token: 0x06001851 RID: 6225 RVA: 0x0008A5ED File Offset: 0x000887ED
	[XuiBindEvent("OnPress", "btnCancel")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnCancel_OnPress(XUiController _sender, int _mouseButton)
	{
		DiscordManager.Instance.AuthManager.AbortAuth();
	}

	// Token: 0x06001852 RID: 6226 RVA: 0x00089E87 File Offset: 0x00088087
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

	// Token: 0x06001853 RID: 6227 RVA: 0x0008A600 File Offset: 0x00088800
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

	// Token: 0x06001854 RID: 6228 RVA: 0x0008A658 File Offset: 0x00088858
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.playerUI.nguiWindowManager.Show(EnumNGUIWindow.Loading, false);
		this.windowGroup.isEscClosable = false;
	}

	// Token: 0x06001855 RID: 6229 RVA: 0x0008A683 File Offset: 0x00088883
	public override void OnClose()
	{
		base.OnClose();
		DiscordManager.Instance.UserAuthorizationResult -= this.authResult;
	}

	// Token: 0x06001856 RID: 6230 RVA: 0x0008A6A1 File Offset: 0x000888A1
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.updateInput();
	}

	// Token: 0x06001857 RID: 6231 RVA: 0x0008A6B0 File Offset: 0x000888B0
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

	// Token: 0x06001858 RID: 6232 RVA: 0x0008A6FC File Offset: 0x000888FC
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

	// Token: 0x06001859 RID: 6233 RVA: 0x0008A778 File Offset: 0x00088978
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateUi(bool _isDone, DiscordManager.EFullAccountLoginResult _fullAccResult, DiscordManager.EProvisionalAccountLoginResult _provisionalAccResult, bool _isExpectedSuccess)
	{
		XUiC_DiscordLogin.<>c__DisplayClass33_0 CS$<>8__locals1;
		CS$<>8__locals1._isDone = _isDone;
		CS$<>8__locals1._fullAccResult = _fullAccResult;
		CS$<>8__locals1._provisionalAccResult = _provisionalAccResult;
		this.IsDone = CS$<>8__locals1._isDone;
		this.Cancellable = CS$<>8__locals1._fullAccResult == DiscordManager.EFullAccountLoginResult.RequestingAuth;
		this.StatusText = XUiC_DiscordLogin.<updateUi>g__BuildText|33_0(ref CS$<>8__locals1);
		base.RefreshBindings();
	}

	// Token: 0x0600185A RID: 6234 RVA: 0x0008A7CC File Offset: 0x000889CC
	public static void Open(Action _onCloseWithOk = null, bool _showSettingsButton = true, bool _waitForResultToShow = false, bool _skipOnSuccess = false, bool _modal = true, bool _cancellable = false)
	{
		XUi xui = LocalPlayerUI.primaryUI.xui;
		LocalPlayerUI playerUI = xui.playerUI;
		XUiC_DiscordLogin childByType = xui.GetWindowGroupById(XUiC_DiscordLogin.ID).Controller.GetChildByType<XUiC_DiscordLogin>();
		DiscordManager.Instance.UserAuthorizationResult += childByType.authResult;
		childByType.StatusText = Localization.Get("xuiDiscordLoginLoggingIn", false, null);
		childByType.IsDone = false;
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

	// Token: 0x0600185B RID: 6235 RVA: 0x0008A882 File Offset: 0x00088A82
	[PublicizedFrom(EAccessModifier.Private)]
	public static void openNow(LocalPlayerUI _playerUi, bool _modal = true)
	{
		_playerUi.windowManager.Open(XUiC_DiscordLogin.ID, _modal, true);
	}

	// Token: 0x0600185E RID: 6238 RVA: 0x0008A8A4 File Offset: 0x00088AA4
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static string <updateUi>g__BuildText|33_0(ref XUiC_DiscordLogin.<>c__DisplayClass33_0 A_0)
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
			return XUiC_DiscordLogin.<updateUi>g__BuildProvisionalOnlyText|33_3(ref A_0);
		}
		if (A_0._fullAccResult == DiscordManager.EFullAccountLoginResult.Success)
		{
			return Localization.Get("xuiDiscordLoginFullAccountSuccess", false, null);
		}
		return XUiC_DiscordLogin.<updateUi>g__BuildFullAccFailureText|33_1(ref A_0);
	}

	// Token: 0x0600185F RID: 6239 RVA: 0x0008A93C File Offset: 0x00088B3C
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static string <updateUi>g__BuildFullAccFailureText|33_1(ref XUiC_DiscordLogin.<>c__DisplayClass33_0 A_0)
	{
		string text = Localization.Get("xuiDiscordLoginFullAccount" + A_0._fullAccResult.ToStringCached<DiscordManager.EFullAccountLoginResult>(), false, null);
		if (A_0._provisionalAccResult == DiscordManager.EProvisionalAccountLoginResult.None || A_0._provisionalAccResult == DiscordManager.EProvisionalAccountLoginResult.NotSupported)
		{
			return text;
		}
		return XUiC_DiscordLogin.<updateUi>g__BuildProvisionalFallbackText|33_2(text, ref A_0);
	}

	// Token: 0x06001860 RID: 6240 RVA: 0x0008A980 File Offset: 0x00088B80
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static string <updateUi>g__BuildProvisionalFallbackText|33_2(string _fullAccFailureText, ref XUiC_DiscordLogin.<>c__DisplayClass33_0 A_1)
	{
		string text = Localization.Get("xuiDiscordLoginFallback" + A_1._provisionalAccResult.ToStringCached<DiscordManager.EProvisionalAccountLoginResult>(), false, null);
		return _fullAccFailureText + "\n\n" + text;
	}

	// Token: 0x06001861 RID: 6241 RVA: 0x0008A9B6 File Offset: 0x00088BB6
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static string <updateUi>g__BuildProvisionalOnlyText|33_3(ref XUiC_DiscordLogin.<>c__DisplayClass33_0 A_0)
	{
		return Localization.Get("xuiDiscordLoginProvisional" + A_0._provisionalAccResult.ToStringCached<DiscordManager.EProvisionalAccountLoginResult>(), false, null);
	}

	// Token: 0x04000F95 RID: 3989
	[PublicizedFrom(EAccessModifier.Private)]
	public static string ID = "";

	// Token: 0x04000F96 RID: 3990
	[XuiBindComponent("btnOk", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnOk;

	// Token: 0x04000F97 RID: 3991
	[XuiBindComponent("btnCancel", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCancel;

	// Token: 0x04000F98 RID: 3992
	[XuiBindComponent("btnSettings", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnSettings;

	// Token: 0x04000F99 RID: 3993
	[PublicizedFrom(EAccessModifier.Private)]
	public Action onCloseWithOk;

	// Token: 0x04000F9A RID: 3994
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showSettingsButton;

	// Token: 0x04000F9B RID: 3995
	[PublicizedFrom(EAccessModifier.Private)]
	public bool skipOnSuccess;

	// Token: 0x04000F9C RID: 3996
	[PublicizedFrom(EAccessModifier.Private)]
	public bool openModal;

	// Token: 0x04000F9D RID: 3997
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isDone;

	// Token: 0x04000F9E RID: 3998
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cancellable;

	// Token: 0x04000F9F RID: 3999
	[PublicizedFrom(EAccessModifier.Private)]
	public string statusText;
}
