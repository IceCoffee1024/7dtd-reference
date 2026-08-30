using System;
using UnityEngine;

// Token: 0x020012AF RID: 4783
[PublicizedFrom(EAccessModifier.Internal)]
public class CVertexWeight
{
	// Token: 0x0600977F RID: 38783 RVA: 0x00399003 File Offset: 0x00397203
	public CVertexWeight(int i, Vector3 p, float w)
	{
		this.index = i;
		this.localPosition = p;
		this.weight = w;
	}

	// Token: 0x040071D6 RID: 29142
	public int index;

	// Token: 0x040071D7 RID: 29143
	public Vector3 localPosition;

	// Token: 0x040071D8 RID: 29144
	public float weight;
}
