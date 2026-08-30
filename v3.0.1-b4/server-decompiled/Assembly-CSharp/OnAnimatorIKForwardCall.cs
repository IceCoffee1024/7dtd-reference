using System;
using UnityEngine;

// Token: 0x02001442 RID: 5186
public class OnAnimatorIKForwardCall : MonoBehaviour
{
	// Token: 0x0600A297 RID: 41623 RVA: 0x003CC1BB File Offset: 0x003CA3BB
	public void OnAnimatorIK(int layerIndex)
	{
		if (this.Callback != null)
		{
			this.Callback.OnAnimatorIK(layerIndex);
		}
	}

	// Token: 0x040079AE RID: 31150
	public IOnAnimatorIKCallback Callback;
}
