using System;
using System.Collections;
using UnityEngine;

// Token: 0x020012FC RID: 4860
[PublicizedFrom(EAccessModifier.Internal)]
public class CWeightList
{
	// Token: 0x0600997F RID: 39295 RVA: 0x003A5218 File Offset: 0x003A3418
	public CWeightList()
	{
		this.weights = new ArrayList();
	}

	// Token: 0x0400736C RID: 29548
	public Transform transform;

	// Token: 0x0400736D RID: 29549
	public ArrayList weights;
}
