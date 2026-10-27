using System;
using UnityEngine;

// Token: 0x0200148D RID: 5261
public class OnActivateItemGameObjectReference : MonoBehaviour
{
	// Token: 0x0600A494 RID: 42132 RVA: 0x003D84D0 File Offset: 0x003D66D0
	public void ActivateItem(bool _activate)
	{
		if (this.ActivateGameObjectTransform != null)
		{
			this.ActivateGameObjectTransform.gameObject.SetActive(_activate);
		}
	}

	// Token: 0x0600A495 RID: 42133 RVA: 0x003D84F1 File Offset: 0x003D66F1
	public bool IsActivated()
	{
		return this.ActivateGameObjectTransform != null && this.ActivateGameObjectTransform.gameObject.activeSelf;
	}

	// Token: 0x04007B42 RID: 31554
	public Transform ActivateGameObjectTransform;
}
