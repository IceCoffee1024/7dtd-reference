using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000356 RID: 854
public class ChunkPreviewManager
{
	// Token: 0x170002EE RID: 750
	// (get) Token: 0x060018B4 RID: 6324 RVA: 0x0008B8B0 File Offset: 0x00089AB0
	public Prefab Prefab
	{
		get
		{
			return this.PreviewData.PrefabData;
		}
	}

	// Token: 0x170002EF RID: 751
	// (get) Token: 0x060018B5 RID: 6325 RVA: 0x0008B8BD File Offset: 0x00089ABD
	public Vector3i WorldPosition
	{
		get
		{
			return this.PreviewData.WorldPosition;
		}
	}

	// Token: 0x060018B6 RID: 6326 RVA: 0x0008B8CC File Offset: 0x00089ACC
	public ChunkPreviewManager()
	{
		ChunkPreviewManager.Instance = this;
		this.PreviewData = new ChunkPreviewData();
		this.PreviewChunksContainer = GameObject.Find("PreviewChunks");
		if (this.PreviewChunksContainer == null)
		{
			this.PreviewChunksContainer = new GameObject("PreviewChunks");
		}
		DynamicMeshThread.SetDefaultThreads();
		this.ThreadData = new DynamicMeshPrefabPreviewThread();
		this.ThreadData.PreviewData = this.PreviewData;
		this.ThreadData.StartThread();
		GameManager.Instance.StartCoroutine(this.LoadPreviewMesh());
	}

	// Token: 0x060018B7 RID: 6327 RVA: 0x0008B97C File Offset: 0x00089B7C
	public void AddChunkPreviewLoadData(DynamicMeshVoxelLoad loadData)
	{
		this.ChunkPreviewMeshData.Enqueue(loadData);
	}

	// Token: 0x060018B8 RID: 6328 RVA: 0x0008B98A File Offset: 0x00089B8A
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator LoadPreviewMesh()
	{
		while (!this.StopRequested)
		{
			DynamicMeshVoxelLoad voxelData;
			if (!this.ChunkPreviewMeshData.TryDequeue(out voxelData))
			{
				yield return null;
			}
			else
			{
				while (voxelData.Item.State == DynamicItemState.Loading)
				{
					Log.Out("delaying load");
					yield return null;
				}
				voxelData.Item.DestroyChunk();
				yield return GameManager.Instance.StartCoroutine(voxelData.Item.CreateMeshFromVoxelCoroutine(true, null, voxelData));
				this.AddPreviewChunk(voxelData);
				if (voxelData.Item.ChunkObject != null)
				{
					voxelData.Item.ChunkObject.transform.parent = this.PreviewChunksContainer.transform;
				}
				voxelData.DisposeMeshes();
				voxelData = null;
			}
		}
		yield break;
	}

	// Token: 0x060018B9 RID: 6329 RVA: 0x0008B99C File Offset: 0x00089B9C
	public void ClearAll()
	{
		foreach (DynamicMeshItem dynamicMeshItem in this.Items)
		{
			dynamicMeshItem.DestroyChunk();
		}
		this.Items.Clear();
	}

	// Token: 0x060018BA RID: 6330 RVA: 0x0008B9F8 File Offset: 0x00089BF8
	public void AddPreviewChunk(DynamicMeshVoxelLoad loadData)
	{
		DynamicMeshItem item = loadData.Item;
		if (this.IsPositionInArea(loadData.Item.WorldPosition))
		{
			this.SetChunkGoVisiblity(item.Key, false);
		}
		else
		{
			this.SetChunkGoVisiblity(item.Key, true);
			loadData.Item.DestroyChunk();
		}
		this.CheckItems();
	}

	// Token: 0x060018BB RID: 6331 RVA: 0x0008BA4D File Offset: 0x00089C4D
	public bool IsPositionInArea(Vector3 pos)
	{
		return this.IsPositionInArea(new Vector3i(pos));
	}

	// Token: 0x060018BC RID: 6332 RVA: 0x0008BA5C File Offset: 0x00089C5C
	public void ActivationChanged(PrefabInstance pi)
	{
		if (pi == null)
		{
			this.SetWorldPosition(new Vector3i(this.WorldPosition.x, -512, this.WorldPosition.z));
			this.CheckItems();
			return;
		}
		this.SetPrefab(pi.prefab, pi, pi.boundingBoxPosition);
	}

	// Token: 0x060018BD RID: 6333 RVA: 0x0008BAAC File Offset: 0x00089CAC
	public bool IsActivePrefab(PrefabInstance pi)
	{
		return pi.prefab == this.PreviewData.PrefabData && this.IsPositionInArea(pi.boundingBoxPosition);
	}

	// Token: 0x060018BE RID: 6334 RVA: 0x0008BAD4 File Offset: 0x00089CD4
	public bool IsPositionInArea(Vector3i fullposition)
	{
		if (this.WorldPosition.y < -256)
		{
			return false;
		}
		Vector3i chunkPositionFromWorldPosition = DynamicMeshUnity.GetChunkPositionFromWorldPosition(fullposition);
		Vector3i chunkPositionFromWorldPosition2 = DynamicMeshUnity.GetChunkPositionFromWorldPosition(this.PreviewData.WorldPosition);
		Vector3i chunkPositionFromWorldPosition3 = DynamicMeshUnity.GetChunkPositionFromWorldPosition(this.PreviewData.WorldPosition + this.PreviewData.PrefabData.size);
		int x = chunkPositionFromWorldPosition2.x;
		int x2 = chunkPositionFromWorldPosition3.x;
		int z = chunkPositionFromWorldPosition2.z;
		int z2 = chunkPositionFromWorldPosition3.z;
		return chunkPositionFromWorldPosition.x >= x && chunkPositionFromWorldPosition.x <= x2 && chunkPositionFromWorldPosition.z >= z && chunkPositionFromWorldPosition.z <= z2;
	}

	// Token: 0x060018BF RID: 6335 RVA: 0x0008BB78 File Offset: 0x00089D78
	[PublicizedFrom(EAccessModifier.Private)]
	public void CheckItems()
	{
		for (int i = this.Items.Count - 1; i >= 0; i--)
		{
			DynamicMeshItem dynamicMeshItem = this.Items[i];
			if (!this.IsPositionInArea(dynamicMeshItem.WorldPosition))
			{
				this.SetChunkGoVisiblity(dynamicMeshItem.Key, true);
				dynamicMeshItem.DestroyChunk();
			}
		}
	}

	// Token: 0x060018C0 RID: 6336 RVA: 0x0008BBCC File Offset: 0x00089DCC
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetChunkGoVisiblity(long chunkKey, bool isVisible)
	{
		string text = string.Format("Chunk_{0},{1}", DynamicMeshUnity.GetChunkSectionX(chunkKey), DynamicMeshUnity.GetChunkSectionZ(chunkKey));
		foreach (ChunkGameObject chunkGameObject in GameManager.Instance.World.m_ChunkManager.GetUsedChunkGameObjects())
		{
			Transform transform = chunkGameObject.transform;
			if (transform.name == text)
			{
				if (isVisible)
				{
					transform.gameObject.transform.localScale = Vector3.one;
				}
				else
				{
					transform.gameObject.transform.localScale = Vector3.zero;
				}
			}
		}
	}

	// Token: 0x060018C1 RID: 6337 RVA: 0x0008BC8C File Offset: 0x00089E8C
	public void CleanUp()
	{
		DynamicMeshPrefabPreviewThread threadData = this.ThreadData;
		if (threadData == null)
		{
			return;
		}
		threadData.StopThread();
	}

	// Token: 0x060018C2 RID: 6338 RVA: 0x0008BCA0 File Offset: 0x00089EA0
	public void SetWorldPosition(Vector3i worldPosition)
	{
		this.PreviewData.WorldPosition = worldPosition;
		if (this.Prefab != null)
		{
			int num = DynamicMeshUnity.RoundChunk(worldPosition.x + this.Prefab.size.x) + 16;
			int num2 = DynamicMeshUnity.RoundChunk(worldPosition.z + this.Prefab.size.z) + 16;
			for (int i = worldPosition.x; i <= num; i += 16)
			{
				for (int j = worldPosition.z; j <= num2; j += 16)
				{
					this.StartChunkPreview(new Vector3i(i, 0, j));
				}
			}
		}
		this.CheckItems();
	}

	// Token: 0x060018C3 RID: 6339 RVA: 0x0008BD3C File Offset: 0x00089F3C
	public void StartChunkPreview(Vector3i chunkPos)
	{
		DynamicMeshItem dynamicMeshItem = this.Get(chunkPos);
		this.ThreadData.AddChunk(dynamicMeshItem);
	}

	// Token: 0x060018C4 RID: 6340 RVA: 0x0008BD5D File Offset: 0x00089F5D
	public void SetPrefab(PrefabInstance prefab)
	{
		this.SetPrefab(prefab.prefab, prefab, this.WorldPosition);
	}

	// Token: 0x060018C5 RID: 6341 RVA: 0x0008BD72 File Offset: 0x00089F72
	public void SetPrefab(Prefab prefab)
	{
		this.SetPrefab(prefab, null, this.WorldPosition);
	}

	// Token: 0x060018C6 RID: 6342 RVA: 0x0008BD84 File Offset: 0x00089F84
	public void SetPrefab(Prefab prefab, PrefabInstance instance, Vector3i worldPosition)
	{
		PrefabInstance prefabInstance = this.PreviewData.PrefabInstance;
		if (prefabInstance != null)
		{
			PrefabLODManager.PrefabGameObject instance2 = GameManager.Instance.prefabLODManager.GetInstance(prefabInstance.id);
			if (instance2 != null)
			{
				instance2.go.SetActive(true);
			}
		}
		this.PreviewData.PrefabData = prefab;
		this.PreviewData.PrefabInstance = instance;
		if (instance != null)
		{
			PrefabLODManager.PrefabGameObject instance3 = GameManager.Instance.prefabLODManager.GetInstance(instance.id);
			if (instance3 != null)
			{
				instance3.go.SetActive(false);
			}
		}
		this.SetWorldPosition(worldPosition);
	}

	// Token: 0x060018C7 RID: 6343 RVA: 0x0008BE0C File Offset: 0x0008A00C
	public DynamicMeshItem Get(Vector3i worldPos)
	{
		worldPos = DynamicMeshUnity.GetChunkPositionFromWorldPosition(worldPos);
		foreach (DynamicMeshItem dynamicMeshItem in this.Items)
		{
			if (dynamicMeshItem.WorldPosition.x == worldPos.x && dynamicMeshItem.WorldPosition.z == worldPos.z)
			{
				return dynamicMeshItem;
			}
		}
		DynamicMeshItem dynamicMeshItem2 = new DynamicMeshItem(worldPos);
		this.Items.Add(dynamicMeshItem2);
		return dynamicMeshItem2;
	}

	// Token: 0x060018C8 RID: 6344 RVA: 0x0008BEA0 File Offset: 0x0008A0A0
	public void Update()
	{
		if (this.NextUpdate > DateTime.Now)
		{
			return;
		}
		this.NextUpdate = DateTime.Now.AddSeconds(1.0);
		foreach (DynamicMeshItem dynamicMeshItem in this.Items)
		{
			this.SetChunkGoVisiblity(dynamicMeshItem.Key, dynamicMeshItem.ChunkObject == null || !this.IsPositionInArea(dynamicMeshItem.WorldPosition));
		}
	}

	// Token: 0x04000FBC RID: 4028
	public static ChunkPreviewManager Instance;

	// Token: 0x04000FBD RID: 4029
	public List<DynamicMeshItem> Items = new List<DynamicMeshItem>();

	// Token: 0x04000FBE RID: 4030
	public ChunkPreviewData PreviewData;

	// Token: 0x04000FBF RID: 4031
	[PublicizedFrom(EAccessModifier.Private)]
	public DynamicMeshPrefabPreviewThread ThreadData;

	// Token: 0x04000FC0 RID: 4032
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject PreviewChunksContainer;

	// Token: 0x04000FC1 RID: 4033
	public ConcurrentQueue<DynamicMeshVoxelLoad> ChunkPreviewMeshData = new ConcurrentQueue<DynamicMeshVoxelLoad>();

	// Token: 0x04000FC2 RID: 4034
	[PublicizedFrom(EAccessModifier.Private)]
	public bool StopRequested;

	// Token: 0x04000FC3 RID: 4035
	[PublicizedFrom(EAccessModifier.Private)]
	public DateTime NextUpdate = DateTime.Now;
}
