using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000A04 RID: 2564
public class GameRandomManager
{
	// Token: 0x17000842 RID: 2114
	// (get) Token: 0x06004C17 RID: 19479 RVA: 0x001D9ACD File Offset: 0x001D7CCD
	public static GameRandomManager Instance
	{
		get
		{
			if (GameRandomManager.instance == null)
			{
				GameRandomManager.instance = new GameRandomManager();
				GameRandomManager.instance.SetBaseSeed((int)Stopwatch.GetTimestamp());
				GameRandomManager.instance.tempRandom = new GameRandom();
			}
			return GameRandomManager.instance;
		}
	}

	// Token: 0x06004C18 RID: 19480 RVA: 0x001D9B04 File Offset: 0x001D7D04
	public void SetBaseSeed(int _baseSeed)
	{
		this.baseSeed = _baseSeed;
	}

	// Token: 0x17000843 RID: 2115
	// (get) Token: 0x06004C19 RID: 19481 RVA: 0x001D9B0D File Offset: 0x001D7D0D
	public int BaseSeed
	{
		get
		{
			return this.baseSeed;
		}
	}

	// Token: 0x06004C1A RID: 19482 RVA: 0x001D9B15 File Offset: 0x001D7D15
	public GameRandom CreateGameRandom()
	{
		return this.CreateGameRandom(this.baseSeed);
	}

	// Token: 0x06004C1B RID: 19483 RVA: 0x001D9B23 File Offset: 0x001D7D23
	public GameRandom CreateGameRandom(int _seed)
	{
		GameRandom gameRandom = this.pool.AllocSync(false);
		gameRandom.SetSeed(_seed);
		return gameRandom;
	}

	// Token: 0x06004C1C RID: 19484 RVA: 0x001D9B38 File Offset: 0x001D7D38
	public void FreeGameRandom(GameRandom _gameRandom)
	{
		if (_gameRandom == null)
		{
			return;
		}
		this.pool.FreeSync(_gameRandom);
	}

	// Token: 0x06004C1D RID: 19485 RVA: 0x001D9B4A File Offset: 0x001D7D4A
	public GameRandom GetTempGameRandom(int _seed)
	{
		this.tempRandom.SetSeed(_seed);
		return this.tempRandom;
	}

	// Token: 0x06004C1E RID: 19486 RVA: 0x001D9B60 File Offset: 0x001D7D60
	[PublicizedFrom(EAccessModifier.Private)]
	public static void log(string _format, params object[] _values)
	{
		float num = -1f;
		if (ThreadManager.IsMainThread())
		{
			num = Time.time;
			if (num != GameRandomManager.logTime)
			{
				if (GameRandomManager.logCount > 10)
				{
					Log.Warning("GameRandomManager {0} more...", new object[] { GameRandomManager.logCount - 10 });
				}
				GameRandomManager.logTime = num;
				GameRandomManager.logCount = 0;
			}
			if (++GameRandomManager.logCount > 10)
			{
				return;
			}
		}
		Log.Warning(string.Format("{0} GameRandomManager ", num.ToCultureInvariantString()) + _format, _values);
	}

	// Token: 0x04003BE4 RID: 15332
	[PublicizedFrom(EAccessModifier.Private)]
	public static GameRandomManager instance;

	// Token: 0x04003BE5 RID: 15333
	[PublicizedFrom(EAccessModifier.Private)]
	public int baseSeed;

	// Token: 0x04003BE6 RID: 15334
	[PublicizedFrom(EAccessModifier.Private)]
	public MemoryPooledObject<GameRandom> pool = new MemoryPooledObject<GameRandom>(30);

	// Token: 0x04003BE7 RID: 15335
	[PublicizedFrom(EAccessModifier.Private)]
	public GameRandom tempRandom;

	// Token: 0x04003BE8 RID: 15336
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cLogMax = 10;

	// Token: 0x04003BE9 RID: 15337
	[PublicizedFrom(EAccessModifier.Private)]
	public static float logTime;

	// Token: 0x04003BEA RID: 15338
	[PublicizedFrom(EAccessModifier.Private)]
	public static int logCount;
}
