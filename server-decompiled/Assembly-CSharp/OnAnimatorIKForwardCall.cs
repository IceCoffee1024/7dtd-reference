using System;
using UnityEngine;

// Token: 0x0200147D RID: 5245
public class OnAnimatorIKForwardCall : MonoBehaviour
{
	// Token: 0x0600A440 RID: 42048 RVA: 0x003D610F File Offset: 0x003D430F
	public void OnAnimatorIK(int layerIndex)
	{
		if (this.Callback != null)
		{
			this.Callback.OnAnimatorIK(layerIndex);
		}
	}

	// Token: 0x04007B02 RID: 31490
	public IOnAnimatorIKCallback Callback;
}
