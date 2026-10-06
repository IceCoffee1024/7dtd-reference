using System;

// Token: 0x020011FB RID: 4603
public enum EnumGamePrefs
{
	// Token: 0x04006C90 RID: 27792
	CreateLevelName,
	// Token: 0x04006C91 RID: 27793
	CreateLevelDim,
	// Token: 0x04006C92 RID: 27794
	OptionsAmbientVolumeLevel,
	// Token: 0x04006C93 RID: 27795
	OptionsMusicVolumeLevel,
	// Token: 0x04006C94 RID: 27796
	OptionsMenuMusicVolumeLevel,
	// Token: 0x04006C95 RID: 27797
	OptionsOverallAudioVolumeLevel,
	// Token: 0x04006C96 RID: 27798
	OptionsGfxAASharpness,
	// Token: 0x04006C97 RID: 27799
	OptionsGfxWaterQuality,
	// Token: 0x04006C98 RID: 27800
	OptionsGfxViewDistance,
	// Token: 0x04006C99 RID: 27801
	OptionsGfxShadowDistance,
	// Token: 0x04006C9A RID: 27802
	UNUSED_OptionsPlayerModel,
	// Token: 0x04006C9B RID: 27803
	OptionsPlayerModelTexture,
	// Token: 0x04006C9C RID: 27804
	OptionsGfxAA,
	// Token: 0x04006C9D RID: 27805
	OptionsLookSensitivity,
	// Token: 0x04006C9E RID: 27806
	OptionsZoomSensitivity,
	// Token: 0x04006C9F RID: 27807
	OptionsInvertMouse,
	// Token: 0x04006CA0 RID: 27808
	OptionsGfxFOV,
	// Token: 0x04006CA1 RID: 27809
	UNUSED_OptionsFieldOfViewNew,
	// Token: 0x04006CA2 RID: 27810
	ServerPort,
	// Token: 0x04006CA3 RID: 27811
	ServerIP,
	// Token: 0x04006CA4 RID: 27812
	ServerPassword,
	// Token: 0x04006CA5 RID: 27813
	ServerName,
	// Token: 0x04006CA6 RID: 27814
	ServerDescription,
	// Token: 0x04006CA7 RID: 27815
	ServerWebsiteURL,
	// Token: 0x04006CA8 RID: 27816
	ServerPasswordCache,
	// Token: 0x04006CA9 RID: 27817
	ServerIsPublic,
	// Token: 0x04006CAA RID: 27818
	ServerMaxPlayerCount,
	// Token: 0x04006CAB RID: 27819
	ServerAllowCrossplay,
	// Token: 0x04006CAC RID: 27820
	ServerEACPeerToPeer,
	// Token: 0x04006CAD RID: 27821
	GameMode,
	// Token: 0x04006CAE RID: 27822
	GameDifficulty,
	// Token: 0x04006CAF RID: 27823
	GameName,
	// Token: 0x04006CB0 RID: 27824
	GameNameClient,
	// Token: 0x04006CB1 RID: 27825
	GameWorld,
	// Token: 0x04006CB2 RID: 27826
	GameVersion,
	// Token: 0x04006CB3 RID: 27827
	ConnectToServerIP,
	// Token: 0x04006CB4 RID: 27828
	ConnectToServerPort,
	// Token: 0x04006CB5 RID: 27829
	PlayerName,
	// Token: 0x04006CB6 RID: 27830
	UNUSED_PlayerId,
	// Token: 0x04006CB7 RID: 27831
	PlayerPassword,
	// Token: 0x04006CB8 RID: 27832
	PlayerAutologin,
	// Token: 0x04006CB9 RID: 27833
	PlayerToken,
	// Token: 0x04006CBA RID: 27834
	PlayerSafeZoneHours,
	// Token: 0x04006CBB RID: 27835
	PlayerSafeZoneLevel,
	// Token: 0x04006CBC RID: 27836
	DebugMenuShowTasks,
	// Token: 0x04006CBD RID: 27837
	DebugMenuEnabled,
	// Token: 0x04006CBE RID: 27838
	DebugStopEnemiesMoving,
	// Token: 0x04006CBF RID: 27839
	CreativeMenuEnabled,
	// Token: 0x04006CC0 RID: 27840
	FavoriteServersList,
	// Token: 0x04006CC1 RID: 27841
	UNUSED_ControlPanelPort,
	// Token: 0x04006CC2 RID: 27842
	UNUSED_ControlPanelPassword,
	// Token: 0x04006CC3 RID: 27843
	DynamicSpawner,
	// Token: 0x04006CC4 RID: 27844
	PlayerKillingMode,
	// Token: 0x04006CC5 RID: 27845
	MatchLength,
	// Token: 0x04006CC6 RID: 27846
	FragLimit,
	// Token: 0x04006CC7 RID: 27847
	RebuildMap,
	// Token: 0x04006CC8 RID: 27848
	JoiningOptions,
	// Token: 0x04006CC9 RID: 27849
	ZombiePlayers,
	// Token: 0x04006CCA RID: 27850
	BuildCreate,
	// Token: 0x04006CCB RID: 27851
	DayCount,
	// Token: 0x04006CCC RID: 27852
	DayNightLength,
	// Token: 0x04006CCD RID: 27853
	DayLightLength,
	// Token: 0x04006CCE RID: 27854
	BloodMoonFrequency,
	// Token: 0x04006CCF RID: 27855
	BloodMoonRange,
	// Token: 0x04006CD0 RID: 27856
	BloodMoonWarning,
	// Token: 0x04006CD1 RID: 27857
	ShowFriendPlayerOnMap,
	// Token: 0x04006CD2 RID: 27858
	AdminFileName,
	// Token: 0x04006CD3 RID: 27859
	UNUSED_ControlPanelEnabled,
	// Token: 0x04006CD4 RID: 27860
	TelnetEnabled,
	// Token: 0x04006CD5 RID: 27861
	TelnetPort,
	// Token: 0x04006CD6 RID: 27862
	ZombieFeralSense,
	// Token: 0x04006CD7 RID: 27863
	UNUSED_OptionsSSAO,
	// Token: 0x04006CD8 RID: 27864
	ZombieMove,
	// Token: 0x04006CD9 RID: 27865
	ZombieMoveNight,
	// Token: 0x04006CDA RID: 27866
	ZombieFeralMove,
	// Token: 0x04006CDB RID: 27867
	ZombieBMMove,
	// Token: 0x04006CDC RID: 27868
	OptionsGfxLODDistance,
	// Token: 0x04006CDD RID: 27869
	DropOnDeath,
	// Token: 0x04006CDE RID: 27870
	DropOnQuit,
	// Token: 0x04006CDF RID: 27871
	DeathPenalty,
	// Token: 0x04006CE0 RID: 27872
	LootTimer,
	// Token: 0x04006CE1 RID: 27873
	BloodMoonEnemyCount,
	// Token: 0x04006CE2 RID: 27874
	EnemySpawnMode,
	// Token: 0x04006CE3 RID: 27875
	EnemyDifficulty,
	// Token: 0x04006CE4 RID: 27876
	BlockDamagePlayer,
	// Token: 0x04006CE5 RID: 27877
	BlockDamageAI,
	// Token: 0x04006CE6 RID: 27878
	BlockDamageAIBM,
	// Token: 0x04006CE7 RID: 27879
	LootAbundance,
	// Token: 0x04006CE8 RID: 27880
	LootRespawnDays,
	// Token: 0x04006CE9 RID: 27881
	TelnetPassword,
	// Token: 0x04006CEA RID: 27882
	LandClaimCount,
	// Token: 0x04006CEB RID: 27883
	LandClaimSize,
	// Token: 0x04006CEC RID: 27884
	LandClaimDeadZone,
	// Token: 0x04006CED RID: 27885
	LandClaimExpiryTime,
	// Token: 0x04006CEE RID: 27886
	LandClaimDecayMode,
	// Token: 0x04006CEF RID: 27887
	LandClaimOnlineDurabilityModifier,
	// Token: 0x04006CF0 RID: 27888
	LandClaimOfflineDurabilityModifier,
	// Token: 0x04006CF1 RID: 27889
	LandClaimOfflineDelay,
	// Token: 0x04006CF2 RID: 27890
	AirDropFrequency,
	// Token: 0x04006CF3 RID: 27891
	MaxSpawnedZombies,
	// Token: 0x04006CF4 RID: 27892
	PartySharedKillRange,
	// Token: 0x04006CF5 RID: 27893
	UNUSED_SaveGameFolder,
	// Token: 0x04006CF6 RID: 27894
	OptionsMicVolumeLevel,
	// Token: 0x04006CF7 RID: 27895
	OptionsVoiceVolumeLevel,
	// Token: 0x04006CF8 RID: 27896
	OptionsVoiceChatEnabled,
	// Token: 0x04006CF9 RID: 27897
	OptionsGfxTexQuality,
	// Token: 0x04006CFA RID: 27898
	AutopilotMode,
	// Token: 0x04006CFB RID: 27899
	SelectionOperationMode,
	// Token: 0x04006CFC RID: 27900
	SelectionContextMode,
	// Token: 0x04006CFD RID: 27901
	EACEnabled,
	// Token: 0x04006CFE RID: 27902
	PersistentPlayerProfiles,
	// Token: 0x04006CFF RID: 27903
	XPMultiplier,
	// Token: 0x04006D00 RID: 27904
	OptionsAudioOcclusion,
	// Token: 0x04006D01 RID: 27905
	LastGameResetRevision,
	// Token: 0x04006D02 RID: 27906
	OptionsGfxResolution,
	// Token: 0x04006D03 RID: 27907
	OptionsGfxVsync,
	// Token: 0x04006D04 RID: 27908
	OptionsGfxReflectQuality,
	// Token: 0x04006D05 RID: 27909
	OptionsGfxResetRevision,
	// Token: 0x04006D06 RID: 27910
	OptionsGfxSignQuality,
	// Token: 0x04006D07 RID: 27911
	UNUSED_OptionsReflectionCullList,
	// Token: 0x04006D08 RID: 27912
	UNUSED_OptionsReflectionFarClip,
	// Token: 0x04006D09 RID: 27913
	UNUSED_OptionsReflectionShadowDistance,
	// Token: 0x04006D0A RID: 27914
	UNUSED_OptionsReflectionBounces,
	// Token: 0x04006D0B RID: 27915
	UNUSED_OptionsReflectionTimeSlicingMode,
	// Token: 0x04006D0C RID: 27916
	UNUSED_OptionsReflectionRefreshMode,
	// Token: 0x04006D0D RID: 27917
	OptionsGfxObjQuality,
	// Token: 0x04006D0E RID: 27918
	OptionsGfxGrassDistance,
	// Token: 0x04006D0F RID: 27919
	UNUSED_OptionsMotionBlur,
	// Token: 0x04006D10 RID: 27920
	UNUSED_OptionsObjectBlur,
	// Token: 0x04006D11 RID: 27921
	MaxSpawnedAnimals,
	// Token: 0x04006D12 RID: 27922
	UNUSED_OptionsBloom,
	// Token: 0x04006D13 RID: 27923
	UNUSED_OptionsSunShafts,
	// Token: 0x04006D14 RID: 27924
	UNUSED_OptionsDOF,
	// Token: 0x04006D15 RID: 27925
	OptionsGfxReflectShadows,
	// Token: 0x04006D16 RID: 27926
	OptionsAllowController,
	// Token: 0x04006D17 RID: 27927
	OptionsGfxQualityPreset,
	// Token: 0x04006D18 RID: 27928
	OptionsScreenBoundsValue,
	// Token: 0x04006D19 RID: 27929
	OptionsInterfaceSensitivity,
	// Token: 0x04006D1A RID: 27930
	OptionsControllerVibration,
	// Token: 0x04006D1B RID: 27931
	NoGraphicsMode,
	// Token: 0x04006D1C RID: 27932
	OptionsHudSize,
	// Token: 0x04006D1D RID: 27933
	OptionsHudOpacity,
	// Token: 0x04006D1E RID: 27934
	OptionsShowCrosshair,
	// Token: 0x04006D1F RID: 27935
	OptionsShowCompass,
	// Token: 0x04006D20 RID: 27936
	ServerDisabledNetworkProtocols,
	// Token: 0x04006D21 RID: 27937
	OptionsBackgroundGlobalOpacity,
	// Token: 0x04006D22 RID: 27938
	OptionsForegroundGlobalOpacity,
	// Token: 0x04006D23 RID: 27939
	UNUSED_OptionsGamma,
	// Token: 0x04006D24 RID: 27940
	OptionsStabSpawnBlocksOnGround,
	// Token: 0x04006D25 RID: 27941
	OptionsTempCelsius,
	// Token: 0x04006D26 RID: 27942
	AirDropMarker,
	// Token: 0x04006D27 RID: 27943
	OptionsGfxWaterPtlLimiter,
	// Token: 0x04006D28 RID: 27944
	UNUSED_OptionsGfxUMATexQuality,
	// Token: 0x04006D29 RID: 27945
	HideCommandExecutionLog,
	// Token: 0x04006D2A RID: 27946
	MaxUncoveredMapChunksPerPlayer,
	// Token: 0x04006D2B RID: 27947
	ServerReservedSlots,
	// Token: 0x04006D2C RID: 27948
	ServerReservedSlotsPermission,
	// Token: 0x04006D2D RID: 27949
	ServerAdminSlots,
	// Token: 0x04006D2E RID: 27950
	ServerAdminSlotsPermission,
	// Token: 0x04006D2F RID: 27951
	GameGuidClient,
	// Token: 0x04006D30 RID: 27952
	BedrollDeadZoneSize,
	// Token: 0x04006D31 RID: 27953
	LastLoadedPrefab,
	// Token: 0x04006D32 RID: 27954
	UNUSED_LastLoadedPrefabSize,
	// Token: 0x04006D33 RID: 27955
	OptionsJournalPopup,
	// Token: 0x04006D34 RID: 27956
	OptionsFilterProfanity,
	// Token: 0x04006D35 RID: 27957
	TelnetFailedLoginLimit,
	// Token: 0x04006D36 RID: 27958
	TelnetFailedLoginsBlocktime,
	// Token: 0x04006D37 RID: 27959
	TerminalWindowEnabled,
	// Token: 0x04006D38 RID: 27960
	ServerEnabled,
	// Token: 0x04006D39 RID: 27961
	ServerVisibility,
	// Token: 0x04006D3A RID: 27962
	ServerLoginConfirmationText,
	// Token: 0x04006D3B RID: 27963
	WorldGenSeed,
	// Token: 0x04006D3C RID: 27964
	WorldGenSize,
	// Token: 0x04006D3D RID: 27965
	OptionsGfxTreeDistance,
	// Token: 0x04006D3E RID: 27966
	OptionsPOICulling,
	// Token: 0x04006D3F RID: 27967
	OptionsDynamicMusicEnabled,
	// Token: 0x04006D40 RID: 27968
	OptionsDynamicMusicDailyTime,
	// Token: 0x04006D41 RID: 27969
	OptionsPlayChanceFrequency,
	// Token: 0x04006D42 RID: 27970
	OptionsPlayChanceProbability,
	// Token: 0x04006D43 RID: 27971
	UNUSED_UserDataFolder,
	// Token: 0x04006D44 RID: 27972
	OptionsGfxStreamMipmaps,
	// Token: 0x04006D45 RID: 27973
	UNUSED_OptionsStreamingMipmapsBudget,
	// Token: 0x04006D46 RID: 27974
	OptionsGfxBloom,
	// Token: 0x04006D47 RID: 27975
	OptionsGfxDOF,
	// Token: 0x04006D48 RID: 27976
	OptionsGfxMotionBlur,
	// Token: 0x04006D49 RID: 27977
	OptionsGfxSSAO,
	// Token: 0x04006D4A RID: 27978
	OptionsGfxSSReflections,
	// Token: 0x04006D4B RID: 27979
	OptionsGfxSunShafts,
	// Token: 0x04006D4C RID: 27980
	OptionsDisableChunkLODs,
	// Token: 0x04006D4D RID: 27981
	ServerMaxWorldTransferSpeedKiBs,
	// Token: 0x04006D4E RID: 27982
	ServerMaxAllowedViewDistance,
	// Token: 0x04006D4F RID: 27983
	OptionsGfxOcclusion,
	// Token: 0x04006D50 RID: 27984
	BedrollExpiryTime,
	// Token: 0x04006D51 RID: 27985
	OptionsGfxTexFilter,
	// Token: 0x04006D52 RID: 27986
	OptionsGfxTerrainQuality,
	// Token: 0x04006D53 RID: 27987
	OptionsGfxBrightness,
	// Token: 0x04006D54 RID: 27988
	LastLoadingTipRead,
	// Token: 0x04006D55 RID: 27989
	OptionsGfxDynamicMode,
	// Token: 0x04006D56 RID: 27990
	OptionsGfxDynamicMinFPS,
	// Token: 0x04006D57 RID: 27991
	OptionsGfxDynamicScale,
	// Token: 0x04006D58 RID: 27992
	OptionsUiFpsScaling,
	// Token: 0x04006D59 RID: 27993
	OptionsControlsResetRevision,
	// Token: 0x04006D5A RID: 27994
	OptionsWeaponAiming,
	// Token: 0x04006D5B RID: 27995
	DynamicMeshEnabled,
	// Token: 0x04006D5C RID: 27996
	DynamicMeshDistance,
	// Token: 0x04006D5D RID: 27997
	DynamicMeshLandClaimOnly,
	// Token: 0x04006D5E RID: 27998
	DynamicMeshLandClaimBuffer,
	// Token: 0x04006D5F RID: 27999
	DynamicMeshUseImposters,
	// Token: 0x04006D60 RID: 28000
	DynamicMeshMaxRegionCache,
	// Token: 0x04006D61 RID: 28001
	DynamicMeshMaxItemCache,
	// Token: 0x04006D62 RID: 28002
	TwitchBloodMoonAllowed,
	// Token: 0x04006D63 RID: 28003
	TwitchServerPermission,
	// Token: 0x04006D64 RID: 28004
	OptionsVehicleLookSensitivity,
	// Token: 0x04006D65 RID: 28005
	OptionsSelectionBoxAlphaMultiplier,
	// Token: 0x04006D66 RID: 28006
	PlaytestBiome,
	// Token: 0x04006D67 RID: 28007
	Language,
	// Token: 0x04006D68 RID: 28008
	LanguageBrowser,
	// Token: 0x04006D69 RID: 28009
	Region,
	// Token: 0x04006D6A RID: 28010
	ServerHistoryCache,
	// Token: 0x04006D6B RID: 28011
	OptionsVoiceInputDevice,
	// Token: 0x04006D6C RID: 28012
	OptionsVoiceOutputDevice,
	// Token: 0x04006D6D RID: 28013
	MaxChunkAge,
	// Token: 0x04006D6E RID: 28014
	SaveDataLimit,
	// Token: 0x04006D6F RID: 28015
	OptionsSubtitlesEnabled,
	// Token: 0x04006D70 RID: 28016
	OptionsIntroMovieEnabled,
	// Token: 0x04006D71 RID: 28017
	AllowSpawnNearBackpack,
	// Token: 0x04006D72 RID: 28018
	OptionsZoomAccel,
	// Token: 0x04006D73 RID: 28019
	UNUSED_NewGameSetDefaults,
	// Token: 0x04006D74 RID: 28020
	UNUSED_OptionsGfxGameplayResolutionWidth,
	// Token: 0x04006D75 RID: 28021
	UNUSED_OptionsGfxGameplayResolutionHeight,
	// Token: 0x04006D76 RID: 28022
	OptionsMumblePositionalAudioSupport,
	// Token: 0x04006D77 RID: 28023
	WebDashboardEnabled,
	// Token: 0x04006D78 RID: 28024
	WebDashboardPort,
	// Token: 0x04006D79 RID: 28025
	WebDashboardUrl,
	// Token: 0x04006D7A RID: 28026
	EnableMapRendering,
	// Token: 0x04006D7B RID: 28027
	OptionsAutoPartyWithFriends,
	// Token: 0x04006D7C RID: 28028
	OptionsQuestsAutoShare,
	// Token: 0x04006D7D RID: 28029
	OptionsQuestsAutoAccept,
	// Token: 0x04006D7E RID: 28030
	OptionsControllerTriggerEffects,
	// Token: 0x04006D7F RID: 28031
	MaxQueuedMeshLayers,
	// Token: 0x04006D80 RID: 28032
	OptionsControllerSensitivityX,
	// Token: 0x04006D81 RID: 28033
	OptionsControllerSensitivityY,
	// Token: 0x04006D82 RID: 28034
	OptionsControllerLookInvert,
	// Token: 0x04006D83 RID: 28035
	OptionsControllerJoystickLayout,
	// Token: 0x04006D84 RID: 28036
	OptionsControllerLookAcceleration,
	// Token: 0x04006D85 RID: 28037
	OptionsControllerZoomSensitivity,
	// Token: 0x04006D86 RID: 28038
	OptionsControllerLookAxisDeadzone,
	// Token: 0x04006D87 RID: 28039
	OptionsControllerMoveAxisDeadzone,
	// Token: 0x04006D88 RID: 28040
	OptionsControllerCursorSnap,
	// Token: 0x04006D89 RID: 28041
	OptionsControllerCursorHoverSensitivity,
	// Token: 0x04006D8A RID: 28042
	OptionsControllerVehicleSensitivity,
	// Token: 0x04006D8B RID: 28043
	OptionsControllerWeaponAiming,
	// Token: 0x04006D8C RID: 28044
	OptionsControllerAimAssists,
	// Token: 0x04006D8D RID: 28045
	OptionsChatCommunication,
	// Token: 0x04006D8E RID: 28046
	OptionsControlsSprintLock,
	// Token: 0x04006D8F RID: 28047
	OptionsDisableXmlEvents,
	// Token: 0x04006D90 RID: 28048
	DebugPanelsEnabled,
	// Token: 0x04006D91 RID: 28049
	OptionsControllerVibrationStrength,
	// Token: 0x04006D92 RID: 28050
	EulaVersionAccepted,
	// Token: 0x04006D93 RID: 28051
	EulaLatestVersion,
	// Token: 0x04006D94 RID: 28052
	OptionsGfxMotionBlurEnabled,
	// Token: 0x04006D95 RID: 28053
	IgnoreEOSSanctions,
	// Token: 0x04006D96 RID: 28054
	SkipSpawnButton,
	// Token: 0x04006D97 RID: 28055
	OptionsUiCompassUseEnglishCardinalDirections,
	// Token: 0x04006D98 RID: 28056
	OptionsGfxShadowQuality,
	// Token: 0x04006D99 RID: 28057
	QuestProgressionDailyLimit,
	// Token: 0x04006D9A RID: 28058
	OptionsControllerIconStyle,
	// Token: 0x04006D9B RID: 28059
	OptionsShowConsoleButton,
	// Token: 0x04006D9C RID: 28060
	SaveDataLimitType,
	// Token: 0x04006D9D RID: 28061
	OptionsCrossplay,
	// Token: 0x04006D9E RID: 28062
	AllowSpawnNearFriend,
	// Token: 0x04006D9F RID: 28063
	BiomeProgression,
	// Token: 0x04006DA0 RID: 28064
	ServerMatchmakingGroup,
	// Token: 0x04006DA1 RID: 28065
	OptionsGfxUpscalerMode,
	// Token: 0x04006DA2 RID: 28066
	OptionsGfxFSRPreset,
	// Token: 0x04006DA3 RID: 28067
	StormFreq,
	// Token: 0x04006DA4 RID: 28068
	OptionsGfxFOV3P,
	// Token: 0x04006DA5 RID: 28069
	OptionsGfxDefaultFirstPersonCamera,
	// Token: 0x04006DA6 RID: 28070
	OptionsGfx3PCameraMode,
	// Token: 0x04006DA7 RID: 28071
	OptionsPoiVolumesSkipDeleteConfirmation,
	// Token: 0x04006DA8 RID: 28072
	CameraRestrictionMode,
	// Token: 0x04006DA9 RID: 28073
	OptionsGfxCameraDistance3P,
	// Token: 0x04006DAA RID: 28074
	AISmellMode,
	// Token: 0x04006DAB RID: 28075
	OptionsControlsDefaultQuickAction,
	// Token: 0x04006DAC RID: 28076
	OptionsBindingsResetRevision,
	// Token: 0x04006DAD RID: 28077
	JarRefund,
	// Token: 0x04006DAE RID: 28078
	OptionsCrosshairEnabled,
	// Token: 0x04006DAF RID: 28079
	OptionsCrosshairScale,
	// Token: 0x04006DB0 RID: 28080
	OptionsCrosshairColor,
	// Token: 0x04006DB1 RID: 28081
	OptionsCrosshairOpacity,
	// Token: 0x04006DB2 RID: 28082
	OptionsCrosshairADS,
	// Token: 0x04006DB3 RID: 28083
	OptionsCrosshairDot3P,
	// Token: 0x04006DB4 RID: 28084
	OptionsCrosshairThickness,
	// Token: 0x04006DB5 RID: 28085
	OptionsCrosshairRangedEnabled,
	// Token: 0x04006DB6 RID: 28086
	GameSaveStorageType,
	// Token: 0x04006DB7 RID: 28087
	SandboxPreset,
	// Token: 0x04006DB8 RID: 28088
	SandboxCode,
	// Token: 0x04006DB9 RID: 28089
	UserWorldStorageType,
	// Token: 0x04006DBA RID: 28090
	DiscordFirstTimeInfoShown,
	// Token: 0x04006DBB RID: 28091
	DiscordDisabled,
	// Token: 0x04006DBC RID: 28092
	DiscordLastAccountType,
	// Token: 0x04006DBD RID: 28093
	DiscordAccessToken,
	// Token: 0x04006DBE RID: 28094
	DiscordRefreshToken,
	// Token: 0x04006DBF RID: 28095
	DiscordSelectedOutputDevice,
	// Token: 0x04006DC0 RID: 28096
	DiscordSelectedInputDevice,
	// Token: 0x04006DC1 RID: 28097
	DiscordOutputVolume,
	// Token: 0x04006DC2 RID: 28098
	DiscordInputVolume,
	// Token: 0x04006DC3 RID: 28099
	DiscordVoiceModePtt,
	// Token: 0x04006DC4 RID: 28100
	DiscordVoiceVadModeAuto,
	// Token: 0x04006DC5 RID: 28101
	DiscordVoiceVadThreshold,
	// Token: 0x04006DC6 RID: 28102
	DiscordDmPrivacyMode,
	// Token: 0x04006DC7 RID: 28103
	DiscordAutoJoinVoiceMode,
	// Token: 0x04006DC8 RID: 28104
	ResetUnprotectedChunks,
	// Token: 0x04006DC9 RID: 28105
	GameWorldLocationType,
	// Token: 0x04006DCA RID: 28106
	OptionsGfxLimitFpsInGame,
	// Token: 0x04006DCB RID: 28107
	DiscordMuteDmNotifications,
	// Token: 0x04006DCC RID: 28108
	Last
}
