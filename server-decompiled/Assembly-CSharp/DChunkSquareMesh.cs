using System;
using UnityEngine;

// Token: 0x02000C37 RID: 3127
public class DChunkSquareMesh
{
	// Token: 0x06005EE7 RID: 24295 RVA: 0x00252A64 File Offset: 0x00250C64
	public DChunkSquareMesh(DistantChunkMap DCMap, int LODLevel)
	{
		DistantChunkMapInfo distantChunkMapInfo = DCMap.ChunkMapInfoArray[LODLevel];
		this.Init(distantChunkMapInfo.BaseMesh.Vertices.Length, LODLevel, distantChunkMapInfo.ChunkResolution, distantChunkMapInfo.ColliderResolution, DCMap.NbResLevel, distantChunkMapInfo.BaseMesh.Triangles.Length);
	}

	// Token: 0x06005EE8 RID: 24296 RVA: 0x00252AC4 File Offset: 0x00250CC4
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

	// Token: 0x040049F4 RID: 18932
	public Vector3[] Normals;

	// Token: 0x040049F5 RID: 18933
	public Vector4[] Tangents;

	// Token: 0x040049F6 RID: 18934
	public Vector3[] EdgeCorNormals;

	// Token: 0x040049F7 RID: 18935
	public Color[] Colors;

	// Token: 0x040049F8 RID: 18936
	public int[] TextureId;

	// Token: 0x040049F9 RID: 18937
	public Bounds ChunkBound;

	// Token: 0x040049FA RID: 18938
	public int WaterPlaneBlockId;

	// Token: 0x040049FB RID: 18939
	public Vector3[] ColVertices;

	// Token: 0x040049FC RID: 18940
	public float[] ColVerticesHeight;

	// Token: 0x040049FD RID: 18941
	public VoxelMeshTerrain VoxelMesh = new VoxelMeshTerrain(0, 500);

	// Token: 0x040049FE RID: 18942
	public bool[] IsWater;
}
