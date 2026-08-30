using System;

// Token: 0x020013BF RID: 5055
public class CountdownTimer
{
	// Token: 0x170012C6 RID: 4806
	// (get) Token: 0x06009F05 RID: 40709 RVA: 0x003BF393 File Offset: 0x003BD593
	// (set) Token: 0x06009F06 RID: 40710 RVA: 0x000027FC File Offset: 0x000009FC
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

	// Token: 0x06009F07 RID: 40711 RVA: 0x003BF3A1 File Offset: 0x003BD5A1
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

	// Token: 0x06009F08 RID: 40712 RVA: 0x003BF3DC File Offset: 0x003BD5DC
	public void SetTimeout(float _seconds)
	{
		this.ms = (long)((int)(_seconds * 1000f));
	}

	// Token: 0x06009F09 RID: 40713 RVA: 0x003BF3F0 File Offset: 0x003BD5F0
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

	// Token: 0x06009F0A RID: 40714 RVA: 0x003BF447 File Offset: 0x003BD647
	public void SetPassedIn(float _seconds)
	{
		this.offset = (long)((float)this.ms - _seconds * 1000f) - this.ElapsedMilliseconds;
	}

	// Token: 0x06009F0B RID: 40715 RVA: 0x003BF468 File Offset: 0x003BD668
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		this.Elapsed = DateTime.Now.Subtract(this.StartTime);
	}

	// Token: 0x06009F0C RID: 40716 RVA: 0x003BF48E File Offset: 0x003BD68E
	public void Reset()
	{
		this.Elapsed = TimeSpan.Zero;
		this.StartTime = DateTime.Now;
		this.IsRunning = false;
	}

	// Token: 0x06009F0D RID: 40717 RVA: 0x003BF4AD File Offset: 0x003BD6AD
	public void ResetAndRestart()
	{
		this.Reset();
		this.IsRunning = true;
	}

	// Token: 0x06009F0E RID: 40718 RVA: 0x003BF4BC File Offset: 0x003BD6BC
	public void Stop()
	{
		this.IsRunning = false;
	}

	// Token: 0x04007879 RID: 30841
	[PublicizedFrom(EAccessModifier.Private)]
	public long ms;

	// Token: 0x0400787A RID: 30842
	[PublicizedFrom(EAccessModifier.Private)]
	public long offset;

	// Token: 0x0400787B RID: 30843
	public TimeSpan Elapsed;

	// Token: 0x0400787C RID: 30844
	public bool IsRunning;

	// Token: 0x0400787D RID: 30845
	[PublicizedFrom(EAccessModifier.Private)]
	public DateTime StartTime;
}
