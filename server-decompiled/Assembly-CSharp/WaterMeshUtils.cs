using System;
using UnityEngine;

// Token: 0x02000CD7 RID: 3287
public static class WaterMeshUtils
{
	// Token: 0x060064D9 RID: 25817 RVA: 0x00277403 File Offset: 0x00275603
	public static void RenderFace(Vector3[] _vertices, LightingAround _lightingAround, long _textureFull, VoxelMesh[] _meshes, Vector2 UVdata, bool _alternateWinding = false)
	{
		_meshes[1].AddBasicQuad(_vertices, Color.white, UVdata, true, _alternateWinding);
	}
}
