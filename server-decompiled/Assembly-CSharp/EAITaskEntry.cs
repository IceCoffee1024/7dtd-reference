using System;
using UnityEngine.Scripting;

// Token: 0x0200045E RID: 1118
[Preserve]
public class EAITaskEntry
{
	// Token: 0x060021E5 RID: 8677 RVA: 0x000CD880 File Offset: 0x000CBA80
	public EAITaskEntry(int _priority, EAIBase _action)
	{
		this.priority = _priority;
		this.action = _action;
	}

	// Token: 0x0400177B RID: 6011
	public EAIBase action;

	// Token: 0x0400177C RID: 6012
	public int priority;

	// Token: 0x0400177D RID: 6013
	public bool isExecuting;

	// Token: 0x0400177E RID: 6014
	public float executeTime;
}
