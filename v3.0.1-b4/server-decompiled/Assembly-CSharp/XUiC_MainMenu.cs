using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000DFD RID: 3581
[Preserve]
public class XUiC_MainMenu : XUiController
{
	// Token: 0x06006DD4 RID: 28116 RVA: 0x002A97C0 File Offset: 0x002A79C0
	[XuiXmlAttribute("windows_to_open_globally", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeWindowsToOpenGlobally(string _value)
	{
		string[] array = _value.Split(',', StringSplitOptions.None);
		for (int i = 0; i < array.Length; i++)
		{
			string text = array[i].Trim();
			if (!string.IsNullOrEmpty(text))
			{
				char c = text[0];
				if (c != '+')
				{
					if (c != '-')
					{
						XUiC_MainMenu.windowsToOpenGloballyWithMainMenu.Add(text);
						XUiC_MainMenu.windowsToCloseGloballyWithMainMenu.Add(text);
					}
					else
					{
						List<string> list = XUiC_MainMenu.windowsToCloseGloballyWithMainMenu;
						string text2 = text;
						list.Add(text2.Substring(1, text2.Length - 1));
					}
				}
				else
				{
					List<string> list2 = XUiC_MainMenu.windowsToOpenGloballyWithMainMenu;
					string text2 = text;
					list2.Add(text2.Substring(1, text2.Length - 1));
				}
			}
		}
	}

	// Token: 0x06006DD5 RID: 28117 RVA: 0x002A9863 File Offset: 0x002A7A63
	public override void Init()
	{
		base.Init();
		XUiC_MainMenu.ID = base.WindowGroup.Id;
	}

	// Token: 0x06006DD6 RID: 28118 RVA: 0x002A987B File Offset: 0x002A7A7B
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.windowGroup.isEscClosable = false;
		base.RefreshBindings();
		if (PlatformApplicationManager.CheckRestartCoroutineReady() && AutomationRunner.Instance.RestartAllowed())
		{
			ThreadManager.StartCoroutine(PlatformApplicationManager.CheckRestartCoroutine(false));
		}
	}

	// Token: 0x06006DD7 RID: 28119 RVA: 0x002A98B8 File Offset: 0x002A7AB8
	public override void OnOpen()
	{
		base.OnOpen();
		bool flag = !XUiC_MainMenu.openedOnce;
		PlatformManager.MultiPlatform.RichPresence.UpdateRichPresence(IRichPresence.PresenceStates.Menu);
		TitleStorageOverridesManager.Instance.FetchFromSource(null);
		ModEvents.SMainMenuOpenedData smainMenuOpenedData = new ModEvents.SMainMenuOpenedData(!XUiC_MainMenu.openedOnce);
		ModEvents.MainMenuOpened.Invoke(ref smainMenuOpenedData);
		XUiC_MainMenu.openedOnce = true;
		SaveDataUtils.SaveDataManager.CommitAsync();
		this.xui.playerUI.windowManager.Close("eacWarning");
		this.xui.playerUI.windowManager.Close("crossplayWarning");
		this.xui.playerUI.windowManager.ResetActionSets();
		XUiC_MainMenu.OpenGlobalMenuWindows(this.xui);
		GameManager.Instance.SetCursorEnabledOverride(false, false);
		this.xui.playerUI.CursorController.SetCursorHidden(false);
		TriggerEffectManager.SetMainMenuLightbarColor();
		if (flag)
		{
			ThreadManager.RunTaskAfterFrames(new Action(Localization.UnloadUnusedLanguages), 1);
		}
		if (flag && !string.IsNullOrEmpty(LaunchPrefs.RunAutomation.Value))
		{
			if (AutomationRunner.Instance.LoadScript(AutomationScript.LoadFromFile(LaunchPrefs.RunAutomation.Value)))
			{
				AutomationRunner.Instance.StartRuns();
			}
			else
			{
				Log.Error("[XUiC_MainMenu] Failed to load automation script '" + LaunchPrefs.RunAutomation.Value + "'.");
			}
		}
		if (flag && MainMenuMono.IsQuickContinue)
		{
			this.quickContinue();
			return;
		}
		if (this.snapCheck())
		{
			return;
		}
		this.clockSyncCheck();
	}

	// Token: 0x06006DD8 RID: 28120 RVA: 0x002A9A24 File Offset: 0x002A7C24
	[PublicizedFrom(EAccessModifier.Private)]
	public void quickContinue()
	{
		Log.Out("QuickContinue mode, loading last savegame");
		GamePrefs.Instance.Load(GameIO.GetSaveGameDir() + "/gameOptions.sdf");
		NetworkConnectionError networkConnectionError = SingletonMonoBehaviour<ConnectionManager>.Instance.StartServers(GamePrefs.GetString(EnumGamePrefs.ServerPassword), false);
		if (networkConnectionError == NetworkConnectionError.NoError)
		{
			return;
		}
		XUiC_MessageBoxWindowGroup.ShowNetworkError(this.xui, networkConnectionError, false);
	}

	// Token: 0x06006DD9 RID: 28121 RVA: 0x002A9A78 File Offset: 0x002A7C78
	[PublicizedFrom(EAccessModifier.Private)]
	public bool snapCheck()
	{
		if (XUiC_MainMenu.snapMicPermissionChecked)
		{
			return false;
		}
		XUiC_MainMenu.snapMicPermissionChecked = true;
		if (VoiceHelpers.IsSnapWithoutMicPermission() && (!DiscordManager.Instance.Settings.DiscordDisabled || GamePrefs.GetBool(EnumGamePrefs.OptionsVoiceChatEnabled)))
		{
			XUiC_MessageBoxWindowGroup.ShowOk(this.xui, Localization.Get("xuiLinuxSnapNoMicPermissionTitle", false, null), Localization.Get("xuiLinuxSnapNoMicPermissionText", false, null), "", null, true, true, false);
			return true;
		}
		return false;
	}

	// Token: 0x06006DDA RID: 28122 RVA: 0x002A9AE4 File Offset: 0x002A7CE4
	[PublicizedFrom(EAccessModifier.Private)]
	public void clockSyncCheck()
	{
		if (XUiC_MainMenu.clockSyncChecked)
		{
			return;
		}
		if (!GameManager.ServerClockSync.RequestComplete)
		{
			return;
		}
		XUiC_MainMenu.clockSyncChecked = true;
		if (!GameManager.ServerClockSync.HasError && Math.Abs(GameManager.ServerClockSync.SecondsOffset) >= 120)
		{
			XUiC_MessageBoxWindowGroup.ShowOk(this.xui, Localization.Get("xuiSystemTimeSyncHeader", false, null), Localization.Get("xuiSystemTimeSyncWarning", false, null), "", null, true, true, false);
		}
	}

	// Token: 0x06006DDB RID: 28123 RVA: 0x002A9B58 File Offset: 0x002A7D58
	public static void Open(XUi _xuiInstance)
	{
		XUiC_MainMenu.OpenGlobalMenuWindows(_xuiInstance);
		if (LaunchPrefs.SkipNewsScreen.Value || !string.IsNullOrEmpty(LaunchPrefs.RunAutomation.Value) || PlatformApplicationManager.LoadSaveGame.GetState() != EPlatformLoadSaveGameState.Done || MainMenuMono.IsQuickContinue)
		{
			XUiC_MainMenu.shownNewsScreenOnce = true;
		}
		if (InviteManager.Instance.HasPendingInvite())
		{
			XUiC_MainMenu.shownNewsScreenOnce = true;
		}
		if (!XUiC_MainMenu.shownNewsScreenOnce)
		{
			XUiC_NewsScreen.Open(_xuiInstance);
			XUiC_MainMenu.shownNewsScreenOnce = true;
			return;
		}
		ModEvents.SMainMenuOpeningData smainMenuOpeningData = new ModEvents.SMainMenuOpeningData(XUiC_MainMenu.openedOnce);
		if (ModEvents.MainMenuOpening.Invoke(ref smainMenuOpeningData).Item1 == ModEvents.EModEventResult.StopHandlersAndVanilla)
		{
			return;
		}
		_xuiInstance.playerUI.windowManager.Open(XUiC_MainMenu.ID, true);
	}

	// Token: 0x06006DDC RID: 28124 RVA: 0x002A9BFC File Offset: 0x002A7DFC
	public static void OpenGlobalMenuWindows(XUi _xui)
	{
		LocalPlayerUI playerUI = _xui.playerUI;
		GUIWindowManager windowManager = playerUI.windowManager;
		playerUI.nguiWindowManager.Show(EnumNGUIWindow.MainMenuBackground, false);
		foreach (string text in XUiC_MainMenu.windowsToOpenGloballyWithMainMenu)
		{
			windowManager.Open(text, false);
		}
	}

	// Token: 0x06006DDD RID: 28125 RVA: 0x002A9C68 File Offset: 0x002A7E68
	public static void CloseGlobalMenuWindows(XUi _xui)
	{
		LocalPlayerUI playerUI = _xui.playerUI;
		GUIWindowManager windowManager = playerUI.windowManager;
		playerUI.nguiWindowManager.Show(EnumNGUIWindow.MainMenuBackground, false);
		foreach (string text in XUiC_MainMenu.windowsToCloseGloballyWithMainMenu)
		{
			windowManager.Close(text);
		}
	}

	// Token: 0x04005353 RID: 21331
	public static string ID = "";

	// Token: 0x04005354 RID: 21332
	public static bool openedOnce;

	// Token: 0x04005355 RID: 21333
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool shownNewsScreenOnce;

	// Token: 0x04005356 RID: 21334
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool clockSyncChecked;

	// Token: 0x04005357 RID: 21335
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool snapMicPermissionChecked;

	// Token: 0x04005358 RID: 21336
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<string> windowsToOpenGloballyWithMainMenu = new List<string>();

	// Token: 0x04005359 RID: 21337
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<string> windowsToCloseGloballyWithMainMenu = new List<string>();
}
