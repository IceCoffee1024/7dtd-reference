using System;
using UnityEngine;

// Token: 0x0200134F RID: 4943
[PreferBinarySerialization]
public class MeshMorph : ScriptableObject
{
	// Token: 0x17001281 RID: 4737
	// (get) Token: 0x06009C11 RID: 39953 RVA: 0x003AEE5D File Offset: 0x003AD05D
	public Vector3[] Vertices
	{
		get
		{
			return this.vertices;
		}
	}

	// Token: 0x06009C12 RID: 39954 RVA: 0x003AEE65 File Offset: 0x003AD065
	public void Init(SkinnedMeshRenderer source, Vector3[] vertices)
	{
		this.source = source;
		this.vertices = vertices;
	}

	// Token: 0x06009C13 RID: 39955 RVA: 0x003AEE78 File Offset: 0x003AD078
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

	// Token: 0x06009C14 RID: 39956 RVA: 0x003AEFAC File Offset: 0x003AD1AC
	public static bool IsInstance(Mesh _mesh)
	{
		return _mesh.name.StartsWith(MeshMorph.cMeshPrefix);
	}

	// Token: 0x040075CE RID: 30158
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public SkinnedMeshRenderer source;

	// Token: 0x040075CF RID: 30159
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3[] vertices;

	// Token: 0x040075D0 RID: 30160
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static string cMeshPrefix = "MeshMorph-";
}
