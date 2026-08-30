using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E12 RID: 3602
[Preserve]
public class XUiC_MainMenu : XUiController
{
	// Token: 0x06006E74 RID: 28276 RVA: 0x002ACCD4 File Offset: 0x002AAED4
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

	// Token: 0x06006E75 RID: 28277 RVA: 0x002ACD77 File Offset: 0x002AAF77
	public override void Init()
	{
		base.Init();
		XUiC_MainMenu.ID = base.WindowGroup.Id;
	}

	// Token: 0x06006E76 RID: 28278 RVA: 0x002ACD8F File Offset: 0x002AAF8F
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

	// Token: 0x06006E77 RID: 28279 RVA: 0x002ACDCC File Offset: 0x002AAFCC
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

	// Token: 0x06006E78 RID: 28280 RVA: 0x002ACF38 File Offset: 0x002AB138
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

	// Token: 0x06006E79 RID: 28281 RVA: 0x002ACF8C File Offset: 0x002AB18C
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

	// Token: 0x06006E7A RID: 28282 RVA: 0x002ACFF8 File Offset: 0x002AB1F8
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

	// Token: 0x06006E7B RID: 28283 RVA: 0x002AD06C File Offset: 0x002AB26C
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

	// Token: 0x06006E7C RID: 28284 RVA: 0x002AD110 File Offset: 0x002AB310
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

	// Token: 0x06006E7D RID: 28285 RVA: 0x002AD17C File Offset: 0x002AB37C
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

	// Token: 0x040053F6 RID: 21494
	public static string ID = "";

	// Token: 0x040053F7 RID: 21495
	public static bool openedOnce;

	// Token: 0x040053F8 RID: 21496
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool shownNewsScreenOnce;

	// Token: 0x040053F9 RID: 21497
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool clockSyncChecked;

	// Token: 0x040053FA RID: 21498
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool snapMicPermissionChecked;

	// Token: 0x040053FB RID: 21499
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<string> windowsToOpenGloballyWithMainMenu = new List<string>();

	// Token: 0x040053FC RID: 21500
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<string> windowsToCloseGloballyWithMainMenu = new List<string>();
}
