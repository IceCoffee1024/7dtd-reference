using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x020008BB RID: 2235
public class OcclusionManager : MonoBehaviour
{
	// Token: 0x170006C6 RID: 1734
	// (get) Token: 0x060040BE RID: 16574 RVA: 0x00193C5F File Offset: 0x00191E5F
	// (set) Token: 0x060040BF RID: 16575 RVA: 0x00193C67 File Offset: 0x00191E67
	public bool GPUCullingEnabled
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.gpuCullingEnabled;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.gpuCullingEnabled = value;
			Shader.SetGlobalFloat("_ChunkTransparentOcclusionDisabled", value ? 0f : 1f);
		}
	}

	// Token: 0x060040C0 RID: 16576 RVA: 0x00193C89 File Offset: 0x00191E89
	public static void Load()
	{
		global::UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("Occlusion/Occlusion"));
	}

	// Token: 0x060040C1 RID: 16577 RVA: 0x00193C9B File Offset: 0x00191E9B
	public void EnableCulling(bool isCulling)
	{
		this.GPUCullingEnabled = isCulling;
	}

	// Token: 0x060040C2 RID: 16578 RVA: 0x00193CA4 File Offset: 0x00191EA4
	public void SetMultipleCameras(bool isMultiple)
	{
		if (!this.isEnabled)
		{
			return;
		}
		this.GPUCullingEnabled = !isMultiple;
		if (isMultiple)
		{
			this.SetRenderersEnabled(true);
		}
	}

	// Token: 0x060040C3 RID: 16579 RVA: 0x00193CC4 File Offset: 0x00191EC4
	public void WorldChanging(bool isEditWorld)
	{
		this.isEnabled = GamePrefs.GetBool(EnumGamePrefs.OptionsGfxOcclusion);
		if (isEditWorld)
		{
			this.isEnabled = false;
		}
		if (GameManager.IsDedicatedServer)
		{
			this.isEnabled = false;
		}
		if (this.isEnabled)
		{
			if (!SystemInfo.supportsAsyncGPUReadback)
			{
				Log.Warning("Occlusion: !supportsAsyncGPUReadback");
				this.isEnabled = false;
			}
			if (!SystemInfo.supportsComputeShaders)
			{
				Log.Warning("Occlusion: !supportsComputeShaders");
				this.isEnabled = false;
			}
		}
		if (this.isEnabled && !this.depthTestMat)
		{
			this.depthTestMat = Resources.Load<Material>("Occlusion/OcclusionDepthTest");
			if (this.depthTestMat == null)
			{
				Log.Error("Occlusion: Missing OcclusionDepthTest mat");
				this.isEnabled = false;
			}
		}
		if (!this.isEnabled)
		{
			base.gameObject.SetActive(false);
			Log.Out("Occlusion: Disabled");
		}
		else
		{
			base.gameObject.SetActive(true);
			Log.Out("Occlusion: Enabled");
		}
		this.GPUCullingEnabled = this.isEnabled;
		this.SetAllCullingTypes(false);
		if (!this.isEnabled)
		{
			return;
		}
		this.cullChunkEntities = true;
		this.cullChunkLayers = true;
		this.cullDecorations = true;
		this.cullEntities = true;
		this.hugeErrorCount = 0;
	}

	// Token: 0x060040C4 RID: 16580 RVA: 0x00193DEC File Offset: 0x00191FEC
	public void AddChunkTransforms(Chunk chunk, List<Transform> transforms)
	{
		OcclusionManager.OccludeeZone occludeeZone = chunk.occludeeZone;
		if (occludeeZone == null)
		{
			occludeeZone = new OcclusionManager.OccludeeZone();
			occludeeZone.extentsTotalMax = 24f;
			chunk.occludeeZone = occludeeZone;
		}
		for (int i = transforms.Count - 1; i >= 0; i--)
		{
			Transform transform = transforms[i];
			if (transform)
			{
				occludeeZone.AddTransform(transform);
			}
		}
		OcclusionManager.tempRenderers.Clear();
		this.UpdateZoneRegistration(occludeeZone);
	}

	// Token: 0x060040C5 RID: 16581 RVA: 0x00193E58 File Offset: 0x00192058
	public void RemoveChunkTransforms(Chunk chunk, List<Transform> transforms)
	{
		OcclusionManager.OccludeeZone occludeeZone = chunk.occludeeZone;
		if (occludeeZone == null)
		{
			return;
		}
		for (int i = transforms.Count - 1; i >= 0; i--)
		{
			Transform transform = transforms[i];
			occludeeZone.RemoveTransform(transform);
		}
		this.UpdateZoneRegistration(occludeeZone);
	}

	// Token: 0x060040C6 RID: 16582 RVA: 0x00193E9C File Offset: 0x0019209C
	public void RemoveChunk(Chunk chunk)
	{
		OcclusionManager.OccludeeZone occludeeZone = chunk.occludeeZone;
		if (occludeeZone == null)
		{
			return;
		}
		this.RemoveFullZone(occludeeZone);
		chunk.occludeeZone = null;
	}

	// Token: 0x060040C7 RID: 16583 RVA: 0x00193EC4 File Offset: 0x001920C4
	public void RemoveFullZone(OcclusionManager.OccludeeZone zone)
	{
		for (int i = zone.layers.Length - 1; i >= 0; i--)
		{
			OcclusionManager.OccludeeLayer occludeeLayer = zone.layers[i];
			if (occludeeLayer != null && occludeeLayer.node != null)
			{
				this.UnregisterOccludee(occludeeLayer.node);
				occludeeLayer.node = null;
			}
		}
	}

	// Token: 0x060040C8 RID: 16584 RVA: 0x00193F10 File Offset: 0x00192110
	public void AddDeco(DecoChunk chunk, List<Transform> addTs)
	{
		OcclusionManager.OccludeeZone occludeeZone = chunk.occludeeZone;
		if (occludeeZone == null)
		{
			occludeeZone = new OcclusionManager.OccludeeZone();
			occludeeZone.extentsTotalMax = 189.44f;
			chunk.occludeeZone = occludeeZone;
		}
		if (!this.pendingZones.Contains(occludeeZone))
		{
			this.pendingZones.Add(occludeeZone);
		}
		occludeeZone.addTs.AddRange(addTs);
	}

	// Token: 0x060040C9 RID: 16585 RVA: 0x00193F68 File Offset: 0x00192168
	public void RemoveDeco(DecoChunk chunk, Transform removeT)
	{
		OcclusionManager.OccludeeZone occludeeZone = chunk.occludeeZone;
		if (occludeeZone == null)
		{
			Log.Error("RemoveDeco !zone");
			return;
		}
		if (!this.pendingZones.Contains(occludeeZone))
		{
			this.pendingZones.Add(occludeeZone);
		}
		occludeeZone.addTs.Remove(removeT);
		occludeeZone.removeTs.Add(removeT);
	}

	// Token: 0x060040CA RID: 16586 RVA: 0x00193FC0 File Offset: 0x001921C0
	public void RemoveDecoChunk(DecoChunk chunk)
	{
		OcclusionManager.OccludeeZone occludeeZone = chunk.occludeeZone;
		if (occludeeZone == null)
		{
			return;
		}
		this.RemoveFullZone(occludeeZone);
		chunk.occludeeZone = null;
	}

	// Token: 0x060040CB RID: 16587 RVA: 0x00193FE8 File Offset: 0x001921E8
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateZones()
	{
		if (this.pendingZones.Count > 0)
		{
			for (int i = this.pendingZones.Count - 1; i >= 0; i--)
			{
				OcclusionManager.OccludeeZone occludeeZone = this.pendingZones[i];
				this.UpdateZonePending(occludeeZone);
			}
			this.pendingZones.Clear();
		}
	}

	// Token: 0x060040CC RID: 16588 RVA: 0x0019403C File Offset: 0x0019223C
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateZonePending(OcclusionManager.OccludeeZone zone)
	{
		for (int i = zone.removeTs.Count - 1; i >= 0; i--)
		{
			Transform transform = zone.removeTs[i];
			zone.RemoveTransform(transform);
		}
		zone.removeTs.Clear();
		for (int j = zone.addTs.Count - 1; j >= 0; j--)
		{
			Transform transform2 = zone.addTs[j];
			if (transform2)
			{
				zone.AddTransform(transform2);
			}
		}
		zone.addTs.Clear();
		this.UpdateZoneRegistration(zone);
	}

	// Token: 0x060040CD RID: 16589 RVA: 0x001940C8 File Offset: 0x001922C8
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateZoneRegistration(OcclusionManager.OccludeeZone zone)
	{
		for (int i = zone.layers.Length - 1; i >= 0; i--)
		{
			OcclusionManager.OccludeeLayer occludeeLayer = zone.layers[i];
			if (occludeeLayer != null && occludeeLayer.isOld)
			{
				occludeeLayer.isOld = false;
				if (occludeeLayer.node != null)
				{
					this.UnregisterOccludee(occludeeLayer.node);
					occludeeLayer.node = null;
				}
				if (occludeeLayer.renderers.Count > 0)
				{
					List<Renderer> list = new List<Renderer>();
					foreach (KeyValuePair<int, OcclusionManager.OccludeeRenderers> keyValuePair in occludeeLayer.renderers)
					{
						list.AddRange(keyValuePair.Value.renderers);
					}
					occludeeLayer.node = this.RegisterOccludee(list.ToArray(), zone.extentsTotalMax);
					if (occludeeLayer.node == null)
					{
						Log.Warning(string.Format("Occlusion: Register({0}.layers[{1}]) Failed as OcclusionManager is out of entries", zone, i));
					}
				}
			}
		}
	}

	// Token: 0x060040CE RID: 16590 RVA: 0x001941C8 File Offset: 0x001923C8
	public static void AddEntity(EntityAlive _ea, float extentsTotalMax = 32f)
	{
		OcclusionManager instance = OcclusionManager.Instance;
		if (instance != null)
		{
			if (!instance.cullEntities)
			{
				return;
			}
			_ea.GetComponentsInChildren<Renderer>(true, OcclusionManager.tempRenderers);
			for (int i = OcclusionManager.tempRenderers.Count - 1; i >= 0; i--)
			{
				if (OcclusionManager.tempRenderers[i] is ParticleSystemRenderer)
				{
					OcclusionManager.tempRenderers.RemoveAt(i);
				}
				else if (OcclusionManager.tempRenderers[i].gameObject.CompareTag("NoOcclude"))
				{
					OcclusionManager.tempRenderers.RemoveAt(i);
				}
			}
			if (OcclusionManager.tempRenderers.Count > 0)
			{
				OcclusionManager.OccludeeEntity occludeeEntity = new OcclusionManager.OccludeeEntity();
				occludeeEntity.entity = _ea;
				occludeeEntity.pos = _ea.position;
				occludeeEntity.entry = instance.RegisterOccludee(OcclusionManager.tempRenderers.ToArray(), extentsTotalMax);
				if (occludeeEntity.entry != null)
				{
					instance.entities[_ea.entityId] = occludeeEntity;
					OcclusionManager.tempRenderers.Clear();
					return;
				}
				Log.Warning("Occlusion: Register({0}) Failed as OcclusionManager is out of entries", new object[] { _ea });
			}
		}
	}

	// Token: 0x060040CF RID: 16591 RVA: 0x001942D0 File Offset: 0x001924D0
	public static void RemoveEntity(EntityAlive _ea)
	{
		OcclusionManager instance = OcclusionManager.Instance;
		if (instance != null)
		{
			if (!instance.cullEntities)
			{
				return;
			}
			OcclusionManager.OccludeeEntity occludeeEntity;
			if (!instance.entities.TryGetValue(_ea.entityId, out occludeeEntity))
			{
				return;
			}
			instance.entities.Remove(_ea.entityId);
			if (occludeeEntity.entry != null)
			{
				instance.UnregisterOccludee(occludeeEntity.entry);
				occludeeEntity.entry = null;
			}
		}
	}

	// Token: 0x060040D0 RID: 16592 RVA: 0x00194338 File Offset: 0x00192538
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateEntities()
	{
		foreach (KeyValuePair<int, OcclusionManager.OccludeeEntity> keyValuePair in this.entities)
		{
			OcclusionManager.OccludeeEntity value = keyValuePair.Value;
			if (value != null && value.entity != null && (value.pos - value.entity.position).sqrMagnitude > 1f)
			{
				value.pos = value.entity.position;
				value.entry.Value.isAreaFound = false;
			}
		}
	}

	// Token: 0x060040D1 RID: 16593 RVA: 0x001943E4 File Offset: 0x001925E4
	public static void AddLight(LightLOD _light)
	{
		OcclusionManager instance = OcclusionManager.Instance;
		if (instance != null)
		{
			OcclusionManager.OccludeeLight occludeeLight = new OcclusionManager.OccludeeLight();
			occludeeLight.lightLOD = _light;
			occludeeLight.pos = _light.transform.position + Origin.position;
			occludeeLight.entry = instance.RegisterOccludee(null, 16f);
			if (occludeeLight.entry != null)
			{
				occludeeLight.entry.Value.light = occludeeLight;
			}
			else
			{
				Log.Warning("Occlusion: Register({0}) Failed as OcclusionManager is out of entries", new object[] { _light });
			}
			int hashCode = _light.GetHashCode();
			instance.lights[hashCode] = occludeeLight;
		}
	}

	// Token: 0x060040D2 RID: 16594 RVA: 0x00194480 File Offset: 0x00192680
	public static void RemoveLight(LightLOD _light)
	{
		OcclusionManager instance = OcclusionManager.Instance;
		if (instance != null)
		{
			int hashCode = _light.GetHashCode();
			OcclusionManager.OccludeeLight occludeeLight;
			if (!instance.lights.TryGetValue(hashCode, out occludeeLight))
			{
				Log.Warning("Occlusion: RemoveLight {0} missing", new object[] { _light });
				return;
			}
			instance.lights.Remove(hashCode);
			if (occludeeLight.entry != null)
			{
				instance.UnregisterOccludee(occludeeLight.entry);
				occludeeLight.entry.Value.light = null;
			}
		}
	}

	// Token: 0x060040D3 RID: 16595 RVA: 0x001944FC File Offset: 0x001926FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateLights()
	{
		foreach (KeyValuePair<int, OcclusionManager.OccludeeLight> keyValuePair in this.lights)
		{
			OcclusionManager.OccludeeLight value = keyValuePair.Value;
			Vector3 vector = value.lightLOD.transform.position + Origin.position;
			if ((value.pos - vector).sqrMagnitude > 1f)
			{
				value.pos = vector;
				value.entry.Value.isAreaFound = false;
				Log.Warning("Occludee light moved {0}, {1}", new object[]
				{
					value.lightLOD.name,
					value.pos
				});
			}
		}
	}

	// Token: 0x060040D4 RID: 16596 RVA: 0x001945D4 File Offset: 0x001927D4
	public LinkedListNode<OcclusionManager.OcclusionEntry> RegisterOccludee(Renderer[] renderers, float extentsTotalMax = 32f)
	{
		if (!this.isEnabled)
		{
			return null;
		}
		LinkedListNode<OcclusionManager.OcclusionEntry> first = this.freeEntries.First;
		if (first != null)
		{
			this.freeEntries.RemoveFirst();
			this.freeEntryCount--;
			this.usedEntries.AddFirst(first);
			this.usedEntryCount++;
			OcclusionManager.OcclusionEntry value = first.Value;
			value.allRenderers = renderers;
			float num = extentsTotalMax * 0.55f;
			value.cullStartDistSq = num * num;
			value.extentsTotalMax = extentsTotalMax;
			value.centerPos.y = -9999f;
			value.isAreaFound = false;
			value.isForceOn = true;
			value.isVisible = true;
			this.totalEntryCount++;
			return first;
		}
		if (Time.frameCount != this.errorFrame)
		{
			this.errorFrame = Time.frameCount;
			if (!this.outOfEntriesErrorReported)
			{
				Log.Warning("Occlusion used all entries");
				Log.Warning("Occlusion used all entries");
			}
			this.outOfEntriesErrorReported = true;
		}
		return null;
	}

	// Token: 0x060040D5 RID: 16597 RVA: 0x001946C8 File Offset: 0x001928C8
	[PublicizedFrom(EAccessModifier.Private)]
	public void CalcArea(OcclusionManager.OcclusionEntry entry)
	{
		Vector3 vector = Vector3.zero;
		if (entry.light != null)
		{
			entry.centerPos = entry.light.pos - Origin.position;
			float num = entry.light.lightLOD.GetLight().range;
			num *= 0.8f;
			vector.x = num;
			vector.y = num;
			vector.z = num;
		}
		else
		{
			if (entry.renderItems == null)
			{
				OcclusionManager.RenderItem[] array = new OcclusionManager.RenderItem[entry.allRenderers.Length];
				entry.renderItems = array;
				int num2 = 0;
				for (int i = entry.allRenderers.Length - 1; i >= 0; i--)
				{
					Renderer renderer = entry.allRenderers[i];
					if (renderer && !renderer.forceRenderingOff)
					{
						ShadowCastingMode shadowCastingMode = renderer.shadowCastingMode;
						if (shadowCastingMode != ShadowCastingMode.ShadowsOnly)
						{
							Vector3 extents = renderer.bounds.extents;
							if (extents.x <= 29f && extents.y <= 35f && extents.z <= 29f)
							{
								OcclusionManager.RenderItem renderItem;
								renderItem.renderer = renderer;
								renderItem.shadowMode = shadowCastingMode;
								array[num2] = renderItem;
								num2++;
							}
						}
					}
				}
				entry.renderItemsUsed = num2;
			}
			Bounds bounds = default(Bounds);
			bool flag = false;
			for (int j = entry.renderItemsUsed - 1; j >= 0; j--)
			{
				Renderer renderer2 = entry.renderItems[j].renderer;
				if (renderer2)
				{
					Bounds bounds2 = renderer2.bounds;
					if (bounds2.extents.sqrMagnitude > 0.001f)
					{
						if (!flag)
						{
							bounds = bounds2;
							flag = true;
						}
						else
						{
							bounds.Encapsulate(bounds2);
							if (bounds.extents.x > entry.extentsTotalMax || bounds.extents.z > entry.extentsTotalMax)
							{
								this.hugeErrorCount++;
								return;
							}
						}
					}
				}
			}
			entry.centerPos = bounds.center;
			vector = bounds.extents;
		}
		if (vector.x < 2f)
		{
			vector.x = 2f;
		}
		if (vector.y < 2f)
		{
			vector.y = 2f;
		}
		if (vector.z < 2f)
		{
			vector.z = 2f;
		}
		entry.size = vector * 4f;
		this.areaMatrix.m03 = entry.centerPos.x;
		this.areaMatrix.m13 = entry.centerPos.y;
		this.areaMatrix.m23 = entry.centerPos.z;
		this.areaMatrix.m00 = entry.size.x;
		this.areaMatrix.m11 = entry.size.y;
		this.areaMatrix.m22 = entry.size.z;
		this.objectMatrixLists[entry.matrixUnitIndex][entry.matrixSubIndex] = this.areaMatrix;
		entry.isAreaFound = true;
	}

	// Token: 0x060040D6 RID: 16598 RVA: 0x001949DC File Offset: 0x00192BDC
	public void UnregisterOccludee(LinkedListNode<OcclusionManager.OcclusionEntry> node)
	{
		if (node != null)
		{
			OcclusionManager.OcclusionEntry value = node.Value;
			this.usedEntries.Remove(node);
			this.usedEntryCount--;
			this.freeEntries.AddFirst(node);
			this.freeEntryCount++;
			value.allRenderers = null;
			if (value.renderItems != null)
			{
				for (int i = value.renderItemsUsed - 1; i >= 0; i--)
				{
					OcclusionManager.RenderItem renderItem = value.renderItems[i];
					if (renderItem.renderer)
					{
						renderItem.renderer.forceRenderingOff = false;
						renderItem.renderer.shadowCastingMode = renderItem.shadowMode;
					}
				}
				value.renderItems = null;
				value.renderItemsUsed = 0;
			}
			this.objectMatrixLists[value.matrixUnitIndex][value.matrixSubIndex] = this.tinyMatrix;
			this.totalEntryCount--;
		}
	}

	// Token: 0x060040D7 RID: 16599 RVA: 0x00194AC4 File Offset: 0x00192CC4
	public void OriginChanged(Vector3 offset)
	{
		for (LinkedListNode<OcclusionManager.OcclusionEntry> linkedListNode = this.usedEntries.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
		{
			OcclusionManager.OcclusionEntry value = linkedListNode.Value;
			value.centerPos += offset;
			this.areaMatrix.m03 = value.centerPos.x;
			this.areaMatrix.m13 = value.centerPos.y;
			this.areaMatrix.m23 = value.centerPos.z;
			this.areaMatrix.m00 = value.size.x;
			this.areaMatrix.m11 = value.size.y;
			this.areaMatrix.m22 = value.size.z;
			this.objectMatrixLists[value.matrixUnitIndex][value.matrixSubIndex] = this.areaMatrix;
		}
	}

	// Token: 0x060040D8 RID: 16600 RVA: 0x00194BB0 File Offset: 0x00192DB0
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		OcclusionManager.Instance = this;
		this.onRequestDelegate = new Action<AsyncGPUReadbackRequest>(this.OnRequest);
		this.tinyMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(0.0001f, 0.0001f, 0.0001f));
		this.cubeObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
		this.cubeMesh = this.cubeObj.GetComponent<MeshFilter>().sharedMesh;
		this.cubeObj.SetActive(false);
		this.cubeObj.transform.SetParent(base.gameObject.transform);
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < 4088; i++)
		{
			this.entries.Add(new OcclusionManager.OcclusionEntry());
			OcclusionManager.OcclusionEntry occlusionEntry = this.entries[i];
			if (num2 == 511)
			{
				num++;
				num2 = 0;
			}
			occlusionEntry.index = i;
			occlusionEntry.matrixUnitIndex = num;
			occlusionEntry.matrixSubIndex = num2++;
			this.freeEntries.AddLast(occlusionEntry);
			this.freeEntryCount++;
		}
		int num3 = 128;
		this.initialData = new uint[num3];
		this.visibleData = new uint[num3];
		for (int j = 0; j <= num; j++)
		{
			this.objectMatrixLists.Add(new Matrix4x4[511]);
		}
		for (int k = 0; k < this.initialData.Length; k++)
		{
			this.initialData[k] = 0U;
		}
		for (int l = 0; l < this.counterBuffer.Length; l++)
		{
			ComputeBuffer computeBuffer = new ComputeBuffer(num3, 4, ComputeBufferType.Default);
			computeBuffer.SetData(this.initialData);
			this.counterBuffer[l] = computeBuffer;
		}
		this.materialBlocks = new MaterialPropertyBlock[this.objectMatrixLists.Count];
		int num4 = 0;
		for (int m = 0; m < this.objectMatrixLists.Count; m++)
		{
			this.materialBlocks[m] = new MaterialPropertyBlock();
			this.materialBlocks[m].SetInt("_InstanceOffset", num4);
			num4 += this.objectMatrixLists[m].Length;
		}
		this.depthCamera = base.GetComponent<Camera>();
		this.depthCamera.enabled = false;
		GameOptionsManager.ResolutionChanged += this.OnResolutionChanged;
		this.CreateDepthRT();
		Log.Out("Occlusion: Awake");
	}

	// Token: 0x060040D9 RID: 16601 RVA: 0x00194E01 File Offset: 0x00193001
	public void SetSourceDepthCamera(Camera _camera)
	{
		if (this.depthCopyCmdBuf != null)
		{
			if (this.sourceDepthCamera != null)
			{
				this.sourceDepthCamera.RemoveCommandBuffer(CameraEvent.AfterForwardOpaque, this.depthCopyCmdBuf);
			}
			_camera.AddCommandBuffer(CameraEvent.AfterForwardOpaque, this.depthCopyCmdBuf);
		}
		this.sourceDepthCamera = _camera;
	}

	// Token: 0x060040DA RID: 16602 RVA: 0x00194E41 File Offset: 0x00193041
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetAllCullingTypes(bool _enabled)
	{
		this.cullChunkEntities = _enabled;
		this.cullChunkLayers = _enabled;
		this.cullDecorations = _enabled;
		this.cullDistantChunks = _enabled;
		this.cullDistantTerrain = _enabled;
		this.cullEntities = _enabled;
		this.cullLights = _enabled;
		this.cullPrefabs = _enabled;
	}

	// Token: 0x060040DB RID: 16603 RVA: 0x00194E7B File Offset: 0x0019307B
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnResolutionChanged(int _width, int _height)
	{
		this.CreateDepthRT();
	}

	// Token: 0x060040DC RID: 16604 RVA: 0x00194E84 File Offset: 0x00193084
	[PublicizedFrom(EAccessModifier.Private)]
	public void CreateDepthRT()
	{
		float num = (float)Screen.width / (float)Screen.height;
		if (this.depthRT)
		{
			this.depthRT.Release();
			this.depthRT = null;
		}
		this.depthRT = new RenderTexture(256, (int)(256f / num), 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
		this.depthRT.name = "Occlusion";
		this.depthRT.Create();
		this.depthCamera.targetTexture = this.depthRT;
	}

	// Token: 0x060040DD RID: 16605 RVA: 0x00194F08 File Offset: 0x00193108
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateVisibility(uint[] data)
	{
		this.visibleEntryCount = 0;
		this.hasVisibilityData = false;
		for (LinkedListNode<OcclusionManager.OcclusionEntry> linkedListNode = this.usedEntries.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
		{
			OcclusionManager.OcclusionEntry value = linkedListNode.Value;
			uint num = 0U;
			uint num2 = (uint)value.index >> 5;
			int num3 = value.index & 31;
			if (((ulong)data[(int)num2] & (ulong)(1L << (num3 & 31))) != 0UL)
			{
				num = 1U;
			}
			if (num > 0U || value.isForceOn)
			{
				if (!value.isVisible)
				{
					value.isVisible = true;
					if (value.light != null)
					{
						value.light.lightLOD.SetCulled(false);
					}
					for (int i = value.renderItemsUsed - 1; i >= 0; i--)
					{
						OcclusionManager.RenderItem renderItem = value.renderItems[i];
						if (renderItem.renderer)
						{
							if (renderItem.shadowMode != ShadowCastingMode.Off)
							{
								renderItem.renderer.shadowCastingMode = renderItem.shadowMode;
							}
							else
							{
								renderItem.renderer.forceRenderingOff = false;
							}
						}
					}
				}
				this.visibleEntryCount++;
			}
			else if (value.isVisible)
			{
				value.isVisible = false;
				if (value.light != null)
				{
					value.light.lightLOD.SetCulled(true);
				}
				for (int j = value.renderItemsUsed - 1; j >= 0; j--)
				{
					OcclusionManager.RenderItem renderItem2 = value.renderItems[j];
					if (renderItem2.renderer)
					{
						if (renderItem2.shadowMode != ShadowCastingMode.Off)
						{
							renderItem2.renderer.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
						}
						else
						{
							renderItem2.renderer.forceRenderingOff = true;
						}
					}
				}
			}
		}
	}

	// Token: 0x060040DE RID: 16606 RVA: 0x00195099 File Offset: 0x00193299
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		if (this.sourceDepthCamera != null && this.depthCopyCmdBuf != null)
		{
			this.sourceDepthCamera.AddCommandBuffer(CameraEvent.AfterForwardOpaque, this.depthCopyCmdBuf);
		}
	}

	// Token: 0x060040DF RID: 16607 RVA: 0x001950C4 File Offset: 0x001932C4
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		if (this.sourceDepthCamera != null && this.depthCopyCmdBuf != null)
		{
			this.sourceDepthCamera.RemoveCommandBuffer(CameraEvent.AfterForwardOpaque, this.depthCopyCmdBuf);
		}
		this.SetRenderersEnabled(true);
	}

	// Token: 0x060040E0 RID: 16608 RVA: 0x001950F8 File Offset: 0x001932F8
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDestroy()
	{
		this.isEnabled = false;
		this.GPUCullingEnabled = false;
		GameOptionsManager.ResolutionChanged -= this.OnResolutionChanged;
		if (this.depthRT)
		{
			this.depthRT.Release();
			this.depthRT = null;
		}
		if (this.depthCopyRT)
		{
			this.depthCopyRT.Release();
			this.depthCopyRT = null;
		}
		for (int i = 0; i < this.counterBuffer.Length; i++)
		{
			if (this.counterBuffer[i] != null)
			{
				this.counterBuffer[i].Release();
			}
		}
		if (this.depthCopyCmdBuf != null)
		{
			if (this.sourceDepthCamera != null)
			{
				this.sourceDepthCamera.RemoveCommandBuffer(CameraEvent.AfterForwardOpaque, this.depthCopyCmdBuf);
			}
			this.depthCopyCmdBuf.Dispose();
			this.depthCopyCmdBuf = null;
		}
	}

	// Token: 0x060040E1 RID: 16609 RVA: 0x001951C8 File Offset: 0x001933C8
	[PublicizedFrom(EAccessModifier.Private)]
	public void RenderOccludees(Camera renderCamera, int layer)
	{
		Vector3 position = this.sourceDepthCamera.transform.position;
		for (LinkedListNode<OcclusionManager.OcclusionEntry> linkedListNode = this.usedEntries.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
		{
			OcclusionManager.OcclusionEntry value = linkedListNode.Value;
			if ((position - value.centerPos).sqrMagnitude < value.cullStartDistSq)
			{
				value.isForceOn = true;
			}
			else if (!value.isAreaFound)
			{
				this.CalcArea(value);
			}
			else
			{
				value.isForceOn = false;
			}
		}
		Graphics.SetRandomWriteTarget(1, this.counterBuffer[this.counterBufferCurrentIndex]);
		for (int i = 0; i < this.objectMatrixLists.Count; i++)
		{
			Matrix4x4[] array = this.objectMatrixLists[i];
			Graphics.DrawMeshInstanced(this.cubeMesh, 0, this.depthTestMat, array, array.Length, this.materialBlocks[i], ShadowCastingMode.Off, false, layer, renderCamera);
		}
	}

	// Token: 0x060040E2 RID: 16610 RVA: 0x001952A4 File Offset: 0x001934A4
	public void LocalPlayerOnPreCull()
	{
		if (!this.GPUCullingEnabled)
		{
			return;
		}
		if (this.forceAllVisible || this.forceAllHidden)
		{
			this.SetRenderersEnabled(this.forceAllVisible);
			this.visibleEntryCount = this.totalEntryCount;
			return;
		}
		if (this.isCameraChanged)
		{
			return;
		}
		Vector3 forward = Camera.current.transform.forward;
		if (Vector3.Dot(this.camDirVec, forward) < 0.94f)
		{
			this.SetRenderersEnabled(true);
			return;
		}
		if (this.hasVisibilityData)
		{
			this.UpdateVisibility(this.visibleData);
		}
	}

	// Token: 0x060040E3 RID: 16611 RVA: 0x0019532C File Offset: 0x0019352C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnRenderObject()
	{
		if (this.isOcclusionChecking || !this.GPUCullingEnabled)
		{
			return;
		}
		if (GameManager.Instance.World == null)
		{
			return;
		}
		if (Camera.current == this.sourceDepthCamera)
		{
			this.UpdateEntities();
			this.UpdateLights();
			this.UpdateZones();
			this.isOcclusionChecking = true;
			this.isCameraChanged = false;
			Transform transform = this.sourceDepthCamera.transform;
			this.camDirVec = transform.forward;
			this.depthCamera.transform.position = transform.position;
			this.depthCamera.transform.rotation = transform.rotation;
			this.depthCamera.fieldOfView = this.sourceDepthCamera.fieldOfView;
			this.depthCamera.nearClipPlane = this.sourceDepthCamera.nearClipPlane;
			this.depthCamera.farClipPlane = this.sourceDepthCamera.farClipPlane;
			if (this.depthCopyCmdBuf != null)
			{
				Graphics.Blit(this.depthCopyRT, this.depthRT, this.depthCopyMat, 1);
			}
			else
			{
				Graphics.Blit(null, this.depthRT, this.depthCopyMat, 2);
			}
			this.depthCamera.Render();
		}
	}

	// Token: 0x060040E4 RID: 16612 RVA: 0x00195452 File Offset: 0x00193652
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPreCull()
	{
		this.counterBuffer[this.counterBufferCurrentIndex].SetData(this.initialData);
		this.RenderOccludees(this.depthCamera, 11);
	}

	// Token: 0x060040E5 RID: 16613 RVA: 0x0019547A File Offset: 0x0019367A
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPostRender()
	{
		Graphics.ClearRandomWriteTargets();
		AsyncGPUReadback.Request(this.counterBuffer[this.counterBufferCurrentIndex], this.onRequestDelegate);
		this.counterBufferCurrentIndex = (this.counterBufferCurrentIndex + 1) % this.counterBuffer.Length;
	}

	// Token: 0x060040E6 RID: 16614 RVA: 0x001954B4 File Offset: 0x001936B4
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnRequest(AsyncGPUReadbackRequest req)
	{
		this.isOcclusionChecking = false;
		if (this.isCameraChanged)
		{
			return;
		}
		req.GetData<uint>(0).CopyTo(this.visibleData);
		this.hasVisibilityData = true;
	}

	// Token: 0x060040E7 RID: 16615 RVA: 0x001954F0 File Offset: 0x001936F0
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetRenderersEnabled(bool isEnabled)
	{
		this.isCameraChanged = true;
		this.hasVisibilityData = false;
		for (LinkedListNode<OcclusionManager.OcclusionEntry> linkedListNode = this.usedEntries.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
		{
			OcclusionManager.OcclusionEntry value = linkedListNode.Value;
			if (value.isVisible != isEnabled && value.renderItems != null)
			{
				value.isVisible = isEnabled;
				for (int i = value.renderItemsUsed - 1; i >= 0; i--)
				{
					OcclusionManager.RenderItem renderItem = value.renderItems[i];
					if (renderItem.renderer)
					{
						if (renderItem.shadowMode != ShadowCastingMode.Off)
						{
							renderItem.renderer.shadowCastingMode = (isEnabled ? renderItem.shadowMode : ShadowCastingMode.ShadowsOnly);
						}
						else
						{
							renderItem.renderer.forceRenderingOff = !isEnabled;
						}
					}
				}
			}
		}
	}

	// Token: 0x060040E8 RID: 16616 RVA: 0x001955A8 File Offset: 0x001937A8
	[PublicizedFrom(EAccessModifier.Internal)]
	public bool WriteListToDisk(out List<string> fileList)
	{
		try
		{
			string text = Path.Join(GameIO.GetApplicationTempPath(), "OcclusionEntityDebugData.txt");
			fileList = new List<string> { Path.GetFullPath(text) };
			Log.WriteLine("Writing OcclusionEntityDebugData data to " + text);
			using (StreamWriter streamWriter = SdFile.CreateText(text))
			{
				streamWriter.WriteLine("{");
				streamWriter.WriteLine(string.Format("\"timestamp\"={0:yyyy-MM-dd HH:mm:ss},", DateTime.Now));
				streamWriter.WriteLine("\"totalEntryCount\"=" + this.totalEntryCount.ToString() + ",");
				streamWriter.WriteLine("\"visibleEntryCount\"=" + this.visibleEntryCount.ToString() + ",");
				streamWriter.WriteLine("\"freeEntryCount\"=" + this.freeEntryCount.ToString() + ",");
				streamWriter.WriteLine("\"usedEntryCount\"=" + this.usedEntryCount.ToString() + ",");
				foreach (OcclusionManager.OcclusionEntry occlusionEntry in this.entries)
				{
					streamWriter.WriteLine("\t\"index\"=" + occlusionEntry.index.ToString() + ",");
					streamWriter.WriteLine("\t\"matrixUnitIndex\"=" + occlusionEntry.matrixUnitIndex.ToString() + ",");
					streamWriter.WriteLine("\t\"matrixSubIndex\"=" + occlusionEntry.matrixSubIndex.ToString() + ",");
					streamWriter.WriteLine("\t\"isVisible\"=" + occlusionEntry.isVisible.ToString() + ",");
					streamWriter.WriteLine("\t\"isForceOn\"=" + occlusionEntry.isForceOn.ToString() + ",");
					streamWriter.WriteLine("\t\"isAreaFound\"=" + occlusionEntry.isAreaFound.ToString() + ",");
					streamWriter.WriteLine(string.Format("\t\"centerPos\"=\"{0:F1},{1:F1},{2:F1}\"", occlusionEntry.centerPos.x, occlusionEntry.centerPos.y, occlusionEntry.centerPos.z));
					streamWriter.WriteLine(string.Format("\t\"size\"=\"{0:F1},{1:F1},{2:F1}\",", occlusionEntry.size.x, occlusionEntry.size.y, occlusionEntry.size.z));
					streamWriter.WriteLine("\t\"renderItemsUsed\"=" + occlusionEntry.renderItemsUsed.ToString() + ",");
					if (occlusionEntry.light != null)
					{
						streamWriter.WriteLine(string.Format("\t\"lightPos\"=\"{0:F1},{1:F1},{2:F1}\",", occlusionEntry.light.pos.x, occlusionEntry.light.pos.y, occlusionEntry.light.pos.z));
						if (occlusionEntry.light.lightLOD != null)
						{
							streamWriter.WriteLine("\t\"lightLodGoName\"=\"" + ((occlusionEntry.light.lightLOD.gameObject == null) ? "NullGO" : occlusionEntry.light.lightLOD.gameObject.name) + "\",");
							streamWriter.WriteLine("\t\"lightLodLitRootObject\"=\"" + ((occlusionEntry.light.lightLOD.LitRootObject == null) ? "NullGO" : occlusionEntry.light.lightLOD.LitRootObject.name) + "\",");
						}
						else
						{
							streamWriter.WriteLine("\t\"lightLod\"=\"Null_LightLod\",");
						}
					}
					if (occlusionEntry.renderItems != null)
					{
						streamWriter.WriteLine("\t\"renderItemNames\"=[");
						foreach (OcclusionManager.RenderItem renderItem in occlusionEntry.renderItems)
						{
							if (renderItem.renderer)
							{
								streamWriter.WriteLine("\t\t\"" + renderItem.renderer.name.Replace(" ", "_") + "\",");
							}
							else
							{
								streamWriter.WriteLine("\t\t<NULL>,");
							}
						}
						streamWriter.WriteLine("\t\"],");
					}
					else
					{
						streamWriter.WriteLine("\t\"renderItemNames\"=[\"Empty\"],");
					}
				}
				streamWriter.WriteLine("\"freeNodeIndices\"=[");
				foreach (OcclusionManager.OcclusionEntry occlusionEntry2 in this.freeEntries)
				{
					streamWriter.WriteLine("\t" + occlusionEntry2.index.ToString() + ",");
				}
				streamWriter.WriteLine("],");
				streamWriter.WriteLine("\"usedNodeIndices\"=[");
				foreach (OcclusionManager.OcclusionEntry occlusionEntry3 in this.usedEntries)
				{
					streamWriter.WriteLine("\t" + occlusionEntry3.index.ToString() + ",");
				}
				streamWriter.WriteLine("],");
				streamWriter.WriteLine("}");
				streamWriter.Close();
			}
			GC.Collect();
		}
		catch (Exception ex)
		{
			string text2 = "Failed to write list to disk: ";
			Exception ex2 = ex;
			Log.Error(text2 + ((ex2 != null) ? ex2.ToString() : null));
			fileList = null;
			return false;
		}
		return true;
	}

	// Token: 0x060040E9 RID: 16617 RVA: 0x00195B7C File Offset: 0x00193D7C
	[PublicizedFrom(EAccessModifier.Private)]
	public static void LogOcclusion(string format, params object[] args)
	{
		format = string.Format("{0} OC {1}", GameManager.frameCount, format);
		Log.Warning(format, args);
	}

	// Token: 0x060040EA RID: 16618 RVA: 0x00195B9C File Offset: 0x00193D9C
	public void ToggleDebugView()
	{
		this.isDebugView = !this.isDebugView;
	}

	// Token: 0x060040EB RID: 16619 RVA: 0x00195BB0 File Offset: 0x00193DB0
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnGUI()
	{
		if (this.isDebugView && this.depthRT)
		{
			GUI.DrawTexture(new Rect(0f, 0f, 256f, (float)(256 * this.depthRT.height / this.depthRT.width)), this.depthRT);
			string text = string.Format("{0} of {1}, huge {2}", this.visibleEntryCount, this.usedEntries.Count, this.hugeErrorCount);
			GUI.color = Color.black;
			GUI.Label(new Rect(1f, 1f, 256f, 256f), text);
			GUI.color = Color.white;
			GUI.Label(new Rect(0f, 0f, 256f, 256f), text);
		}
	}

	// Token: 0x0400346D RID: 13421
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int cDepthRTWidth = 256;

	// Token: 0x0400346E RID: 13422
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cBoundsScale = 1f;

	// Token: 0x0400346F RID: 13423
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cCameraEnableAllAngleCos = 0.94f;

	// Token: 0x04003470 RID: 13424
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool outOfEntriesErrorReported;

	// Token: 0x04003471 RID: 13425
	public static OcclusionManager Instance;

	// Token: 0x04003472 RID: 13426
	public bool isEnabled;

	// Token: 0x04003473 RID: 13427
	public Material depthTestMat;

	// Token: 0x04003474 RID: 13428
	public int visibleEntryCount;

	// Token: 0x04003475 RID: 13429
	public int totalEntryCount;

	// Token: 0x04003476 RID: 13430
	public int freeEntryCount;

	// Token: 0x04003477 RID: 13431
	public int usedEntryCount;

	// Token: 0x04003478 RID: 13432
	public bool forceAllVisible;

	// Token: 0x04003479 RID: 13433
	public bool forceAllHidden;

	// Token: 0x0400347A RID: 13434
	public bool cullChunkEntities;

	// Token: 0x0400347B RID: 13435
	public bool cullChunkLayers;

	// Token: 0x0400347C RID: 13436
	public bool cullDecorations;

	// Token: 0x0400347D RID: 13437
	public bool cullDistantChunks;

	// Token: 0x0400347E RID: 13438
	public bool cullDistantTerrain;

	// Token: 0x0400347F RID: 13439
	public bool cullEntities;

	// Token: 0x04003480 RID: 13440
	public bool cullLights;

	// Token: 0x04003481 RID: 13441
	public bool cullPrefabs;

	// Token: 0x04003482 RID: 13442
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int cMaxUnits = 511;

	// Token: 0x04003483 RID: 13443
	[PublicizedFrom(EAccessModifier.Internal)]
	[NonSerialized]
	public const int cMaxEntries = 4088;

	// Token: 0x04003484 RID: 13444
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Matrix4x4 tinyMatrix;

	// Token: 0x04003485 RID: 13445
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ComputeBuffer[] counterBuffer = new ComputeBuffer[3];

	// Token: 0x04003486 RID: 13446
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int counterBufferCurrentIndex;

	// Token: 0x04003487 RID: 13447
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public uint[] initialData;

	// Token: 0x04003488 RID: 13448
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool hasVisibilityData;

	// Token: 0x04003489 RID: 13449
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public uint[] visibleData;

	// Token: 0x0400348A RID: 13450
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Camera sourceDepthCamera;

	// Token: 0x0400348B RID: 13451
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public CommandBuffer depthCopyCmdBuf;

	// Token: 0x0400348C RID: 13452
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<OcclusionManager.OcclusionEntry> entries = new List<OcclusionManager.OcclusionEntry>();

	// Token: 0x0400348D RID: 13453
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LinkedList<OcclusionManager.OcclusionEntry> freeEntries = new LinkedList<OcclusionManager.OcclusionEntry>();

	// Token: 0x0400348E RID: 13454
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LinkedList<OcclusionManager.OcclusionEntry> usedEntries = new LinkedList<OcclusionManager.OcclusionEntry>();

	// Token: 0x0400348F RID: 13455
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Matrix4x4[]> objectMatrixLists = new List<Matrix4x4[]>();

	// Token: 0x04003490 RID: 13456
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public MaterialPropertyBlock[] materialBlocks;

	// Token: 0x04003491 RID: 13457
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject cubeObj;

	// Token: 0x04003492 RID: 13458
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Mesh cubeMesh;

	// Token: 0x04003493 RID: 13459
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool gpuCullingEnabled;

	// Token: 0x04003494 RID: 13460
	public Camera depthCamera;

	// Token: 0x04003495 RID: 13461
	public Material depthCopyMat;

	// Token: 0x04003496 RID: 13462
	public RenderTexture depthRT;

	// Token: 0x04003497 RID: 13463
	public RenderTexture depthCopyRT;

	// Token: 0x04003498 RID: 13464
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isOcclusionChecking;

	// Token: 0x04003499 RID: 13465
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isCameraChanged;

	// Token: 0x0400349A RID: 13466
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Matrix4x4 areaMatrix = Matrix4x4.identity;

	// Token: 0x0400349B RID: 13467
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Action<AsyncGPUReadbackRequest> onRequestDelegate;

	// Token: 0x0400349C RID: 13468
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int errorFrame;

	// Token: 0x0400349D RID: 13469
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int hugeErrorCount;

	// Token: 0x0400349E RID: 13470
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<OcclusionManager.OccludeeZone> pendingZones = new List<OcclusionManager.OccludeeZone>();

	// Token: 0x0400349F RID: 13471
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly List<Renderer> tempRenderers = new List<Renderer>();

	// Token: 0x040034A0 RID: 13472
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Dictionary<int, OcclusionManager.OccludeeEntity> entities = new Dictionary<int, OcclusionManager.OccludeeEntity>();

	// Token: 0x040034A1 RID: 13473
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Dictionary<int, OcclusionManager.OccludeeLight> lights = new Dictionary<int, OcclusionManager.OccludeeLight>();

	// Token: 0x040034A2 RID: 13474
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 camDirVec;

	// Token: 0x040034A3 RID: 13475
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isDebugView;

	// Token: 0x020008BC RID: 2236
	public struct RenderItem
	{
		// Token: 0x040034A4 RID: 13476
		public Renderer renderer;

		// Token: 0x040034A5 RID: 13477
		public ShadowCastingMode shadowMode;
	}

	// Token: 0x020008BD RID: 2237
	public class OcclusionEntry
	{
		// Token: 0x040034A6 RID: 13478
		public Renderer[] allRenderers;

		// Token: 0x040034A7 RID: 13479
		public OcclusionManager.RenderItem[] renderItems;

		// Token: 0x040034A8 RID: 13480
		public int renderItemsUsed;

		// Token: 0x040034A9 RID: 13481
		public OcclusionManager.OccludeeLight light;

		// Token: 0x040034AA RID: 13482
		public float cullStartDistSq;

		// Token: 0x040034AB RID: 13483
		public float extentsTotalMax;

		// Token: 0x040034AC RID: 13484
		public int index;

		// Token: 0x040034AD RID: 13485
		public int matrixUnitIndex;

		// Token: 0x040034AE RID: 13486
		public int matrixSubIndex;

		// Token: 0x040034AF RID: 13487
		public bool isAreaFound;

		// Token: 0x040034B0 RID: 13488
		public Vector3 centerPos;

		// Token: 0x040034B1 RID: 13489
		public Vector3 size;

		// Token: 0x040034B2 RID: 13490
		public bool isForceOn;

		// Token: 0x040034B3 RID: 13491
		public bool isVisible;
	}

	// Token: 0x020008BE RID: 2238
	public class OccludeeZone
	{
		// Token: 0x060040EF RID: 16623 RVA: 0x00195D1C File Offset: 0x00193F1C
		public int GetIndex(float y)
		{
			int num = (int)y >> 3;
			if (num < 32)
			{
				return num;
			}
			if (num <= 0)
			{
				return 0;
			}
			return 31;
		}

		// Token: 0x060040F0 RID: 16624 RVA: 0x00195D40 File Offset: 0x00193F40
		public void AddTransform(Transform t)
		{
			int index = this.GetIndex(t.position.y + Origin.position.y);
			OcclusionManager.OccludeeLayer occludeeLayer = this.layers[index];
			if (occludeeLayer == null)
			{
				occludeeLayer = new OcclusionManager.OccludeeLayer();
				this.layers[index] = occludeeLayer;
			}
			occludeeLayer.isOld = true;
			OcclusionManager.OccludeeRenderers occludeeRenderers = occludeeLayer.AddTransform(t);
			t.GetComponentsInChildren<Renderer>(true, OcclusionManager.tempRenderers);
			if (OcclusionManager.tempRenderers.Count == 0)
			{
				OcclusionManager.LogOcclusion("AddTransform {0} tempRenderers 0", new object[] { t.name });
			}
			foreach (Renderer renderer in OcclusionManager.tempRenderers)
			{
				if (!(renderer is ParticleSystemRenderer) && renderer.shadowCastingMode != ShadowCastingMode.ShadowsOnly && !renderer.CompareTag("NoOcclude"))
				{
					occludeeRenderers.renderers.Add(renderer);
				}
			}
			OcclusionManager.tempRenderers.Clear();
		}

		// Token: 0x060040F1 RID: 16625 RVA: 0x00195E3C File Offset: 0x0019403C
		public void RemoveTransform(Transform t)
		{
			int hashCode = t.GetHashCode();
			if (t)
			{
				int index = this.GetIndex(t.position.y + Origin.position.y);
				OcclusionManager.OccludeeLayer occludeeLayer = this.layers[index];
				if (occludeeLayer != null && occludeeLayer.renderers.Remove(hashCode))
				{
					occludeeLayer.isOld = true;
					return;
				}
			}
			for (int i = 0; i < this.layers.Length; i++)
			{
				OcclusionManager.OccludeeLayer occludeeLayer2 = this.layers[i];
				if (occludeeLayer2 != null && occludeeLayer2.renderers.Remove(hashCode))
				{
					occludeeLayer2.isOld = true;
					return;
				}
			}
		}

		// Token: 0x040034B4 RID: 13492
		public float extentsTotalMax;

		// Token: 0x040034B5 RID: 13493
		public OcclusionManager.OccludeeLayer[] layers = new OcclusionManager.OccludeeLayer[32];

		// Token: 0x040034B6 RID: 13494
		public List<Transform> addTs = new List<Transform>();

		// Token: 0x040034B7 RID: 13495
		public List<Transform> removeTs = new List<Transform>();

		// Token: 0x040034B8 RID: 13496
		[PublicizedFrom(EAccessModifier.Private)]
		public const int cLayerShift = 3;

		// Token: 0x040034B9 RID: 13497
		[PublicizedFrom(EAccessModifier.Private)]
		public const int cLayerH = 8;

		// Token: 0x040034BA RID: 13498
		[PublicizedFrom(EAccessModifier.Private)]
		public const int cLayerCount = 32;
	}

	// Token: 0x020008BF RID: 2239
	public class OccludeeLayer
	{
		// Token: 0x060040F3 RID: 16627 RVA: 0x00195EFC File Offset: 0x001940FC
		public OcclusionManager.OccludeeRenderers AddTransform(Transform t)
		{
			int hashCode = t.GetHashCode();
			OcclusionManager.OccludeeRenderers occludeeRenderers;
			if (this.renderers.TryGetValue(hashCode, out occludeeRenderers))
			{
				Log.Warning("OccludeeLayer AddTransform {0} {1} exists", new object[]
				{
					t ? t.name : "",
					hashCode
				});
				return occludeeRenderers;
			}
			occludeeRenderers = new OcclusionManager.OccludeeRenderers();
			this.renderers.Add(hashCode, occludeeRenderers);
			return occludeeRenderers;
		}

		// Token: 0x040034BB RID: 13499
		public Dictionary<int, OcclusionManager.OccludeeRenderers> renderers = new Dictionary<int, OcclusionManager.OccludeeRenderers>();

		// Token: 0x040034BC RID: 13500
		public LinkedListNode<OcclusionManager.OcclusionEntry> node;

		// Token: 0x040034BD RID: 13501
		public bool isOld;
	}

	// Token: 0x020008C0 RID: 2240
	public class OccludeeRenderers
	{
		// Token: 0x040034BE RID: 13502
		public List<Renderer> renderers = new List<Renderer>();
	}

	// Token: 0x020008C1 RID: 2241
	public class OccludeeEntity
	{
		// Token: 0x040034BF RID: 13503
		public Entity entity;

		// Token: 0x040034C0 RID: 13504
		public Vector3 pos;

		// Token: 0x040034C1 RID: 13505
		public LinkedListNode<OcclusionManager.OcclusionEntry> entry;
	}

	// Token: 0x020008C2 RID: 2242
	public class OccludeeLight
	{
		// Token: 0x040034C2 RID: 13506
		public LightLOD lightLOD;

		// Token: 0x040034C3 RID: 13507
		public Vector3 pos;

		// Token: 0x040034C4 RID: 13508
		public LinkedListNode<OcclusionManager.OcclusionEntry> entry;
	}
}
