using System;
using UnityEngine;

// Token: 0x020012FB RID: 4859
[PublicizedFrom(EAccessModifier.Internal)]
public class CVertexWeight
{
	// Token: 0x0600997E RID: 39294 RVA: 0x003A51FB File Offset: 0x003A33FB
	public CVertexWeight(int i, Vector3 p, float w)
	{
		this.index = i;
		this.localPosition = p;
		this.weight = w;
	}

	// Token: 0x04007369 RID: 29545
	public int index;

	// Token: 0x0400736A RID: 29546
	public Vector3 localPosition;

	// Token: 0x0400736B RID: 29547
	public float weight;
}
