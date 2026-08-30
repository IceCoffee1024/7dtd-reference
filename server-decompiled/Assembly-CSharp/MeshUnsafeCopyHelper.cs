using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x0200144C RID: 5196
public static class MeshUnsafeCopyHelper
{
	// Token: 0x0600A32C RID: 41772 RVA: 0x003CFF40 File Offset: 0x003CE140
	public static void CopyVertices(ArrayListMP<Vector3> _source, Mesh _mesh)
	{
		IndexFormat indexFormat = ((_source.Count <= 65535) ? IndexFormat.UInt16 : IndexFormat.UInt32);
		_mesh.indexFormat = indexFormat;
		_mesh.SetVertices(_source.Items, 0, _source.Count);
	}

	// Token: 0x0600A32D RID: 41773 RVA: 0x003CFF79 File Offset: 0x003CE179
	public static void CopyUV(ArrayListMP<Vector2> _source, Mesh _mesh)
	{
		_mesh.SetUVs(0, _source.Items, 0, _source.Count);
	}

	// Token: 0x0600A32E RID: 41774 RVA: 0x003CFF8F File Offset: 0x003CE18F
	public static void CopyUV2(ArrayListMP<Vector2> _source, Mesh _mesh)
	{
		_mesh.SetUVs(1, _source.Items, 0, _source.Count);
	}

	// Token: 0x0600A32F RID: 41775 RVA: 0x003CFFA5 File Offset: 0x003CE1A5
	public static void CopyUV3(ArrayListMP<Vector2> _source, Mesh _mesh)
	{
		_mesh.SetUVs(2, _source.Items, 0, _source.Count);
	}

	// Token: 0x0600A330 RID: 41776 RVA: 0x003CFFBB File Offset: 0x003CE1BB
	public static void CopyUV4(ArrayListMP<Vector2> _source, Mesh _mesh)
	{
		_mesh.SetUVs(3, _source.Items, 0, _source.Count);
	}

	// Token: 0x0600A331 RID: 41777 RVA: 0x003CFFD1 File Offset: 0x003CE1D1
	public static void CopyNormals(ArrayListMP<Vector3> _source, Mesh _mesh)
	{
		_mesh.SetNormals(_source.Items, 0, _source.Count);
	}

	// Token: 0x0600A332 RID: 41778 RVA: 0x003CFFE6 File Offset: 0x003CE1E6
	public static void CopyTangents(ArrayListMP<Vector4> _source, Mesh _mesh)
	{
		_mesh.SetTangents(_source.Items, 0, _source.Count);
	}

	// Token: 0x0600A333 RID: 41779 RVA: 0x003CFFFB File Offset: 0x003CE1FB
	public static void CopyTriangles(ArrayListMP<int> _source, Mesh _mesh)
	{
		_mesh.SetTriangles(_source.Items, 0, _source.Count, 0, true, 0);
	}

	// Token: 0x0600A334 RID: 41780 RVA: 0x003D0013 File Offset: 0x003CE213
	public static void CopyTriangles(ArrayListMP<int> _source, Mesh _mesh, int _subMeshIdx)
	{
		_mesh.SetTriangles(_source.Items, 0, _source.Count, _subMeshIdx, true, 0);
	}

	// Token: 0x0600A335 RID: 41781 RVA: 0x003D002B File Offset: 0x003CE22B
	public static void CopyColors(ArrayListMP<Color> _source, Mesh _mesh)
	{
		_mesh.SetColors(_source.Items, 0, _source.Count);
	}
}
