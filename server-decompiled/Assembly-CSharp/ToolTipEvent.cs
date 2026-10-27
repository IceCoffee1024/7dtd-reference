using System;

// Token: 0x02000FCF RID: 4047
public class ToolTipEvent
{
	// Token: 0x17000F00 RID: 3840
	// (get) Token: 0x06007F30 RID: 32560 RVA: 0x00306BF2 File Offset: 0x00304DF2
	// (set) Token: 0x06007F31 RID: 32561 RVA: 0x00306BFA File Offset: 0x00304DFA
	public object Parameter { get; set; }

	// Token: 0x140000D6 RID: 214
	// (add) Token: 0x06007F32 RID: 32562 RVA: 0x00306C04 File Offset: 0x00304E04
	// (remove) Token: 0x06007F33 RID: 32563 RVA: 0x00306C3C File Offset: 0x00304E3C
	public event ToolTipEventHandler EventHandler;

	// Token: 0x06007F34 RID: 32564 RVA: 0x00306C71 File Offset: 0x00304E71
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
