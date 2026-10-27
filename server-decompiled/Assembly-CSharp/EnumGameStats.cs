using System;

// Token: 0x02001229 RID: 4649
public enum EnumGameStats
{
	// Token: 0x04006E95 RID: 28309
	GameState,
	// Token: 0x04006E96 RID: 28310
	GameModeId,
	// Token: 0x04006E97 RID: 28311
	TimeLimitActive,
	// Token: 0x04006E98 RID: 28312
	TimeLimitThisRound,
	// Token: 0x04006E99 RID: 28313
	DayLimitActive,
	// Token: 0x04006E9A RID: 28314
	DayLimitThisRound,
	// Token: 0x04006E9B RID: 28315
	FragLimitActive,
	// Token: 0x04006E9C RID: 28316
	FragLimitThisRound,
	// Token: 0x04006E9D RID: 28317
	ShowWindow,
	// Token: 0x04006E9E RID: 28318
	LoadScene,
	// Token: 0x04006E9F RID: 28319
	CurrentRoundIx,
	// Token: 0x04006EA0 RID: 28320
	TimeOfDayIncPerSec,
	// Token: 0x04006EA1 RID: 28321
	EnemyCount,
	// Token: 0x04006EA2 RID: 28322
	AnimalCount,
	// Token: 0x04006EA3 RID: 28323
	ShowAllPlayersOnMap,
	// Token: 0x04006EA4 RID: 28324
	ShowFriendPlayerOnMap,
	// Token: 0x04006EA5 RID: 28325
	ShowSpawnWindow,
	// Token: 0x04006EA6 RID: 28326
	UNUSED_ShowZombieCounter,
	// Token: 0x04006EA7 RID: 28327
	IsCreativeMenuEnabled,
	// Token: 0x04006EA8 RID: 28328
	IsTeleportEnabled,
	// Token: 0x04006EA9 RID: 28329
	IsFlyingEnabled,
	// Token: 0x04006EAA RID: 28330
	IsPlayerDamageEnabled,
	// Token: 0x04006EAB RID: 28331
	IsPlayerCollisionEnabled,
	// Token: 0x04006EAC RID: 28332
	PlayerKillingMode,
	// Token: 0x04006EAD RID: 28333
	IsSpawnEnemies,
	// Token: 0x04006EAE RID: 28334
	IsSpawnNearOtherPlayer,
	// Token: 0x04006EAF RID: 28335
	IsSaveSupplyCrates,
	// Token: 0x04006EB0 RID: 28336
	IsResetMapOnRestart,
	// Token: 0x04006EB1 RID: 28337
	ScoreZombieKillMultiplier,
	// Token: 0x04006EB2 RID: 28338
	ScorePlayerKillMultiplier,
	// Token: 0x04006EB3 RID: 28339
	ScoreDiedMultiplier,
	// Token: 0x04006EB4 RID: 28340
	IsVersionCheckDone,
	// Token: 0x04006EB5 RID: 28341
	ZombieHordeMeter,
	// Token: 0x04006EB6 RID: 28342
	DropOnDeath,
	// Token: 0x04006EB7 RID: 28343
	DropOnQuit,
	// Token: 0x04006EB8 RID: 28344
	DeathPenalty,
	// Token: 0x04006EB9 RID: 28345
	LootTimer,
	// Token: 0x04006EBA RID: 28346
	GameDifficulty,
	// Token: 0x04006EBB RID: 28347
	UNUSED_GameDifficultyBonus,
	// Token: 0x04006EBC RID: 28348
	BloodMoonEnemyCount,
	// Token: 0x04006EBD RID: 28349
	EnemySpawnMode,
	// Token: 0x04006EBE RID: 28350
	EnemyDifficulty,
	// Token: 0x04006EBF RID: 28351
	DayLightLength,
	// Token: 0x04006EC0 RID: 28352
	LandClaimCount,
	// Token: 0x04006EC1 RID: 28353
	LandClaimSize,
	// Token: 0x04006EC2 RID: 28354
	LandClaimDeadZone,
	// Token: 0x04006EC3 RID: 28355
	LandClaimExpiryTime,
	// Token: 0x04006EC4 RID: 28356
	LandClaimDecayMode,
	// Token: 0x04006EC5 RID: 28357
	LandClaimOnlineDurabilityModifier,
	// Token: 0x04006EC6 RID: 28358
	LandClaimOfflineDurabilityModifier,
	// Token: 0x04006EC7 RID: 28359
	LandClaimOfflineDelay,
	// Token: 0x04006EC8 RID: 28360
	AirDropFrequency,
	// Token: 0x04006EC9 RID: 28361
	GlobalMessageToShow,
	// Token: 0x04006ECA RID: 28362
	AirDropMarker,
	// Token: 0x04006ECB RID: 28363
	PartySharedKillRange,
	// Token: 0x04006ECC RID: 28364
	ChunkStabilityEnabled,
	// Token: 0x04006ECD RID: 28365
	AutoParty,
	// Token: 0x04006ECE RID: 28366
	OptionsPOICulling,
	// Token: 0x04006ECF RID: 28367
	BloodMoonDay,
	// Token: 0x04006ED0 RID: 28368
	BlockDamagePlayer,
	// Token: 0x04006ED1 RID: 28369
	XPMultiplier,
	// Token: 0x04006ED2 RID: 28370
	BloodMoonWarning,
	// Token: 0x04006ED3 RID: 28371
	AllowedViewDistance,
	// Token: 0x04006ED4 RID: 28372
	BedrollExpiryTime,
	// Token: 0x04006ED5 RID: 28373
	TwitchBloodMoonAllowed,
	// Token: 0x04006ED6 RID: 28374
	QuestProgressionDailyLimit,
	// Token: 0x04006ED7 RID: 28375
	BiomeProgression,
	// Token: 0x04006ED8 RID: 28376
	StormFreq,
	// Token: 0x04006ED9 RID: 28377
	CameraRestrictionMode,
	// Token: 0x04006EDA RID: 28378
	JarRefund,
	// Token: 0x04006EDB RID: 28379
	SandboxPreset,
	// Token: 0x04006EDC RID: 28380
	SandboxCode,
	// Token: 0x04006EDD RID: 28381
	DayNightLength,
	// Token: 0x04006EDE RID: 28382
	BlockDamageAI,
	// Token: 0x04006EDF RID: 28383
	BlockDamageAIBM,
	// Token: 0x04006EE0 RID: 28384
	LootAbundance,
	// Token: 0x04006EE1 RID: 28385
	LootRespawnDays,
	// Token: 0x04006EE2 RID: 28386
	GlobalGSModifier,
	// Token: 0x04006EE3 RID: 28387
	BiomeGSModifier,
	// Token: 0x04006EE4 RID: 28388
	GlobalLSModifier,
	// Token: 0x04006EE5 RID: 28389
	BiomeLSModifier,
	// Token: 0x04006EE6 RID: 28390
	Last
}
