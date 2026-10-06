using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001218 RID: 4632
public class GameStateManager
{
	// Token: 0x060093BD RID: 37821 RVA: 0x0037DA37 File Offset: 0x0037BC37
	public GameStateManager(GameManager _gameManager)
	{
		this.bGameStarted = false;
		this.gameManager = _gameManager;
	}

	// Token: 0x060093BE RID: 37822 RVA: 0x0037DA60 File Offset: 0x0037BC60
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

	// Token: 0x060093BF RID: 37823 RVA: 0x0037DB04 File Offset: 0x0037BD04
	public void StartGame()
	{
		this.bGameStarted = true;
	}

	// Token: 0x060093C0 RID: 37824 RVA: 0x0037DB0D File Offset: 0x0037BD0D
	public bool IsGameStarted()
	{
		return this.bGameStarted;
	}

	// Token: 0x060093C1 RID: 37825 RVA: 0x0037DB15 File Offset: 0x0037BD15
	public void EndGame()
	{
		GameStats.Set(EnumGameStats.GameState, 0);
		this.bDirty = true;
		this.bGameStarted = false;
		this.bServer = false;
	}

	// Token: 0x060093C2 RID: 37826 RVA: 0x0037DB34 File Offset: 0x0037BD34
	public void SetBloodMoonDay(int day)
	{
		int @int = GameStats.GetInt(EnumGameStats.BloodMoonDay);
		if (day != @int)
		{
			GameStats.Set(EnumGameStats.BloodMoonDay, day);
			this.bDirty = true;
		}
	}

	// Token: 0x060093C3 RID: 37827 RVA: 0x0037DB5C File Offset: 0x0037BD5C
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

	// Token: 0x060093C4 RID: 37828 RVA: 0x0037DBAC File Offset: 0x0037BDAC
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

	// Token: 0x060093C5 RID: 37829 RVA: 0x0037DDD0 File Offset: 0x0037BFD0
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

	// Token: 0x060093C6 RID: 37830 RVA: 0x0037DE2D File Offset: 0x0037C02D
	public GameMode GetGameMode()
	{
		return this.currentGameMode;
	}

	// Token: 0x04006E73 RID: 28275
	[PublicizedFrom(EAccessModifier.Private)]
	public float timeRoundStarted;

	// Token: 0x04006E74 RID: 28276
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bDirty;

	// Token: 0x04006E75 RID: 28277
	[PublicizedFrom(EAccessModifier.Private)]
	public GameMode currentGameMode;

	// Token: 0x04006E76 RID: 28278
	[PublicizedFrom(EAccessModifier.Private)]
	public GameManager gameManager;

	// Token: 0x04006E77 RID: 28279
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bServer;

	// Token: 0x04006E78 RID: 28280
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bGameStarted;

	// Token: 0x04006E79 RID: 28281
	[PublicizedFrom(EAccessModifier.Private)]
	public int fragLimitCounter;

	// Token: 0x04006E7A RID: 28282
	[PublicizedFrom(EAccessModifier.Private)]
	public int lastGameModeID = -1;

	// Token: 0x04006E7B RID: 28283
	[PublicizedFrom(EAccessModifier.Private)]
	public string lastGameModeString = string.Empty;
}
