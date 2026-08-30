using System;

// Token: 0x020013A4 RID: 5028
public class CountdownTimer
{
	// Token: 0x1700128D RID: 4749
	// (get) Token: 0x06009E08 RID: 40456 RVA: 0x003BA04F File Offset: 0x003B824F
	// (set) Token: 0x06009E09 RID: 40457 RVA: 0x000027FC File Offset: 0x000009FC
	public long ElapsedMilliseconds
	{
		get
		{
			return (long)this.Elapsed.TotalMilliseconds;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
		}
	}

	// Token: 0x06009E0A RID: 40458 RVA: 0x003BA05D File Offset: 0x003B825D
	public CountdownTimer(float _seconds, bool _start = true)
	{
		this.ms = (long)((int)(_seconds * 1000f));
		this.IsRunning = _start;
		this.offset = 0L;
		if (this.IsRunning)
		{
			this.ResetAndRestart();
			return;
		}
		this.Reset();
	}

	// Token: 0x06009E0B RID: 40459 RVA: 0x003BA098 File Offset: 0x003B8298
	public void SetTimeout(float _seconds)
	{
		this.ms = (long)((int)(_seconds * 1000f));
	}

	// Token: 0x06009E0C RID: 40460 RVA: 0x003BA0AC File Offset: 0x003B82AC
	public bool HasPassed()
	{
		bool flag = false;
		if (this.IsRunning)
		{
			this.Update();
			flag = ((this.offset == 0L) ? (this.ElapsedMilliseconds > this.ms) : (this.ElapsedMilliseconds + this.offset > this.ms));
			if (flag)
			{
				this.offset = 0L;
			}
		}
		return flag;
	}

	// Token: 0x06009E0D RID: 40461 RVA: 0x003BA103 File Offset: 0x003B8303
	public void SetPassedIn(float _seconds)
	{
		this.offset = (long)((float)this.ms - _seconds * 1000f) - this.ElapsedMilliseconds;
	}

	// Token: 0x06009E0E RID: 40462 RVA: 0x003BA124 File Offset: 0x003B8324
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		this.Elapsed = DateTime.Now.Subtract(this.StartTime);
	}

	// Token: 0x06009E0F RID: 40463 RVA: 0x003BA14A File Offset: 0x003B834A
	public void Reset()
	{
		this.Elapsed = TimeSpan.Zero;
		this.StartTime = DateTime.Now;
		this.IsRunning = false;
	}

	// Token: 0x06009E10 RID: 40464 RVA: 0x003BA169 File Offset: 0x003B8369
	public void ResetAndRestart()
	{
		this.Reset();
		this.IsRunning = true;
	}

	// Token: 0x06009E11 RID: 40465 RVA: 0x003BA178 File Offset: 0x003B8378
	public void Stop()
	{
		this.IsRunning = false;
	}

	// Token: 0x040077A0 RID: 30624
	[PublicizedFrom(EAccessModifier.Private)]
	public long ms;

	// Token: 0x040077A1 RID: 30625
	[PublicizedFrom(EAccessModifier.Private)]
	public long offset;

	// Token: 0x040077A2 RID: 30626
	public TimeSpan Elapsed;

	// Token: 0x040077A3 RID: 30627
	public bool IsRunning;

	// Token: 0x040077A4 RID: 30628
	[PublicizedFrom(EAccessModifier.Private)]
	public DateTime StartTime;
}
