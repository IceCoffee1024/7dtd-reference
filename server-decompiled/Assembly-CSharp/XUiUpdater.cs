using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020011A9 RID: 4521
[PublicizedFrom(EAccessModifier.Internal)]
public static class XUiUpdater
{
	// Token: 0x0600908A RID: 37002 RVA: 0x0036639D File Offset: 0x0036459D
	public static void Add(XUi _ui)
	{
		if (!XUiUpdater.uiToUpdate.Contains(_ui))
		{
			XUiUpdater.uiToUpdate.Add(_ui);
		}
	}

	// Token: 0x0600908B RID: 37003 RVA: 0x003663B7 File Offset: 0x003645B7
	public static void Remove(XUi _ui)
	{
		XUiUpdater.uiToUpdate.Remove(_ui);
	}

	// Token: 0x0600908C RID: 37004 RVA: 0x003663C8 File Offset: 0x003645C8
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

	// Token: 0x040069C9 RID: 27081
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<XUi> uiToUpdate = new List<XUi>();
}
