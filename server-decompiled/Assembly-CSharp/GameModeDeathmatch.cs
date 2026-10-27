using System;

// Token: 0x020011F6 RID: 4598
public class GameModeDeathmatch : GameModeAbstract
{
	// Token: 0x060092B6 RID: 37558 RVA: 0x00375A5C File Offset: 0x00373C5C
	public override string GetName()
	{
		return "gmDeathmatch";
	}

	// Token: 0x060092B7 RID: 37559 RVA: 0x00375A63 File Offset: 0x00373C63
	public override string GetDescription()
	{
		return "gmDeathmatchDesc";
	}

	// Token: 0x060092B8 RID: 37560 RVA: 0x00047076 File Offset: 0x00045276
	public override int GetID()
	{
		return 3;
	}

	// Token: 0x060092B9 RID: 37561 RVA: 0x00375A6C File Offset: 0x00373C6C
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

	// Token: 0x060092BA RID: 37562 RVA: 0x000809D4 File Offset: 0x0007EBD4
	public override int GetRoundCount()
	{
		return 4;
	}

	// Token: 0x060092BB RID: 37563 RVA: 0x00375BC0 File Offset: 0x00373DC0
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

	// Token: 0x060092BC RID: 37564 RVA: 0x00375C78 File Offset: 0x00373E78
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

	// Token: 0x060092BD RID: 37565 RVA: 0x000027FC File Offset: 0x000009FC
	public override void EndRound(int _idx)
	{
	}

	// Token: 0x04006C72 RID: 27762
	public static readonly string TypeName = typeof(GameModeDeathmatch).Name;
}
