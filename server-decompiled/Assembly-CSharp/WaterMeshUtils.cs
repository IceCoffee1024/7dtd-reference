using System;
using UnityEngine;

// Token: 0x02000CF0 RID: 3312
public static class WaterMeshUtils
{
	// Token: 0x060064F0 RID: 25840 RVA: 0x0027BCCF File Offset: 0x00279ECF
	public static void RenderFace(Vector3[] _vertices, LightingAround _lightingAround, long _textureFull, VoxelMesh[] _meshes, Vector2 UVdata, bool _alternateWinding = false)
	{
		_meshes[1].AddBasicQuad(_vertices, Color.white, UVdata, true, _alternateWinding);
	}
}
