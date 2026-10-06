using System;

// Token: 0x02001096 RID: 4246
public class TimerEventData
{
	// Token: 0x140000DC RID: 220
	// (add) Token: 0x0600869F RID: 34463 RVA: 0x00336EE4 File Offset: 0x003350E4
	// (remove) Token: 0x060086A0 RID: 34464 RVA: 0x00336F1C File Offset: 0x0033511C
	public event TimerEventHandler FullTimeFinishEvent;

	// Token: 0x140000DD RID: 221
	// (add) Token: 0x060086A1 RID: 34465 RVA: 0x00336F54 File Offset: 0x00335154
	// (remove) Token: 0x060086A2 RID: 34466 RVA: 0x00336F8C File Offset: 0x0033518C
	public event TimerEventHandler CloseEvent;

	// Token: 0x140000DE RID: 222
	// (add) Token: 0x060086A3 RID: 34467 RVA: 0x00336FC4 File Offset: 0x003351C4
	// (remove) Token: 0x060086A4 RID: 34468 RVA: 0x00336FFC File Offset: 0x003351FC
	public event TimerEventHandler AlternateEvent;

	// Token: 0x060086A5 RID: 34469 RVA: 0x00337031 File Offset: 0x00335231
	public void HandleFullTimeFinished()
	{
		TimerEventHandler fullTimeFinishEvent = this.FullTimeFinishEvent;
		if (fullTimeFinishEvent == null)
		{
			return;
		}
		fullTimeFinishEvent(this);
	}

	// Token: 0x060086A6 RID: 34470 RVA: 0x00337044 File Offset: 0x00335244
	public void HandleAlternateEvent()
	{
		TimerEventHandler alternateEvent = this.AlternateEvent;
		if (alternateEvent == null)
		{
			return;
		}
		alternateEvent(this);
	}

	// Token: 0x060086A7 RID: 34471 RVA: 0x00337057 File Offset: 0x00335257
	public void HandleClosed()
	{
		TimerEventHandler closeEvent = this.CloseEvent;
		if (closeEvent == null)
		{
			return;
		}
		closeEvent(this);
	}

	// Token: 0x04006417 RID: 25623
	public object Data;

	// Token: 0x0400641B RID: 25627
	public bool CancelWithActivateButton;

	// Token: 0x0400641C RID: 25628
	public bool CloseOnHit;

	// Token: 0x0400641D RID: 25629
	public float AlternateTime = -1f;

	// Token: 0x0400641E RID: 25630
	public float Completion = -1f;
}
