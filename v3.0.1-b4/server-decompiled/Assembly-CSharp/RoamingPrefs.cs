using System;
using System.IO;
using Platform;

// Token: 0x020009FE RID: 2558
public static class RoamingPrefs
{
	// Token: 0x1700082C RID: 2092
	// (get) Token: 0x06004CE6 RID: 19686 RVA: 0x001D8368 File Offset: 0x001D6568
	// (set) Token: 0x06004CE7 RID: 19687 RVA: 0x001D836F File Offset: 0x001D656F
	public static PrefVersionStore Store
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06004CE8 RID: 19688 RVA: 0x001D8378 File Offset: 0x001D6578
	public static void Init()
	{
		if (!PlatformManager.MultiPlatform.UserDataRoaming.IsSupported || !PlatformManager.MultiPlatform.UserDataRoaming.IsRoamingOptional)
		{
			return;
		}
		RoamingPrefs.Store = new PrefVersionStore(Path.Combine(GameIO.GetRoamingUserGameDataDir(), "RoamingPlayerPrefs"));
		foreach (EnumGamePrefs enumGamePrefs in RoamingPrefs.s_gamePrefs)
		{
			PrefType prefType;
			if (GameOptionsReset.TryGetPrefType(enumGamePrefs, out prefType))
			{
				string groupId = GameOptionsReset.GetGroupId(enumGamePrefs);
				RoamingPrefs.Store.AddPref(groupId, enumGamePrefs.ToStringCached<EnumGamePrefs>(), prefType);
			}
		}
		foreach (ValueTuple<PlayerActionsBase, string> valueTuple in GameOptionsControls.ActionSetPrefs)
		{
			string item = valueTuple.Item2;
			RoamingPrefs.Store.AddPref(GameOptionsReset.Bindings.VersionId, item, PrefType.String);
		}
		RoamingPrefs.Store.AddPref(GameOptionsReset.Bindings.VersionId, "ActionSetsSaved", PrefType.Int);
		RoamingPrefs.Store.AddPref(GameOptionsReset.Bindings.VersionId, "Controls", PrefType.String);
		Log.Out("[RoamingPrefs] initialized");
	}

	// Token: 0x06004CE9 RID: 19689 RVA: 0x001D8498 File Offset: 0x001D6698
	public static void Destroy()
	{
		RoamingPrefs.Store = null;
	}

	// Token: 0x04003B7E RID: 15230
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly EnumGamePrefs[] s_gamePrefs = new EnumGamePrefs[]
	{
		EnumGamePrefs.OptionsAmbientVolumeLevel,
		EnumGamePrefs.OptionsDynamicMusicEnabled,
		EnumGamePrefs.OptionsDynamicMusicDailyTime,
		EnumGamePrefs.OptionsMusicVolumeLevel,
		EnumGamePrefs.OptionsMenuMusicVolumeLevel,
		EnumGamePrefs.OptionsOverallAudioVolumeLevel,
		EnumGamePrefs.OptionsSubtitlesEnabled,
		EnumGamePrefs.OptionsFilterProfanity,
		EnumGamePrefs.OptionsLookSensitivity,
		EnumGamePrefs.OptionsZoomSensitivity,
		EnumGamePrefs.OptionsZoomAccel,
		EnumGamePrefs.OptionsVehicleLookSensitivity,
		EnumGamePrefs.OptionsWeaponAiming,
		EnumGamePrefs.OptionsInvertMouse,
		EnumGamePrefs.OptionsControlsSprintLock,
		EnumGamePrefs.OptionsControlsDefaultQuickAction,
		EnumGamePrefs.OptionsControllerIconStyle,
		EnumGamePrefs.OptionsControllerJoystickLayout,
		EnumGamePrefs.OptionsControllerSensitivityX,
		EnumGamePrefs.OptionsControllerSensitivityY,
		EnumGamePrefs.OptionsControllerZoomSensitivity,
		EnumGamePrefs.OptionsControllerVehicleSensitivity,
		EnumGamePrefs.OptionsControllerLookInvert,
		EnumGamePrefs.OptionsControllerLookAcceleration,
		EnumGamePrefs.OptionsControllerLookAxisDeadzone,
		EnumGamePrefs.OptionsControllerMoveAxisDeadzone,
		EnumGamePrefs.OptionsControllerAimAssists,
		EnumGamePrefs.OptionsControllerWeaponAiming,
		EnumGamePrefs.OptionsControllerVibration,
		EnumGamePrefs.OptionsControllerVibrationStrength,
		EnumGamePrefs.OptionsControllerTriggerEffects,
		EnumGamePrefs.OptionsControllerCursorHoverSensitivity,
		EnumGamePrefs.OptionsControllerCursorSnap,
		EnumGamePrefs.ConnectToServerIP,
		EnumGamePrefs.ConnectToServerPort,
		EnumGamePrefs.LanguageBrowser,
		EnumGamePrefs.Region,
		EnumGamePrefs.ServerPasswordCache,
		EnumGamePrefs.FavoriteServersList,
		EnumGamePrefs.ServerHistoryCache,
		EnumGamePrefs.Language,
		EnumGamePrefs.OptionsUiCompassUseEnglishCardinalDirections,
		EnumGamePrefs.OptionsTempCelsius,
		EnumGamePrefs.OptionsDisableXmlEvents,
		EnumGamePrefs.OptionsQuestsAutoShare,
		EnumGamePrefs.OptionsQuestsAutoAccept,
		EnumGamePrefs.OptionsAutoPartyWithFriends,
		EnumGamePrefs.OptionsChatCommunication,
		EnumGamePrefs.OptionsCrossplay,
		EnumGamePrefs.OptionsShowConsoleButton,
		EnumGamePrefs.OptionsCrosshairEnabled,
		EnumGamePrefs.OptionsCrosshairScale,
		EnumGamePrefs.OptionsCrosshairColor,
		EnumGamePrefs.OptionsCrosshairOpacity,
		EnumGamePrefs.OptionsCrosshairADS,
		EnumGamePrefs.OptionsCrosshairDot3P,
		EnumGamePrefs.OptionsCrosshairThickness,
		EnumGamePrefs.OptionsCrosshairRangedEnabled,
		EnumGamePrefs.EulaVersionAccepted
	};
}
