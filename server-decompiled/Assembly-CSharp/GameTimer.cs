using System;
using UnityEngine;

// Token: 0x0200142E RID: 5166
public class GameTimer
{
	// Token: 0x17001334 RID: 4916
	// (get) Token: 0x0600A204 RID: 41476 RVA: 0x003CD007 File Offset: 0x003CB207
	public static GameTimer Instance
	{
		get
		{
			if (GameTimer.m_Instance == null)
			{
				GameTimer.m_Instance = new GameTimer(20f);
			}
			return GameTimer.m_Instance;
		}
	}

	// Token: 0x0600A205 RID: 41477 RVA: 0x003CD024 File Offset: 0x003CB224
	public GameTimer(float _t)
	{
		this.ticksPerSecond = _t;
		this.ms = new MicroStopwatch();
		this.Reset(0UL);
	}

	// Token: 0x0600A206 RID: 41478 RVA: 0x003CD046 File Offset: 0x003CB246
	public void Reset(ulong _ticks = 0UL)
	{
		this.elapsedPartialTicks = 0f;
		this.ticks = _ticks;
		this.ticksSincePlayfieldLoaded = 0UL;
		this.elapsedTicksD = 0.0;
		this.lastMillis = 0L;
		this.ms.ResetAndRestart();
	}

	// Token: 0x0600A207 RID: 41479 RVA: 0x003CD084 File Offset: 0x003CB284
	public void updateTimer(bool _bServerIsStopped)
	{
		if (_bServerIsStopped)
		{
			this.Reset(this.ticks);
			return;
		}
		long elapsedMilliseconds = this.ms.ElapsedMilliseconds;
		long num = elapsedMilliseconds - this.lastMillis;
		this.lastMillis = elapsedMilliseconds;
		this.elapsedTicksD += (double)(Time.timeScale * (float)num) / 1000.0 * (double)this.ticksPerSecond;
		this.elapsedTicks = (int)this.elapsedTicksD;
		this.elapsedPartialTicks = (float)(this.elapsedTicksD - (double)this.elapsedTicks);
		this.elapsedTicksD -= (double)this.elapsedTicks;
		this.ticks += (ulong)((long)this.elapsedTicks);
		this.ticksSincePlayfieldLoaded += (ulong)((long)this.elapsedTicks);
	}

	// Token: 0x040079EB RID: 31211
	public ulong ticks;

	// Token: 0x040079EC RID: 31212
	public ulong ticksSincePlayfieldLoaded;

	// Token: 0x040079ED RID: 31213
	public int elapsedTicks;

	// Token: 0x040079EE RID: 31214
	public float elapsedPartialTicks;

	// Token: 0x040079EF RID: 31215
	[PublicizedFrom(EAccessModifier.Private)]
	public double elapsedTicksD;

	// Token: 0x040079F0 RID: 31216
	[PublicizedFrom(EAccessModifier.Private)]
	public float ticksPerSecond;

	// Token: 0x040079F1 RID: 31217
	[PublicizedFrom(EAccessModifier.Private)]
	public long lastMillis;

	// Token: 0x040079F2 RID: 31218
	[PublicizedFrom(EAccessModifier.Private)]
	public static GameTimer m_Instance;

	// Token: 0x040079F3 RID: 31219
	[PublicizedFrom(EAccessModifier.Private)]
	public MicroStopwatch ms;
}
