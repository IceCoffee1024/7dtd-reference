using System;
using UnityEngine;

// Token: 0x020012AF RID: 4783
public class NGuiHUDRoot : MonoBehaviour
{
	// Token: 0x060097A5 RID: 38821 RVA: 0x00390C67 File Offset: 0x0038EE67
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Awake()
	{
		NGuiHUDRoot.go = base.gameObject;
	}

	// Token: 0x04007179 RID: 29049
	public static GameObject go;
}
