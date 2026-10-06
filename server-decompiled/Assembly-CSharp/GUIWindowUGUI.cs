using System;
using UnityEngine;

// Token: 0x02001240 RID: 4672
public abstract class GUIWindowUGUI : GUIWindow
{
	// Token: 0x170011E2 RID: 4578
	// (get) Token: 0x06009505 RID: 38149
	public abstract string UIPrefabPath { get; }

	// Token: 0x06009506 RID: 38150 RVA: 0x00386698 File Offset: 0x00384898
	public GUIWindowUGUI(string _id)
		: base(_id)
	{
		this.uiPrefab = DataLoader.LoadAsset<GameObject>(this.UIPrefabPath, false);
		this.canvas = global::UnityEngine.Object.Instantiate<GameObject>(this.uiPrefab).GetComponent<Canvas>();
		this.canvas.gameObject.SetActive(false);
	}

	// Token: 0x06009507 RID: 38151 RVA: 0x003866E5 File Offset: 0x003848E5
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

	// Token: 0x06009508 RID: 38152 RVA: 0x0038670D File Offset: 0x0038490D
	public override void Update()
	{
		if (this.shouldOpen && !this.canvas.gameObject.activeSelf)
		{
			this.canvas.gameObject.SetActive(true);
			this.shouldOpen = false;
		}
	}

	// Token: 0x06009509 RID: 38153 RVA: 0x00386741 File Offset: 0x00384941
	public override void OnClose()
	{
		base.OnClose();
		this.canvas.gameObject.SetActive(false);
	}

	// Token: 0x0600950A RID: 38154 RVA: 0x0038675A File Offset: 0x0038495A
	public override void Cleanup()
	{
		global::UnityEngine.Object.Destroy(this.canvas.gameObject);
		this.uiPrefab = null;
	}

	// Token: 0x04006FA5 RID: 28581
	[PublicizedFrom(EAccessModifier.Protected)]
	public GameObject uiPrefab;

	// Token: 0x04006FA6 RID: 28582
	[PublicizedFrom(EAccessModifier.Protected)]
	public Canvas canvas;

	// Token: 0x04006FA7 RID: 28583
	[PublicizedFrom(EAccessModifier.Private)]
	public bool shouldOpen;
}
