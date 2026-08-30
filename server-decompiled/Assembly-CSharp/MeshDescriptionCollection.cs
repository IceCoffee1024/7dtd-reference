using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000B73 RID: 2931
public class MeshDescriptionCollection : MonoBehaviour
{
	// Token: 0x17000958 RID: 2392
	// (get) Token: 0x060058B8 RID: 22712 RVA: 0x0021E303 File Offset: 0x0021C503
	public MeshDescription[] Meshes
	{
		get
		{
			return this.currentMeshes;
		}
	}

	// Token: 0x060058B9 RID: 22713 RVA: 0x0021E30C File Offset: 0x0021C50C
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

	// Token: 0x060058BA RID: 22714 RVA: 0x0021E3CA File Offset: 0x0021C5CA
	public void UnloadTextureArrays(int _index)
	{
		this.currentMeshes[_index].UnloadTextureArrays(_index);
	}

	// Token: 0x060058BB RID: 22715 RVA: 0x0021E3DA File Offset: 0x0021C5DA
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

	// Token: 0x060058BC RID: 22716 RVA: 0x0021E3F0 File Offset: 0x0021C5F0
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

	// Token: 0x060058BD RID: 22717 RVA: 0x0021E408 File Offset: 0x0021C608
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

	// Token: 0x060058BE RID: 22718 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x040044DA RID: 17626
	public MeshDescription[] meshes;

	// Token: 0x040044DB RID: 17627
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public MeshDescription[] currentMeshes;

	// Token: 0x040044DC RID: 17628
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int quality = -1;

	// Token: 0x040044DD RID: 17629
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int[] filterToAnisoLevel = new int[] { 1, 2, 4, 8, 9 };
}
