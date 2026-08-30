using System;
using System.Diagnostics;

// Token: 0x02000003 RID: 3
public class MicroStopwatch : Stopwatch
{
	// Token: 0x06000016 RID: 22 RVA: 0x000027EA File Offset: 0x000009EA
	public MicroStopwatch()
		: this(true)
	{
	}

	// Token: 0x06000017 RID: 23 RVA: 0x000027F3 File Offset: 0x000009F3
	public MicroStopwatch(bool _bStart)
	{
		if (!Stopwatch.IsHighResolution)
		{
			throw new Exception("On this system the high-resolution performance counter is not available");
		}
		if (_bStart)
		{
			base.Start();
		}
	}

	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000018 RID: 24 RVA: 0x0000282C File Offset: 0x00000A2C
	public long ElapsedMicroseconds
	{
		get
		{
			return (long)((double)base.ElapsedTicks * this.microSecPerTick);
		}
	}

	// Token: 0x06000019 RID: 25 RVA: 0x0000283D File Offset: 0x00000A3D
	public static MicroStopwatch a()
	{
		MicroStopwatch microStopwatch = new MicroStopwatch();
		microStopwatch.Start();
		return microStopwatch;
	}

	// Token: 0x0600001A RID: 26 RVA: 0x0000284A File Offset: 0x00000A4A
	public void ResetAndRestart()
	{
		base.Reset();
		base.Start();
	}

	// Token: 0x04000008 RID: 8
	private readonly double microSecPerTick = 1000000.0 / (double)Stopwatch.Frequency;
}
