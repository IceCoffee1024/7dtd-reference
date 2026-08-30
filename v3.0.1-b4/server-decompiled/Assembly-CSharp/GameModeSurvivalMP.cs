using System;

// Token: 0x020011B5 RID: 4533
public class GameModeSurvivalMP : GameModeAbstract
{
	// Token: 0x060090ED RID: 37101 RVA: 0x0036A921 File Offset: 0x00368B21
	public override string GetName()
	{
		return "gmSurvivalMP";
	}

	// Token: 0x060090EE RID: 37102 RVA: 0x0036A928 File Offset: 0x00368B28
	public override string GetDescription()
	{
		return "gmSurvivalMPDesc";
	}

	// Token: 0x060090EF RID: 37103 RVA: 0x0005FC95 File Offset: 0x0005DE95
	public override int GetID()
	{
		return 7;
	}

	// Token: 0x060090F0 RID: 37104 RVA: 0x0036A930 File Offset: 0x00368B30
	public override GameMode.ModeGamePref[] GetSupportedGamePrefsInfo()
	{
		return new GameMode.ModeGamePref[]
		{
			new GameMode.ModeGamePref(EnumGamePrefs.GameDifficulty, GamePrefs.EnumType.Int, 1, null),
			new GameMode.ModeGamePref(EnumGamePrefs.DayNightLength, GamePrefs.EnumType.Int, 60, null),
			new GameMode.ModeGamePref(EnumGamePrefs.DayLightLength, GamePrefs.EnumType.Int, 18, null),
			new GameMode.ModeGamePref(EnumGamePrefs.PlayerKillingMode, GamePrefs.EnumType.Int, EnumPlayerKillingMode.KillStrangersOnly, null),
			new GameMode.ModeGamePref(EnumGamePrefs.ShowFriendPlayerOnMap, GamePrefs.EnumType.Bool, true, null),
			new GameMode.ModeGamePref(EnumGamePrefs.LootAbundance, GamePrefs.EnumType.Int, 100, null),
			new GameMode.ModeGamePref(EnumGamePrefs.LootRespawnDays, GamePrefs.EnumType.Int, 7, null),
			new GameMode.ModeGamePref(EnumGamePrefs.DropOnDeath, GamePrefs.EnumType.Int, 0, null),
			new GameMode.ModeGamePref(EnumGamePrefs.DropOnQuit, GamePrefs.EnumType.Int, 0, null),
			new GameMode.ModeGamePref(EnumGamePrefs.BloodMoonEnemyCount, GamePrefs.EnumType.Int, 8, null),
			new GameMode.ModeGamePref(EnumGamePrefs.EnemySpawnMode, GamePrefs.EnumType.Bool, true, null),
			new GameMode.ModeGamePref(EnumGamePrefs.EnemyDifficulty, GamePrefs.EnumType.Int, 0, null),
			new GameMode.ModeGamePref(EnumGamePrefs.ServerEnabled, GamePrefs.EnumType.Bool, false, null),
			new GameMode.ModeGamePref(EnumGamePrefs.ServerIsPublic, GamePrefs.EnumType.Bool, true, null),
			new GameMode.ModeGamePref(EnumGamePrefs.ServerVisibility, GamePrefs.EnumType.Int, 2, null),
			new GameMode.ModeGamePref(EnumGamePrefs.ServerMaxPlayerCount, GamePrefs.EnumType.Int, 8, null),
			new GameMode.ModeGamePref(EnumGamePrefs.ServerPassword, GamePrefs.EnumType.String, "", null),
			new GameMode.ModeGamePref(EnumGamePrefs.ServerPort, GamePrefs.EnumType.Int, Constants.cDefaultPort, null),
			new GameMode.ModeGamePref(EnumGamePrefs.ServerEACPeerToPeer, GamePrefs.EnumType.Bool, true, null),
			new GameMode.ModeGamePref(EnumGamePrefs.LandClaimCount, GamePrefs.EnumType.Int, 5, null),
			new GameMode.ModeGamePref(EnumGamePrefs.LandClaimSize, GamePrefs.EnumType.Int, 41, null),
			new GameMode.ModeGamePref(EnumGamePrefs.LandClaimDeadZone, GamePrefs.EnumType.Int, 30, null),
			new GameMode.ModeGamePref(EnumGamePrefs.LandClaimExpiryTime, GamePrefs.EnumType.Int, 3, null),
			new GameMode.ModeGamePref(EnumGamePrefs.LandClaimDecayMode, GamePrefs.EnumType.Int, 0, null),
			new GameMode.ModeGamePref(EnumGamePrefs.LandClaimOnlineDurabilityModifier, GamePrefs.EnumType.Int, 4, null),
			new GameMode.ModeGamePref(EnumGamePrefs.LandClaimOfflineDurabilityModifier, GamePrefs.EnumType.Int, 4, null),
			new GameMode.ModeGamePref(EnumGamePrefs.LandClaimOfflineDelay, GamePrefs.EnumType.Int, 0, null),
			new GameMode.ModeGamePref(EnumGamePrefs.BedrollDeadZoneSize, GamePrefs.EnumType.Int, 15, null),
			new GameMode.ModeGamePref(EnumGamePrefs.BedrollExpiryTime, GamePrefs.EnumType.Int, 45, null),
			new GameMode.ModeGamePref(EnumGamePrefs.AirDropFrequency, GamePrefs.EnumType.Int, 3, null),
			new GameMode.ModeGamePref(EnumGamePrefs.BuildCreate, GamePrefs.EnumType.Bool, false, null),
			new GameMode.ModeGamePref(EnumGamePrefs.PersistentPlayerProfiles, GamePrefs.EnumType.Bool, false, null),
			new GameMode.ModeGamePref(EnumGamePrefs.AirDropMarker, GamePrefs.EnumType.Bool, true, null),
			new GameMode.ModeGamePref(EnumGamePrefs.PartySharedKillRange, GamePrefs.EnumType.Int, 100, null),
			new GameMode.ModeGamePref(EnumGamePrefs.QuestProgressionDailyLimit, GamePrefs.EnumType.Int, 4, null),
			new GameMode.ModeGamePref(EnumGamePrefs.MaxChunkAge, GamePrefs.EnumType.Int, -1, null),
			new GameMode.ModeGamePref(EnumGamePrefs.BiomeProgression, GamePrefs.EnumType.Bool, true, null)
		};
	}

	// Token: 0x060090F1 RID: 37105 RVA: 0x0036ACB4 File Offset: 0x00368EB4
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
		GameStats.Set(EnumGameStats.IsFlyingEnabled, GamePrefs.GetBool(EnumGamePrefs.BuildCreate));
	}

	// Token: 0x060090F2 RID: 37106 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override int GetRoundCount()
	{
		return 1;
	}

	// Token: 0x060090F3 RID: 37107 RVA: 0x00369D80 File Offset: 0x00367F80
	public override void StartRound(int _idx)
	{
		GameStats.Set(EnumGameStats.GameState, 1);
	}

	// Token: 0x060090F4 RID: 37108 RVA: 0x000027FC File Offset: 0x000009FC
	public override void EndRound(int _idx)
	{
	}

	// Token: 0x04006B1C RID: 27420
	public static readonly string TypeName = typeof(GameModeSurvivalMP).Name;
}
