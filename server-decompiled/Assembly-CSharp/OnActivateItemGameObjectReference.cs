using System;
using UnityEngine;

// Token: 0x0200145B RID: 5211
public class OnActivateItemGameObjectReference : MonoBehaviour
{
	// Token: 0x0600A390 RID: 41872 RVA: 0x003D14BC File Offset: 0x003CF6BC
	public void ActivateItem(bool _activate)
	{
		if (this.ActivateGameObjectTransform != null)
		{
			this.ActivateGameObjectTransform.gameObject.SetActive(_activate);
		}
	}

	// Token: 0x0600A391 RID: 41873 RVA: 0x003D14DD File Offset: 0x003CF6DD
	public bool IsActivated()
	{
		return this.ActivateGameObjectTransform != null && this.ActivateGameObjectTransform.gameObject.activeSelf;
	}

	// Token: 0x04007A86 RID: 31366
	public Transform ActivateGameObjectTransform;
}
