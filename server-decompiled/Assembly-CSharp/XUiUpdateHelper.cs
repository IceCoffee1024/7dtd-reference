using System;
using UnityEngine;

// Token: 0x02001166 RID: 4454
[PublicizedFrom(EAccessModifier.Internal)]
public class XUiUpdateHelper : MonoBehaviour
{
	// Token: 0x06008EB0 RID: 36528 RVA: 0x0035B03E File Offset: 0x0035923E
	[PublicizedFrom(EAccessModifier.Private)]
	public void LateUpdate()
	{
		XUiUpdater.Update();
	}
}
