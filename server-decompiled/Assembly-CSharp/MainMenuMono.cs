using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Platform;
using Services.Analytics;
using UnityEngine;
using WorldGenerationEngineFinal;

// Token: 0x02001271 RID: 4721
public class MainMenuMono : MonoBehaviour
{
	// Token: 0x170011DE RID: 4574
	// (get) Token: 0x06009640 RID: 38464 RVA: 0x00389167 File Offset: 0x00387367
	public static bool IsQuickContinue
	{
		get
		{
			return GameUtils.GetLaunchArgument("quick-continue") != null || (ToggleCapsLock.GetScrollLock() && Application.isEditor);
		}
	}

	// Token: 0x06009641 RID: 38465 RVA: 0x00389185 File Offset: 0x00387385
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Awake()
	{
		this.windowManager = base.GetComponent<GUIWindowManager>();
		this.nguiWindowManager = base.GetComponent<NGUIWindowManager>();
	}

	// Token: 0x06009642 RID: 38466 RVA: 0x003891A0 File Offset: 0x003873A0
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

	// Token: 0x06009643 RID: 38467 RVA: 0x003891F5 File Offset: 0x003873F5
	[PublicizedFrom(EAccessModifier.Private)]
	public bool CheckLogin()
	{
		this.loginCheckDone = true;
		this.bOpenMainMenu = false;
		XUiC_LoginBase.Login(this.windowManager.playerUI.xui, new Action(this.OnLoginComplete));
		return this.bOpenMainMenu;
	}

	// Token: 0x06009644 RID: 38468 RVA: 0x0038922C File Offset: 0x0038742C
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

	// Token: 0x06009645 RID: 38469 RVA: 0x0038931C File Offset: 0x0038751C
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

	// Token: 0x06009646 RID: 38470 RVA: 0x003894C1 File Offset: 0x003876C1
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

	// Token: 0x06009647 RID: 38471 RVA: 0x003894D0 File Offset: 0x003876D0
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

	// Token: 0x06009648 RID: 38472 RVA: 0x003894F0 File Offset: 0x003876F0
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

	// Token: 0x0600964A RID: 38474 RVA: 0x0038954C File Offset: 0x0038774C
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator <OnLoginComplete>g__ResolveInitialPermissions|9_0(EUserPerms _perms)
	{
		yield return PermissionsManager.ResolvePermissions(_perms, false, null);
		this.bOpenMainMenu = true;
		yield break;
	}

	// Token: 0x04007086 RID: 28806
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GUIWindowManager windowManager;

	// Token: 0x04007087 RID: 28807
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public NGUIWindowManager nguiWindowManager;

	// Token: 0x04007088 RID: 28808
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bOpenMainMenu;

	// Token: 0x04007089 RID: 28809
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool loginCheckDone;

	// Token: 0x0400708A RID: 28810
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string loadingText = "";

	// Token: 0x0400708B RID: 28811
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string displayedLoadAction = "";

	// Token: 0x0400708C RID: 28812
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float ServerStartDelaySec = 2.5f;
}
