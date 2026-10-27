using System;
using UnityEngine;

// Token: 0x02000C48 RID: 3144
public class DistantChunkMapInfo
{
	// Token: 0x04004A80 RID: 19072
	public Vector4[] ChunkTriggerArea;

	// Token: 0x04004A81 RID: 19073
	public Vector2i[][] ChunkToDelete;

	// Token: 0x04004A82 RID: 19074
	public Vector2i[][] ChunkToAdd;

	// Token: 0x04004A83 RID: 19075
	public Vector2i[][] ChunkToConvDel;

	// Token: 0x04004A84 RID: 19076
	public Vector2i[][] ChunkToConvAdd;

	// Token: 0x04004A85 RID: 19077
	public Vector4[][] ChunkToConvAddEdgeFactor;

	// Token: 0x04004A86 RID: 19078
	public Vector4[][] ChunkToAddEdgeFactor;

	// Token: 0x04004A87 RID: 19079
	public Vector2i[][][] ChunkEdgeToOwnResLevel;

	// Token: 0x04004A88 RID: 19080
	public int[][][] ChunkEdgeToOwnRLEdgeId;

	// Token: 0x04004A89 RID: 19081
	public Vector2i[][][] ChunkEdgeToNextResLevel;

	// Token: 0x04004A8A RID: 19082
	public int[][][] ChunkEdgeToNextRLEdgeId;

	// Token: 0x04004A8B RID: 19083
	public Vector2i[] ChunkLLIntPos;

	// Token: 0x04004A8C RID: 19084
	public Vector2[] ChunkLLPos;

	// Token: 0x04004A8D RID: 19085
	public int[][] NeighbResLevel;

	// Token: 0x04004A8E RID: 19086
	public float[][] EdgeResFactor;

	// Token: 0x04004A8F RID: 19087
	public float NextResLevelEdgeFactor;

	// Token: 0x04004A90 RID: 19088
	public int NbChunk;

	// Token: 0x04004A91 RID: 19089
	public int NbCurChunkInOneNextLevelChunk;

	// Token: 0x04004A92 RID: 19090
	public int ResLevel;

	// Token: 0x04004A93 RID: 19091
	public int LayerId = 28;

	// Token: 0x04004A94 RID: 19092
	public int ChunkDataListResLevel;

	// Token: 0x04004A95 RID: 19093
	public int ChunkResolution;

	// Token: 0x04004A96 RID: 19094
	public int ColliderResolution;

	// Token: 0x04004A97 RID: 19095
	public bool IsColliderEnabled;

	// Token: 0x04004A98 RID: 19096
	public float ResRadius;

	// Token: 0x04004A99 RID: 19097
	public int IntResRadius;

	// Token: 0x04004A9A RID: 19098
	public Vector2i LLIntArea;

	// Token: 0x04004A9B RID: 19099
	public float ChunkWidth;

	// Token: 0x04004A9C RID: 19100
	public float UnitStep;

	// Token: 0x04004A9D RID: 19101
	public Vector2 ShiftVec;

	// Token: 0x04004A9E RID: 19102
	public Vector3 ChunkExtraShiftVector;

	// Token: 0x04004A9F RID: 19103
	public DistantChunkBasicMesh BaseMesh;

	// Token: 0x04004AA0 RID: 19104
	public int[][] EdgeMap;

	// Token: 0x04004AA1 RID: 19105
	public int[] SouthMap;

	// Token: 0x04004AA2 RID: 19106
	public int[] WestMap;

	// Token: 0x04004AA3 RID: 19107
	public int[] NorthMap;

	// Token: 0x04004AA4 RID: 19108
	public int[] EastMap;
}
