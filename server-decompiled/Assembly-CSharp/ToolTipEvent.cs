using System;

// Token: 0x02000FB0 RID: 4016
public class ToolTipEvent
{
	// Token: 0x17000ECE RID: 3790
	// (get) Token: 0x06007ED2 RID: 32466 RVA: 0x003020FE File Offset: 0x003002FE
	// (set) Token: 0x06007ED3 RID: 32467 RVA: 0x00302106 File Offset: 0x00300306
	public object Parameter { get; set; }

	// Token: 0x140000D7 RID: 215
	// (add) Token: 0x06007ED4 RID: 32468 RVA: 0x00302110 File Offset: 0x00300310
	// (remove) Token: 0x06007ED5 RID: 32469 RVA: 0x00302148 File Offset: 0x00300348
	public event ToolTipEventHandler EventHandler;

	// Token: 0x06007ED6 RID: 32470 RVA: 0x0030217D File Offset: 0x0030037D
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
