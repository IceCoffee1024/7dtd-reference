using System;

// Token: 0x02000F97 RID: 3991
public class ToolTipEvent
{
	// Token: 0x17000E95 RID: 3733
	// (get) Token: 0x06007DE0 RID: 32224 RVA: 0x002FD92A File Offset: 0x002FBB2A
	// (set) Token: 0x06007DE1 RID: 32225 RVA: 0x002FD932 File Offset: 0x002FBB32
	public object Parameter { get; set; }

	// Token: 0x140000D6 RID: 214
	// (add) Token: 0x06007DE2 RID: 32226 RVA: 0x002FD93C File Offset: 0x002FBB3C
	// (remove) Token: 0x06007DE3 RID: 32227 RVA: 0x002FD974 File Offset: 0x002FBB74
	public event ToolTipEventHandler EventHandler;

	// Token: 0x06007DE4 RID: 32228 RVA: 0x002FD9A9 File Offset: 0x002FBBA9
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
