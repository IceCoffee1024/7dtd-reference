using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000A0E RID: 2574
public class GameRandomManager
{
	// Token: 0x17000848 RID: 2120
	// (get) Token: 0x06004C3B RID: 19515 RVA: 0x001DABB5 File Offset: 0x001D8DB5
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

	// Token: 0x06004C3C RID: 19516 RVA: 0x001DABEC File Offset: 0x001D8DEC
	public void SetBaseSeed(int _baseSeed)
	{
		this.baseSeed = _baseSeed;
	}

	// Token: 0x17000849 RID: 2121
	// (get) Token: 0x06004C3D RID: 19517 RVA: 0x001DABF5 File Offset: 0x001D8DF5
	public int BaseSeed
	{
		get
		{
			return this.baseSeed;
		}
	}

	// Token: 0x06004C3E RID: 19518 RVA: 0x001DABFD File Offset: 0x001D8DFD
	public GameRandom CreateGameRandom()
	{
		return this.CreateGameRandom(this.baseSeed);
	}

	// Token: 0x06004C3F RID: 19519 RVA: 0x001DAC0B File Offset: 0x001D8E0B
	public GameRandom CreateGameRandom(int _seed)
	{
		GameRandom gameRandom = this.pool.AllocSync(false);
		gameRandom.SetSeed(_seed);
		return gameRandom;
	}

	// Token: 0x06004C40 RID: 19520 RVA: 0x001DAC20 File Offset: 0x001D8E20
	public void FreeGameRandom(GameRandom _gameRandom)
	{
		if (_gameRandom == null)
		{
			return;
		}
		this.pool.FreeSync(_gameRandom);
	}

	// Token: 0x06004C41 RID: 19521 RVA: 0x001DAC32 File Offset: 0x001D8E32
	public GameRandom GetTempGameRandom(int _seed)
	{
		this.tempRandom.SetSeed(_seed);
		return this.tempRandom;
	}

	// Token: 0x06004C42 RID: 19522 RVA: 0x001DAC48 File Offset: 0x001D8E48
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

	// Token: 0x04003C05 RID: 15365
	[PublicizedFrom(EAccessModifier.Private)]
	public static GameRandomManager instance;

	// Token: 0x04003C06 RID: 15366
	[PublicizedFrom(EAccessModifier.Private)]
	public int baseSeed;

	// Token: 0x04003C07 RID: 15367
	[PublicizedFrom(EAccessModifier.Private)]
	public MemoryPooledObject<GameRandom> pool = new MemoryPooledObject<GameRandom>(30);

	// Token: 0x04003C08 RID: 15368
	[PublicizedFrom(EAccessModifier.Private)]
	public GameRandom tempRandom;

	// Token: 0x04003C09 RID: 15369
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cLogMax = 10;

	// Token: 0x04003C0A RID: 15370
	[PublicizedFrom(EAccessModifier.Private)]
	public static float logTime;

	// Token: 0x04003C0B RID: 15371
	[PublicizedFrom(EAccessModifier.Private)]
	public static int logCount;
}
