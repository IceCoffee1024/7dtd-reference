using System;
using UnityEngine.Scripting;

// Token: 0x02000439 RID: 1081
[Preserve]
public class EAIBlockingTargetTask : EAIBase
{
	// Token: 0x060020FE RID: 8446 RVA: 0x000C7564 File Offset: 0x000C5764
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity);
		this.MutexBits = 1;
	}

	// Token: 0x060020FF RID: 8447 RVA: 0x000C7706 File Offset: 0x000C5906
	public override bool CanExecute()
	{
		return this.canExecute;
	}

	// Token: 0x06002100 RID: 8448 RVA: 0x000C7706 File Offset: 0x000C5906
	public override bool Continue()
	{
		return this.canExecute;
	}

	// Token: 0x040016B5 RID: 5813
	public bool canExecute;
}
