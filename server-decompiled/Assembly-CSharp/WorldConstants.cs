using System;
using UnityEngine;

// Token: 0x02000C26 RID: 3110
public class WorldConstants
{
	// Token: 0x06005E43 RID: 24131 RVA: 0x0024CBE8 File Offset: 0x0024ADE8
	public static Rect MapBlockToUVRect(int _meshIndex, BlockValue _blockValue, BlockFace _blockFace)
	{
		int sideTextureId = _blockValue.Block.GetSideTextureId(_blockValue, _blockFace, 0);
		if (sideTextureId >= 0 && sideTextureId < MeshDescription.meshes[_meshIndex].textureAtlas.uvMapping.Length)
		{
			return MeshDescription.meshes[_meshIndex].textureAtlas.uvMapping[sideTextureId].uv;
		}
		return WorldConstants.uvRectZero;
	}

	// Token: 0x06005E44 RID: 24132 RVA: 0x0024CC44 File Offset: 0x0024AE44
	public static Rect MapDamageToUVRect(BlockValue _blockValue)
	{
		if (_blockValue.hasdecal || _blockValue.ischild || _blockValue.damage == 0)
		{
			return WorldConstants.uvRectZero;
		}
		return WorldConstants.rectCracks[Mathf.Min((int)((float)_blockValue.damage * 10f / (float)_blockValue.Block.MaxDamage), WorldConstants.rectCracks.Length - 1)];
	}

	// Token: 0x04004917 RID: 18711
	public const int ChunkBlockXPow = 4;

	// Token: 0x04004918 RID: 18712
	public const int ChunkBlockYPow = 8;

	// Token: 0x04004919 RID: 18713
	public const int ChunkBlockZPow = 4;

	// Token: 0x0400491A RID: 18714
	public const int ChunkBlockLayerHeight = 4;

	// Token: 0x0400491B RID: 18715
	public const int ChunkBlockLayerHeightPow = 2;

	// Token: 0x0400491C RID: 18716
	public const int ChunkBlockLayerHeightMask = 3;

	// Token: 0x0400491D RID: 18717
	public const int ChunkBlockLayers = 64;

	// Token: 0x0400491E RID: 18718
	public const int ChunkBlockXDim = 16;

	// Token: 0x0400491F RID: 18719
	public const int ChunkBlockYDim = 256;

	// Token: 0x04004920 RID: 18720
	public const int ChunkBlockZDim = 16;

	// Token: 0x04004921 RID: 18721
	public const int ChunkBlockXDimM1 = 15;

	// Token: 0x04004922 RID: 18722
	public const int ChunkBlockYDimM1 = 255;

	// Token: 0x04004923 RID: 18723
	public const int ChunkBlockZDimM1 = 15;

	// Token: 0x04004924 RID: 18724
	public const int ChunkAreaDim = 256;

	// Token: 0x04004925 RID: 18725
	public const int ChunkVolumeDim = 65536;

	// Token: 0x04004926 RID: 18726
	public const int ChunkBlockXMask = 15;

	// Token: 0x04004927 RID: 18727
	public const int ChunkBlockYMask = 255;

	// Token: 0x04004928 RID: 18728
	public const int ChunkBlockZMask = 15;

	// Token: 0x04004929 RID: 18729
	public const int ChunkMeshLayerHeight = 16;

	// Token: 0x0400492A RID: 18730
	public const int ChunkMeshLayerShift = 4;

	// Token: 0x0400492B RID: 18731
	public const int ChunkMeshLayerHeightMask = 65535;

	// Token: 0x0400492C RID: 18732
	public const int ChunkDensityXPow = 4;

	// Token: 0x0400492D RID: 18733
	public const int ChunkDensityYPow = 8;

	// Token: 0x0400492E RID: 18734
	public const int ChunkDensityZPow = 4;

	// Token: 0x0400492F RID: 18735
	public const int ChunkDensityXDim = 16;

	// Token: 0x04004930 RID: 18736
	public const int ChunkDensityYDim = 256;

	// Token: 0x04004931 RID: 18737
	public const int ChunkDensityZDim = 16;

	// Token: 0x04004932 RID: 18738
	public const int ChunkDensityXMask = 15;

	// Token: 0x04004933 RID: 18739
	public const int ChunkDensityYMask = 255;

	// Token: 0x04004934 RID: 18740
	public const int ChunkDensityZMask = 15;

	// Token: 0x04004935 RID: 18741
	public const int NumberOfLightShades = 16;

	// Token: 0x04004936 RID: 18742
	public const int MaxDarkness = 16;

	// Token: 0x04004937 RID: 18743
	public const int cTimePerHour = 1000;

	// Token: 0x04004938 RID: 18744
	public const int cTimePerDay = 24000;

	// Token: 0x04004939 RID: 18745
	public const int cDuskHour = 22;

	// Token: 0x0400493A RID: 18746
	public static float WaterLevel = Block.cWaterLevel;

	// Token: 0x0400493B RID: 18747
	public static Rect uvRectZero = new Rect(0f, 0f, 0f, 0f);

	// Token: 0x0400493C RID: 18748
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
