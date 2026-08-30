using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E2F RID: 3631
[Preserve]
public class XUiC_PlayGameMenu : XUiController
{
	// Token: 0x17000C8D RID: 3213
	// (get) Token: 0x06006FAA RID: 28586 RVA: 0x002B1448 File Offset: 0x002AF648
	[XuiXmlBinding("has_profile")]
	public bool HasProfile
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return !string.IsNullOrEmpty(ProfileSDF.CurrentProfileName());
		}
	}

	// Token: 0x17000C8E RID: 3214
	// (get) Token: 0x06006FAB RID: 28587 RVA: 0x002B1457 File Offset: 0x002AF657
	// (set) Token: 0x06006FAC RID: 28588 RVA: 0x002B145F File Offset: 0x002AF65F
	[XuiXmlBinding("has_saved_game")]
	public bool HasSavedGame
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000C8F RID: 3215
	// (get) Token: 0x06006FAD RID: 28589 RVA: 0x002B1468 File Offset: 0x002AF668
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

	// Token: 0x06006FAE RID: 28590 RVA: 0x002B14B8 File Offset: 0x002AF6B8
	[XuiBindEvent("WindowSelected", "parentSelector")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void onWindowSelected(XUiC_WindowSelector _sender, string _windowId)
	{
		GUIWindowManager windowManager = this.xui.playerUI.windowManager;
		if (_windowId == "playerProfiles")
		{
			XUiC_PlayerProfile.Open(this.xui, null);
			return;
		}
		if (_windowId == "newGame")
		{
			windowManager.Open(XUiC_NewGame.ID, true);
			return;
		}
		if (_windowId == "continueGame")
		{
			windowManager.Open(XUiC_ContinueGame.ID, true);
			return;
		}
		if (!(_windowId == "serverBrowser"))
		{
			return;
		}
		if (windowManager.IsWindowOpen(XUiC_ServerBrowser.ID))
		{
			return;
		}
		XUiC_MultiplayerPrivilegeNotification.ResolvePrivilegesWithDialog(EUserPerms.Multiplayer, delegate(bool _)
		{
			if (PermissionsManager.IsMultiplayerAllowed())
			{
				windowManager.Open(XUiC_ServerBrowser.ID, true);
			}
		}, -1f, false, null);
	}

	// Token: 0x06006FAF RID: 28591 RVA: 0x002B1574 File Offset: 0x002AF774
	public override void Init()
	{
		base.Init();
		XUiC_PlayGameMenu.ID = base.WindowGroup.Id;
	}

	// Token: 0x06006FB0 RID: 28592 RVA: 0x002AAC41 File Offset: 0x002A8E41
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.RefreshBindings();
	}

	// Token: 0x06006FB1 RID: 28593 RVA: 0x002B158C File Offset: 0x002AF78C
	public void SetWindow(string _windowName)
	{
		this.parentSelector.SetSelected(_windowName);
		this.explicitlyRequestedWindow = true;
	}

	// Token: 0x06006FB2 RID: 28594 RVA: 0x002B15A4 File Offset: 0x002AF7A4
	public override void OnOpen()
	{
		this.HasSavedGame = GameIO.GetPlayerSaves(null, false) > 0;
		base.RefreshBindings();
		if (!this.openedBefore || XUiC_NewContinueBase.LastStartedNewGame)
		{
			XUiC_NewContinueBase.LastStartedNewGame = false;
			this.openedBefore = true;
			if (!this.HasProfile)
			{
				this.parentSelector.SetSelected("playerProfiles");
			}
			else if (!this.explicitlyRequestedWindow)
			{
				if (this.HasSavedGame)
				{
					this.parentSelector.SetSelected("continueGame");
				}
				else
				{
					this.parentSelector.SetSelected("newGame");
				}
			}
		}
		this.explicitlyRequestedWindow = false;
		base.OnOpen();
	}

	// Token: 0x0400547F RID: 21631
	public static string ID;

	// Token: 0x04005480 RID: 21632
	[PublicizedFrom(EAccessModifier.Private)]
	public bool openedBefore;

	// Token: 0x04005481 RID: 21633
	[PublicizedFrom(EAccessModifier.Private)]
	public bool explicitlyRequestedWindow;

	// Token: 0x04005482 RID: 21634
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_WindowSelector parentSelector;
}
