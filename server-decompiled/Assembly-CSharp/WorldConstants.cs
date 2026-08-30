using System;
using UnityEngine;

// Token: 0x02000BF7 RID: 3063
public class WorldConstants
{
	// Token: 0x06005D7B RID: 23931 RVA: 0x00245620 File Offset: 0x00243820
	public static Rect MapBlockToUVRect(int _meshIndex, BlockValue _blockValue, BlockFace _blockFace)
	{
		int sideTextureId = _blockValue.Block.GetSideTextureId(_blockValue, _blockFace, 0);
		if (sideTextureId >= 0 && sideTextureId < MeshDescription.meshes[_meshIndex].textureAtlas.uvMapping.Length)
		{
			return MeshDescription.meshes[_meshIndex].textureAtlas.uvMapping[sideTextureId].uv;
		}
		return WorldConstants.uvRectZero;
	}

	// Token: 0x06005D7C RID: 23932 RVA: 0x0024567C File Offset: 0x0024387C
	public static Rect MapDamageToUVRect(BlockValue _blockValue)
	{
		if (_blockValue.hasdecal || _blockValue.ischild || _blockValue.damage == 0)
		{
			return WorldConstants.uvRectZero;
		}
		return WorldConstants.rectCracks[Mathf.Min((int)((float)_blockValue.damage * 10f / (float)_blockValue.Block.MaxDamage), WorldConstants.rectCracks.Length - 1)];
	}

	// Token: 0x04004817 RID: 18455
	public const int ChunkBlockXPow = 4;

	// Token: 0x04004818 RID: 18456
	public const int ChunkBlockYPow = 8;

	// Token: 0x04004819 RID: 18457
	public const int ChunkBlockZPow = 4;

	// Token: 0x0400481A RID: 18458
	public const int ChunkBlockLayerHeight = 4;

	// Token: 0x0400481B RID: 18459
	public const int ChunkBlockLayerHeightPow = 2;

	// Token: 0x0400481C RID: 18460
	public const int ChunkBlockLayerHeightMask = 3;

	// Token: 0x0400481D RID: 18461
	public const int ChunkBlockLayers = 64;

	// Token: 0x0400481E RID: 18462
	public const int ChunkBlockXDim = 16;

	// Token: 0x0400481F RID: 18463
	public const int ChunkBlockYDim = 256;

	// Token: 0x04004820 RID: 18464
	public const int ChunkBlockZDim = 16;

	// Token: 0x04004821 RID: 18465
	public const int ChunkBlockXDimM1 = 15;

	// Token: 0x04004822 RID: 18466
	public const int ChunkBlockYDimM1 = 255;

	// Token: 0x04004823 RID: 18467
	public const int ChunkBlockZDimM1 = 15;

	// Token: 0x04004824 RID: 18468
	public const int ChunkAreaDim = 256;

	// Token: 0x04004825 RID: 18469
	public const int ChunkVolumeDim = 65536;

	// Token: 0x04004826 RID: 18470
	public const int ChunkBlockXMask = 15;

	// Token: 0x04004827 RID: 18471
	public const int ChunkBlockYMask = 255;

	// Token: 0x04004828 RID: 18472
	public const int ChunkBlockZMask = 15;

	// Token: 0x04004829 RID: 18473
	public const int ChunkMeshLayerHeight = 16;

	// Token: 0x0400482A RID: 18474
	public const int ChunkMeshLayerShift = 4;

	// Token: 0x0400482B RID: 18475
	public const int ChunkMeshLayerHeightMask = 65535;

	// Token: 0x0400482C RID: 18476
	public const int ChunkDensityXPow = 4;

	// Token: 0x0400482D RID: 18477
	public const int ChunkDensityYPow = 8;

	// Token: 0x0400482E RID: 18478
	public const int ChunkDensityZPow = 4;

	// Token: 0x0400482F RID: 18479
	public const int ChunkDensityXDim = 16;

	// Token: 0x04004830 RID: 18480
	public const int ChunkDensityYDim = 256;

	// Token: 0x04004831 RID: 18481
	public const int ChunkDensityZDim = 16;

	// Token: 0x04004832 RID: 18482
	public const int ChunkDensityXMask = 15;

	// Token: 0x04004833 RID: 18483
	public const int ChunkDensityYMask = 255;

	// Token: 0x04004834 RID: 18484
	public const int ChunkDensityZMask = 15;

	// Token: 0x04004835 RID: 18485
	public const int NumberOfLightShades = 16;

	// Token: 0x04004836 RID: 18486
	public const int MaxDarkness = 16;

	// Token: 0x04004837 RID: 18487
	public const int cTimePerHour = 1000;

	// Token: 0x04004838 RID: 18488
	public const int cTimePerDay = 24000;

	// Token: 0x04004839 RID: 18489
	public const int cDuskHour = 22;

	// Token: 0x0400483A RID: 18490
	public static float WaterLevel = Block.cWaterLevel;

	// Token: 0x0400483B RID: 18491
	public static Rect uvRectZero = new Rect(0f, 0f, 0f, 0f);

	// Token: 0x0400483C RID: 18492
	[PublicizedFrom(EAccessModifier.Private)]
	public static Rect[] rectCracks = new Rect[]
	{
		new Rect(0f, 0f, 0.1f, 1f),
		new Rect(0.1f, 0f, 0.1f, 1f),
		new Rect(0.2f, 0f, 0.1f, 1f),
		new Rect(0.3f, 0f, 0.1f, 1f),
		new Rect(0.4f, 0f, 0.1f, 1f),
		new Rect(0.5f, 0f, 0.1f, 1f),
		new Rect(0.6f, 0f, 0.1f, 1f),
		new Rect(0.7f, 0f, 0.1f, 1f),
		new Rect(0.8f, 0f, 0.1f, 1f),
		new Rect(0.9f, 0f, 0.1f, 1f)
	};
}
