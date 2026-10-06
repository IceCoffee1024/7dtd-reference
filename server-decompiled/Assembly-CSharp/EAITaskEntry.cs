using System;
using UnityEngine.Scripting;

// Token: 0x0200045C RID: 1116
[Preserve]
public class EAITaskEntry
{
	// Token: 0x060021E0 RID: 8672 RVA: 0x000CD4D4 File Offset: 0x000CB6D4
	public EAITaskEntry(int _priority, EAIBase _action)
	{
		this.priority = _priority;
		this.action = _action;
	}

	// Token: 0x0400176B RID: 5995
	public EAIBase action;

	// Token: 0x0400176C RID: 5996
	public int priority;

	// Token: 0x0400176D RID: 5997
	public bool isExecuting;

	// Token: 0x0400176E RID: 5998
	public float executeTime;
}
