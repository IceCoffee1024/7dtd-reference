using System;

// Token: 0x020011E5 RID: 4581
public enum EnumGameStats
{
	// Token: 0x04006D3A RID: 27962
	GameState,
	// Token: 0x04006D3B RID: 27963
	GameModeId,
	// Token: 0x04006D3C RID: 27964
	TimeLimitActive,
	// Token: 0x04006D3D RID: 27965
	TimeLimitThisRound,
	// Token: 0x04006D3E RID: 27966
	DayLimitActive,
	// Token: 0x04006D3F RID: 27967
	DayLimitThisRound,
	// Token: 0x04006D40 RID: 27968
	FragLimitActive,
	// Token: 0x04006D41 RID: 27969
	FragLimitThisRound,
	// Token: 0x04006D42 RID: 27970
	ShowWindow,
	// Token: 0x04006D43 RID: 27971
	LoadScene,
	// Token: 0x04006D44 RID: 27972
	CurrentRoundIx,
	// Token: 0x04006D45 RID: 27973
	TimeOfDayIncPerSec,
	// Token: 0x04006D46 RID: 27974
	EnemyCount,
	// Token: 0x04006D47 RID: 27975
	AnimalCount,
	// Token: 0x04006D48 RID: 27976
	ShowAllPlayersOnMap,
	// Token: 0x04006D49 RID: 27977
	ShowFriendPlayerOnMap,
	// Token: 0x04006D4A RID: 27978
	ShowSpawnWindow,
	// Token: 0x04006D4B RID: 27979
	UNUSED_ShowZombieCounter,
	// Token: 0x04006D4C RID: 27980
	IsCreativeMenuEnabled,
	// Token: 0x04006D4D RID: 27981
	IsTeleportEnabled,
	// Token: 0x04006D4E RID: 27982
	IsFlyingEnabled,
	// Token: 0x04006D4F RID: 27983
	IsPlayerDamageEnabled,
	// Token: 0x04006D50 RID: 27984
	IsPlayerCollisionEnabled,
	// Token: 0x04006D51 RID: 27985
	PlayerKillingMode,
	// Token: 0x04006D52 RID: 27986
	IsSpawnEnemies,
	// Token: 0x04006D53 RID: 27987
	IsSpawnNearOtherPlayer,
	// Token: 0x04006D54 RID: 27988
	IsSaveSupplyCrates,
	// Token: 0x04006D55 RID: 27989
	IsResetMapOnRestart,
	// Token: 0x04006D56 RID: 27990
	ScoreZombieKillMultiplier,
	// Token: 0x04006D57 RID: 27991
	ScorePlayerKillMultiplier,
	// Token: 0x04006D58 RID: 27992
	ScoreDiedMultiplier,
	// Token: 0x04006D59 RID: 27993
	IsVersionCheckDone,
	// Token: 0x04006D5A RID: 27994
	ZombieHordeMeter,
	// Token: 0x04006D5B RID: 27995
	DropOnDeath,
	// Token: 0x04006D5C RID: 27996
	DropOnQuit,
	// Token: 0x04006D5D RID: 27997
	DeathPenalty,
	// Token: 0x04006D5E RID: 27998
	LootTimer,
	// Token: 0x04006D5F RID: 27999
	GameDifficulty,
	// Token: 0x04006D60 RID: 28000
	UNUSED_GameDifficultyBonus,
	// Token: 0x04006D61 RID: 28001
	BloodMoonEnemyCount,
	// Token: 0x04006D62 RID: 28002
	EnemySpawnMode,
	// Token: 0x04006D63 RID: 28003
	EnemyDifficulty,
	// Token: 0x04006D64 RID: 28004
	DayLightLength,
	// Token: 0x04006D65 RID: 28005
	LandClaimCount,
	// Token: 0x04006D66 RID: 28006
	LandClaimSize,
	// Token: 0x04006D67 RID: 28007
	LandClaimDeadZone,
	// Token: 0x04006D68 RID: 28008
	LandClaimExpiryTime,
	// Token: 0x04006D69 RID: 28009
	LandClaimDecayMode,
	// Token: 0x04006D6A RID: 28010
	LandClaimOnlineDurabilityModifier,
	// Token: 0x04006D6B RID: 28011
	LandClaimOfflineDurabilityModifier,
	// Token: 0x04006D6C RID: 28012
	LandClaimOfflineDelay,
	// Token: 0x04006D6D RID: 28013
	AirDropFrequency,
	// Token: 0x04006D6E RID: 28014
	GlobalMessageToShow,
	// Token: 0x04006D6F RID: 28015
	AirDropMarker,
	// Token: 0x04006D70 RID: 28016
	PartySharedKillRange,
	// Token: 0x04006D71 RID: 28017
	ChunkStabilityEnabled,
	// Token: 0x04006D72 RID: 28018
	AutoParty,
	// Token: 0x04006D73 RID: 28019
	OptionsPOICulling,
	// Token: 0x04006D74 RID: 28020
	BloodMoonDay,
	// Token: 0x04006D75 RID: 28021
	BlockDamagePlayer,
	// Token: 0x04006D76 RID: 28022
	XPMultiplier,
	// Token: 0x04006D77 RID: 28023
	BloodMoonWarning,
	// Token: 0x04006D78 RID: 28024
	AllowedViewDistance,
	// Token: 0x04006D79 RID: 28025
	BedrollExpiryTime,
	// Token: 0x04006D7A RID: 28026
	TwitchBloodMoonAllowed,
	// Token: 0x04006D7B RID: 28027
	QuestProgressionDailyLimit,
	// Token: 0x04006D7C RID: 28028
	BiomeProgression,
	// Token: 0x04006D7D RID: 28029
	StormFreq,
	// Token: 0x04006D7E RID: 28030
	CameraRestrictionMode,
	// Token: 0x04006D7F RID: 28031
	JarRefund,
	// Token: 0x04006D80 RID: 28032
	SandboxPreset,
	// Token: 0x04006D81 RID: 28033
	SandboxCode,
	// Token: 0x04006D82 RID: 28034
	DayNightLength,
	// Token: 0x04006D83 RID: 28035
	BlockDamageAI,
	// Token: 0x04006D84 RID: 28036
	BlockDamageAIBM,
	// Token: 0x04006D85 RID: 28037
	LootAbundance,
	// Token: 0x04006D86 RID: 28038
	LootRespawnDays,
	// Token: 0x04006D87 RID: 28039
	GlobalGSModifier,
	// Token: 0x04006D88 RID: 28040
	BiomeGSModifier,
	// Token: 0x04006D89 RID: 28041
	GlobalLSModifier,
	// Token: 0x04006D8A RID: 28042
	BiomeLSModifier,
	// Token: 0x04006D8B RID: 28043
	Last
}
