using System;
using UnityEngine;

// Token: 0x02000641 RID: 1601
public class MarchingCubes : IMarchingCubes
{
	// Token: 0x060033F5 RID: 13301 RVA: 0x000027FC File Offset: 0x000009FC
	public void Polygonize(INeighborBlockCache _nBlocks, Vector3i _localPos, Vector3 _offsetPos, byte _sunLight, byte _blockLight, VoxelMesh _mesh)
	{
	}

	// Token: 0x060033F6 RID: 13302 RVA: 0x0014D644 File Offset: 0x0014B844
	public static float GetDecorationOffsetY(sbyte _densY, sbyte _densYm1)
	{
		float num = (float)(_densY + _densYm1);
		return Utils.FastClamp(-0.0035f * num, -0.4f, 0.4f);
	}

	// Token: 0x04002919 RID: 10521
	public static readonly sbyte DensityAir = sbyte.MaxValue;

	// Token: 0x0400291A RID: 10522
	public static readonly sbyte DensityAirHi = 100;

	// Token: 0x0400291B RID: 10523
	public static readonly sbyte DensityTerrain = sbyte.MinValue;

	// Token: 0x0400291C RID: 10524
	public static readonly sbyte DensityTerrainHi = -100;
}
