using System;
using UnityEngine;

// Token: 0x02001198 RID: 4504
[PublicizedFrom(EAccessModifier.Internal)]
public class XUiUpdateHelper : MonoBehaviour
{
	// Token: 0x06009034 RID: 36916 RVA: 0x003642A6 File Offset: 0x003624A6
	[PublicizedFrom(EAccessModifier.Private)]
	public void LateUpdate()
	{
		XUiUpdater.Update();
	}
}
