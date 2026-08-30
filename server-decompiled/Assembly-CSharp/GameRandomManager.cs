using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020009E9 RID: 2537
public class GameRandomManager
{
	// Token: 0x17000810 RID: 2064
	// (get) Token: 0x06004BC2 RID: 19394 RVA: 0x001D6035 File Offset: 0x001D4235
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

	// Token: 0x06004BC3 RID: 19395 RVA: 0x001D606C File Offset: 0x001D426C
	public void SetBaseSeed(int _baseSeed)
	{
		this.baseSeed = _baseSeed;
	}

	// Token: 0x17000811 RID: 2065
	// (get) Token: 0x06004BC4 RID: 19396 RVA: 0x001D6075 File Offset: 0x001D4275
	public int BaseSeed
	{
		get
		{
			return this.baseSeed;
		}
	}

	// Token: 0x06004BC5 RID: 19397 RVA: 0x001D607D File Offset: 0x001D427D
	public GameRandom CreateGameRandom()
	{
		return this.CreateGameRandom(this.baseSeed);
	}

	// Token: 0x06004BC6 RID: 19398 RVA: 0x001D608B File Offset: 0x001D428B
	public GameRandom CreateGameRandom(int _seed)
	{
		GameRandom gameRandom = this.pool.AllocSync(false);
		gameRandom.SetSeed(_seed);
		return gameRandom;
	}

	// Token: 0x06004BC7 RID: 19399 RVA: 0x001D60A0 File Offset: 0x001D42A0
	public void FreeGameRandom(GameRandom _gameRandom)
	{
		if (_gameRandom == null)
		{
			return;
		}
		this.pool.FreeSync(_gameRandom);
	}

	// Token: 0x06004BC8 RID: 19400 RVA: 0x001D60B2 File Offset: 0x001D42B2
	public GameRandom GetTempGameRandom(int _seed)
	{
		this.tempRandom.SetSeed(_seed);
		return this.tempRandom;
	}

	// Token: 0x06004BC9 RID: 19401 RVA: 0x001D60C8 File Offset: 0x001D42C8
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

	// Token: 0x04003B46 RID: 15174
	[PublicizedFrom(EAccessModifier.Private)]
	public static GameRandomManager instance;

	// Token: 0x04003B47 RID: 15175
	[PublicizedFrom(EAccessModifier.Private)]
	public int baseSeed;

	// Token: 0x04003B48 RID: 15176
	[PublicizedFrom(EAccessModifier.Private)]
	public MemoryPooledObject<GameRandom> pool = new MemoryPooledObject<GameRandom>(30);

	// Token: 0x04003B49 RID: 15177
	[PublicizedFrom(EAccessModifier.Private)]
	public GameRandom tempRandom;

	// Token: 0x04003B4A RID: 15178
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cLogMax = 10;

	// Token: 0x04003B4B RID: 15179
	[PublicizedFrom(EAccessModifier.Private)]
	public static float logTime;

	// Token: 0x04003B4C RID: 15180
	[PublicizedFrom(EAccessModifier.Private)]
	public static int logCount;
}
