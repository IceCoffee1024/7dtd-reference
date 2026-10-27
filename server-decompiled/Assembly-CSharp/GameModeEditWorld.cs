using System;
using UnityEngine.Scripting;

// Token: 0x020011F7 RID: 4599
[Preserve]
public class GameModeEditWorld : GameModeAbstract
{
	// Token: 0x060092C0 RID: 37568 RVA: 0x00375D49 File Offset: 0x00373F49
	public override string GetName()
	{
		return "gmEditWorld";
	}

	// Token: 0x060092C1 RID: 37569 RVA: 0x00375D50 File Offset: 0x00373F50
	public override string GetDescription()
	{
		return "gmEditWorldDesc";
	}

	// Token: 0x060092C2 RID: 37570 RVA: 0x000816F5 File Offset: 0x0007F8F5
	public override int GetID()
	{
		return 8;
	}

	// Token: 0x060092C3 RID: 37571 RVA: 0x00375D58 File Offset: 0x00373F58
	public override GameMode.ModeGamePref[] GetSupportedGamePrefsInfo()
	{
		return new GameMode.ModeGamePref[]
		{
			new GameMode.ModeGamePref(EnumGamePrefs.GameDifficulty, GamePrefs.EnumType.Int, 1, null),
			new GameMode.ModeGamePref(EnumGamePrefs.ServerIsPublic, GamePrefs.EnumType.Bool, false, null),
			new GameMode.ModeGamePref(EnumGamePrefs.ServerVisibility, GamePrefs.EnumType.Int, 1, null),
			new GameMode.ModeGamePref(EnumGamePrefs.ServerMaxPlayerCount, GamePrefs.EnumType.Int, 8, null)
		};
	}

	// Token: 0x060092C4 RID: 37572 RVA: 0x00375DC8 File Offset: 0x00373FC8
	public override void Init()
	{
		base.Init();
		GameStats.Set(EnumGameStats.ShowSpawnWindow, false);
		GameStats.Set(EnumGameStats.TimeLimitActive, false);
		GameStats.Set(EnumGameStats.DayLimitActive, false);
		GameStats.Set(EnumGameStats.IsSpawnNearOtherPlayer, true);
		GameStats.Set(EnumGameStats.ShowWindow, "");
		GameStats.Set(EnumGameStats.TimeOfDayIncPerSec, 0);
		GameStats.Set(EnumGameStats.IsSpawnEnemies, false);
		GameStats.Set(EnumGameStats.IsTeleportEnabled, true);
		GameStats.Set(EnumGameStats.IsFlyingEnabled, true);
		GameStats.Set(EnumGameStats.IsCreativeMenuEnabled, true);
		GameStats.Set(EnumGameStats.IsPlayerDamageEnabled, false);
		GameStats.Set(EnumGameStats.AirDropFrequency, 0);
		GameStats.Set(EnumGameStats.AutoParty, true);
		GamePrefs.Set(EnumGamePrefs.DynamicSpawner, "");
	}

	// Token: 0x060092C5 RID: 37573 RVA: 0x00020119 File Offset: 0x0001E319
	public override int GetRoundCount()
	{
		return 1;
	}

	// Token: 0x060092C6 RID: 37574 RVA: 0x003759D8 File Offset: 0x00373BD8
	public override void StartRound(int _idx)
	{
		GameStats.Set(EnumGameStats.GameState, 1);
	}

	// Token: 0x060092C7 RID: 37575 RVA: 0x000027FC File Offset: 0x000009FC
	public override void EndRound(int _idx)
	{
	}

	// Token: 0x060092C8 RID: 37576 RVA: 0x00375E50 File Offset: 0x00374050
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

	// Token: 0x04006C73 RID: 27763
	public static readonly string TypeName = typeof(GameModeEditWorld).Name;
}
