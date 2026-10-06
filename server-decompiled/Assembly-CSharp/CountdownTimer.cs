using System;

// Token: 0x020013DF RID: 5087
public class CountdownTimer
{
	// Token: 0x1700130B RID: 4875
	// (get) Token: 0x06009FB0 RID: 40880 RVA: 0x003C3F2F File Offset: 0x003C212F
	// (set) Token: 0x06009FB1 RID: 40881 RVA: 0x000027FC File Offset: 0x000009FC
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

	// Token: 0x06009FB2 RID: 40882 RVA: 0x003C3F3D File Offset: 0x003C213D
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

	// Token: 0x06009FB3 RID: 40883 RVA: 0x003C3F78 File Offset: 0x003C2178
	public void SetTimeout(float _seconds)
	{
		this.ms = (long)((int)(_seconds * 1000f));
	}

	// Token: 0x06009FB4 RID: 40884 RVA: 0x003C3F8C File Offset: 0x003C218C
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

	// Token: 0x06009FB5 RID: 40885 RVA: 0x003C3FE3 File Offset: 0x003C21E3
	public void SetPassedIn(float _seconds)
	{
		this.offset = (long)((float)this.ms - _seconds * 1000f) - this.ElapsedMilliseconds;
	}

	// Token: 0x06009FB6 RID: 40886 RVA: 0x003C4004 File Offset: 0x003C2204
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		this.Elapsed = DateTime.Now.Subtract(this.StartTime);
	}

	// Token: 0x06009FB7 RID: 40887 RVA: 0x003C402A File Offset: 0x003C222A
	public void Reset()
	{
		this.Elapsed = TimeSpan.Zero;
		this.StartTime = DateTime.Now;
		this.IsRunning = false;
	}

	// Token: 0x06009FB8 RID: 40888 RVA: 0x003C4049 File Offset: 0x003C2249
	public void ResetAndRestart()
	{
		this.Reset();
		this.IsRunning = true;
	}

	// Token: 0x06009FB9 RID: 40889 RVA: 0x003C4058 File Offset: 0x003C2258
	public void Stop()
	{
		this.IsRunning = false;
	}

	// Token: 0x040078F4 RID: 30964
	[PublicizedFrom(EAccessModifier.Private)]
	public long ms;

	// Token: 0x040078F5 RID: 30965
	[PublicizedFrom(EAccessModifier.Private)]
	public long offset;

	// Token: 0x040078F6 RID: 30966
	public TimeSpan Elapsed;

	// Token: 0x040078F7 RID: 30967
	public bool IsRunning;

	// Token: 0x040078F8 RID: 30968
	[PublicizedFrom(EAccessModifier.Private)]
	public DateTime StartTime;
}
