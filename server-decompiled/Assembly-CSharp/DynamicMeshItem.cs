using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

// Token: 0x02000374 RID: 884
public class DynamicMeshItem : DynamicMeshContainer, IEquatable<DynamicMeshItem>
{
	// Token: 0x17000318 RID: 792
	// (get) Token: 0x06001A2D RID: 6701 RVA: 0x0009A899 File Offset: 0x00098A99
	// (set) Token: 0x06001A2E RID: 6702 RVA: 0x0009A8A1 File Offset: 0x00098AA1
	public Rect Rect { get; set; }

	// Token: 0x06001A2F RID: 6703 RVA: 0x0009A8AA File Offset: 0x00098AAA
	public override GameObject GetGameObject()
	{
		return this.ChunkObject;
	}

	// Token: 0x06001A30 RID: 6704 RVA: 0x0009A8B4 File Offset: 0x00098AB4
	public DynamicMeshItem(Vector3i pos)
	{
		this.WorldPosition = pos;
		this.Rect = new Rect((float)pos.x, (float)pos.z, 16f, 16f);
		this.Key = WorldChunkCache.MakeChunkKey(World.toChunkXZ(pos.x), World.toChunkXZ(pos.z));
	}

	// Token: 0x06001A31 RID: 6705 RVA: 0x0009A919 File Offset: 0x00098B19
	public static void AddToMeshPool(GameObject go)
	{
		if (go == null)
		{
			return;
		}
		DynamicMeshManager.MeshDestroy(go);
	}

	// Token: 0x06001A32 RID: 6706 RVA: 0x0009A92C File Offset: 0x00098B2C
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool AddToPoolInternal(GameObject go)
	{
		using (HashSet<GameObject>.Enumerator enumerator = DynamicMeshItem.MeshPool.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.GetInstanceID() == go.GetInstanceID())
				{
					Log.Warning("Duplicate pool add. Name: " + go.name);
					return false;
				}
			}
		}
		DynamicMeshItem.MeshPool.Add(go);
		go.transform.parent = null;
		go.SetActive(false);
		go.GetComponent<MeshFilter>().mesh.Clear(false);
		return true;
	}

	// Token: 0x06001A33 RID: 6707 RVA: 0x0009A9D0 File Offset: 0x00098BD0
	public static GameObject GetItemMeshRendererFromPool()
	{
		GameObject gameObject;
		if (DynamicMeshItem.MeshPool.Count > 0)
		{
			gameObject = DynamicMeshItem.MeshPool.Last<GameObject>();
			DynamicMeshItem.MeshPool.Remove(gameObject);
		}
		else
		{
			gameObject = DynamicMeshFile.CreateMeshObject(string.Empty, false);
		}
		gameObject.transform.position = Vector3.zero;
		gameObject.transform.parent = DynamicMeshManager.ParentTransform;
		return gameObject;
	}

	// Token: 0x06001A34 RID: 6708 RVA: 0x0009AA30 File Offset: 0x00098C30
	public static GameObject GetRegionMeshRendererFromPool()
	{
		GameObject gameObject;
		if (DynamicMeshItem.MeshPool.Count > 0)
		{
			gameObject = DynamicMeshItem.MeshPool.Last<GameObject>();
			DynamicMeshItem.MeshPool.Remove(gameObject);
		}
		else
		{
			gameObject = DynamicMeshFile.CreateMeshObject(string.Empty, true);
		}
		gameObject.transform.position = Vector3.zero;
		gameObject.transform.parent = DynamicMeshManager.ParentTransform;
		return gameObject;
	}

	// Token: 0x06001A35 RID: 6709 RVA: 0x0009AA90 File Offset: 0x00098C90
	public static GameObject GetTerrainMeshRendererFromPool()
	{
		GameObject gameObject;
		if (DynamicMeshItem.TerrainMeshPool.Count > 0)
		{
			gameObject = DynamicMeshItem.TerrainMeshPool.Last<GameObject>();
			DynamicMeshItem.TerrainMeshPool.Remove(gameObject);
		}
		else
		{
			gameObject = DynamicMeshFile.CreateTerrainMeshObject(string.Empty);
		}
		gameObject.transform.position = Vector3.zero;
		gameObject.transform.parent = DynamicMeshManager.ParentTransform;
		return gameObject;
	}

	// Token: 0x06001A36 RID: 6710 RVA: 0x0009AAEF File Offset: 0x00098CEF
	public void CleanUp()
	{
		if (this.ChunkObject != null)
		{
			DynamicMeshManager.MeshDestroy(this.ChunkObject);
		}
		this.State = DynamicItemState.UpdateRequired;
	}

	// Token: 0x17000319 RID: 793
	// (get) Token: 0x06001A37 RID: 6711 RVA: 0x0009AB14 File Offset: 0x00098D14
	public int Triangles
	{
		get
		{
			int num = 0;
			if (this.ChunkObject != null)
			{
				num += this.ChunkObject.GetComponent<MeshFilter>().mesh.triangles.Length;
				foreach (object obj in this.ChunkObject.transform)
				{
					Transform transform = (Transform)obj;
					num += transform.gameObject.GetComponent<MeshFilter>().mesh.triangles.Length;
				}
			}
			return num;
		}
	}

	// Token: 0x1700031A RID: 794
	// (get) Token: 0x06001A38 RID: 6712 RVA: 0x0009ABB0 File Offset: 0x00098DB0
	public int Vertices
	{
		get
		{
			int num = 0;
			if (this.ChunkObject != null)
			{
				num += this.ChunkObject.GetComponent<MeshFilter>().mesh.vertexCount;
				foreach (object obj in this.ChunkObject.transform)
				{
					Transform transform = (Transform)obj;
					num += transform.gameObject.GetComponent<MeshFilter>().mesh.vertexCount;
				}
			}
			return num;
		}
	}

	// Token: 0x06001A39 RID: 6713 RVA: 0x0009AC48 File Offset: 0x00098E48
	public Vector3i GetRegionLocation()
	{
		return DynamicMeshUnity.GetRegionPositionFromWorldPosition(this.WorldPosition);
	}

	// Token: 0x06001A3A RID: 6714 RVA: 0x0009AC55 File Offset: 0x00098E55
	public long GetRegionKey()
	{
		return WorldChunkCache.MakeChunkKey(World.toChunkXZ(DynamicMeshUnity.RoundRegion(this.WorldPosition.x)), World.toChunkXZ(DynamicMeshUnity.RoundRegion(this.WorldPosition.z)));
	}

	// Token: 0x06001A3B RID: 6715 RVA: 0x0009AC88 File Offset: 0x00098E88
	public int ReadUpdateTimeFromFile()
	{
		int num;
		using (Stream stream = SdFile.OpenRead(this.Path))
		{
			using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
			{
				pooledBinaryReader.SetBaseStream(stream);
				num = pooledBinaryReader.ReadInt32();
			}
		}
		return num;
	}

	// Token: 0x06001A3C RID: 6716 RVA: 0x0009ACF0 File Offset: 0x00098EF0
	public DynamicMeshRegion GetRegion()
	{
		DynamicMeshRegion.GetRegionFromWorldPosition(this.WorldPosition);
		DynamicMeshRegion dynamicMeshRegion;
		DynamicMeshRegion.Regions.TryGetValue(this.GetRegionKey(), out dynamicMeshRegion);
		return dynamicMeshRegion;
	}

	// Token: 0x1700031B RID: 795
	// (get) Token: 0x06001A3D RID: 6717 RVA: 0x0009AD1D File Offset: 0x00098F1D
	public bool IsVisible
	{
		get
		{
			return this.ChunkObject != null && this.ChunkObject.activeSelf;
		}
	}

	// Token: 0x1700031C RID: 796
	// (get) Token: 0x06001A3E RID: 6718 RVA: 0x0009AD3C File Offset: 0x00098F3C
	public bool IsChunkInView
	{
		get
		{
			if (GameManager.IsDedicatedServer)
			{
				return false;
			}
			if (DynamicMeshManager.Instance == null)
			{
				return false;
			}
			Vector3 position = DynamicMeshItem.player.position;
			int viewSize = DynamicMeshManager.GetViewSize(DynamicMeshItem.player);
			int num = World.toChunkXZ(Utils.Fastfloor(position.x)) * 16;
			int num2 = World.toChunkXZ(Utils.Fastfloor(position.z)) * 16;
			int num3 = num - viewSize;
			int num4 = num + viewSize;
			int num5 = num2 - viewSize;
			int num6 = num2 + viewSize;
			return this.WorldPosition.x > num3 && this.WorldPosition.x <= num4 && this.WorldPosition.z > num5 && this.WorldPosition.z <= num6;
		}
	}

	// Token: 0x1700031D RID: 797
	// (get) Token: 0x06001A3F RID: 6719 RVA: 0x0009ADEE File Offset: 0x00098FEE
	public bool IsChunkInGame
	{
		get
		{
			return !GameManager.IsDedicatedServer && DynamicMeshManager.ChunkGameObjects.Contains(this.Key);
		}
	}

	// Token: 0x06001A40 RID: 6720 RVA: 0x0009AE0C File Offset: 0x0009900C
	public void SetVisible(bool active, string reason)
	{
		if (this.ChunkObject == null)
		{
			return;
		}
		if (active != this.ChunkObject.activeSelf)
		{
			if (DynamicMeshManager.DebugItemPositions)
			{
				this.ChunkObject.name = string.Concat(new string[]
				{
					"C ",
					base.ToDebugLocation(),
					": ",
					reason,
					" (",
					active.ToString(),
					")"
				});
			}
			this.ChunkObject.SetActive(active);
			if (DynamicMeshManager.DoLog)
			{
				Log.Out(string.Concat(new string[]
				{
					"Chunk ",
					this.WorldPosition.x.ToString(),
					",",
					this.WorldPosition.z.ToString(),
					" Visible: ",
					active.ToString(),
					" reason: ",
					reason,
					" inview: ",
					this.IsChunkInView.ToString()
				}));
			}
		}
	}

	// Token: 0x06001A41 RID: 6721 RVA: 0x0009AF20 File Offset: 0x00099120
	public void ForceHide()
	{
		if (this.ChunkObject == null || !this.ChunkObject.activeSelf)
		{
			return;
		}
		if (DynamicMeshManager.DebugItemPositions)
		{
			this.ChunkObject.name = "C " + base.ToDebugLocation() + ": forceHide";
		}
		this.ChunkObject.SetActive(false);
		if (DynamicMeshManager.DoLog)
		{
			Log.Out(string.Concat(new string[]
			{
				"Chunk ",
				this.WorldPosition.x.ToString(),
				",",
				this.WorldPosition.z.ToString(),
				" ForceHide"
			}));
		}
	}

	// Token: 0x06001A42 RID: 6722 RVA: 0x0009AFD4 File Offset: 0x000991D4
	public void OnCorrupted()
	{
		if (DynamicMeshManager.DoLog)
		{
			string text = "Corrupted item. Adding for regen ";
			Vector3i worldPosition = this.WorldPosition;
			DynamicMeshManager.LogMsg(text + worldPosition.ToString());
		}
		DynamicMeshManager.Instance.AddChunk(this.WorldPosition, true);
	}

	// Token: 0x06001A43 RID: 6723 RVA: 0x0009B020 File Offset: 0x00099220
	public bool LoadIfEmpty(string caller, bool urgentLoad, bool regionInBuffer)
	{
		if (this.ChunkObject != null)
		{
			return false;
		}
		if (this.State == DynamicItemState.ReadyToDelete)
		{
			return false;
		}
		if (this.State == DynamicItemState.Empty)
		{
			return false;
		}
		if (this.State == DynamicItemState.LoadRequested)
		{
			return false;
		}
		if (!DynamicMeshManager.Instance.IsInLoadableArea(this.Key))
		{
			return false;
		}
		this.State = DynamicItemState.LoadRequested;
		DynamicMeshManager.Instance.AddItemLoadRequest(this, urgentLoad);
		return true;
	}

	// Token: 0x06001A44 RID: 6724 RVA: 0x0009B086 File Offset: 0x00099286
	public bool Load(string caller, bool urgentLoad, bool regionInBuffer)
	{
		if (this.State == DynamicItemState.ReadyToDelete)
		{
			return false;
		}
		if (this.State == DynamicItemState.LoadRequested)
		{
			return false;
		}
		this.State = DynamicItemState.LoadRequested;
		DynamicMeshManager.Instance.AddItemLoadRequest(this, urgentLoad);
		return true;
	}

	// Token: 0x06001A45 RID: 6725 RVA: 0x0009B0B4 File Offset: 0x000992B4
	public float DistanceToPlayer(Vector3i playerPos)
	{
		return Math.Abs(Mathf.Sqrt(Mathf.Pow((float)(playerPos.x - this.WorldPosition.x), 2f) + Mathf.Pow((float)(playerPos.z - this.WorldPosition.z), 2f)));
	}

	// Token: 0x06001A46 RID: 6726 RVA: 0x0009B108 File Offset: 0x00099308
	public float DistanceToPlayer()
	{
		EntityPlayerLocal player = DynamicMeshItem.player;
		Vector3 vector = ((player == null) ? Vector3.zero : player.position);
		return Math.Abs(Mathf.Sqrt(Mathf.Pow(vector.x - (float)this.WorldPosition.x, 2f) + Mathf.Pow(vector.z - (float)this.WorldPosition.z, 2f)));
	}

	// Token: 0x06001A47 RID: 6727 RVA: 0x0009B177 File Offset: 0x00099377
	public float DistanceToPlayer(float x, float z)
	{
		return Math.Abs(Mathf.Sqrt(Mathf.Pow(x - (float)this.WorldPosition.x, 2f) + Mathf.Pow(z - (float)this.WorldPosition.z, 2f)));
	}

	// Token: 0x06001A48 RID: 6728 RVA: 0x0009B1B4 File Offset: 0x000993B4
	public bool DestroyChunk()
	{
		bool flag = false;
		if (this.ChunkObject != null)
		{
			flag = true;
			if (DynamicMeshManager.DoLog)
			{
				DynamicMeshManager.LogMsg("Destroying chunk " + base.ToDebugLocation());
			}
			DynamicMeshManager.MeshDestroy(this.ChunkObject);
		}
		return flag;
	}

	// Token: 0x06001A49 RID: 6729 RVA: 0x0009B1FB File Offset: 0x000993FB
	public void DestroyMesh()
	{
		DynamicMeshManager.Instance.AddObjectForDestruction(this.ChunkObject);
		this.ChunkObject = null;
	}

	// Token: 0x06001A4A RID: 6730 RVA: 0x0009B214 File Offset: 0x00099414
	public bool CreateMeshSync(bool isVisible)
	{
		if (this.ChunkObject != null)
		{
			this.SetVisible(isVisible, "Create mesh exists");
			return false;
		}
		string path = this.Path;
		if (!SdFile.Exists(path))
		{
			this.State = DynamicItemState.Empty;
			return false;
		}
		using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
		{
			using (Stream readStream = DynamicMeshFile.GetReadStream(path))
			{
				pooledBinaryReader.SetBaseStream(readStream);
				if (pooledBinaryReader.BaseStream.Position == pooledBinaryReader.BaseStream.Length)
				{
					this.State = DynamicItemState.Empty;
				}
				else
				{
					DynamicMeshFile.ReadItemMesh(pooledBinaryReader, this, isVisible);
				}
			}
		}
		if (this.ChunkObject != null)
		{
			this.SetVisible(isVisible, "create mesh complete");
			this.SetPosition();
			Quaternion identity = Quaternion.identity;
			this.ChunkObject.transform.parent = DynamicMeshManager.ParentTransform;
			this.ChunkObject.transform.rotation = identity;
		}
		this.PackageLength = this.GetStreamLength();
		this.State = DynamicItemState.Loaded;
		return true;
	}

	// Token: 0x06001A4B RID: 6731 RVA: 0x0009B32C File Offset: 0x0009952C
	public void SetPosition()
	{
		if (this.ChunkObject == null)
		{
			return;
		}
		this.ChunkObject.transform.position = this.WorldPosition.ToVector3() - Origin.position;
	}

	// Token: 0x1700031E RID: 798
	// (get) Token: 0x06001A4C RID: 6732 RVA: 0x0009B362 File Offset: 0x00099562
	public string Path
	{
		get
		{
			return DynamicMeshUnity.GetItemPath(this.Key);
		}
	}

	// Token: 0x06001A4D RID: 6733 RVA: 0x0009B36F File Offset: 0x0009956F
	public bool FileExists()
	{
		return SdFile.Exists(this.Path);
	}

	// Token: 0x06001A4E RID: 6734 RVA: 0x0009B37C File Offset: 0x0009957C
	public IEnumerator CreateMeshFromVoxelCoroutine(bool isVisible, MicroStopwatch stop, DynamicMeshVoxelLoad data)
	{
		GameObject oldMesh = this.ChunkObject;
		this.ChunkObject = null;
		this.State = DynamicItemState.Loading;
		yield return GameManager.Instance.StartCoroutine(data.CreateMeshCoroutine(this));
		if (this.ChunkObject != null)
		{
			Quaternion identity = Quaternion.identity;
			this.ChunkObject.transform.parent = DynamicMeshManager.ParentTransform;
			this.ChunkObject.transform.rotation = identity;
			this.SetVisible(isVisible, "create mesh complete coroutine");
			this.SetPosition();
			this.State = DynamicItemState.Loaded;
		}
		else
		{
			this.State = DynamicItemState.Empty;
		}
		if (oldMesh != null)
		{
			DynamicMeshItem.AddToMeshPool(oldMesh);
		}
		yield break;
	}

	// Token: 0x06001A4F RID: 6735 RVA: 0x0009B399 File Offset: 0x00099599
	public override int GetHashCode()
	{
		return this.Key.GetHashCode();
	}

	// Token: 0x06001A50 RID: 6736 RVA: 0x0009B3A8 File Offset: 0x000995A8
	public override bool Equals(object obj)
	{
		DynamicMeshItem dynamicMeshItem = obj as DynamicMeshItem;
		return dynamicMeshItem != null && dynamicMeshItem.Key == this.Key;
	}

	// Token: 0x06001A51 RID: 6737 RVA: 0x0009B3D0 File Offset: 0x000995D0
	public int GetStreamLength()
	{
		int num = 20;
		if (this.ChunkObject == null)
		{
			return num;
		}
		MeshFilter meshFilter = this.ChunkObject.GetComponent<MeshFilter>();
		num += 12 + meshFilter.mesh.vertexCount * 6 + meshFilter.mesh.vertexCount * 8 + meshFilter.mesh.triangles.Length * 2;
		foreach (object obj in this.ChunkObject.transform)
		{
			meshFilter = ((Transform)obj).gameObject.GetComponent<MeshFilter>();
			num += 12 + meshFilter.mesh.vertexCount * 6 + meshFilter.mesh.vertexCount * 8 + meshFilter.mesh.triangles.Length * 2;
		}
		return num;
	}

	// Token: 0x06001A52 RID: 6738 RVA: 0x0009B4B4 File Offset: 0x000996B4
	public int GetStreamLength(List<VoxelMesh> meshes, List<VoxelMeshTerrain> terrainMeshes)
	{
		int num = 20;
		foreach (VoxelMesh voxelMesh in meshes)
		{
			num += voxelMesh.GetByteLength();
		}
		foreach (VoxelMeshTerrain voxelMeshTerrain in terrainMeshes)
		{
			num += voxelMeshTerrain.GetByteLength();
		}
		return num;
	}

	// Token: 0x06001A53 RID: 6739 RVA: 0x0009B54C File Offset: 0x0009974C
	public bool Equals(DynamicMeshItem other)
	{
		return other.Key == this.Key;
	}

	// Token: 0x1700031F RID: 799
	// (get) Token: 0x06001A54 RID: 6740 RVA: 0x0009B55C File Offset: 0x0009975C
	public static EntityPlayerLocal player
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (GameManager.Instance.World != null)
			{
				return GameManager.Instance.World.GetPrimaryPlayer();
			}
			return null;
		}
	}

	// Token: 0x040010B0 RID: 4272
	public static HashSet<GameObject> MeshPool = new HashSet<GameObject>();

	// Token: 0x040010B1 RID: 4273
	public static HashSet<GameObject> TerrainMeshPool = new HashSet<GameObject>();

	// Token: 0x040010B2 RID: 4274
	[PublicizedFrom(EAccessModifier.Private)]
	public static int cacheId = 0;

	// Token: 0x040010B4 RID: 4276
	public GameObject ChunkObject;

	// Token: 0x040010B5 RID: 4277
	public int UpdateTime;

	// Token: 0x040010B6 RID: 4278
	public int PackageLength;

	// Token: 0x040010B7 RID: 4279
	public DynamicItemState State = DynamicItemState.UpdateRequired;
}
