using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001228 RID: 4648
public class GameStateManager
{
	// Token: 0x06009413 RID: 37907 RVA: 0x0037FBAB File Offset: 0x0037DDAB
	public GameStateManager(GameManager _gameManager)
	{
		this.bGameStarted = false;
		this.gameManager = _gameManager;
	}

	// Token: 0x06009414 RID: 37908 RVA: 0x0037FBD4 File Offset: 0x0037DDD4
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

	// Token: 0x06009415 RID: 37909 RVA: 0x0037FC78 File Offset: 0x0037DE78
	public void StartGame()
	{
		this.bGameStarted = true;
	}

	// Token: 0x06009416 RID: 37910 RVA: 0x0037FC81 File Offset: 0x0037DE81
	public bool IsGameStarted()
	{
		return this.bGameStarted;
	}

	// Token: 0x06009417 RID: 37911 RVA: 0x0037FC89 File Offset: 0x0037DE89
	public void EndGame()
	{
		GameStats.Set(EnumGameStats.GameState, 0);
		this.bDirty = true;
		this.bGameStarted = false;
		this.bServer = false;
	}

	// Token: 0x06009418 RID: 37912 RVA: 0x0037FCA8 File Offset: 0x0037DEA8
	public void SetBloodMoonDay(int day)
	{
		int @int = GameStats.GetInt(EnumGameStats.BloodMoonDay);
		if (day != @int)
		{
			GameStats.Set(EnumGameStats.BloodMoonDay, day);
			this.bDirty = true;
		}
	}

	// Token: 0x06009419 RID: 37913 RVA: 0x0037FCD0 File Offset: 0x0037DED0
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

	// Token: 0x0600941A RID: 37914 RVA: 0x0037FD20 File Offset: 0x0037DF20
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

	// Token: 0x0600941B RID: 37915 RVA: 0x0037FF44 File Offset: 0x0037E144
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

	// Token: 0x0600941C RID: 37916 RVA: 0x0037FFA1 File Offset: 0x0037E1A1
	public GameMode GetGameMode()
	{
		return this.currentGameMode;
	}

	// Token: 0x04006E8B RID: 28299
	[PublicizedFrom(EAccessModifier.Private)]
	public float timeRoundStarted;

	// Token: 0x04006E8C RID: 28300
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bDirty;

	// Token: 0x04006E8D RID: 28301
	[PublicizedFrom(EAccessModifier.Private)]
	public GameMode currentGameMode;

	// Token: 0x04006E8E RID: 28302
	[PublicizedFrom(EAccessModifier.Private)]
	public GameManager gameManager;

	// Token: 0x04006E8F RID: 28303
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bServer;

	// Token: 0x04006E90 RID: 28304
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bGameStarted;

	// Token: 0x04006E91 RID: 28305
	[PublicizedFrom(EAccessModifier.Private)]
	public int fragLimitCounter;

	// Token: 0x04006E92 RID: 28306
	[PublicizedFrom(EAccessModifier.Private)]
	public int lastGameModeID = -1;

	// Token: 0x04006E93 RID: 28307
	[PublicizedFrom(EAccessModifier.Private)]
	public string lastGameModeString = string.Empty;
}
