using System;
using UnityEngine;

// Token: 0x02001354 RID: 4948
[RequireComponent(typeof(SkinnedMeshRenderer))]
public class Morphable : MonoBehaviour
{
	// Token: 0x06009C1F RID: 39967 RVA: 0x003AF038 File Offset: 0x003AD238
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

	// Token: 0x040075E1 RID: 30177
	public string MorphSetPath;

	// Token: 0x040075E2 RID: 30178
	public string MorphName;

	// Token: 0x040075E3 RID: 30179
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static string cMeshPrefix = "MeshMorph-";
}
