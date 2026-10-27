using System;
using UnityEngine;

// Token: 0x02001386 RID: 4998
[RequireComponent(typeof(SkinnedMeshRenderer))]
public class Morphable : MonoBehaviour
{
	// Token: 0x06009D23 RID: 40227 RVA: 0x003B5FB8 File Offset: 0x003B41B8
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

	// Token: 0x0400769D RID: 30365
	public string MorphSetPath;

	// Token: 0x0400769E RID: 30366
	public string MorphName;

	// Token: 0x0400769F RID: 30367
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static string cMeshPrefix = "MeshMorph-";
}
