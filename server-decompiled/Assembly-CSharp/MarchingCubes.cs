using System;
using UnityEngine;

// Token: 0x02000643 RID: 1603
public class MarchingCubes : IMarchingCubes
{
	// Token: 0x06003438 RID: 13368 RVA: 0x000027FC File Offset: 0x000009FC
	public void Polygonize(INeighborBlockCache _nBlocks, Vector3i _localPos, Vector3 _offsetPos, byte _sunLight, byte _blockLight, VoxelMesh _mesh)
	{
	}

	// Token: 0x06003439 RID: 13369 RVA: 0x0014DFB4 File Offset: 0x0014C1B4
	public static float GetDecorationOffsetY(sbyte _densY, sbyte _densYm1)
	{
		float num = (float)(_densY + _densYm1);
		return Utils.FastClamp(-0.0035f * num, -0.4f, 0.4f);
	}

	// Token: 0x04002912 RID: 10514
	public static readonly sbyte DensityAir = sbyte.MaxValue;

	// Token: 0x04002913 RID: 10515
	public static readonly sbyte DensityAirHi = 100;

	// Token: 0x04002914 RID: 10516
	public static readonly sbyte DensityTerrain = sbyte.MinValue;

	// Token: 0x04002915 RID: 10517
	public static readonly sbyte DensityTerrainHi = -100;
}
