using System;
using UnityEngine;

// Token: 0x0200133A RID: 4922
[RequireComponent(typeof(SkinnedMeshRenderer))]
public class Morphable : MonoBehaviour
{
	// Token: 0x06009B24 RID: 39716 RVA: 0x003A9D90 File Offset: 0x003A7F90
	public void MorphHeadgear(Archetype archetype, bool _ignoreDlcEntitlements)
	{
		SkinnedMeshRenderer component = base.GetComponent<SkinnedMeshRenderer>();
		if (component == null || component.sharedMesh == null)
		{
			Log.Error("Morphable: SkinnedMeshRenderer not found", new object[] { this });
			return;
		}
		LoadManager.AssetRequestTask<MeshMorph> assetRequestTask = LoadManager.LoadAsset<MeshMorph>(string.Concat(new string[]
		{
			this.MorphSetPath,
			"/",
			archetype.Race,
			archetype.Variant.ToString("00"),
			"/",
			this.MorphName,
			".asset"
		}), null, null, false, true, _ignoreDlcEntitlements);
		MeshMorph asset = assetRequestTask.Asset;
		if (asset == null || asset.Vertices == null || asset.Vertices.Length == 0)
		{
			Log.Error("Morphable: MeshMorph not found", new object[] { this });
			return;
		}
		Mesh mesh = global::UnityEngine.Object.Instantiate<Mesh>(component.sharedMesh);
		mesh.name = Morphable.cMeshPrefix + component.gameObject.name;
		mesh.SetVertices(asset.Vertices);
		mesh.RecalculateBounds();
		component.sharedMesh = mesh;
		assetRequestTask.Release();
	}

	// Token: 0x04007509 RID: 29961
	public string MorphSetPath;

	// Token: 0x0400750A RID: 29962
	public string MorphName;

	// Token: 0x0400750B RID: 29963
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static string cMeshPrefix = "MeshMorph-";
}
