using System;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000DFE RID: 3582
[Preserve]
public class XUiC_MainMenuButtons : XUiController
{
	// Token: 0x17000C33 RID: 3123
	// (get) Token: 0x06006DE0 RID: 28128 RVA: 0x002A9CF4 File Offset: 0x002A7EF4
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

	// Token: 0x06006DE1 RID: 28129 RVA: 0x002A9D44 File Offset: 0x002A7F44
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

	// Token: 0x06006DE2 RID: 28130 RVA: 0x002A9EC4 File Offset: 0x002A80C4
	[PublicizedFrom(EAccessModifier.Private)]
	public void NewsBoxOnNewsEntryClicked(NewsManager.NewsEntry _newsEntry)
	{
		this.btnNews_OnPressed(this, -1);
	}

	// Token: 0x06006DE3 RID: 28131 RVA: 0x002A9ECE File Offset: 0x002A80CE
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

	// Token: 0x06006DE4 RID: 28132 RVA: 0x002A9EFF File Offset: 0x002A80FF
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

	// Token: 0x06006DE5 RID: 28133 RVA: 0x002A9F30 File Offset: 0x002A8130
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

	// Token: 0x06006DE6 RID: 28134 RVA: 0x002A9F88 File Offset: 0x002A8188
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

	// Token: 0x06006DE7 RID: 28135 RVA: 0x002A9FAE File Offset: 0x002A81AE
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnOptions_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.xui.playerUI.windowManager.Open(XUiC_OptionsMenuNew.ParentSelector.WindowGroup, false);
	}

	// Token: 0x06006DE8 RID: 28136 RVA: 0x002A9FEC File Offset: 0x002A81EC
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnCredits_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.xui.playerUI.windowManager.Open(XUiC_Credits.ID, true);
	}

	// Token: 0x06006DE9 RID: 28137 RVA: 0x002AA025 File Offset: 0x002A8225
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnNews_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		XUiC_NewsScreen.Open(this.xui);
	}

	// Token: 0x06006DEA RID: 28138 RVA: 0x002AA04E File Offset: 0x002A824E
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnDlc_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.xui.playerUI.windowManager.Open(XUiC_DlcWindowNew.ID, true);
	}

	// Token: 0x06006DEB RID: 28139 RVA: 0x0007E0EF File Offset: 0x0007C2EF
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnQuit_OnPressed(XUiController _sender, int _mouseButton)
	{
		Application.Quit();
	}

	// Token: 0x06006DEC RID: 28140 RVA: 0x002AA088 File Offset: 0x002A8288
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

	// Token: 0x06006DED RID: 28141 RVA: 0x002AA105 File Offset: 0x002A8305
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.RefreshBindings();
		if (this.btnPlayGame != null)
		{
			this.DoLoadSaveGameAutomation();
		}
	}

	// Token: 0x06006DEE RID: 28142 RVA: 0x002AA122 File Offset: 0x002A8322
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

	// Token: 0x06006DEF RID: 28143 RVA: 0x002AA158 File Offset: 0x002A8358
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

	// Token: 0x0400535A RID: 21338
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Button btnPlayGame;
}
