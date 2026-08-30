using System;
using UnityEngine;

// Token: 0x020004B4 RID: 1204
public class EntityInstanceAssets
{
	// Token: 0x17000442 RID: 1090
	// (get) Token: 0x06002621 RID: 9761 RVA: 0x000E9569 File Offset: 0x000E7769
	// (set) Token: 0x06002622 RID: 9762 RVA: 0x000E9571 File Offset: 0x000E7771
	public Transform PrefabT
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000443 RID: 1091
	// (get) Token: 0x06002623 RID: 9763 RVA: 0x000E957A File Offset: 0x000E777A
	public bool IsLoadComplete
	{
		get
		{
			return this.prefabHandle.IsDone;
		}
	}

	// Token: 0x17000444 RID: 1092
	// (get) Token: 0x06002624 RID: 9764 RVA: 0x000E9587 File Offset: 0x000E7787
	public bool IsLoadSuccessful
	{
		get
		{
			return this.IsLoadComplete && !(this.PrefabT == null);
		}
	}

	// Token: 0x06002625 RID: 9765 RVA: 0x000E95A4 File Offset: 0x000E77A4
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

	// Token: 0x06002626 RID: 9766 RVA: 0x000E9601 File Offset: 0x000E7801
	public void OnPrefabLoaded(GameObject prefab)
	{
		this.PrefabT = prefab.transform;
	}

	// Token: 0x06002627 RID: 9767 RVA: 0x000E960F File Offset: 0x000E780F
	public void WaitForComplete()
	{
		if (!this.prefabHandle.IsDone)
		{
			this.prefabHandle.WaitForComplete();
		}
	}

	// Token: 0x06002628 RID: 9768 RVA: 0x000E9629 File Offset: 0x000E7829
	public void Release()
	{
		LoadManager.AssetRequestTask<GameObject> assetRequestTask = this.prefabHandle;
		if (assetRequestTask == null)
		{
			return;
		}
		assetRequestTask.Release();
	}

	// Token: 0x04001C55 RID: 7253
	[PublicizedFrom(EAccessModifier.Private)]
	public LoadManager.AssetRequestTask<GameObject> prefabHandle;
}
