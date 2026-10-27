using System;
using UnityEngine;

// Token: 0x0200064A RID: 1610
public class MarchingCubes : IMarchingCubes
{
	// Token: 0x06003446 RID: 13382 RVA: 0x000027FC File Offset: 0x000009FC
	public void Polygonize(INeighborBlockCache _nBlocks, Vector3i _localPos, Vector3 _offsetPos, byte _sunLight, byte _blockLight, VoxelMesh _mesh)
	{
	}

	// Token: 0x06003447 RID: 13383 RVA: 0x0014EBA8 File Offset: 0x0014CDA8
	public static float GetDecorationOffsetY(sbyte _densY, sbyte _densYm1)
	{
		float num = (float)(_densY + _densYm1);
		return Utils.FastClamp(-0.0035f * num, -0.4f, 0.4f);
	}

	// Token: 0x0400292D RID: 10541
	public static readonly sbyte DensityAir = sbyte.MaxValue;

	// Token: 0x0400292E RID: 10542
	public static readonly sbyte DensityAirHi = 100;

	// Token: 0x0400292F RID: 10543
	public static readonly sbyte DensityTerrain = sbyte.MinValue;

	// Token: 0x04002930 RID: 10544
	public static readonly sbyte DensityTerrainHi = -100;
}
