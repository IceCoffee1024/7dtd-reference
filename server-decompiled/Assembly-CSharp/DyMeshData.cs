using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

// Token: 0x0200036D RID: 877
[DebuggerDisplay("{Name} O:{OpaqueMesh.Vertices.Count} T:{TerrainMesh.Vertices.Count}")]
public class DyMeshData
{
	// Token: 0x17000302 RID: 770
	// (get) Token: 0x06001993 RID: 6547 RVA: 0x00092D38 File Offset: 0x00090F38
	public static int TotalItems
	{
		get
		{
			return DyMeshData.ActiveItems + DyMeshData.Cache.Count;
		}
	}

	// Token: 0x06001994 RID: 6548 RVA: 0x00092D4A File Offset: 0x00090F4A
	public void Reset()
	{
		DyMeshData.ResetMesh(this.OpaqueMesh);
		DyMeshData.ResetTerrainMesh(this.TerrainMesh);
	}

	// Token: 0x06001995 RID: 6549 RVA: 0x00092D64 File Offset: 0x00090F64
	public static void ResetMeshLayer(VoxelMeshLayer layer)
	{
		VoxelMesh[] meshes = layer.meshes;
		for (int i = 0; i < meshes.Length; i++)
		{
			DyMeshData.ResetMesh(meshes[i]);
		}
	}

	// Token: 0x06001996 RID: 6550 RVA: 0x00092D90 File Offset: 0x00090F90
	public static void ResetMesh(VoxelMesh mesh)
	{
		VoxelMeshTerrain voxelMeshTerrain = mesh as VoxelMeshTerrain;
		if (voxelMeshTerrain != null)
		{
			DyMeshData.ResetTerrainMesh(voxelMeshTerrain);
			return;
		}
		DyMeshData.ResetOpaqueMesh(mesh);
	}

	// Token: 0x06001997 RID: 6551 RVA: 0x00092DB4 File Offset: 0x00090FB4
	public static void ResetTerrainMesh(VoxelMeshTerrain mesh)
	{
		DyMeshData.ResetOpaqueMesh(mesh);
		mesh.submeshes.Clear();
	}

	// Token: 0x06001998 RID: 6552 RVA: 0x00092DC8 File Offset: 0x00090FC8
	public static void ResetOpaqueMesh(VoxelMesh mesh)
	{
		mesh.CurTriangleIndex = 0;
		mesh.Vertices.Count = 0;
		mesh.Indices.Count = 0;
		mesh.Uvs.Count = 0;
		if (mesh.UvsCrack != null)
		{
			mesh.UvsCrack.Count = 0;
		}
		if (mesh.Uvs3 != null)
		{
			mesh.Uvs3.Count = 0;
		}
		if (mesh.Uvs4 != null)
		{
			mesh.Uvs4.Count = 0;
		}
		mesh.ColorVertices.Count = 0;
		mesh.m_Normals.Count = 0;
		mesh.m_Tangents.Count = 0;
		if (mesh.m_CollVertices != null)
		{
			mesh.m_CollVertices.Count = 0;
			mesh.m_CollIndices.Count = 0;
		}
	}

	// Token: 0x06001999 RID: 6553 RVA: 0x00092E80 File Offset: 0x00091080
	[PublicizedFrom(EAccessModifier.Private)]
	public static string DebugDyMeshdata(DyMeshData data)
	{
		string text = "\r\nOpaque\r\nVertices: {0}\r\nIndices: {1}\r\nUvs1: {2}\r\nUvs2: {3}\r\nUvs3: {4}\r\nUvs4: {5}\r\nColorVertices: {6}\r\nm_Normals: {7}\r\nm_Tangents: {8}\r\nm_CollVertices: {9}\r\nm_CollIndices: {10}\r\n\r\nTerrain\r\nVertices: {11}\r\nIndices: {12}\r\nUvs1: {13}\r\nUvs2: {14}\r\nUvs3: {15}\r\nUvs4: {16}\r\nColorVertices: {17}\r\nm_Normals: {18}\r\nm_Tangents: {19}\r\nm_CollVertices: {20}\r\nm_CollIndices: {21}\r\nsubmeshes: {22}\r\n\r\n";
		object[] array = new object[23];
		array[0] = data.OpaqueMesh.Vertices.Items.Length;
		array[1] = data.OpaqueMesh.Indices.Items.Length;
		array[2] = data.OpaqueMesh.Uvs.Items.Length;
		array[3] = data.OpaqueMesh.UvsCrack.Items.Length;
		int num = 4;
		ArrayListMP<Vector2> uvs = data.OpaqueMesh.Uvs3;
		array[num] = ((uvs != null) ? uvs.Items.Length : 0);
		int num2 = 5;
		ArrayListMP<Vector2> uvs2 = data.OpaqueMesh.Uvs4;
		array[num2] = ((uvs2 != null) ? uvs2.Items.Length : 0);
		int num3 = 6;
		ArrayListMP<Color> colorVertices = data.OpaqueMesh.ColorVertices;
		array[num3] = ((colorVertices != null) ? colorVertices.Items.Length : 0);
		int num4 = 7;
		ArrayListMP<Vector3> normals = data.OpaqueMesh.m_Normals;
		array[num4] = ((normals != null) ? normals.Items.Length : 0);
		int num5 = 8;
		ArrayListMP<Vector4> tangents = data.OpaqueMesh.m_Tangents;
		array[num5] = ((tangents != null) ? tangents.Items.Length : 0);
		int num6 = 9;
		ArrayListMP<Vector3> collVertices = data.OpaqueMesh.m_CollVertices;
		array[num6] = ((collVertices != null) ? collVertices.Items.Length : 0);
		int num7 = 10;
		ArrayListMP<int> collIndices = data.OpaqueMesh.m_CollIndices;
		array[num7] = ((collIndices != null) ? collIndices.Items.Length : 0);
		array[11] = data.TerrainMesh.Vertices.Items.Length;
		array[12] = data.TerrainMesh.Indices.Items.Length;
		array[13] = data.TerrainMesh.Uvs.Items.Length;
		array[14] = data.TerrainMesh.UvsCrack.Items.Length;
		int num8 = 15;
		ArrayListMP<Vector2> uvs3 = data.TerrainMesh.Uvs3;
		array[num8] = ((uvs3 != null) ? uvs3.Items.Length : 0);
		int num9 = 16;
		ArrayListMP<Vector2> uvs4 = data.TerrainMesh.Uvs4;
		array[num9] = ((uvs4 != null) ? uvs4.Items.Length : 0);
		int num10 = 17;
		ArrayListMP<Color> colorVertices2 = data.TerrainMesh.ColorVertices;
		array[num10] = ((colorVertices2 != null) ? colorVertices2.Items.Length : 0);
		int num11 = 18;
		ArrayListMP<Vector3> normals2 = data.TerrainMesh.m_Normals;
		array[num11] = ((normals2 != null) ? normals2.Items.Length : 0);
		int num12 = 19;
		ArrayListMP<Vector4> tangents2 = data.TerrainMesh.m_Tangents;
		array[num12] = ((tangents2 != null) ? tangents2.Items.Length : 0);
		int num13 = 20;
		ArrayListMP<Vector3> collVertices2 = data.TerrainMesh.m_CollVertices;
		array[num13] = ((collVertices2 != null) ? collVertices2.Items.Length : 0);
		int num14 = 21;
		ArrayListMP<int> collIndices2 = data.TerrainMesh.m_CollIndices;
		array[num14] = ((collIndices2 != null) ? collIndices2.Items.Length : 0);
		int num15 = 22;
		List<TerrainSubMesh> submeshes = data.TerrainMesh.submeshes;
		array[num15] = ((submeshes != null) ? submeshes.Count : 0);
		return string.Format(text, array);
	}

	// Token: 0x0600199A RID: 6554 RVA: 0x0009316C File Offset: 0x0009136C
	public static DyMeshData AddToCache(DyMeshData data)
	{
		if (data == null)
		{
			return null;
		}
		DyMeshData.ActiveItems--;
		if (DyMeshData.TotalItems < DynamicMeshSettings.MaxDyMeshData)
		{
			if (DyMeshData.Cache.Any<DyMeshData>((DyMeshData d) => d.Name == data.Name))
			{
				Log.Error("duplicate in cache: " + data.Name);
				return null;
			}
			data.Reset();
			DyMeshData.Cache.Enqueue(data);
		}
		return null;
	}

	// Token: 0x0600199B RID: 6555 RVA: 0x000931F8 File Offset: 0x000913F8
	public static DyMeshData GetFromCache()
	{
		DyMeshData dyMeshData;
		if (!DyMeshData.Cache.TryDequeue(out dyMeshData))
		{
			if (DyMeshData.TotalItems >= DynamicMeshSettings.MaxDyMeshData)
			{
				return null;
			}
			dyMeshData = DyMeshData.Create(32000, 32000);
		}
		DyMeshData.ActiveItems++;
		return dyMeshData;
	}

	// Token: 0x0600199C RID: 6556 RVA: 0x00093240 File Offset: 0x00091440
	public static DyMeshData Create(int opaqueSize = 32000, int terrainSize = 32000)
	{
		DyMeshData dyMeshData = new DyMeshData();
		dyMeshData.OpaqueMesh = new VoxelMesh(0, opaqueSize, VoxelMesh.CreateFlags.Default);
		dyMeshData.TerrainMesh = new VoxelMeshTerrain(5, terrainSize);
		int num = ++DyMeshData.InstanceCount;
		dyMeshData.Name = num.ToString();
		return dyMeshData;
	}

	// Token: 0x0400102F RID: 4143
	[PublicizedFrom(EAccessModifier.Private)]
	public static ConcurrentQueue<DyMeshData> Cache = new ConcurrentQueue<DyMeshData>();

	// Token: 0x04001030 RID: 4144
	public static int ActiveItems = 0;

	// Token: 0x04001031 RID: 4145
	[PublicizedFrom(EAccessModifier.Private)]
	public static int InstanceCount;

	// Token: 0x04001032 RID: 4146
	public string Name;

	// Token: 0x04001033 RID: 4147
	public VoxelMesh OpaqueMesh;

	// Token: 0x04001034 RID: 4148
	public VoxelMeshTerrain TerrainMesh;
}
