using System;
using UnityEngine;

// Token: 0x02000373 RID: 883
public abstract class DynamicMeshContainer
{
	// Token: 0x06001A2A RID: 6698 RVA: 0x0009A86D File Offset: 0x00098A6D
	public string ToDebugLocation()
	{
		return this.WorldPosition.x.ToString() + " " + this.WorldPosition.z.ToString();
	}

	// Token: 0x06001A2B RID: 6699
	public abstract GameObject GetGameObject();

	// Token: 0x06001A2C RID: 6700 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public DynamicMeshContainer()
	{
	}

	// Token: 0x040010AE RID: 4270
	public Vector3i WorldPosition;

	// Token: 0x040010AF RID: 4271
	public long Key;
}
