using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200045C RID: 1116
[Preserve]
public class EAISetNearestEntityAsTargetSorter : IComparer<Entity>
{
	// Token: 0x060021DD RID: 8669 RVA: 0x000CD6BD File Offset: 0x000CB8BD
	public EAISetNearestEntityAsTargetSorter(Entity _entity)
	{
		this.theEntity = _entity;
	}

	// Token: 0x060021DE RID: 8670 RVA: 0x000CD6CC File Offset: 0x000CB8CC
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

	// Token: 0x04001777 RID: 6007
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Entity theEntity;
}
