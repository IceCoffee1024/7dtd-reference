using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001167 RID: 4455
[PublicizedFrom(EAccessModifier.Internal)]
public static class XUiUpdater
{
	// Token: 0x06008EB2 RID: 36530 RVA: 0x0035B045 File Offset: 0x00359245
	public static void Add(XUi _ui)
	{
		if (!XUiUpdater.uiToUpdate.Contains(_ui))
		{
			XUiUpdater.uiToUpdate.Add(_ui);
		}
	}

	// Token: 0x06008EB3 RID: 36531 RVA: 0x0035B05F File Offset: 0x0035925F
	public static void Remove(XUi _ui)
	{
		XUiUpdater.uiToUpdate.Remove(_ui);
	}

	// Token: 0x06008EB4 RID: 36532 RVA: 0x0035B070 File Offset: 0x00359270
	public static void Update()
	{
		for (int i = 0; i < XUiUpdater.uiToUpdate.Count; i++)
		{
			if (XUiUpdater.uiToUpdate[i] != null)
			{
				XUiUpdater.uiToUpdate[i].OnUpdateDeltaTime(Time.deltaTime);
			}
		}
	}

	// Token: 0x04006854 RID: 26708
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<XUi> uiToUpdate = new List<XUi>();
}
