using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001239 RID: 4665
public class GUIWindowConsoleComponents : MonoBehaviour
{
	// Token: 0x060094C1 RID: 38081 RVA: 0x00384AF9 File Offset: 0x00382CF9
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.buttonPrompts = new List<GUIButtonPrompt>(base.GetComponentsInChildren<GUIButtonPrompt>());
	}

	// Token: 0x060094C2 RID: 38082 RVA: 0x00384B0C File Offset: 0x00382D0C
	public void RefreshButtonPrompts()
	{
		foreach (GUIButtonPrompt guibuttonPrompt in this.buttonPrompts)
		{
			guibuttonPrompt.RefreshIcon();
		}
	}

	// Token: 0x04006F75 RID: 28533
	public ScrollRect scrollRect;

	// Token: 0x04006F76 RID: 28534
	public Transform contentRect;

	// Token: 0x04006F77 RID: 28535
	public InputField commandField;

	// Token: 0x04006F78 RID: 28536
	public Button closeButton;

	// Token: 0x04006F79 RID: 28537
	public Button openLogsButton;

	// Token: 0x04006F7A RID: 28538
	public GameObject controllerPrompts;

	// Token: 0x04006F7B RID: 28539
	public GameObject consoleLinePrefab;

	// Token: 0x04006F7C RID: 28540
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<GUIButtonPrompt> buttonPrompts;
}
