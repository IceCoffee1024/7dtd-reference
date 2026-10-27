using System;
using UnityEngine.Scripting;

// Token: 0x020011F5 RID: 4597
[Preserve]
public class GameModeCreative : GameModeAbstract
{
	// Token: 0x060092AB RID: 37547 RVA: 0x003758CA File Offset: 0x00373ACA
	public override string GetName()
	{
		return "gmCreative";
	}

	// Token: 0x060092AC RID: 37548 RVA: 0x003758D1 File Offset: 0x00373AD1
	public override string GetDescription()
	{
		return "gmCreativeDesc";
	}

	// Token: 0x060092AD RID: 37549 RVA: 0x0002F260 File Offset: 0x0002D460
	public override int GetID()
	{
		return 2;
	}

	// Token: 0x060092AE RID: 37550 RVA: 0x003758D8 File Offset: 0x00373AD8
	public override GameMode.ModeGamePref[] GetSupportedGamePrefsInfo()
	{
		return new GameMode.ModeGamePref[]
		{
			new GameMode.ModeGamePref(EnumGamePrefs.ServerIsPublic, GamePrefs.EnumType.Bool, false, null),
			new GameMode.ModeGamePref(EnumGamePrefs.DayNightLength, GamePrefs.EnumType.Int, 60, null),
			new GameMode.ModeGamePref(EnumGamePrefs.DayLightLength, GamePrefs.EnumType.Int, 18, null)
		};
	}

	// Token: 0x060092AF RID: 37551 RVA: 0x00375930 File Offset: 0x00373B30
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

	// Token: 0x060092B0 RID: 37552 RVA: 0x00020119 File Offset: 0x0001E319
	public override int GetRoundCount()
	{
		return 1;
	}

	// Token: 0x060092B1 RID: 37553 RVA: 0x003759D8 File Offset: 0x00373BD8
	public override void StartRound(int _idx)
	{
		GameStats.Set(EnumGameStats.GameState, 1);
	}

	// Token: 0x060092B2 RID: 37554 RVA: 0x000027FC File Offset: 0x000009FC
	public override void EndRound(int _idx)
	{
	}

	// Token: 0x060092B3 RID: 37555 RVA: 0x003759E4 File Offset: 0x00373BE4
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

	// Token: 0x04006C71 RID: 27761
	public static readonly string TypeName = typeof(GameModeCreative).Name;
}
