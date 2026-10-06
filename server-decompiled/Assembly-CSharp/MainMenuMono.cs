using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Platform;
using Services.Analytics;
using UnityEngine;
using WorldGenerationEngineFinal;

// Token: 0x02001289 RID: 4745
public class MainMenuMono : MonoBehaviour
{
	// Token: 0x17001222 RID: 4642
	// (get) Token: 0x060096CD RID: 38605 RVA: 0x0038D9AF File Offset: 0x0038BBAF
	public static bool IsQuickContinue
	{
		get
		{
			return GameUtils.GetLaunchArgument("quick-continue") != null || (ToggleCapsLock.GetScrollLock() && Application.isEditor);
		}
	}

	// Token: 0x060096CE RID: 38606 RVA: 0x0038D9CD File Offset: 0x0038BBCD
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Awake()
	{
		this.windowManager = base.GetComponent<GUIWindowManager>();
		this.nguiWindowManager = base.GetComponent<NGUIWindowManager>();
	}

	// Token: 0x060096CF RID: 38607 RVA: 0x0038D9E8 File Offset: 0x0038BBE8
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Start()
	{
		if (GameManager.IsDedicatedServer)
		{
			ThreadManager.StartCoroutine(this.startDedicatedServer());
			return;
		}
		this.nguiWindowManager.SetLabelText(EnumNGUIWindow.Version, Constants.cVersionInformation.LongString, false);
		Cursor.visible = true;
		Cursor.lockState = SoftCursor.DefaultCursorLockState;
		this.nguiWindowManager.Show(EnumNGUIWindow.Loading, true);
	}

	// Token: 0x060096D0 RID: 38608 RVA: 0x0038DA3D File Offset: 0x0038BC3D
	[PublicizedFrom(EAccessModifier.Private)]
	public bool CheckLogin()
	{
		this.loginCheckDone = true;
		this.bOpenMainMenu = false;
		XUiC_LoginBase.Login(this.windowManager.playerUI.xui, new Action(this.OnLoginComplete));
		return this.bOpenMainMenu;
	}

	// Token: 0x060096D1 RID: 38609 RVA: 0x0038DA74 File Offset: 0x0038BC74
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnLoginComplete()
	{
		if (!GameManager.RemoteResourcesLoaded)
		{
			GameManager.LoadRemoteResources(null);
		}
		base.StartCoroutine(Helper.LoginEventAnalyticCoroutine(true));
		if (PlatformManager.MultiPlatform.User.UserStatus == EUserStatus.OfflineMode)
		{
			this.bOpenMainMenu = true;
			return;
		}
		if (PlatformManager.MultiPlatform.User.UserStatus == EUserStatus.LoggedIn)
		{
			EUserPerms euserPerms = EUserPerms.All;
			if (!GamePrefs.GetBool(EnumGamePrefs.ServerEnabled))
			{
				euserPerms &= ~EUserPerms.HostMultiplayer;
			}
			this.nguiWindowManager.SetLabelText(EnumNGUIWindow.Loading, Localization.Get("xuiSteamLoginProgressCheckAccount", false, null) + "...", false);
			base.StartCoroutine(this.<OnLoginComplete>g__ResolveInitialPermissions|9_0(euserPerms));
			return;
		}
		string text = "Login complete but user is not in valid state. Native platform user status: {0}, Crossplatform user status: {1}";
		object obj = PlatformManager.NativePlatform.User.UserStatus;
		IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
		object obj2;
		if (crossplatformPlatform == null)
		{
			obj2 = null;
		}
		else
		{
			IUserClient user = crossplatformPlatform.User;
			obj2 = ((user != null) ? user.UserStatus.ToString() : null);
		}
		Log.Error(string.Format(text, obj, obj2 ?? "N/A"));
	}

	// Token: 0x060096D2 RID: 38610 RVA: 0x0038DB64 File Offset: 0x0038BD64
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Update()
	{
		if (!GameStartupHelper.Instance.OpenMainMenuAfterAwake)
		{
			return;
		}
		if (!GameManager.Instance.bStaticDataLoaded)
		{
			if (this.loadingText == "")
			{
				this.loadingText = Localization.Get("loadActionLoading", false, null);
			}
			if (GameManager.Instance.CurrentLoadAction != this.displayedLoadAction)
			{
				this.displayedLoadAction = GameManager.Instance.CurrentLoadAction;
				this.nguiWindowManager.SetLabelText(EnumNGUIWindow.Loading, this.loadingText + " " + this.displayedLoadAction + "...", false);
			}
		}
		else
		{
			if (this.windowManager.playerUI == null || this.windowManager.playerUI.xui == null || !this.windowManager.playerUI.xui.IsReady)
			{
				return;
			}
			if (!this.loginCheckDone && !this.CheckLogin())
			{
				return;
			}
			if (!this.bOpenMainMenu)
			{
				return;
			}
			if (this.nguiWindowManager.IsShowing(EnumNGUIWindow.Loading))
			{
				this.nguiWindowManager.Show(EnumNGUIWindow.Loading, false);
			}
			if (ModManager.GetFailedMods(new Mod.EModLoadState?(Mod.EModLoadState.NotAntiCheatCompatible)).Count > 0)
			{
				IAntiCheatClient antiCheatClient = PlatformManager.MultiPlatform.AntiCheatClient;
				if (antiCheatClient != null && antiCheatClient.ClientAntiCheatEnabled())
				{
					XUiC_MessageBoxWindowGroup.ShowCustom(this.windowManager.playerUI.xui, Localization.Get("xuiModsAntiCheatModWithCodeTitle", false, null), Localization.Get("xuiModsAntiCheatModWithCodeText", false, null), "", delegate(XUiC_MessageBoxWindowGroup _wdw)
					{
						_wdw.Buttons[0].DefaultConfirm("xuiRestart", delegate
						{
							Utils.RestartGame(Utils.ERestartAntiCheatMode.ForceOff);
						}, true, 0f, 0f, false);
						_wdw.Buttons[2].DefaultCancel("btnContinue", delegate
						{
							XUiC_EulaWindow.Open(this.windowManager.playerUI.xui, false);
						}, true, 0f, 0f, false);
					}, false, true, -1, -1);
					goto IL_0191;
				}
			}
			XUiC_EulaWindow.Open(this.windowManager.playerUI.xui, false);
		}
		IL_0191:
		this.bOpenMainMenu = false;
	}

	// Token: 0x060096D3 RID: 38611 RVA: 0x0038DD09 File Offset: 0x0038BF09
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator startDedicatedServer()
	{
		bool abort = false;
		bool serverUserReady = false;
		IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
		IUserClient userClient = ((crossplatformPlatform != null) ? crossplatformPlatform.UserServer : null);
		if (userClient != null)
		{
			userClient.Login(delegate(IPlatform _, EApiStatusReason _reason, string _text)
			{
				if (_reason != EApiStatusReason.Ok)
				{
					abort = true;
					this.abortServerStartupWithError(new string[]
					{
						"Error starting dedicated server: Failed initializing cross platform user:",
						_reason.ToStringCached<EApiStatusReason>() + ": " + _text
					});
					return;
				}
				serverUserReady = true;
			});
		}
		else
		{
			serverUserReady = true;
		}
		while (!serverUserReady && !abort)
		{
			yield return null;
		}
		if (abort)
		{
			yield break;
		}
		UserDataStorageType userDataStorageType = UserDataStorageType.DeviceLocal;
		GamePrefs.Set(EnumGamePrefs.GameSaveStorageType, (int)userDataStorageType);
		GamePrefs.Set(EnumGamePrefs.UserWorldStorageType, (int)userDataStorageType);
		GamePrefs.Set(EnumGamePrefs.GameWorldLocationType, 5);
		if (GamePrefs.GetString(EnumGamePrefs.GameWorld) == "RWG")
		{
			yield return this.startGeneration(userDataStorageType, delegate(bool _success)
			{
				abort = !_success;
			});
		}
		if (abort)
		{
			yield break;
		}
		yield return new WaitForSeconds(2.5f);
		GamePrefs.Set(EnumGamePrefs.ServerEnabled, true);
		yield return PermissionsManager.ResolvePermissions(EUserPerms.All, false, null);
		string @string = GamePrefs.GetString(EnumGamePrefs.GameWorld);
		if (PathAbstractions.Contextual.FindActiveWorldLocation().Type == PathAbstractions.EAbstractedLocationType.None)
		{
			this.abortServerStartupWithError(new string[] { "Error starting dedicated server: GameWorld \"" + @string + "\" not found!" });
			yield break;
		}
		NetworkConnectionError networkConnectionError = SingletonMonoBehaviour<ConnectionManager>.Instance.StartServers(GamePrefs.GetString(EnumGamePrefs.ServerPassword), false);
		if (networkConnectionError != NetworkConnectionError.NoError)
		{
			this.abortServerStartupWithError(new string[]
			{
				"Error starting dedicated server: " + networkConnectionError.ToStringCached<NetworkConnectionError>(),
				"Make sure all required ports are unused: " + SingletonMonoBehaviour<ConnectionManager>.Instance.GetRequiredPortsString()
			});
		}
		yield break;
	}

	// Token: 0x060096D4 RID: 38612 RVA: 0x0038DD18 File Offset: 0x0038BF18
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator startGeneration(UserDataStorageType _worldStorageType, Action<bool> _finishedCallback)
	{
		string @string = GamePrefs.GetString(EnumGamePrefs.WorldGenSeed);
		int @int = GamePrefs.GetInt(EnumGamePrefs.WorldGenSize);
		string worldName = WorldBuilder.GetGeneratedWorldName(@string, @int);
		PathAbstractions.SearchDefinition worldsSearchPaths = PathAbstractions.WorldsSearchPaths;
		string text = worldName;
		UserDataStorageType? userDataStorageType = new UserDataStorageType?(_worldStorageType);
		PathAbstractions.AbstractedLocation location = worldsSearchPaths.GetLocation(text, null, userDataStorageType);
		if (location.Type == PathAbstractions.EAbstractedLocationType.None)
		{
			WorldBuilder worldBuilder = new WorldBuilder(@string, @int, _worldStorageType);
			yield return worldBuilder.GenerateFromServer();
		}
		else if (GameUtils.WorldInfo.LoadWorldInfo(location) == null)
		{
			this.abortServerStartupWithError(new string[]
			{
				string.Concat(new string[] { "Error starting dedicated server: Folder for requested RWG world \"", worldName, "\" to be generated from seed \"", @string, "\" already exists." }),
				"It does not contain a map_info.xml, so the world likely was never successfully generated!",
				"Either delete the folder or change the WorldGenSeed and/or WorldGenSize settings!",
				"(Path of the world: " + location.FullPath + ")"
			});
			_finishedCallback(false);
			yield break;
		}
		GamePrefs.Set(EnumGamePrefs.GameWorld, worldName);
		yield return new WaitForSeconds(2f);
		_finishedCallback(true);
		yield break;
	}

	// Token: 0x060096D5 RID: 38613 RVA: 0x0038DD38 File Offset: 0x0038BF38
	[PublicizedFrom(EAccessModifier.Private)]
	public void abortServerStartupWithError(params string[] _errorMessage)
	{
		Log.Error("====================================================================================================");
		for (int i = 0; i < _errorMessage.Length; i++)
		{
			Log.Error(_errorMessage[i]);
		}
		Log.Error("====================================================================================================");
		Application.Quit();
	}

	// Token: 0x060096D7 RID: 38615 RVA: 0x0038DD94 File Offset: 0x0038BF94
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator <OnLoginComplete>g__ResolveInitialPermissions|9_0(EUserPerms _perms)
	{
		yield return PermissionsManager.ResolvePermissions(_perms, false, null);
		this.bOpenMainMenu = true;
		yield break;
	}

	// Token: 0x040070F0 RID: 28912
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GUIWindowManager windowManager;

	// Token: 0x040070F1 RID: 28913
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public NGUIWindowManager nguiWindowManager;

	// Token: 0x040070F2 RID: 28914
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bOpenMainMenu;

	// Token: 0x040070F3 RID: 28915
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool loginCheckDone;

	// Token: 0x040070F4 RID: 28916
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string loadingText = "";

	// Token: 0x040070F5 RID: 28917
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string displayedLoadAction = "";

	// Token: 0x040070F6 RID: 28918
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float ServerStartDelaySec = 2.5f;
}
