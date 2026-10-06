using System;
using UnityEngine;

// Token: 0x020003F2 RID: 1010
public class EModelInstanceAssets
{
	// Token: 0x1700039E RID: 926
	// (get) Token: 0x06001E7C RID: 7804 RVA: 0x000B95F5 File Offset: 0x000B77F5
	// (set) Token: 0x06001E7D RID: 7805 RVA: 0x000B95FD File Offset: 0x000B77FD
	public Transform Mesh
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700039F RID: 927
	// (get) Token: 0x06001E7E RID: 7806 RVA: 0x000B9606 File Offset: 0x000B7806
	// (set) Token: 0x06001E7F RID: 7807 RVA: 0x000B960E File Offset: 0x000B780E
	public Material AltMaterial
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170003A0 RID: 928
	// (get) Token: 0x06001E80 RID: 7808 RVA: 0x000B9617 File Offset: 0x000B7817
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
	// (get) Token: 0x06001E81 RID: 7809 RVA: 0x000B9640 File Offset: 0x000B7840
	public bool IsLoadSuccessful
	{
		get
		{
			return this.IsLoadComplete && (this.meshHandle == null || !(this.Mesh == null));
		}
	}

	// Token: 0x06001E82 RID: 7810 RVA: 0x000B9668 File Offset: 0x000B7868
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

	// Token: 0x06001E83 RID: 7811 RVA: 0x000B974C File Offset: 0x000B794C
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

	// Token: 0x06001E84 RID: 7812 RVA: 0x000B97A0 File Offset: 0x000B79A0
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

	// Token: 0x06001E85 RID: 7813 RVA: 0x000B97F0 File Offset: 0x000B79F0
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

	// Token: 0x06001E86 RID: 7814 RVA: 0x000B983D File Offset: 0x000B7A3D
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

	// Token: 0x0400146D RID: 5229
	[PublicizedFrom(EAccessModifier.Private)]
	public LoadManager.AssetRequestTask<GameObject> meshHandle;

	// Token: 0x0400146E RID: 5230
	[PublicizedFrom(EAccessModifier.Private)]
	public LoadManager.AssetRequestTask<Material> altMaterialHandle;
}
