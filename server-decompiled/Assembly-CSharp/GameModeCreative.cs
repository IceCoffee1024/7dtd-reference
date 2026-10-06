using System;
using UnityEngine.Scripting;

// Token: 0x020011E5 RID: 4581
[Preserve]
public class GameModeCreative : GameModeAbstract
{
	// Token: 0x06009255 RID: 37461 RVA: 0x00373756 File Offset: 0x00371956
	public override string GetName()
	{
		return "gmCreative";
	}

	// Token: 0x06009256 RID: 37462 RVA: 0x0037375D File Offset: 0x0037195D
	public override string GetDescription()
	{
		return "gmCreativeDesc";
	}

	// Token: 0x06009257 RID: 37463 RVA: 0x0002F260 File Offset: 0x0002D460
	public override int GetID()
	{
		return 2;
	}

	// Token: 0x06009258 RID: 37464 RVA: 0x00373764 File Offset: 0x00371964
	public override GameMode.ModeGamePref[] GetSupportedGamePrefsInfo()
	{
		return new GameMode.ModeGamePref[]
		{
			new GameMode.ModeGamePref(EnumGamePrefs.ServerIsPublic, GamePrefs.EnumType.Bool, false, null),
			new GameMode.ModeGamePref(EnumGamePrefs.DayNightLength, GamePrefs.EnumType.Int, 60, null),
			new GameMode.ModeGamePref(EnumGamePrefs.DayLightLength, GamePrefs.EnumType.Int, 18, null)
		};
	}

	// Token: 0x06009259 RID: 37465 RVA: 0x003737BC File Offset: 0x003719BC
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

	// Token: 0x0600925A RID: 37466 RVA: 0x00020119 File Offset: 0x0001E319
	public override int GetRoundCount()
	{
		return 1;
	}

	// Token: 0x0600925B RID: 37467 RVA: 0x00373864 File Offset: 0x00371A64
	public override void StartRound(int _idx)
	{
		GameStats.Set(EnumGameStats.GameState, 1);
	}

	// Token: 0x0600925C RID: 37468 RVA: 0x000027FC File Offset: 0x000009FC
	public override void EndRound(int _idx)
	{
	}

	// Token: 0x0600925D RID: 37469 RVA: 0x00373870 File Offset: 0x00371A70
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

	// Token: 0x04006C59 RID: 27737
	public static readonly string TypeName = typeof(GameModeCreative).Name;
}
