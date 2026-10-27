using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E49 RID: 3657
[Preserve]
public class XUiC_PlayGameMenu : XUiController
{
	// Token: 0x17000CAE RID: 3246
	// (get) Token: 0x06006FC7 RID: 28615 RVA: 0x002B5D64 File Offset: 0x002B3F64
	[XuiXmlBinding("has_profile")]
	public bool HasProfile
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return !string.IsNullOrEmpty(ProfileSDF.CurrentProfileName());
		}
	}

	// Token: 0x17000CAF RID: 3247
	// (get) Token: 0x06006FC8 RID: 28616 RVA: 0x002B5D73 File Offset: 0x002B3F73
	// (set) Token: 0x06006FC9 RID: 28617 RVA: 0x002B5D7B File Offset: 0x002B3F7B
	[XuiXmlBinding("has_saved_game")]
	public bool HasSavedGame
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000CB0 RID: 3248
	// (get) Token: 0x06006FCA RID: 28618 RVA: 0x002B5D84 File Offset: 0x002B3F84
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

	// Token: 0x06006FCB RID: 28619 RVA: 0x002B5DD4 File Offset: 0x002B3FD4
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

	// Token: 0x06006FCC RID: 28620 RVA: 0x002B5E90 File Offset: 0x002B4090
	public override void Init()
	{
		base.Init();
		XUiC_PlayGameMenu.ID = base.WindowGroup.Id;
	}

	// Token: 0x06006FCD RID: 28621 RVA: 0x002AF555 File Offset: 0x002AD755
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.RefreshBindings();
	}

	// Token: 0x06006FCE RID: 28622 RVA: 0x002B5EA8 File Offset: 0x002B40A8
	public void SetWindow(string _windowName)
	{
		this.parentSelector.SetSelected(_windowName);
		this.explicitlyRequestedWindow = true;
	}

	// Token: 0x06006FCF RID: 28623 RVA: 0x002B5EC0 File Offset: 0x002B40C0
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

	// Token: 0x040054DB RID: 21723
	public static string ID;

	// Token: 0x040054DC RID: 21724
	[PublicizedFrom(EAccessModifier.Private)]
	public bool openedBefore;

	// Token: 0x040054DD RID: 21725
	[PublicizedFrom(EAccessModifier.Private)]
	public bool explicitlyRequestedWindow;

	// Token: 0x040054DE RID: 21726
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_WindowSelector parentSelector;
}
