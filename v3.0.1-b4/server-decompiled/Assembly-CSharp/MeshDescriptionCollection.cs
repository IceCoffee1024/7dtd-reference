using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000B61 RID: 2913
public class MeshDescriptionCollection : MonoBehaviour
{
	// Token: 0x17000946 RID: 2374
	// (get) Token: 0x06005830 RID: 22576 RVA: 0x0021B5E7 File Offset: 0x002197E7
	public MeshDescription[] Meshes
	{
		get
		{
			return this.currentMeshes;
		}
	}

	// Token: 0x06005831 RID: 22577 RVA: 0x0021B5F0 File Offset: 0x002197F0
	public void Init()
	{
		MeshDescription meshDescription = this.meshes[0];
		if (meshDescription.TexDiffuse || meshDescription.TexNormal || meshDescription.TexSpecular)
		{
			Log.Error("MeshDescriptionCollection should not have MESH_OPAQUE textures");
		}
		meshDescription = this.meshes[5];
		if (meshDescription.TexDiffuse || meshDescription.TexNormal || meshDescription.TexSpecular)
		{
			Log.Error("MeshDescriptionCollection should not have MESH_TERRAIN textures");
		}
		this.currentMeshes = new MeshDescription[this.meshes.Length];
		for (int i = 0; i < this.meshes.Length; i++)
		{
			meshDescription = new MeshDescription(this.meshes[i]);
			this.currentMeshes[i] = meshDescription;
		}
	}

	// Token: 0x06005832 RID: 22578 RVA: 0x0021B6AE File Offset: 0x002198AE
	public void UnloadTextureArrays(int _index)
	{
		this.currentMeshes[_index].UnloadTextureArrays(_index);
	}

	// Token: 0x06005833 RID: 22579 RVA: 0x0021B6BE File Offset: 0x002198BE
	public IEnumerator LoadTextureArrays(bool _isReload = false)
	{
		MicroStopwatch ms = new MicroStopwatch(true);
		MeshDescription[] mds = this.currentMeshes;
		for (int i = 0; i < mds.Length; i++)
		{
			mds[i].UnloadTextureArrays(i);
		}
		if (_isReload)
		{
			Resources.UnloadUnusedAssets();
		}
		int num;
		for (int index = 0; index < mds.Length; index = num + 1)
		{
			MeshDescription meshDescription = mds[index];
			yield return meshDescription.LoadTextureArraysForQuality(this, index, this.quality, _isReload);
			yield return null;
			num = index;
		}
		Log.Out("LoadTextureArraysForQuality took {0}", new object[] { (float)ms.ElapsedMilliseconds * 0.001f });
		if (GameManager.Instance != null && GameManager.Instance.prefabLODManager != null)
		{
			GameManager.Instance.prefabLODManager.UpdateMaterials();
		}
		yield break;
	}

	// Token: 0x06005834 RID: 22580 RVA: 0x0021B6D4 File Offset: 0x002198D4
	public IEnumerator LoadTextureArraysForQuality(bool _isReload = false)
	{
		if (GameManager.IsDedicatedServer)
		{
			yield break;
		}
		int num = GameOptionsManager.GetTextureQuality(-1);
		if (num >= 3)
		{
			num = 2;
		}
		Log.Out("LoadTextureArraysForQuality quality {0} to {1}, reload {2}", new object[] { this.quality, num, _isReload });
		if (_isReload && num == this.quality)
		{
			yield break;
		}
		this.quality = num;
		yield return this.LoadTextureArrays(_isReload);
		yield break;
	}

	// Token: 0x06005835 RID: 22581 RVA: 0x0021B6EC File Offset: 0x002198EC
	public void SetTextureArraysFilter()
	{
		if (GameManager.IsDedicatedServer)
		{
			return;
		}
		int textureFilter = GameOptionsManager.GetTextureFilter();
		int num = MeshDescriptionCollection.filterToAnisoLevel[textureFilter];
		Log.Out("SetTextureArraysFilter {0}, AF {1}", new object[] { textureFilter, num });
		MeshDescription[] array = this.currentMeshes;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetTextureFilter(i, num);
		}
	}

	// Token: 0x06005836 RID: 22582 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x0400443F RID: 17471
	public MeshDescription[] meshes;

	// Token: 0x04004440 RID: 17472
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public MeshDescription[] currentMeshes;

	// Token: 0x04004441 RID: 17473
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int quality = -1;

	// Token: 0x04004442 RID: 17474
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int[] filterToAnisoLevel = new int[] { 1, 2, 4, 8, 9 };
}
