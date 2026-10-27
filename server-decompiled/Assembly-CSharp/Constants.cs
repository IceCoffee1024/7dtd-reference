using System;
using UnityEngine;

// Token: 0x020011B5 RID: 4533
public class Constants
{
	// Token: 0x040069FE RID: 27134
	public static bool IsWebPlayer = false;

	// Token: 0x040069FF RID: 27135
	public static bool Is32BitOs = IntPtr.Size == 4;

	// Token: 0x04006A00 RID: 27136
	public const int cMinShadowDistance = 20;

	// Token: 0x04006A01 RID: 27137
	public const string cOptionsIni = "UserOptions.ini";

	// Token: 0x04006A02 RID: 27138
	public const string cProduct = "7 Days To Die";

	// Token: 0x04006A03 RID: 27139
	public const string cProductAbbrev = "7DTD";

	// Token: 0x04006A04 RID: 27140
	public const VersionInformation.EGameReleaseType cReleaseType = VersionInformation.EGameReleaseType.V;

	// Token: 0x04006A05 RID: 27141
	public const int cVersionMajor = 3;

	// Token: 0x04006A06 RID: 27142
	public const int cVersionMinor = 40;

	// Token: 0x04006A07 RID: 27143
	public const int cVersionBuild = 6;

	// Token: 0x04006A08 RID: 27144
	public static readonly VersionInformation cVersionInformation = new VersionInformation(VersionInformation.EGameReleaseType.V, 3, 40, 6);

	// Token: 0x04006A09 RID: 27145
	public const int cGameResetRevision = 13;

	// Token: 0x04006A0A RID: 27146
	public const int cGraphicsResetRevision = 4;

	// Token: 0x04006A0B RID: 27147
	public const int cControlsResetRevision = 7;

	// Token: 0x04006A0C RID: 27148
	public const int cBindingsResetRevision = 1;

	// Token: 0x04006A0D RID: 27149
	public const string cCopyright = "Copyright (c) 2014-2026 The Fun Pimps LLC All Rights Reserved.";

	// Token: 0x04006A0E RID: 27150
	public const int cMaxMPPlayers = 8;

	// Token: 0x04006A0F RID: 27151
	public const int cMaxCrossplayMPPlayers = 8;

	// Token: 0x04006A10 RID: 27152
	public const int cDefaultUserPermissionLevel = 1000;

	// Token: 0x04006A11 RID: 27153
	public const string cDefaultPlayerName = "Player";

	// Token: 0x04006A12 RID: 27154
	public const string cDirWorlds = "Data/Worlds";

	// Token: 0x04006A13 RID: 27155
	public const string cDirPrefabs = "Data/Prefabs";

	// Token: 0x04006A14 RID: 27156
	public const string cDirBluff = "Data/Bluffs";

	// Token: 0x04006A15 RID: 27157
	public const string cDirBlocks = "Data/Config";

	// Token: 0x04006A16 RID: 27158
	public const string cDirGroups = "Data/Config/Groups";

	// Token: 0x04006A17 RID: 27159
	public const string cDirItems = "Data/Config";

	// Token: 0x04006A18 RID: 27160
	public const string cDirWorldCreation = "Data/Config";

	// Token: 0x04006A19 RID: 27161
	public const string cDirConfig = "Data/Config";

	// Token: 0x04006A1A RID: 27162
	public const string cDirPresets = "Data/Presets";

	// Token: 0x04006A1B RID: 27163
	public const string cDirHeightmaps = "Data/Heightmaps";

	// Token: 0x04006A1C RID: 27164
	public const string cDirBackgroundImage = "Data/Textures/misc/background.jpg";

	// Token: 0x04006A1D RID: 27165
	public const string cDirAssetBundles = "Data/Bundles/Standalone";

	// Token: 0x04006A1E RID: 27166
	public const string cDirPrefabParts = "Data/Prefabs/Parts";

	// Token: 0x04006A1F RID: 27167
	public const string cFolderResourcesSounds = "Sounds";

	// Token: 0x04006A20 RID: 27168
	public const string cFolderResourcesConfig = "Data/Config";

	// Token: 0x04006A21 RID: 27169
	public const string cFolderResourcesTextures = "Textures";

	// Token: 0x04006A22 RID: 27170
	public const string cFolderResourcesEnvironment = "Textures/Environment";

	// Token: 0x04006A23 RID: 27171
	public const string cFolderResourcesTerrainTextures = "Textures/Terrain";

	// Token: 0x04006A24 RID: 27172
	public const string cFolderResourcesLocalizationEnglish = "GUI/Localization/English";

	// Token: 0x04006A25 RID: 27173
	public const string cFolderSaveGame = "Saves";

	// Token: 0x04006A26 RID: 27174
	public const string cFolderSaveGameLocal = "SavesLocal";

	// Token: 0x04006A27 RID: 27175
	public const string cFolderSaveRegion = "Region";

	// Token: 0x04006A28 RID: 27176
	public const string cFolderSavePlayer = "Player";

	// Token: 0x04006A29 RID: 27177
	public const string cFolderGeneratedWorlds = "GeneratedWorlds";

	// Token: 0x04006A2A RID: 27178
	public const string cFolderRemoteWorld = "World";

	// Token: 0x04006A2B RID: 27179
	public const string cFolderLocalPrefabs = "LocalPrefabs";

	// Token: 0x04006A2C RID: 27180
	public const string cFolderSaveTwitch = "Twitch";

	// Token: 0x04006A2D RID: 27181
	public const string cFolderSavePresets = "Presets";

	// Token: 0x04006A2E RID: 27182
	public const string cDirConfigInternal = "DataInternal/Config";

	// Token: 0x04006A2F RID: 27183
	public const string cExtLevels = ".ttw";

	// Token: 0x04006A30 RID: 27184
	public const string cExtPrefabs = ".tts";

	// Token: 0x04006A31 RID: 27185
	public const string cExtPrefabImposters = ".mesh";

	// Token: 0x04006A32 RID: 27186
	public const string cExtIdNameMappings = ".nim";

	// Token: 0x04006A33 RID: 27187
	public const string cExtPlayedLevel = ".played";

	// Token: 0x04006A34 RID: 27188
	public const string cExtSdf = ".sdf";

	// Token: 0x04006A35 RID: 27189
	public const string cExtFlag = ".flag";

	// Token: 0x04006A36 RID: 27190
	public const string cFileMainTTW = "main.ttw";

	// Token: 0x04006A37 RID: 27191
	public const string cFileWorldChecksums = "checksums.txt";

	// Token: 0x04006A38 RID: 27192
	public static readonly string cFileBlockMappings = "blockmappings.nim";

	// Token: 0x04006A39 RID: 27193
	public static readonly string cFileItemMappings = "itemmappings.nim";

	// Token: 0x04006A3A RID: 27194
	public const string cFileDecos = "decoration.7dt";

	// Token: 0x04006A3B RID: 27195
	public const string cFileMultiBlocks = "multiblocks.7dt";

	// Token: 0x04006A3C RID: 27196
	public const string cGameNameDefault = "Region";

	// Token: 0x04006A3D RID: 27197
	public const string cSdfFileName = "gameOptions.sdf";

	// Token: 0x04006A3E RID: 27198
	public const string cPersistentPlayersFileName = "players.xml";

	// Token: 0x04006A3F RID: 27199
	public const string cAchievementFilename = "achievements.bin";

	// Token: 0x04006A40 RID: 27200
	public const string cNewGameSdfFileName = "newGameOptions.sdf";

	// Token: 0x04006A41 RID: 27201
	public const string cFileArchivedFlag = "archived.flag";

	// Token: 0x04006A42 RID: 27202
	public static string cPrefixAtlas = "ta_";

	// Token: 0x04006A43 RID: 27203
	public const string cLevelPrefab = "prefabs";

	// Token: 0x04006A44 RID: 27204
	public const string cSpawnPoints = "spawnpoints";

	// Token: 0x04006A45 RID: 27205
	public const string cExtTexturePack = ".xml";

	// Token: 0x04006A46 RID: 27206
	public static string cArgDedicatedServer = "-dedicated";

	// Token: 0x04006A47 RID: 27207
	public static string cArgSubmissionBuild = "-submission";

	// Token: 0x04006A48 RID: 27208
	public const int cMaxBiomes = 50;

	// Token: 0x04006A49 RID: 27209
	public const string cTagLargeEntityBlocker = "LargeEntityBlocker";

	// Token: 0x04006A4A RID: 27210
	public const string cTagPhysics = "Physics";

	// Token: 0x04006A4B RID: 27211
	public const int cLayerDefault = 0;

	// Token: 0x04006A4C RID: 27212
	public const int cLayerTransparentFx = 1;

	// Token: 0x04006A4D RID: 27213
	public const int cLayerIgnoreRaycast = 2;

	// Token: 0x04006A4E RID: 27214
	public const int cLayerRemotePlayerCCPhysics = 3;

	// Token: 0x04006A4F RID: 27215
	public const int cLayerWater = 4;

	// Token: 0x04006A50 RID: 27216
	public const int cLayerNoShadow = 8;

	// Token: 0x04006A51 RID: 27217
	public const int cLayerBackgroundImage = 9;

	// Token: 0x04006A52 RID: 27218
	public const int cLayerHoldingItem = 10;

	// Token: 0x04006A53 RID: 27219
	public const int cLayerRenderInTexture = 11;

	// Token: 0x04006A54 RID: 27220
	public const int cLayerNGUI = 12;

	// Token: 0x04006A55 RID: 27221
	public const int cLayerItems = 13;

	// Token: 0x04006A56 RID: 27222
	public const int cLayerNoCollision = 14;

	// Token: 0x04006A57 RID: 27223
	public const int cLayerCCPhysics = 15;

	// Token: 0x04006A58 RID: 27224
	public const int cLayerTerrainCollision = 16;

	// Token: 0x04006A59 RID: 27225
	public const int cLayerPhysicsDead = 17;

	// Token: 0x04006A5A RID: 27226
	public const int cLayerGrass = 18;

	// Token: 0x04006A5B RID: 27227
	public const int cLayerLargeEntityBlocker = 19;

	// Token: 0x04006A5C RID: 27228
	public const int cLayerLocalCCPhysics = 20;

	// Token: 0x04006A5D RID: 27229
	public const int cLayerPhysics = 21;

	// Token: 0x04006A5E RID: 27230
	public const int cLayerUnderwaterEffects = 22;

	// Token: 0x04006A5F RID: 27231
	public const int cLayerTrees = 23;

	// Token: 0x04006A60 RID: 27232
	public const int cLayerLocalPlayer = 24;

	// Token: 0x04006A61 RID: 27233
	public const int cLayerPlayerRagdollsOLD = 27;

	// Token: 0x04006A62 RID: 27234
	public const int cLayerTerrain = 28;

	// Token: 0x04006A63 RID: 27235
	public const int cLayerWires = 29;

	// Token: 0x04006A64 RID: 27236
	public const int cLayerGlass = 30;

	// Token: 0x04006A65 RID: 27237
	public const int cLayerVolumes = 31;

	// Token: 0x04006A66 RID: 27238
	public const int cLayerMaskItems = 8192;

	// Token: 0x04006A67 RID: 27239
	public const int cLayerMaskIgnoreRayCast = 538480652;

	// Token: 0x04006A68 RID: 27240
	public const int cLayerMaskGrass = 262144;

	// Token: 0x04006A69 RID: 27241
	public const int cLayerMaskWater = 16;

	// Token: 0x04006A6A RID: 27242
	public const int cLayerMaskLocalPlayer = 17825792;

	// Token: 0x04006A6B RID: 27243
	public const int cLayerMaskAllLayers = -538480653;

	// Token: 0x04006A6C RID: 27244
	public const int cLayerMaskNoItems = -538488845;

	// Token: 0x04006A6D RID: 27245
	public const int cLayerMaskOnlyItemsAndCollision = 73728;

	// Token: 0x04006A6E RID: 27246
	public const int cLayerMaskNoItemsNoGrass = -538750989;

	// Token: 0x04006A6F RID: 27247
	public const int cLayerMaskNoItemsNoGrassNoWater = -538751005;

	// Token: 0x04006A70 RID: 27248
	public const int cLayerMaskNoItemsNoLocalPlayer = -555266061;

	// Token: 0x04006A71 RID: 27249
	public const int cLayerMaskAttackingBlocksMask = 1073807360;

	// Token: 0x04006A72 RID: 27250
	public const int cLayerMaskSight = -1612492829;

	// Token: 0x04006A73 RID: 27251
	public const int cLayerMaskTrajectory = 1082195968;

	// Token: 0x04006A74 RID: 27252
	public static int cDistanceRandomDisplayUpdates = 30;

	// Token: 0x04006A75 RID: 27253
	public static float cRunningFOVMultiplier = 1.05f;

	// Token: 0x04006A76 RID: 27254
	public static float cRunningFOVSpeedDown = 3f;

	// Token: 0x04006A77 RID: 27255
	public static float cRunningFOVSpeedUp = 1f;

	// Token: 0x04006A78 RID: 27256
	public static int cDefaultCameraFieldOfView = 65;

	// Token: 0x04006A79 RID: 27257
	public static int cMinCameraFieldOfView = 50;

	// Token: 0x04006A7A RID: 27258
	public static int cMaxCameraFieldOfView = 85;

	// Token: 0x04006A7B RID: 27259
	public static readonly Vector3 cDefaultCameraPlayerOffset = new Vector3(0f, 1.6f, 0f);

	// Token: 0x04006A7C RID: 27260
	public const int cMaxVertices = 786432;

	// Token: 0x04006A7D RID: 27261
	public static float cMinGlobalBackgroundOpacity = 0.55f;

	// Token: 0x04006A7E RID: 27262
	public static float cMinGlobalForegroundOpacity = 0.75f;

	// Token: 0x04006A7F RID: 27263
	public const int cTicksPerSecond = 20;

	// Token: 0x04006A80 RID: 27264
	public const float cTickDuration = 0.05f;

	// Token: 0x04006A81 RID: 27265
	public const float cPhysicsTicksPerSecond = 50f;

	// Token: 0x04006A82 RID: 27266
	public const float cPhysicsTickDuration = 0.02f;

	// Token: 0x04006A83 RID: 27267
	public static float cDefaultDistortionFactor = 1f;

	// Token: 0x04006A84 RID: 27268
	public static int cMaxEntitiesPerMobSpawner = 8;

	// Token: 0x04006A85 RID: 27269
	public const byte cMaxLightValue = 15;

	// Token: 0x04006A86 RID: 27270
	public static float cSizePlanesAround = 250f;

	// Token: 0x04006A87 RID: 27271
	public static int cDefaultPort = 26900;

	// Token: 0x04006A88 RID: 27272
	public static int cLevelServerPort = 6789;

	// Token: 0x04006A89 RID: 27273
	public static int cRandomSpawnPointsToPlace = 4;

	// Token: 0x04006A8A RID: 27274
	public static int cStartTeamTickets = 0;

	// Token: 0x04006A8B RID: 27275
	public static int cTeamTicketsAlarm = 10;

	// Token: 0x04006A8C RID: 27276
	public static float cDecreaseOneTicketTime = 5f;

	// Token: 0x04006A8D RID: 27277
	public static float cTimeGameOverButWaitSeconds = 5f;

	// Token: 0x04006A8E RID: 27278
	public static float cDigAndBuildDistance = 4f;

	// Token: 0x04006A8F RID: 27279
	public static float cCollectItemDistance = 2f;

	// Token: 0x04006A90 RID: 27280
	public static float cPlayerInteractDistance = 5f;

	// Token: 0x04006A91 RID: 27281
	public static float cRespawnAfterDeathTime = 3f;

	// Token: 0x04006A92 RID: 27282
	public static float cRespawnEnterGameTime = 0f;

	// Token: 0x04006A93 RID: 27283
	public static float cRespawnAfterFallenDown = 3f;

	// Token: 0x04006A94 RID: 27284
	public static float cHitColorDuration = 0.15f;

	// Token: 0x04006A95 RID: 27285
	public static float cItemDroppedOnDeathLifetime = 300f;

	// Token: 0x04006A96 RID: 27286
	public const float cItemDroppedLifetime = 60f;

	// Token: 0x04006A97 RID: 27287
	public static float cItemExplosionLifetime = 30f;

	// Token: 0x04006A98 RID: 27288
	public static float cItemHealthDroppedLifetime = 60f;

	// Token: 0x04006A99 RID: 27289
	public static float cItemSpawnPointLifetime = float.MaxValue;

	// Token: 0x04006A9A RID: 27290
	public static float cItemPortalLifetime = float.MaxValue;

	// Token: 0x04006A9B RID: 27291
	public static float cItemItemSpawnerLifetime = float.MaxValue;

	// Token: 0x04006A9C RID: 27292
	public static int cHardenBrickTime = 20;

	// Token: 0x04006A9D RID: 27293
	public const int cItemQualityTierVariations = 1;

	// Token: 0x04006A9E RID: 27294
	public const ushort cItemMaxQuality = 6;

	// Token: 0x04006A9F RID: 27295
	public static float cMinHolsterTime = 0.1f;

	// Token: 0x04006AA0 RID: 27296
	public static float cMinUnHolsterTime = 0.1f;

	// Token: 0x04006AA1 RID: 27297
	public static float cEnergyJetpackPerBlock = 10f;

	// Token: 0x04006AA2 RID: 27298
	public static int cHealthPotionAdd = 30;

	// Token: 0x04006AA3 RID: 27299
	public static int cMaxPlayerFood = 100;

	// Token: 0x04006AA4 RID: 27300
	public static int cFoodOversaturate = 100;

	// Token: 0x04006AA5 RID: 27301
	public static int cMaxPlayerDrink = 100;

	// Token: 0x04006AA6 RID: 27302
	public static int cDrinkOversaturate = 100;

	// Token: 0x04006AA7 RID: 27303
	public static int cItemDropCountWhenDead = 3;

	// Token: 0x04006AA8 RID: 27304
	public static float cBuildIntervall = 0.5f;

	// Token: 0x04006AA9 RID: 27305
	public static float cSendWorldTickTimeToClients = 1.5f;

	// Token: 0x04006AAA RID: 27306
	public static float cCheckGameState = 0.5f;

	// Token: 0x04006AAB RID: 27307
	public static float cSneakDamageMultiplier = 2f;

	// Token: 0x04006AAC RID: 27308
	public static int cNumberOfTeams = 2;

	// Token: 0x04006AAD RID: 27309
	public static Color[] cTeamColors = new Color[]
	{
		Color.white,
		new Color(0f, 0.8f, 1f),
		Color.red
	};

	// Token: 0x04006AAE RID: 27310
	public static string[] cTeamName = new string[] { "No", "BLUE", "RED" };

	// Token: 0x04006AAF RID: 27311
	public static string[] cTeamSkinName = new string[] { "Soldier Blue", "Soldier Blue", "Soldier Red" };

	// Token: 0x04006AB0 RID: 27312
	public static float cDarkAtNightSubtraction = 12f;

	// Token: 0x04006AB1 RID: 27313
	public static float cDefaultMonsterSeeDistance = 48f;

	// Token: 0x04006AB2 RID: 27314
	public static float cPlayerSpeedModifierRunning = 1.6f;

	// Token: 0x04006AB3 RID: 27315
	public static float cPlayerSpeedModifierWalking = 0.8f;

	// Token: 0x04006AB4 RID: 27316
	public static float cPlayerSpeedModifierCrouching = 0.4f;

	// Token: 0x04006AB5 RID: 27317
	public static int cPosInventoryYSub = 70;

	// Token: 0x04006AB6 RID: 27318
	public static int cPosMinimapY = 10;

	// Token: 0x04006AB7 RID: 27319
	public static int cPosMinimapSubRight = 20;

	// Token: 0x04006AB8 RID: 27320
	public static Color cColorBlood = new Color(0.8f, 0f, 0.08f);

	// Token: 0x04006AB9 RID: 27321
	public static Color cColorBorderBox = new Color(0.8f, 0f, 0f, 0.5f);

	// Token: 0x04006ABA RID: 27322
	public static Vector3 cStartPositionPlayerInLevel = new Vector3(0f, 200f, 0f);

	// Token: 0x04006ABB RID: 27323
	public static Vector3 cStartRotationPlayerInLevel = new Vector3(0f, 0f, 0f);

	// Token: 0x04006ABC RID: 27324
	public static BlockValue cTerrainBlockValue = new BlockValue(1U);

	// Token: 0x04006ABD RID: 27325
	public static string cTerrainFillerBlockName = "terrainFiller";

	// Token: 0x04006ABE RID: 27326
	public static string cTerrainFiller2BlockName = "terrainFillerAdaptive";

	// Token: 0x04006ABF RID: 27327
	public static string cPOIFillerBlock = "poiFillerBlock";

	// Token: 0x04006AC0 RID: 27328
	public static string cQuestLootFetchContainerIndexName = "FetchContainer";

	// Token: 0x04006AC1 RID: 27329
	public static string cQuestRestorePowerIndexName = "QuestRestorePower";

	// Token: 0x04006AC2 RID: 27330
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

	// Token: 0x04006AC3 RID: 27331
	public const int cMaxViewDistanceOptions = 7;

	// Token: 0x04006AC4 RID: 27332
	public const int cMinViewDistance = 4;

	// Token: 0x04006AC5 RID: 27333
	public const int cMaxViewDistance = 12;

	// Token: 0x04006AC6 RID: 27334
	public const float cBlockDamageLosesPaint = 1f;

	// Token: 0x04006AC7 RID: 27335
	public static int cEnemySenseMemory = 60;

	// Token: 0x04006AC8 RID: 27336
	public const int ChunkCompressionLevel = 3;

	// Token: 0x04006AC9 RID: 27337
	public const int NetworkCompressionLevel = 3;

	// Token: 0x04006ACA RID: 27338
	public const string cSpecialWorldName_Empty = "Empty";

	// Token: 0x04006ACB RID: 27339
	public const string cSpecialWorldName_Playtesting = "Playtesting";

	// Token: 0x04006ACC RID: 27340
	public const string cSpecialWorldName_Navezgane = "Navezgane";

	// Token: 0x04006ACD RID: 27341
	public const float cMouseSensitivityMin = 0.01f;

	// Token: 0x04006ACE RID: 27342
	public const float cMouseSensitivityMax = 1.5f;

	// Token: 0x04006ACF RID: 27343
	public const float cControllerSensitivityMin = 0.05f;

	// Token: 0x04006AD0 RID: 27344
	public const float cControllerSensitivityMax = 1f;

	// Token: 0x04006AD1 RID: 27345
	public const float cControllerModifierSensitivityMax = 2f;

	// Token: 0x04006AD2 RID: 27346
	public const int cPlayTestingSpawnOffset = 10;

	// Token: 0x04006AD3 RID: 27347
	public const float cAimAssistMaxDistance = 50f;

	// Token: 0x04006AD4 RID: 27348
	public const float cAimAssistSlowDownEntity = 0.5f;

	// Token: 0x04006AD5 RID: 27349
	public const float cAimAssistSlowDownItem = 0.6f;

	// Token: 0x04006AD6 RID: 27350
	public const float cAimAssistSlowDownItemDistance = 10f;

	// Token: 0x04006AD7 RID: 27351
	public const float cAimAssistSlowThreatLevelThreshold = 0.75f;

	// Token: 0x04006AD8 RID: 27352
	public const float cAimAssistSnapScreenDistance = 0.15f;

	// Token: 0x04006AD9 RID: 27353
	public const float cCameraSnapTime = 0.3f;

	// Token: 0x04006ADA RID: 27354
	public const float cAimAssistSnapMaximumAngle = 15f;

	// Token: 0x04006ADB RID: 27355
	public const float cAimAssistZoomSnapSpeed = 1f;

	// Token: 0x04006ADC RID: 27356
	public const float cAimAssistMeleeSnapAngle = 20f;

	// Token: 0x04006ADD RID: 27357
	public const float cAimAssistMeleeSnapSpeed = 1.5f;

	// Token: 0x04006ADE RID: 27358
	public const float cAimAssistMeleeHitSnapAngle = 30f;

	// Token: 0x04006ADF RID: 27359
	public const float cRapidTriggerFireDelay = 0.25f;

	// Token: 0x04006AE0 RID: 27360
	public const float cMapViewControllerSpeed = 500f;

	// Token: 0x04006AE1 RID: 27361
	public const float cRunToggleHoldTime = 0.2f;

	// Token: 0x04006AE2 RID: 27362
	public const float cRecoveryPositionAttemptTime = 30f;

	// Token: 0x04006AE3 RID: 27363
	public const float cRecoveryPositionMinSqrMagnitude = 10000f;

	// Token: 0x04006AE4 RID: 27364
	public const int cMaxRecoveryPositions = 5;

	// Token: 0x04006AE5 RID: 27365
	public const float cCursorSensitivityMin = 0.1f;

	// Token: 0x04006AE6 RID: 27366
	public const float cCursorSensitivityMax = 1f;

	// Token: 0x04006AE7 RID: 27367
	public const int cNavWorldSizeX = 6144;

	// Token: 0x04006AE8 RID: 27368
	public const int cNavWorldSizeZ = 6144;

	// Token: 0x04006AE9 RID: 27369
	public const float cPartyActivationRange = 15f;

	// Token: 0x04006AEA RID: 27370
	public const int cMaxPartySize = 8;

	// Token: 0x04006AEB RID: 27371
	public const int cDummyWaterTexId = 5000;

	// Token: 0x04006AEC RID: 27372
	public static int cMaxLoadTimePixelsPerTest = 4096;

	// Token: 0x04006AED RID: 27373
	public static int cMaxLoadTimePerFrameMillis = 50;

	// Token: 0x04006AEE RID: 27374
	public const float cInputRepeatDelay = 0.1f;

	// Token: 0x04006AEF RID: 27375
	public const float cInputInitialRepeatDelay = 0.35f;

	// Token: 0x04006AF0 RID: 27376
	public const int cConsoleMaxPersistentPlayerDataEntries = 100;

	// Token: 0x04006AF1 RID: 27377
	public const int cVirtualKeyboardDefaultCharacterLimit = 200;

	// Token: 0x020011B6 RID: 4534
	public enum EBiomePoiMap : byte
	{
		// Token: 0x04006AF3 RID: 27379
		CityAsphalt = 1,
		// Token: 0x04006AF4 RID: 27380
		CountryRoadAsphalt,
		// Token: 0x04006AF5 RID: 27381
		RoadGravel,
		// Token: 0x04006AF6 RID: 27382
		Sand,
		// Token: 0x04006AF7 RID: 27383
		Free
	}
}
