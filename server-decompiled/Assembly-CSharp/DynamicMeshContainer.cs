using System;
using UnityEngine;

// Token: 0x0200037C RID: 892
public abstract class DynamicMeshContainer
{
	// Token: 0x06001A46 RID: 6726 RVA: 0x0009B461 File Offset: 0x00099661
	public string ToDebugLocation()
	{
		return this.WorldPosition.x.ToString() + " " + this.WorldPosition.z.ToString();
	}

	// Token: 0x06001A47 RID: 6727
	public abstract GameObject GetGameObject();

	// Token: 0x06001A48 RID: 6728 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public DynamicMeshContainer()
	{
	}

	// Token: 0x040010CC RID: 4300
	public Vector3i WorldPosition;

	// Token: 0x040010CD RID: 4301
	public long Key;
}
