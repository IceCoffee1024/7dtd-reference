using System;
using UnityEngine.Scripting;

// Token: 0x020011E7 RID: 4583
[Preserve]
public class GameModeEditWorld : GameModeAbstract
{
	// Token: 0x0600926A RID: 37482 RVA: 0x00373BD5 File Offset: 0x00371DD5
	public override string GetName()
	{
		return "gmEditWorld";
	}

	// Token: 0x0600926B RID: 37483 RVA: 0x00373BDC File Offset: 0x00371DDC
	public override string GetDescription()
	{
		return "gmEditWorldDesc";
	}

	// Token: 0x0600926C RID: 37484 RVA: 0x000814D1 File Offset: 0x0007F6D1
	public override int GetID()
	{
		return 8;
	}

	// Token: 0x0600926D RID: 37485 RVA: 0x00373BE4 File Offset: 0x00371DE4
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

	// Token: 0x0600926E RID: 37486 RVA: 0x00373C54 File Offset: 0x00371E54
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

	// Token: 0x0600926F RID: 37487 RVA: 0x00020119 File Offset: 0x0001E319
	public override int GetRoundCount()
	{
		return 1;
	}

	// Token: 0x06009270 RID: 37488 RVA: 0x00373864 File Offset: 0x00371A64
	public override void StartRound(int _idx)
	{
		GameStats.Set(EnumGameStats.GameState, 1);
	}

	// Token: 0x06009271 RID: 37489 RVA: 0x000027FC File Offset: 0x000009FC
	public override void EndRound(int _idx)
	{
	}

	// Token: 0x06009272 RID: 37490 RVA: 0x00373CDC File Offset: 0x00371EDC
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

	// Token: 0x04006C5B RID: 27739
	public static readonly string TypeName = typeof(GameModeEditWorld).Name;
}
