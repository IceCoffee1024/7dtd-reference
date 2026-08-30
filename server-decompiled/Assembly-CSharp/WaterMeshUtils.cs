using System;
using UnityEngine;

// Token: 0x02000CC5 RID: 3269
public static class WaterMeshUtils
{
	// Token: 0x0600644D RID: 25677 RVA: 0x00274507 File Offset: 0x00272707
	public static void RenderFace(Vector3[] _vertices, LightingAround _lightingAround, long _textureFull, VoxelMesh[] _meshes, Vector2 UVdata, bool _alternateWinding = false)
	{
		_meshes[1].AddBasicQuad(_vertices, Color.white, UVdata, true, _alternateWinding);
	}
}
