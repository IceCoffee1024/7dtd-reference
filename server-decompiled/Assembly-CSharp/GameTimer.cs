using System;
using UnityEngine;

// Token: 0x0200140E RID: 5134
public class GameTimer
{
	// Token: 0x170012EF RID: 4847
	// (get) Token: 0x0600A159 RID: 41305 RVA: 0x003C846B File Offset: 0x003C666B
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

	// Token: 0x0600A15A RID: 41306 RVA: 0x003C8488 File Offset: 0x003C6688
	public GameTimer(float _t)
	{
		this.ticksPerSecond = _t;
		this.ms = new MicroStopwatch();
		this.Reset(0UL);
	}

	// Token: 0x0600A15B RID: 41307 RVA: 0x003C84AA File Offset: 0x003C66AA
	public void Reset(ulong _ticks = 0UL)
	{
		this.elapsedPartialTicks = 0f;
		this.ticks = _ticks;
		this.ticksSincePlayfieldLoaded = 0UL;
		this.elapsedTicksD = 0.0;
		this.lastMillis = 0L;
		this.ms.ResetAndRestart();
	}

	// Token: 0x0600A15C RID: 41308 RVA: 0x003C84E8 File Offset: 0x003C66E8
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

	// Token: 0x04007970 RID: 31088
	public ulong ticks;

	// Token: 0x04007971 RID: 31089
	public ulong ticksSincePlayfieldLoaded;

	// Token: 0x04007972 RID: 31090
	public int elapsedTicks;

	// Token: 0x04007973 RID: 31091
	public float elapsedPartialTicks;

	// Token: 0x04007974 RID: 31092
	[PublicizedFrom(EAccessModifier.Private)]
	public double elapsedTicksD;

	// Token: 0x04007975 RID: 31093
	[PublicizedFrom(EAccessModifier.Private)]
	public float ticksPerSecond;

	// Token: 0x04007976 RID: 31094
	[PublicizedFrom(EAccessModifier.Private)]
	public long lastMillis;

	// Token: 0x04007977 RID: 31095
	[PublicizedFrom(EAccessModifier.Private)]
	public static GameTimer m_Instance;

	// Token: 0x04007978 RID: 31096
	[PublicizedFrom(EAccessModifier.Private)]
	public MicroStopwatch ms;
}
