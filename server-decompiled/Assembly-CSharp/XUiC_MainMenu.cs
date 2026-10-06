using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E21 RID: 3617
[Preserve]
public class XUiC_MainMenu : XUiController
{
	// Token: 0x06006E6C RID: 28268 RVA: 0x002B007C File Offset: 0x002AE27C
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

	// Token: 0x06006E6D RID: 28269 RVA: 0x002B011F File Offset: 0x002AE31F
	public override void Init()
	{
		base.Init();
		XUiC_MainMenu.ID = base.WindowGroup.Id;
	}

	// Token: 0x06006E6E RID: 28270 RVA: 0x002B0137 File Offset: 0x002AE337
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

	// Token: 0x06006E6F RID: 28271 RVA: 0x002B0174 File Offset: 0x002AE374
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

	// Token: 0x06006E70 RID: 28272 RVA: 0x002B02E0 File Offset: 0x002AE4E0
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

	// Token: 0x06006E71 RID: 28273 RVA: 0x002B0334 File Offset: 0x002AE534
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

	// Token: 0x06006E72 RID: 28274 RVA: 0x002B03A0 File Offset: 0x002AE5A0
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

	// Token: 0x06006E73 RID: 28275 RVA: 0x002B0414 File Offset: 0x002AE614
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

	// Token: 0x06006E74 RID: 28276 RVA: 0x002B04B8 File Offset: 0x002AE6B8
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

	// Token: 0x06006E75 RID: 28277 RVA: 0x002B0524 File Offset: 0x002AE724
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

	// Token: 0x0400542D RID: 21549
	public static string ID = "";

	// Token: 0x0400542E RID: 21550
	public static bool openedOnce;

	// Token: 0x0400542F RID: 21551
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool shownNewsScreenOnce;

	// Token: 0x04005430 RID: 21552
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool clockSyncChecked;

	// Token: 0x04005431 RID: 21553
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool snapMicPermissionChecked;

	// Token: 0x04005432 RID: 21554
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<string> windowsToOpenGloballyWithMainMenu = new List<string>();

	// Token: 0x04005433 RID: 21555
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<string> windowsToCloseGloballyWithMainMenu = new List<string>();
}
