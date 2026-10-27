using System;
using UnityEngine;

// Token: 0x02001440 RID: 5184
public class GameTimer
{
	// Token: 0x17001340 RID: 4928
	// (get) Token: 0x0600A25C RID: 41564 RVA: 0x003CF40B File Offset: 0x003CD60B
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

	// Token: 0x0600A25D RID: 41565 RVA: 0x003CF428 File Offset: 0x003CD628
	public GameTimer(float _t)
	{
		this.ticksPerSecond = _t;
		this.ms = new MicroStopwatch();
		this.Reset(0UL);
	}

	// Token: 0x0600A25E RID: 41566 RVA: 0x003CF44A File Offset: 0x003CD64A
	public void Reset(ulong _ticks = 0UL)
	{
		this.elapsedPartialTicks = 0f;
		this.ticks = _ticks;
		this.ticksSincePlayfieldLoaded = 0UL;
		this.elapsedTicksD = 0.0;
		this.lastMillis = 0L;
		this.ms.ResetAndRestart();
	}

	// Token: 0x0600A25F RID: 41567 RVA: 0x003CF488 File Offset: 0x003CD688
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

	// Token: 0x04007A2C RID: 31276
	public ulong ticks;

	// Token: 0x04007A2D RID: 31277
	public ulong ticksSincePlayfieldLoaded;

	// Token: 0x04007A2E RID: 31278
	public int elapsedTicks;

	// Token: 0x04007A2F RID: 31279
	public float elapsedPartialTicks;

	// Token: 0x04007A30 RID: 31280
	[PublicizedFrom(EAccessModifier.Private)]
	public double elapsedTicksD;

	// Token: 0x04007A31 RID: 31281
	[PublicizedFrom(EAccessModifier.Private)]
	public float ticksPerSecond;

	// Token: 0x04007A32 RID: 31282
	[PublicizedFrom(EAccessModifier.Private)]
	public long lastMillis;

	// Token: 0x04007A33 RID: 31283
	[PublicizedFrom(EAccessModifier.Private)]
	public static GameTimer m_Instance;

	// Token: 0x04007A34 RID: 31284
	[PublicizedFrom(EAccessModifier.Private)]
	public MicroStopwatch ms;
}
