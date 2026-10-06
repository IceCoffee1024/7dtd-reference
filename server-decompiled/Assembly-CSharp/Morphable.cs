using System;
using UnityEngine;

// Token: 0x02001374 RID: 4980
[RequireComponent(typeof(SkinnedMeshRenderer))]
public class Morphable : MonoBehaviour
{
	// Token: 0x06009CCB RID: 40139 RVA: 0x003B3BB4 File Offset: 0x003B1DB4
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

	// Token: 0x0400765C RID: 30300
	public string MorphSetPath;

	// Token: 0x0400765D RID: 30301
	public string MorphName;

	// Token: 0x0400765E RID: 30302
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static string cMeshPrefix = "MeshMorph-";
}
