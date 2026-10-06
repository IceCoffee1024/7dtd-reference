using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200045A RID: 1114
[Preserve]
public class EAISetNearestEntityAsTargetSorter : IComparer<Entity>
{
	// Token: 0x060021D8 RID: 8664 RVA: 0x000CD311 File Offset: 0x000CB511
	public EAISetNearestEntityAsTargetSorter(Entity _entity)
	{
		this.theEntity = _entity;
	}

	// Token: 0x060021D9 RID: 8665 RVA: 0x000CD320 File Offset: 0x000CB520
	public int Compare(Entity _e, Entity _e2)
	{
		float distanceSq = this.theEntity.GetDistanceSq(_e);
		float distanceSq2 = this.theEntity.GetDistanceSq(_e2);
		if (distanceSq < distanceSq2)
		{
			return -1;
		}
		if (distanceSq > distanceSq2)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x04001767 RID: 5991
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Entity theEntity;
}
