using System;

// Token: 0x020011EA RID: 4586
public class GameModeSurvivalPvP : GameModeAbstract
{
	// Token: 0x06009289 RID: 37513 RVA: 0x00374827 File Offset: 0x00372A27
	public override string GetName()
	{
		return "gmSurvivalPvP";
	}

	// Token: 0x0600928A RID: 37514 RVA: 0x0037482E File Offset: 0x00372A2E
	public override string GetDescription()
	{
		return "gmSurvivalPvPDesc";
	}

	// Token: 0x0600928B RID: 37515 RVA: 0x000809CD File Offset: 0x0007EBCD
	public override int GetID()
	{
		return 5;
	}

	// Token: 0x0600928C RID: 37516 RVA: 0x00374838 File Offset: 0x00372A38
	public override GameMode.ModeGamePref[] GetSupportedGamePrefsInfo()
	{
		return new GameMode.ModeGamePref[]
		{
			new GameMode.ModeGamePref(EnumGamePrefs.GameDifficulty, GamePrefs.EnumType.Int, 1, null),
			new GameMode.ModeGamePref(EnumGamePrefs.DayNightLength, GamePrefs.EnumType.Int, 60, null),
			new GameMode.ModeGamePref(EnumGamePrefs.DayLightLength, GamePrefs.EnumType.Int, 18, null),
			new GameMode.ModeGamePref(EnumGamePrefs.PlayerKillingMode, GamePrefs.EnumType.Int, EnumPlayerKillingMode.KillEveryone, null),
			new GameMode.ModeGamePref(EnumGamePrefs.ShowFriendPlayerOnMap, GamePrefs.EnumType.Bool, false, null),
			new GameMode.ModeGamePref(EnumGamePrefs.LootAbundance, GamePrefs.EnumType.Int, 100, null),
			new GameMode.ModeGamePref(EnumGamePrefs.LootRespawnDays, GamePrefs.EnumType.Int, 7, null),
			new GameMode.ModeGamePref(EnumGamePrefs.DropOnDeath, GamePrefs.EnumType.Int, 1, null),
			new GameMode.ModeGamePref(EnumGamePrefs.DropOnQuit, GamePrefs.EnumType.Int, 0, null),
			new GameMode.ModeGamePref(EnumGamePrefs.BloodMoonEnemyCount, GamePrefs.EnumType.Int, 8, null),
			new GameMode.ModeGamePref(EnumGamePrefs.EnemySpawnMode, GamePrefs.EnumType.Bool, true, null),
			new GameMode.ModeGamePref(EnumGamePrefs.BuildCreate, GamePrefs.EnumType.Bool, false, null),
			new GameMode.ModeGamePref(EnumGamePrefs.ServerIsPublic, GamePrefs.EnumType.Bool, false, null),
			new GameMode.ModeGamePref(EnumGamePrefs.ServerMaxPlayerCount, GamePrefs.EnumType.Int, 4, null),
			new GameMode.ModeGamePref(EnumGamePrefs.ServerPassword, GamePrefs.EnumType.String, "", null),
			new GameMode.ModeGamePref(EnumGamePrefs.ServerPort, GamePrefs.EnumType.Int, Constants.cDefaultPort, null),
			new GameMode.ModeGamePref(EnumGamePrefs.LandClaimCount, GamePrefs.EnumType.Int, 5, null),
			new GameMode.ModeGamePref(EnumGamePrefs.LandClaimSize, GamePrefs.EnumType.Int, 41, null),
			new GameMode.ModeGamePref(EnumGamePrefs.LandClaimDeadZone, GamePrefs.EnumType.Int, 30, null),
			new GameMode.ModeGamePref(EnumGamePrefs.LandClaimExpiryTime, GamePrefs.EnumType.Int, 3, null),
			new GameMode.ModeGamePref(EnumGamePrefs.LandClaimDecayMode, GamePrefs.EnumType.Int, 0, null),
			new GameMode.ModeGamePref(EnumGamePrefs.LandClaimOnlineDurabilityModifier, GamePrefs.EnumType.Int, 32, null),
			new GameMode.ModeGamePref(EnumGamePrefs.LandClaimOfflineDurabilityModifier, GamePrefs.EnumType.Int, 32, null),
			new GameMode.ModeGamePref(EnumGamePrefs.LandClaimOfflineDelay, GamePrefs.EnumType.Int, 0, null),
			new GameMode.ModeGamePref(EnumGamePrefs.BedrollDeadZoneSize, GamePrefs.EnumType.Int, 15, null),
			new GameMode.ModeGamePref(EnumGamePrefs.BedrollExpiryTime, GamePrefs.EnumType.Int, 45, null),
			new GameMode.ModeGamePref(EnumGamePrefs.MaxChunkAge, GamePrefs.EnumType.Int, -1, null)
		};
	}

	// Token: 0x0600928D RID: 37517 RVA: 0x00374AC8 File Offset: 0x00372CC8
	public override void Init()
	{
		base.Init();
		GameStats.Set(EnumGameStats.ShowSpawnWindow, false);
		GameStats.Set(EnumGameStats.TimeLimitActive, false);
		GameStats.Set(EnumGameStats.DayLimitActive, false);
		GameStats.Set(EnumGameStats.ShowWindow, "");
		GameStats.Set(EnumGameStats.IsSpawnEnemies, GamePrefs.GetBool(EnumGamePrefs.EnemySpawnMode));
		GameStats.Set(EnumGameStats.ScorePlayerKillMultiplier, 0);
		GameStats.Set(EnumGameStats.ScoreZombieKillMultiplier, 1);
		GameStats.Set(EnumGameStats.ScoreDiedMultiplier, -5);
		GameStats.Set(EnumGameStats.IsSpawnNearOtherPlayer, false);
		GameStats.Set(EnumGameStats.ZombieHordeMeter, true);
	}

	// Token: 0x0600928E RID: 37518 RVA: 0x00020119 File Offset: 0x0001E319
	public override int GetRoundCount()
	{
		return 1;
	}

	// Token: 0x0600928F RID: 37519 RVA: 0x00373864 File Offset: 0x00371A64
	public override void StartRound(int _idx)
	{
		GameStats.Set(EnumGameStats.GameState, 1);
	}

	// Token: 0x06009290 RID: 37520 RVA: 0x000027FC File Offset: 0x000009FC
	public override void EndRound(int _idx)
	{
	}

	// Token: 0x04006C5F RID: 27743
	public static readonly string TypeName = typeof(GameModeSurvivalPvP).Name;
}
