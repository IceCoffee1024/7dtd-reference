using System;
using System.Text;
using UnityEngine.Scripting;

// Token: 0x020011FC RID: 4604
[Preserve]
public class GameModeZombieHorde : GameModeAbstract
{
	// Token: 0x060092F3 RID: 37619 RVA: 0x00376EF3 File Offset: 0x003750F3
	public override string GetName()
	{
		return "gmZombieHorde";
	}

	// Token: 0x060092F4 RID: 37620 RVA: 0x00376EFA File Offset: 0x003750FA
	public override string GetDescription()
	{
		return "gmZombieHordeDesc";
	}

	// Token: 0x060092F5 RID: 37621 RVA: 0x000809D4 File Offset: 0x0007EBD4
	public override int GetID()
	{
		return 4;
	}

	// Token: 0x060092F6 RID: 37622 RVA: 0x00376F04 File Offset: 0x00375104
	public override GameMode.ModeGamePref[] GetSupportedGamePrefsInfo()
	{
		return new GameMode.ModeGamePref[]
		{
			new GameMode.ModeGamePref(EnumGamePrefs.GameDifficulty, GamePrefs.EnumType.Int, 1, null),
			new GameMode.ModeGamePref(EnumGamePrefs.DayCount, GamePrefs.EnumType.Int, 9, null),
			new GameMode.ModeGamePref(EnumGamePrefs.DayNightLength, GamePrefs.EnumType.Int, 8, null),
			new GameMode.ModeGamePref(EnumGamePrefs.PlayerKillingMode, GamePrefs.EnumType.Int, EnumPlayerKillingMode.KillStrangersOnly, null),
			new GameMode.ModeGamePref(EnumGamePrefs.ShowFriendPlayerOnMap, GamePrefs.EnumType.Bool, false, null),
			new GameMode.ModeGamePref(EnumGamePrefs.LootAbundance, GamePrefs.EnumType.Int, 100, null),
			new GameMode.ModeGamePref(EnumGamePrefs.LootRespawnDays, GamePrefs.EnumType.Int, 7, null),
			new GameMode.ModeGamePref(EnumGamePrefs.DropOnDeath, GamePrefs.EnumType.Int, 1, null),
			new GameMode.ModeGamePref(EnumGamePrefs.DropOnQuit, GamePrefs.EnumType.Int, 1, null),
			new GameMode.ModeGamePref(EnumGamePrefs.BloodMoonEnemyCount, GamePrefs.EnumType.Int, 8, null),
			new GameMode.ModeGamePref(EnumGamePrefs.EnemySpawnMode, GamePrefs.EnumType.Bool, true, null),
			new GameMode.ModeGamePref(EnumGamePrefs.BuildCreate, GamePrefs.EnumType.Bool, false, null),
			new GameMode.ModeGamePref(EnumGamePrefs.RebuildMap, GamePrefs.EnumType.Bool, true, null),
			new GameMode.ModeGamePref(EnumGamePrefs.ServerIsPublic, GamePrefs.EnumType.Bool, false, null),
			new GameMode.ModeGamePref(EnumGamePrefs.ServerMaxPlayerCount, GamePrefs.EnumType.Int, 4, null),
			new GameMode.ModeGamePref(EnumGamePrefs.ServerPassword, GamePrefs.EnumType.String, "", null),
			new GameMode.ModeGamePref(EnumGamePrefs.ServerPort, GamePrefs.EnumType.Int, Constants.cDefaultPort, null),
			new GameMode.ModeGamePref(EnumGamePrefs.MaxChunkAge, GamePrefs.EnumType.Int, -1, null)
		};
	}

	// Token: 0x060092F7 RID: 37623 RVA: 0x000809D4 File Offset: 0x0007EBD4
	public override int GetRoundCount()
	{
		return 4;
	}

	// Token: 0x060092F8 RID: 37624 RVA: 0x003770B8 File Offset: 0x003752B8
	public override void Init()
	{
		base.Init();
		GameStats.Set(EnumGameStats.TimeLimitActive, false);
		GameStats.Set(EnumGameStats.ShowWindow, "");
		GameStats.Set(EnumGameStats.ShowSpawnWindow, true);
		GameStats.Set(EnumGameStats.IsSpawnNearOtherPlayer, false);
		GameStats.Set(EnumGameStats.IsSpawnEnemies, GamePrefs.GetInt(EnumGamePrefs.EnemySpawnMode) > 0);
		GameStats.Set(EnumGameStats.IsResetMapOnRestart, GamePrefs.GetBool(EnumGamePrefs.RebuildMap));
		GameStats.Set(EnumGameStats.ScorePlayerKillMultiplier, 1);
		GameStats.Set(EnumGameStats.ScoreZombieKillMultiplier, 3);
		GameStats.Set(EnumGameStats.ScoreDiedMultiplier, -5);
		GamePrefs.Set(EnumGamePrefs.DynamicSpawner, "SpawnHordeMode");
		GameStats.Set(EnumGameStats.DropOnDeath, GamePrefs.GetInt(EnumGamePrefs.DropOnDeath));
		GameStats.Set(EnumGameStats.DropOnQuit, GamePrefs.GetInt(EnumGamePrefs.DropOnQuit));
		GameStats.Set(EnumGameStats.BloodMoonEnemyCount, GamePrefs.GetInt(EnumGamePrefs.BloodMoonEnemyCount));
		GameStats.Set(EnumGameStats.EnemySpawnMode, GamePrefs.GetBool(EnumGamePrefs.EnemySpawnMode));
	}

	// Token: 0x060092F9 RID: 37625 RVA: 0x0037716C File Offset: 0x0037536C
	public override void StartRound(int _idx)
	{
		switch (_idx)
		{
		case 0:
			GameStats.Set(EnumGameStats.DayLimitThisRound, GamePrefs.GetInt(EnumGamePrefs.DayCount));
			GameStats.Set(EnumGameStats.DayLimitActive, GamePrefs.GetInt(EnumGamePrefs.DayCount) > 0);
			GameStats.Set(EnumGameStats.TimeLimitActive, false);
			GameStats.Set(EnumGameStats.FragLimitActive, false);
			GameStats.Set(EnumGameStats.GameState, 1);
			return;
		case 1:
			GameStats.Set(EnumGameStats.TimeLimitThisRound, 10);
			GameStats.Set(EnumGameStats.TimeLimitActive, true);
			GameStats.Set(EnumGameStats.DayLimitActive, false);
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

	// Token: 0x060092FA RID: 37626 RVA: 0x000027FC File Offset: 0x000009FC
	public override void EndRound(int _idx)
	{
	}

	// Token: 0x060092FB RID: 37627 RVA: 0x00377210 File Offset: 0x00375410
	public override string GetAdditionalGameInfo(World _world)
	{
		if (GameStats.GetBool(EnumGameStats.TimeLimitActive) || GameStats.GetBool(EnumGameStats.DayLimitActive) || GameStats.GetBool(EnumGameStats.FragLimitActive))
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (GameStats.GetBool(EnumGameStats.FragLimitActive))
			{
				stringBuilder.AppendFormat("  Frag limit: {0}", GameStats.GetInt(EnumGameStats.FragLimitThisRound));
			}
			if (GameStats.GetBool(EnumGameStats.TimeLimitActive))
			{
				int @int = GameStats.GetInt(EnumGameStats.TimeLimitThisRound);
				stringBuilder.AppendFormat("  Time limit: {0:D2}:{1:D2}", @int / 60, @int % 60);
			}
			if (GameStats.GetBool(EnumGameStats.DayLimitActive))
			{
				int int2 = GameStats.GetInt(EnumGameStats.DayLimitThisRound);
				ValueTuple<int, int, int> valueTuple = GameUtils.WorldTimeToElements(_world.worldTime);
				int item = valueTuple.Item1;
				int item2 = valueTuple.Item2;
				int item3 = valueTuple.Item3;
				int num = int2 - item;
				int num2 = ((num >= 0) ? (24 - item2) : 0);
				int num3 = ((num >= 0) ? (60 - item3) : 0);
				stringBuilder.AppendFormat("  Days to play: {0}:{1:D2}:{2:D2}", (num >= 0) ? num.ToString("D2") : "0", num2, num3);
			}
			return stringBuilder.ToString();
		}
		return string.Empty;
	}

	// Token: 0x04006C79 RID: 27769
	public static readonly string TypeName = typeof(GameModeZombieHorde).Name;
}
