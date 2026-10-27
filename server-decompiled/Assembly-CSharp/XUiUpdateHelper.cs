using System;
using UnityEngine;

// Token: 0x020011A8 RID: 4520
[PublicizedFrom(EAccessModifier.Internal)]
public class XUiUpdateHelper : MonoBehaviour
{
	// Token: 0x06009088 RID: 37000 RVA: 0x00366396 File Offset: 0x00364596
	[PublicizedFrom(EAccessModifier.Private)]
	public void LateUpdate()
	{
		XUiUpdater.Update();
	}
}
