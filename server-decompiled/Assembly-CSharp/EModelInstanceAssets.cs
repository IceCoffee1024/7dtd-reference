using System;
using UnityEngine;

// Token: 0x020003F4 RID: 1012
public class EModelInstanceAssets
{
	// Token: 0x1700039E RID: 926
	// (get) Token: 0x06001E81 RID: 7809 RVA: 0x000B99A1 File Offset: 0x000B7BA1
	// (set) Token: 0x06001E82 RID: 7810 RVA: 0x000B99A9 File Offset: 0x000B7BA9
	public Transform Mesh
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700039F RID: 927
	// (get) Token: 0x06001E83 RID: 7811 RVA: 0x000B99B2 File Offset: 0x000B7BB2
	// (set) Token: 0x06001E84 RID: 7812 RVA: 0x000B99BA File Offset: 0x000B7BBA
	public Material AltMaterial
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170003A0 RID: 928
	// (get) Token: 0x06001E85 RID: 7813 RVA: 0x000B99C3 File Offset: 0x000B7BC3
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

	// Token: 0x170003A1 RID: 929
	// (get) Token: 0x06001E86 RID: 7814 RVA: 0x000B99EC File Offset: 0x000B7BEC
	public bool IsLoadSuccessful
	{
		get
		{
			return this.IsLoadComplete && (this.meshHandle == null || !(this.Mesh == null));
		}
	}

	// Token: 0x06001E87 RID: 7815 RVA: 0x000B9A14 File Offset: 0x000B7C14
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

	// Token: 0x06001E88 RID: 7816 RVA: 0x000B9AF8 File Offset: 0x000B7CF8
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

	// Token: 0x06001E89 RID: 7817 RVA: 0x000B9B4C File Offset: 0x000B7D4C
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

	// Token: 0x06001E8A RID: 7818 RVA: 0x000B9B9C File Offset: 0x000B7D9C
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

	// Token: 0x06001E8B RID: 7819 RVA: 0x000B9BE9 File Offset: 0x000B7DE9
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

	// Token: 0x0400147D RID: 5245
	[PublicizedFrom(EAccessModifier.Private)]
	public LoadManager.AssetRequestTask<GameObject> meshHandle;

	// Token: 0x0400147E RID: 5246
	[PublicizedFrom(EAccessModifier.Private)]
	public LoadManager.AssetRequestTask<Material> altMaterialHandle;
}
