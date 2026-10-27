using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000B90 RID: 2960
public class MeshDescriptionCollection : MonoBehaviour
{
	// Token: 0x17000980 RID: 2432
	// (get) Token: 0x060058F7 RID: 22775 RVA: 0x00222AEB File Offset: 0x00220CEB
	public MeshDescription[] Meshes
	{
		get
		{
			return this.currentMeshes;
		}
	}

	// Token: 0x060058F8 RID: 22776 RVA: 0x00222AF4 File Offset: 0x00220CF4
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

	// Token: 0x060058F9 RID: 22777 RVA: 0x00222BB2 File Offset: 0x00220DB2
	public void UnloadTextureArrays(int _index)
	{
		this.currentMeshes[_index].UnloadTextureArrays(_index);
	}

	// Token: 0x060058FA RID: 22778 RVA: 0x00222BC2 File Offset: 0x00220DC2
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

	// Token: 0x060058FB RID: 22779 RVA: 0x00222BD8 File Offset: 0x00220DD8
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

	// Token: 0x060058FC RID: 22780 RVA: 0x00222BF0 File Offset: 0x00220DF0
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

	// Token: 0x060058FD RID: 22781 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x0400453D RID: 17725
	public MeshDescription[] meshes;

	// Token: 0x0400453E RID: 17726
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public MeshDescription[] currentMeshes;

	// Token: 0x0400453F RID: 17727
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int quality = -1;

	// Token: 0x04004540 RID: 17728
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int[] filterToAnisoLevel = new int[] { 1, 2, 4, 8, 9 };
}
