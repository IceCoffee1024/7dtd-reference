using System;
using UnityEngine;

// Token: 0x02001297 RID: 4759
public class NGuiHUDRoot : MonoBehaviour
{
	// Token: 0x06009718 RID: 38680 RVA: 0x0038C41F File Offset: 0x0038A61F
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Awake()
	{
		NGuiHUDRoot.go = base.gameObject;
	}

	// Token: 0x0400710F RID: 28943
	public static GameObject go;
}
