using System;
using UnityEngine.Scripting;

// Token: 0x020011B3 RID: 4531
[Preserve]
public class GameModeEditWorld : GameModeAbstract
{
	// Token: 0x060090D8 RID: 37080 RVA: 0x0036A0F1 File Offset: 0x003682F1
	public override string GetName()
	{
		return "gmEditWorld";
	}

	// Token: 0x060090D9 RID: 37081 RVA: 0x0036A0F8 File Offset: 0x003682F8
	public override string GetDescription()
	{
		return "gmEditWorldDesc";
	}

	// Token: 0x060090DA RID: 37082 RVA: 0x00080B69 File Offset: 0x0007ED69
	public override int GetID()
	{
		return 8;
	}

	// Token: 0x060090DB RID: 37083 RVA: 0x0036A100 File Offset: 0x00368300
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

	// Token: 0x060090DC RID: 37084 RVA: 0x0036A170 File Offset: 0x00368370
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

	// Token: 0x060090DD RID: 37085 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override int GetRoundCount()
	{
		return 1;
	}

	// Token: 0x060090DE RID: 37086 RVA: 0x00369D80 File Offset: 0x00367F80
	public override void StartRound(int _idx)
	{
		GameStats.Set(EnumGameStats.GameState, 1);
	}

	// Token: 0x060090DF RID: 37087 RVA: 0x000027FC File Offset: 0x000009FC
	public override void EndRound(int _idx)
	{
	}

	// Token: 0x060090E0 RID: 37088 RVA: 0x0036A1F8 File Offset: 0x003683F8
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

	// Token: 0x04006B19 RID: 27417
	public static readonly string TypeName = typeof(GameModeEditWorld).Name;
}
