using System;
using System.Collections;
using UnityEngine;

// Token: 0x020012B0 RID: 4784
[PublicizedFrom(EAccessModifier.Internal)]
public class CWeightList
{
	// Token: 0x06009780 RID: 38784 RVA: 0x00399020 File Offset: 0x00397220
	public CWeightList()
	{
		this.weights = new ArrayList();
	}

	// Token: 0x040071D9 RID: 29145
	public Transform transform;

	// Token: 0x040071DA RID: 29146
	public ArrayList weights;
}
