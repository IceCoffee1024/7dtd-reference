using System;
using UnityEngine;

// Token: 0x02000C3D RID: 3133
public class DistantChunkMapInfo
{
	// Token: 0x04004A5B RID: 19035
	public Vector4[] ChunkTriggerArea;

	// Token: 0x04004A5C RID: 19036
	public Vector2i[][] ChunkToDelete;

	// Token: 0x04004A5D RID: 19037
	public Vector2i[][] ChunkToAdd;

	// Token: 0x04004A5E RID: 19038
	public Vector2i[][] ChunkToConvDel;

	// Token: 0x04004A5F RID: 19039
	public Vector2i[][] ChunkToConvAdd;

	// Token: 0x04004A60 RID: 19040
	public Vector4[][] ChunkToConvAddEdgeFactor;

	// Token: 0x04004A61 RID: 19041
	public Vector4[][] ChunkToAddEdgeFactor;

	// Token: 0x04004A62 RID: 19042
	public Vector2i[][][] ChunkEdgeToOwnResLevel;

	// Token: 0x04004A63 RID: 19043
	public int[][][] ChunkEdgeToOwnRLEdgeId;

	// Token: 0x04004A64 RID: 19044
	public Vector2i[][][] ChunkEdgeToNextResLevel;

	// Token: 0x04004A65 RID: 19045
	public int[][][] ChunkEdgeToNextRLEdgeId;

	// Token: 0x04004A66 RID: 19046
	public Vector2i[] ChunkLLIntPos;

	// Token: 0x04004A67 RID: 19047
	public Vector2[] ChunkLLPos;

	// Token: 0x04004A68 RID: 19048
	public int[][] NeighbResLevel;

	// Token: 0x04004A69 RID: 19049
	public float[][] EdgeResFactor;

	// Token: 0x04004A6A RID: 19050
	public float NextResLevelEdgeFactor;

	// Token: 0x04004A6B RID: 19051
	public int NbChunk;

	// Token: 0x04004A6C RID: 19052
	public int NbCurChunkInOneNextLevelChunk;

	// Token: 0x04004A6D RID: 19053
	public int ResLevel;

	// Token: 0x04004A6E RID: 19054
	public int LayerId = 28;

	// Token: 0x04004A6F RID: 19055
	public int ChunkDataListResLevel;

	// Token: 0x04004A70 RID: 19056
	public int ChunkResolution;

	// Token: 0x04004A71 RID: 19057
	public int ColliderResolution;

	// Token: 0x04004A72 RID: 19058
	public bool IsColliderEnabled;

	// Token: 0x04004A73 RID: 19059
	public float ResRadius;

	// Token: 0x04004A74 RID: 19060
	public int IntResRadius;

	// Token: 0x04004A75 RID: 19061
	public Vector2i LLIntArea;

	// Token: 0x04004A76 RID: 19062
	public float ChunkWidth;

	// Token: 0x04004A77 RID: 19063
	public float UnitStep;

	// Token: 0x04004A78 RID: 19064
	public Vector2 ShiftVec;

	// Token: 0x04004A79 RID: 19065
	public Vector3 ChunkExtraShiftVector;

	// Token: 0x04004A7A RID: 19066
	public DistantChunkBasicMesh BaseMesh;

	// Token: 0x04004A7B RID: 19067
	public int[][] EdgeMap;

	// Token: 0x04004A7C RID: 19068
	public int[] SouthMap;

	// Token: 0x04004A7D RID: 19069
	public int[] WestMap;

	// Token: 0x04004A7E RID: 19070
	public int[] NorthMap;

	// Token: 0x04004A7F RID: 19071
	public int[] EastMap;
}
