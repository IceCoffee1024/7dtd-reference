using System;
using UnityEngine;

// Token: 0x02000640 RID: 1600
public interface IMarchingCubes
{
	// Token: 0x060033F4 RID: 13300
	void Polygonize(INeighborBlockCache _nBlocks, Vector3i _localPos, Vector3 _offsetPos, byte _sunLight, byte _blockLight, VoxelMesh _mesh);
}
