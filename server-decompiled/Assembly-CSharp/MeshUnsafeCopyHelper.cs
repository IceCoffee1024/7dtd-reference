using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02001431 RID: 5169
public static class MeshUnsafeCopyHelper
{
	// Token: 0x0600A22F RID: 41519 RVA: 0x003CABFC File Offset: 0x003C8DFC
	public static void CopyVertices(ArrayListMP<Vector3> _source, Mesh _mesh)
	{
		IndexFormat indexFormat = ((_source.Count <= 65535) ? IndexFormat.UInt16 : IndexFormat.UInt32);
		_mesh.indexFormat = indexFormat;
		_mesh.SetVertices(_source.Items, 0, _source.Count);
	}

	// Token: 0x0600A230 RID: 41520 RVA: 0x003CAC35 File Offset: 0x003C8E35
	public static void CopyUV(ArrayListMP<Vector2> _source, Mesh _mesh)
	{
		_mesh.SetUVs(0, _source.Items, 0, _source.Count);
	}

	// Token: 0x0600A231 RID: 41521 RVA: 0x003CAC4B File Offset: 0x003C8E4B
	public static void CopyUV2(ArrayListMP<Vector2> _source, Mesh _mesh)
	{
		_mesh.SetUVs(1, _source.Items, 0, _source.Count);
	}

	// Token: 0x0600A232 RID: 41522 RVA: 0x003CAC61 File Offset: 0x003C8E61
	public static void CopyUV3(ArrayListMP<Vector2> _source, Mesh _mesh)
	{
		_mesh.SetUVs(2, _source.Items, 0, _source.Count);
	}

	// Token: 0x0600A233 RID: 41523 RVA: 0x003CAC77 File Offset: 0x003C8E77
	public static void CopyUV4(ArrayListMP<Vector2> _source, Mesh _mesh)
	{
		_mesh.SetUVs(3, _source.Items, 0, _source.Count);
	}

	// Token: 0x0600A234 RID: 41524 RVA: 0x003CAC8D File Offset: 0x003C8E8D
	public static void CopyNormals(ArrayListMP<Vector3> _source, Mesh _mesh)
	{
		_mesh.SetNormals(_source.Items, 0, _source.Count);
	}

	// Token: 0x0600A235 RID: 41525 RVA: 0x003CACA2 File Offset: 0x003C8EA2
	public static void CopyTangents(ArrayListMP<Vector4> _source, Mesh _mesh)
	{
		_mesh.SetTangents(_source.Items, 0, _source.Count);
	}

	// Token: 0x0600A236 RID: 41526 RVA: 0x003CACB7 File Offset: 0x003C8EB7
	public static void CopyTriangles(ArrayListMP<int> _source, Mesh _mesh)
	{
		_mesh.SetTriangles(_source.Items, 0, _source.Count, 0, true, 0);
	}

	// Token: 0x0600A237 RID: 41527 RVA: 0x003CACCF File Offset: 0x003C8ECF
	public static void CopyTriangles(ArrayListMP<int> _source, Mesh _mesh, int _subMeshIdx)
	{
		_mesh.SetTriangles(_source.Items, 0, _source.Count, _subMeshIdx, true, 0);
	}

	// Token: 0x0600A238 RID: 41528 RVA: 0x003CACE7 File Offset: 0x003C8EE7
	public static void CopyColors(ArrayListMP<Color> _source, Mesh _mesh)
	{
		_mesh.SetColors(_source.Items, 0, _source.Count);
	}
}
