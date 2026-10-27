using System;
using UnityEngine;

// Token: 0x020004C0 RID: 1216
public class EntityInstanceAssets
{
	// Token: 0x1700043C RID: 1084
	// (get) Token: 0x06002648 RID: 9800 RVA: 0x000EA8B9 File Offset: 0x000E8AB9
	// (set) Token: 0x06002649 RID: 9801 RVA: 0x000EA8C1 File Offset: 0x000E8AC1
	public Transform PrefabT
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700043D RID: 1085
	// (get) Token: 0x0600264A RID: 9802 RVA: 0x000EA8CA File Offset: 0x000E8ACA
	public bool IsLoadComplete
	{
		get
		{
			return this.prefabHandle.IsDone;
		}
	}

	// Token: 0x1700043E RID: 1086
	// (get) Token: 0x0600264B RID: 9803 RVA: 0x000EA8D7 File Offset: 0x000E8AD7
	public bool IsLoadSuccessful
	{
		get
		{
			return this.IsLoadComplete && !(this.PrefabT == null);
		}
	}

	// Token: 0x0600264C RID: 9804 RVA: 0x000EA8F4 File Offset: 0x000E8AF4
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

	// Token: 0x0600264D RID: 9805 RVA: 0x000EA951 File Offset: 0x000E8B51
	public void OnPrefabLoaded(GameObject prefab)
	{
		this.PrefabT = prefab.transform;
	}

	// Token: 0x0600264E RID: 9806 RVA: 0x000EA95F File Offset: 0x000E8B5F
	public void WaitForComplete()
	{
		if (!this.prefabHandle.IsDone)
		{
			this.prefabHandle.WaitForComplete();
		}
	}

	// Token: 0x0600264F RID: 9807 RVA: 0x000EA979 File Offset: 0x000E8B79
	public void Release()
	{
		LoadManager.AssetRequestTask<GameObject> assetRequestTask = this.prefabHandle;
		if (assetRequestTask == null)
		{
			return;
		}
		assetRequestTask.Release();
	}

	// Token: 0x04001C88 RID: 7304
	[PublicizedFrom(EAccessModifier.Private)]
	public LoadManager.AssetRequestTask<GameObject> prefabHandle;
}
