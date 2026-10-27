using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x020003B0 RID: 944
public class DyMeshRegionLoadRequest
{
	// Token: 0x06001C0D RID: 7181 RVA: 0x000A6BB6 File Offset: 0x000A4DB6
	public static DyMeshRegionLoadRequest Create(long key)
	{
		return new DyMeshRegionLoadRequest
		{
			Key = key,
			OpaqueMesh = MeshLists.GetList(),
			TerrainMesh = MeshLists.GetList()
		};
	}

	// Token: 0x06001C0E RID: 7182 RVA: 0x000A6BDC File Offset: 0x000A4DDC
	public void CreateMeshSync(DynamicMeshRegion region)
	{
		GameObject regionMeshRendererFromPool = DynamicMeshItem.GetRegionMeshRendererFromPool();
		regionMeshRendererFromPool.name = "R: " + region.ToDebugLocation();
		regionMeshRendererFromPool.transform.position = new Vector3(0f, 0f, 0f);
		regionMeshRendererFromPool.SetActive(false);
		this.CreateOpaqueMeshSync(this.OpaqueMesh, regionMeshRendererFromPool, DynamicMeshSettings.MaxRegionLoadMsPerFrame);
		this.CreateTerrainGoSync(this.TerrainMesh, regionMeshRendererFromPool, DynamicMeshSettings.MaxRegionLoadMsPerFrame);
		region.RegionObject = regionMeshRendererFromPool;
		region.IsMeshLoaded = true;
		region.SetPosition();
		region.SetVisibleNew(region.VisibleChunks == 0 || !region.InBuffer, "create mesh sync finished", true);
	}

	// Token: 0x06001C0F RID: 7183 RVA: 0x000A6C83 File Offset: 0x000A4E83
	public IEnumerator CreateMeshCoroutine(DynamicMeshRegion region)
	{
		GameObject newRegionObject = DynamicMeshItem.GetRegionMeshRendererFromPool();
		newRegionObject.name = "R: " + region.ToDebugLocation();
		newRegionObject.transform.position = new Vector3(0f, 0f, 0f);
		newRegionObject.SetActive(false);
		yield return GameManager.Instance.StartCoroutine(this.CreateOpaqueMesh(this.OpaqueMesh, newRegionObject, DynamicMeshSettings.MaxRegionLoadMsPerFrame));
		yield return GameManager.Instance.StartCoroutine(this.CreateTerrainGo(this.TerrainMesh, newRegionObject, DynamicMeshSettings.MaxRegionLoadMsPerFrame));
		region.RegionObject = newRegionObject;
		region.IsMeshLoaded = true;
		region.SetPosition();
		region.SetVisibleNew(region.VisibleChunks == 0 || !region.InBuffer, "create mesh coroutine finished", true);
		yield break;
	}

	// Token: 0x06001C10 RID: 7184 RVA: 0x000A6C99 File Offset: 0x000A4E99
	public IEnumerator CreateOpaqueMesh(MeshLists cache, GameObject parent, int maxMsPerFrame)
	{
		if (cache.Vertices.Count == 0)
		{
			yield break;
		}
		DateTime dateTime = DateTime.Now;
		GameObject itemMeshRendererFromPool = DynamicMeshItem.GetItemMeshRendererFromPool();
		itemMeshRendererFromPool.transform.parent = parent.transform;
		itemMeshRendererFromPool.transform.localPosition = Vector3.zero;
		itemMeshRendererFromPool.SetActive(true);
		MeshFilter component = itemMeshRendererFromPool.GetComponent<MeshFilter>();
		Mesh mesh;
		if (component.mesh == null)
		{
			mesh = new Mesh();
		}
		else
		{
			mesh = component.sharedMesh;
		}
		mesh.indexFormat = ((cache.Vertices.Count < 65535) ? IndexFormat.UInt16 : IndexFormat.UInt32);
		component.sharedMesh = mesh;
		mesh.SetVertices(cache.Vertices);
		if ((DateTime.Now - dateTime).TotalMilliseconds > (double)maxMsPerFrame)
		{
			yield return null;
			dateTime = DateTime.Now;
		}
		mesh.SetUVs(0, cache.Uvs);
		if ((DateTime.Now - dateTime).TotalMilliseconds > (double)maxMsPerFrame)
		{
			yield return null;
			dateTime = DateTime.Now;
		}
		mesh.SetColors(cache.Colours);
		if ((DateTime.Now - dateTime).TotalMilliseconds > (double)maxMsPerFrame)
		{
			yield return null;
			dateTime = DateTime.Now;
		}
		mesh.SetTriangles(cache.Triangles, 0);
		if ((DateTime.Now - dateTime).TotalMilliseconds > (double)maxMsPerFrame)
		{
			yield return null;
			dateTime = DateTime.Now;
		}
		if (mesh != null)
		{
			mesh.SetNormals(cache.Normals);
		}
		if (mesh != null)
		{
			mesh.SetTangents(cache.Tangents);
		}
		if (mesh != null)
		{
			GameUtils.SetMeshVertexAttributes(mesh, true);
			mesh.UploadMeshData(DynamicMeshThread.LockMeshesAfterGenerating);
		}
		yield break;
	}

	// Token: 0x06001C11 RID: 7185 RVA: 0x000A6CB8 File Offset: 0x000A4EB8
	public void CreateOpaqueMeshSync(MeshLists cache, GameObject parent, int maxMsPerFrame)
	{
		if (cache.Vertices.Count == 0)
		{
			return;
		}
		GameObject itemMeshRendererFromPool = DynamicMeshItem.GetItemMeshRendererFromPool();
		itemMeshRendererFromPool.transform.parent = parent.transform;
		itemMeshRendererFromPool.transform.localPosition = Vector3.zero;
		itemMeshRendererFromPool.SetActive(true);
		MeshFilter component = itemMeshRendererFromPool.GetComponent<MeshFilter>();
		Mesh mesh;
		if (component.mesh == null)
		{
			mesh = new Mesh();
		}
		else
		{
			mesh = component.sharedMesh;
		}
		mesh.indexFormat = ((cache.Vertices.Count < 65535) ? IndexFormat.UInt16 : IndexFormat.UInt32);
		component.sharedMesh = mesh;
		mesh.SetVertices(cache.Vertices);
		mesh.SetUVs(0, cache.Uvs);
		mesh.SetColors(cache.Colours);
		mesh.SetTriangles(cache.Triangles, 0);
		mesh.SetNormals(cache.Normals);
		mesh.SetTangents(cache.Tangents);
		GameUtils.SetMeshVertexAttributes(mesh, true);
		mesh.UploadMeshData(DynamicMeshThread.LockMeshesAfterGenerating);
	}

	// Token: 0x06001C12 RID: 7186 RVA: 0x000A6DA2 File Offset: 0x000A4FA2
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator CreateTerrainGo(MeshLists cache, GameObject parent, int maxMsPerFrame)
	{
		if (cache.Vertices.Count == 0)
		{
			yield break;
		}
		DateTime dateTime = DateTime.Now;
		GameObject terrainMeshRendererFromPool = DynamicMeshItem.GetTerrainMeshRendererFromPool();
		terrainMeshRendererFromPool.transform.parent = parent.transform;
		terrainMeshRendererFromPool.transform.localPosition = Vector3.zero;
		terrainMeshRendererFromPool.SetActive(true);
		MeshFilter component = terrainMeshRendererFromPool.GetComponent<MeshFilter>();
		Renderer component2 = terrainMeshRendererFromPool.GetComponent<Renderer>();
		Mesh mesh;
		if (component.mesh == null)
		{
			mesh = new Mesh();
		}
		else
		{
			mesh = component.sharedMesh;
		}
		mesh.indexFormat = ((cache.Vertices.Count < 65535) ? IndexFormat.UInt16 : IndexFormat.UInt32);
		component.sharedMesh = mesh;
		try
		{
			int num = component2.sharedMaterials.Length;
			if (!DynamicMeshFile.TerrainSharedMaterials.ContainsKey(num))
			{
				Material[] array = new Material[num];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = MeshDescription.meshes[5].material;
				}
				DynamicMeshFile.TerrainSharedMaterials.Add(num, array);
			}
			component2.sharedMaterials = DynamicMeshFile.TerrainSharedMaterials[num];
		}
		catch (Exception)
		{
			Log.Warning("Setting shared material failed on terrain material.");
			yield break;
		}
		mesh.SetVertices(cache.Vertices);
		if ((DateTime.Now - dateTime).TotalMilliseconds > (double)maxMsPerFrame)
		{
			yield return null;
			dateTime = DateTime.Now;
		}
		mesh.subMeshCount = 1;
		int num2;
		for (int x = 0; x < 1; x = num2 + 1)
		{
			mesh.SetTriangles(cache.Triangles, x);
			if ((DateTime.Now - dateTime).TotalMilliseconds > (double)maxMsPerFrame)
			{
				yield return null;
				dateTime = DateTime.Now;
			}
			num2 = x;
		}
		if ((DateTime.Now - dateTime).TotalMilliseconds > (double)maxMsPerFrame)
		{
			yield return null;
			dateTime = DateTime.Now;
		}
		mesh.SetUVs(0, cache.Uvs);
		if ((DateTime.Now - dateTime).TotalMilliseconds > (double)maxMsPerFrame)
		{
			yield return null;
			dateTime = DateTime.Now;
		}
		mesh.SetUVs(1, cache.Uvs2);
		if ((DateTime.Now - dateTime).TotalMilliseconds > (double)maxMsPerFrame)
		{
			yield return null;
			dateTime = DateTime.Now;
		}
		mesh.SetUVs(2, cache.Uvs3);
		if ((DateTime.Now - dateTime).TotalMilliseconds > (double)maxMsPerFrame)
		{
			yield return null;
			dateTime = DateTime.Now;
		}
		mesh.SetUVs(3, cache.Uvs4);
		if ((DateTime.Now - dateTime).TotalMilliseconds > (double)maxMsPerFrame)
		{
			yield return null;
			dateTime = DateTime.Now;
		}
		dateTime = DateTime.Now;
		mesh.SetNormals(cache.Normals);
		int num3 = (int)(DateTime.Now - dateTime).TotalMilliseconds;
		if (num3 > 1)
		{
			Log.Out("recalc terrain time: " + num3.ToString() + "ms");
		}
		if ((DateTime.Now - dateTime).TotalMilliseconds > (double)maxMsPerFrame)
		{
			yield return null;
		}
		mesh.SetColors(cache.Colours);
		mesh.UploadMeshData(DynamicMeshThread.LockMeshesAfterGenerating);
		yield break;
	}

	// Token: 0x06001C13 RID: 7187 RVA: 0x000A6DC0 File Offset: 0x000A4FC0
	[PublicizedFrom(EAccessModifier.Private)]
	public void CreateTerrainGoSync(MeshLists cache, GameObject parent, int maxMsPerFrame)
	{
		if (cache.Vertices.Count == 0)
		{
			return;
		}
		GameObject terrainMeshRendererFromPool = DynamicMeshItem.GetTerrainMeshRendererFromPool();
		terrainMeshRendererFromPool.transform.parent = parent.transform;
		terrainMeshRendererFromPool.transform.localPosition = Vector3.zero;
		terrainMeshRendererFromPool.SetActive(true);
		MeshFilter component = terrainMeshRendererFromPool.GetComponent<MeshFilter>();
		Renderer component2 = terrainMeshRendererFromPool.GetComponent<Renderer>();
		Mesh mesh;
		if (component.mesh == null)
		{
			mesh = new Mesh();
		}
		else
		{
			mesh = component.sharedMesh;
		}
		mesh.indexFormat = ((cache.Vertices.Count < 65535) ? IndexFormat.UInt16 : IndexFormat.UInt32);
		component.sharedMesh = mesh;
		try
		{
			int num = component2.sharedMaterials.Length;
			if (!DynamicMeshFile.TerrainSharedMaterials.ContainsKey(num))
			{
				Material[] array = new Material[num];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = MeshDescription.meshes[5].material;
				}
				DynamicMeshFile.TerrainSharedMaterials.Add(num, array);
			}
			component2.sharedMaterials = DynamicMeshFile.TerrainSharedMaterials[num];
		}
		catch (Exception)
		{
			Log.Warning("Setting shared material failed on terrain material.");
			return;
		}
		mesh.SetVertices(cache.Vertices);
		mesh.subMeshCount = 1;
		for (int j = 0; j < 1; j++)
		{
			mesh.SetTriangles(cache.Triangles, j);
		}
		mesh.SetUVs(0, cache.Uvs);
		mesh.SetUVs(1, cache.Uvs2);
		mesh.SetUVs(2, cache.Uvs3);
		mesh.SetUVs(3, cache.Uvs4);
		mesh.SetNormals(cache.Normals);
		mesh.SetColors(cache.Colours);
		GameUtils.SetMeshVertexAttributes(mesh, true);
		mesh.UploadMeshData(DynamicMeshThread.LockMeshesAfterGenerating);
	}

	// Token: 0x04001247 RID: 4679
	public long Key;

	// Token: 0x04001248 RID: 4680
	public MeshLists OpaqueMesh;

	// Token: 0x04001249 RID: 4681
	public MeshLists TerrainMesh;
}
