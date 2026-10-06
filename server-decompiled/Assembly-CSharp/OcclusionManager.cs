using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x020008C0 RID: 2240
public class OcclusionManager : MonoBehaviour
{
	// Token: 0x170006E5 RID: 1765
	// (get) Token: 0x06004054 RID: 16468 RVA: 0x00193D63 File Offset: 0x00191F63
	// (set) Token: 0x06004055 RID: 16469 RVA: 0x00193D6B File Offset: 0x00191F6B
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

	// Token: 0x06004056 RID: 16470 RVA: 0x00193D8D File Offset: 0x00191F8D
	public static void Load()
	{
		global::UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("Occlusion/Occlusion"));
	}

	// Token: 0x06004057 RID: 16471 RVA: 0x00193D9F File Offset: 0x00191F9F
	public void EnableCulling(bool isCulling)
	{
		this.GPUCullingEnabled = isCulling;
	}

	// Token: 0x06004058 RID: 16472 RVA: 0x00193DA8 File Offset: 0x00191FA8
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

	// Token: 0x06004059 RID: 16473 RVA: 0x00193DC8 File Offset: 0x00191FC8
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

	// Token: 0x0600405A RID: 16474 RVA: 0x00193EF0 File Offset: 0x001920F0
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

	// Token: 0x0600405B RID: 16475 RVA: 0x00193F5C File Offset: 0x0019215C
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

	// Token: 0x0600405C RID: 16476 RVA: 0x00193FA0 File Offset: 0x001921A0
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

	// Token: 0x0600405D RID: 16477 RVA: 0x00193FC8 File Offset: 0x001921C8
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

	// Token: 0x0600405E RID: 16478 RVA: 0x00194014 File Offset: 0x00192214
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

	// Token: 0x0600405F RID: 16479 RVA: 0x0019406C File Offset: 0x0019226C
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

	// Token: 0x06004060 RID: 16480 RVA: 0x001940C4 File Offset: 0x001922C4
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

	// Token: 0x06004061 RID: 16481 RVA: 0x001940EC File Offset: 0x001922EC
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

	// Token: 0x06004062 RID: 16482 RVA: 0x00194140 File Offset: 0x00192340
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

	// Token: 0x06004063 RID: 16483 RVA: 0x001941CC File Offset: 0x001923CC
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

	// Token: 0x06004064 RID: 16484 RVA: 0x001942CC File Offset: 0x001924CC
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

	// Token: 0x06004065 RID: 16485 RVA: 0x001943D4 File Offset: 0x001925D4
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

	// Token: 0x06004066 RID: 16486 RVA: 0x0019443C File Offset: 0x0019263C
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

	// Token: 0x06004067 RID: 16487 RVA: 0x001944E8 File Offset: 0x001926E8
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

	// Token: 0x06004068 RID: 16488 RVA: 0x00194584 File Offset: 0x00192784
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

	// Token: 0x06004069 RID: 16489 RVA: 0x00194600 File Offset: 0x00192800
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

	// Token: 0x0600406A RID: 16490 RVA: 0x001946D8 File Offset: 0x001928D8
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

	// Token: 0x0600406B RID: 16491 RVA: 0x001947CC File Offset: 0x001929CC
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

	// Token: 0x0600406C RID: 16492 RVA: 0x00194AE0 File Offset: 0x00192CE0
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

	// Token: 0x0600406D RID: 16493 RVA: 0x00194BC8 File Offset: 0x00192DC8
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

	// Token: 0x0600406E RID: 16494 RVA: 0x00194CB4 File Offset: 0x00192EB4
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

	// Token: 0x0600406F RID: 16495 RVA: 0x00194F05 File Offset: 0x00193105
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

	// Token: 0x06004070 RID: 16496 RVA: 0x00194F45 File Offset: 0x00193145
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

	// Token: 0x06004071 RID: 16497 RVA: 0x00194F7F File Offset: 0x0019317F
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnResolutionChanged(int _width, int _height)
	{
		this.CreateDepthRT();
	}

	// Token: 0x06004072 RID: 16498 RVA: 0x00194F88 File Offset: 0x00193188
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

	// Token: 0x06004073 RID: 16499 RVA: 0x0019500C File Offset: 0x0019320C
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

	// Token: 0x06004074 RID: 16500 RVA: 0x0019519D File Offset: 0x0019339D
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		if (this.sourceDepthCamera != null && this.depthCopyCmdBuf != null)
		{
			this.sourceDepthCamera.AddCommandBuffer(CameraEvent.AfterForwardOpaque, this.depthCopyCmdBuf);
		}
	}

	// Token: 0x06004075 RID: 16501 RVA: 0x001951C8 File Offset: 0x001933C8
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		if (this.sourceDepthCamera != null && this.depthCopyCmdBuf != null)
		{
			this.sourceDepthCamera.RemoveCommandBuffer(CameraEvent.AfterForwardOpaque, this.depthCopyCmdBuf);
		}
		this.SetRenderersEnabled(true);
	}

	// Token: 0x06004076 RID: 16502 RVA: 0x001951FC File Offset: 0x001933FC
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

	// Token: 0x06004077 RID: 16503 RVA: 0x001952CC File Offset: 0x001934CC
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

	// Token: 0x06004078 RID: 16504 RVA: 0x001953A8 File Offset: 0x001935A8
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

	// Token: 0x06004079 RID: 16505 RVA: 0x00195430 File Offset: 0x00193630
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

	// Token: 0x0600407A RID: 16506 RVA: 0x00195556 File Offset: 0x00193756
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPreCull()
	{
		this.counterBuffer[this.counterBufferCurrentIndex].SetData(this.initialData);
		this.RenderOccludees(this.depthCamera, 11);
	}

	// Token: 0x0600407B RID: 16507 RVA: 0x0019557E File Offset: 0x0019377E
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPostRender()
	{
		Graphics.ClearRandomWriteTargets();
		AsyncGPUReadback.Request(this.counterBuffer[this.counterBufferCurrentIndex], this.onRequestDelegate);
		this.counterBufferCurrentIndex = (this.counterBufferCurrentIndex + 1) % this.counterBuffer.Length;
	}

	// Token: 0x0600407C RID: 16508 RVA: 0x001955B8 File Offset: 0x001937B8
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

	// Token: 0x0600407D RID: 16509 RVA: 0x001955F4 File Offset: 0x001937F4
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

	// Token: 0x0600407E RID: 16510 RVA: 0x001956AC File Offset: 0x001938AC
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

	// Token: 0x0600407F RID: 16511 RVA: 0x00195C80 File Offset: 0x00193E80
	[PublicizedFrom(EAccessModifier.Private)]
	public static void LogOcclusion(string format, params object[] args)
	{
		format = string.Format("{0} OC {1}", GameManager.frameCount, format);
		Log.Warning(format, args);
	}

	// Token: 0x06004080 RID: 16512 RVA: 0x00195CA0 File Offset: 0x00193EA0
	public void ToggleDebugView()
	{
		this.isDebugView = !this.isDebugView;
	}

	// Token: 0x06004081 RID: 16513 RVA: 0x00195CB4 File Offset: 0x00193EB4
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

	// Token: 0x04003461 RID: 13409
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int cDepthRTWidth = 256;

	// Token: 0x04003462 RID: 13410
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cBoundsScale = 1f;

	// Token: 0x04003463 RID: 13411
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cCameraEnableAllAngleCos = 0.94f;

	// Token: 0x04003464 RID: 13412
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool outOfEntriesErrorReported;

	// Token: 0x04003465 RID: 13413
	public static OcclusionManager Instance;

	// Token: 0x04003466 RID: 13414
	public bool isEnabled;

	// Token: 0x04003467 RID: 13415
	public Material depthTestMat;

	// Token: 0x04003468 RID: 13416
	public int visibleEntryCount;

	// Token: 0x04003469 RID: 13417
	public int totalEntryCount;

	// Token: 0x0400346A RID: 13418
	public int freeEntryCount;

	// Token: 0x0400346B RID: 13419
	public int usedEntryCount;

	// Token: 0x0400346C RID: 13420
	public bool forceAllVisible;

	// Token: 0x0400346D RID: 13421
	public bool forceAllHidden;

	// Token: 0x0400346E RID: 13422
	public bool cullChunkEntities;

	// Token: 0x0400346F RID: 13423
	public bool cullChunkLayers;

	// Token: 0x04003470 RID: 13424
	public bool cullDecorations;

	// Token: 0x04003471 RID: 13425
	public bool cullDistantChunks;

	// Token: 0x04003472 RID: 13426
	public bool cullDistantTerrain;

	// Token: 0x04003473 RID: 13427
	public bool cullEntities;

	// Token: 0x04003474 RID: 13428
	public bool cullLights;

	// Token: 0x04003475 RID: 13429
	public bool cullPrefabs;

	// Token: 0x04003476 RID: 13430
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int cMaxUnits = 511;

	// Token: 0x04003477 RID: 13431
	[PublicizedFrom(EAccessModifier.Internal)]
	[NonSerialized]
	public const int cMaxEntries = 4088;

	// Token: 0x04003478 RID: 13432
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Matrix4x4 tinyMatrix;

	// Token: 0x04003479 RID: 13433
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ComputeBuffer[] counterBuffer = new ComputeBuffer[3];

	// Token: 0x0400347A RID: 13434
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int counterBufferCurrentIndex;

	// Token: 0x0400347B RID: 13435
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public uint[] initialData;

	// Token: 0x0400347C RID: 13436
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool hasVisibilityData;

	// Token: 0x0400347D RID: 13437
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public uint[] visibleData;

	// Token: 0x0400347E RID: 13438
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Camera sourceDepthCamera;

	// Token: 0x0400347F RID: 13439
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public CommandBuffer depthCopyCmdBuf;

	// Token: 0x04003480 RID: 13440
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<OcclusionManager.OcclusionEntry> entries = new List<OcclusionManager.OcclusionEntry>();

	// Token: 0x04003481 RID: 13441
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LinkedList<OcclusionManager.OcclusionEntry> freeEntries = new LinkedList<OcclusionManager.OcclusionEntry>();

	// Token: 0x04003482 RID: 13442
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LinkedList<OcclusionManager.OcclusionEntry> usedEntries = new LinkedList<OcclusionManager.OcclusionEntry>();

	// Token: 0x04003483 RID: 13443
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Matrix4x4[]> objectMatrixLists = new List<Matrix4x4[]>();

	// Token: 0x04003484 RID: 13444
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public MaterialPropertyBlock[] materialBlocks;

	// Token: 0x04003485 RID: 13445
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject cubeObj;

	// Token: 0x04003486 RID: 13446
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Mesh cubeMesh;

	// Token: 0x04003487 RID: 13447
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool gpuCullingEnabled;

	// Token: 0x04003488 RID: 13448
	public Camera depthCamera;

	// Token: 0x04003489 RID: 13449
	public Material depthCopyMat;

	// Token: 0x0400348A RID: 13450
	public RenderTexture depthRT;

	// Token: 0x0400348B RID: 13451
	public RenderTexture depthCopyRT;

	// Token: 0x0400348C RID: 13452
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isOcclusionChecking;

	// Token: 0x0400348D RID: 13453
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isCameraChanged;

	// Token: 0x0400348E RID: 13454
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Matrix4x4 areaMatrix = Matrix4x4.identity;

	// Token: 0x0400348F RID: 13455
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Action<AsyncGPUReadbackRequest> onRequestDelegate;

	// Token: 0x04003490 RID: 13456
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int errorFrame;

	// Token: 0x04003491 RID: 13457
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int hugeErrorCount;

	// Token: 0x04003492 RID: 13458
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<OcclusionManager.OccludeeZone> pendingZones = new List<OcclusionManager.OccludeeZone>();

	// Token: 0x04003493 RID: 13459
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly List<Renderer> tempRenderers = new List<Renderer>();

	// Token: 0x04003494 RID: 13460
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Dictionary<int, OcclusionManager.OccludeeEntity> entities = new Dictionary<int, OcclusionManager.OccludeeEntity>();

	// Token: 0x04003495 RID: 13461
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Dictionary<int, OcclusionManager.OccludeeLight> lights = new Dictionary<int, OcclusionManager.OccludeeLight>();

	// Token: 0x04003496 RID: 13462
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 camDirVec;

	// Token: 0x04003497 RID: 13463
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isDebugView;

	// Token: 0x020008C1 RID: 2241
	public struct RenderItem
	{
		// Token: 0x04003498 RID: 13464
		public Renderer renderer;

		// Token: 0x04003499 RID: 13465
		public ShadowCastingMode shadowMode;
	}

	// Token: 0x020008C2 RID: 2242
	public class OcclusionEntry
	{
		// Token: 0x0400349A RID: 13466
		public Renderer[] allRenderers;

		// Token: 0x0400349B RID: 13467
		public OcclusionManager.RenderItem[] renderItems;

		// Token: 0x0400349C RID: 13468
		public int renderItemsUsed;

		// Token: 0x0400349D RID: 13469
		public OcclusionManager.OccludeeLight light;

		// Token: 0x0400349E RID: 13470
		public float cullStartDistSq;

		// Token: 0x0400349F RID: 13471
		public float extentsTotalMax;

		// Token: 0x040034A0 RID: 13472
		public int index;

		// Token: 0x040034A1 RID: 13473
		public int matrixUnitIndex;

		// Token: 0x040034A2 RID: 13474
		public int matrixSubIndex;

		// Token: 0x040034A3 RID: 13475
		public bool isAreaFound;

		// Token: 0x040034A4 RID: 13476
		public Vector3 centerPos;

		// Token: 0x040034A5 RID: 13477
		public Vector3 size;

		// Token: 0x040034A6 RID: 13478
		public bool isForceOn;

		// Token: 0x040034A7 RID: 13479
		public bool isVisible;
	}

	// Token: 0x020008C3 RID: 2243
	public class OccludeeZone
	{
		// Token: 0x06004085 RID: 16517 RVA: 0x00195E20 File Offset: 0x00194020
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

		// Token: 0x06004086 RID: 16518 RVA: 0x00195E44 File Offset: 0x00194044
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

		// Token: 0x06004087 RID: 16519 RVA: 0x00195F40 File Offset: 0x00194140
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

		// Token: 0x040034A8 RID: 13480
		public float extentsTotalMax;

		// Token: 0x040034A9 RID: 13481
		public OcclusionManager.OccludeeLayer[] layers = new OcclusionManager.OccludeeLayer[32];

		// Token: 0x040034AA RID: 13482
		public List<Transform> addTs = new List<Transform>();

		// Token: 0x040034AB RID: 13483
		public List<Transform> removeTs = new List<Transform>();

		// Token: 0x040034AC RID: 13484
		[PublicizedFrom(EAccessModifier.Private)]
		public const int cLayerShift = 3;

		// Token: 0x040034AD RID: 13485
		[PublicizedFrom(EAccessModifier.Private)]
		public const int cLayerH = 8;

		// Token: 0x040034AE RID: 13486
		[PublicizedFrom(EAccessModifier.Private)]
		public const int cLayerCount = 32;
	}

	// Token: 0x020008C4 RID: 2244
	public class OccludeeLayer
	{
		// Token: 0x06004089 RID: 16521 RVA: 0x00196000 File Offset: 0x00194200
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

		// Token: 0x040034AF RID: 13487
		public Dictionary<int, OcclusionManager.OccludeeRenderers> renderers = new Dictionary<int, OcclusionManager.OccludeeRenderers>();

		// Token: 0x040034B0 RID: 13488
		public LinkedListNode<OcclusionManager.OcclusionEntry> node;

		// Token: 0x040034B1 RID: 13489
		public bool isOld;
	}

	// Token: 0x020008C5 RID: 2245
	public class OccludeeRenderers
	{
		// Token: 0x040034B2 RID: 13490
		public List<Renderer> renderers = new List<Renderer>();
	}

	// Token: 0x020008C6 RID: 2246
	public class OccludeeEntity
	{
		// Token: 0x040034B3 RID: 13491
		public Entity entity;

		// Token: 0x040034B4 RID: 13492
		public Vector3 pos;

		// Token: 0x040034B5 RID: 13493
		public LinkedListNode<OcclusionManager.OcclusionEntry> entry;
	}

	// Token: 0x020008C7 RID: 2247
	public class OccludeeLight
	{
		// Token: 0x040034B6 RID: 13494
		public LightLOD lightLOD;

		// Token: 0x040034B7 RID: 13495
		public Vector3 pos;

		// Token: 0x040034B8 RID: 13496
		public LinkedListNode<OcclusionManager.OcclusionEntry> entry;
	}
}
