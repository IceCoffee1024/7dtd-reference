using System;

// Token: 0x020011C7 RID: 4551
public enum EnumGamePrefs
{
	// Token: 0x04006B4E RID: 27470
	CreateLevelName,
	// Token: 0x04006B4F RID: 27471
	CreateLevelDim,
	// Token: 0x04006B50 RID: 27472
	OptionsAmbientVolumeLevel,
	// Token: 0x04006B51 RID: 27473
	OptionsMusicVolumeLevel,
	// Token: 0x04006B52 RID: 27474
	OptionsMenuMusicVolumeLevel,
	// Token: 0x04006B53 RID: 27475
	OptionsOverallAudioVolumeLevel,
	// Token: 0x04006B54 RID: 27476
	OptionsGfxAASharpness,
	// Token: 0x04006B55 RID: 27477
	OptionsGfxWaterQuality,
	// Token: 0x04006B56 RID: 27478
	OptionsGfxViewDistance,
	// Token: 0x04006B57 RID: 27479
	OptionsGfxShadowDistance,
	// Token: 0x04006B58 RID: 27480
	UNUSED_OptionsPlayerModel,
	// Token: 0x04006B59 RID: 27481
	OptionsPlayerModelTexture,
	// Token: 0x04006B5A RID: 27482
	OptionsGfxAA,
	// Token: 0x04006B5B RID: 27483
	OptionsLookSensitivity,
	// Token: 0x04006B5C RID: 27484
	OptionsZoomSensitivity,
	// Token: 0x04006B5D RID: 27485
	OptionsInvertMouse,
	// Token: 0x04006B5E RID: 27486
	OptionsGfxFOV,
	// Token: 0x04006B5F RID: 27487
	UNUSED_OptionsFieldOfViewNew,
	// Token: 0x04006B60 RID: 27488
	ServerPort,
	// Token: 0x04006B61 RID: 27489
	ServerIP,
	// Token: 0x04006B62 RID: 27490
	ServerPassword,
	// Token: 0x04006B63 RID: 27491
	ServerName,
	// Token: 0x04006B64 RID: 27492
	ServerDescription,
	// Token: 0x04006B65 RID: 27493
	ServerWebsiteURL,
	// Token: 0x04006B66 RID: 27494
	ServerPasswordCache,
	// Token: 0x04006B67 RID: 27495
	ServerIsPublic,
	// Token: 0x04006B68 RID: 27496
	ServerMaxPlayerCount,
	// Token: 0x04006B69 RID: 27497
	ServerAllowCrossplay,
	// Token: 0x04006B6A RID: 27498
	ServerEACPeerToPeer,
	// Token: 0x04006B6B RID: 27499
	GameMode,
	// Token: 0x04006B6C RID: 27500
	GameDifficulty,
	// Token: 0x04006B6D RID: 27501
	GameName,
	// Token: 0x04006B6E RID: 27502
	GameNameClient,
	// Token: 0x04006B6F RID: 27503
	GameWorld,
	// Token: 0x04006B70 RID: 27504
	GameVersion,
	// Token: 0x04006B71 RID: 27505
	ConnectToServerIP,
	// Token: 0x04006B72 RID: 27506
	ConnectToServerPort,
	// Token: 0x04006B73 RID: 27507
	PlayerName,
	// Token: 0x04006B74 RID: 27508
	UNUSED_PlayerId,
	// Token: 0x04006B75 RID: 27509
	PlayerPassword,
	// Token: 0x04006B76 RID: 27510
	PlayerAutologin,
	// Token: 0x04006B77 RID: 27511
	PlayerToken,
	// Token: 0x04006B78 RID: 27512
	PlayerSafeZoneHours,
	// Token: 0x04006B79 RID: 27513
	PlayerSafeZoneLevel,
	// Token: 0x04006B7A RID: 27514
	DebugMenuShowTasks,
	// Token: 0x04006B7B RID: 27515
	DebugMenuEnabled,
	// Token: 0x04006B7C RID: 27516
	DebugStopEnemiesMoving,
	// Token: 0x04006B7D RID: 27517
	CreativeMenuEnabled,
	// Token: 0x04006B7E RID: 27518
	FavoriteServersList,
	// Token: 0x04006B7F RID: 27519
	UNUSED_ControlPanelPort,
	// Token: 0x04006B80 RID: 27520
	UNUSED_ControlPanelPassword,
	// Token: 0x04006B81 RID: 27521
	DynamicSpawner,
	// Token: 0x04006B82 RID: 27522
	PlayerKillingMode,
	// Token: 0x04006B83 RID: 27523
	MatchLength,
	// Token: 0x04006B84 RID: 27524
	FragLimit,
	// Token: 0x04006B85 RID: 27525
	RebuildMap,
	// Token: 0x04006B86 RID: 27526
	JoiningOptions,
	// Token: 0x04006B87 RID: 27527
	ZombiePlayers,
	// Token: 0x04006B88 RID: 27528
	BuildCreate,
	// Token: 0x04006B89 RID: 27529
	DayCount,
	// Token: 0x04006B8A RID: 27530
	DayNightLength,
	// Token: 0x04006B8B RID: 27531
	DayLightLength,
	// Token: 0x04006B8C RID: 27532
	BloodMoonFrequency,
	// Token: 0x04006B8D RID: 27533
	BloodMoonRange,
	// Token: 0x04006B8E RID: 27534
	BloodMoonWarning,
	// Token: 0x04006B8F RID: 27535
	ShowFriendPlayerOnMap,
	// Token: 0x04006B90 RID: 27536
	AdminFileName,
	// Token: 0x04006B91 RID: 27537
	UNUSED_ControlPanelEnabled,
	// Token: 0x04006B92 RID: 27538
	TelnetEnabled,
	// Token: 0x04006B93 RID: 27539
	TelnetPort,
	// Token: 0x04006B94 RID: 27540
	ZombieFeralSense,
	// Token: 0x04006B95 RID: 27541
	UNUSED_OptionsSSAO,
	// Token: 0x04006B96 RID: 27542
	ZombieMove,
	// Token: 0x04006B97 RID: 27543
	ZombieMoveNight,
	// Token: 0x04006B98 RID: 27544
	ZombieFeralMove,
	// Token: 0x04006B99 RID: 27545
	ZombieBMMove,
	// Token: 0x04006B9A RID: 27546
	OptionsGfxLODDistance,
	// Token: 0x04006B9B RID: 27547
	DropOnDeath,
	// Token: 0x04006B9C RID: 27548
	DropOnQuit,
	// Token: 0x04006B9D RID: 27549
	DeathPenalty,
	// Token: 0x04006B9E RID: 27550
	LootTimer,
	// Token: 0x04006B9F RID: 27551
	BloodMoonEnemyCount,
	// Token: 0x04006BA0 RID: 27552
	EnemySpawnMode,
	// Token: 0x04006BA1 RID: 27553
	EnemyDifficulty,
	// Token: 0x04006BA2 RID: 27554
	BlockDamagePlayer,
	// Token: 0x04006BA3 RID: 27555
	BlockDamageAI,
	// Token: 0x04006BA4 RID: 27556
	BlockDamageAIBM,
	// Token: 0x04006BA5 RID: 27557
	LootAbundance,
	// Token: 0x04006BA6 RID: 27558
	LootRespawnDays,
	// Token: 0x04006BA7 RID: 27559
	TelnetPassword,
	// Token: 0x04006BA8 RID: 27560
	LandClaimCount,
	// Token: 0x04006BA9 RID: 27561
	LandClaimSize,
	// Token: 0x04006BAA RID: 27562
	LandClaimDeadZone,
	// Token: 0x04006BAB RID: 27563
	LandClaimExpiryTime,
	// Token: 0x04006BAC RID: 27564
	LandClaimDecayMode,
	// Token: 0x04006BAD RID: 27565
	LandClaimOnlineDurabilityModifier,
	// Token: 0x04006BAE RID: 27566
	LandClaimOfflineDurabilityModifier,
	// Token: 0x04006BAF RID: 27567
	LandClaimOfflineDelay,
	// Token: 0x04006BB0 RID: 27568
	AirDropFrequency,
	// Token: 0x04006BB1 RID: 27569
	MaxSpawnedZombies,
	// Token: 0x04006BB2 RID: 27570
	PartySharedKillRange,
	// Token: 0x04006BB3 RID: 27571
	UNUSED_SaveGameFolder,
	// Token: 0x04006BB4 RID: 27572
	OptionsMicVolumeLevel,
	// Token: 0x04006BB5 RID: 27573
	OptionsVoiceVolumeLevel,
	// Token: 0x04006BB6 RID: 27574
	OptionsVoiceChatEnabled,
	// Token: 0x04006BB7 RID: 27575
	OptionsGfxTexQuality,
	// Token: 0x04006BB8 RID: 27576
	AutopilotMode,
	// Token: 0x04006BB9 RID: 27577
	SelectionOperationMode,
	// Token: 0x04006BBA RID: 27578
	SelectionContextMode,
	// Token: 0x04006BBB RID: 27579
	EACEnabled,
	// Token: 0x04006BBC RID: 27580
	PersistentPlayerProfiles,
	// Token: 0x04006BBD RID: 27581
	XPMultiplier,
	// Token: 0x04006BBE RID: 27582
	OptionsAudioOcclusion,
	// Token: 0x04006BBF RID: 27583
	LastGameResetRevision,
	// Token: 0x04006BC0 RID: 27584
	OptionsGfxResolution,
	// Token: 0x04006BC1 RID: 27585
	OptionsGfxVsync,
	// Token: 0x04006BC2 RID: 27586
	OptionsGfxReflectQuality,
	// Token: 0x04006BC3 RID: 27587
	OptionsGfxResetRevision,
	// Token: 0x04006BC4 RID: 27588
	OptionsGfxSignQuality,
	// Token: 0x04006BC5 RID: 27589
	UNUSED_OptionsReflectionCullList,
	// Token: 0x04006BC6 RID: 27590
	UNUSED_OptionsReflectionFarClip,
	// Token: 0x04006BC7 RID: 27591
	UNUSED_OptionsReflectionShadowDistance,
	// Token: 0x04006BC8 RID: 27592
	UNUSED_OptionsReflectionBounces,
	// Token: 0x04006BC9 RID: 27593
	UNUSED_OptionsReflectionTimeSlicingMode,
	// Token: 0x04006BCA RID: 27594
	UNUSED_OptionsReflectionRefreshMode,
	// Token: 0x04006BCB RID: 27595
	OptionsGfxObjQuality,
	// Token: 0x04006BCC RID: 27596
	OptionsGfxGrassDistance,
	// Token: 0x04006BCD RID: 27597
	UNUSED_OptionsMotionBlur,
	// Token: 0x04006BCE RID: 27598
	UNUSED_OptionsObjectBlur,
	// Token: 0x04006BCF RID: 27599
	MaxSpawnedAnimals,
	// Token: 0x04006BD0 RID: 27600
	UNUSED_OptionsBloom,
	// Token: 0x04006BD1 RID: 27601
	UNUSED_OptionsSunShafts,
	// Token: 0x04006BD2 RID: 27602
	UNUSED_OptionsDOF,
	// Token: 0x04006BD3 RID: 27603
	OptionsGfxReflectShadows,
	// Token: 0x04006BD4 RID: 27604
	OptionsAllowController,
	// Token: 0x04006BD5 RID: 27605
	OptionsGfxQualityPreset,
	// Token: 0x04006BD6 RID: 27606
	OptionsScreenBoundsValue,
	// Token: 0x04006BD7 RID: 27607
	OptionsInterfaceSensitivity,
	// Token: 0x04006BD8 RID: 27608
	OptionsControllerVibration,
	// Token: 0x04006BD9 RID: 27609
	NoGraphicsMode,
	// Token: 0x04006BDA RID: 27610
	OptionsHudSize,
	// Token: 0x04006BDB RID: 27611
	OptionsHudOpacity,
	// Token: 0x04006BDC RID: 27612
	OptionsShowCrosshair,
	// Token: 0x04006BDD RID: 27613
	OptionsShowCompass,
	// Token: 0x04006BDE RID: 27614
	ServerDisabledNetworkProtocols,
	// Token: 0x04006BDF RID: 27615
	OptionsBackgroundGlobalOpacity,
	// Token: 0x04006BE0 RID: 27616
	OptionsForegroundGlobalOpacity,
	// Token: 0x04006BE1 RID: 27617
	UNUSED_OptionsGamma,
	// Token: 0x04006BE2 RID: 27618
	OptionsStabSpawnBlocksOnGround,
	// Token: 0x04006BE3 RID: 27619
	OptionsTempCelsius,
	// Token: 0x04006BE4 RID: 27620
	AirDropMarker,
	// Token: 0x04006BE5 RID: 27621
	OptionsGfxWaterPtlLimiter,
	// Token: 0x04006BE6 RID: 27622
	UNUSED_OptionsGfxUMATexQuality,
	// Token: 0x04006BE7 RID: 27623
	HideCommandExecutionLog,
	// Token: 0x04006BE8 RID: 27624
	MaxUncoveredMapChunksPerPlayer,
	// Token: 0x04006BE9 RID: 27625
	ServerReservedSlots,
	// Token: 0x04006BEA RID: 27626
	ServerReservedSlotsPermission,
	// Token: 0x04006BEB RID: 27627
	ServerAdminSlots,
	// Token: 0x04006BEC RID: 27628
	ServerAdminSlotsPermission,
	// Token: 0x04006BED RID: 27629
	GameGuidClient,
	// Token: 0x04006BEE RID: 27630
	BedrollDeadZoneSize,
	// Token: 0x04006BEF RID: 27631
	LastLoadedPrefab,
	// Token: 0x04006BF0 RID: 27632
	UNUSED_LastLoadedPrefabSize,
	// Token: 0x04006BF1 RID: 27633
	OptionsJournalPopup,
	// Token: 0x04006BF2 RID: 27634
	OptionsFilterProfanity,
	// Token: 0x04006BF3 RID: 27635
	TelnetFailedLoginLimit,
	// Token: 0x04006BF4 RID: 27636
	TelnetFailedLoginsBlocktime,
	// Token: 0x04006BF5 RID: 27637
	TerminalWindowEnabled,
	// Token: 0x04006BF6 RID: 27638
	ServerEnabled,
	// Token: 0x04006BF7 RID: 27639
	ServerVisibility,
	// Token: 0x04006BF8 RID: 27640
	ServerLoginConfirmationText,
	// Token: 0x04006BF9 RID: 27641
	WorldGenSeed,
	// Token: 0x04006BFA RID: 27642
	WorldGenSize,
	// Token: 0x04006BFB RID: 27643
	OptionsGfxTreeDistance,
	// Token: 0x04006BFC RID: 27644
	OptionsPOICulling,
	// Token: 0x04006BFD RID: 27645
	OptionsDynamicMusicEnabled,
	// Token: 0x04006BFE RID: 27646
	OptionsDynamicMusicDailyTime,
	// Token: 0x04006BFF RID: 27647
	OptionsPlayChanceFrequency,
	// Token: 0x04006C00 RID: 27648
	OptionsPlayChanceProbability,
	// Token: 0x04006C01 RID: 27649
	UNUSED_UserDataFolder,
	// Token: 0x04006C02 RID: 27650
	OptionsGfxStreamMipmaps,
	// Token: 0x04006C03 RID: 27651
	UNUSED_OptionsStreamingMipmapsBudget,
	// Token: 0x04006C04 RID: 27652
	OptionsGfxBloom,
	// Token: 0x04006C05 RID: 27653
	OptionsGfxDOF,
	// Token: 0x04006C06 RID: 27654
	OptionsGfxMotionBlur,
	// Token: 0x04006C07 RID: 27655
	OptionsGfxSSAO,
	// Token: 0x04006C08 RID: 27656
	OptionsGfxSSReflections,
	// Token: 0x04006C09 RID: 27657
	OptionsGfxSunShafts,
	// Token: 0x04006C0A RID: 27658
	OptionsDisableChunkLODs,
	// Token: 0x04006C0B RID: 27659
	ServerMaxWorldTransferSpeedKiBs,
	// Token: 0x04006C0C RID: 27660
	ServerMaxAllowedViewDistance,
	// Token: 0x04006C0D RID: 27661
	OptionsGfxOcclusion,
	// Token: 0x04006C0E RID: 27662
	BedrollExpiryTime,
	// Token: 0x04006C0F RID: 27663
	OptionsGfxTexFilter,
	// Token: 0x04006C10 RID: 27664
	OptionsGfxTerrainQuality,
	// Token: 0x04006C11 RID: 27665
	OptionsGfxBrightness,
	// Token: 0x04006C12 RID: 27666
	LastLoadingTipRead,
	// Token: 0x04006C13 RID: 27667
	OptionsGfxDynamicMode,
	// Token: 0x04006C14 RID: 27668
	OptionsGfxDynamicMinFPS,
	// Token: 0x04006C15 RID: 27669
	OptionsGfxDynamicScale,
	// Token: 0x04006C16 RID: 27670
	OptionsUiFpsScaling,
	// Token: 0x04006C17 RID: 27671
	OptionsControlsResetRevision,
	// Token: 0x04006C18 RID: 27672
	OptionsWeaponAiming,
	// Token: 0x04006C19 RID: 27673
	DynamicMeshEnabled,
	// Token: 0x04006C1A RID: 27674
	DynamicMeshDistance,
	// Token: 0x04006C1B RID: 27675
	DynamicMeshLandClaimOnly,
	// Token: 0x04006C1C RID: 27676
	DynamicMeshLandClaimBuffer,
	// Token: 0x04006C1D RID: 27677
	DynamicMeshUseImposters,
	// Token: 0x04006C1E RID: 27678
	DynamicMeshMaxRegionCache,
	// Token: 0x04006C1F RID: 27679
	DynamicMeshMaxItemCache,
	// Token: 0x04006C20 RID: 27680
	TwitchBloodMoonAllowed,
	// Token: 0x04006C21 RID: 27681
	TwitchServerPermission,
	// Token: 0x04006C22 RID: 27682
	OptionsVehicleLookSensitivity,
	// Token: 0x04006C23 RID: 27683
	OptionsSelectionBoxAlphaMultiplier,
	// Token: 0x04006C24 RID: 27684
	PlaytestBiome,
	// Token: 0x04006C25 RID: 27685
	Language,
	// Token: 0x04006C26 RID: 27686
	LanguageBrowser,
	// Token: 0x04006C27 RID: 27687
	Region,
	// Token: 0x04006C28 RID: 27688
	ServerHistoryCache,
	// Token: 0x04006C29 RID: 27689
	OptionsVoiceInputDevice,
	// Token: 0x04006C2A RID: 27690
	OptionsVoiceOutputDevice,
	// Token: 0x04006C2B RID: 27691
	MaxChunkAge,
	// Token: 0x04006C2C RID: 27692
	SaveDataLimit,
	// Token: 0x04006C2D RID: 27693
	OptionsSubtitlesEnabled,
	// Token: 0x04006C2E RID: 27694
	OptionsIntroMovieEnabled,
	// Token: 0x04006C2F RID: 27695
	AllowSpawnNearBackpack,
	// Token: 0x04006C30 RID: 27696
	OptionsZoomAccel,
	// Token: 0x04006C31 RID: 27697
	UNUSED_NewGameSetDefaults,
	// Token: 0x04006C32 RID: 27698
	UNUSED_OptionsGfxGameplayResolutionWidth,
	// Token: 0x04006C33 RID: 27699
	UNUSED_OptionsGfxGameplayResolutionHeight,
	// Token: 0x04006C34 RID: 27700
	OptionsMumblePositionalAudioSupport,
	// Token: 0x04006C35 RID: 27701
	WebDashboardEnabled,
	// Token: 0x04006C36 RID: 27702
	WebDashboardPort,
	// Token: 0x04006C37 RID: 27703
	WebDashboardUrl,
	// Token: 0x04006C38 RID: 27704
	EnableMapRendering,
	// Token: 0x04006C39 RID: 27705
	OptionsAutoPartyWithFriends,
	// Token: 0x04006C3A RID: 27706
	OptionsQuestsAutoShare,
	// Token: 0x04006C3B RID: 27707
	OptionsQuestsAutoAccept,
	// Token: 0x04006C3C RID: 27708
	OptionsControllerTriggerEffects,
	// Token: 0x04006C3D RID: 27709
	MaxQueuedMeshLayers,
	// Token: 0x04006C3E RID: 27710
	OptionsControllerSensitivityX,
	// Token: 0x04006C3F RID: 27711
	OptionsControllerSensitivityY,
	// Token: 0x04006C40 RID: 27712
	OptionsControllerLookInvert,
	// Token: 0x04006C41 RID: 27713
	OptionsControllerJoystickLayout,
	// Token: 0x04006C42 RID: 27714
	OptionsControllerLookAcceleration,
	// Token: 0x04006C43 RID: 27715
	OptionsControllerZoomSensitivity,
	// Token: 0x04006C44 RID: 27716
	OptionsControllerLookAxisDeadzone,
	// Token: 0x04006C45 RID: 27717
	OptionsControllerMoveAxisDeadzone,
	// Token: 0x04006C46 RID: 27718
	OptionsControllerCursorSnap,
	// Token: 0x04006C47 RID: 27719
	OptionsControllerCursorHoverSensitivity,
	// Token: 0x04006C48 RID: 27720
	OptionsControllerVehicleSensitivity,
	// Token: 0x04006C49 RID: 27721
	OptionsControllerWeaponAiming,
	// Token: 0x04006C4A RID: 27722
	OptionsControllerAimAssists,
	// Token: 0x04006C4B RID: 27723
	OptionsChatCommunication,
	// Token: 0x04006C4C RID: 27724
	OptionsControlsSprintLock,
	// Token: 0x04006C4D RID: 27725
	OptionsDisableXmlEvents,
	// Token: 0x04006C4E RID: 27726
	DebugPanelsEnabled,
	// Token: 0x04006C4F RID: 27727
	OptionsControllerVibrationStrength,
	// Token: 0x04006C50 RID: 27728
	EulaVersionAccepted,
	// Token: 0x04006C51 RID: 27729
	EulaLatestVersion,
	// Token: 0x04006C52 RID: 27730
	OptionsGfxMotionBlurEnabled,
	// Token: 0x04006C53 RID: 27731
	IgnoreEOSSanctions,
	// Token: 0x04006C54 RID: 27732
	SkipSpawnButton,
	// Token: 0x04006C55 RID: 27733
	OptionsUiCompassUseEnglishCardinalDirections,
	// Token: 0x04006C56 RID: 27734
	OptionsGfxShadowQuality,
	// Token: 0x04006C57 RID: 27735
	QuestProgressionDailyLimit,
	// Token: 0x04006C58 RID: 27736
	OptionsControllerIconStyle,
	// Token: 0x04006C59 RID: 27737
	OptionsShowConsoleButton,
	// Token: 0x04006C5A RID: 27738
	SaveDataLimitType,
	// Token: 0x04006C5B RID: 27739
	OptionsCrossplay,
	// Token: 0x04006C5C RID: 27740
	AllowSpawnNearFriend,
	// Token: 0x04006C5D RID: 27741
	BiomeProgression,
	// Token: 0x04006C5E RID: 27742
	ServerMatchmakingGroup,
	// Token: 0x04006C5F RID: 27743
	OptionsGfxUpscalerMode,
	// Token: 0x04006C60 RID: 27744
	OptionsGfxFSRPreset,
	// Token: 0x04006C61 RID: 27745
	StormFreq,
	// Token: 0x04006C62 RID: 27746
	OptionsGfxFOV3P,
	// Token: 0x04006C63 RID: 27747
	OptionsGfxDefaultFirstPersonCamera,
	// Token: 0x04006C64 RID: 27748
	OptionsGfx3PCameraMode,
	// Token: 0x04006C65 RID: 27749
	OptionsPoiVolumesSkipDeleteConfirmation,
	// Token: 0x04006C66 RID: 27750
	CameraRestrictionMode,
	// Token: 0x04006C67 RID: 27751
	OptionsGfxCameraDistance3P,
	// Token: 0x04006C68 RID: 27752
	AISmellMode,
	// Token: 0x04006C69 RID: 27753
	OptionsControlsDefaultQuickAction,
	// Token: 0x04006C6A RID: 27754
	OptionsBindingsResetRevision,
	// Token: 0x04006C6B RID: 27755
	JarRefund,
	// Token: 0x04006C6C RID: 27756
	OptionsCrosshairEnabled,
	// Token: 0x04006C6D RID: 27757
	OptionsCrosshairScale,
	// Token: 0x04006C6E RID: 27758
	OptionsCrosshairColor,
	// Token: 0x04006C6F RID: 27759
	OptionsCrosshairOpacity,
	// Token: 0x04006C70 RID: 27760
	OptionsCrosshairADS,
	// Token: 0x04006C71 RID: 27761
	OptionsCrosshairDot3P,
	// Token: 0x04006C72 RID: 27762
	OptionsCrosshairThickness,
	// Token: 0x04006C73 RID: 27763
	OptionsCrosshairRangedEnabled,
	// Token: 0x04006C74 RID: 27764
	GameSaveStorageType,
	// Token: 0x04006C75 RID: 27765
	SandboxPreset,
	// Token: 0x04006C76 RID: 27766
	SandboxCode,
	// Token: 0x04006C77 RID: 27767
	UserWorldStorageType,
	// Token: 0x04006C78 RID: 27768
	DiscordFirstTimeInfoShown,
	// Token: 0x04006C79 RID: 27769
	DiscordDisabled,
	// Token: 0x04006C7A RID: 27770
	DiscordLastAccountType,
	// Token: 0x04006C7B RID: 27771
	DiscordAccessToken,
	// Token: 0x04006C7C RID: 27772
	DiscordRefreshToken,
	// Token: 0x04006C7D RID: 27773
	DiscordSelectedOutputDevice,
	// Token: 0x04006C7E RID: 27774
	DiscordSelectedInputDevice,
	// Token: 0x04006C7F RID: 27775
	DiscordOutputVolume,
	// Token: 0x04006C80 RID: 27776
	DiscordInputVolume,
	// Token: 0x04006C81 RID: 27777
	DiscordVoiceModePtt,
	// Token: 0x04006C82 RID: 27778
	DiscordVoiceVadModeAuto,
	// Token: 0x04006C83 RID: 27779
	DiscordVoiceVadThreshold,
	// Token: 0x04006C84 RID: 27780
	DiscordDmPrivacyMode,
	// Token: 0x04006C85 RID: 27781
	DiscordAutoJoinVoiceMode,
	// Token: 0x04006C86 RID: 27782
	ResetUnprotectedChunks,
	// Token: 0x04006C87 RID: 27783
	GameWorldLocationType,
	// Token: 0x04006C88 RID: 27784
	OptionsGfxLimitFpsInGame,
	// Token: 0x04006C89 RID: 27785
	Last
}
