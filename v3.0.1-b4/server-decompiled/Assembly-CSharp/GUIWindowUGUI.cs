using System;
using UnityEngine;

// Token: 0x0200120C RID: 4620
public abstract class GUIWindowUGUI : GUIWindow
{
	// Token: 0x17001163 RID: 4451
	// (get) Token: 0x06009373 RID: 37747
	public abstract string UIPrefabPath { get; }

	// Token: 0x06009374 RID: 37748 RVA: 0x0037CB74 File Offset: 0x0037AD74
	public GUIWindowUGUI(string _id)
		: base(_id)
	{
		this.uiPrefab = DataLoader.LoadAsset<GameObject>(this.UIPrefabPath, false);
		this.canvas = global::UnityEngine.Object.Instantiate<GameObject>(this.uiPrefab).GetComponent<Canvas>();
		this.canvas.gameObject.SetActive(false);
	}

	// Token: 0x06009375 RID: 37749 RVA: 0x0037CBC1 File Offset: 0x0037ADC1
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

	// Token: 0x06009376 RID: 37750 RVA: 0x0037CBE9 File Offset: 0x0037ADE9
	public override void Update()
	{
		if (this.shouldOpen && !this.canvas.gameObject.activeSelf)
		{
			this.canvas.gameObject.SetActive(true);
			this.shouldOpen = false;
		}
	}

	// Token: 0x06009377 RID: 37751 RVA: 0x0037CC1D File Offset: 0x0037AE1D
	public override void OnClose()
	{
		base.OnClose();
		this.canvas.gameObject.SetActive(false);
	}

	// Token: 0x06009378 RID: 37752 RVA: 0x0037CC36 File Offset: 0x0037AE36
	public override void Cleanup()
	{
		global::UnityEngine.Object.Destroy(this.canvas.gameObject);
		this.uiPrefab = null;
	}

	// Token: 0x04006E62 RID: 28258
	[PublicizedFrom(EAccessModifier.Protected)]
	public GameObject uiPrefab;

	// Token: 0x04006E63 RID: 28259
	[PublicizedFrom(EAccessModifier.Protected)]
	public Canvas canvas;

	// Token: 0x04006E64 RID: 28260
	[PublicizedFrom(EAccessModifier.Private)]
	public bool shouldOpen;
}
