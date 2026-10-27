using System;

// Token: 0x020010A6 RID: 4262
public class TimerEventData
{
	// Token: 0x140000DC RID: 220
	// (add) Token: 0x060086F2 RID: 34546 RVA: 0x00338F68 File Offset: 0x00337168
	// (remove) Token: 0x060086F3 RID: 34547 RVA: 0x00338FA0 File Offset: 0x003371A0
	public event TimerEventHandler FullTimeFinishEvent;

	// Token: 0x140000DD RID: 221
	// (add) Token: 0x060086F4 RID: 34548 RVA: 0x00338FD8 File Offset: 0x003371D8
	// (remove) Token: 0x060086F5 RID: 34549 RVA: 0x00339010 File Offset: 0x00337210
	public event TimerEventHandler CloseEvent;

	// Token: 0x140000DE RID: 222
	// (add) Token: 0x060086F6 RID: 34550 RVA: 0x00339048 File Offset: 0x00337248
	// (remove) Token: 0x060086F7 RID: 34551 RVA: 0x00339080 File Offset: 0x00337280
	public event TimerEventHandler AlternateEvent;

	// Token: 0x060086F8 RID: 34552 RVA: 0x003390B5 File Offset: 0x003372B5
	public void HandleFullTimeFinished()
	{
		TimerEventHandler fullTimeFinishEvent = this.FullTimeFinishEvent;
		if (fullTimeFinishEvent == null)
		{
			return;
		}
		fullTimeFinishEvent(this);
	}

	// Token: 0x060086F9 RID: 34553 RVA: 0x003390C8 File Offset: 0x003372C8
	public void HandleAlternateEvent()
	{
		TimerEventHandler alternateEvent = this.AlternateEvent;
		if (alternateEvent == null)
		{
			return;
		}
		alternateEvent(this);
	}

	// Token: 0x060086FA RID: 34554 RVA: 0x003390DB File Offset: 0x003372DB
	public void HandleClosed()
	{
		TimerEventHandler closeEvent = this.CloseEvent;
		if (closeEvent == null)
		{
			return;
		}
		closeEvent(this);
	}

	// Token: 0x04006456 RID: 25686
	public object Data;

	// Token: 0x0400645A RID: 25690
	public bool CancelWithActivateButton;

	// Token: 0x0400645B RID: 25691
	public bool CloseOnHit;

	// Token: 0x0400645C RID: 25692
	public float AlternateTime = -1f;

	// Token: 0x0400645D RID: 25693
	public float Completion = -1f;
}
