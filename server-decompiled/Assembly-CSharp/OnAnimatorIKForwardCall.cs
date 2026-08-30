using System;
using UnityEngine;

// Token: 0x0200145D RID: 5213
public class OnAnimatorIKForwardCall : MonoBehaviour
{
	// Token: 0x0600A394 RID: 41876 RVA: 0x003D14FF File Offset: 0x003CF6FF
	public void OnAnimatorIK(int layerIndex)
	{
		if (this.Callback != null)
		{
			this.Callback.OnAnimatorIK(layerIndex);
		}
	}

	// Token: 0x04007A87 RID: 31367
	public IOnAnimatorIKCallback Callback;
}
