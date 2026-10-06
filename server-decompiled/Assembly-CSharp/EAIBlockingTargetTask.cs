using System;
using UnityEngine.Scripting;

// Token: 0x02000437 RID: 1079
[Preserve]
public class EAIBlockingTargetTask : EAIBase
{
	// Token: 0x060020F9 RID: 8441 RVA: 0x000C71B8 File Offset: 0x000C53B8
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity);
		this.MutexBits = 1;
	}

	// Token: 0x060020FA RID: 8442 RVA: 0x000C735A File Offset: 0x000C555A
	public override bool CanExecute()
	{
		return this.canExecute;
	}

	// Token: 0x060020FB RID: 8443 RVA: 0x000C735A File Offset: 0x000C555A
	public override bool Continue()
	{
		return this.canExecute;
	}

	// Token: 0x040016A5 RID: 5797
	public bool canExecute;
}
