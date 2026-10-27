using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

// Token: 0x0200037D RID: 893
public class DynamicMeshItem : DynamicMeshContainer, IEquatable<DynamicMeshItem>
{
	// Token: 0x17000314 RID: 788
	// (get) Token: 0x06001A49 RID: 6729 RVA: 0x0009B48D File Offset: 0x0009968D
	// (set) Token: 0x06001A4A RID: 6730 RVA: 0x0009B495 File Offset: 0x00099695
	public Rect Rect { get; set; }

	// Token: 0x06001A4B RID: 6731 RVA: 0x0009B49E File Offset: 0x0009969E
	public override GameObject GetGameObject()
	{
		return this.ChunkObject;
	}

	// Token: 0x06001A4C RID: 6732 RVA: 0x0009B4A8 File Offset: 0x000996A8
	public DynamicMeshItem(Vector3i pos)
	{
		this.WorldPosition = pos;
		this.Rect = new Rect((float)pos.x, (float)pos.z, 16f, 16f);
		this.Key = WorldChunkCache.MakeChunkKey(World.toChunkXZ(pos.x), World.toChunkXZ(pos.z));
	}

	// Token: 0x06001A4D RID: 6733 RVA: 0x0009B50D File Offset: 0x0009970D
	public static void AddToMeshPool(GameObject go)
	{
		if (go == null)
		{
			return;
		}
		DynamicMeshManager.MeshDestroy(go);
	}

	// Token: 0x06001A4E RID: 6734 RVA: 0x0009B520 File Offset: 0x00099720
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

	// Token: 0x06001A4F RID: 6735 RVA: 0x0009B5C4 File Offset: 0x000997C4
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

	// Token: 0x06001A50 RID: 6736 RVA: 0x0009B624 File Offset: 0x00099824
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

	// Token: 0x06001A51 RID: 6737 RVA: 0x0009B684 File Offset: 0x00099884
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

	// Token: 0x06001A52 RID: 6738 RVA: 0x0009B6E3 File Offset: 0x000998E3
	public void CleanUp()
	{
		if (this.ChunkObject != null)
		{
			DynamicMeshManager.MeshDestroy(this.ChunkObject);
		}
		this.State = DynamicItemState.UpdateRequired;
	}

	// Token: 0x17000315 RID: 789
	// (get) Token: 0x06001A53 RID: 6739 RVA: 0x0009B708 File Offset: 0x00099908
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

	// Token: 0x17000316 RID: 790
	// (get) Token: 0x06001A54 RID: 6740 RVA: 0x0009B7A4 File Offset: 0x000999A4
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

	// Token: 0x06001A55 RID: 6741 RVA: 0x0009B83C File Offset: 0x00099A3C
	public Vector3i GetRegionLocation()
	{
		return DynamicMeshUnity.GetRegionPositionFromWorldPosition(this.WorldPosition);
	}

	// Token: 0x06001A56 RID: 6742 RVA: 0x0009B849 File Offset: 0x00099A49
	public long GetRegionKey()
	{
		return WorldChunkCache.MakeChunkKey(World.toChunkXZ(DynamicMeshUnity.RoundRegion(this.WorldPosition.x)), World.toChunkXZ(DynamicMeshUnity.RoundRegion(this.WorldPosition.z)));
	}

	// Token: 0x06001A57 RID: 6743 RVA: 0x0009B87C File Offset: 0x00099A7C
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

	// Token: 0x06001A58 RID: 6744 RVA: 0x0009B8E4 File Offset: 0x00099AE4
	public DynamicMeshRegion GetRegion()
	{
		DynamicMeshRegion.GetRegionFromWorldPosition(this.WorldPosition);
		DynamicMeshRegion dynamicMeshRegion;
		DynamicMeshRegion.Regions.TryGetValue(this.GetRegionKey(), out dynamicMeshRegion);
		return dynamicMeshRegion;
	}

	// Token: 0x17000317 RID: 791
	// (get) Token: 0x06001A59 RID: 6745 RVA: 0x0009B911 File Offset: 0x00099B11
	public bool IsVisible
	{
		get
		{
			return this.ChunkObject != null && this.ChunkObject.activeSelf;
		}
	}

	// Token: 0x17000318 RID: 792
	// (get) Token: 0x06001A5A RID: 6746 RVA: 0x0009B930 File Offset: 0x00099B30
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

	// Token: 0x17000319 RID: 793
	// (get) Token: 0x06001A5B RID: 6747 RVA: 0x0009B9E2 File Offset: 0x00099BE2
	public bool IsChunkInGame
	{
		get
		{
			return !GameManager.IsDedicatedServer && DynamicMeshManager.ChunkGameObjects.Contains(this.Key);
		}
	}

	// Token: 0x06001A5C RID: 6748 RVA: 0x0009BA00 File Offset: 0x00099C00
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

	// Token: 0x06001A5D RID: 6749 RVA: 0x0009BB14 File Offset: 0x00099D14
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

	// Token: 0x06001A5E RID: 6750 RVA: 0x0009BBC8 File Offset: 0x00099DC8
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

	// Token: 0x06001A5F RID: 6751 RVA: 0x0009BC14 File Offset: 0x00099E14
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

	// Token: 0x06001A60 RID: 6752 RVA: 0x0009BC7A File Offset: 0x00099E7A
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

	// Token: 0x06001A61 RID: 6753 RVA: 0x0009BCA8 File Offset: 0x00099EA8
	public float DistanceToPlayer(Vector3i playerPos)
	{
		return Math.Abs(Mathf.Sqrt(Mathf.Pow((float)(playerPos.x - this.WorldPosition.x), 2f) + Mathf.Pow((float)(playerPos.z - this.WorldPosition.z), 2f)));
	}

	// Token: 0x06001A62 RID: 6754 RVA: 0x0009BCFC File Offset: 0x00099EFC
	public float DistanceToPlayer()
	{
		EntityPlayerLocal player = DynamicMeshItem.player;
		Vector3 vector = ((player == null) ? Vector3.zero : player.position);
		return Math.Abs(Mathf.Sqrt(Mathf.Pow(vector.x - (float)this.WorldPosition.x, 2f) + Mathf.Pow(vector.z - (float)this.WorldPosition.z, 2f)));
	}

	// Token: 0x06001A63 RID: 6755 RVA: 0x0009BD6B File Offset: 0x00099F6B
	public float DistanceToPlayer(float x, float z)
	{
		return Math.Abs(Mathf.Sqrt(Mathf.Pow(x - (float)this.WorldPosition.x, 2f) + Mathf.Pow(z - (float)this.WorldPosition.z, 2f)));
	}

	// Token: 0x06001A64 RID: 6756 RVA: 0x0009BDA8 File Offset: 0x00099FA8
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

	// Token: 0x06001A65 RID: 6757 RVA: 0x0009BDEF File Offset: 0x00099FEF
	public void DestroyMesh()
	{
		DynamicMeshManager.Instance.AddObjectForDestruction(this.ChunkObject);
		this.ChunkObject = null;
	}

	// Token: 0x06001A66 RID: 6758 RVA: 0x0009BE08 File Offset: 0x0009A008
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

	// Token: 0x06001A67 RID: 6759 RVA: 0x0009BF20 File Offset: 0x0009A120
	public void SetPosition()
	{
		if (this.ChunkObject == null)
		{
			return;
		}
		this.ChunkObject.transform.position = this.WorldPosition.ToVector3() - Origin.position;
	}

	// Token: 0x1700031A RID: 794
	// (get) Token: 0x06001A68 RID: 6760 RVA: 0x0009BF56 File Offset: 0x0009A156
	public string Path
	{
		get
		{
			return DynamicMeshUnity.GetItemPath(this.Key);
		}
	}

	// Token: 0x06001A69 RID: 6761 RVA: 0x0009BF63 File Offset: 0x0009A163
	public bool FileExists()
	{
		return SdFile.Exists(this.Path);
	}

	// Token: 0x06001A6A RID: 6762 RVA: 0x0009BF70 File Offset: 0x0009A170
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

	// Token: 0x06001A6B RID: 6763 RVA: 0x0009BF8D File Offset: 0x0009A18D
	public override int GetHashCode()
	{
		return this.Key.GetHashCode();
	}

	// Token: 0x06001A6C RID: 6764 RVA: 0x0009BF9C File Offset: 0x0009A19C
	public override bool Equals(object obj)
	{
		DynamicMeshItem dynamicMeshItem = obj as DynamicMeshItem;
		return dynamicMeshItem != null && dynamicMeshItem.Key == this.Key;
	}

	// Token: 0x06001A6D RID: 6765 RVA: 0x0009BFC4 File Offset: 0x0009A1C4
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

	// Token: 0x06001A6E RID: 6766 RVA: 0x0009C0A8 File Offset: 0x0009A2A8
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

	// Token: 0x06001A6F RID: 6767 RVA: 0x0009C140 File Offset: 0x0009A340
	public bool Equals(DynamicMeshItem other)
	{
		return other.Key == this.Key;
	}

	// Token: 0x1700031B RID: 795
	// (get) Token: 0x06001A70 RID: 6768 RVA: 0x0009C150 File Offset: 0x0009A350
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

	// Token: 0x040010CE RID: 4302
	public static HashSet<GameObject> MeshPool = new HashSet<GameObject>();

	// Token: 0x040010CF RID: 4303
	public static HashSet<GameObject> TerrainMeshPool = new HashSet<GameObject>();

	// Token: 0x040010D0 RID: 4304
	[PublicizedFrom(EAccessModifier.Private)]
	public static int cacheId = 0;

	// Token: 0x040010D2 RID: 4306
	public GameObject ChunkObject;

	// Token: 0x040010D3 RID: 4307
	public int UpdateTime;

	// Token: 0x040010D4 RID: 4308
	public int PackageLength;

	// Token: 0x040010D5 RID: 4309
	public DynamicItemState State = DynamicItemState.UpdateRequired;
}
