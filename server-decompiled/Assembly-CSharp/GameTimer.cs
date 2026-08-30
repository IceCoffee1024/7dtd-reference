using System;
using UnityEngine;

// Token: 0x020013F3 RID: 5107
public class GameTimer
{
	// Token: 0x170012B6 RID: 4790
	// (get) Token: 0x0600A05C RID: 41052 RVA: 0x003C3127 File Offset: 0x003C1327
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

	// Token: 0x0600A05D RID: 41053 RVA: 0x003C3144 File Offset: 0x003C1344
	public GameTimer(float _t)
	{
		this.ticksPerSecond = _t;
		this.ms = new MicroStopwatch();
		this.Reset(0UL);
	}

	// Token: 0x0600A05E RID: 41054 RVA: 0x003C3166 File Offset: 0x003C1366
	public void Reset(ulong _ticks = 0UL)
	{
		this.elapsedPartialTicks = 0f;
		this.ticks = _ticks;
		this.ticksSincePlayfieldLoaded = 0UL;
		this.elapsedTicksD = 0.0;
		this.lastMillis = 0L;
		this.ms.ResetAndRestart();
	}

	// Token: 0x0600A05F RID: 41055 RVA: 0x003C31A4 File Offset: 0x003C13A4
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

	// Token: 0x04007897 RID: 30871
	public ulong ticks;

	// Token: 0x04007898 RID: 30872
	public ulong ticksSincePlayfieldLoaded;

	// Token: 0x04007899 RID: 30873
	public int elapsedTicks;

	// Token: 0x0400789A RID: 30874
	public float elapsedPartialTicks;

	// Token: 0x0400789B RID: 30875
	[PublicizedFrom(EAccessModifier.Private)]
	public double elapsedTicksD;

	// Token: 0x0400789C RID: 30876
	[PublicizedFrom(EAccessModifier.Private)]
	public float ticksPerSecond;

	// Token: 0x0400789D RID: 30877
	[PublicizedFrom(EAccessModifier.Private)]
	public long lastMillis;

	// Token: 0x0400789E RID: 30878
	[PublicizedFrom(EAccessModifier.Private)]
	public static GameTimer m_Instance;

	// Token: 0x0400789F RID: 30879
	[PublicizedFrom(EAccessModifier.Private)]
	public MicroStopwatch ms;
}
