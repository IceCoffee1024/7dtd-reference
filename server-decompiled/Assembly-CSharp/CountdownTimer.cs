using System;

// Token: 0x020013F1 RID: 5105
public class CountdownTimer
{
	// Token: 0x17001317 RID: 4887
	// (get) Token: 0x0600A008 RID: 40968 RVA: 0x003C6333 File Offset: 0x003C4533
	// (set) Token: 0x0600A009 RID: 40969 RVA: 0x000027FC File Offset: 0x000009FC
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

	// Token: 0x0600A00A RID: 40970 RVA: 0x003C6341 File Offset: 0x003C4541
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

	// Token: 0x0600A00B RID: 40971 RVA: 0x003C637C File Offset: 0x003C457C
	public void SetTimeout(float _seconds)
	{
		this.ms = (long)((int)(_seconds * 1000f));
	}

	// Token: 0x0600A00C RID: 40972 RVA: 0x003C6390 File Offset: 0x003C4590
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

	// Token: 0x0600A00D RID: 40973 RVA: 0x003C63E7 File Offset: 0x003C45E7
	public void SetPassedIn(float _seconds)
	{
		this.offset = (long)((float)this.ms - _seconds * 1000f) - this.ElapsedMilliseconds;
	}

	// Token: 0x0600A00E RID: 40974 RVA: 0x003C6408 File Offset: 0x003C4608
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		this.Elapsed = DateTime.Now.Subtract(this.StartTime);
	}

	// Token: 0x0600A00F RID: 40975 RVA: 0x003C642E File Offset: 0x003C462E
	public void Reset()
	{
		this.Elapsed = TimeSpan.Zero;
		this.StartTime = DateTime.Now;
		this.IsRunning = false;
	}

	// Token: 0x0600A010 RID: 40976 RVA: 0x003C644D File Offset: 0x003C464D
	public void ResetAndRestart()
	{
		this.Reset();
		this.IsRunning = true;
	}

	// Token: 0x0600A011 RID: 40977 RVA: 0x003C645C File Offset: 0x003C465C
	public void Stop()
	{
		this.IsRunning = false;
	}

	// Token: 0x04007935 RID: 31029
	[PublicizedFrom(EAccessModifier.Private)]
	public long ms;

	// Token: 0x04007936 RID: 31030
	[PublicizedFrom(EAccessModifier.Private)]
	public long offset;

	// Token: 0x04007937 RID: 31031
	public TimeSpan Elapsed;

	// Token: 0x04007938 RID: 31032
	public bool IsRunning;

	// Token: 0x04007939 RID: 31033
	[PublicizedFrom(EAccessModifier.Private)]
	public DateTime StartTime;
}
