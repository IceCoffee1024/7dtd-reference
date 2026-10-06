using System;

// Token: 0x02000FBF RID: 4031
public class ToolTipEvent
{
	// Token: 0x17000EF5 RID: 3829
	// (get) Token: 0x06007EE1 RID: 32481 RVA: 0x003050D6 File Offset: 0x003032D6
	// (set) Token: 0x06007EE2 RID: 32482 RVA: 0x003050DE File Offset: 0x003032DE
	public object Parameter { get; set; }

	// Token: 0x140000D6 RID: 214
	// (add) Token: 0x06007EE3 RID: 32483 RVA: 0x003050E8 File Offset: 0x003032E8
	// (remove) Token: 0x06007EE4 RID: 32484 RVA: 0x00305120 File Offset: 0x00303320
	public event ToolTipEventHandler EventHandler;

	// Token: 0x06007EE5 RID: 32485 RVA: 0x00305155 File Offset: 0x00303355
	public void HandleEvent()
	{
		ToolTipEventHandler eventHandler = this.EventHandler;
		if (eventHandler == null)
		{
			return;
		}
		eventHandler(this.Parameter);
	}
}
