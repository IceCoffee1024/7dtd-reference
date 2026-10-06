using System;
using UnityEngine;

// Token: 0x0200037A RID: 890
public abstract class DynamicMeshContainer
{
	// Token: 0x06001A42 RID: 6722 RVA: 0x0009B22D File Offset: 0x0009942D
	public string ToDebugLocation()
	{
		return this.WorldPosition.x.ToString() + " " + this.WorldPosition.z.ToString();
	}

	// Token: 0x06001A43 RID: 6723
	public abstract GameObject GetGameObject();

	// Token: 0x06001A44 RID: 6724 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public DynamicMeshContainer()
	{
	}

	// Token: 0x040010BC RID: 4284
	public Vector3i WorldPosition;

	// Token: 0x040010BD RID: 4285
	public long Key;
}
