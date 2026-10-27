using System;
using UnityEngine;

// Token: 0x02000C42 RID: 3138
public class DChunkSquareMesh
{
	// Token: 0x06005F0C RID: 24332 RVA: 0x00253F8C File Offset: 0x0025218C
	public DChunkSquareMesh(DistantChunkMap DCMap, int LODLevel)
	{
		DistantChunkMapInfo distantChunkMapInfo = DCMap.ChunkMapInfoArray[LODLevel];
		this.Init(distantChunkMapInfo.BaseMesh.Vertices.Length, LODLevel, distantChunkMapInfo.ChunkResolution, distantChunkMapInfo.ColliderResolution, DCMap.NbResLevel, distantChunkMapInfo.BaseMesh.Triangles.Length);
	}

	// Token: 0x06005F0D RID: 24333 RVA: 0x00253FEC File Offset: 0x002521EC
	public void Init(int NbVertices, int ResLevel, int Resolution, int ColliderResolution, int MaxNbResLevel, int NbTriangles)
	{
		this.Normals = new Vector3[NbVertices];
		this.Tangents = new Vector4[NbVertices];
		this.EdgeCorNormals = new Vector3[Resolution * 4];
		this.Colors = new Color[NbVertices];
		this.TextureId = new int[NbVertices];
		this.IsWater = new bool[NbVertices];
		this.ChunkBound = default(Bounds);
		this.ColVertices = new Vector3[ColliderResolution * ColliderResolution];
		this.ColVerticesHeight = new float[ColliderResolution * ColliderResolution];
	}

	// Token: 0x04004A19 RID: 18969
	public Vector3[] Normals;

	// Token: 0x04004A1A RID: 18970
	public Vector4[] Tangents;

	// Token: 0x04004A1B RID: 18971
	public Vector3[] EdgeCorNormals;

	// Token: 0x04004A1C RID: 18972
	public Color[] Colors;

	// Token: 0x04004A1D RID: 18973
	public int[] TextureId;

	// Token: 0x04004A1E RID: 18974
	public Bounds ChunkBound;

	// Token: 0x04004A1F RID: 18975
	public int WaterPlaneBlockId;

	// Token: 0x04004A20 RID: 18976
	public Vector3[] ColVertices;

	// Token: 0x04004A21 RID: 18977
	public float[] ColVerticesHeight;

	// Token: 0x04004A22 RID: 18978
	public VoxelMeshTerrain VoxelMesh = new VoxelMeshTerrain(0, 500);

	// Token: 0x04004A23 RID: 18979
	public bool[] IsWater;
}
