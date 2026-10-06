using System;
using UnityEngine;

// Token: 0x02000C1B RID: 3099
public class WorldConstants
{
	// Token: 0x06005E1E RID: 24094 RVA: 0x0024B6C0 File Offset: 0x002498C0
	public static Rect MapBlockToUVRect(int _meshIndex, BlockValue _blockValue, BlockFace _blockFace)
	{
		int sideTextureId = _blockValue.Block.GetSideTextureId(_blockValue, _blockFace, 0);
		if (sideTextureId >= 0 && sideTextureId < MeshDescription.meshes[_meshIndex].textureAtlas.uvMapping.Length)
		{
			return MeshDescription.meshes[_meshIndex].textureAtlas.uvMapping[sideTextureId].uv;
		}
		return WorldConstants.uvRectZero;
	}

	// Token: 0x06005E1F RID: 24095 RVA: 0x0024B71C File Offset: 0x0024991C
	public static Rect MapDamageToUVRect(BlockValue _blockValue)
	{
		if (_blockValue.hasdecal || _blockValue.ischild || _blockValue.damage == 0)
		{
			return WorldConstants.uvRectZero;
		}
		return WorldConstants.rectCracks[Mathf.Min((int)((float)_blockValue.damage * 10f / (float)_blockValue.Block.MaxDamage), WorldConstants.rectCracks.Length - 1)];
	}

	// Token: 0x040048F2 RID: 18674
	public const int ChunkBlockXPow = 4;

	// Token: 0x040048F3 RID: 18675
	public const int ChunkBlockYPow = 8;

	// Token: 0x040048F4 RID: 18676
	public const int ChunkBlockZPow = 4;

	// Token: 0x040048F5 RID: 18677
	public const int ChunkBlockLayerHeight = 4;

	// Token: 0x040048F6 RID: 18678
	public const int ChunkBlockLayerHeightPow = 2;

	// Token: 0x040048F7 RID: 18679
	public const int ChunkBlockLayerHeightMask = 3;

	// Token: 0x040048F8 RID: 18680
	public const int ChunkBlockLayers = 64;

	// Token: 0x040048F9 RID: 18681
	public const int ChunkBlockXDim = 16;

	// Token: 0x040048FA RID: 18682
	public const int ChunkBlockYDim = 256;

	// Token: 0x040048FB RID: 18683
	public const int ChunkBlockZDim = 16;

	// Token: 0x040048FC RID: 18684
	public const int ChunkBlockXDimM1 = 15;

	// Token: 0x040048FD RID: 18685
	public const int ChunkBlockYDimM1 = 255;

	// Token: 0x040048FE RID: 18686
	public const int ChunkBlockZDimM1 = 15;

	// Token: 0x040048FF RID: 18687
	public const int ChunkAreaDim = 256;

	// Token: 0x04004900 RID: 18688
	public const int ChunkVolumeDim = 65536;

	// Token: 0x04004901 RID: 18689
	public const int ChunkBlockXMask = 15;

	// Token: 0x04004902 RID: 18690
	public const int ChunkBlockYMask = 255;

	// Token: 0x04004903 RID: 18691
	public const int ChunkBlockZMask = 15;

	// Token: 0x04004904 RID: 18692
	public const int ChunkMeshLayerHeight = 16;

	// Token: 0x04004905 RID: 18693
	public const int ChunkMeshLayerShift = 4;

	// Token: 0x04004906 RID: 18694
	public const int ChunkMeshLayerHeightMask = 65535;

	// Token: 0x04004907 RID: 18695
	public const int ChunkDensityXPow = 4;

	// Token: 0x04004908 RID: 18696
	public const int ChunkDensityYPow = 8;

	// Token: 0x04004909 RID: 18697
	public const int ChunkDensityZPow = 4;

	// Token: 0x0400490A RID: 18698
	public const int ChunkDensityXDim = 16;

	// Token: 0x0400490B RID: 18699
	public const int ChunkDensityYDim = 256;

	// Token: 0x0400490C RID: 18700
	public const int ChunkDensityZDim = 16;

	// Token: 0x0400490D RID: 18701
	public const int ChunkDensityXMask = 15;

	// Token: 0x0400490E RID: 18702
	public const int ChunkDensityYMask = 255;

	// Token: 0x0400490F RID: 18703
	public const int ChunkDensityZMask = 15;

	// Token: 0x04004910 RID: 18704
	public const int NumberOfLightShades = 16;

	// Token: 0x04004911 RID: 18705
	public const int MaxDarkness = 16;

	// Token: 0x04004912 RID: 18706
	public const int cTimePerHour = 1000;

	// Token: 0x04004913 RID: 18707
	public const int cTimePerDay = 24000;

	// Token: 0x04004914 RID: 18708
	public const int cDuskHour = 22;

	// Token: 0x04004915 RID: 18709
	public static float WaterLevel = Block.cWaterLevel;

	// Token: 0x04004916 RID: 18710
	public static Rect uvRectZero = new Rect(0f, 0f, 0f, 0f);

	// Token: 0x04004917 RID: 18711
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
