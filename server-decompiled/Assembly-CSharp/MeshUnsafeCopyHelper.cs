using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x0200146C RID: 5228
public static class MeshUnsafeCopyHelper
{
	// Token: 0x0600A3D8 RID: 41944 RVA: 0x003D4B50 File Offset: 0x003D2D50
	public static void CopyVertices(ArrayListMP<Vector3> _source, Mesh _mesh)
	{
		IndexFormat indexFormat = ((_source.Count <= 65535) ? IndexFormat.UInt16 : IndexFormat.UInt32);
		_mesh.indexFormat = indexFormat;
		_mesh.SetVertices(_source.Items, 0, _source.Count);
	}

	// Token: 0x0600A3D9 RID: 41945 RVA: 0x003D4B89 File Offset: 0x003D2D89
	public static void CopyUV(ArrayListMP<Vector2> _source, Mesh _mesh)
	{
		_mesh.SetUVs(0, _source.Items, 0, _source.Count);
	}

	// Token: 0x0600A3DA RID: 41946 RVA: 0x003D4B9F File Offset: 0x003D2D9F
	public static void CopyUV2(ArrayListMP<Vector2> _source, Mesh _mesh)
	{
		_mesh.SetUVs(1, _source.Items, 0, _source.Count);
	}

	// Token: 0x0600A3DB RID: 41947 RVA: 0x003D4BB5 File Offset: 0x003D2DB5
	public static void CopyUV3(ArrayListMP<Vector2> _source, Mesh _mesh)
	{
		_mesh.SetUVs(2, _source.Items, 0, _source.Count);
	}

	// Token: 0x0600A3DC RID: 41948 RVA: 0x003D4BCB File Offset: 0x003D2DCB
	public static void CopyUV4(ArrayListMP<Vector2> _source, Mesh _mesh)
	{
		_mesh.SetUVs(3, _source.Items, 0, _source.Count);
	}

	// Token: 0x0600A3DD RID: 41949 RVA: 0x003D4BE1 File Offset: 0x003D2DE1
	public static void CopyNormals(ArrayListMP<Vector3> _source, Mesh _mesh)
	{
		_mesh.SetNormals(_source.Items, 0, _source.Count);
	}

	// Token: 0x0600A3DE RID: 41950 RVA: 0x003D4BF6 File Offset: 0x003D2DF6
	public static void CopyTangents(ArrayListMP<Vector4> _source, Mesh _mesh)
	{
		_mesh.SetTangents(_source.Items, 0, _source.Count);
	}

	// Token: 0x0600A3DF RID: 41951 RVA: 0x003D4C0B File Offset: 0x003D2E0B
	public static void CopyTriangles(ArrayListMP<int> _source, Mesh _mesh)
	{
		_mesh.SetTriangles(_source.Items, 0, _source.Count, 0, true, 0);
	}

	// Token: 0x0600A3E0 RID: 41952 RVA: 0x003D4C23 File Offset: 0x003D2E23
	public static void CopyTriangles(ArrayListMP<int> _source, Mesh _mesh, int _subMeshIdx)
	{
		_mesh.SetTriangles(_source.Items, 0, _source.Count, _subMeshIdx, true, 0);
	}

	// Token: 0x0600A3E1 RID: 41953 RVA: 0x003D4C3B File Offset: 0x003D2E3B
	public static void CopyColors(ArrayListMP<Color> _source, Mesh _mesh)
	{
		_mesh.SetColors(_source.Items, 0, _source.Count);
	}
}
