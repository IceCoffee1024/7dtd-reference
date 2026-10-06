using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E3E RID: 3646
[Preserve]
public class XUiC_PlayGameMenu : XUiController
{
	// Token: 0x17000CA8 RID: 3240
	// (get) Token: 0x06006FA2 RID: 28578 RVA: 0x002B47F8 File Offset: 0x002B29F8
	[XuiXmlBinding("has_profile")]
	public bool HasProfile
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return !string.IsNullOrEmpty(ProfileSDF.CurrentProfileName());
		}
	}

	// Token: 0x17000CA9 RID: 3241
	// (get) Token: 0x06006FA3 RID: 28579 RVA: 0x002B4807 File Offset: 0x002B2A07
	// (set) Token: 0x06006FA4 RID: 28580 RVA: 0x002B480F File Offset: 0x002B2A0F
	[XuiXmlBinding("has_saved_game")]
	public bool HasSavedGame
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000CAA RID: 3242
	// (get) Token: 0x06006FA5 RID: 28581 RVA: 0x002B4818 File Offset: 0x002B2A18
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

	// Token: 0x06006FA6 RID: 28582 RVA: 0x002B4868 File Offset: 0x002B2A68
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

	// Token: 0x06006FA7 RID: 28583 RVA: 0x002B4924 File Offset: 0x002B2B24
	public override void Init()
	{
		base.Init();
		XUiC_PlayGameMenu.ID = base.WindowGroup.Id;
	}

	// Token: 0x06006FA8 RID: 28584 RVA: 0x002ADFE9 File Offset: 0x002AC1E9
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.RefreshBindings();
	}

	// Token: 0x06006FA9 RID: 28585 RVA: 0x002B493C File Offset: 0x002B2B3C
	public void SetWindow(string _windowName)
	{
		this.parentSelector.SetSelected(_windowName);
		this.explicitlyRequestedWindow = true;
	}

	// Token: 0x06006FAA RID: 28586 RVA: 0x002B4954 File Offset: 0x002B2B54
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

	// Token: 0x040054B6 RID: 21686
	public static string ID;

	// Token: 0x040054B7 RID: 21687
	[PublicizedFrom(EAccessModifier.Private)]
	public bool openedBefore;

	// Token: 0x040054B8 RID: 21688
	[PublicizedFrom(EAccessModifier.Private)]
	public bool explicitlyRequestedWindow;

	// Token: 0x040054B9 RID: 21689
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_WindowSelector parentSelector;
}
