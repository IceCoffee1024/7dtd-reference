using System;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E22 RID: 3618
[Preserve]
public class XUiC_MainMenuButtons : XUiController
{
	// Token: 0x17000C67 RID: 3175
	// (get) Token: 0x06006E78 RID: 28280 RVA: 0x002B05B0 File Offset: 0x002AE7B0
	[XuiXmlBinding("online_mode")]
	public bool OnlineMode
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			IPlatform multiPlatform = PlatformManager.MultiPlatform;
			EUserStatus? euserStatus;
			if (multiPlatform == null)
			{
				euserStatus = null;
			}
			else
			{
				IUserClient user = multiPlatform.User;
				euserStatus = ((user != null) ? new EUserStatus?(user.UserStatus) : null);
			}
			EUserStatus? euserStatus2 = euserStatus;
			return euserStatus2.GetValueOrDefault() != EUserStatus.OfflineMode;
		}
	}

	// Token: 0x06006E79 RID: 28281 RVA: 0x002B0600 File Offset: 0x002AE800
	public override void Init()
	{
		base.Init();
		if (base.TryGetChildByIdAndType<XUiC_Button>("btnPlayGame", out this.btnPlayGame))
		{
			this.btnPlayGame.OnPress += this.btnPlayGame_OnPressed;
		}
		XUiC_Button xuiC_Button;
		if (base.TryGetChildByIdAndType<XUiC_Button>("btnConnectToServer", out xuiC_Button))
		{
			xuiC_Button.OnPress += this.btnConnectToServer_OnPressed;
		}
		XUiC_Button xuiC_Button2;
		if (base.TryGetChildByIdAndType<XUiC_Button>("btnOptions", out xuiC_Button2))
		{
			xuiC_Button2.OnPress += this.btnOptions_OnPressed;
		}
		XUiC_Button xuiC_Button3;
		if (base.TryGetChildByIdAndType<XUiC_Button>("btnCredits", out xuiC_Button3))
		{
			xuiC_Button3.OnPress += this.btnCredits_OnPressed;
		}
		XUiC_Button xuiC_Button4;
		if (base.TryGetChildByIdAndType<XUiC_Button>("btnNews", out xuiC_Button4))
		{
			xuiC_Button4.OnPress += this.btnNews_OnPressed;
		}
		XUiC_Button xuiC_Button5;
		if (base.TryGetChildByIdAndType<XUiC_Button>("btnQuit", out xuiC_Button5))
		{
			xuiC_Button5.OnPress += this.btnQuit_OnPressed;
		}
		XUiC_Button xuiC_Button6;
		if (base.TryGetChildByIdAndType<XUiC_Button>("btnEditingTools", out xuiC_Button6))
		{
			xuiC_Button6.OnPress += this.btnEditingTools_OnPressed;
		}
		XUiC_Button xuiC_Button7;
		if (base.TryGetChildByIdAndType<XUiC_Button>("btnRWG", out xuiC_Button7))
		{
			xuiC_Button7.OnPress += this.btnRWG_OnPressed;
		}
		XUiC_Button xuiC_Button8;
		if (base.TryGetChildByIdAndType<XUiC_Button>("btnDlc", out xuiC_Button8))
		{
			xuiC_Button8.OnPress += this.btnDlc_OnPressed;
		}
		XUiC_NewsWindow[] childControllers = base.GetChildControllers<XUiC_NewsWindow>("", null);
		for (int i = 0; i < childControllers.Length; i++)
		{
			childControllers[i].NewsEntryClicked += this.NewsBoxOnNewsEntryClicked;
		}
	}

	// Token: 0x06006E7A RID: 28282 RVA: 0x002B0780 File Offset: 0x002AE980
	[PublicizedFrom(EAccessModifier.Private)]
	public void NewsBoxOnNewsEntryClicked(NewsManager.NewsEntry _newsEntry)
	{
		this.btnNews_OnPressed(this, -1);
	}

	// Token: 0x06006E7B RID: 28283 RVA: 0x002B078A File Offset: 0x002AE98A
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnPlayGame_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (GameManager.HasAcceptedLatestEula())
		{
			this.xui.playerUI.windowManager.Open(XUiC_PlayGameMenu.ID, true);
			return;
		}
		XUiC_EulaWindow.Open(this.xui, false);
	}

	// Token: 0x06006E7C RID: 28284 RVA: 0x002B07BB File Offset: 0x002AE9BB
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnConnectToServer_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (!GameManager.HasAcceptedLatestEula())
		{
			XUiC_EulaWindow.Open(this.xui, false);
			return;
		}
		XUiC_MultiplayerPrivilegeNotification.ResolvePrivilegesWithDialog(EUserPerms.Multiplayer, delegate(bool _)
		{
			if (PermissionsManager.IsMultiplayerAllowed())
			{
				this.CheckProfile(XUiC_ServerBrowser.ID);
			}
		}, -1f, false, null);
	}

	// Token: 0x06006E7D RID: 28285 RVA: 0x002B07EC File Offset: 0x002AE9EC
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnEditingTools_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (GameManager.HasAcceptedLatestEula())
		{
			this.xui.playerUI.windowManager.Close(this.windowGroup, false);
			this.xui.playerUI.windowManager.Open(XUiC_EditingTools.ID, true);
			return;
		}
		XUiC_EulaWindow.Open(this.xui, false);
	}

	// Token: 0x06006E7E RID: 28286 RVA: 0x002B0844 File Offset: 0x002AEA44
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnRWG_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (!GameManager.HasAcceptedLatestEula())
		{
			XUiC_EulaWindow.Open(this.xui, false);
			return;
		}
		XUiC_WorldGenerationWindow.Open(this.xui, XUiC_MainMenu.ID);
	}

	// Token: 0x06006E7F RID: 28287 RVA: 0x002B086A File Offset: 0x002AEA6A
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnOptions_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.xui.playerUI.windowManager.Open(XUiC_OptionsMenuNew.ParentSelector.WindowGroup, false);
	}

	// Token: 0x06006E80 RID: 28288 RVA: 0x002B08A8 File Offset: 0x002AEAA8
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnCredits_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.xui.playerUI.windowManager.Open(XUiC_Credits.ID, true);
	}

	// Token: 0x06006E81 RID: 28289 RVA: 0x002B08E1 File Offset: 0x002AEAE1
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnNews_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		XUiC_NewsScreen.Open(this.xui);
	}

	// Token: 0x06006E82 RID: 28290 RVA: 0x002B090A File Offset: 0x002AEB0A
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnDlc_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.xui.playerUI.windowManager.Open(XUiC_DlcWindowNew.ID, true);
	}

	// Token: 0x06006E83 RID: 28291 RVA: 0x0007E9FB File Offset: 0x0007CBFB
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnQuit_OnPressed(XUiController _sender, int _mouseButton)
	{
		Application.Quit();
	}

	// Token: 0x06006E84 RID: 28292 RVA: 0x002B0944 File Offset: 0x002AEB44
	[PublicizedFrom(EAccessModifier.Private)]
	public void CheckProfile(string _windowToOpen)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		if (ProfileSDF.CurrentProfileName().Length == 0)
		{
			XUiC_PlayerProfile.Open(this.xui, delegate
			{
				this.xui.playerUI.windowManager.Open(_windowToOpen, true);
			});
			return;
		}
		this.xui.playerUI.windowManager.Open(_windowToOpen, true);
	}

	// Token: 0x06006E85 RID: 28293 RVA: 0x002B09C1 File Offset: 0x002AEBC1
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.RefreshBindings();
		if (this.btnPlayGame != null)
		{
			this.DoLoadSaveGameAutomation();
		}
	}

	// Token: 0x06006E86 RID: 28294 RVA: 0x002B09DE File Offset: 0x002AEBDE
	[PublicizedFrom(EAccessModifier.Private)]
	public bool TryGoToPlayGameWindow(string name)
	{
		if (!this.btnPlayGame.ViewComponent.Enabled)
		{
			return false;
		}
		this.xui.GetChildByType<XUiC_PlayGameMenu>().SetWindow(name);
		this.btnPlayGame_OnPressed(this.btnPlayGame, -1);
		return true;
	}

	// Token: 0x06006E87 RID: 28295 RVA: 0x002B0A14 File Offset: 0x002AEC14
	[PublicizedFrom(EAccessModifier.Private)]
	public void DoLoadSaveGameAutomation()
	{
		EPlatformLoadSaveGameState state = PlatformApplicationManager.LoadSaveGame.GetState();
		if (state != EPlatformLoadSaveGameState.NewGameOpen)
		{
			if (state != EPlatformLoadSaveGameState.ContinueGameOpen)
			{
				return;
			}
			if (!this.TryGoToPlayGameWindow(XUiC_ContinueGame.ID))
			{
				PlatformApplicationManager.LoadSaveGame.SetFailed();
				return;
			}
			PlatformApplicationManager.LoadSaveGame.AdvanceStateFrom(state);
			return;
		}
		else
		{
			if (!this.TryGoToPlayGameWindow(XUiC_NewGame.ID))
			{
				PlatformApplicationManager.LoadSaveGame.SetFailed();
				return;
			}
			PlatformApplicationManager.LoadSaveGame.AdvanceStateFrom(state);
			return;
		}
	}

	// Token: 0x04005434 RID: 21556
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Button btnPlayGame;
}
