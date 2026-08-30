using System;
using UnityEngine;

// Token: 0x02000C13 RID: 3091
public class DChunkSquareMesh
{
	// Token: 0x06005E44 RID: 24132 RVA: 0x0024C8B8 File Offset: 0x0024AAB8
	public DChunkSquareMesh(DistantChunkMap DCMap, int LODLevel)
	{
		DistantChunkMapInfo distantChunkMapInfo = DCMap.ChunkMapInfoArray[LODLevel];
		this.Init(distantChunkMapInfo.BaseMesh.Vertices.Length, LODLevel, distantChunkMapInfo.ChunkResolution, distantChunkMapInfo.ColliderResolution, DCMap.NbResLevel, distantChunkMapInfo.BaseMesh.Triangles.Length);
	}

	// Token: 0x06005E45 RID: 24133 RVA: 0x0024C918 File Offset: 0x0024AB18
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

	// Token: 0x04004918 RID: 18712
	public Vector3[] Normals;

	// Token: 0x04004919 RID: 18713
	public Vector4[] Tangents;

	// Token: 0x0400491A RID: 18714
	public Vector3[] EdgeCorNormals;

	// Token: 0x0400491B RID: 18715
	public Color[] Colors;

	// Token: 0x0400491C RID: 18716
	public int[] TextureId;

	// Token: 0x0400491D RID: 18717
	public Bounds ChunkBound;

	// Token: 0x0400491E RID: 18718
	public int WaterPlaneBlockId;

	// Token: 0x0400491F RID: 18719
	public Vector3[] ColVertices;

	// Token: 0x04004920 RID: 18720
	public float[] ColVerticesHeight;

	// Token: 0x04004921 RID: 18721
	public VoxelMeshTerrain VoxelMesh = new VoxelMeshTerrain(0, 500);

	// Token: 0x04004922 RID: 18722
	public bool[] IsWater;
}
