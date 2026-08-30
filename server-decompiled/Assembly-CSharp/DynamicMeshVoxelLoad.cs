using System;
using System.Collections;
using System.Collections.Concurrent;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x020003B2 RID: 946
public class DynamicMeshVoxelLoad
{
	// Token: 0x17000358 RID: 856
	// (get) Token: 0x06001C6C RID: 7276 RVA: 0x000A9E74 File Offset: 0x000A8074
	// (set) Token: 0x06001C6D RID: 7277 RVA: 0x000A9E7C File Offset: 0x000A807C
	public DynamicMeshItem Item { get; set; }

	// Token: 0x17000359 RID: 857
	// (get) Token: 0x06001C6E RID: 7278 RVA: 0x000A9E85 File Offset: 0x000A8085
	// (set) Token: 0x06001C6F RID: 7279 RVA: 0x000A9E8D File Offset: 0x000A808D
	public DyMeshData Data { get; set; }

	// Token: 0x06001C70 RID: 7280 RVA: 0x000A9E96 File Offset: 0x000A8096
	public void DisposeMeshes()
	{
		this.Data = DyMeshData.AddToCache(this.Data);
	}

	// Token: 0x06001C71 RID: 7281 RVA: 0x000A9EA9 File Offset: 0x000A80A9
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

	// Token: 0x06001C72 RID: 7282 RVA: 0x000A9EC0 File Offset: 0x000A80C0
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

	// Token: 0x06001C73 RID: 7283 RVA: 0x000AA0FF File Offset: 0x000A82FF
	public static DynamicMeshVoxelLoad Create(DynamicMeshItem item, DyMeshData data)
	{
		return new DynamicMeshVoxelLoad
		{
			Item = item,
			Data = data
		};
	}

	// Token: 0x04001273 RID: 4723
	public static ConcurrentQueue<VoxelMeshLayer> LayerCache = new ConcurrentQueue<VoxelMeshLayer>();
}
