using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001199 RID: 4505
[PublicizedFrom(EAccessModifier.Internal)]
public static class XUiUpdater
{
	// Token: 0x06009036 RID: 36918 RVA: 0x003642AD File Offset: 0x003624AD
	public static void Add(XUi _ui)
	{
		if (!XUiUpdater.uiToUpdate.Contains(_ui))
		{
			XUiUpdater.uiToUpdate.Add(_ui);
		}
	}

	// Token: 0x06009037 RID: 36919 RVA: 0x003642C7 File Offset: 0x003624C7
	public static void Remove(XUi _ui)
	{
		XUiUpdater.uiToUpdate.Remove(_ui);
	}

	// Token: 0x06009038 RID: 36920 RVA: 0x003642D8 File Offset: 0x003624D8
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

	// Token: 0x04006989 RID: 27017
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<XUi> uiToUpdate = new List<XUi>();
}
