using System;
using UnityEngine;

// Token: 0x0200148F RID: 5263
public class OnAnimatorIKForwardCall : MonoBehaviour
{
	// Token: 0x0600A498 RID: 42136 RVA: 0x003D8513 File Offset: 0x003D6713
	public void OnAnimatorIK(int layerIndex)
	{
		if (this.Callback != null)
		{
			this.Callback.OnAnimatorIK(layerIndex);
		}
	}

	// Token: 0x04007B43 RID: 31555
	public IOnAnimatorIKCallback Callback;
}
