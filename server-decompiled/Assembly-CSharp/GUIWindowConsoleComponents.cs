using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001205 RID: 4613
public class GUIWindowConsoleComponents : MonoBehaviour
{
	// Token: 0x0600932F RID: 37679 RVA: 0x0037AFD5 File Offset: 0x003791D5
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.buttonPrompts = new List<GUIButtonPrompt>(base.GetComponentsInChildren<GUIButtonPrompt>());
	}

	// Token: 0x06009330 RID: 37680 RVA: 0x0037AFE8 File Offset: 0x003791E8
	public void RefreshButtonPrompts()
	{
		foreach (GUIButtonPrompt guibuttonPrompt in this.buttonPrompts)
		{
			guibuttonPrompt.RefreshIcon();
		}
	}

	// Token: 0x04006E32 RID: 28210
	public ScrollRect scrollRect;

	// Token: 0x04006E33 RID: 28211
	public Transform contentRect;

	// Token: 0x04006E34 RID: 28212
	public InputField commandField;

	// Token: 0x04006E35 RID: 28213
	public Button closeButton;

	// Token: 0x04006E36 RID: 28214
	public Button openLogsButton;

	// Token: 0x04006E37 RID: 28215
	public GameObject controllerPrompts;

	// Token: 0x04006E38 RID: 28216
	public GameObject consoleLinePrefab;

	// Token: 0x04006E39 RID: 28217
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<GUIButtonPrompt> buttonPrompts;
}
