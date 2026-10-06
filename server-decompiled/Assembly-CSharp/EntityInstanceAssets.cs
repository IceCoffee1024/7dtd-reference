using System;
using UnityEngine;

// Token: 0x020004BE RID: 1214
public class EntityInstanceAssets
{
	// Token: 0x1700043C RID: 1084
	// (get) Token: 0x06002641 RID: 9793 RVA: 0x000EA4A9 File Offset: 0x000E86A9
	// (set) Token: 0x06002642 RID: 9794 RVA: 0x000EA4B1 File Offset: 0x000E86B1
	public Transform PrefabT
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700043D RID: 1085
	// (get) Token: 0x06002643 RID: 9795 RVA: 0x000EA4BA File Offset: 0x000E86BA
	public bool IsLoadComplete
	{
		get
		{
			return this.prefabHandle.IsDone;
		}
	}

	// Token: 0x1700043E RID: 1086
	// (get) Token: 0x06002644 RID: 9796 RVA: 0x000EA4C7 File Offset: 0x000E86C7
	public bool IsLoadSuccessful
	{
		get
		{
			return this.IsLoadComplete && !(this.PrefabT == null);
		}
	}

	// Token: 0x06002645 RID: 9797 RVA: 0x000EA4E4 File Offset: 0x000E86E4
	public void Load(bool _loadSync, EntityClass _ec, bool isLocalPlayer)
	{
		if (isLocalPlayer)
		{
			this.prefabHandle = LoadManager.LoadAsset<GameObject>("Prefabs/prefabEntityPlayerLocal", null, null, false, true, false);
			this.PrefabT = this.prefabHandle.Asset.transform;
			return;
		}
		this.prefabHandle = LoadManager.LoadAsset<GameObject>(_ec.prefabPath, new Action<GameObject>(this.OnPrefabLoaded), null, false, _loadSync, false);
	}

	// Token: 0x06002646 RID: 9798 RVA: 0x000EA541 File Offset: 0x000E8741
	public void OnPrefabLoaded(GameObject prefab)
	{
		this.PrefabT = prefab.transform;
	}

	// Token: 0x06002647 RID: 9799 RVA: 0x000EA54F File Offset: 0x000E874F
	public void WaitForComplete()
	{
		if (!this.prefabHandle.IsDone)
		{
			this.prefabHandle.WaitForComplete();
		}
	}

	// Token: 0x06002648 RID: 9800 RVA: 0x000EA569 File Offset: 0x000E8769
	public void Release()
	{
		LoadManager.AssetRequestTask<GameObject> assetRequestTask = this.prefabHandle;
		if (assetRequestTask == null)
		{
			return;
		}
		assetRequestTask.Release();
	}

	// Token: 0x04001C78 RID: 7288
	[PublicizedFrom(EAccessModifier.Private)]
	public LoadManager.AssetRequestTask<GameObject> prefabHandle;
}
