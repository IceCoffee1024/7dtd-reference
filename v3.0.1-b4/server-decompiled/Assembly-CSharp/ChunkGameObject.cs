using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000B0F RID: 2831
public class ChunkGameObject : MonoBehaviour
{
	// Token: 0x06005591 RID: 21905 RVA: 0x0020B983 File Offset: 0x00209B83
	[PublicizedFrom(EAccessModifier.Private)]
	public ChunkGameObject()
	{
	}

	// Token: 0x06005592 RID: 21906 RVA: 0x0020B9A4 File Offset: 0x00209BA4
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Awake()
	{
		this.blockEntitiesParentT = new GameObject("_BlockEntities").transform;
		this.blockEntitiesParentT.SetParent(base.transform, false);
	}

	// Token: 0x06005593 RID: 21907 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnEnable()
	{
	}

	// Token: 0x06005594 RID: 21908 RVA: 0x0020B9CD File Offset: 0x00209BCD
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

	// Token: 0x06005595 RID: 21909 RVA: 0x0020BA00 File Offset: 0x00209C00
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

	// Token: 0x06005596 RID: 21910 RVA: 0x0020BAB3 File Offset: 0x00209CB3
	public Chunk GetChunk()
	{
		return this.chunk;
	}

	// Token: 0x06005597 RID: 21911 RVA: 0x0020BABC File Offset: 0x00209CBC
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

	// Token: 0x06005598 RID: 21912 RVA: 0x0020BC24 File Offset: 0x00209E24
	[PublicizedFrom(EAccessModifier.Private)]
	public void RemoveWallVolumes()
	{
		if (this.wallVolumesParentT)
		{
			global::UnityEngine.Object.Destroy(this.wallVolumesParentT.gameObject);
		}
	}

	// Token: 0x06005599 RID: 21913 RVA: 0x0020BC43 File Offset: 0x00209E43
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

	// Token: 0x0600559A RID: 21914 RVA: 0x0020BC60 File Offset: 0x00209E60
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

	// Token: 0x0600559B RID: 21915 RVA: 0x0020BD70 File Offset: 0x00209F70
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

	// Token: 0x0600559C RID: 21916 RVA: 0x0020BDC8 File Offset: 0x00209FC8
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

	// Token: 0x0600559D RID: 21917 RVA: 0x0020BE84 File Offset: 0x0020A084
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

	// Token: 0x0600559E RID: 21918 RVA: 0x0020BEE4 File Offset: 0x0020A0E4
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

	// Token: 0x0600559F RID: 21919 RVA: 0x0020BF50 File Offset: 0x0020A150
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

	// Token: 0x060055A0 RID: 21920 RVA: 0x0020BFD4 File Offset: 0x0020A1D4
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

	// Token: 0x060055A1 RID: 21921 RVA: 0x0020C07C File Offset: 0x0020A27C
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

	// Token: 0x060055A2 RID: 21922 RVA: 0x0020C258 File Offset: 0x0020A458
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

	// Token: 0x060055A3 RID: 21923 RVA: 0x0020C2EE File Offset: 0x0020A4EE
	public ChunkGameObjectLayer GetLayer(int _layer)
	{
		return this.layers[_layer];
	}

	// Token: 0x040041F2 RID: 16882
	public Transform blockEntitiesParentT;

	// Token: 0x040041F3 RID: 16883
	public static int InstanceCount;

	// Token: 0x040041F4 RID: 16884
	public Chunk chunk;

	// Token: 0x040041F5 RID: 16885
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ChunkCluster chunkCluster;

	// Token: 0x040041F6 RID: 16886
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly ChunkGameObjectLayer[] layers = new ChunkGameObjectLayer[16];

	// Token: 0x040041F7 RID: 16887
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public VoxelMeshLayer vml;

	// Token: 0x040041F8 RID: 16888
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int currentlyCopiedMeshIdx;

	// Token: 0x040041F9 RID: 16889
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isCopyCollidersThisCall;

	// Token: 0x040041FA RID: 16890
	public Transform wallVolumesParentT;

	// Token: 0x040041FB RID: 16891
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject[] wallVolumes;

	// Token: 0x040041FC RID: 16892
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public MicroStopwatch nextMS = new MicroStopwatch(false);
}
