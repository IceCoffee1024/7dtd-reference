using System;
using System.IO;
using Platform;

// Token: 0x02000A23 RID: 2595
public static class RoamingPrefs
{
	// Token: 0x17000864 RID: 2148
	// (get) Token: 0x06004D5F RID: 19807 RVA: 0x001DD198 File Offset: 0x001DB398
	// (set) Token: 0x06004D60 RID: 19808 RVA: 0x001DD19F File Offset: 0x001DB39F
	public static PrefVersionStore Store
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06004D61 RID: 19809 RVA: 0x001DD1A8 File Offset: 0x001DB3A8
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

	// Token: 0x06004D62 RID: 19810 RVA: 0x001DD2C8 File Offset: 0x001DB4C8
	public static void Destroy()
	{
		RoamingPrefs.Store = null;
	}

	// Token: 0x04003C3D RID: 15421
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
