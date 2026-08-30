using System;
using UnityEngine;

// Token: 0x0200005A RID: 90
public class Tutorial5 : MonoBehaviour
{
	// Token: 0x060001CB RID: 459 RVA: 0x000109A4 File Offset: 0x0000EBA4
	public void SetDurationToCurrentProgress()
	{
		UITweener[] componentsInChildren = base.GetComponentsInChildren<UITweener>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].duration = Mathf.Lerp(2f, 0.5f, UIProgressBar.current.value);
		}
	}
}
