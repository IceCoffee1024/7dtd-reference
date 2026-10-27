using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200124B RID: 4683
public class GUIWindowConsoleComponents : MonoBehaviour
{
	// Token: 0x06009518 RID: 38168 RVA: 0x00386EF5 File Offset: 0x003850F5
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.buttonPrompts = new List<GUIButtonPrompt>(base.GetComponentsInChildren<GUIButtonPrompt>());
	}

	// Token: 0x06009519 RID: 38169 RVA: 0x00386F08 File Offset: 0x00385108
	public void RefreshButtonPrompts()
	{
		foreach (GUIButtonPrompt guibuttonPrompt in this.buttonPrompts)
		{
			guibuttonPrompt.RefreshIcon();
		}
	}

	// Token: 0x04006FB6 RID: 28598
	public ScrollRect scrollRect;

	// Token: 0x04006FB7 RID: 28599
	public Transform contentRect;

	// Token: 0x04006FB8 RID: 28600
	public InputField commandField;

	// Token: 0x04006FB9 RID: 28601
	public Button closeButton;

	// Token: 0x04006FBA RID: 28602
	public Button openLogsButton;

	// Token: 0x04006FBB RID: 28603
	public GameObject controllerPrompts;

	// Token: 0x04006FBC RID: 28604
	public GameObject consoleLinePrefab;

	// Token: 0x04006FBD RID: 28605
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<GUIButtonPrompt> buttonPrompts;
}
