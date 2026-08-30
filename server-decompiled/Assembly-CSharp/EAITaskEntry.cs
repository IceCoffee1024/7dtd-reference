using System;
using UnityEngine.Scripting;

// Token: 0x02000456 RID: 1110
[Preserve]
public class EAITaskEntry
{
	// Token: 0x060021D4 RID: 8660 RVA: 0x000CCC84 File Offset: 0x000CAE84
	public EAITaskEntry(int _priority, EAIBase _action)
	{
		this.priority = _priority;
		this.action = _action;
	}

	// Token: 0x0400175E RID: 5982
	public EAIBase action;

	// Token: 0x0400175F RID: 5983
	public int priority;

	// Token: 0x04001760 RID: 5984
	public bool isExecuting;

	// Token: 0x04001761 RID: 5985
	public float executeTime;
}
