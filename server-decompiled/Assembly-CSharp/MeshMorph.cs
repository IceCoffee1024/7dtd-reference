using System;
using UnityEngine;

// Token: 0x0200136F RID: 4975
[PreferBinarySerialization]
public class MeshMorph : ScriptableObject
{
	// Token: 0x170012C6 RID: 4806
	// (get) Token: 0x06009CBD RID: 40125 RVA: 0x003B39D9 File Offset: 0x003B1BD9
	public Vector3[] Vertices
	{
		get
		{
			return this.vertices;
		}
	}

	// Token: 0x06009CBE RID: 40126 RVA: 0x003B39E1 File Offset: 0x003B1BE1
	public void Init(SkinnedMeshRenderer source, Vector3[] vertices)
	{
		this.source = source;
		this.vertices = vertices;
	}

	// Token: 0x06009CBF RID: 40127 RVA: 0x003B39F4 File Offset: 0x003B1BF4
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

	// Token: 0x06009CC0 RID: 40128 RVA: 0x003B3B28 File Offset: 0x003B1D28
	public static bool IsInstance(Mesh _mesh)
	{
		return _mesh.name.StartsWith(MeshMorph.cMeshPrefix);
	}

	// Token: 0x04007649 RID: 30281
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public SkinnedMeshRenderer source;

	// Token: 0x0400764A RID: 30282
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3[] vertices;

	// Token: 0x0400764B RID: 30283
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static string cMeshPrefix = "MeshMorph-";
}
