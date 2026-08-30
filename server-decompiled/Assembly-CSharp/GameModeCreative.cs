using System;
using UnityEngine.Scripting;

// Token: 0x020011B1 RID: 4529
[Preserve]
public class GameModeCreative : GameModeAbstract
{
	// Token: 0x060090C3 RID: 37059 RVA: 0x00369C72 File Offset: 0x00367E72
	public override string GetName()
	{
		return "gmCreative";
	}

	// Token: 0x060090C4 RID: 37060 RVA: 0x00369C79 File Offset: 0x00367E79
	public override string GetDescription()
	{
		return "gmCreativeDesc";
	}

	// Token: 0x060090C5 RID: 37061 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override int GetID()
	{
		return 2;
	}

	// Token: 0x060090C6 RID: 37062 RVA: 0x00369C80 File Offset: 0x00367E80
	public override GameMode.ModeGamePref[] GetSupportedGamePrefsInfo()
	{
		return new GameMode.ModeGamePref[]
		{
			new GameMode.ModeGamePref(EnumGamePrefs.ServerIsPublic, GamePrefs.EnumType.Bool, false, null),
			new GameMode.ModeGamePref(EnumGamePrefs.DayNightLength, GamePrefs.EnumType.Int, 60, null),
			new GameMode.ModeGamePref(EnumGamePrefs.DayLightLength, GamePrefs.EnumType.Int, 18, null)
		};
	}

	// Token: 0x060090C7 RID: 37063 RVA: 0x00369CD8 File Offset: 0x00367ED8
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

	// Token: 0x060090C8 RID: 37064 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override int GetRoundCount()
	{
		return 1;
	}

	// Token: 0x060090C9 RID: 37065 RVA: 0x00369D80 File Offset: 0x00367F80
	public override void StartRound(int _idx)
	{
		GameStats.Set(EnumGameStats.GameState, 1);
	}

	// Token: 0x060090CA RID: 37066 RVA: 0x000027FC File Offset: 0x000009FC
	public override void EndRound(int _idx)
	{
	}

	// Token: 0x060090CB RID: 37067 RVA: 0x00369D8C File Offset: 0x00367F8C
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

	// Token: 0x04006B17 RID: 27415
	public static readonly string TypeName = typeof(GameModeCreative).Name;
}
