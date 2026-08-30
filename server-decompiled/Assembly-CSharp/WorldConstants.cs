using System;
using UnityEngine;

// Token: 0x02000C09 RID: 3081
public class WorldConstants
{
	// Token: 0x06005E05 RID: 24069 RVA: 0x002483C4 File Offset: 0x002465C4
	public static Rect MapBlockToUVRect(int _meshIndex, BlockValue _blockValue, BlockFace _blockFace)
	{
		int sideTextureId = _blockValue.Block.GetSideTextureId(_blockValue, _blockFace, 0);
		if (sideTextureId >= 0 && sideTextureId < MeshDescription.meshes[_meshIndex].textureAtlas.uvMapping.Length)
		{
			return MeshDescription.meshes[_meshIndex].textureAtlas.uvMapping[sideTextureId].uv;
		}
		return WorldConstants.uvRectZero;
	}

	// Token: 0x06005E06 RID: 24070 RVA: 0x00248420 File Offset: 0x00246620
	public static Rect MapDamageToUVRect(BlockValue _blockValue)
	{
		if (_blockValue.hasdecal || _blockValue.ischild || _blockValue.damage == 0)
		{
			return WorldConstants.uvRectZero;
		}
		return WorldConstants.rectCracks[Mathf.Min((int)((float)_blockValue.damage * 10f / (float)_blockValue.Block.MaxDamage), WorldConstants.rectCracks.Length - 1)];
	}

	// Token: 0x040048B3 RID: 18611
	public const int ChunkBlockXPow = 4;

	// Token: 0x040048B4 RID: 18612
	public const int ChunkBlockYPow = 8;

	// Token: 0x040048B5 RID: 18613
	public const int ChunkBlockZPow = 4;

	// Token: 0x040048B6 RID: 18614
	public const int ChunkBlockLayerHeight = 4;

	// Token: 0x040048B7 RID: 18615
	public const int ChunkBlockLayerHeightPow = 2;

	// Token: 0x040048B8 RID: 18616
	public const int ChunkBlockLayerHeightMask = 3;

	// Token: 0x040048B9 RID: 18617
	public const int ChunkBlockLayers = 64;

	// Token: 0x040048BA RID: 18618
	public const int ChunkBlockXDim = 16;

	// Token: 0x040048BB RID: 18619
	public const int ChunkBlockYDim = 256;

	// Token: 0x040048BC RID: 18620
	public const int ChunkBlockZDim = 16;

	// Token: 0x040048BD RID: 18621
	public const int ChunkBlockXDimM1 = 15;

	// Token: 0x040048BE RID: 18622
	public const int ChunkBlockYDimM1 = 255;

	// Token: 0x040048BF RID: 18623
	public const int ChunkBlockZDimM1 = 15;

	// Token: 0x040048C0 RID: 18624
	public const int ChunkAreaDim = 256;

	// Token: 0x040048C1 RID: 18625
	public const int ChunkVolumeDim = 65536;

	// Token: 0x040048C2 RID: 18626
	public const int ChunkBlockXMask = 15;

	// Token: 0x040048C3 RID: 18627
	public const int ChunkBlockYMask = 255;

	// Token: 0x040048C4 RID: 18628
	public const int ChunkBlockZMask = 15;

	// Token: 0x040048C5 RID: 18629
	public const int ChunkMeshLayerHeight = 16;

	// Token: 0x040048C6 RID: 18630
	public const int ChunkMeshLayerShift = 4;

	// Token: 0x040048C7 RID: 18631
	public const int ChunkMeshLayerHeightMask = 65535;

	// Token: 0x040048C8 RID: 18632
	public const int ChunkDensityXPow = 4;

	// Token: 0x040048C9 RID: 18633
	public const int ChunkDensityYPow = 8;

	// Token: 0x040048CA RID: 18634
	public const int ChunkDensityZPow = 4;

	// Token: 0x040048CB RID: 18635
	public const int ChunkDensityXDim = 16;

	// Token: 0x040048CC RID: 18636
	public const int ChunkDensityYDim = 256;

	// Token: 0x040048CD RID: 18637
	public const int ChunkDensityZDim = 16;

	// Token: 0x040048CE RID: 18638
	public const int ChunkDensityXMask = 15;

	// Token: 0x040048CF RID: 18639
	public const int ChunkDensityYMask = 255;

	// Token: 0x040048D0 RID: 18640
	public const int ChunkDensityZMask = 15;

	// Token: 0x040048D1 RID: 18641
	public const int NumberOfLightShades = 16;

	// Token: 0x040048D2 RID: 18642
	public const int MaxDarkness = 16;

	// Token: 0x040048D3 RID: 18643
	public const int cTimePerHour = 1000;

	// Token: 0x040048D4 RID: 18644
	public const int cTimePerDay = 24000;

	// Token: 0x040048D5 RID: 18645
	public const int cDuskHour = 22;

	// Token: 0x040048D6 RID: 18646
	public static float WaterLevel = Block.cWaterLevel;

	// Token: 0x040048D7 RID: 18647
	public static Rect uvRectZero = new Rect(0f, 0f, 0f, 0f);

	// Token: 0x040048D8 RID: 18648
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
