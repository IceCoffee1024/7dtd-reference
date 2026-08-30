using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E1A RID: 3610
[Preserve]
public class XUiC_PlayGameMenu : XUiController
{
	// Token: 0x17000C74 RID: 3188
	// (get) Token: 0x06006F0A RID: 28426 RVA: 0x002ADF34 File Offset: 0x002AC134
	[XuiXmlBinding("has_profile")]
	public bool HasProfile
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return !string.IsNullOrEmpty(ProfileSDF.CurrentProfileName());
		}
	}

	// Token: 0x17000C75 RID: 3189
	// (get) Token: 0x06006F0B RID: 28427 RVA: 0x002ADF43 File Offset: 0x002AC143
	// (set) Token: 0x06006F0C RID: 28428 RVA: 0x002ADF4B File Offset: 0x002AC14B
	[XuiXmlBinding("has_saved_game")]
	public bool HasSavedGame
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000C76 RID: 3190
	// (get) Token: 0x06006F0D RID: 28429 RVA: 0x002ADF54 File Offset: 0x002AC154
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

	// Token: 0x06006F0E RID: 28430 RVA: 0x002ADFA4 File Offset: 0x002AC1A4
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

	// Token: 0x06006F0F RID: 28431 RVA: 0x002AE060 File Offset: 0x002AC260
	public override void Init()
	{
		base.Init();
		XUiC_PlayGameMenu.ID = base.WindowGroup.Id;
	}

	// Token: 0x06006F10 RID: 28432 RVA: 0x002A777D File Offset: 0x002A597D
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.RefreshBindings();
	}

	// Token: 0x06006F11 RID: 28433 RVA: 0x002AE078 File Offset: 0x002AC278
	public void SetWindow(string _windowName)
	{
		this.parentSelector.SetSelected(_windowName);
		this.explicitlyRequestedWindow = true;
	}

	// Token: 0x06006F12 RID: 28434 RVA: 0x002AE090 File Offset: 0x002AC290
	public override void OnOpen()
	{
		this.HasSavedGame = GameIO.GetPlayerSaves(null, false) > 0;
		base.RefreshBindings();
		if (!this.openedBefore)
		{
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

	// Token: 0x040053DB RID: 21467
	public static string ID;

	// Token: 0x040053DC RID: 21468
	[PublicizedFrom(EAccessModifier.Private)]
	public bool openedBefore;

	// Token: 0x040053DD RID: 21469
	[PublicizedFrom(EAccessModifier.Private)]
	public bool explicitlyRequestedWindow;

	// Token: 0x040053DE RID: 21470
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_WindowSelector parentSelector;
}
