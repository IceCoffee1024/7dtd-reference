using System;
using UnityEngine;

// Token: 0x02001381 RID: 4993
[PreferBinarySerialization]
public class MeshMorph : ScriptableObject
{
	// Token: 0x170012D2 RID: 4818
	// (get) Token: 0x06009D15 RID: 40213 RVA: 0x003B5DDD File Offset: 0x003B3FDD
	public Vector3[] Vertices
	{
		get
		{
			return this.vertices;
		}
	}

	// Token: 0x06009D16 RID: 40214 RVA: 0x003B5DE5 File Offset: 0x003B3FE5
	public void Init(SkinnedMeshRenderer source, Vector3[] vertices)
	{
		this.source = source;
		this.vertices = vertices;
	}

	// Token: 0x06009D17 RID: 40215 RVA: 0x003B5DF8 File Offset: 0x003B3FF8
	public GameObject GetMorphedSkinnedMesh()
	{
		if (this.source == null || this.source.sharedMesh == null || this.vertices == null || this.vertices.Length == 0)
		{
			Debug.LogError("MeshMorph: source or vertices are null or empty", this);
			return null;
		}
		Mesh mesh = global::UnityEngine.Object.Instantiate<Mesh>(this.source.sharedMesh);
		mesh.name = MeshMorph.cMeshPrefix + this.source.gameObject.name;
		mesh.SetVertices(this.vertices);
		mesh.RecalculateBounds();
		GameObject gameObject = new GameObject(this.source.gameObject.name);
		gameObject.transform.localPosition = this.source.transform.localPosition;
		gameObject.transform.localRotation = this.source.transform.localRotation;
		gameObject.transform.localScale = this.source.transform.localScale;
		SkinnedMeshRenderer skinnedMeshRenderer = gameObject.AddComponent<SkinnedMeshRenderer>();
		skinnedMeshRenderer.sharedMesh = mesh;
		skinnedMeshRenderer.sharedMaterials = this.source.sharedMaterials;
		skinnedMeshRenderer.rootBone = this.source.rootBone;
		skinnedMeshRenderer.bones = this.source.bones;
		return gameObject;
	}

	// Token: 0x06009D18 RID: 40216 RVA: 0x003B5F2C File Offset: 0x003B412C
	public static bool IsInstance(Mesh _mesh)
	{
		return _mesh.name.StartsWith(MeshMorph.cMeshPrefix);
	}

	// Token: 0x0400768A RID: 30346
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public SkinnedMeshRenderer source;

	// Token: 0x0400768B RID: 30347
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3[] vertices;

	// Token: 0x0400768C RID: 30348
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static string cMeshPrefix = "MeshMorph-";
}
