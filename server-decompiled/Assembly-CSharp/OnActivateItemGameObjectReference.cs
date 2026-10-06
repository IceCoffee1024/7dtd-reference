using System;
using UnityEngine;

// Token: 0x0200147B RID: 5243
public class OnActivateItemGameObjectReference : MonoBehaviour
{
	// Token: 0x0600A43C RID: 42044 RVA: 0x003D60CC File Offset: 0x003D42CC
	public void ActivateItem(bool _activate)
	{
		if (this.ActivateGameObjectTransform != null)
		{
			this.ActivateGameObjectTransform.gameObject.SetActive(_activate);
		}
	}

	// Token: 0x0600A43D RID: 42045 RVA: 0x003D60ED File Offset: 0x003D42ED
	public bool IsActivated()
	{
		return this.ActivateGameObjectTransform != null && this.ActivateGameObjectTransform.gameObject.activeSelf;
	}

	// Token: 0x04007B01 RID: 31489
	public Transform ActivateGameObjectTransform;
}
