using System;
using UnityEngine;

// Token: 0x0200127D RID: 4733
public class NGuiHUDRoot : MonoBehaviour
{
	// Token: 0x0600961D RID: 38429 RVA: 0x003872C3 File Offset: 0x003854C3
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Awake()
	{
		NGuiHUDRoot.go = base.gameObject;
	}

	// Token: 0x0400703A RID: 28730
	public static GameObject go;
}
