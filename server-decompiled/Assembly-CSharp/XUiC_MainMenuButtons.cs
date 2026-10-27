using System;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E2D RID: 3629
[Preserve]
public class XUiC_MainMenuButtons : XUiController
{
	// Token: 0x17000C6D RID: 3181
	// (get) Token: 0x06006E9D RID: 28317 RVA: 0x002B1B1C File Offset: 0x002AFD1C
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

	// Token: 0x06006E9E RID: 28318 RVA: 0x002B1B6C File Offset: 0x002AFD6C
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

	// Token: 0x06006E9F RID: 28319 RVA: 0x002B1CEC File Offset: 0x002AFEEC
	[PublicizedFrom(EAccessModifier.Private)]
	public void NewsBoxOnNewsEntryClicked(NewsManager.NewsEntry _newsEntry)
	{
		this.btnNews_OnPressed(this, -1);
	}

	// Token: 0x06006EA0 RID: 28320 RVA: 0x002B1CF6 File Offset: 0x002AFEF6
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

	// Token: 0x06006EA1 RID: 28321 RVA: 0x002B1D27 File Offset: 0x002AFF27
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

	// Token: 0x06006EA2 RID: 28322 RVA: 0x002B1D58 File Offset: 0x002AFF58
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

	// Token: 0x06006EA3 RID: 28323 RVA: 0x002B1DB0 File Offset: 0x002AFFB0
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

	// Token: 0x06006EA4 RID: 28324 RVA: 0x002B1DD6 File Offset: 0x002AFFD6
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnOptions_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.xui.playerUI.windowManager.Open(XUiC_OptionsMenuNew.ParentSelector.WindowGroup, false);
	}

	// Token: 0x06006EA5 RID: 28325 RVA: 0x002B1E14 File Offset: 0x002B0014
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnCredits_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.xui.playerUI.windowManager.Open(XUiC_Credits.ID, true);
	}

	// Token: 0x06006EA6 RID: 28326 RVA: 0x002B1E4D File Offset: 0x002B004D
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnNews_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		XUiC_NewsScreen.Open(this.xui);
	}

	// Token: 0x06006EA7 RID: 28327 RVA: 0x002B1E76 File Offset: 0x002B0076
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnDlc_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.xui.playerUI.windowManager.Open(XUiC_DlcWindowNew.ID, true);
	}

	// Token: 0x06006EA8 RID: 28328 RVA: 0x0007EBCB File Offset: 0x0007CDCB
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnQuit_OnPressed(XUiController _sender, int _mouseButton)
	{
		Application.Quit();
	}

	// Token: 0x06006EA9 RID: 28329 RVA: 0x002B1EB0 File Offset: 0x002B00B0
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

	// Token: 0x06006EAA RID: 28330 RVA: 0x002B1F2D File Offset: 0x002B012D
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.RefreshBindings();
		if (this.btnPlayGame != null)
		{
			this.DoLoadSaveGameAutomation();
		}
	}

	// Token: 0x06006EAB RID: 28331 RVA: 0x002B1F4A File Offset: 0x002B014A
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

	// Token: 0x06006EAC RID: 28332 RVA: 0x002B1F80 File Offset: 0x002B0180
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

	// Token: 0x04005459 RID: 21593
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Button btnPlayGame;
}
