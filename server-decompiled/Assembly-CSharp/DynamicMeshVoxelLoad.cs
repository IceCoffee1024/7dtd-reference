using System;
using System.Collections;
using System.Collections.Concurrent;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x020003B9 RID: 953
public class DynamicMeshVoxelLoad
{
	// Token: 0x17000354 RID: 852
	// (get) Token: 0x06001C82 RID: 7298 RVA: 0x000AA7F4 File Offset: 0x000A89F4
	// (set) Token: 0x06001C83 RID: 7299 RVA: 0x000AA7FC File Offset: 0x000A89FC
	public DynamicMeshItem Item { get; set; }

	// Token: 0x17000355 RID: 853
	// (get) Token: 0x06001C84 RID: 7300 RVA: 0x000AA805 File Offset: 0x000A8A05
	// (set) Token: 0x06001C85 RID: 7301 RVA: 0x000AA80D File Offset: 0x000A8A0D
	public DyMeshData Data { get; set; }

	// Token: 0x06001C86 RID: 7302 RVA: 0x000AA816 File Offset: 0x000A8A16
	public void DisposeMeshes()
	{
		this.Data = DyMeshData.AddToCache(this.Data);
	}

	// Token: 0x06001C87 RID: 7303 RVA: 0x000AA829 File Offset: 0x000A8A29
	public IEnumerator CreateMeshCoroutine(DynamicMeshItem item)
	{
		DateTime start = DateTime.Now;
		if (item.Key != this.Item.Key)
		{
			Log.Error("mismatching items in Mesh creation");
		}
		GameObject child = DynamicMeshItem.GetItemMeshRendererFromPool();
		child.SetActive(false);
		bool hasMeshes = false;
		VoxelMesh opaqueMesh = this.Data.OpaqueMesh;
		if (opaqueMesh.Vertices.Count > 0)
		{
			hasMeshes = true;
			MeshFilter component = child.GetComponent<MeshFilter>();
			MeshRenderer component2 = child.GetComponent<MeshRenderer>();
			Mesh mesh;
			if (component.sharedMesh == null)
			{
				mesh = new Mesh();
			}
			else
			{
				mesh = component.sharedMesh;
			}
			mesh.indexFormat = ((opaqueMesh.Vertices.Count > 65535) ? IndexFormat.UInt32 : IndexFormat.UInt16);
			component.sharedMesh = mesh;
			opaqueMesh.CopyToMesh(component, component2, 0, null);
			if ((DateTime.Now - start).TotalMilliseconds > 3.0)
			{
				start = DateTime.Now;
				yield return null;
			}
		}
		if ((DateTime.Now - start).TotalMilliseconds > 3.0)
		{
			start = DateTime.Now;
			yield return null;
		}
		VoxelMeshTerrain terrainMesh = this.Data.TerrainMesh;
		if (terrainMesh != null && terrainMesh.Vertices != null && terrainMesh.Vertices.Count != 0)
		{
			hasMeshes = true;
			GameObject terrainMeshRendererFromPool = DynamicMeshItem.GetTerrainMeshRendererFromPool();
			terrainMeshRendererFromPool.SetActive(true);
			terrainMeshRendererFromPool.transform.parent = child.transform;
			MeshFilter component3 = terrainMeshRendererFromPool.GetComponent<MeshFilter>();
			Mesh mesh2;
			if (component3.sharedMesh == null)
			{
				mesh2 = new Mesh();
			}
			else
			{
				mesh2 = component3.sharedMesh;
			}
			mesh2.indexFormat = ((mesh2.vertexCount > 65535) ? IndexFormat.UInt32 : IndexFormat.UInt16);
			component3.sharedMesh = mesh2;
			DynamicMeshVoxelLoad.CopyTerrain(terrainMesh, mesh2, component3, new MeshTiming(), this.Item);
			if ((DateTime.Now - start).TotalMilliseconds > 3.0)
			{
				start = DateTime.Now;
				yield return null;
			}
		}
		if (hasMeshes)
		{
			item.ChunkObject = child;
		}
		else
		{
			DynamicMeshManager.MeshDestroy(child);
		}
		yield break;
	}

	// Token: 0x06001C88 RID: 7304 RVA: 0x000AA840 File Offset: 0x000A8A40
	public static void CopyTerrain(VoxelMeshTerrain terrain, Mesh mesh, MeshFilter filter, MeshTiming time, DynamicMeshItem item)
	{
		time.Reset();
		MeshUnsafeCopyHelper.CopyVertices(terrain.Vertices, mesh);
		time.CopyVerts = time.GetTime();
		time.Reset();
		MeshUnsafeCopyHelper.CopyUV(terrain.Uvs, mesh);
		time.CopyUv = time.time;
		time.Reset();
		MeshUnsafeCopyHelper.CopyUV2(terrain.UvsCrack, mesh);
		time.CopyUv2 = time.time;
		if (((terrain != null) ? terrain.Uvs3.Items : null) != null)
		{
			time.Reset();
			MeshUnsafeCopyHelper.CopyUV3(terrain.Uvs3, mesh);
			time.CopyUv3 = time.time;
		}
		if (((terrain != null) ? terrain.Uvs4.Items : null) != null)
		{
			time.Reset();
			MeshUnsafeCopyHelper.CopyUV4(terrain.Uvs4, mesh);
			time.CopyUv4 = time.time;
		}
		time.Reset();
		MeshUnsafeCopyHelper.CopyColors(terrain.ColorVertices, mesh);
		time.CopyColours = time.time;
		time.Reset();
		if (terrain.Indices.Count > 0)
		{
			MeshUnsafeCopyHelper.CopyTriangles(terrain.Indices, mesh);
		}
		else
		{
			mesh.subMeshCount = ((terrain.Indices.Count > 0) ? 1 : terrain.submeshes.Count);
			for (int i = 0; i < terrain.submeshes.Count; i++)
			{
				MeshUnsafeCopyHelper.CopyTriangles(terrain.submeshes[i].triangles, mesh, i);
			}
		}
		time.CopyTriangles = time.time;
		if (terrain.Normals.Count == 0)
		{
			time.Reset();
			mesh.RecalculateNormals();
			time.NormalRecalc = time.time;
		}
		else
		{
			time.Reset();
			MeshUnsafeCopyHelper.CopyNormals(terrain.Normals, mesh);
			time.CopyNormals = time.time;
		}
		time.Reset();
		time.CopyTangents = time.time;
		time.Reset();
		GameUtils.SetMeshVertexAttributes(mesh, true);
		mesh.UploadMeshData(false);
		time.UploadMesh = time.time;
		Renderer component = filter.GetComponent<Renderer>();
		int num = component.sharedMaterials.Length;
		if (!DynamicMeshFile.TerrainSharedMaterials.ContainsKey(num))
		{
			Material[] array = new Material[num];
			for (int j = 0; j < array.Length; j++)
			{
				array[j] = MeshDescription.meshes[5].material;
			}
			DynamicMeshFile.TerrainSharedMaterials.Add(num, array);
		}
		component.sharedMaterials = DynamicMeshFile.TerrainSharedMaterials[num];
	}

	// Token: 0x06001C89 RID: 7305 RVA: 0x000AAA7F File Offset: 0x000A8C7F
	public static DynamicMeshVoxelLoad Create(DynamicMeshItem item, DyMeshData data)
	{
		return new DynamicMeshVoxelLoad
		{
			Item = item,
			Data = data
		};
	}

	// Token: 0x04001281 RID: 4737
	public static ConcurrentQueue<VoxelMeshLayer> LayerCache = new ConcurrentQueue<VoxelMeshLayer>();
}
