using System;
using UnityEngine;

// Token: 0x020012C1 RID: 4801
public class NGuiHUDRoot : MonoBehaviour
{
	// Token: 0x060097FD RID: 38909 RVA: 0x0039306F File Offset: 0x0039126F
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Awake()
	{
		NGuiHUDRoot.go = base.gameObject;
	}

	// Token: 0x040071BA RID: 29114
	public static GameObject go;
}
