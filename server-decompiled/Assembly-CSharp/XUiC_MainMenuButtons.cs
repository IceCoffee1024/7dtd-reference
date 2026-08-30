using System;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E13 RID: 3603
[Preserve]
public class XUiC_MainMenuButtons : XUiController
{
	// Token: 0x17000C4C RID: 3148
	// (get) Token: 0x06006E80 RID: 28288 RVA: 0x002AD208 File Offset: 0x002AB408
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

	// Token: 0x06006E81 RID: 28289 RVA: 0x002AD258 File Offset: 0x002AB458
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

	// Token: 0x06006E82 RID: 28290 RVA: 0x002AD3D8 File Offset: 0x002AB5D8
	[PublicizedFrom(EAccessModifier.Private)]
	public void NewsBoxOnNewsEntryClicked(NewsManager.NewsEntry _newsEntry)
	{
		this.btnNews_OnPressed(this, -1);
	}

	// Token: 0x06006E83 RID: 28291 RVA: 0x002AD3E2 File Offset: 0x002AB5E2
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

	// Token: 0x06006E84 RID: 28292 RVA: 0x002AD413 File Offset: 0x002AB613
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

	// Token: 0x06006E85 RID: 28293 RVA: 0x002AD444 File Offset: 0x002AB644
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

	// Token: 0x06006E86 RID: 28294 RVA: 0x002AD49C File Offset: 0x002AB69C
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

	// Token: 0x06006E87 RID: 28295 RVA: 0x002AD4C2 File Offset: 0x002AB6C2
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnOptions_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.xui.playerUI.windowManager.Open(XUiC_OptionsMenuNew.ParentSelector.WindowGroup, false);
	}

	// Token: 0x06006E88 RID: 28296 RVA: 0x002AD500 File Offset: 0x002AB700
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnCredits_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.xui.playerUI.windowManager.Open(XUiC_Credits.ID, true);
	}

	// Token: 0x06006E89 RID: 28297 RVA: 0x002AD539 File Offset: 0x002AB739
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnNews_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		XUiC_NewsScreen.Open(this.xui);
	}

	// Token: 0x06006E8A RID: 28298 RVA: 0x002AD562 File Offset: 0x002AB762
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnDlc_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.xui.playerUI.windowManager.Open(XUiC_DlcWindowNew.ID, true);
	}

	// Token: 0x06006E8B RID: 28299 RVA: 0x0007E747 File Offset: 0x0007C947
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnQuit_OnPressed(XUiController _sender, int _mouseButton)
	{
		Application.Quit();
	}

	// Token: 0x06006E8C RID: 28300 RVA: 0x002AD59C File Offset: 0x002AB79C
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

	// Token: 0x06006E8D RID: 28301 RVA: 0x002AD619 File Offset: 0x002AB819
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.RefreshBindings();
		if (this.btnPlayGame != null)
		{
			this.DoLoadSaveGameAutomation();
		}
	}

	// Token: 0x06006E8E RID: 28302 RVA: 0x002AD636 File Offset: 0x002AB836
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

	// Token: 0x06006E8F RID: 28303 RVA: 0x002AD66C File Offset: 0x002AB86C
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

	// Token: 0x040053FD RID: 21501
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Button btnPlayGame;
}
