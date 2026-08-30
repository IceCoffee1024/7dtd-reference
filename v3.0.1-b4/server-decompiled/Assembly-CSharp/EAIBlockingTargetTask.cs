using System;
using UnityEngine.Scripting;

// Token: 0x02000431 RID: 1073
[Preserve]
public class EAIBlockingTargetTask : EAIBase
{
	// Token: 0x060020ED RID: 8429 RVA: 0x000C68F8 File Offset: 0x000C4AF8
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity);
		this.MutexBits = 1;
	}

	// Token: 0x060020EE RID: 8430 RVA: 0x000C6A9A File Offset: 0x000C4C9A
	public override bool CanExecute()
	{
		return this.canExecute;
	}

	// Token: 0x060020EF RID: 8431 RVA: 0x000C6A9A File Offset: 0x000C4C9A
	public override bool Continue()
	{
		return this.canExecute;
	}

	// Token: 0x04001699 RID: 5785
	public bool canExecute;
}
