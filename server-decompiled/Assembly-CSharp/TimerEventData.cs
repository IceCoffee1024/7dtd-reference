using System;

// Token: 0x02001086 RID: 4230
public class TimerEventData
{
	// Token: 0x140000DD RID: 221
	// (add) Token: 0x0600865D RID: 34397 RVA: 0x003335AC File Offset: 0x003317AC
	// (remove) Token: 0x0600865E RID: 34398 RVA: 0x003335E4 File Offset: 0x003317E4
	public event TimerEventHandler FullTimeFinishEvent;

	// Token: 0x140000DE RID: 222
	// (add) Token: 0x0600865F RID: 34399 RVA: 0x0033361C File Offset: 0x0033181C
	// (remove) Token: 0x06008660 RID: 34400 RVA: 0x00333654 File Offset: 0x00331854
	public event TimerEventHandler CloseEvent;

	// Token: 0x140000DF RID: 223
	// (add) Token: 0x06008661 RID: 34401 RVA: 0x0033368C File Offset: 0x0033188C
	// (remove) Token: 0x06008662 RID: 34402 RVA: 0x003336C4 File Offset: 0x003318C4
	public event TimerEventHandler AlternateEvent;

	// Token: 0x06008663 RID: 34403 RVA: 0x003336F9 File Offset: 0x003318F9
	public void HandleFullTimeFinished()
	{
		TimerEventHandler fullTimeFinishEvent = this.FullTimeFinishEvent;
		if (fullTimeFinishEvent == null)
		{
			return;
		}
		fullTimeFinishEvent(this);
	}

	// Token: 0x06008664 RID: 34404 RVA: 0x0033370C File Offset: 0x0033190C
	public void HandleAlternateEvent()
	{
		TimerEventHandler alternateEvent = this.AlternateEvent;
		if (alternateEvent == null)
		{
			return;
		}
		alternateEvent(this);
	}

	// Token: 0x06008665 RID: 34405 RVA: 0x0033371F File Offset: 0x0033191F
	public void HandleClosed()
	{
		TimerEventHandler closeEvent = this.CloseEvent;
		if (closeEvent == null)
		{
			return;
		}
		closeEvent(this);
	}

	// Token: 0x040063D7 RID: 25559
	public object Data;

	// Token: 0x040063DB RID: 25563
	public bool CancelWithActivateButton;

	// Token: 0x040063DC RID: 25564
	public bool CloseOnHit;

	// Token: 0x040063DD RID: 25565
	public float AlternateTime = -1f;

	// Token: 0x040063DE RID: 25566
	public float Completion = -1f;
}
