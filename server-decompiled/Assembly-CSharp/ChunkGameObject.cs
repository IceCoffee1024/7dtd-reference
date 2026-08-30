using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000B21 RID: 2849
public class ChunkGameObject : MonoBehaviour
{
	// Token: 0x06005619 RID: 22041 RVA: 0x0020E69F File Offset: 0x0020C89F
	[PublicizedFrom(EAccessModifier.Private)]
	public ChunkGameObject()
	{
	}

	// Token: 0x0600561A RID: 22042 RVA: 0x0020E6C0 File Offset: 0x0020C8C0
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Awake()
	{
		this.blockEntitiesParentT = new GameObject("_BlockEntities").transform;
		this.blockEntitiesParentT.SetParent(base.transform, false);
	}

	// Token: 0x0600561B RID: 22043 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnEnable()
	{
	}

	// Token: 0x0600561C RID: 22044 RVA: 0x0020E6E9 File Offset: 0x0020C8E9
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnDisable()
	{
		if (this.chunk != null)
		{
			this.chunk.OnHide();
		}
		if (this.vml != null)
		{
			this.vml.TryFree();
			this.vml = null;
		}
	}

	// Token: 0x0600561D RID: 22045 RVA: 0x0020E71C File Offset: 0x0020C91C
	public void SetStatic(bool _bStatic)
	{
		for (int i = 0; i < this.layers.Length; i++)
		{
			if (this.layers[i] != null)
			{
				for (int j = 0; j < MeshDescription.meshes.Length; j++)
				{
					this.layers[i].m_MeshesGO[j].isStatic = _bStatic;
				}
				for (int k = 0; k < this.layers[i].m_MeshCollider.Length; k++)
				{
					if (this.layers[i].m_MeshCollider[k] != null)
					{
						this.layers[i].m_MeshCollider[k].gameObject.isStatic = _bStatic;
					}
				}
			}
		}
		base.transform.gameObject.isStatic = _bStatic;
	}

	// Token: 0x0600561E RID: 22046 RVA: 0x0020E7CF File Offset: 0x0020C9CF
	public Chunk GetChunk()
	{
		return this.chunk;
	}

	// Token: 0x0600561F RID: 22047 RVA: 0x0020E7D8 File Offset: 0x0020C9D8
	public void SetChunk(Chunk _chunk, ChunkCluster _chunkCluster)
	{
		if (this.chunk != null && this.chunk != _chunk)
		{
			this.chunk.OnHide();
			Chunk chunk = this.chunk;
			lock (chunk)
			{
				this.chunk.IsCollisionMeshGenerated = false;
				this.chunk.IsDisplayed = false;
			}
		}
		for (int i = 0; i < this.layers.Length; i++)
		{
			ChunkGameObjectLayer chunkGameObjectLayer = this.layers[i];
			if (chunkGameObjectLayer != null)
			{
				chunkGameObjectLayer.m_ParentGO.SetActive(false);
				chunkGameObjectLayer.m_ParentGO.transform.SetParent(null, false);
				MemoryPools.poolCGOL.FreeSync(chunkGameObjectLayer);
				this.layers[i] = null;
			}
		}
		this.chunk = _chunk;
		this.chunkCluster = _chunkCluster;
		Transform transform = base.transform;
		if (this.chunk != null)
		{
			this.chunk.IsDisplayed = true;
			transform.name = _chunk.ToString();
			transform.localPosition = new Vector3((float)(this.chunk.X * 16), 0f, (float)(this.chunk.Z * 16)) - Origin.position;
			GameManager.Instance.StartCoroutine(this.HandleWallVolumes(this.chunk, this.chunkCluster));
			return;
		}
		transform.name = "ChunkEmpty";
		this.RemoveWallVolumes();
	}

	// Token: 0x06005620 RID: 22048 RVA: 0x0020E940 File Offset: 0x0020CB40
	[PublicizedFrom(EAccessModifier.Private)]
	public void RemoveWallVolumes()
	{
		if (this.wallVolumesParentT)
		{
			global::UnityEngine.Object.Destroy(this.wallVolumesParentT.gameObject);
		}
	}

	// Token: 0x06005621 RID: 22049 RVA: 0x0020E95F File Offset: 0x0020CB5F
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator HandleWallVolumes(Chunk _chunk, ChunkCluster _chunkCluster)
	{
		this.RemoveWallVolumes();
		List<int> wallVolumesId = _chunk.GetWallVolumes();
		WorldBase world = _chunkCluster.GetWorld();
		if (wallVolumesId.Count > 0)
		{
			while (!world.HasWallVolumes(wallVolumesId))
			{
				yield return new WaitForSeconds(1f);
			}
			if (!this.wallVolumesParentT)
			{
				this.wallVolumesParentT = new GameObject("_WallVolumes").transform;
				this.wallVolumesParentT.SetParent(base.transform, false);
			}
			Vector3 vector = _chunk.GetWorldPos();
			this.wallVolumes = new GameObject[wallVolumesId.Count];
			for (int i = 0; i < wallVolumesId.Count; i++)
			{
				int num = wallVolumesId[i];
				WallVolume wallVolume = world.GetWallVolume(num);
				GameObject gameObject = new GameObject(num.ToString());
				gameObject.layer = 16;
				Transform transform = gameObject.transform;
				transform.SetParent(this.wallVolumesParentT, false);
				transform.localPosition = wallVolume.Center - vector;
				gameObject.AddComponent<BoxCollider>().size = wallVolume.BoxMax - wallVolume.BoxMin;
				this.wallVolumes[i] = gameObject;
			}
		}
		yield break;
	}

	// Token: 0x06005622 RID: 22050 RVA: 0x0020E97C File Offset: 0x0020CB7C
	public int StartCopyMeshLayer()
	{
		this.currentlyCopiedMeshIdx = 0;
		this.isCopyCollidersThisCall = false;
		ChunkGameObjectLayer chunkGameObjectLayer;
		for (;;)
		{
			this.vml = this.chunk.GetMeshLayer();
			if (this.vml == null)
			{
				break;
			}
			chunkGameObjectLayer = this.layers[this.vml.idx];
			if (this.vml.HasContent())
			{
				goto IL_00A4;
			}
			if (chunkGameObjectLayer != null)
			{
				this.layers[this.vml.idx] = null;
				chunkGameObjectLayer.m_ParentGO.SetActive(false);
				chunkGameObjectLayer.m_ParentGO.transform.SetParent(null, false);
				MemoryPools.poolCGOL.FreeSync(chunkGameObjectLayer);
			}
			MemoryPools.poolVML.FreeSync(this.vml);
			this.vml = null;
		}
		return -1;
		IL_00A4:
		if (chunkGameObjectLayer == null)
		{
			chunkGameObjectLayer = MemoryPools.poolCGOL.AllocSync(false);
			chunkGameObjectLayer.Init(this.vml.idx, ChunkCluster.LayerMappingTable, base.transform, base.gameObject.isStatic);
			this.layers[this.vml.idx] = chunkGameObjectLayer;
		}
		this.vml.StartCopyMeshes();
		return this.vml.idx;
	}

	// Token: 0x06005623 RID: 22051 RVA: 0x0020EA8C File Offset: 0x0020CC8C
	public void EndCopyMeshLayer()
	{
		if (this.vml == null)
		{
			return;
		}
		ChunkGameObjectLayer chunkGameObjectLayer = this.layers[this.vml.idx];
		if (chunkGameObjectLayer != null)
		{
			chunkGameObjectLayer.m_ParentGO.SetActive(true);
			Occludee.Refresh(chunkGameObjectLayer.m_ParentGO);
		}
		this.vml.EndCopyMeshes();
		this.vml = null;
	}

	// Token: 0x06005624 RID: 22052 RVA: 0x0020EAE4 File Offset: 0x0020CCE4
	public bool CreateFromChunkNext(out int _startIdx, out int _endIdx, out int _triangles, out int _colliderTriangles)
	{
		this.nextMS.ResetAndRestart();
		_startIdx = this.currentlyCopiedMeshIdx;
		_triangles = 0;
		_colliderTriangles = 0;
		while (this.currentlyCopiedMeshIdx < MeshDescription.meshes.Length)
		{
			if (!this.isCopyCollidersThisCall && !this.chunk.NeedsOnlyCollisionMesh)
			{
				_triangles += this.copyToMesh(this.currentlyCopiedMeshIdx);
				this.isCopyCollidersThisCall = true;
			}
			else
			{
				_colliderTriangles += this.copyToColliders(this.currentlyCopiedMeshIdx);
				this.isCopyCollidersThisCall = false;
				this.currentlyCopiedMeshIdx++;
			}
			if ((float)this.nextMS.ElapsedMilliseconds >= 0.5f)
			{
				break;
			}
		}
		_endIdx = this.currentlyCopiedMeshIdx - 1;
		return this.currentlyCopiedMeshIdx < MeshDescription.meshes.Length;
	}

	// Token: 0x06005625 RID: 22053 RVA: 0x0020EBA0 File Offset: 0x0020CDA0
	public void CreateMeshAll(out int triangles, out int colliderTriangles)
	{
		triangles = 0;
		colliderTriangles = 0;
		for (int i = 0; i < MeshDescription.meshes.Length; i++)
		{
			colliderTriangles += this.copyToColliders(i);
		}
		if (!this.chunk.NeedsOnlyCollisionMesh)
		{
			for (int j = 0; j < MeshDescription.meshes.Length; j++)
			{
				triangles += this.copyToMesh(j);
			}
		}
	}

	// Token: 0x06005626 RID: 22054 RVA: 0x0020EC00 File Offset: 0x0020CE00
	[PublicizedFrom(EAccessModifier.Private)]
	public int copyToMesh(int _meshIdx)
	{
		ChunkGameObjectLayer chunkGameObjectLayer = this.layers[this.vml.idx];
		if (chunkGameObjectLayer == null)
		{
			return 0;
		}
		MeshFilter meshFilter = chunkGameObjectLayer.m_MeshFilter[_meshIdx];
		int num = this.vml.CopyToMesh(_meshIdx, meshFilter, chunkGameObjectLayer.m_MeshRenderer[_meshIdx], 0);
		bool flag = num != 0;
		if (!GameManager.bShowPaintables && _meshIdx == 0)
		{
			flag = false;
		}
		meshFilter.gameObject.SetActive(flag);
		if (num > 0)
		{
			this.CheckLODs(_meshIdx);
		}
		return num;
	}

	// Token: 0x06005627 RID: 22055 RVA: 0x0020EC6C File Offset: 0x0020CE6C
	[PublicizedFrom(EAccessModifier.Private)]
	public int copyToColliders(int _meshIdx)
	{
		ChunkGameObjectLayer chunkGameObjectLayer = this.layers[this.vml.idx];
		if (chunkGameObjectLayer == null)
		{
			return 0;
		}
		MeshCollider meshCollider = chunkGameObjectLayer.m_MeshCollider[_meshIdx];
		if (meshCollider == null)
		{
			return 0;
		}
		Mesh mesh;
		int num = this.vml.meshes[_meshIdx].CopyToColliders(meshCollider, out mesh);
		if (num != 0)
		{
			GameManager.Instance.World.m_ChunkManager.BakeAdd(mesh, meshCollider);
		}
		bool flag = num != 0;
		if (!GameManager.bShowPaintables && _meshIdx == 0)
		{
			flag = false;
		}
		meshCollider.gameObject.SetActive(flag);
		return num;
	}

	// Token: 0x06005628 RID: 22056 RVA: 0x0020ECF0 File Offset: 0x0020CEF0
	public void Cleanup()
	{
		base.gameObject.SetActive(false);
		for (int i = 0; i < this.layers.Length; i++)
		{
			if (this.layers[i] != null)
			{
				this.layers[i].Cleanup();
			}
		}
		for (int j = base.transform.childCount - 1; j >= 0; j--)
		{
			global::UnityEngine.Object.Destroy(base.transform.GetChild(j).gameObject);
		}
		for (int k = 0; k < this.layers.Length; k++)
		{
			if (this.layers[k] != null)
			{
				global::UnityEngine.Object.Destroy(this.layers[k].m_ParentGO);
			}
		}
		this.RemoveWallVolumes();
	}

	// Token: 0x06005629 RID: 22057 RVA: 0x0020ED98 File Offset: 0x0020CF98
	public void CheckLODs(int _limitToMesh = -1)
	{
		if (this.chunk == null)
		{
			return;
		}
		EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
		if (!primaryPlayer)
		{
			return;
		}
		float num = primaryPlayer.position.x - (float)(this.chunk.X * 16 + 8);
		float num2 = primaryPlayer.position.z - (float)(this.chunk.Z * 16 + 8);
		float num3 = num * num + num2 * num2;
		bool @bool = GamePrefs.GetBool(EnumGamePrefs.OptionsDisableChunkLODs);
		if (_limitToMesh == -1 || _limitToMesh == 4)
		{
			this.SetLOD(4, (@bool || num3 < 1681f) ? 0 : 1);
		}
		if (_limitToMesh == -1 || _limitToMesh == 3)
		{
			float num4 = 48f;
			float num5 = 0f;
			int @int = GamePrefs.GetInt(EnumGamePrefs.OptionsGfxGrassDistance);
			int int2 = GamePrefs.GetInt(EnumGamePrefs.OptionsGfxShadowDistance);
			switch (@int)
			{
			case 1:
				num4 = 64f;
				break;
			case 2:
				num4 = 96f;
				break;
			case 3:
				num4 = 112f;
				if (int2 >= 3)
				{
					num5 = ((int2 == 3) ? 2.3f : 3.6f) * 16f;
				}
				break;
			}
			bool flag = num3 < num4 * num4;
			bool flag2 = num3 < num5 * num5;
			for (int i = 0; i < this.layers.Length; i++)
			{
				ChunkGameObjectLayer chunkGameObjectLayer = this.layers[i];
				if (chunkGameObjectLayer != null)
				{
					GameObject gameObject = chunkGameObjectLayer.m_MeshesGO[3];
					if (gameObject)
					{
						if (flag)
						{
							if (!gameObject.activeSelf)
							{
								gameObject.SetActive(true);
							}
							if (flag2)
							{
								if (!chunkGameObjectLayer.isGrassCastShadows)
								{
									chunkGameObjectLayer.isGrassCastShadows = true;
									chunkGameObjectLayer.m_MeshRenderer[3].shadowCastingMode = ShadowCastingMode.On;
								}
							}
							else if (chunkGameObjectLayer.isGrassCastShadows)
							{
								chunkGameObjectLayer.isGrassCastShadows = false;
								chunkGameObjectLayer.m_MeshRenderer[3].shadowCastingMode = ShadowCastingMode.Off;
							}
						}
						else if (gameObject.activeSelf)
						{
							gameObject.SetActive(false);
						}
					}
				}
			}
		}
	}

	// Token: 0x0600562A RID: 22058 RVA: 0x0020EF74 File Offset: 0x0020D174
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetLOD(int _meshIdx, int _lodLevel)
	{
		if (_lodLevel == 0)
		{
			for (int i = 0; i < this.layers.Length; i++)
			{
				ChunkGameObjectLayer chunkGameObjectLayer = this.layers[i];
				if (chunkGameObjectLayer != null)
				{
					GameObject gameObject = chunkGameObjectLayer.m_MeshesGO[_meshIdx];
					if (gameObject && !gameObject.activeSelf)
					{
						gameObject.SetActive(true);
					}
				}
			}
			return;
		}
		for (int j = 0; j < this.layers.Length; j++)
		{
			ChunkGameObjectLayer chunkGameObjectLayer2 = this.layers[j];
			if (chunkGameObjectLayer2 != null)
			{
				GameObject gameObject2 = chunkGameObjectLayer2.m_MeshesGO[_meshIdx];
				if (gameObject2 && gameObject2.activeSelf)
				{
					gameObject2.SetActive(false);
				}
			}
		}
	}

	// Token: 0x0600562B RID: 22059 RVA: 0x0020F00A File Offset: 0x0020D20A
	public ChunkGameObjectLayer GetLayer(int _layer)
	{
		return this.layers[_layer];
	}

	// Token: 0x0400428D RID: 17037
	public Transform blockEntitiesParentT;

	// Token: 0x0400428E RID: 17038
	public static int InstanceCount;

	// Token: 0x0400428F RID: 17039
	public Chunk chunk;

	// Token: 0x04004290 RID: 17040
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ChunkCluster chunkCluster;

	// Token: 0x04004291 RID: 17041
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly ChunkGameObjectLayer[] layers = new ChunkGameObjectLayer[16];

	// Token: 0x04004292 RID: 17042
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public VoxelMeshLayer vml;

	// Token: 0x04004293 RID: 17043
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int currentlyCopiedMeshIdx;

	// Token: 0x04004294 RID: 17044
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isCopyCollidersThisCall;

	// Token: 0x04004295 RID: 17045
	public Transform wallVolumesParentT;

	// Token: 0x04004296 RID: 17046
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject[] wallVolumes;

	// Token: 0x04004297 RID: 17047
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public MicroStopwatch nextMS = new MicroStopwatch(false);
}
