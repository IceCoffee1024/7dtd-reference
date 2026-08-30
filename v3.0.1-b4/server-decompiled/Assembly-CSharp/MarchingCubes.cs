using System;
using UnityEngine;

// Token: 0x02000638 RID: 1592
public class MarchingCubes : IMarchingCubes
{
	// Token: 0x060033A2 RID: 13218 RVA: 0x000027FC File Offset: 0x000009FC
	public void Polygonize(INeighborBlockCache _nBlocks, Vector3i _localPos, Vector3 _offsetPos, byte _sunLight, byte _blockLight, VoxelMesh _mesh)
	{
	}

	// Token: 0x060033A3 RID: 13219 RVA: 0x0014B528 File Offset: 0x00149728
	public static float GetDecorationOffsetY(sbyte _densY, sbyte _densYm1)
	{
		float num = (float)(_densY + _densYm1);
		return Utils.FastClamp(-0.0035f * num, -0.4f, 0.4f);
	}

	// Token: 0x040028A6 RID: 10406
	public static readonly sbyte DensityAir = sbyte.MaxValue;

	// Token: 0x040028A7 RID: 10407
	public static readonly sbyte DensityAirHi = 100;

	// Token: 0x040028A8 RID: 10408
	public static readonly sbyte DensityTerrain = sbyte.MinValue;

	// Token: 0x040028A9 RID: 10409
	public static readonly sbyte DensityTerrainHi = -100;
}
