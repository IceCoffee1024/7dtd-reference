using System;

// Token: 0x02001219 RID: 4633
public enum EnumGameStats
{
	// Token: 0x04006E7D RID: 28285
	GameState,
	// Token: 0x04006E7E RID: 28286
	GameModeId,
	// Token: 0x04006E7F RID: 28287
	TimeLimitActive,
	// Token: 0x04006E80 RID: 28288
	TimeLimitThisRound,
	// Token: 0x04006E81 RID: 28289
	DayLimitActive,
	// Token: 0x04006E82 RID: 28290
	DayLimitThisRound,
	// Token: 0x04006E83 RID: 28291
	FragLimitActive,
	// Token: 0x04006E84 RID: 28292
	FragLimitThisRound,
	// Token: 0x04006E85 RID: 28293
	ShowWindow,
	// Token: 0x04006E86 RID: 28294
	LoadScene,
	// Token: 0x04006E87 RID: 28295
	CurrentRoundIx,
	// Token: 0x04006E88 RID: 28296
	TimeOfDayIncPerSec,
	// Token: 0x04006E89 RID: 28297
	EnemyCount,
	// Token: 0x04006E8A RID: 28298
	AnimalCount,
	// Token: 0x04006E8B RID: 28299
	ShowAllPlayersOnMap,
	// Token: 0x04006E8C RID: 28300
	ShowFriendPlayerOnMap,
	// Token: 0x04006E8D RID: 28301
	ShowSpawnWindow,
	// Token: 0x04006E8E RID: 28302
	UNUSED_ShowZombieCounter,
	// Token: 0x04006E8F RID: 28303
	IsCreativeMenuEnabled,
	// Token: 0x04006E90 RID: 28304
	IsTeleportEnabled,
	// Token: 0x04006E91 RID: 28305
	IsFlyingEnabled,
	// Token: 0x04006E92 RID: 28306
	IsPlayerDamageEnabled,
	// Token: 0x04006E93 RID: 28307
	IsPlayerCollisionEnabled,
	// Token: 0x04006E94 RID: 28308
	PlayerKillingMode,
	// Token: 0x04006E95 RID: 28309
	IsSpawnEnemies,
	// Token: 0x04006E96 RID: 28310
	IsSpawnNearOtherPlayer,
	// Token: 0x04006E97 RID: 28311
	IsSaveSupplyCrates,
	// Token: 0x04006E98 RID: 28312
	IsResetMapOnRestart,
	// Token: 0x04006E99 RID: 28313
	ScoreZombieKillMultiplier,
	// Token: 0x04006E9A RID: 28314
	ScorePlayerKillMultiplier,
	// Token: 0x04006E9B RID: 28315
	ScoreDiedMultiplier,
	// Token: 0x04006E9C RID: 28316
	IsVersionCheckDone,
	// Token: 0x04006E9D RID: 28317
	ZombieHordeMeter,
	// Token: 0x04006E9E RID: 28318
	DropOnDeath,
	// Token: 0x04006E9F RID: 28319
	DropOnQuit,
	// Token: 0x04006EA0 RID: 28320
	DeathPenalty,
	// Token: 0x04006EA1 RID: 28321
	LootTimer,
	// Token: 0x04006EA2 RID: 28322
	GameDifficulty,
	// Token: 0x04006EA3 RID: 28323
	UNUSED_GameDifficultyBonus,
	// Token: 0x04006EA4 RID: 28324
	BloodMoonEnemyCount,
	// Token: 0x04006EA5 RID: 28325
	EnemySpawnMode,
	// Token: 0x04006EA6 RID: 28326
	EnemyDifficulty,
	// Token: 0x04006EA7 RID: 28327
	DayLightLength,
	// Token: 0x04006EA8 RID: 28328
	LandClaimCount,
	// Token: 0x04006EA9 RID: 28329
	LandClaimSize,
	// Token: 0x04006EAA RID: 28330
	LandClaimDeadZone,
	// Token: 0x04006EAB RID: 28331
	LandClaimExpiryTime,
	// Token: 0x04006EAC RID: 28332
	LandClaimDecayMode,
	// Token: 0x04006EAD RID: 28333
	LandClaimOnlineDurabilityModifier,
	// Token: 0x04006EAE RID: 28334
	LandClaimOfflineDurabilityModifier,
	// Token: 0x04006EAF RID: 28335
	LandClaimOfflineDelay,
	// Token: 0x04006EB0 RID: 28336
	AirDropFrequency,
	// Token: 0x04006EB1 RID: 28337
	GlobalMessageToShow,
	// Token: 0x04006EB2 RID: 28338
	AirDropMarker,
	// Token: 0x04006EB3 RID: 28339
	PartySharedKillRange,
	// Token: 0x04006EB4 RID: 28340
	ChunkStabilityEnabled,
	// Token: 0x04006EB5 RID: 28341
	AutoParty,
	// Token: 0x04006EB6 RID: 28342
	OptionsPOICulling,
	// Token: 0x04006EB7 RID: 28343
	BloodMoonDay,
	// Token: 0x04006EB8 RID: 28344
	BlockDamagePlayer,
	// Token: 0x04006EB9 RID: 28345
	XPMultiplier,
	// Token: 0x04006EBA RID: 28346
	BloodMoonWarning,
	// Token: 0x04006EBB RID: 28347
	AllowedViewDistance,
	// Token: 0x04006EBC RID: 28348
	BedrollExpiryTime,
	// Token: 0x04006EBD RID: 28349
	TwitchBloodMoonAllowed,
	// Token: 0x04006EBE RID: 28350
	QuestProgressionDailyLimit,
	// Token: 0x04006EBF RID: 28351
	BiomeProgression,
	// Token: 0x04006EC0 RID: 28352
	StormFreq,
	// Token: 0x04006EC1 RID: 28353
	CameraRestrictionMode,
	// Token: 0x04006EC2 RID: 28354
	JarRefund,
	// Token: 0x04006EC3 RID: 28355
	SandboxPreset,
	// Token: 0x04006EC4 RID: 28356
	SandboxCode,
	// Token: 0x04006EC5 RID: 28357
	DayNightLength,
	// Token: 0x04006EC6 RID: 28358
	BlockDamageAI,
	// Token: 0x04006EC7 RID: 28359
	BlockDamageAIBM,
	// Token: 0x04006EC8 RID: 28360
	LootAbundance,
	// Token: 0x04006EC9 RID: 28361
	LootRespawnDays,
	// Token: 0x04006ECA RID: 28362
	GlobalGSModifier,
	// Token: 0x04006ECB RID: 28363
	BiomeGSModifier,
	// Token: 0x04006ECC RID: 28364
	GlobalLSModifier,
	// Token: 0x04006ECD RID: 28365
	BiomeLSModifier,
	// Token: 0x04006ECE RID: 28366
	Last
}
