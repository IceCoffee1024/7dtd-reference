using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000454 RID: 1108
[Preserve]
public class EAISetNearestEntityAsTargetSorter : IComparer<Entity>
{
	// Token: 0x060021CC RID: 8652 RVA: 0x000CCAC1 File Offset: 0x000CACC1
	public EAISetNearestEntityAsTargetSorter(Entity _entity)
	{
		this.theEntity = _entity;
	}

	// Token: 0x060021CD RID: 8653 RVA: 0x000CCAD0 File Offset: 0x000CACD0
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

	// Token: 0x0400175A RID: 5978
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Entity theEntity;
}
