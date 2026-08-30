using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020011E4 RID: 4580
public class GameStateManager
{
	// Token: 0x0600922B RID: 37419 RVA: 0x00373F23 File Offset: 0x00372123
	public GameStateManager(GameManager _gameManager)
	{
		this.bGameStarted = false;
		this.gameManager = _gameManager;
	}

	// Token: 0x0600922C RID: 37420 RVA: 0x00373F4C File Offset: 0x0037214C
	public void InitGame(bool _bServer)
	{
		this.bServer = _bServer;
		GameStats.Set(EnumGameStats.GameState, 1);
		Type type = Type.GetType(GamePrefs.GetString(EnumGamePrefs.GameMode));
		if (type == null)
		{
			type = Type.GetType((string)GamePrefs.GetDefault(EnumGamePrefs.GameMode));
		}
		this.currentGameMode = (GameMode)Activator.CreateInstance(type);
		GameStats.Set(EnumGameStats.GameModeId, this.currentGameMode.GetID());
		if (this.bServer)
		{
			GameStats.Set(EnumGameStats.CurrentRoundIx, 0);
			this.timeRoundStarted = Time.time;
			this.currentGameMode.Init();
			this.currentGameMode.StartRound(GameStats.GetInt(EnumGameStats.CurrentRoundIx));
			this.bDirty = true;
		}
	}

	// Token: 0x0600922D RID: 37421 RVA: 0x00373FF0 File Offset: 0x003721F0
	public void StartGame()
	{
		this.bGameStarted = true;
	}

	// Token: 0x0600922E RID: 37422 RVA: 0x00373FF9 File Offset: 0x003721F9
	public bool IsGameStarted()
	{
		return this.bGameStarted;
	}

	// Token: 0x0600922F RID: 37423 RVA: 0x00374001 File Offset: 0x00372201
	public void EndGame()
	{
		GameStats.Set(EnumGameStats.GameState, 0);
		this.bDirty = true;
		this.bGameStarted = false;
		this.bServer = false;
	}

	// Token: 0x06009230 RID: 37424 RVA: 0x00374020 File Offset: 0x00372220
	public void SetBloodMoonDay(int day)
	{
		int @int = GameStats.GetInt(EnumGameStats.BloodMoonDay);
		if (day != @int)
		{
			GameStats.Set(EnumGameStats.BloodMoonDay, day);
			this.bDirty = true;
		}
	}

	// Token: 0x06009231 RID: 37425 RVA: 0x00374048 File Offset: 0x00372248
	[PublicizedFrom(EAccessModifier.Private)]
	public int nextRound()
	{
		int num = GameStats.GetInt(EnumGameStats.CurrentRoundIx);
		this.currentGameMode.EndRound(num);
		if (++num >= this.currentGameMode.GetRoundCount())
		{
			num = 0;
		}
		GameStats.Set(EnumGameStats.CurrentRoundIx, num);
		this.bDirty = true;
		this.timeRoundStarted = Time.time;
		return num;
	}

	// Token: 0x06009232 RID: 37426 RVA: 0x00374098 File Offset: 0x00372298
	public bool OnUpdateTick()
	{
		if (this.bServer)
		{
			if (GameStats.GetBool(EnumGameStats.TimeLimitActive))
			{
				int num = GameStats.GetInt(EnumGameStats.TimeLimitThisRound);
				if (num >= 0 && Time.time - this.timeRoundStarted >= 1f)
				{
					int num2 = (int)(Time.time - this.timeRoundStarted);
					this.timeRoundStarted = Time.time;
					num -= num2;
					GameStats.Set(EnumGameStats.TimeLimitThisRound, num);
					if (num < 0)
					{
						this.currentGameMode.StartRound(this.nextRound());
					}
					this.bDirty = true;
				}
			}
			if (GameStats.GetBool(EnumGameStats.DayLimitActive) && GameUtils.WorldTimeToDays(this.gameManager.World.worldTime) > GameStats.GetInt(EnumGameStats.DayLimitThisRound))
			{
				this.currentGameMode.StartRound(this.nextRound());
			}
			if (GameStats.GetBool(EnumGameStats.FragLimitActive))
			{
				int num3 = this.fragLimitCounter + 1;
				this.fragLimitCounter = num3;
				if (num3 > 40)
				{
					this.fragLimitCounter = 0;
					int @int = GameStats.GetInt(EnumGameStats.FragLimitThisRound);
					for (int i = 0; i < this.gameManager.World.Players.list.Count; i++)
					{
						if (this.gameManager.World.Players.list[i].KilledPlayers >= @int)
						{
							this.currentGameMode.StartRound(this.nextRound());
							break;
						}
					}
				}
			}
			if (GameTimer.Instance.ticks % 20UL == 0UL)
			{
				int num4 = 0;
				int num5 = 0;
				List<Entity> list = this.gameManager.World.Entities.list;
				for (int j = list.Count - 1; j >= 0; j--)
				{
					Entity entity = list[j];
					if (!entity.IsDead())
					{
						EntityClass entityClass = EntityClass.list[entity.entityClass];
						if (entityClass.bIsEnemyEntity)
						{
							num4++;
						}
						else if (entityClass.bIsAnimalEntity)
						{
							num5++;
						}
					}
				}
				GameStats.Set(EnumGameStats.EnemyCount, num4);
				GameStats.Set(EnumGameStats.AnimalCount, num5);
			}
			if (this.bDirty)
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageGameStats>().Setup(GameStats.Instance), true, -1, -1, -1, null, 192, false);
				this.bDirty = false;
			}
		}
		return true;
	}

	// Token: 0x06009233 RID: 37427 RVA: 0x003742BC File Offset: 0x003724BC
	public string GetModeName()
	{
		if (this.currentGameMode == null)
		{
			return string.Empty;
		}
		if (this.currentGameMode.GetID() != this.lastGameModeID)
		{
			this.lastGameModeString = Localization.Get(this.currentGameMode.GetName(), false, null);
			this.lastGameModeID = this.currentGameMode.GetID();
		}
		return this.lastGameModeString;
	}

	// Token: 0x06009234 RID: 37428 RVA: 0x00374319 File Offset: 0x00372519
	public GameMode GetGameMode()
	{
		return this.currentGameMode;
	}

	// Token: 0x04006D30 RID: 27952
	[PublicizedFrom(EAccessModifier.Private)]
	public float timeRoundStarted;

	// Token: 0x04006D31 RID: 27953
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bDirty;

	// Token: 0x04006D32 RID: 27954
	[PublicizedFrom(EAccessModifier.Private)]
	public GameMode currentGameMode;

	// Token: 0x04006D33 RID: 27955
	[PublicizedFrom(EAccessModifier.Private)]
	public GameManager gameManager;

	// Token: 0x04006D34 RID: 27956
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bServer;

	// Token: 0x04006D35 RID: 27957
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bGameStarted;

	// Token: 0x04006D36 RID: 27958
	[PublicizedFrom(EAccessModifier.Private)]
	public int fragLimitCounter;

	// Token: 0x04006D37 RID: 27959
	[PublicizedFrom(EAccessModifier.Private)]
	public int lastGameModeID = -1;

	// Token: 0x04006D38 RID: 27960
	[PublicizedFrom(EAccessModifier.Private)]
	public string lastGameModeString = string.Empty;
}
