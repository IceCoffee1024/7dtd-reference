using System;
using UnityEngine.Scripting;

// Token: 0x020011CD RID: 4557
[Preserve]
public class GameModeCreative : GameModeAbstract
{
	// Token: 0x060091C7 RID: 37319 RVA: 0x0036EF0E File Offset: 0x0036D10E
	public override string GetName()
	{
		return "gmCreative";
	}

	// Token: 0x060091C8 RID: 37320 RVA: 0x0036EF15 File Offset: 0x0036D115
	public override string GetDescription()
	{
		return "gmCreativeDesc";
	}

	// Token: 0x060091C9 RID: 37321 RVA: 0x0002F168 File Offset: 0x0002D368
	public override int GetID()
	{
		return 2;
	}

	// Token: 0x060091CA RID: 37322 RVA: 0x0036EF1C File Offset: 0x0036D11C
	public override GameMode.ModeGamePref[] GetSupportedGamePrefsInfo()
	{
		return new GameMode.ModeGamePref[]
		{
			new GameMode.ModeGamePref(EnumGamePrefs.ServerIsPublic, GamePrefs.EnumType.Bool, false, null),
			new GameMode.ModeGamePref(EnumGamePrefs.DayNightLength, GamePrefs.EnumType.Int, 60, null),
			new GameMode.ModeGamePref(EnumGamePrefs.DayLightLength, GamePrefs.EnumType.Int, 18, null)
		};
	}

	// Token: 0x060091CB RID: 37323 RVA: 0x0036EF74 File Offset: 0x0036D174
	public override void Init()
	{
		base.Init();
		GameStats.Set(EnumGameStats.ShowSpawnWindow, false);
		GameStats.Set(EnumGameStats.TimeLimitActive, false);
		GameStats.Set(EnumGameStats.DayLimitActive, false);
		GameStats.Set(EnumGameStats.IsSpawnNearOtherPlayer, true);
		GameStats.Set(EnumGameStats.ShowWindow, "");
		GameStats.Set(EnumGameStats.TimeOfDayIncPerSec, 4);
		GameStats.Set(EnumGameStats.IsSpawnEnemies, false);
		GameStats.Set(EnumGameStats.ShowAllPlayersOnMap, true);
		GameStats.Set(EnumGameStats.ZombieHordeMeter, false);
		GameStats.Set(EnumGameStats.DeathPenalty, 0);
		GameStats.Set(EnumGameStats.DropOnDeath, 0);
		GameStats.Set(EnumGameStats.DropOnQuit, 0);
		GameStats.Set(EnumGameStats.IsTeleportEnabled, true);
		GameStats.Set(EnumGameStats.IsFlyingEnabled, true);
		GameStats.Set(EnumGameStats.IsCreativeMenuEnabled, true);
		GameStats.Set(EnumGameStats.IsPlayerDamageEnabled, false);
		GameStats.Set(EnumGameStats.AutoParty, false);
		GamePrefs.Set(EnumGamePrefs.DynamicSpawner, "");
	}

	// Token: 0x060091CC RID: 37324 RVA: 0x00020021 File Offset: 0x0001E221
	public override int GetRoundCount()
	{
		return 1;
	}

	// Token: 0x060091CD RID: 37325 RVA: 0x0036F01C File Offset: 0x0036D21C
	public override void StartRound(int _idx)
	{
		GameStats.Set(EnumGameStats.GameState, 1);
	}

	// Token: 0x060091CE RID: 37326 RVA: 0x000027FC File Offset: 0x000009FC
	public override void EndRound(int _idx)
	{
	}

	// Token: 0x060091CF RID: 37327 RVA: 0x0036F028 File Offset: 0x0036D228
	public override string GetAdditionalGameInfo(World _world)
	{
		switch (GamePrefs.GetInt(EnumGamePrefs.SelectionOperationMode))
		{
		case 0:
			return "Player move";
		case 1:
			return "Selection move " + ((GamePrefs.GetInt(EnumGamePrefs.SelectionContextMode) == 0) ? "absolute" : "relative");
		case 2:
			return "Selection size";
		default:
			return "";
		}
	}

	// Token: 0x04006BEF RID: 27631
	public static readonly string TypeName = typeof(GameModeCreative).Name;
}
