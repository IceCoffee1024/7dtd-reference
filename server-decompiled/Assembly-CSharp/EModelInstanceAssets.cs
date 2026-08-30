using System;
using UnityEngine;

// Token: 0x020003EF RID: 1007
public class EModelInstanceAssets
{
	// Token: 0x170003A6 RID: 934
	// (get) Token: 0x06001E85 RID: 7813 RVA: 0x000B93F9 File Offset: 0x000B75F9
	// (set) Token: 0x06001E86 RID: 7814 RVA: 0x000B9401 File Offset: 0x000B7601
	public Transform Mesh
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170003A7 RID: 935
	// (get) Token: 0x06001E87 RID: 7815 RVA: 0x000B940A File Offset: 0x000B760A
	// (set) Token: 0x06001E88 RID: 7816 RVA: 0x000B9412 File Offset: 0x000B7612
	public Material AltMaterial
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170003A8 RID: 936
	// (get) Token: 0x06001E89 RID: 7817 RVA: 0x000B941B File Offset: 0x000B761B
	public bool IsLoadComplete
	{
		get
		{
			LoadManager.AssetRequestTask<GameObject> assetRequestTask = this.meshHandle;
			if (assetRequestTask == null || assetRequestTask.IsDone)
			{
				LoadManager.AssetRequestTask<Material> assetRequestTask2 = this.altMaterialHandle;
				return assetRequestTask2 == null || assetRequestTask2.IsDone;
			}
			return false;
		}
	}

	// Token: 0x170003A9 RID: 937
	// (get) Token: 0x06001E8A RID: 7818 RVA: 0x000B9444 File Offset: 0x000B7644
	public bool IsLoadSuccessful
	{
		get
		{
			return this.IsLoadComplete && (this.meshHandle == null || !(this.Mesh == null));
		}
	}

	// Token: 0x06001E8B RID: 7819 RVA: 0x000B946C File Offset: 0x000B766C
	public void Load(bool _loadSync, EntityCreationData _ecd, EntityClass _ec)
	{
		if (!string.IsNullOrEmpty(_ec.meshPath))
		{
			this.meshHandle = LoadManager.LoadAsset<GameObject>(_ec.meshPath, delegate(GameObject mesh)
			{
				this.OnMeshLoaded(mesh, _ec.meshPath, _ec.entityClassName);
			}, null, false, _loadSync, false);
		}
		if (_ec.AltMatNames != null)
		{
			GameRandom gameRandom = GameRandomManager.Instance.CreateGameRandom(_ecd.id);
			int num = gameRandom.RandomRange(_ec.AltMatNames.Length + 1) - 1;
			GameRandomManager.Instance.FreeGameRandom(gameRandom);
			if (num >= 0)
			{
				string assetPath = _ec.AltMatNames[num];
				this.altMaterialHandle = LoadManager.LoadAsset<Material>(assetPath, delegate(Material mat)
				{
					this.OnAltMaterialLoaded(mat, assetPath, _ec.entityClassName);
				}, null, false, _loadSync, false);
			}
		}
	}

	// Token: 0x06001E8C RID: 7820 RVA: 0x000B9550 File Offset: 0x000B7750
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnMeshLoaded(GameObject mesh, string assetPath, string entityClassName)
	{
		if (mesh == null)
		{
			Log.Error(string.Concat(new string[] { "Could not load mesh '", assetPath, "' for entity_class '", entityClassName, "'" }));
			return;
		}
		this.Mesh = mesh.transform;
	}

	// Token: 0x06001E8D RID: 7821 RVA: 0x000B95A4 File Offset: 0x000B77A4
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnAltMaterialLoaded(Material material, string assetPath, string entityClassName)
	{
		if (material == null)
		{
			Log.Error(string.Concat(new string[] { "Could not load alt material '", assetPath, "' for entity_class '", entityClassName, "'" }));
			return;
		}
		this.AltMaterial = material;
	}

	// Token: 0x06001E8E RID: 7822 RVA: 0x000B95F4 File Offset: 0x000B77F4
	public void WaitForComplete()
	{
		if (this.meshHandle != null && !this.meshHandle.IsDone)
		{
			this.meshHandle.WaitForComplete();
		}
		if (this.altMaterialHandle != null && !this.altMaterialHandle.IsDone)
		{
			this.altMaterialHandle.WaitForComplete();
		}
	}

	// Token: 0x06001E8F RID: 7823 RVA: 0x000B9641 File Offset: 0x000B7841
	public void Release()
	{
		LoadManager.AssetRequestTask<GameObject> assetRequestTask = this.meshHandle;
		if (assetRequestTask != null)
		{
			assetRequestTask.Release();
		}
		LoadManager.AssetRequestTask<Material> assetRequestTask2 = this.altMaterialHandle;
		if (assetRequestTask2 == null)
		{
			return;
		}
		assetRequestTask2.Release();
	}

	// Token: 0x04001474 RID: 5236
	[PublicizedFrom(EAccessModifier.Private)]
	public LoadManager.AssetRequestTask<GameObject> meshHandle;

	// Token: 0x04001475 RID: 5237
	[PublicizedFrom(EAccessModifier.Private)]
	public LoadManager.AssetRequestTask<Material> altMaterialHandle;
}
