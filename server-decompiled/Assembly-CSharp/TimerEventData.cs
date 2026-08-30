using System;

// Token: 0x0200106D RID: 4205
public class TimerEventData
{
	// Token: 0x140000DC RID: 220
	// (add) Token: 0x0600856B RID: 34155 RVA: 0x0032ED84 File Offset: 0x0032CF84
	// (remove) Token: 0x0600856C RID: 34156 RVA: 0x0032EDBC File Offset: 0x0032CFBC
	public event TimerEventHandler FullTimeFinishEvent;

	// Token: 0x140000DD RID: 221
	// (add) Token: 0x0600856D RID: 34157 RVA: 0x0032EDF4 File Offset: 0x0032CFF4
	// (remove) Token: 0x0600856E RID: 34158 RVA: 0x0032EE2C File Offset: 0x0032D02C
	public event TimerEventHandler CloseEvent;

	// Token: 0x140000DE RID: 222
	// (add) Token: 0x0600856F RID: 34159 RVA: 0x0032EE64 File Offset: 0x0032D064
	// (remove) Token: 0x06008570 RID: 34160 RVA: 0x0032EE9C File Offset: 0x0032D09C
	public event TimerEventHandler AlternateEvent;

	// Token: 0x06008571 RID: 34161 RVA: 0x0032EED1 File Offset: 0x0032D0D1
	public void HandleFullTimeFinished()
	{
		TimerEventHandler fullTimeFinishEvent = this.FullTimeFinishEvent;
		if (fullTimeFinishEvent == null)
		{
			return;
		}
		fullTimeFinishEvent(this);
	}

	// Token: 0x06008572 RID: 34162 RVA: 0x0032EEE4 File Offset: 0x0032D0E4
	public void HandleAlternateEvent()
	{
		TimerEventHandler alternateEvent = this.AlternateEvent;
		if (alternateEvent == null)
		{
			return;
		}
		alternateEvent(this);
	}

	// Token: 0x06008573 RID: 34163 RVA: 0x0032EEF7 File Offset: 0x0032D0F7
	public void HandleClosed()
	{
		TimerEventHandler closeEvent = this.CloseEvent;
		if (closeEvent == null)
		{
			return;
		}
		closeEvent(this);
	}

	// Token: 0x04006310 RID: 25360
	public object Data;

	// Token: 0x04006314 RID: 25364
	public bool CancelWithActivateButton;

	// Token: 0x04006315 RID: 25365
	public bool CloseOnHit;

	// Token: 0x04006316 RID: 25366
	public float AlternateTime = -1f;

	// Token: 0x04006317 RID: 25367
	public float Completion = -1f;
}
