using System;
using UnityEngine;

// Token: 0x02000C19 RID: 3097
public class DistantChunkMapInfo
{
	// Token: 0x0400497F RID: 18815
	public Vector4[] ChunkTriggerArea;

	// Token: 0x04004980 RID: 18816
	public Vector2i[][] ChunkToDelete;

	// Token: 0x04004981 RID: 18817
	public Vector2i[][] ChunkToAdd;

	// Token: 0x04004982 RID: 18818
	public Vector2i[][] ChunkToConvDel;

	// Token: 0x04004983 RID: 18819
	public Vector2i[][] ChunkToConvAdd;

	// Token: 0x04004984 RID: 18820
	public Vector4[][] ChunkToConvAddEdgeFactor;

	// Token: 0x04004985 RID: 18821
	public Vector4[][] ChunkToAddEdgeFactor;

	// Token: 0x04004986 RID: 18822
	public Vector2i[][][] ChunkEdgeToOwnResLevel;

	// Token: 0x04004987 RID: 18823
	public int[][][] ChunkEdgeToOwnRLEdgeId;

	// Token: 0x04004988 RID: 18824
	public Vector2i[][][] ChunkEdgeToNextResLevel;

	// Token: 0x04004989 RID: 18825
	public int[][][] ChunkEdgeToNextRLEdgeId;

	// Token: 0x0400498A RID: 18826
	public Vector2i[] ChunkLLIntPos;

	// Token: 0x0400498B RID: 18827
	public Vector2[] ChunkLLPos;

	// Token: 0x0400498C RID: 18828
	public int[][] NeighbResLevel;

	// Token: 0x0400498D RID: 18829
	public float[][] EdgeResFactor;

	// Token: 0x0400498E RID: 18830
	public float NextResLevelEdgeFactor;

	// Token: 0x0400498F RID: 18831
	public int NbChunk;

	// Token: 0x04004990 RID: 18832
	public int NbCurChunkInOneNextLevelChunk;

	// Token: 0x04004991 RID: 18833
	public int ResLevel;

	// Token: 0x04004992 RID: 18834
	public int LayerId = 28;

	// Token: 0x04004993 RID: 18835
	public int ChunkDataListResLevel;

	// Token: 0x04004994 RID: 18836
	public int ChunkResolution;

	// Token: 0x04004995 RID: 18837
	public int ColliderResolution;

	// Token: 0x04004996 RID: 18838
	public bool IsColliderEnabled;

	// Token: 0x04004997 RID: 18839
	public float ResRadius;

	// Token: 0x04004998 RID: 18840
	public int IntResRadius;

	// Token: 0x04004999 RID: 18841
	public Vector2i LLIntArea;

	// Token: 0x0400499A RID: 18842
	public float ChunkWidth;

	// Token: 0x0400499B RID: 18843
	public float UnitStep;

	// Token: 0x0400499C RID: 18844
	public Vector2 ShiftVec;

	// Token: 0x0400499D RID: 18845
	public Vector3 ChunkExtraShiftVector;

	// Token: 0x0400499E RID: 18846
	public DistantChunkBasicMesh BaseMesh;

	// Token: 0x0400499F RID: 18847
	public int[][] EdgeMap;

	// Token: 0x040049A0 RID: 18848
	public int[] SouthMap;

	// Token: 0x040049A1 RID: 18849
	public int[] WestMap;

	// Token: 0x040049A2 RID: 18850
	public int[] NorthMap;

	// Token: 0x040049A3 RID: 18851
	public int[] EastMap;
}
