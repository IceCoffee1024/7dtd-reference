using System;
using UnityEngine;

// Token: 0x020004B8 RID: 1208
public class EntityInstanceAssets
{
	// Token: 0x17000446 RID: 1094
	// (get) Token: 0x06002643 RID: 9795 RVA: 0x000EA1C1 File Offset: 0x000E83C1
	// (set) Token: 0x06002644 RID: 9796 RVA: 0x000EA1C9 File Offset: 0x000E83C9
	public Transform PrefabT
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000447 RID: 1095
	// (get) Token: 0x06002645 RID: 9797 RVA: 0x000EA1D2 File Offset: 0x000E83D2
	public bool IsLoadComplete
	{
		get
		{
			return this.prefabHandle.IsDone;
		}
	}

	// Token: 0x17000448 RID: 1096
	// (get) Token: 0x06002646 RID: 9798 RVA: 0x000EA1DF File Offset: 0x000E83DF
	public bool IsLoadSuccessful
	{
		get
		{
			return this.IsLoadComplete && !(this.PrefabT == null);
		}
	}

	// Token: 0x06002647 RID: 9799 RVA: 0x000EA1FC File Offset: 0x000E83FC
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

	// Token: 0x06002648 RID: 9800 RVA: 0x000EA259 File Offset: 0x000E8459
	public void OnPrefabLoaded(GameObject prefab)
	{
		this.PrefabT = prefab.transform;
	}

	// Token: 0x06002649 RID: 9801 RVA: 0x000EA267 File Offset: 0x000E8467
	public void WaitForComplete()
	{
		if (!this.prefabHandle.IsDone)
		{
			this.prefabHandle.WaitForComplete();
		}
	}

	// Token: 0x0600264A RID: 9802 RVA: 0x000EA281 File Offset: 0x000E8481
	public void Release()
	{
		LoadManager.AssetRequestTask<GameObject> assetRequestTask = this.prefabHandle;
		if (assetRequestTask == null)
		{
			return;
		}
		assetRequestTask.Release();
	}

	// Token: 0x04001C77 RID: 7287
	[PublicizedFrom(EAccessModifier.Private)]
	public LoadManager.AssetRequestTask<GameObject> prefabHandle;
}
