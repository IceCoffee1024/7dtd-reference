using System;
using UnityEngine;

// Token: 0x02001173 RID: 4467
public class Constants
{
	// Token: 0x04006889 RID: 26761
	public static bool IsWebPlayer = false;

	// Token: 0x0400688A RID: 26762
	public static bool Is32BitOs = IntPtr.Size == 4;

	// Token: 0x0400688B RID: 26763
	public const int cMinShadowDistance = 20;

	// Token: 0x0400688C RID: 26764
	public const string cOptionsIni = "UserOptions.ini";

	// Token: 0x0400688D RID: 26765
	public const string cProduct = "7 Days To Die";

	// Token: 0x0400688E RID: 26766
	public const string cProductAbbrev = "7DTD";

	// Token: 0x0400688F RID: 26767
	public const VersionInformation.EGameReleaseType cReleaseType = VersionInformation.EGameReleaseType.V;

	// Token: 0x04006890 RID: 26768
	public const int cVersionMajor = 3;

	// Token: 0x04006891 RID: 26769
	public const int cVersionMinor = 1;

	// Token: 0x04006892 RID: 26770
	public const int cVersionBuild = 4;

	// Token: 0x04006893 RID: 26771
	public static readonly VersionInformation cVersionInformation = new VersionInformation(VersionInformation.EGameReleaseType.V, 3, 1, 4);

	// Token: 0x04006894 RID: 26772
	public const int cGameResetRevision = 13;

	// Token: 0x04006895 RID: 26773
	public const int cGraphicsResetRevision = 4;

	// Token: 0x04006896 RID: 26774
	public const int cControlsResetRevision = 7;

	// Token: 0x04006897 RID: 26775
	public const int cBindingsResetRevision = 1;

	// Token: 0x04006898 RID: 26776
	public const string cCopyright = "Copyright (c) 2014-2026 The Fun Pimps LLC All Rights Reserved.";

	// Token: 0x04006899 RID: 26777
	public const int cMaxMPPlayers = 8;

	// Token: 0x0400689A RID: 26778
	public const int cMaxCrossplayMPPlayers = 8;

	// Token: 0x0400689B RID: 26779
	public const int cDefaultUserPermissionLevel = 1000;

	// Token: 0x0400689C RID: 26780
	public const string cDefaultPlayerName = "Player";

	// Token: 0x0400689D RID: 26781
	public const string cDirWorlds = "Data/Worlds";

	// Token: 0x0400689E RID: 26782
	public const string cDirPrefabs = "Data/Prefabs";

	// Token: 0x0400689F RID: 26783
	public const string cDirBluff = "Data/Bluffs";

	// Token: 0x040068A0 RID: 26784
	public const string cDirBlocks = "Data/Config";

	// Token: 0x040068A1 RID: 26785
	public const string cDirGroups = "Data/Config/Groups";

	// Token: 0x040068A2 RID: 26786
	public const string cDirItems = "Data/Config";

	// Token: 0x040068A3 RID: 26787
	public const string cDirWorldCreation = "Data/Config";

	// Token: 0x040068A4 RID: 26788
	public const string cDirConfig = "Data/Config";

	// Token: 0x040068A5 RID: 26789
	public const string cDirPresets = "Data/Presets";

	// Token: 0x040068A6 RID: 26790
	public const string cDirHeightmaps = "Data/Heightmaps";

	// Token: 0x040068A7 RID: 26791
	public const string cDirBackgroundImage = "Data/Textures/misc/background.jpg";

	// Token: 0x040068A8 RID: 26792
	public const string cDirAssetBundles = "Data/Bundles/Standalone";

	// Token: 0x040068A9 RID: 26793
	public const string cDirPrefabParts = "Data/Prefabs/Parts";

	// Token: 0x040068AA RID: 26794
	public const string cFolderResourcesSounds = "Sounds";

	// Token: 0x040068AB RID: 26795
	public const string cFolderResourcesConfig = "Data/Config";

	// Token: 0x040068AC RID: 26796
	public const string cFolderResourcesTextures = "Textures";

	// Token: 0x040068AD RID: 26797
	public const string cFolderResourcesEnvironment = "Textures/Environment";

	// Token: 0x040068AE RID: 26798
	public const string cFolderResourcesTerrainTextures = "Textures/Terrain";

	// Token: 0x040068AF RID: 26799
	public const string cFolderResourcesLocalizationEnglish = "GUI/Localization/English";

	// Token: 0x040068B0 RID: 26800
	public const string cFolderSaveGame = "Saves";

	// Token: 0x040068B1 RID: 26801
	public const string cFolderSaveGameLocal = "SavesLocal";

	// Token: 0x040068B2 RID: 26802
	public const string cFolderSaveRegion = "Region";

	// Token: 0x040068B3 RID: 26803
	public const string cFolderSavePlayer = "Player";

	// Token: 0x040068B4 RID: 26804
	public const string cFolderGeneratedWorlds = "GeneratedWorlds";

	// Token: 0x040068B5 RID: 26805
	public const string cFolderRemoteWorld = "World";

	// Token: 0x040068B6 RID: 26806
	public const string cFolderLocalPrefabs = "LocalPrefabs";

	// Token: 0x040068B7 RID: 26807
	public const string cFolderSaveTwitch = "Twitch";

	// Token: 0x040068B8 RID: 26808
	public const string cFolderSavePresets = "Presets";

	// Token: 0x040068B9 RID: 26809
	public const string cDirConfigInternal = "DataInternal/Config";

	// Token: 0x040068BA RID: 26810
	public const string cExtLevels = ".ttw";

	// Token: 0x040068BB RID: 26811
	public const string cExtPrefabs = ".tts";

	// Token: 0x040068BC RID: 26812
	public const string cExtPrefabImposters = ".mesh";

	// Token: 0x040068BD RID: 26813
	public const string cExtIdNameMappings = ".nim";

	// Token: 0x040068BE RID: 26814
	public const string cExtPlayedLevel = ".played";

	// Token: 0x040068BF RID: 26815
	public const string cExtSdf = ".sdf";

	// Token: 0x040068C0 RID: 26816
	public const string cExtFlag = ".flag";

	// Token: 0x040068C1 RID: 26817
	public const string cFileMainTTW = "main.ttw";

	// Token: 0x040068C2 RID: 26818
	public const string cFileWorldChecksums = "checksums.txt";

	// Token: 0x040068C3 RID: 26819
	public static readonly string cFileBlockMappings = "blockmappings.nim";

	// Token: 0x040068C4 RID: 26820
	public static readonly string cFileItemMappings = "itemmappings.nim";

	// Token: 0x040068C5 RID: 26821
	public const string cFileDecos = "decoration.7dt";

	// Token: 0x040068C6 RID: 26822
	public const string cFileMultiBlocks = "multiblocks.7dt";

	// Token: 0x040068C7 RID: 26823
	public const string cGameNameDefault = "Region";

	// Token: 0x040068C8 RID: 26824
	public const string cSdfFileName = "gameOptions.sdf";

	// Token: 0x040068C9 RID: 26825
	public const string cPersistentPlayersFileName = "players.xml";

	// Token: 0x040068CA RID: 26826
	public const string cAchievementFilename = "achievements.bin";

	// Token: 0x040068CB RID: 26827
	public const string cNewGameSdfFileName = "newGameOptions.sdf";

	// Token: 0x040068CC RID: 26828
	public const string cFileArchivedFlag = "archived.flag";

	// Token: 0x040068CD RID: 26829
	public static string cPrefixAtlas = "ta_";

	// Token: 0x040068CE RID: 26830
	public const string cLevelPrefab = "prefabs";

	// Token: 0x040068CF RID: 26831
	public const string cSpawnPoints = "spawnpoints";

	// Token: 0x040068D0 RID: 26832
	public const string cExtTexturePack = ".xml";

	// Token: 0x040068D1 RID: 26833
	public static string cArgDedicatedServer = "-dedicated";

	// Token: 0x040068D2 RID: 26834
	public static string cArgSubmissionBuild = "-submission";

	// Token: 0x040068D3 RID: 26835
	public const int cMaxBiomes = 50;

	// Token: 0x040068D4 RID: 26836
	public const string cTagLargeEntityBlocker = "LargeEntityBlocker";

	// Token: 0x040068D5 RID: 26837
	public const string cTagPhysics = "Physics";

	// Token: 0x040068D6 RID: 26838
	public const int cLayerDefault = 0;

	// Token: 0x040068D7 RID: 26839
	public const int cLayerTransparentFx = 1;

	// Token: 0x040068D8 RID: 26840
	public const int cLayerIgnoreRaycast = 2;

	// Token: 0x040068D9 RID: 26841
	public const int cLayerRemotePlayerCCPhysics = 3;

	// Token: 0x040068DA RID: 26842
	public const int cLayerWater = 4;

	// Token: 0x040068DB RID: 26843
	public const int cLayerNoShadow = 8;

	// Token: 0x040068DC RID: 26844
	public const int cLayerBackgroundImage = 9;

	// Token: 0x040068DD RID: 26845
	public const int cLayerHoldingItem = 10;

	// Token: 0x040068DE RID: 26846
	public const int cLayerRenderInTexture = 11;

	// Token: 0x040068DF RID: 26847
	public const int cLayerNGUI = 12;

	// Token: 0x040068E0 RID: 26848
	public const int cLayerItems = 13;

	// Token: 0x040068E1 RID: 26849
	public const int cLayerNoCollision = 14;

	// Token: 0x040068E2 RID: 26850
	public const int cLayerCCPhysics = 15;

	// Token: 0x040068E3 RID: 26851
	public const int cLayerTerrainCollision = 16;

	// Token: 0x040068E4 RID: 26852
	public const int cLayerPhysicsDead = 17;

	// Token: 0x040068E5 RID: 26853
	public const int cLayerGrass = 18;

	// Token: 0x040068E6 RID: 26854
	public const int cLayerLargeEntityBlocker = 19;

	// Token: 0x040068E7 RID: 26855
	public const int cLayerLocalCCPhysics = 20;

	// Token: 0x040068E8 RID: 26856
	public const int cLayerPhysics = 21;

	// Token: 0x040068E9 RID: 26857
	public const int cLayerUnderwaterEffects = 22;

	// Token: 0x040068EA RID: 26858
	public const int cLayerTrees = 23;

	// Token: 0x040068EB RID: 26859
	public const int cLayerLocalPlayer = 24;

	// Token: 0x040068EC RID: 26860
	public const int cLayerPlayerRagdollsOLD = 27;

	// Token: 0x040068ED RID: 26861
	public const int cLayerTerrain = 28;

	// Token: 0x040068EE RID: 26862
	public const int cLayerWires = 29;

	// Token: 0x040068EF RID: 26863
	public const int cLayerGlass = 30;

	// Token: 0x040068F0 RID: 26864
	public const int cLayerVolumes = 31;

	// Token: 0x040068F1 RID: 26865
	public const int cLayerMaskItems = 8192;

	// Token: 0x040068F2 RID: 26866
	public const int cLayerMaskIgnoreRayCast = 538480652;

	// Token: 0x040068F3 RID: 26867
	public const int cLayerMaskGrass = 262144;

	// Token: 0x040068F4 RID: 26868
	public const int cLayerMaskWater = 16;

	// Token: 0x040068F5 RID: 26869
	public const int cLayerMaskLocalPlayer = 17825792;

	// Token: 0x040068F6 RID: 26870
	public const int cLayerMaskAllLayers = -538480653;

	// Token: 0x040068F7 RID: 26871
	public const int cLayerMaskNoItems = -538488845;

	// Token: 0x040068F8 RID: 26872
	public const int cLayerMaskOnlyItemsAndCollision = 73728;

	// Token: 0x040068F9 RID: 26873
	public const int cLayerMaskNoItemsNoGrass = -538750989;

	// Token: 0x040068FA RID: 26874
	public const int cLayerMaskNoItemsNoGrassNoWater = -538751005;

	// Token: 0x040068FB RID: 26875
	public const int cLayerMaskNoItemsNoLocalPlayer = -555266061;

	// Token: 0x040068FC RID: 26876
	public const int cLayerMaskAttackingBlocksMask = 1073807360;

	// Token: 0x040068FD RID: 26877
	public const int cLayerMaskSight = -1612492829;

	// Token: 0x040068FE RID: 26878
	public const int cLayerMaskTrajectory = 1082195968;

	// Token: 0x040068FF RID: 26879
	public static int cDistanceRandomDisplayUpdates = 30;

	// Token: 0x04006900 RID: 26880
	public static float cRunningFOVMultiplier = 1.05f;

	// Token: 0x04006901 RID: 26881
	public static float cRunningFOVSpeedDown = 3f;

	// Token: 0x04006902 RID: 26882
	public static float cRunningFOVSpeedUp = 1f;

	// Token: 0x04006903 RID: 26883
	public static int cDefaultCameraFieldOfView = 65;

	// Token: 0x04006904 RID: 26884
	public static int cMinCameraFieldOfView = 50;

	// Token: 0x04006905 RID: 26885
	public static int cMaxCameraFieldOfView = 85;

	// Token: 0x04006906 RID: 26886
	public static readonly Vector3 cDefaultCameraPlayerOffset = new Vector3(0f, 1.6f, 0f);

	// Token: 0x04006907 RID: 26887
	public const int cMaxVertices = 786432;

	// Token: 0x04006908 RID: 26888
	public static float cMinGlobalBackgroundOpacity = 0.55f;

	// Token: 0x04006909 RID: 26889
	public static float cMinGlobalForegroundOpacity = 0.75f;

	// Token: 0x0400690A RID: 26890
	public const int cTicksPerSecond = 20;

	// Token: 0x0400690B RID: 26891
	public const float cTickDuration = 0.05f;

	// Token: 0x0400690C RID: 26892
	public const float cPhysicsTicksPerSecond = 50f;

	// Token: 0x0400690D RID: 26893
	public const float cPhysicsTickDuration = 0.02f;

	// Token: 0x0400690E RID: 26894
	public static float cDefaultDistortionFactor = 1f;

	// Token: 0x0400690F RID: 26895
	public static int cMaxEntitiesPerMobSpawner = 8;

	// Token: 0x04006910 RID: 26896
	public const byte cMaxLightValue = 15;

	// Token: 0x04006911 RID: 26897
	public static float cSizePlanesAround = 250f;

	// Token: 0x04006912 RID: 26898
	public static int cDefaultPort = 26900;

	// Token: 0x04006913 RID: 26899
	public static int cLevelServerPort = 6789;

	// Token: 0x04006914 RID: 26900
	public static int cRandomSpawnPointsToPlace = 4;

	// Token: 0x04006915 RID: 26901
	public static int cStartTeamTickets = 0;

	// Token: 0x04006916 RID: 26902
	public static int cTeamTicketsAlarm = 10;

	// Token: 0x04006917 RID: 26903
	public static float cDecreaseOneTicketTime = 5f;

	// Token: 0x04006918 RID: 26904
	public static float cTimeGameOverButWaitSeconds = 5f;

	// Token: 0x04006919 RID: 26905
	public static float cDigAndBuildDistance = 4f;

	// Token: 0x0400691A RID: 26906
	public static float cCollectItemDistance = 2f;

	// Token: 0x0400691B RID: 26907
	public static float cPlayerInteractDistance = 5f;

	// Token: 0x0400691C RID: 26908
	public static float cRespawnAfterDeathTime = 3f;

	// Token: 0x0400691D RID: 26909
	public static float cRespawnEnterGameTime = 0f;

	// Token: 0x0400691E RID: 26910
	public static float cRespawnAfterFallenDown = 3f;

	// Token: 0x0400691F RID: 26911
	public static float cHitColorDuration = 0.15f;

	// Token: 0x04006920 RID: 26912
	public static float cItemDroppedOnDeathLifetime = 300f;

	// Token: 0x04006921 RID: 26913
	public const float cItemDroppedLifetime = 60f;

	// Token: 0x04006922 RID: 26914
	public static float cItemExplosionLifetime = 30f;

	// Token: 0x04006923 RID: 26915
	public static float cItemHealthDroppedLifetime = 60f;

	// Token: 0x04006924 RID: 26916
	public static float cItemSpawnPointLifetime = float.MaxValue;

	// Token: 0x04006925 RID: 26917
	public static float cItemPortalLifetime = float.MaxValue;

	// Token: 0x04006926 RID: 26918
	public static float cItemItemSpawnerLifetime = float.MaxValue;

	// Token: 0x04006927 RID: 26919
	public static int cHardenBrickTime = 20;

	// Token: 0x04006928 RID: 26920
	public const int cItemQualityTierVariations = 1;

	// Token: 0x04006929 RID: 26921
	public const ushort cItemMaxQuality = 6;

	// Token: 0x0400692A RID: 26922
	public static float cMinHolsterTime = 0.1f;

	// Token: 0x0400692B RID: 26923
	public static float cMinUnHolsterTime = 0.1f;

	// Token: 0x0400692C RID: 26924
	public static float cEnergyJetpackPerBlock = 10f;

	// Token: 0x0400692D RID: 26925
	public static int cHealthPotionAdd = 30;

	// Token: 0x0400692E RID: 26926
	public static int cMaxPlayerFood = 100;

	// Token: 0x0400692F RID: 26927
	public static int cFoodOversaturate = 100;

	// Token: 0x04006930 RID: 26928
	public static int cMaxPlayerDrink = 100;

	// Token: 0x04006931 RID: 26929
	public static int cDrinkOversaturate = 100;

	// Token: 0x04006932 RID: 26930
	public static int cItemDropCountWhenDead = 3;

	// Token: 0x04006933 RID: 26931
	public static float cBuildIntervall = 0.5f;

	// Token: 0x04006934 RID: 26932
	public static float cSendWorldTickTimeToClients = 1.5f;

	// Token: 0x04006935 RID: 26933
	public static float cCheckGameState = 0.5f;

	// Token: 0x04006936 RID: 26934
	public static float cSneakDamageMultiplier = 2f;

	// Token: 0x04006937 RID: 26935
	public static int cNumberOfTeams = 2;

	// Token: 0x04006938 RID: 26936
	public static Color[] cTeamColors = new Color[]
	{
		Color.white,
		new Color(0f, 0.8f, 1f),
		Color.red
	};

	// Token: 0x04006939 RID: 26937
	public static string[] cTeamName = new string[] { "No", "BLUE", "RED" };

	// Token: 0x0400693A RID: 26938
	public static string[] cTeamSkinName = new string[] { "Soldier Blue", "Soldier Blue", "Soldier Red" };

	// Token: 0x0400693B RID: 26939
	public static float cDarkAtNightSubtraction = 12f;

	// Token: 0x0400693C RID: 26940
	public static float cDefaultMonsterSeeDistance = 48f;

	// Token: 0x0400693D RID: 26941
	public static float cPlayerSpeedModifierRunning = 1.6f;

	// Token: 0x0400693E RID: 26942
	public static float cPlayerSpeedModifierWalking = 0.8f;

	// Token: 0x0400693F RID: 26943
	public static float cPlayerSpeedModifierCrouching = 0.4f;

	// Token: 0x04006940 RID: 26944
	public static int cPosInventoryYSub = 70;

	// Token: 0x04006941 RID: 26945
	public static int cPosMinimapY = 10;

	// Token: 0x04006942 RID: 26946
	public static int cPosMinimapSubRight = 20;

	// Token: 0x04006943 RID: 26947
	public static Color cColorBlood = new Color(0.8f, 0f, 0.08f);

	// Token: 0x04006944 RID: 26948
	public static Color cColorBorderBox = new Color(0.8f, 0f, 0f, 0.5f);

	// Token: 0x04006945 RID: 26949
	public static Vector3 cStartPositionPlayerInLevel = new Vector3(0f, 200f, 0f);

	// Token: 0x04006946 RID: 26950
	public static Vector3 cStartRotationPlayerInLevel = new Vector3(0f, 0f, 0f);

	// Token: 0x04006947 RID: 26951
	public static BlockValue cTerrainBlockValue = new BlockValue(1U);

	// Token: 0x04006948 RID: 26952
	public static string cTerrainFillerBlockName = "terrainFiller";

	// Token: 0x04006949 RID: 26953
	public static string cTerrainFiller2BlockName = "terrainFillerAdaptive";

	// Token: 0x0400694A RID: 26954
	public static string cPOIFillerBlock = "poiFillerBlock";

	// Token: 0x0400694B RID: 26955
	public static string cQuestLootFetchContainerIndexName = "FetchContainer";

	// Token: 0x0400694C RID: 26956
	public static string cQuestRestorePowerIndexName = "QuestRestorePower";

	// Token: 0x0400694D RID: 26957
	public static Color[] TrackedFriendColors = new Color[]
	{
		Color.green,
		Color.blue,
		Color.yellow,
		new Color(1f, 0f, 1f),
		new Color(0.5f, 0.25f, 0f),
		new Color(1f, 0.5f, 0f),
		new Color32(56, 35, 16, byte.MaxValue),
		new Color32(42, 59, 0, byte.MaxValue)
	};

	// Token: 0x0400694E RID: 26958
	public const int cMaxViewDistanceOptions = 7;

	// Token: 0x0400694F RID: 26959
	public const int cMinViewDistance = 4;

	// Token: 0x04006950 RID: 26960
	public const int cMaxViewDistance = 12;

	// Token: 0x04006951 RID: 26961
	public const float cBlockDamageLosesPaint = 1f;

	// Token: 0x04006952 RID: 26962
	public static int cEnemySenseMemory = 60;

	// Token: 0x04006953 RID: 26963
	public const int ChunkCompressionLevel = 3;

	// Token: 0x04006954 RID: 26964
	public const int NetworkCompressionLevel = 3;

	// Token: 0x04006955 RID: 26965
	public const string cSpecialWorldName_Empty = "Empty";

	// Token: 0x04006956 RID: 26966
	public const string cSpecialWorldName_Playtesting = "Playtesting";

	// Token: 0x04006957 RID: 26967
	public const string cSpecialWorldName_Navezgane = "Navezgane";

	// Token: 0x04006958 RID: 26968
	public const float cMouseSensitivityMin = 0.01f;

	// Token: 0x04006959 RID: 26969
	public const float cMouseSensitivityMax = 1.5f;

	// Token: 0x0400695A RID: 26970
	public const float cControllerSensitivityMin = 0.05f;

	// Token: 0x0400695B RID: 26971
	public const float cControllerSensitivityMax = 1f;

	// Token: 0x0400695C RID: 26972
	public const float cControllerModifierSensitivityMax = 2f;

	// Token: 0x0400695D RID: 26973
	public const int cPlayTestingSpawnOffset = 10;

	// Token: 0x0400695E RID: 26974
	public const float cAimAssistMaxDistance = 50f;

	// Token: 0x0400695F RID: 26975
	public const float cAimAssistSlowDownEntity = 0.5f;

	// Token: 0x04006960 RID: 26976
	public const float cAimAssistSlowDownItem = 0.6f;

	// Token: 0x04006961 RID: 26977
	public const float cAimAssistSlowDownItemDistance = 10f;

	// Token: 0x04006962 RID: 26978
	public const float cAimAssistSlowThreatLevelThreshold = 0.75f;

	// Token: 0x04006963 RID: 26979
	public const float cAimAssistSnapScreenDistance = 0.15f;

	// Token: 0x04006964 RID: 26980
	public const float cCameraSnapTime = 0.3f;

	// Token: 0x04006965 RID: 26981
	public const float cAimAssistSnapMaximumAngle = 15f;

	// Token: 0x04006966 RID: 26982
	public const float cAimAssistZoomSnapSpeed = 1f;

	// Token: 0x04006967 RID: 26983
	public const float cAimAssistMeleeSnapAngle = 20f;

	// Token: 0x04006968 RID: 26984
	public const float cAimAssistMeleeSnapSpeed = 1.5f;

	// Token: 0x04006969 RID: 26985
	public const float cAimAssistMeleeHitSnapAngle = 30f;

	// Token: 0x0400696A RID: 26986
	public const float cRapidTriggerFireDelay = 0.25f;

	// Token: 0x0400696B RID: 26987
	public const float cMapViewControllerSpeed = 500f;

	// Token: 0x0400696C RID: 26988
	public const float cRunToggleHoldTime = 0.2f;

	// Token: 0x0400696D RID: 26989
	public const float cRecoveryPositionAttemptTime = 30f;

	// Token: 0x0400696E RID: 26990
	public const float cRecoveryPositionMinSqrMagnitude = 10000f;

	// Token: 0x0400696F RID: 26991
	public const int cMaxRecoveryPositions = 5;

	// Token: 0x04006970 RID: 26992
	public const float cCursorSensitivityMin = 0.1f;

	// Token: 0x04006971 RID: 26993
	public const float cCursorSensitivityMax = 1f;

	// Token: 0x04006972 RID: 26994
	public const int cNavWorldSizeX = 6144;

	// Token: 0x04006973 RID: 26995
	public const int cNavWorldSizeZ = 6144;

	// Token: 0x04006974 RID: 26996
	public const float cPartyActivationRange = 15f;

	// Token: 0x04006975 RID: 26997
	public const int cMaxPartySize = 8;

	// Token: 0x04006976 RID: 26998
	public const int cDummyWaterTexId = 5000;

	// Token: 0x04006977 RID: 26999
	public static int cMaxLoadTimePixelsPerTest = 4096;

	// Token: 0x04006978 RID: 27000
	public static int cMaxLoadTimePerFrameMillis = 50;

	// Token: 0x04006979 RID: 27001
	public const float cInputRepeatDelay = 0.1f;

	// Token: 0x0400697A RID: 27002
	public const float cInputInitialRepeatDelay = 0.35f;

	// Token: 0x0400697B RID: 27003
	public const int cConsoleMaxPersistentPlayerDataEntries = 100;

	// Token: 0x0400697C RID: 27004
	public const int cVirtualKeyboardDefaultCharacterLimit = 200;

	// Token: 0x02001174 RID: 4468
	public enum EBiomePoiMap : byte
	{
		// Token: 0x0400697E RID: 27006
		CityAsphalt = 1,
		// Token: 0x0400697F RID: 27007
		CountryRoadAsphalt,
		// Token: 0x04006980 RID: 27008
		RoadGravel,
		// Token: 0x04006981 RID: 27009
		Sand,
		// Token: 0x04006982 RID: 27010
		Free
	}
}
