using System;
using UnityEngine;

// Token: 0x02000078 RID: 120
public class ScreenshotData : MonoBehaviour
{
	// Token: 0x06000243 RID: 579 RVA: 0x000027FC File Offset: 0x000009FC
	public void Start()
	{
	}

	// Token: 0x06000244 RID: 580 RVA: 0x000027FC File Offset: 0x000009FC
	public void Update()
	{
	}

	// Token: 0x06000245 RID: 581 RVA: 0x00012CE4 File Offset: 0x00010EE4
	public void OnGUI()
	{
		if (GameManager.Instance == null || GameManager.Instance.World == null || GameManager.Instance.World.GetPrimaryPlayer() == null)
		{
			return;
		}
		LocalPlayerUI uiforPrimaryPlayer = LocalPlayerUI.GetUIForPrimaryPlayer();
		if (uiforPrimaryPlayer == null || !uiforPrimaryPlayer.windowManager.IsHUDEnabled() || GameManager.Instance.ShowBackground())
		{
			return;
		}
		GUI.Label(new Rect(10f, 10f, 200f, 20f), GameManager.Instance.backgroundColor.ToCultureInvariantString());
	}
}
