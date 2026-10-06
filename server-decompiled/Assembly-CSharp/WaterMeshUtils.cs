using System;
using UnityEngine;

// Token: 0x02000CE5 RID: 3301
public static class WaterMeshUtils
{
	// Token: 0x060064CB RID: 25803 RVA: 0x0027A7A7 File Offset: 0x002789A7
	public static void RenderFace(Vector3[] _vertices, LightingAround _lightingAround, long _textureFull, VoxelMesh[] _meshes, Vector2 UVdata, bool _alternateWinding = false)
	{
		_meshes[1].AddBasicQuad(_vertices, Color.white, UVdata, true, _alternateWinding);
	}
}
