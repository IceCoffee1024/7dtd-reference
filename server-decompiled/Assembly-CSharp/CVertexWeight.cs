using System;
using UnityEngine;

// Token: 0x020012E9 RID: 4841
[PublicizedFrom(EAccessModifier.Internal)]
public class CVertexWeight
{
	// Token: 0x06009926 RID: 39206 RVA: 0x003A2DF7 File Offset: 0x003A0FF7
	public CVertexWeight(int i, Vector3 p, float w)
	{
		this.index = i;
		this.localPosition = p;
		this.weight = w;
	}

	// Token: 0x04007328 RID: 29480
	public int index;

	// Token: 0x04007329 RID: 29481
	public Vector3 localPosition;

	// Token: 0x0400732A RID: 29482
	public float weight;
}
