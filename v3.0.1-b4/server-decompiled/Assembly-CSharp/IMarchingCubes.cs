using System;
using UnityEngine;

// Token: 0x02000637 RID: 1591
public interface IMarchingCubes
{
	// Token: 0x060033A1 RID: 13217
	void Polygonize(INeighborBlockCache _nBlocks, Vector3i _localPos, Vector3 _offsetPos, byte _sunLight, byte _blockLight, VoxelMesh _mesh);
}
