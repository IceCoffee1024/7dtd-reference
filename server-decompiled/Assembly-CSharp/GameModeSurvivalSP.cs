using System;
using UnityEngine.Scripting;

// Token: 0x020011EB RID: 4587
[Preserve]
public class GameModeSurvivalSP : GameModeAbstract
{
	// Token: 0x06009293 RID: 37523 RVA: 0x00374B49 File Offset: 0x00372D49
	public override string GetName()
	{
		return "gmSurvivalSP";
	}

	// Token: 0x06009294 RID: 37524 RVA: 0x00374B50 File Offset: 0x00372D50
	public override string GetDescription()
	{
		return "gmSurvivalSPDesc";
	}

	// Token: 0x06009295 RID: 37525 RVA: 0x00081AB0 File Offset: 0x0007FCB0
	public override int GetID()
	{
		return 6;
	}

	// Token: 0x06009296 RID: 37526 RVA: 0x00374B58 File Offset: 0x00372D58
	public override GameMode.ModeGamePref[] GetSupportedGamePrefsInfo()
	{
		return new GameMode.ModeGamePref[]
		{
			new GameMode.ModeGamePref(EnumGamePrefs.GameDifficulty, GamePrefs.EnumType.Int, 1, null),
			new GameMode.ModeGamePref(EnumGamePrefs.DayNightLength, GamePrefs.EnumType.Int, 60, null),
			new GameMode.ModeGamePref(EnumGamePrefs.DayLightLength, GamePrefs.EnumType.Int, 18, null),
			new GameMode.ModeGamePref(EnumGamePrefs.LootAbundance, GamePrefs.EnumType.Int, 100, null),
			new GameMode.ModeGamePref(EnumGamePrefs.LootRespawnDays, GamePrefs.EnumType.Int, 7, null),
			new GameMode.ModeGamePref(EnumGamePrefs.DropOnDeath, GamePrefs.EnumType.Int, 0, null),
			new GameMode.ModeGamePref(EnumGamePrefs.BloodMoonEnemyCount, GamePrefs.EnumType.Int, 8, null),
			new GameMode.ModeGamePref(EnumGamePrefs.EnemySpawnMode, GamePrefs.EnumType.Bool, true, null),
			new GameMode.ModeGamePref(EnumGamePrefs.EnemyDifficulty, GamePrefs.EnumType.Int, 0, null),
			new GameMode.ModeGamePref(EnumGamePrefs.AirDropFrequency, GamePrefs.EnumType.Int, 3, null),
			new GameMode.ModeGamePref(EnumGamePrefs.BuildCreate, GamePrefs.EnumType.Bool, false, null),
			new GameMode.ModeGamePref(EnumGamePrefs.PersistentPlayerProfiles, GamePrefs.EnumType.Bool, false, null),
			new GameMode.ModeGamePref(EnumGamePrefs.AirDropMarker, GamePrefs.EnumType.Bool, true, null),
			new GameMode.ModeGamePref(EnumGamePrefs.BedrollDeadZoneSize, GamePrefs.EnumType.Int, 15, null),
			new GameMode.ModeGamePref(EnumGamePrefs.MaxChunkAge, GamePrefs.EnumType.Int, -1, null)
		};
	}

	// Token: 0x06009297 RID: 37527 RVA: 0x00374CCC File Offset: 0x00372ECC
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
		GameStats.Set(EnumGameStats.DropOnQuit, 0);
		GamePrefs.Set(EnumGamePrefs.ServerMaxPlayerCount, 1);
		GamePrefs.Set(EnumGamePrefs.ServerIsPublic, false);
		GamePrefs.Set(EnumGamePrefs.ServerPort, Constants.cDefaultPort);
		GameStats.Set(EnumGameStats.IsFlyingEnabled, GamePrefs.GetBool(EnumGamePrefs.BuildCreate));
	}

	// Token: 0x06009298 RID: 37528 RVA: 0x00020119 File Offset: 0x0001E319
	public override int GetRoundCount()
	{
		return 1;
	}

	// Token: 0x06009299 RID: 37529 RVA: 0x00373864 File Offset: 0x00371A64
	public override void StartRound(int _idx)
	{
		GameStats.Set(EnumGameStats.GameState, 1);
	}

	// Token: 0x0600929A RID: 37530 RVA: 0x000027FC File Offset: 0x000009FC
	public override void EndRound(int _idx)
	{
	}

	// Token: 0x04006C60 RID: 27744
	public static readonly string TypeName = typeof(GameModeSurvivalSP).Name;
}
