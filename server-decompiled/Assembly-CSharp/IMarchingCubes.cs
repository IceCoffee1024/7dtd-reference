using System;
using UnityEngine;

// Token: 0x02000649 RID: 1609
public interface IMarchingCubes
{
	// Token: 0x06003445 RID: 13381
	void Polygonize(INeighborBlockCache _nBlocks, Vector3i _localPos, Vector3 _offsetPos, byte _sunLight, byte _blockLight, VoxelMesh _mesh);
}
