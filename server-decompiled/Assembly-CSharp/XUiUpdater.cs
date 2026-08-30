using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001181 RID: 4481
[PublicizedFrom(EAccessModifier.Internal)]
public static class XUiUpdater
{
	// Token: 0x06008FAA RID: 36778 RVA: 0x0035FAE1 File Offset: 0x0035DCE1
	public static void Add(XUi _ui)
	{
		if (!XUiUpdater.uiToUpdate.Contains(_ui))
		{
			XUiUpdater.uiToUpdate.Add(_ui);
		}
	}

	// Token: 0x06008FAB RID: 36779 RVA: 0x0035FAFB File Offset: 0x0035DCFB
	public static void Remove(XUi _ui)
	{
		XUiUpdater.uiToUpdate.Remove(_ui);
	}

	// Token: 0x06008FAC RID: 36780 RVA: 0x0035FB0C File Offset: 0x0035DD0C
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

	// Token: 0x04006923 RID: 26915
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<XUi> uiToUpdate = new List<XUi>();
}
