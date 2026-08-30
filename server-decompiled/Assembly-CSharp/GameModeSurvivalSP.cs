using System;
using UnityEngine.Scripting;

// Token: 0x020011B7 RID: 4535
[Preserve]
public class GameModeSurvivalSP : GameModeAbstract
{
	// Token: 0x06009101 RID: 37121 RVA: 0x0036B065 File Offset: 0x00369265
	public override string GetName()
	{
		return "gmSurvivalSP";
	}

	// Token: 0x06009102 RID: 37122 RVA: 0x0036B06C File Offset: 0x0036926C
	public override string GetDescription()
	{
		return "gmSurvivalSPDesc";
	}

	// Token: 0x06009103 RID: 37123 RVA: 0x00081148 File Offset: 0x0007F348
	public override int GetID()
	{
		return 6;
	}

	// Token: 0x06009104 RID: 37124 RVA: 0x0036B074 File Offset: 0x00369274
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

	// Token: 0x06009105 RID: 37125 RVA: 0x0036B1E8 File Offset: 0x003693E8
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

	// Token: 0x06009106 RID: 37126 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override int GetRoundCount()
	{
		return 1;
	}

	// Token: 0x06009107 RID: 37127 RVA: 0x00369D80 File Offset: 0x00367F80
	public override void StartRound(int _idx)
	{
		GameStats.Set(EnumGameStats.GameState, 1);
	}

	// Token: 0x06009108 RID: 37128 RVA: 0x000027FC File Offset: 0x000009FC
	public override void EndRound(int _idx)
	{
	}

	// Token: 0x04006B1E RID: 27422
	public static readonly string TypeName = typeof(GameModeSurvivalSP).Name;
}
