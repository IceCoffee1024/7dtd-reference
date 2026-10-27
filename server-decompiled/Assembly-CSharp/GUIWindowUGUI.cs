using System;
using UnityEngine;

// Token: 0x02001252 RID: 4690
public abstract class GUIWindowUGUI : GUIWindow
{
	// Token: 0x170011ED RID: 4589
	// (get) Token: 0x0600955C RID: 38236
	public abstract string UIPrefabPath { get; }

	// Token: 0x0600955D RID: 38237 RVA: 0x00388A94 File Offset: 0x00386C94
	public GUIWindowUGUI(string _id)
		: base(_id)
	{
		this.uiPrefab = DataLoader.LoadAsset<GameObject>(this.UIPrefabPath, false);
		this.canvas = global::UnityEngine.Object.Instantiate<GameObject>(this.uiPrefab).GetComponent<Canvas>();
		this.canvas.gameObject.SetActive(false);
	}

	// Token: 0x0600955E RID: 38238 RVA: 0x00388AE1 File Offset: 0x00386CE1
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

	// Token: 0x0600955F RID: 38239 RVA: 0x00388B09 File Offset: 0x00386D09
	public override void Update()
	{
		if (this.shouldOpen && !this.canvas.gameObject.activeSelf)
		{
			this.canvas.gameObject.SetActive(true);
			this.shouldOpen = false;
		}
	}

	// Token: 0x06009560 RID: 38240 RVA: 0x00388B3D File Offset: 0x00386D3D
	public override void OnClose()
	{
		base.OnClose();
		this.canvas.gameObject.SetActive(false);
	}

	// Token: 0x06009561 RID: 38241 RVA: 0x00388B56 File Offset: 0x00386D56
	public override void Cleanup()
	{
		global::UnityEngine.Object.Destroy(this.canvas.gameObject);
		this.uiPrefab = null;
	}

	// Token: 0x04006FE6 RID: 28646
	[PublicizedFrom(EAccessModifier.Protected)]
	public GameObject uiPrefab;

	// Token: 0x04006FE7 RID: 28647
	[PublicizedFrom(EAccessModifier.Protected)]
	public Canvas canvas;

	// Token: 0x04006FE8 RID: 28648
	[PublicizedFrom(EAccessModifier.Private)]
	public bool shouldOpen;
}
