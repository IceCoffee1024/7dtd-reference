using System;

// Token: 0x020011E6 RID: 4582
public class GameModeDeathmatch : GameModeAbstract
{
	// Token: 0x06009260 RID: 37472 RVA: 0x003738E8 File Offset: 0x00371AE8
	public override string GetName()
	{
		return "gmDeathmatch";
	}

	// Token: 0x06009261 RID: 37473 RVA: 0x003738EF File Offset: 0x00371AEF
	public override string GetDescription()
	{
		return "gmDeathmatchDesc";
	}

	// Token: 0x06009262 RID: 37474 RVA: 0x000470B6 File Offset: 0x000452B6
	public override int GetID()
	{
		return 3;
	}

	// Token: 0x06009263 RID: 37475 RVA: 0x003738F8 File Offset: 0x00371AF8
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

	// Token: 0x06009264 RID: 37476 RVA: 0x00080804 File Offset: 0x0007EA04
	public override int GetRoundCount()
	{
		return 4;
	}

	// Token: 0x06009265 RID: 37477 RVA: 0x00373A4C File Offset: 0x00371C4C
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

	// Token: 0x06009266 RID: 37478 RVA: 0x00373B04 File Offset: 0x00371D04
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

	// Token: 0x06009267 RID: 37479 RVA: 0x000027FC File Offset: 0x000009FC
	public override void EndRound(int _idx)
	{
	}

	// Token: 0x04006C5A RID: 27738
	public static readonly string TypeName = typeof(GameModeDeathmatch).Name;
}
