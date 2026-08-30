using System;
using UnityEngine;

// Token: 0x020003EB RID: 1003
public class EModelInstanceAssets
{
	// Token: 0x170003A2 RID: 930
	// (get) Token: 0x06001E68 RID: 7784 RVA: 0x000B8CB5 File Offset: 0x000B6EB5
	// (set) Token: 0x06001E69 RID: 7785 RVA: 0x000B8CBD File Offset: 0x000B6EBD
	public Transform Mesh
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170003A3 RID: 931
	// (get) Token: 0x06001E6A RID: 7786 RVA: 0x000B8CC6 File Offset: 0x000B6EC6
	// (set) Token: 0x06001E6B RID: 7787 RVA: 0x000B8CCE File Offset: 0x000B6ECE
	public Material AltMaterial
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170003A4 RID: 932
	// (get) Token: 0x06001E6C RID: 7788 RVA: 0x000B8CD7 File Offset: 0x000B6ED7
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

	// Token: 0x170003A5 RID: 933
	// (get) Token: 0x06001E6D RID: 7789 RVA: 0x000B8D00 File Offset: 0x000B6F00
	public bool IsLoadSuccessful
	{
		get
		{
			return this.IsLoadComplete && (this.meshHandle == null || !(this.Mesh == null));
		}
	}

	// Token: 0x06001E6E RID: 7790 RVA: 0x000B8D28 File Offset: 0x000B6F28
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

	// Token: 0x06001E6F RID: 7791 RVA: 0x000B8E0C File Offset: 0x000B700C
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

	// Token: 0x06001E70 RID: 7792 RVA: 0x000B8E60 File Offset: 0x000B7060
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

	// Token: 0x06001E71 RID: 7793 RVA: 0x000B8EB0 File Offset: 0x000B70B0
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

	// Token: 0x06001E72 RID: 7794 RVA: 0x000B8EFD File Offset: 0x000B70FD
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

	// Token: 0x0400145F RID: 5215
	[PublicizedFrom(EAccessModifier.Private)]
	public LoadManager.AssetRequestTask<GameObject> meshHandle;

	// Token: 0x04001460 RID: 5216
	[PublicizedFrom(EAccessModifier.Private)]
	public LoadManager.AssetRequestTask<Material> altMaterialHandle;
}
