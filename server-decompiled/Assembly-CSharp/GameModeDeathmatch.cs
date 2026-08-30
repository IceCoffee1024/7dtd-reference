using System;

// Token: 0x020011CE RID: 4558
public class GameModeDeathmatch : GameModeAbstract
{
	// Token: 0x060091D2 RID: 37330 RVA: 0x0036F0A0 File Offset: 0x0036D2A0
	public override string GetName()
	{
		return "gmDeathmatch";
	}

	// Token: 0x060091D3 RID: 37331 RVA: 0x0036F0A7 File Offset: 0x0036D2A7
	public override string GetDescription()
	{
		return "gmDeathmatchDesc";
	}

	// Token: 0x060091D4 RID: 37332 RVA: 0x00046EDA File Offset: 0x000450DA
	public override int GetID()
	{
		return 3;
	}

	// Token: 0x060091D5 RID: 37333 RVA: 0x0036F0B0 File Offset: 0x0036D2B0
	public override GameMode.ModeGamePref[] GetSupportedGamePrefsInfo()
	{
		return new GameMode.ModeGamePref[]
		{
			new GameMode.ModeGamePref(EnumGamePrefs.GameDifficulty, GamePrefs.EnumType.Int, 1, null),
			new GameMode.ModeGamePref(EnumGamePrefs.MatchLength, GamePrefs.EnumType.Int, 10, null),
			new GameMode.ModeGamePref(EnumGamePrefs.FragLimit, GamePrefs.EnumType.Int, 20, null),
			new GameMode.ModeGamePref(EnumGamePrefs.DayNightLength, GamePrefs.EnumType.Int, 10, null),
			new GameMode.ModeGamePref(EnumGamePrefs.DropOnDeath, GamePrefs.EnumType.Int, 1, null),
			new GameMode.ModeGamePref(EnumGamePrefs.DropOnQuit, GamePrefs.EnumType.Int, 1, null),
			new GameMode.ModeGamePref(EnumGamePrefs.BloodMoonEnemyCount, GamePrefs.EnumType.Int, 8, null),
			new GameMode.ModeGamePref(EnumGamePrefs.EnemySpawnMode, GamePrefs.EnumType.Bool, true, null),
			new GameMode.ModeGamePref(EnumGamePrefs.BuildCreate, GamePrefs.EnumType.Bool, false, null),
			new GameMode.ModeGamePref(EnumGamePrefs.RebuildMap, GamePrefs.EnumType.Bool, true, null),
			new GameMode.ModeGamePref(EnumGamePrefs.ServerIsPublic, GamePrefs.EnumType.Bool, true, null),
			new GameMode.ModeGamePref(EnumGamePrefs.ServerMaxPlayerCount, GamePrefs.EnumType.Int, 4, null),
			new GameMode.ModeGamePref(EnumGamePrefs.ServerPassword, GamePrefs.EnumType.String, "", null),
			new GameMode.ModeGamePref(EnumGamePrefs.ServerPort, GamePrefs.EnumType.Int, Constants.cDefaultPort, null)
		};
	}

	// Token: 0x060091D6 RID: 37334 RVA: 0x00080550 File Offset: 0x0007E750
	public override int GetRoundCount()
	{
		return 4;
	}

	// Token: 0x060091D7 RID: 37335 RVA: 0x0036F204 File Offset: 0x0036D404
	public override void Init()
	{
		base.Init();
		GameStats.Set(EnumGameStats.ShowSpawnWindow, true);
		GameStats.Set(EnumGameStats.TimeLimitActive, false);
		GameStats.Set(EnumGameStats.ShowWindow, "");
		GameStats.Set(EnumGameStats.IsResetMapOnRestart, GamePrefs.GetBool(EnumGamePrefs.RebuildMap));
		GameStats.Set(EnumGameStats.IsSpawnNearOtherPlayer, false);
		GameStats.Set(EnumGameStats.IsSpawnEnemies, GamePrefs.GetBool(EnumGamePrefs.EnemySpawnMode));
		GameStats.Set(EnumGameStats.PlayerKillingMode, 3);
		GameStats.Set(EnumGameStats.ScorePlayerKillMultiplier, 3);
		GameStats.Set(EnumGameStats.ScoreZombieKillMultiplier, 1);
		GameStats.Set(EnumGameStats.ScoreDiedMultiplier, -5);
		GamePrefs.Set(EnumGamePrefs.DynamicSpawner, "");
		GameStats.Set(EnumGameStats.DropOnDeath, GamePrefs.GetInt(EnumGamePrefs.DropOnDeath));
		GameStats.Set(EnumGameStats.DropOnQuit, GamePrefs.GetInt(EnumGamePrefs.DropOnQuit));
		GameStats.Set(EnumGameStats.BloodMoonEnemyCount, GamePrefs.GetInt(EnumGamePrefs.BloodMoonEnemyCount));
		GameStats.Set(EnumGameStats.EnemySpawnMode, GamePrefs.GetBool(EnumGamePrefs.EnemySpawnMode));
	}

	// Token: 0x060091D8 RID: 37336 RVA: 0x0036F2BC File Offset: 0x0036D4BC
	public override void StartRound(int _idx)
	{
		switch (_idx)
		{
		case 0:
			GameStats.Set(EnumGameStats.TimeLimitThisRound, GamePrefs.GetInt(EnumGamePrefs.MatchLength) * 60);
			GameStats.Set(EnumGameStats.TimeLimitActive, GamePrefs.GetInt(EnumGamePrefs.MatchLength) > 0);
			GameStats.Set(EnumGameStats.FragLimitThisRound, GamePrefs.GetInt(EnumGamePrefs.FragLimit));
			GameStats.Set(EnumGameStats.FragLimitActive, GamePrefs.GetInt(EnumGamePrefs.FragLimit) > 0);
			GameStats.Set(EnumGameStats.DayLimitActive, false);
			GameStats.Set(EnumGameStats.GameState, 1);
			return;
		case 1:
			GameStats.Set(EnumGameStats.TimeLimitThisRound, 10);
			GameStats.Set(EnumGameStats.TimeLimitActive, true);
			GameStats.Set(EnumGameStats.FragLimitActive, false);
			GameStats.Set(EnumGameStats.ShowWindow, null);
			GameStats.Set(EnumGameStats.GameState, 2);
			return;
		case 2:
			GameStats.Set(EnumGameStats.TimeLimitThisRound, 2);
			GameStats.Set(EnumGameStats.ShowWindow, XUiC_LoadingScreen.ID);
			return;
		case 3:
			GameStats.Set(EnumGameStats.TimeLimitActive, false);
			GameStats.Set(EnumGameStats.LoadScene, "SceneGame");
			return;
		default:
			return;
		}
	}

	// Token: 0x060091D9 RID: 37337 RVA: 0x000027FC File Offset: 0x000009FC
	public override void EndRound(int _idx)
	{
	}

	// Token: 0x04006BF0 RID: 27632
	public static readonly string TypeName = typeof(GameModeDeathmatch).Name;
}
