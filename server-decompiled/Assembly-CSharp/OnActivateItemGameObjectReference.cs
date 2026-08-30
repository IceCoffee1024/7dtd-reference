using System;
using UnityEngine;

// Token: 0x02001440 RID: 5184
public class OnActivateItemGameObjectReference : MonoBehaviour
{
	// Token: 0x0600A293 RID: 41619 RVA: 0x003CC178 File Offset: 0x003CA378
	public void ActivateItem(bool _activate)
	{
		if (this.ActivateGameObjectTransform != null)
		{
			this.ActivateGameObjectTransform.gameObject.SetActive(_activate);
		}
	}

	// Token: 0x0600A294 RID: 41620 RVA: 0x003CC199 File Offset: 0x003CA399
	public bool IsActivated()
	{
		return this.ActivateGameObjectTransform != null && this.ActivateGameObjectTransform.gameObject.activeSelf;
	}

	// Token: 0x040079AD RID: 31149
	public Transform ActivateGameObjectTransform;
}
