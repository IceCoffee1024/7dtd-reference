using System;
using UnityEngine;

// Token: 0x02001228 RID: 4648
public abstract class GUIWindowUGUI : GUIWindow
{
	// Token: 0x1700119E RID: 4510
	// (get) Token: 0x06009477 RID: 38007
	public abstract string UIPrefabPath { get; }

	// Token: 0x06009478 RID: 38008 RVA: 0x00381E50 File Offset: 0x00380050
	public GUIWindowUGUI(string _id)
		: base(_id)
	{
		this.uiPrefab = DataLoader.LoadAsset<GameObject>(this.UIPrefabPath, false);
		this.canvas = global::UnityEngine.Object.Instantiate<GameObject>(this.uiPrefab).GetComponent<Canvas>();
		this.canvas.gameObject.SetActive(false);
	}

	// Token: 0x06009479 RID: 38009 RVA: 0x00381E9D File Offset: 0x0038009D
	public override void OnOpen()
	{
		base.OnOpen();
		if (ThreadManager.IsMainThread())
		{
			this.canvas.gameObject.SetActive(true);
			return;
		}
		this.shouldOpen = true;
	}

	// Token: 0x0600947A RID: 38010 RVA: 0x00381EC5 File Offset: 0x003800C5
	public override void Update()
	{
		if (this.shouldOpen && !this.canvas.gameObject.activeSelf)
		{
			this.canvas.gameObject.SetActive(true);
			this.shouldOpen = false;
		}
	}

	// Token: 0x0600947B RID: 38011 RVA: 0x00381EF9 File Offset: 0x003800F9
	public override void OnClose()
	{
		base.OnClose();
		this.canvas.gameObject.SetActive(false);
	}

	// Token: 0x0600947C RID: 38012 RVA: 0x00381F12 File Offset: 0x00380112
	public override void Cleanup()
	{
		global::UnityEngine.Object.Destroy(this.canvas.gameObject);
		this.uiPrefab = null;
	}

	// Token: 0x04006F3B RID: 28475
	[PublicizedFrom(EAccessModifier.Protected)]
	public GameObject uiPrefab;

	// Token: 0x04006F3C RID: 28476
	[PublicizedFrom(EAccessModifier.Protected)]
	public Canvas canvas;

	// Token: 0x04006F3D RID: 28477
	[PublicizedFrom(EAccessModifier.Private)]
	public bool shouldOpen;
}
