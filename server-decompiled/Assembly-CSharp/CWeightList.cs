using System;
using System.Collections;
using UnityEngine;

// Token: 0x020012EA RID: 4842
[PublicizedFrom(EAccessModifier.Internal)]
public class CWeightList
{
	// Token: 0x06009927 RID: 39207 RVA: 0x003A2E14 File Offset: 0x003A1014
	public CWeightList()
	{
		this.weights = new ArrayList();
	}

	// Token: 0x0400732B RID: 29483
	public Transform transform;

	// Token: 0x0400732C RID: 29484
	public ArrayList weights;
}
