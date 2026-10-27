using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x0200147E RID: 5246
public static class MeshUnsafeCopyHelper
{
	// Token: 0x0600A430 RID: 42032 RVA: 0x003D6F54 File Offset: 0x003D5154
	public static void CopyVertices(ArrayListMP<Vector3> _source, Mesh _mesh)
	{
		IndexFormat indexFormat = ((_source.Count <= 65535) ? IndexFormat.UInt16 : IndexFormat.UInt32);
		_mesh.indexFormat = indexFormat;
		_mesh.SetVertices(_source.Items, 0, _source.Count);
	}

	// Token: 0x0600A431 RID: 42033 RVA: 0x003D6F8D File Offset: 0x003D518D
	public static void CopyUV(ArrayListMP<Vector2> _source, Mesh _mesh)
	{
		_mesh.SetUVs(0, _source.Items, 0, _source.Count);
	}

	// Token: 0x0600A432 RID: 42034 RVA: 0x003D6FA3 File Offset: 0x003D51A3
	public static void CopyUV2(ArrayListMP<Vector2> _source, Mesh _mesh)
	{
		_mesh.SetUVs(1, _source.Items, 0, _source.Count);
	}

	// Token: 0x0600A433 RID: 42035 RVA: 0x003D6FB9 File Offset: 0x003D51B9
	public static void CopyUV3(ArrayListMP<Vector2> _source, Mesh _mesh)
	{
		_mesh.SetUVs(2, _source.Items, 0, _source.Count);
	}

	// Token: 0x0600A434 RID: 42036 RVA: 0x003D6FCF File Offset: 0x003D51CF
	public static void CopyUV4(ArrayListMP<Vector2> _source, Mesh _mesh)
	{
		_mesh.SetUVs(3, _source.Items, 0, _source.Count);
	}

	// Token: 0x0600A435 RID: 42037 RVA: 0x003D6FE5 File Offset: 0x003D51E5
	public static void CopyNormals(ArrayListMP<Vector3> _source, Mesh _mesh)
	{
		_mesh.SetNormals(_source.Items, 0, _source.Count);
	}

	// Token: 0x0600A436 RID: 42038 RVA: 0x003D6FFA File Offset: 0x003D51FA
	public static void CopyTangents(ArrayListMP<Vector4> _source, Mesh _mesh)
	{
		_mesh.SetTangents(_source.Items, 0, _source.Count);
	}

	// Token: 0x0600A437 RID: 42039 RVA: 0x003D700F File Offset: 0x003D520F
	public static void CopyTriangles(ArrayListMP<int> _source, Mesh _mesh)
	{
		_mesh.SetTriangles(_source.Items, 0, _source.Count, 0, true, 0);
	}

	// Token: 0x0600A438 RID: 42040 RVA: 0x003D7027 File Offset: 0x003D5227
	public static void CopyTriangles(ArrayListMP<int> _source, Mesh _mesh, int _subMeshIdx)
	{
		_mesh.SetTriangles(_source.Items, 0, _source.Count, _subMeshIdx, true, 0);
	}

	// Token: 0x0600A439 RID: 42041 RVA: 0x003D703F File Offset: 0x003D523F
	public static void CopyColors(ArrayListMP<Color> _source, Mesh _mesh)
	{
		_mesh.SetColors(_source.Items, 0, _source.Count);
	}
}
