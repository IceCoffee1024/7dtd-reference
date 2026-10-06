using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000B85 RID: 2949
public class MeshDescriptionCollection : MonoBehaviour
{
	// Token: 0x1700097A RID: 2426
	// (get) Token: 0x060058D2 RID: 22738 RVA: 0x0022158F File Offset: 0x0021F78F
	public MeshDescription[] Meshes
	{
		get
		{
			return this.currentMeshes;
		}
	}

	// Token: 0x060058D3 RID: 22739 RVA: 0x00221598 File Offset: 0x0021F798
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

	// Token: 0x060058D4 RID: 22740 RVA: 0x00221656 File Offset: 0x0021F856
	public void UnloadTextureArrays(int _index)
	{
		this.currentMeshes[_index].UnloadTextureArrays(_index);
	}

	// Token: 0x060058D5 RID: 22741 RVA: 0x00221666 File Offset: 0x0021F866
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

	// Token: 0x060058D6 RID: 22742 RVA: 0x0022167C File Offset: 0x0021F87C
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

	// Token: 0x060058D7 RID: 22743 RVA: 0x00221694 File Offset: 0x0021F894
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

	// Token: 0x060058D8 RID: 22744 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x04004518 RID: 17688
	public MeshDescription[] meshes;

	// Token: 0x04004519 RID: 17689
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public MeshDescription[] currentMeshes;

	// Token: 0x0400451A RID: 17690
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int quality = -1;

	// Token: 0x0400451B RID: 17691
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int[] filterToAnisoLevel = new int[] { 1, 2, 4, 8, 9 };
}
