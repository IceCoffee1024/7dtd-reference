using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C20 RID: 3104
public class WorldStats
{
	// Token: 0x170009C4 RID: 2500
	// (get) Token: 0x06005E4C RID: 24140 RVA: 0x0024DC7C File Offset: 0x0024BE7C
	public int TotalVertices
	{
		get
		{
			int num = 0;
			if (this.BlockEntities != null)
			{
				num = this.BlockEntities.Vertices;
			}
			if (this.ChunkMeshes != null)
			{
				foreach (WorldStats.MeshStats meshStats in this.ChunkMeshes)
				{
					if (meshStats != null)
					{
						num += meshStats.Vertices;
					}
				}
			}
			return num;
		}
	}

	// Token: 0x170009C5 RID: 2501
	// (get) Token: 0x06005E4D RID: 24141 RVA: 0x0024DCD0 File Offset: 0x0024BED0
	public int TotalTriangles
	{
		get
		{
			int num = 0;
			if (this.BlockEntities != null)
			{
				num = this.BlockEntities.Triangles;
			}
			if (this.ChunkMeshes != null)
			{
				foreach (WorldStats.MeshStats meshStats in this.ChunkMeshes)
				{
					if (meshStats != null)
					{
						num += meshStats.Triangles;
					}
				}
			}
			return num;
		}
	}

	// Token: 0x06005E4E RID: 24142 RVA: 0x0024DD21 File Offset: 0x0024BF21
	[PublicizedFrom(EAccessModifier.Private)]
	public WorldStats(WorldStats.MeshStats _blockEntities, WorldStats.MeshStats[] _chunkMeshes, float _lightsVolume)
	{
		this.BlockEntities = _blockEntities;
		this.ChunkMeshes = _chunkMeshes;
		this.LightsVolume = _lightsVolume;
	}

	// Token: 0x06005E4F RID: 24143 RVA: 0x0024DD40 File Offset: 0x0024BF40
	public static WorldStats FromProperties(DynamicProperties _props)
	{
		WorldStats.MeshStats meshStats = null;
		DynamicProperties dynamicProperties;
		if (_props.Classes.TryGetValue("BlockEntities", out dynamicProperties))
		{
			meshStats = WorldStats.MeshStats.FromProperties(dynamicProperties);
		}
		WorldStats.MeshStats[] array = new WorldStats.MeshStats[(MeshDescription.meshes.Length != 0) ? MeshDescription.meshes.Length : 20];
		for (int i = 0; i < array.Length; i++)
		{
			if (_props.Classes.TryGetValue("ChunkMeshes" + i.ToString(), out dynamicProperties))
			{
				array[i] = WorldStats.MeshStats.FromProperties(dynamicProperties);
			}
		}
		float num = 0f;
		string text;
		if (_props.Values.TryGetValue("LightsVolume", out text) && !StringParsers.TryParseFloat(text, out num))
		{
			Log.Warning("Could not parse LightsVolume string '" + text + "'");
		}
		return new WorldStats(meshStats, array, num);
	}

	// Token: 0x06005E50 RID: 24144 RVA: 0x0024DE04 File Offset: 0x0024C004
	public DynamicProperties ToProperties()
	{
		DynamicProperties dynamicProperties = new DynamicProperties();
		if (this.BlockEntities != null)
		{
			dynamicProperties.Classes["BlockEntities"] = this.BlockEntities.ToProperties();
		}
		if (this.ChunkMeshes != null)
		{
			for (int i = 0; i < this.ChunkMeshes.Length; i++)
			{
				if (this.ChunkMeshes[i] != null)
				{
					dynamicProperties.Classes["ChunkMeshes" + i.ToString()] = this.ChunkMeshes[i].ToProperties();
				}
			}
		}
		dynamicProperties.Values["LightsVolume"] = this.LightsVolume.ToCultureInvariantString();
		dynamicProperties.Values["TotalVertices"] = this.TotalVertices.ToString();
		dynamicProperties.Values["TotalTriangles"] = this.TotalTriangles.ToString();
		return dynamicProperties;
	}

	// Token: 0x06005E51 RID: 24145 RVA: 0x0024DEE0 File Offset: 0x0024C0E0
	public static WorldStats CaptureWorldStats()
	{
		MicroStopwatch microStopwatch = new MicroStopwatch(true);
		IEnumerable<ChunkGameObject> displayedChunkGameObjects = GameManager.Instance.World.m_ChunkManager.GetDisplayedChunkGameObjects();
		int num = 0;
		int num2 = 0;
		float num3 = 0f;
		int[] array = new int[MeshDescription.meshes.Length];
		int[] array2 = new int[MeshDescription.meshes.Length];
		foreach (ChunkGameObject chunkGameObject in displayedChunkGameObjects)
		{
			Chunk chunk = chunkGameObject.GetChunk();
			if (!chunkGameObject.blockEntitiesParentT)
			{
				Log.Warning("{0} WorldStats CaptureWorldStats cgo {1}, null", new object[]
				{
					Time.frameCount,
					chunk
				});
			}
			else
			{
				foreach (object obj in chunkGameObject.blockEntitiesParentT)
				{
					Transform transform = (Transform)obj;
					WorldStats.calcLightsVolume(transform.gameObject, ref num3);
					WorldStats.calcComplexityGameObject(transform.gameObject, ref num, ref num2, true, chunk);
				}
				for (int i = 0; i < 16; i++)
				{
					ChunkGameObjectLayer layer = chunkGameObject.GetLayer(i);
					if (layer != null)
					{
						for (int j = 0; j < layer.m_MeshFilter.Length; j++)
						{
							MeshFilter meshFilter = layer.m_MeshFilter[j];
							if (meshFilter)
							{
								Mesh sharedMesh = meshFilter.sharedMesh;
								if (sharedMesh)
								{
									array[j] += sharedMesh.vertexCount;
									array2[j] += sharedMesh.triangles.Length / 3;
								}
							}
						}
					}
				}
			}
		}
		WorldStats.MeshStats[] array3 = new WorldStats.MeshStats[array.Length];
		for (int k = 0; k < array.Length; k++)
		{
			array3[k] = new WorldStats.MeshStats(array[k], array2[k]);
		}
		WorldStats worldStats = new WorldStats(new WorldStats.MeshStats(num, num2), array3, num3);
		Log.Out("Measuring took {0} ms", new object[] { microStopwatch.ElapsedMilliseconds });
		return worldStats;
	}

	// Token: 0x06005E52 RID: 24146 RVA: 0x0024E120 File Offset: 0x0024C320
	[PublicizedFrom(EAccessModifier.Private)]
	public static void calcLightsVolume(GameObject _go, ref float _lightsVolume)
	{
		LightLOD componentInChildren = _go.GetComponentInChildren<LightLOD>();
		if (componentInChildren == null)
		{
			return;
		}
		if (!componentInChildren.bSwitchedOn)
		{
			return;
		}
		_lightsVolume += 4.1887903f * Mathf.Pow(componentInChildren.lightRange, 3f);
	}

	// Token: 0x06005E53 RID: 24147 RVA: 0x0024E164 File Offset: 0x0024C364
	[PublicizedFrom(EAccessModifier.Private)]
	public static void calcComplexityGameObject(GameObject _go, ref int _verts, ref int _tris, bool _onlyActive = true, Chunk _onChunk = null)
	{
		int num = 0;
		int num2 = 0;
		BlockEntityData blockEntity = _onChunk.GetBlockEntity(_go.transform);
		if (blockEntity == null)
		{
			Log.Warning("GameObject without BlockEntity: " + _go.GetGameObjectPath());
		}
		else if (blockEntity.blockValue.Block.IsSleeperBlock)
		{
			return;
		}
		HashSet<MeshRenderer> hashSet = new HashSet<MeshRenderer>();
		foreach (LODGroup lodgroup in _go.transform.GetComponentsInChildren<LODGroup>(!_onlyActive))
		{
			if (!(lodgroup == null))
			{
				LOD[] lods = lodgroup.GetLODs();
				for (int j = 0; j < lods.Length; j++)
				{
					foreach (Renderer renderer in lods[j].renderers)
					{
						if (!(renderer == null))
						{
							MeshRenderer meshRenderer = renderer as MeshRenderer;
							if (!(meshRenderer == null))
							{
								hashSet.Add(meshRenderer);
								if (j == 0 && meshRenderer.gameObject.activeInHierarchy)
								{
									MeshFilter component = meshRenderer.GetComponent<MeshFilter>();
									if (!(component == null))
									{
										Mesh sharedMesh = component.sharedMesh;
										if (sharedMesh)
										{
											if (!sharedMesh.isReadable)
											{
												EntityMeshCache component2 = _go.GetComponent<EntityMeshCache>();
												CachedMeshData cachedMeshData;
												if (component2 != null && component2.TryGetMeshData(sharedMesh.name, out cachedMeshData))
												{
													num += cachedMeshData.vertexCount;
													num2 += cachedMeshData.triCount;
												}
											}
											else
											{
												num += sharedMesh.vertexCount;
												num2 += sharedMesh.triangles.Length;
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
		foreach (MeshRenderer meshRenderer2 in _go.transform.GetComponentsInChildren<MeshRenderer>(!_onlyActive))
		{
			if (!hashSet.Contains(meshRenderer2))
			{
				MeshFilter component3 = meshRenderer2.GetComponent<MeshFilter>();
				if (!(component3 == null))
				{
					Mesh sharedMesh2 = component3.sharedMesh;
					if (sharedMesh2)
					{
						if (!sharedMesh2.isReadable)
						{
							EntityMeshCache component4 = _go.GetComponent<EntityMeshCache>();
							CachedMeshData cachedMeshData2;
							if (component4 != null && component4.TryGetMeshData(sharedMesh2.name, out cachedMeshData2))
							{
								num += cachedMeshData2.vertexCount;
								num2 += cachedMeshData2.triCount;
							}
						}
						else
						{
							num += sharedMesh2.vertexCount;
							num2 += sharedMesh2.triangles.Length;
						}
					}
				}
			}
		}
		num2 /= 3;
		_verts += num;
		_tris += num2;
	}

	// Token: 0x06005E54 RID: 24148 RVA: 0x0024E3CE File Offset: 0x0024C5CE
	public static IEnumerator CaptureCameraStatsCo(XUi _xui)
	{
		PrefabEditModeManager.Instance.UpdateMinMax();
		Vector3i minPos = PrefabEditModeManager.Instance.minPos;
		Vector3i maxPos = PrefabEditModeManager.Instance.maxPos;
		BoundsInt boundsInt = new BoundsInt(minPos, maxPos);
		GameObject uiRootObj = global::UnityEngine.Object.FindAnyObjectByType<UIRoot>().gameObject;
		Camera playerCam = _xui.playerUI.entityPlayer.playerCamera;
		GameObject camObj = new GameObject("StatsCam");
		Camera camera = camObj.AddComponent<Camera>();
		camera.CopyFrom(playerCam);
		camera.farClipPlane = 10000f;
		camObj.AddComponent<AudioListener>();
		Transform transform = camera.transform;
		transform.position = new Vector3(boundsInt.center.x, (float)(boundsInt.yMax + 2000), boundsInt.center.z);
		transform.eulerAngles = new Vector3(90f, 0f, 0f);
		float oldLodBias = QualitySettings.lodBias;
		ShadowQuality oldShadowQuality = QualitySettings.shadows;
		float oldShadowDistance = QualitySettings.shadowDistance;
		bool oldDisableChunkLoDs = GamePrefs.GetBool(EnumGamePrefs.OptionsDisableChunkLODs);
		ReflectionManager.ApplyOptions(true);
		playerCam.gameObject.SetActive(false);
		uiRootObj.SetActive(false);
		QualitySettings.lodBias = 1000000f;
		QualitySettings.shadows = ShadowQuality.Disable;
		QualitySettings.shadowDistance = 1000000f;
		SkyManager.skyManager.gameObject.SetActive(false);
		GamePrefs.Set(EnumGamePrefs.OptionsDisableChunkLODs, true);
		IList<ChunkGameObject> displayedChunkGameObjects = GameManager.Instance.World.m_ChunkManager.GetDisplayedChunkGameObjects();
		for (int i = 0; i < displayedChunkGameObjects.Count; i++)
		{
			displayedChunkGameObjects[i].CheckLODs(-1);
		}
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		ReflectionManager.ApplyOptions(false);
		playerCam.gameObject.SetActive(true);
		uiRootObj.SetActive(true);
		QualitySettings.lodBias = oldLodBias;
		QualitySettings.shadows = oldShadowQuality;
		QualitySettings.shadowDistance = oldShadowDistance;
		SkyManager.skyManager.gameObject.SetActive(true);
		GamePrefs.Set(EnumGamePrefs.OptionsDisableChunkLODs, oldDisableChunkLoDs);
		global::UnityEngine.Object.Destroy(camObj);
		yield break;
	}

	// Token: 0x04004958 RID: 18776
	public readonly WorldStats.MeshStats BlockEntities;

	// Token: 0x04004959 RID: 18777
	public readonly WorldStats.MeshStats[] ChunkMeshes;

	// Token: 0x0400495A RID: 18778
	public readonly float LightsVolume;

	// Token: 0x02000C21 RID: 3105
	public class MeshStats
	{
		// Token: 0x06005E55 RID: 24149 RVA: 0x0024E3DD File Offset: 0x0024C5DD
		public MeshStats(int _vertices, int _triangles)
		{
			this.Vertices = _vertices;
			this.Triangles = _triangles;
		}

		// Token: 0x06005E56 RID: 24150 RVA: 0x0024E3F4 File Offset: 0x0024C5F4
		public static WorldStats.MeshStats FromProperties(DynamicProperties _props)
		{
			int @int = _props.GetInt("Vertices");
			int int2 = _props.GetInt("Triangles");
			return new WorldStats.MeshStats(@int, int2);
		}

		// Token: 0x06005E57 RID: 24151 RVA: 0x0024E420 File Offset: 0x0024C620
		public DynamicProperties ToProperties()
		{
			DynamicProperties dynamicProperties = new DynamicProperties();
			dynamicProperties.Values["Vertices"] = this.Vertices.ToString();
			dynamicProperties.Values["Triangles"] = this.Triangles.ToString();
			return dynamicProperties;
		}

		// Token: 0x0400495B RID: 18779
		public readonly int Vertices;

		// Token: 0x0400495C RID: 18780
		public readonly int Triangles;
	}
}
