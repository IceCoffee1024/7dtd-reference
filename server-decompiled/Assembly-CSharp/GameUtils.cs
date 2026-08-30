using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;
using Epic.OnlineServices.AntiCheatCommon;
using GamePath;
using Platform;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x020011EC RID: 4588
public class GameUtils
{
	// Token: 0x06009256 RID: 37462 RVA: 0x003752C0 File Offset: 0x003734C0
	public static bool FindMasterBlockForEntityModelBlock(World _world, Vector3 _dirNormalized, string _phsxTag, Vector3 _hitPointPos, Transform _hitTransform, WorldRayHitInfo _hitInfo)
	{
		ChunkCluster chunkCache = _world.ChunkCache;
		if (chunkCache == null)
		{
			return false;
		}
		Vector3 vector = chunkCache.ToLocalPosition(_hitPointPos);
		Vector3 vector2 = chunkCache.ToLocalVector(_dirNormalized);
		Vector3i vector3i = World.worldToBlockPos(vector);
		Transform transform = RootTransformRefParent.FindRoot(_hitTransform);
		int num = World.toBlockXZ(vector3i.x);
		int num2 = World.toBlockXZ(vector3i.z);
		int num3 = World.toChunkXZ(vector3i.x);
		int num4 = World.toChunkXZ(vector3i.z);
		HitInfoDetails.PropData from = HitInfoDetails.PropData.GetFrom(chunkCache, transform);
		_hitInfo.hit.propData = from;
		_hitInfo.fmcHit.propData = from;
		BlockEntityData blockEntityData;
		if (GameUtils.checkChunk(chunkCache, num3, num4, transform, vector, vector2, _hitInfo, out blockEntityData))
		{
			_hitInfo.hit.pos = _hitPointPos;
			_hitInfo.lastBlockPos = World.worldToBlockPos(_hitInfo.hit.pos);
			if (!blockEntityData.blockValue.Block.isMultiBlock || blockEntityData.blockValue.Block.multiBlockPos.ContainsPos(_world, blockEntityData.pos, blockEntityData.blockValue, _hitInfo.lastBlockPos))
			{
				BlockValue blockValue;
				_hitInfo.lastBlockPos = Voxel.GoBackOnVoxels(chunkCache, new Ray(chunkCache.ToLocalPosition(_hitPointPos + vector2 * 0.01f), -vector2), out blockValue);
			}
			return true;
		}
		if (GameUtils.checkChunk(chunkCache, (num < 8) ? (num3 - 1) : (num3 + 1), num4, transform, vector, vector2, _hitInfo, out blockEntityData))
		{
			_hitInfo.hit.pos = _hitPointPos;
			_hitInfo.lastBlockPos = World.worldToBlockPos(_hitInfo.hit.pos);
			if (!blockEntityData.blockValue.Block.isMultiBlock || blockEntityData.blockValue.Block.multiBlockPos.ContainsPos(_world, blockEntityData.pos, blockEntityData.blockValue, _hitInfo.lastBlockPos))
			{
				BlockValue blockValue;
				_hitInfo.lastBlockPos = Voxel.GoBackOnVoxels(chunkCache, new Ray(chunkCache.ToLocalPosition(_hitPointPos + vector2 * 0.01f), -vector2), out blockValue);
			}
			return true;
		}
		if (GameUtils.checkChunk(chunkCache, num3, (num2 < 8) ? (num4 - 1) : (num4 + 1), transform, vector, vector2, _hitInfo, out blockEntityData))
		{
			_hitInfo.hit.pos = _hitPointPos;
			_hitInfo.lastBlockPos = World.worldToBlockPos(_hitInfo.hit.pos);
			if (!blockEntityData.blockValue.Block.isMultiBlock || blockEntityData.blockValue.Block.multiBlockPos.ContainsPos(_world, blockEntityData.pos, blockEntityData.blockValue, _hitInfo.lastBlockPos))
			{
				BlockValue blockValue;
				_hitInfo.lastBlockPos = Voxel.GoBackOnVoxels(chunkCache, new Ray(chunkCache.ToLocalPosition(_hitPointPos + vector2 * 0.01f), -vector2), out blockValue);
			}
			return true;
		}
		if (GameUtils.checkChunk(chunkCache, (num < 8) ? (num3 - 1) : (num3 + 1), (num2 < 8) ? (num4 - 1) : (num4 + 1), transform, vector, vector2, _hitInfo, out blockEntityData))
		{
			_hitInfo.hit.pos = _hitPointPos;
			_hitInfo.lastBlockPos = World.worldToBlockPos(_hitInfo.hit.pos);
			if (!blockEntityData.blockValue.Block.isMultiBlock || blockEntityData.blockValue.Block.multiBlockPos.ContainsPos(_world, blockEntityData.pos, blockEntityData.blockValue, _hitInfo.lastBlockPos))
			{
				BlockValue blockValue;
				_hitInfo.lastBlockPos = Voxel.GoBackOnVoxels(chunkCache, new Ray(chunkCache.ToLocalPosition(_hitPointPos + vector2 * 0.01f), -vector2), out blockValue);
			}
			return true;
		}
		return false;
	}

	// Token: 0x06009257 RID: 37463 RVA: 0x00375640 File Offset: 0x00373840
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool checkChunk(ChunkCluster cc, int cX, int cZ, Transform parentTransform, Vector3 localHitPos, Vector3 localDirNormalized, WorldRayHitInfo _hitInfo, out BlockEntityData _ebcd)
	{
		_ebcd = null;
		IChunk chunkSync = cc.GetChunkSync(cX, cZ);
		if (chunkSync == null)
		{
			return false;
		}
		BlockEntityData blockEntity;
		_ebcd = (blockEntity = chunkSync.GetBlockEntity(parentTransform));
		if (blockEntity != null)
		{
			_hitInfo.hit.blockPos = _ebcd.pos;
			_hitInfo.hit.voxelData = HitInfoDetails.VoxelData.GetFrom(chunkSync, World.toBlockXZ(_ebcd.pos.x), World.toBlockY(_ebcd.pos.y), World.toBlockXZ(_ebcd.pos.z));
			Ray ray = new Ray(localHitPos, -1f * localDirNormalized);
			int num = 0;
			do
			{
				Vector3 vector;
				BlockFace blockFace;
				_hitInfo.lastBlockPos = Voxel.OneVoxelStep(Vector3i.FromVector3Rounded(ray.origin), ray.origin, ray.direction, out vector, out blockFace);
				ray.origin = vector + localDirNormalized * 0.001f;
			}
			while (!cc.GetBlock(_hitInfo.lastBlockPos).isair && num++ < 3);
			return true;
		}
		return false;
	}

	// Token: 0x06009258 RID: 37464 RVA: 0x0037574C File Offset: 0x0037394C
	public static void EnableRagdoll(GameObject _model, bool _bEnable, bool _bUseGravity)
	{
		foreach (Rigidbody rigidbody in _model.GetComponentsInChildren<Rigidbody>())
		{
			rigidbody.isKinematic = !_bEnable;
			rigidbody.useGravity = _bUseGravity;
		}
		Collider[] componentsInChildren2 = _model.GetComponentsInChildren<Collider>();
		for (int i = 0; i < componentsInChildren2.Length; i++)
		{
			componentsInChildren2[i].enabled = _bEnable;
		}
	}

	// Token: 0x06009259 RID: 37465 RVA: 0x003757A0 File Offset: 0x003739A0
	public static Entity GetHitRootEntity(string _tag, Transform _hitTransform)
	{
		if (_tag.StartsWith("E_BP_"))
		{
			RootTransformRefEntity rootTransformRefEntity;
			if (_hitTransform.TryGetComponent<RootTransformRefEntity>(out rootTransformRefEntity))
			{
				if (rootTransformRefEntity.RootTransform)
				{
					return rootTransformRefEntity.RootTransform.GetComponent<Entity>();
				}
			}
			else
			{
				Transform transform = RootTransformRefEntity.FindEntityUpwards(_hitTransform);
				if (transform)
				{
					return transform.GetComponent<Entity>();
				}
			}
		}
		else if (_tag.StartsWith("E_Vehicle"))
		{
			return CollisionCallForward.FindEntity(_hitTransform);
		}
		return null;
	}

	// Token: 0x0600925A RID: 37466 RVA: 0x00375808 File Offset: 0x00373A08
	public static Transform GetHitRootTransform(string _tag, Transform _hitTransform)
	{
		if (!_tag.StartsWith("E_BP_"))
		{
			if (_tag.Equals("E_Vehicle"))
			{
				Entity entity = CollisionCallForward.FindEntity(_hitTransform);
				if (entity)
				{
					return entity.transform;
				}
			}
			return _hitTransform;
		}
		RootTransformRefEntity rootTransformRefEntity;
		if (_hitTransform.TryGetComponent<RootTransformRefEntity>(out rootTransformRefEntity))
		{
			return rootTransformRefEntity.RootTransform;
		}
		return RootTransformRefEntity.FindEntityUpwards(_hitTransform);
	}

	// Token: 0x0600925B RID: 37467 RVA: 0x0037585E File Offset: 0x00373A5E
	public static string GetTransformPath(Transform _t)
	{
		if (!_t)
		{
			return "null";
		}
		if (!_t.parent)
		{
			return _t.name;
		}
		return GameUtils.GetTransformPath(_t.parent) + "/" + _t.name;
	}

	// Token: 0x0600925C RID: 37468 RVA: 0x003758A0 File Offset: 0x00373AA0
	public static string GetChildTransformPath(Transform _parent, Transform _child)
	{
		if (_child.parent == null)
		{
			throw new Exception(string.Concat(new string[] { "GetChildTransformPath: '", _child.name, "' is a root object and not in the path underneath '", _parent.name, "'" }));
		}
		if (_child.parent == _parent)
		{
			return _child.name;
		}
		return GameUtils.GetChildTransformPath(_parent, _child.parent) + "/" + _child.name;
	}

	// Token: 0x0600925D RID: 37469 RVA: 0x00375928 File Offset: 0x00373B28
	public static void FindTagInChilds(Transform _parent, string _tag, List<Transform> _list)
	{
		int childCount = _parent.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = _parent.GetChild(i);
			if (child.CompareTag(_tag))
			{
				_list.Add(child);
			}
			GameUtils.FindTagInChilds(child, _tag, _list);
		}
	}

	// Token: 0x0600925E RID: 37470 RVA: 0x00375968 File Offset: 0x00373B68
	public static Transform FindTagInChilds(Transform _parent, string _tag)
	{
		int childCount = _parent.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = _parent.GetChild(i);
			if (child.CompareTag(_tag))
			{
				return child;
			}
		}
		for (int j = 0; j < childCount; j++)
		{
			Transform transform = GameUtils.FindTagInChilds(_parent.GetChild(j), _tag);
			if (transform != null)
			{
				return transform;
			}
		}
		return null;
	}

	// Token: 0x0600925F RID: 37471 RVA: 0x003759C4 File Offset: 0x00373BC4
	public static Transform FindTagInDirectChilds(Transform _parent, string _tag)
	{
		int childCount = _parent.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = _parent.GetChild(i);
			if (child.CompareTag(_tag))
			{
				return child;
			}
		}
		return null;
	}

	// Token: 0x06009260 RID: 37472 RVA: 0x003759F8 File Offset: 0x00373BF8
	public static Transform FindChildWithPartialName(Transform root, params string[] names)
	{
		foreach (string text in names)
		{
			if (root.name.ContainsCaseInsensitive(text))
			{
				return root;
			}
			for (int j = 0; j < root.childCount; j++)
			{
				Transform child = root.GetChild(j);
				if (child.name.ContainsCaseInsensitive(text))
				{
					return child;
				}
			}
		}
		return null;
	}

	// Token: 0x06009261 RID: 37473 RVA: 0x00375A58 File Offset: 0x00373C58
	public static void FindDeepChildWithPartialName(Transform root, string name, ref List<Transform> found)
	{
		if (root.name.ContainsCaseInsensitive(name))
		{
			found.Add(root);
		}
		for (int i = 0; i < root.childCount; i++)
		{
			GameUtils.FindDeepChildWithPartialName(root.GetChild(i), name, ref found);
		}
	}

	// Token: 0x06009262 RID: 37474 RVA: 0x00375A9A File Offset: 0x00373C9A
	[Conditional("UNITY_EDITOR")]
	public static void HideObjectInEditor(GameObject _obj)
	{
		global::UnityEngine.Object.DontDestroyOnLoad(_obj);
	}

	// Token: 0x06009263 RID: 37475 RVA: 0x00375AA4 File Offset: 0x00373CA4
	public static bool IsColliderWithinBlock(Vector3i blockPosition, BlockValue blockValue)
	{
		int num = 3899400;
		Quaternion rotation = blockValue.Block.shape.GetRotation(blockValue);
		Bounds blockPlacementBounds = GameUtils.GetBlockPlacementBounds(blockValue.Block);
		Vector3 vector = blockPlacementBounds.size;
		Vector3 vector2 = World.blockToTransformPos(blockPosition) - Origin.position + new Vector3(0f, 0.5f, 0f);
		if (blockPlacementBounds.center != Vector3.zero)
		{
			vector2 += rotation * blockPlacementBounds.center;
		}
		if (blockValue.Block.isOversized)
		{
			num |= 1082130432;
			vector -= new Vector3(0.1f, 0.1f, 0.1f);
		}
		else if (blockValue.Block.shape.IsTerrain())
		{
			vector2 -= new Vector3(0f, 0.25f, 0f);
		}
		Vector3 vector3 = vector * 0.5f;
		int num2 = Physics.OverlapBoxNonAlloc(vector2, vector3, GameUtils.overlapBoxHits, rotation, num);
		if (num2 == GameUtils.overlapBoxHits.Length)
		{
			global::UnityEngine.Debug.LogError(string.Format("OverlapBox reached maximum hit count ({0}); overlapBoxHits array size may be insufficient.", num2));
		}
		for (int i = 0; i < num2; i++)
		{
			if (blockValue.Block.isOversized)
			{
				if (!GameUtils.overlapBoxHits[i].CompareTag("T_Mesh"))
				{
					return true;
				}
			}
			else
			{
				if (!GameUtils.IsBlockOrTerrain(GameUtils.overlapBoxHits[i]) && !GameUtils.overlapBoxHits[i].CompareTag("Item"))
				{
					return true;
				}
				if (GameUtils.overlapBoxHits[i].CompareTag("T_Block"))
				{
					Transform transform = RootTransformRefParent.FindRoot(GameUtils.overlapBoxHits[i].transform);
					BlockEntityData blockEntityData;
					if (GameUtils.TryFindEntityData(blockPosition, transform, out blockEntityData) && blockEntityData.blockValue.Block.isOversized)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	// Token: 0x06009264 RID: 37476 RVA: 0x00375C80 File Offset: 0x00373E80
	public static Vector3 GetMultiBlockBoundsOffset(Vector3 multiBlockDim)
	{
		return new Vector3((multiBlockDim.x % 2f == 0f) ? (-0.5f) : 0f, multiBlockDim.y / 2f - 0.5f, (multiBlockDim.z % 2f == 0f) ? (-0.5f) : 0f);
	}

	// Token: 0x06009265 RID: 37477 RVA: 0x00375CE4 File Offset: 0x00373EE4
	public static Bounds GetBlockPlacementBounds(Block block)
	{
		if (block.isOversized)
		{
			return block.oversizedBounds;
		}
		if (block.isMultiBlock)
		{
			Vector3 vector = block.multiBlockPos.dim;
			return new Bounds(GameUtils.GetMultiBlockBoundsOffset(vector), vector);
		}
		return new Bounds(Vector3.zero, Vector3.one);
	}

	// Token: 0x06009266 RID: 37478 RVA: 0x00375D38 File Offset: 0x00373F38
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool TryFindEntityData(Vector3i entityWorldHitPosition, Transform entityParentTransform, out BlockEntityData ebcd)
	{
		int num = World.toBlockXZ(entityWorldHitPosition.x);
		int num2 = World.toBlockXZ(entityWorldHitPosition.z);
		int num3 = World.toChunkXZ(entityWorldHitPosition.x);
		int num4 = World.toChunkXZ(entityWorldHitPosition.z);
		int num5 = ((num < 8) ? (-1) : 1);
		int num6 = ((num2 < 8) ? (-1) : 1);
		ChunkCluster chunkCache = GameManager.Instance.World.ChunkCache;
		if (chunkCache != null)
		{
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < 2; j++)
				{
					Chunk chunkSync = chunkCache.GetChunkSync(num3 + j * num5, num4 + i * num6);
					ebcd = ((chunkSync != null) ? chunkSync.GetBlockEntity(entityParentTransform) : null);
					if (ebcd != null)
					{
						return true;
					}
				}
			}
		}
		global::UnityEngine.Debug.LogWarning(string.Format("Failed to find entity data for Transform \"{0}\" with hit position \"{1}\"", entityParentTransform.name, entityWorldHitPosition), entityParentTransform);
		ebcd = null;
		return false;
	}

	// Token: 0x06009267 RID: 37479 RVA: 0x000027FC File Offset: 0x000009FC
	public static void DebugDrawPathFromEntity(Entity _e, PathEntity _path, Color _color)
	{
	}

	// Token: 0x06009268 RID: 37480 RVA: 0x00375E08 File Offset: 0x00374008
	public static void CreateEmptyFlatLevel(string _worldName, PathAbstractions.AbstractedLocation _worldLocation, int _worldSize, int _terrainHeight = 60)
	{
		SdDirectory.CreateDirectory(_worldLocation.FullPath);
		World world = new World();
		WorldState worldState = new WorldState();
		worldState.SetFrom(world, EnumChunkProviderId.ChunkDataDriven);
		worldState.ResetDynamicData();
		worldState.Save(_worldLocation.FullPath + "/main.ttw");
		GameUtils.CreateWorldFilesForFlatLevel(_worldName, _worldSize, _worldLocation, _terrainHeight);
	}

	// Token: 0x06009269 RID: 37481 RVA: 0x00375E5C File Offset: 0x0037405C
	[PublicizedFrom(EAccessModifier.Private)]
	public static void CreateWorldFilesForFlatLevel(string _worldName, int _worldSize, PathAbstractions.AbstractedLocation _worldLocation, int _terrainHeight)
	{
		int num = _worldSize * 2;
		byte[] array = new byte[num];
		for (int i = 0; i < _worldSize; i++)
		{
			array[2 * i] = 0;
			array[2 * i + 1] = 60;
		}
		using (BufferedStream bufferedStream = new BufferedStream(SdFile.OpenWrite(_worldLocation.FullPath + "/dtm.raw")))
		{
			for (int j = 0; j < _worldSize; j++)
			{
				bufferedStream.Write(array, 0, num);
			}
		}
		new GameUtils.WorldInfo(_worldName, "Empty World", new string[] { "Survival", "Creative" }, new Vector2i(_worldSize, _worldSize), 1, false, false, Constants.cVersionInformation, null).Save(_worldLocation);
		new SpawnPointManager(true)
		{
			spawnPointList = 
			{
				new SpawnPoint(new Vector3i(0, _terrainHeight + 1, 0))
			}
		}.Save(_worldLocation.FullPath);
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.CreateXmlDeclaration();
		xmlDocument.AddXmlElement("WaterSources");
		xmlDocument.SdSave(_worldLocation.FullPath + "/water_info.xml");
		GameUtils.CreateForestBiomeMap(_worldSize, _worldLocation, BiomeDefinition.BiomeType.PineForest);
		XmlDocument xmlDocument2 = new XmlDocument();
		xmlDocument2.CreateXmlDeclaration();
		xmlDocument2.AddXmlElement("prefabs");
		xmlDocument2.SdSave(_worldLocation.FullPath + "/prefabs.xml");
		GameUtils.CreateSimpleRadiationMap(_worldSize, _worldLocation, (_worldSize <= 1024) ? 128 : ((_worldSize <= 2048) ? 256 : 512), 16);
		GameUtils.CreateEmptySplatMap(_worldSize, _worldLocation);
	}

	// Token: 0x0600926A RID: 37482 RVA: 0x00375FDC File Offset: 0x003741DC
	[PublicizedFrom(EAccessModifier.Private)]
	public static void CreateSimpleRadiationMap(int _worldSize, PathAbstractions.AbstractedLocation _worldLocation, int _radiationBorderSize = 128, int _downScale = 16)
	{
		int num = _worldSize / _downScale;
		Color red = Color.red;
		Color32 color = new Color32(0, 0, 0, byte.MaxValue);
		Texture2D texture2D = new Texture2D(num, num, TextureFormat.RGBA32, false);
		texture2D.FillTexture(red, false, false);
		int num2 = _radiationBorderSize / _downScale;
		int num3 = num - 2 * num2;
		Color32[] array = new Color32[num3];
		for (int i = 0; i < num3; i++)
		{
			array[i] = color;
		}
		for (int j = num2; j < num - num2; j++)
		{
			texture2D.SetPixels32(num2, j, num3, 1, array);
		}
		texture2D.Apply();
		SdFile.WriteAllBytes(_worldLocation.FullPath + "/radiation.png", texture2D.EncodeToPNG());
	}

	// Token: 0x0600926B RID: 37483 RVA: 0x0037608C File Offset: 0x0037428C
	[PublicizedFrom(EAccessModifier.Private)]
	public static void CreateEmptySplatMap(int _worldSize, PathAbstractions.AbstractedLocation _worldLocation)
	{
		Color32 color = new Color32(0, 0, 0, 0);
		Texture2D texture2D = new Texture2D(_worldSize, _worldSize, TextureFormat.RGBA32, false);
		texture2D.FillTexture(color, false, false);
		texture2D.Apply();
		byte[] array = texture2D.EncodeToPNG();
		MicroStopwatch microStopwatch = new MicroStopwatch(true);
		SdFile.WriteAllBytes(_worldLocation.FullPath + "/splat1.png", array);
		SdFile.WriteAllBytes(_worldLocation.FullPath + "/splat2.png", array);
		SdFile.WriteAllBytes(_worldLocation.FullPath + "/splat3.png", array);
		Log.Out(string.Format("Write tex took {0} ms", microStopwatch.ElapsedMilliseconds));
	}

	// Token: 0x0600926C RID: 37484 RVA: 0x00376130 File Offset: 0x00374330
	[PublicizedFrom(EAccessModifier.Private)]
	public static void CreateForestBiomeMap(int _worldSize, PathAbstractions.AbstractedLocation _worldLocation, BiomeDefinition.BiomeType _biome = BiomeDefinition.BiomeType.PineForest)
	{
		Color32 color = GameUtils.UIntToColor(BiomeDefinition.GetBiomeColor(_biome), false);
		Texture2D texture2D = new Texture2D(_worldSize, _worldSize, TextureFormat.RGBA32, false);
		texture2D.FillTexture(color, false, false);
		texture2D.Apply();
		MicroStopwatch microStopwatch = new MicroStopwatch(true);
		SdFile.WriteAllBytes(_worldLocation.FullPath + "/biomes.png", texture2D.EncodeToPNG());
		Log.Out(string.Format("Write tex took {0} ms", microStopwatch.ElapsedMilliseconds));
	}

	// Token: 0x0600926D RID: 37485 RVA: 0x003761A8 File Offset: 0x003743A8
	public static void DeleteWorld(PathAbstractions.AbstractedLocation _worldLocation)
	{
		if (string.IsNullOrEmpty(_worldLocation.Name))
		{
			return;
		}
		SdDirectory.Delete(_worldLocation.FullPath, true);
		foreach (string text in GameIO.GetSaveGameDirs(_worldLocation.Name))
		{
			if (SdDirectory.Exists(text))
			{
				SdDirectory.Delete(text, true);
			}
		}
		PathAbstractions.WorldsSearchPaths.InvalidateCache();
	}

	// Token: 0x0600926E RID: 37486 RVA: 0x00376228 File Offset: 0x00374428
	public static int WorldTimeToDays(ulong _worldTime)
	{
		return (int)(_worldTime / 24000UL + 1UL);
	}

	// Token: 0x0600926F RID: 37487 RVA: 0x00376236 File Offset: 0x00374436
	public static int WorldTimeToHours(ulong _worldTime)
	{
		return (int)(_worldTime / 1000UL) % 24;
	}

	// Token: 0x06009270 RID: 37488 RVA: 0x00376244 File Offset: 0x00374444
	public static int WorldTimeToMinutes(ulong _worldTime)
	{
		return (int)(_worldTime / 1000.0 * 60.0) % 60;
	}

	// Token: 0x06009271 RID: 37489 RVA: 0x00376261 File Offset: 0x00374461
	public static float WorldTimeToTotalSeconds(float _worldTime)
	{
		return _worldTime * 3.6f;
	}

	// Token: 0x06009272 RID: 37490 RVA: 0x0037626A File Offset: 0x0037446A
	public static uint WorldTimeToTotalMinutes(ulong _worldTime)
	{
		return (uint)(_worldTime * 0.06);
	}

	// Token: 0x06009273 RID: 37491 RVA: 0x0037627A File Offset: 0x0037447A
	public static int WorldTimeToTotalHours(ulong _worldTime)
	{
		return (int)(_worldTime / 1000UL);
	}

	// Token: 0x06009274 RID: 37492 RVA: 0x00376285 File Offset: 0x00374485
	public static ulong TotalMinutesToWorldTime(uint _totalMinutes)
	{
		return (ulong)(_totalMinutes / 0.06);
	}

	// Token: 0x06009275 RID: 37493 RVA: 0x00376298 File Offset: 0x00374498
	[return: TupleElementNames(new string[] { "Days", "Hours", "Minutes" })]
	public static ValueTuple<int, int, int> WorldTimeToElements(ulong _worldTime)
	{
		int num = (int)(_worldTime / 24000UL + 1UL);
		int num2 = (int)(_worldTime / 1000UL) % 24;
		int num3 = (int)(_worldTime * 0.06) % 60;
		return new ValueTuple<int, int, int>(num, num2, num3);
	}

	// Token: 0x06009276 RID: 37494 RVA: 0x003762D8 File Offset: 0x003744D8
	public static string WorldTimeToString(ulong _worldTime)
	{
		ValueTuple<int, int, int> valueTuple = GameUtils.WorldTimeToElements(_worldTime);
		int item = valueTuple.Item1;
		int item2 = valueTuple.Item2;
		int item3 = valueTuple.Item3;
		return string.Format("{0} {1:D2}:{2:D2}", item, item2, item3);
	}

	// Token: 0x06009277 RID: 37495 RVA: 0x0037631C File Offset: 0x0037451C
	public static string WorldTimeToHourMinutesString(ulong _worldTime)
	{
		ValueTuple<int, int, int> valueTuple = GameUtils.WorldTimeToElements(_worldTime);
		int item = valueTuple.Item2;
		int item2 = valueTuple.Item3;
		return string.Format("{0:D2}:{1:D2}", item, item2);
	}

	// Token: 0x06009278 RID: 37496 RVA: 0x00376354 File Offset: 0x00374554
	public static string WorldTimeDeltaToString(ulong _worldTime)
	{
		ValueTuple<int, int, int> valueTuple = GameUtils.WorldTimeToElements(_worldTime);
		int item = valueTuple.Item1;
		int item2 = valueTuple.Item2;
		int item3 = valueTuple.Item3;
		return string.Format("{0} {1:D2}:{2:D2}", item - 1, item2, item3);
	}

	// Token: 0x06009279 RID: 37497 RVA: 0x00376399 File Offset: 0x00374599
	public static ulong DayTimeToWorldTime(int _day, int _hours, int _minutes)
	{
		if (_day < 1)
		{
			return 0UL;
		}
		return (ulong)((long)(_day - 1) * 24000L + (long)(_hours * 1000) + (long)(_minutes * 1000 / 60));
	}

	// Token: 0x0600927A RID: 37498 RVA: 0x003763C2 File Offset: 0x003745C2
	public static ulong DaysToWorldTime(int _day)
	{
		if (_day < 1)
		{
			return 0UL;
		}
		return (ulong)(((long)_day - 1L) * 24000L);
	}

	// Token: 0x0600927B RID: 37499 RVA: 0x003763D7 File Offset: 0x003745D7
	public static ulong DaysToWorldTimeMidnight(int _day)
	{
		return GameUtils.DaysToWorldTime(_day) + 16000UL;
	}

	// Token: 0x0600927C RID: 37500 RVA: 0x003763E8 File Offset: 0x003745E8
	[return: TupleElementNames(new string[] { "duskHour", "dawnHour" })]
	public static ValueTuple<int, int> CalcDuskDawnHours(int _dayLightLength)
	{
		ValueTuple<int, int> valueTuple;
		if (_dayLightLength == 0 || _dayLightLength == 24)
		{
			valueTuple.Item2 = 4;
			valueTuple.Item1 = 22;
		}
		else
		{
			valueTuple.Item1 = 22;
			if (_dayLightLength > 22)
			{
				valueTuple.Item1 = Mathf.Clamp(_dayLightLength, 0, 23);
			}
			if (_dayLightLength < 18)
			{
				valueTuple.Item1 = 12 + _dayLightLength / 2;
			}
			valueTuple.Item2 = Mathf.Clamp(valueTuple.Item1 - _dayLightLength, 0, 23);
		}
		return valueTuple;
	}

	// Token: 0x0600927D RID: 37501 RVA: 0x00376458 File Offset: 0x00374658
	public static bool IsBloodMoonTime(ulong _worldTime, [TupleElementNames(new string[] { "duskHour", "dawnHour" })] ValueTuple<int, int> _duskDawnTimes, int _bmDay)
	{
		ValueTuple<int, int, int> valueTuple = GameUtils.WorldTimeToElements(_worldTime);
		int item = valueTuple.Item1;
		int item2 = valueTuple.Item2;
		return GameUtils.IsBloodMoonTime(_duskDawnTimes, item2, _bmDay, item);
	}

	// Token: 0x0600927E RID: 37502 RVA: 0x00376481 File Offset: 0x00374681
	public static bool IsBloodMoonTime([TupleElementNames(new string[] { "duskHour", "dawnHour" })] ValueTuple<int, int> _duskDawnTimes, int _hour, int _bmDay, int _day)
	{
		if (_day == _bmDay)
		{
			if (_hour >= _duskDawnTimes.Item1)
			{
				return true;
			}
		}
		else if (_day > 1 && _day == _bmDay + 1 && _hour < _duskDawnTimes.Item2)
		{
			return true;
		}
		return false;
	}

	// Token: 0x0600927F RID: 37503 RVA: 0x003764A8 File Offset: 0x003746A8
	public static List<string> GetWorldFilesToTransmitToClient(string _worldFolder)
	{
		string[] files = SdDirectory.GetFiles(_worldFolder);
		for (int i = 0; i < files.Length; i++)
		{
			files[i] = GameIO.GetFilenameFromPath(files[i]);
		}
		return GameUtils.GetWorldFilesToTransmitToClient(files);
	}

	// Token: 0x06009280 RID: 37504 RVA: 0x003764DC File Offset: 0x003746DC
	public static List<string> GetWorldFilesToTransmitToClient(ICollection<string> _files)
	{
		HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		foreach (string text in _files)
		{
			hashSet.Add(GameIO.GetFilenameFromPathWithoutExtension(text));
		}
		List<string> list = new List<string>();
		foreach (string text2 in _files)
		{
			string filenameFromPathWithoutExtension = GameIO.GetFilenameFromPathWithoutExtension(text2);
			if (true & !hashSet.Contains(filenameFromPathWithoutExtension + "_processed") & !text2.ContainsCaseInsensitive("GenerationInfo") & !text2.EndsWith(".bak", StringComparison.OrdinalIgnoreCase) & !text2.ContainsCaseInsensitive("Version.txt") & !text2.ContainsCaseInsensitive("checksums.txt"))
			{
				list.Add(text2);
			}
		}
		return list;
	}

	// Token: 0x06009281 RID: 37505 RVA: 0x003765D8 File Offset: 0x003747D8
	public static void DebugOutputGamePrefs(GameUtils.OutputDelegate _output)
	{
		SortedList<string, string> sortedList = new SortedList<string, string>();
		for (int num = 0; num != 315; num++)
		{
			string text = ((EnumGamePrefs)num).ToStringCached<EnumGamePrefs>();
			if (!text.Contains("Password") && text != "ServerHistoryCache")
			{
				SortedList<string, string> sortedList2 = sortedList;
				string text2 = text;
				string text3 = text;
				string text4 = " = ";
				object @object = GamePrefs.GetObject((EnumGamePrefs)num);
				sortedList2.Add(text2, text3 + text4 + ((@object != null) ? @object.ToString() : null));
			}
		}
		foreach (string text5 in sortedList.Keys)
		{
			_output(sortedList[text5]);
		}
	}

	// Token: 0x06009282 RID: 37506 RVA: 0x00376688 File Offset: 0x00374888
	public static void DebugOutputGameStats(GameUtils.OutputDelegate _output)
	{
		SortedList<string, string> sortedList = new SortedList<string, string>();
		for (int num = 0; num != 81; num++)
		{
			string text = ((EnumGameStats)num).ToStringCached<EnumGameStats>();
			SortedList<string, string> sortedList2 = sortedList;
			string text2 = text;
			string text3 = text;
			string text4 = " = ";
			object @object = GameStats.GetObject((EnumGameStats)num);
			sortedList2.Add(text2, text3 + text4 + ((@object != null) ? @object.ToString() : null));
		}
		foreach (string text5 in sortedList.Keys)
		{
			_output(sortedList[text5]);
		}
	}

	// Token: 0x06009283 RID: 37507 RVA: 0x0037671C File Offset: 0x0037491C
	public static void KickPlayerForClientInfo(ClientInfo _cInfo, GameUtils.KickPlayerData _kickData)
	{
		_cInfo.SendPackage(NetPackageManager.GetPackage<NetPackagePlayerDenied>().Setup(_kickData));
		string text = _cInfo.ToString();
		string text2 = "Kicking player (";
		GameUtils.KickPlayerData kickPlayerData = _kickData;
		Log.Out(text2 + kickPlayerData.ToString() + "): " + text);
		ThreadManager.StartCoroutine(GameUtils.disconnectLater(0.5f, _cInfo));
	}

	// Token: 0x06009284 RID: 37508 RVA: 0x00376776 File Offset: 0x00374976
	[PublicizedFrom(EAccessModifier.Protected)]
	public static IEnumerator disconnectLater(float _delayInSec, ClientInfo _clientInfo)
	{
		_clientInfo.disconnecting = true;
		yield return new WaitForSecondsRealtime(_delayInSec);
		SingletonMonoBehaviour<ConnectionManager>.Instance.DisconnectClient(_clientInfo, false, false);
		yield break;
	}

	// Token: 0x06009285 RID: 37509 RVA: 0x0037678C File Offset: 0x0037498C
	public static void ForceDisconnect()
	{
		GameUtils.ForceDisconnect(new GameUtils.KickPlayerData(GameUtils.EKickReason.InternalNetConnectionError, 0, default(DateTime), ""));
	}

	// Token: 0x06009286 RID: 37510 RVA: 0x003767B4 File Offset: 0x003749B4
	public static void ForceDisconnect(GameUtils.KickPlayerData _kickData)
	{
		ThreadManager.StartCoroutine(GameUtils.ForceDisconnectRoutine(0.5f, _kickData));
	}

	// Token: 0x06009287 RID: 37511 RVA: 0x003767C7 File Offset: 0x003749C7
	[PublicizedFrom(EAccessModifier.Protected)]
	public static IEnumerator ForceDisconnectRoutine(float delay, GameUtils.KickPlayerData kickData)
	{
		yield return new WaitForSeconds(delay);
		GameManager.Instance.Disconnect();
		if (!GameManager.IsDedicatedServer)
		{
			yield return new WaitForSeconds(0.5f);
			GameManager.Instance.ShowMessagePlayerDenied(kickData);
		}
		yield break;
	}

	// Token: 0x06009288 RID: 37512 RVA: 0x003767E0 File Offset: 0x003749E0
	public static void WriteItemStack(BinaryWriter _bw, IList<ItemStack> _itemStack)
	{
		_bw.Write((ushort)_itemStack.Count);
		for (int i = 0; i < _itemStack.Count; i++)
		{
			_itemStack[i].Write(_bw);
		}
	}

	// Token: 0x06009289 RID: 37513 RVA: 0x00376818 File Offset: 0x00374A18
	public static ItemStack[] ReadItemStackOld(BinaryReader _br)
	{
		int num = (int)_br.ReadUInt16();
		ItemStack[] array = ItemStack.CreateArray(num);
		for (int i = 0; i < num; i++)
		{
			array[i].ReadOld(_br);
			if (ItemClass.GetForId(array[i].itemValue.type) == null)
			{
				array[i] = ItemStack.Empty;
			}
		}
		return array;
	}

	// Token: 0x0600928A RID: 37514 RVA: 0x00376868 File Offset: 0x00374A68
	public static ItemStack[] ReadItemStack(BinaryReader _br)
	{
		int num = (int)_br.ReadUInt16();
		ItemStack[] array = ItemStack.CreateArray(num);
		for (int i = 0; i < num; i++)
		{
			array[i].Read(_br);
			if (ItemClass.GetForId(array[i].itemValue.type) == null)
			{
				array[i] = ItemStack.Empty;
			}
		}
		return array;
	}

	// Token: 0x0600928B RID: 37515 RVA: 0x003768B8 File Offset: 0x00374AB8
	public static void WriteItemValueArray(BinaryWriter _bw, ItemValue[] _items)
	{
		_bw.Write((ushort)_items.Length);
		foreach (ItemValue itemValue in _items)
		{
			bool flag = itemValue != null;
			_bw.Write(flag);
			if (flag)
			{
				itemValue.Write(_bw);
			}
		}
	}

	// Token: 0x0600928C RID: 37516 RVA: 0x003768F8 File Offset: 0x00374AF8
	public static ItemValue[] ReadItemValueArray(BinaryReader _br)
	{
		ItemValue[] array = new ItemValue[(int)_br.ReadUInt16()];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = new ItemValue();
			if (_br.ReadBoolean())
			{
				array[i].Read(_br);
			}
		}
		return array;
	}

	// Token: 0x0600928D RID: 37517 RVA: 0x0037693C File Offset: 0x00374B3C
	public static void HarvestOnAttack(ItemActionData _actionData, Dictionary<string, ItemActionAttack.Bonuses> ToolBonuses)
	{
		if (_actionData.invData.world.IsEditor() || !(_actionData.invData.holdingEntity is EntityPlayerLocal) || _actionData.attackDetails == null || _actionData.attackDetails.itemsToDrop == null)
		{
			return;
		}
		if (GameUtils.random == null)
		{
			GameUtils.random = GameRandomManager.Instance.CreateGameRandom();
			GameUtils.random.SetSeed((int)Stopwatch.GetTimestamp());
		}
		Block block = _actionData.attackDetails.blockBeingDamaged.Block;
		if (block.RepairItemsMeshDamage != null)
		{
			BlockValue blockBeingDamaged = _actionData.attackDetails.blockBeingDamaged;
			blockBeingDamaged.damage += _actionData.attackDetails.damageGiven;
			_actionData.attackDetails.bKilled = blockBeingDamaged.damage < block.MaxDamage && block.shape.UseRepairDamageState(blockBeingDamaged);
		}
		if (_actionData.attackDetails.bKilled)
		{
			if (!_actionData.attackDetails.itemsToDrop.ContainsKey(EnumDropEvent.Destroy))
			{
				if (!_actionData.attackDetails.blockBeingDamaged.isair && _actionData.attackDetails.bBlockHit)
				{
					ItemValue itemValue = _actionData.attackDetails.blockBeingDamaged.ToItemValue();
					int num = 1;
					GameUtils.collectHarvestedItem(_actionData, itemValue, num, 1f, false);
				}
			}
			else
			{
				List<Block.SItemDropProb> list = _actionData.attackDetails.itemsToDrop[EnumDropEvent.Destroy];
				for (int i = 0; i < list.Count; i++)
				{
					if (_actionData.attackDetails.bBlockHit && list[i].name.Equals("[recipe]"))
					{
						List<Recipe> recipes = CraftingManager.GetRecipes(_actionData.attackDetails.blockBeingDamaged.Block.GetBlockName());
						if (recipes.Count > 0)
						{
							for (int j = 0; j < recipes[0].ingredients.Count; j++)
							{
								if (recipes[0].ingredients[j].count / 2 > 0)
								{
									GameUtils.collectHarvestedItem(_actionData, recipes[0].ingredients[j].itemValue, recipes[0].ingredients[j].count / 2, 1f, false);
								}
							}
						}
					}
					else
					{
						float num2 = 1f;
						if (list[i].toolCategory != null)
						{
							num2 = 0f;
							if (ToolBonuses != null && ToolBonuses.ContainsKey(list[i].toolCategory))
							{
								num2 = ToolBonuses[list[i].toolCategory].Tool;
							}
						}
						ItemValue itemValue2 = (list[i].name.Equals("*") ? _actionData.attackDetails.blockBeingDamaged.ToItemValue() : new ItemValue(ItemClass.GetItem(list[i].name, false).type, false));
						if (itemValue2.type != 0 && ItemClass.list[itemValue2.type] != null)
						{
							num2 = EffectManager.GetValue(PassiveEffects.HarvestCount, _actionData.invData.itemValue, num2, _actionData.invData.holdingEntity, null, FastTags<TagGroup.Global>.Parse(list[i].tag), true, true, true, true, true, 1, true, false);
							float num3 = 1f;
							if (itemValue2.ItemClass.HasAnyTags(GameUtils.seedMultiTag))
							{
								num3 *= XUiM_Recipes.SeedDropOutputModifier;
							}
							else if (block.HasAnyFastTags(GameUtils.cropTag))
							{
								num3 *= XUiM_Recipes.CropOutputModifier;
							}
							else if (block.HasAnyFastTags(GameUtils.oreTag))
							{
								num3 *= XUiM_Recipes.MiningOutputModifier;
							}
							else
							{
								num3 *= XUiM_Recipes.HarvestingOutputModifier;
							}
							if (num3 != 0f && (list[i].prob > 0.999f || GameUtils.random.RandomFloat <= list[i].prob))
							{
								int num4 = (int)((float)GameUtils.random.RandomRange(list[i].minCount, list[i].maxCount + 1) * num2);
								if (num4 > 0)
								{
									GameUtils.collectHarvestedItem(_actionData, itemValue2, Mathf.Max(1, (int)((float)num4 * num3)), 1f, false);
								}
							}
						}
					}
				}
			}
		}
		if (_actionData.attackDetails.bBlockHit)
		{
			_actionData.invData.holdingEntity.MinEventContext.BlockValue = _actionData.attackDetails.blockBeingDamaged;
			_actionData.invData.holdingEntity.FireEvent(MinEventTypes.onSelfHarvestBlock, true);
		}
		else
		{
			_actionData.invData.holdingEntity.MinEventContext.Other = _actionData.attackDetails.entityHit as EntityAlive;
			_actionData.invData.holdingEntity.FireEvent(MinEventTypes.onSelfHarvestOther, true);
		}
		if (_actionData.attackDetails.itemsToDrop.ContainsKey(EnumDropEvent.Harvest))
		{
			List<Block.SItemDropProb> list2 = _actionData.attackDetails.itemsToDrop[EnumDropEvent.Harvest];
			for (int k = 0; k < list2.Count; k++)
			{
				float num5 = 0f;
				if (list2[k].toolCategory != null)
				{
					num5 = 0f;
					if (ToolBonuses != null && ToolBonuses.ContainsKey(list2[k].toolCategory))
					{
						num5 = ToolBonuses[list2[k].toolCategory].Tool;
					}
				}
				ItemValue itemValue3 = (list2[k].name.Equals("*") ? _actionData.attackDetails.blockBeingDamaged.ToItemValue() : new ItemValue(ItemClass.GetItem(list2[k].name, false).type, false));
				if (itemValue3.type != 0 && ItemClass.list[itemValue3.type] != null)
				{
					num5 = EffectManager.GetValue(PassiveEffects.HarvestCount, _actionData.invData.itemValue, num5, _actionData.invData.holdingEntity, null, FastTags<TagGroup.Global>.Parse(list2[k].tag), true, true, true, true, true, 1, true, false);
					float num6 = 1f;
					if (itemValue3.ItemClass.HasAnyTags(GameUtils.seedMultiTag))
					{
						num6 *= XUiM_Recipes.SeedDropOutputModifier;
					}
					else if (block.HasAnyFastTags(GameUtils.cropTag))
					{
						num6 *= XUiM_Recipes.CropOutputModifier;
					}
					else if (block.HasAnyFastTags(GameUtils.oreTag))
					{
						num6 *= XUiM_Recipes.MiningOutputModifier;
					}
					else
					{
						num6 *= XUiM_Recipes.HarvestingOutputModifier;
					}
					int num7 = (int)((float)GameUtils.random.RandomRange(list2[k].minCount, list2[k].maxCount + 1) * num5);
					int num8 = num7 - num7 / 3;
					if (num6 != 0f)
					{
						if (num8 > 0)
						{
							GameUtils.collectHarvestedItem(_actionData, itemValue3, Mathf.Max((int)((float)num8 * num6), 1), list2[k].prob, true);
						}
						if (_actionData.attackDetails.bKilled)
						{
							num8 = num7 / 3;
							float num9 = list2[k].prob;
							float resourceScale = list2[k].resourceScale;
							if (resourceScale > 0f && resourceScale < 1f)
							{
								num9 /= resourceScale;
								num8 = (int)((float)num8 * resourceScale);
								if (num8 < 1)
								{
									num8++;
								}
							}
							if (num8 > 0)
							{
								GameUtils.collectHarvestedItem(_actionData, itemValue3, Mathf.Max((int)((float)num8 * num6), 1), num9, false);
							}
						}
					}
				}
			}
		}
		_actionData.attackDetails.blockBeingDamaged = BlockValue.Air;
	}

	// Token: 0x0600928E RID: 37518 RVA: 0x00377088 File Offset: 0x00375288
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool collectHarvestedItem(ItemActionData _actionData, ItemValue _iv, int _count, float _prob, bool _bScaleCountOnDamage = true)
	{
		if (GameUtils.random == null)
		{
			GameUtils.random = GameRandomManager.Instance.CreateGameRandom();
			GameUtils.random.SetSeed((int)Stopwatch.GetTimestamp());
		}
		if (_bScaleCountOnDamage)
		{
			float num = (float)_actionData.attackDetails.damageMax / (float)_count;
			int num2 = (int)((Utils.FastMin(_actionData.attackDetails.damageTotalOfTarget, (float)_actionData.attackDetails.damageMax) - (float)_actionData.attackDetails.damageGiven) / num + 0.5f);
			int num3 = Mathf.Min((int)(_actionData.attackDetails.damageTotalOfTarget / num + 0.5f), _count);
			int num4 = _count;
			_count = num3 - num2;
			if (_actionData.attackDetails.damageTotalOfTarget > (float)_actionData.attackDetails.damageMax)
			{
				_count = Mathf.Min(_count, num4);
			}
		}
		if (GameUtils.random.RandomFloat <= _prob && _count > 0)
		{
			ItemStack itemStack = new ItemStack(_iv, _count);
			LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(_actionData.invData.holdingEntity as EntityPlayerLocal);
			XUiM_PlayerInventory playerInventory = uiforPlayer.xui.PlayerInventory;
			QuestEventManager.Current.HarvestedItem(_actionData.invData.itemValue, itemStack, _actionData.attackDetails.blockBeingDamaged);
			if (!playerInventory.AddItem(itemStack))
			{
				GameManager.Instance.ItemDropServer(new ItemStack(_iv, itemStack.count), GameManager.Instance.World.GetPrimaryPlayer().GetDropPosition(), new Vector3(0.5f, 0.5f, 0.5f), GameManager.Instance.World.GetPrimaryPlayerId(), 60f, false);
			}
			uiforPlayer.entityPlayer.Progression.AddLevelExp((int)(itemStack.itemValue.ItemClass.MadeOfMaterial.Experience * (float)_count), "_xpFromHarvesting", Progression.XPTypes.Harvesting, true, true, -1, null);
			return true;
		}
		return false;
	}

	// Token: 0x0600928F RID: 37519 RVA: 0x00377238 File Offset: 0x00375438
	public static void DrawCube(Vector3 _pos, Color _col)
	{
		global::UnityEngine.Debug.DrawLine(_pos + new Vector3(0.1f, 0.1f, 0.1f), _pos + new Vector3(0.9f, 0.1f, 0.1f), _col, 10f);
		global::UnityEngine.Debug.DrawLine(_pos + new Vector3(0.1f, 0.1f, 0.1f), _pos + new Vector3(0.1f, 0.1f, 0.9f), _col, 10f);
		global::UnityEngine.Debug.DrawLine(_pos + new Vector3(0.9f, 0.1f, 0.1f), _pos + new Vector3(0.9f, 0.1f, 0.9f), _col, 10f);
		global::UnityEngine.Debug.DrawLine(_pos + new Vector3(0.9f, 0.1f, 0.9f), _pos + new Vector3(0.1f, 0.1f, 0.9f), _col, 10f);
		global::UnityEngine.Debug.DrawLine(_pos + new Vector3(0.1f, 0.9f, 0.1f), _pos + new Vector3(0.9f, 0.9f, 0.1f), _col, 10f);
		global::UnityEngine.Debug.DrawLine(_pos + new Vector3(0.1f, 0.9f, 0.1f), _pos + new Vector3(0.1f, 0.9f, 0.9f), _col, 10f);
		global::UnityEngine.Debug.DrawLine(_pos + new Vector3(0.9f, 0.9f, 0.1f), _pos + new Vector3(0.9f, 0.9f, 0.9f), _col, 10f);
		global::UnityEngine.Debug.DrawLine(_pos + new Vector3(0.9f, 0.9f, 0.9f), _pos + new Vector3(0.1f, 0.9f, 0.9f), _col, 10f);
		global::UnityEngine.Debug.DrawLine(_pos + new Vector3(0.1f, 0.1f, 0.1f), _pos + new Vector3(0.1f, 0.9f, 0.1f), _col, 10f);
		global::UnityEngine.Debug.DrawLine(_pos + new Vector3(0.1f, 0.1f, 0.9f), _pos + new Vector3(0.1f, 0.9f, 0.9f), _col, 10f);
		global::UnityEngine.Debug.DrawLine(_pos + new Vector3(0.9f, 0.1f, 0.1f), _pos + new Vector3(0.9f, 0.9f, 0.1f), _col, 10f);
		global::UnityEngine.Debug.DrawLine(_pos + new Vector3(0.9f, 0.1f, 0.9f), _pos + new Vector3(0.9f, 0.9f, 0.9f), _col, 10f);
	}

	// Token: 0x06009290 RID: 37520 RVA: 0x00377539 File Offset: 0x00375739
	public static string SafeStringFormat(string _s)
	{
		return _s.Replace("{", "{{").Replace("}", "}}");
	}

	// Token: 0x06009291 RID: 37521 RVA: 0x0037755C File Offset: 0x0037575C
	public static Vector3 GetNormalFromHitInfo(Vector3i _blockPos, Collider _hitCollider, int _hitTriangleIdx, out Vector3 _hitFaceCenter)
	{
		_hitFaceCenter = Vector3.zero;
		if (_hitTriangleIdx < 0)
		{
			return Vector3.zero;
		}
		MeshCollider meshCollider = _hitCollider as MeshCollider;
		if (!(meshCollider != null) || !(meshCollider.sharedMesh != null) || !meshCollider.sharedMesh.isReadable)
		{
			return Vector3.zero;
		}
		Mesh sharedMesh = meshCollider.sharedMesh;
		GameUtils.tempVertices.Clear();
		sharedMesh.GetVertices(GameUtils.tempVertices);
		GameUtils.tempTriangles.Clear();
		sharedMesh.GetTriangles(GameUtils.tempTriangles, 0);
		int num = _hitTriangleIdx * 3;
		if (num >= GameUtils.tempTriangles.Count || GameUtils.tempTriangles[num] >= GameUtils.tempVertices.Count)
		{
			return Vector3.zero;
		}
		Vector3 vector = GameUtils.tempVertices[GameUtils.tempTriangles[num]];
		Vector3 vector2 = GameUtils.tempVertices[GameUtils.tempTriangles[num + 1]];
		Vector3 vector3 = GameUtils.tempVertices[GameUtils.tempTriangles[num + 2]];
		Vector3 vector4 = Vector3.Cross(vector - vector2, vector - vector3);
		Vector3 vector5 = (vector + vector2 + vector3) / 3f;
		_hitFaceCenter = vector5 + World.toChunkXyzWorldPos(_blockPos);
		return vector4;
	}

	// Token: 0x06009292 RID: 37522 RVA: 0x003776A0 File Offset: 0x003758A0
	public static BlockFace GetBlockFaceFromHitInfo(Vector3i _blockPos, BlockValue _blockValue, Collider _hitCollider, int _hitTriangleIdx, out Vector3 _hitFaceCenter, out Vector3 _hitFaceNormal)
	{
		_hitFaceCenter = Vector3.zero;
		_hitFaceNormal = Vector3.zero;
		if (_hitTriangleIdx < 0)
		{
			return BlockFace.None;
		}
		MeshCollider meshCollider = _hitCollider as MeshCollider;
		if (meshCollider != null && meshCollider.sharedMesh != null && meshCollider.sharedMesh.isReadable)
		{
			Mesh sharedMesh = meshCollider.sharedMesh;
			GameUtils.tempVertices.Clear();
			sharedMesh.GetVertices(GameUtils.tempVertices);
			GameUtils.tempTriangles.Clear();
			sharedMesh.GetTriangles(GameUtils.tempTriangles, 0);
			int num = _hitTriangleIdx * 3;
			if (num >= GameUtils.tempTriangles.Count || GameUtils.tempTriangles[num] >= GameUtils.tempVertices.Count)
			{
				return BlockFace.None;
			}
			Vector3 vector = GameUtils.tempVertices[GameUtils.tempTriangles[num]];
			Vector3 vector2 = GameUtils.tempVertices[GameUtils.tempTriangles[num + 1]];
			Vector3 vector3 = GameUtils.tempVertices[GameUtils.tempTriangles[num + 2]];
			_hitFaceNormal = Vector3.Cross(vector - vector2, vector - vector3);
			Vector3 vector4 = (vector + vector2 + vector3) / 3f;
			_hitFaceCenter = vector4 + World.toChunkXyzWorldPos(_blockPos);
			Vector3 vector5 = World.toBlock(_blockPos).ToVector3();
			vector -= vector5;
			vector2 -= vector5;
			vector3 -= vector5;
			if (!_blockValue.Block.isMultiBlock)
			{
				if ((double)vector.x < -0.001)
				{
					vector.x += 16f;
				}
				else if (vector.x > 15f)
				{
					vector.x -= 16f;
				}
				if ((double)vector.y < -0.001)
				{
					vector.y += 16f;
				}
				else if (vector.y > 15f)
				{
					vector.y -= 16f;
				}
				if ((double)vector.z < -0.001)
				{
					vector.z += 16f;
				}
				else if (vector.z > 15f)
				{
					vector.z -= 16f;
				}
				if ((double)vector2.x < -0.001)
				{
					vector2.x += 16f;
				}
				else if (vector2.x > 15f)
				{
					vector2.x -= 16f;
				}
				if ((double)vector2.y < -0.001)
				{
					vector2.y += 16f;
				}
				else if (vector2.y > 15f)
				{
					vector2.y -= 16f;
				}
				if ((double)vector2.z < -0.001)
				{
					vector2.z += 16f;
				}
				else if (vector2.z > 15f)
				{
					vector2.z -= 16f;
				}
				if ((double)vector3.x < -0.001)
				{
					vector3.x += 16f;
				}
				else if (vector3.x > 15f)
				{
					vector3.x -= 16f;
				}
				if ((double)vector3.y < -0.001)
				{
					vector3.y += 16f;
				}
				else if (vector3.y > 15f)
				{
					vector3.y -= 16f;
				}
				if ((double)vector3.z < -0.001)
				{
					vector3.z += 16f;
				}
				else if (vector3.z > 15f)
				{
					vector3.z -= 16f;
				}
			}
			BlockShapeNew blockShapeNew = _blockValue.Block.shape as BlockShapeNew;
			if (blockShapeNew != null)
			{
				Vector3 vector6 = Vector3.one * 0.5f;
				Quaternion quaternion = Quaternion.Inverse(blockShapeNew.GetRotation(_blockValue));
				vector = quaternion * (vector - vector6) + vector6;
				vector2 = quaternion * (vector2 - vector6) + vector6;
				vector3 = quaternion * (vector3 - vector6) + vector6;
				return blockShapeNew.GetBlockFaceFromColliderTriangle(_blockValue, vector, vector2, vector3);
			}
		}
		return BlockFace.None;
	}

	// Token: 0x06009293 RID: 37523 RVA: 0x00377B0C File Offset: 0x00375D0C
	public static string GetLaunchArgument(string _argumentName)
	{
		if (GameUtils.arguments == null)
		{
			GameUtils.arguments = new CaseInsensitiveStringDictionary<string>();
			string[] commandLineArgs = GameStartupHelper.GetCommandLineArgs();
			for (int i = 0; i < commandLineArgs.Length; i++)
			{
				if (!string.IsNullOrEmpty(commandLineArgs[i]) && commandLineArgs[i][0] == '-')
				{
					int num = commandLineArgs[i].IndexOf('=');
					string text;
					string text2;
					if (num >= 0)
					{
						text = commandLineArgs[i].Substring(1, num - 1);
						text2 = commandLineArgs[i].Substring(num + 1);
					}
					else
					{
						text = commandLineArgs[i].Substring(1);
						text2 = string.Empty;
					}
					GameUtils.arguments[text] = text2;
				}
			}
		}
		if (GameUtils.arguments.ContainsKey(_argumentName))
		{
			return GameUtils.arguments[_argumentName];
		}
		return null;
	}

	// Token: 0x06009294 RID: 37524 RVA: 0x00377BBC File Offset: 0x00375DBC
	public static bool IsBlockOrTerrain(string _tag)
	{
		return _tag == "B_Mesh" || _tag == "T_Mesh" || _tag == "T_Mesh_B" || _tag == "T_Block" || _tag == "T_Deco";
	}

	// Token: 0x06009295 RID: 37525 RVA: 0x00377C0C File Offset: 0x00375E0C
	public static bool IsBlockOrTerrain(Component component)
	{
		return component.CompareTag("B_Mesh") || component.CompareTag("T_Mesh") || component.CompareTag("T_Mesh_B") || component.CompareTag("T_Block") || component.CompareTag("T_Deco");
	}

	// Token: 0x06009296 RID: 37526 RVA: 0x00377C5C File Offset: 0x00375E5C
	public static ulong Vector3iToUInt64(Vector3i _v)
	{
		return (ulong)(((long)((_v.x + 32768) & 65535) << 32) | ((long)((_v.y + 32768) & 65535) << 16) | ((long)(_v.z + 32768) & 65535L));
	}

	// Token: 0x06009297 RID: 37527 RVA: 0x00377CAB File Offset: 0x00375EAB
	public static Vector3i UInt64ToVector3i(ulong _fullValue)
	{
		return new Vector3i((int)((_fullValue >> 32) & 65535UL) - 32768, (int)((_fullValue >> 16) & 65535UL) - 32768, (int)(_fullValue & 65535UL) - 32768);
	}

	// Token: 0x06009298 RID: 37528 RVA: 0x00377CE8 File Offset: 0x00375EE8
	public static char ValidateGameNameInput(string _text, int _charIndex, char _addedChar)
	{
		if (_addedChar >= 'Ā')
		{
			return _addedChar;
		}
		if ((_addedChar >= 'a' && _addedChar <= 'z') || (_addedChar >= 'A' && _addedChar <= 'Z') || (_addedChar >= '0' && _addedChar <= '9'))
		{
			return _addedChar;
		}
		if (_addedChar == '_' || _addedChar == '-')
		{
			return _addedChar;
		}
		if (_charIndex > 0 && (_addedChar == '.' || _addedChar == ' '))
		{
			return _addedChar;
		}
		return '\0';
	}

	// Token: 0x06009299 RID: 37529 RVA: 0x00377D3C File Offset: 0x00375F3C
	public static char ValidateHexInput(string _text, int _charIndex, char _addedChar)
	{
		if ((_addedChar >= 'a' && _addedChar <= 'f') || (_addedChar >= 'A' && _addedChar <= 'F') || (_addedChar >= '0' && _addedChar <= '9'))
		{
			return _addedChar;
		}
		return '\0';
	}

	// Token: 0x0600929A RID: 37530 RVA: 0x00377D60 File Offset: 0x00375F60
	public static bool ValidateGameName(string _gameName)
	{
		string text = _gameName.Trim();
		if (string.IsNullOrEmpty(text) || text.Length != _gameName.Length)
		{
			return false;
		}
		for (int i = 0; i < _gameName.Length; i++)
		{
			if (GameUtils.ValidateGameNameInput(_gameName, i, _gameName[i]) == '\0')
			{
				return false;
			}
		}
		return !_gameName.EndsWith(".");
	}

	// Token: 0x0600929B RID: 37531 RVA: 0x00377DC0 File Offset: 0x00375FC0
	public static PrefabInstance FindPrefabForBlockPos(List<PrefabInstance> prefabs, Vector3i hitPointBlockPos)
	{
		for (int i = 0; i < prefabs.Count; i++)
		{
			if (prefabs[i].boundingBoxPosition.x <= hitPointBlockPos.x && prefabs[i].boundingBoxPosition.x + prefabs[i].boundingBoxSize.x >= hitPointBlockPos.x && prefabs[i].boundingBoxPosition.z <= hitPointBlockPos.z && prefabs[i].boundingBoxPosition.z + prefabs[i].boundingBoxSize.z >= hitPointBlockPos.z)
			{
				return prefabs[i];
			}
		}
		return null;
	}

	// Token: 0x0600929C RID: 37532 RVA: 0x00377E78 File Offset: 0x00376078
	public static int FindPaintIdForBlockFace(BlockValue _bv, BlockFace blockFace, out string _name, int _channel)
	{
		int sideTextureId = _bv.Block.GetSideTextureId(_bv, blockFace, _channel);
		for (int i = 0; i < BlockTextureData.list.Length; i++)
		{
			if (BlockTextureData.list[i] != null && (int)BlockTextureData.list[i].TextureID == sideTextureId)
			{
				_name = BlockTextureData.list[i].Name;
				return i;
			}
		}
		_name = string.Empty;
		return 0;
	}

	// Token: 0x0600929D RID: 37533 RVA: 0x00377ED8 File Offset: 0x003760D8
	public static Vector3i Mirror(EnumMirrorAlong _axis, Vector3i _pos, Vector3i _prefabSize)
	{
		if (_axis == EnumMirrorAlong.XAxis)
		{
			return new Vector3i(_prefabSize.x - _pos.x - 1, _pos.y, _pos.z);
		}
		if (_axis != EnumMirrorAlong.YAxis)
		{
			return new Vector3i(_pos.x, _pos.y, _prefabSize.z - _pos.z - 1);
		}
		return new Vector3i(_pos.x, _prefabSize.y - _pos.y - 1, _pos.z);
	}

	// Token: 0x0600929E RID: 37534 RVA: 0x00377F50 File Offset: 0x00376150
	public static Vector3 Mirror(EnumMirrorAlong _axis, Vector3 _pos, Vector3i _prefabSize)
	{
		if (_axis == EnumMirrorAlong.XAxis)
		{
			return new Vector3((float)_prefabSize.x - _pos.x, _pos.y, _pos.z);
		}
		if (_axis != EnumMirrorAlong.YAxis)
		{
			return new Vector3(_pos.x, _pos.y, (float)_prefabSize.z - _pos.z);
		}
		return new Vector3(_pos.x, (float)_prefabSize.y - _pos.y, _pos.z);
	}

	// Token: 0x0600929F RID: 37535 RVA: 0x00377FC5 File Offset: 0x003761C5
	public static void TakeScreenShot(GameUtils.EScreenshotMode _screenshotMode, string _overrideScreenshotFilePath = null, float _borderPerc = 0f, bool _b4to3 = false, int _rescaleToW = 0, int _rescaleToH = 0, bool _isSaveTGA = false)
	{
		ThreadManager.StartCoroutine(GameUtils.TakeScreenshotEnum(_screenshotMode, _overrideScreenshotFilePath, _borderPerc, _b4to3, _rescaleToW, _rescaleToH, _isSaveTGA));
	}

	// Token: 0x060092A0 RID: 37536 RVA: 0x00377FDC File Offset: 0x003761DC
	public static IEnumerator TakeScreenshotEnum(GameUtils.EScreenshotMode _screenshotMode, string _overrideScreenshotFilePath = null, float _borderPerc = 0f, bool _b4to3 = false, int _rescaleToW = 0, int _rescaleToH = 0, bool _isSaveTGA = false)
	{
		yield return new WaitForEndOfFrame();
		Rect screenshotRect = GameUtils.GetScreenshotRect(_borderPerc, _b4to3);
		Texture2D texture2D = new Texture2D((int)screenshotRect.width, (int)screenshotRect.height, TextureFormat.RGB24, false);
		texture2D.ReadPixels(screenshotRect, 0, 0);
		if (_rescaleToW != 0 && _rescaleToH != 0)
		{
			TextureScale.Bilinear(texture2D, _rescaleToW, _rescaleToH);
		}
		texture2D.Apply();
		if (_screenshotMode != GameUtils.EScreenshotMode.File)
		{
			TextureUtils.CopyToClipboard(texture2D);
		}
		if (_screenshotMode != GameUtils.EScreenshotMode.Clipboard)
		{
			string text3;
			if (_overrideScreenshotFilePath == null)
			{
				string text = GameIO.GetDeviceLocalUserGameDataDir() + "/Screenshots";
				if (!SdDirectory.Exists(text))
				{
					SdDirectory.CreateDirectory(text);
				}
				string text2 = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
				text3 = string.Concat(new string[]
				{
					text,
					"/",
					Constants.cVersionInformation.ShortString,
					"_",
					text2
				});
			}
			else
			{
				text3 = _overrideScreenshotFilePath;
			}
			text3 += (_isSaveTGA ? ".tga" : ".jpg");
			GameUtils.lastSavedScreenshotFilename = text3;
			if (_isSaveTGA)
			{
				SdFile.WriteAllBytes(text3, texture2D.EncodeToTGA());
			}
			else
			{
				SdFile.WriteAllBytes(text3, texture2D.EncodeToJPG());
			}
		}
		global::UnityEngine.Object.Destroy(texture2D);
		yield break;
	}

	// Token: 0x060092A1 RID: 37537 RVA: 0x00378018 File Offset: 0x00376218
	public static Rect GetScreenshotRect(float _borderPerc = 0f, bool _b4to3 = false)
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = (float)Screen.width;
		float num4 = (float)Screen.height;
		if (_borderPerc > 0.001f)
		{
			float num5 = (float)Screen.width * _borderPerc;
			float num6 = (float)Screen.height * _borderPerc;
			num += num5;
			num3 -= num5 * 2f;
			num2 += num6;
			num4 -= num6 * 2f;
		}
		if (_b4to3)
		{
			num3 = 1.3333334f * num4;
			num = ((float)Screen.width - num3) / 2f;
		}
		return new Rect(num, num2, num3, num4);
	}

	// Token: 0x060092A2 RID: 37538 RVA: 0x0037809E File Offset: 0x0037629E
	public static void StartPlaytesting(string _prefabName)
	{
		if (string.IsNullOrEmpty(_prefabName))
		{
			return;
		}
		GameManager.bHideMainMenuNextTime = true;
		GameManager.Instance.Disconnect();
		ThreadManager.StartCoroutine(GameUtils.startPlaytestLater(_prefabName));
	}

	// Token: 0x060092A3 RID: 37539 RVA: 0x003780C5 File Offset: 0x003762C5
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator startPlaytestLater(string _prefabName)
	{
		yield return new WaitForSeconds(2f);
		GamePrefs.Set(EnumGamePrefs.SandboxPreset, "");
		GamePrefs.Set(EnumGamePrefs.SandboxCode, "");
		GamePrefs.Set(EnumGamePrefs.GameWorld, "Playtesting");
		GamePrefs.Set(EnumGamePrefs.GameWorldLocationType, 4);
		GamePrefs.Set(EnumGamePrefs.GameMode, EnumGameMode.Survival.ToStringCached<EnumGameMode>());
		GamePrefs.Set(EnumGamePrefs.GameName, _prefabName);
		GamePrefs.Set(EnumGamePrefs.GameSaveStorageType, 0);
		string saveGameDir = GameIO.GetSaveGameDir();
		if (SdDirectory.Exists(saveGameDir))
		{
			SdDirectory.Delete(saveGameDir, true);
		}
		SingletonMonoBehaviour<ConnectionManager>.Instance.StartServers(GamePrefs.GetString(EnumGamePrefs.ServerPassword), false);
		yield break;
	}

	// Token: 0x060092A4 RID: 37540 RVA: 0x003780D4 File Offset: 0x003762D4
	public static bool IsPlaytesting()
	{
		return GamePrefs.GetString(EnumGamePrefs.GameWorld) == "Playtesting";
	}

	// Token: 0x060092A5 RID: 37541 RVA: 0x003780E7 File Offset: 0x003762E7
	public static bool IsWorldEditor()
	{
		return GameModeEditWorld.TypeName.Equals(GamePrefs.GetString(EnumGamePrefs.GameMode)) && GamePrefs.GetString(EnumGamePrefs.GameName) == "WorldEditor";
	}

	// Token: 0x060092A6 RID: 37542 RVA: 0x0037810F File Offset: 0x0037630F
	public static void StartSinglePrefabEditingWithMessage(XUi _xui = null)
	{
		GameUtils.StartSinglePrefabEditing(delegate(NetworkConnectionError _error)
		{
			if (_error != NetworkConnectionError.NoError)
			{
				XUiC_MessageBoxWindowGroup.ShowNetworkError(_xui ?? LocalPlayerUI.primaryUI.xui, _error, false);
			}
		});
	}

	// Token: 0x060092A7 RID: 37543 RVA: 0x00378130 File Offset: 0x00376330
	public static void StartSinglePrefabEditing(Action<NetworkConnectionError> _callback = null)
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			GameManager.bHideMainMenuNextTime = true;
			GameManager.Instance.Disconnect();
			ThreadManager.StartCoroutine(GameUtils.startSinglePrefabEditingLater(2f, _callback));
			return;
		}
		ThreadManager.StartCoroutine(GameUtils.startSinglePrefabEditingLater(0f, _callback));
	}

	// Token: 0x060092A8 RID: 37544 RVA: 0x0037817C File Offset: 0x0037637C
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator startSinglePrefabEditingLater(float _delay, Action<NetworkConnectionError> _callback)
	{
		yield return new WaitForSeconds(_delay);
		new GameModeEditWorld().ResetGamePrefs();
		GamePrefs.Set(EnumGamePrefs.SandboxPreset, "");
		GamePrefs.Set(EnumGamePrefs.SandboxCode, "");
		GamePrefs.Set(EnumGamePrefs.GameWorld, "Empty");
		GamePrefs.Set(EnumGamePrefs.GameWorldLocationType, 4);
		GamePrefs.Set(EnumGamePrefs.GameMode, GameModeEditWorld.TypeName);
		GamePrefs.Set(EnumGamePrefs.GameName, "PrefabEditor");
		GamePrefs.Set(EnumGamePrefs.GameSaveStorageType, 0);
		GamePrefs.Set(EnumGamePrefs.ServerPort, 27020);
		NetworkConnectionError networkConnectionError = SingletonMonoBehaviour<ConnectionManager>.Instance.StartServers(GamePrefs.GetString(EnumGamePrefs.ServerPassword), false);
		if (_callback != null)
		{
			_callback(networkConnectionError);
		}
		yield break;
	}

	// Token: 0x060092A9 RID: 37545 RVA: 0x00378192 File Offset: 0x00376392
	public static float GetOreNoiseAt(PerlinNoise _noise, int _x, int _y, int _z)
	{
		return ((float)_noise.Noise((double)((float)_x * 0.05f), (double)((float)_y * 0.05f), (double)((float)_z * 0.05f)) - 0.333f) * 3f;
	}

	// Token: 0x060092AA RID: 37546 RVA: 0x003781C2 File Offset: 0x003763C2
	public static bool CheckOreNoiseAt(PerlinNoise _noise, int _x, int _y, int _z)
	{
		return GameUtils.GetOreNoiseAt(_noise, _x, _y, _z) > 0f;
	}

	// Token: 0x060092AB RID: 37547 RVA: 0x003781D4 File Offset: 0x003763D4
	public static Color32 UIntToColor(uint color, bool _includeAlpha = false)
	{
		if (_includeAlpha)
		{
			return new Color32((byte)(color >> 16), (byte)(color >> 8), (byte)color, (byte)(color >> 24));
		}
		return new Color32((byte)(color >> 16), (byte)(color >> 8), (byte)color, byte.MaxValue);
	}

	// Token: 0x060092AC RID: 37548 RVA: 0x00378204 File Offset: 0x00376404
	public static uint ColorToUInt(Color32 color, bool _includeAlpha = false)
	{
		if (_includeAlpha)
		{
			return (uint)(((int)color.r << 24) | ((int)color.r << 16) | ((int)color.g << 8) | (int)color.b);
		}
		return (uint)(((int)color.r << 16) | ((int)color.g << 8) | (int)color.b);
	}

	// Token: 0x060092AD RID: 37549 RVA: 0x00378254 File Offset: 0x00376454
	public static void WaterFloodFill(GridCompressedData<byte> _cols, byte[] _waterChunks16x16Height, int _width, HeightMap _heightMap, int _posX, int _maxY, int _posZ, byte _colWater, byte _colBorder, List<Vector2i> _listPos, int _minX = -2147483648, int _maxX = 2147483647, int _minZ = -2147483648, int _maxZ = 2147483647, int _worldScale = 1)
	{
		int num = _heightMap.GetHeight() * _worldScale;
		do
		{
			int num2 = _posX + _width / 2;
			int num3 = _posZ + num / 2;
			if (_heightMap.GetAt(num2, num3) < (float)(_maxY + 1))
			{
				_cols.SetValue(num2, num3, _colWater);
				_waterChunks16x16Height[num2 / 16 + num3 / 16 * _width / 16] = (byte)_maxY;
				Vector2i vector2i;
				if (num2 < _width - 1 && _posX < _maxX && _cols.GetValue(num2 + 1, num3) == 0 && _listPos.Count < 100000)
				{
					vector2i.x = _posX + 1;
					vector2i.y = _posZ;
					_listPos.Add(vector2i);
				}
				if (num2 > 0 && _posX > _minX && _cols.GetValue(num2 - 1, num3) == 0 && _listPos.Count < 100000)
				{
					vector2i.x = _posX - 1;
					vector2i.y = _posZ;
					_listPos.Add(vector2i);
				}
				if (num3 > 0 && _posZ > _minZ && _cols.GetValue(num2, num3 - 1) == 0 && _listPos.Count < 100000)
				{
					vector2i.x = _posX;
					vector2i.y = _posZ - 1;
					_listPos.Add(vector2i);
				}
				if (num3 < num - 1 && _posZ < _maxZ && _cols.GetValue(num2, num3 + 1) == 0 && _listPos.Count < 100000)
				{
					vector2i.x = _posX;
					vector2i.y = _posZ + 1;
					_listPos.Add(vector2i);
				}
			}
			else
			{
				_cols.SetValue(num2, num3, _colBorder);
			}
			int count = _listPos.Count;
			if (count > 0)
			{
				Vector2i vector2i = _listPos[count - 1];
				_posX = vector2i.x;
				_posZ = vector2i.y;
				_listPos.RemoveAt(count - 1);
			}
		}
		while (_listPos.Count > 0);
	}

	// Token: 0x060092AE RID: 37550 RVA: 0x003783FC File Offset: 0x003765FC
	public static GameUtils.EPlayerHomeType CheckForAnyPlayerHome(World world, Vector3i BoxMin, Vector3i BoxMax)
	{
		double num = (double)GameStats.GetInt(EnumGameStats.LandClaimExpiryTime) * 24.0;
		double num2 = (double)GameStats.GetInt(EnumGameStats.BedrollExpiryTime) * 24.0;
		int @int = GamePrefs.GetInt(EnumGamePrefs.BedrollDeadZoneSize);
		Vector3i vector3i = new Vector3i(@int, @int, @int);
		Vector3i vector3i2 = BoxMin - vector3i;
		Vector3i vector3i3 = BoxMax + vector3i;
		int int2 = GameStats.GetInt(EnumGameStats.LandClaimSize);
		int num3 = int2 / 2;
		foreach (KeyValuePair<PlatformUserIdentifierAbs, PersistentPlayerData> keyValuePair in GameManager.Instance.GetPersistentPlayerList().Players)
		{
			if (keyValuePair.Value.OfflineHours < num2 && keyValuePair.Value.HasBedrollPos)
			{
				Vector3i bedrollPos = keyValuePair.Value.BedrollPos;
				if (bedrollPos.x >= vector3i2.x && bedrollPos.x < vector3i3.x && bedrollPos.z >= vector3i2.z && bedrollPos.z < vector3i3.z)
				{
					return GameUtils.EPlayerHomeType.Bedroll;
				}
			}
			List<Vector3i> lpblocks = keyValuePair.Value.LPBlocks;
			if (keyValuePair.Value.OfflineHours < num && lpblocks != null && lpblocks.Count > 0)
			{
				for (int i = 0; i < lpblocks.Count; i++)
				{
					Vector3i vector3i4 = lpblocks[i];
					vector3i4.x -= num3;
					vector3i4.z -= num3;
					if (vector3i4.x <= BoxMax.x && vector3i4.x + int2 >= BoxMin.x && vector3i4.z <= BoxMax.z && vector3i4.z + int2 >= BoxMin.z)
					{
						return GameUtils.EPlayerHomeType.Landclaim;
					}
				}
			}
		}
		return GameUtils.EPlayerHomeType.None;
	}

	// Token: 0x060092AF RID: 37551 RVA: 0x003785EC File Offset: 0x003767EC
	public static Transform FindDeepChild(Transform _parent, string _transformName)
	{
		Transform transform = _parent.Find(_transformName);
		if (transform != null)
		{
			return transform;
		}
		int childCount = _parent.childCount;
		for (int i = 0; i < childCount; i++)
		{
			transform = GameUtils.FindDeepChild(_parent.GetChild(i), _transformName);
			if (transform != null)
			{
				return transform;
			}
		}
		return transform;
	}

	// Token: 0x060092B0 RID: 37552 RVA: 0x0037863C File Offset: 0x0037683C
	public static Transform FindDeepChildActive(Transform _parent, string _transformName)
	{
		Transform transform = _parent.Find(_transformName);
		if (transform != null && transform.gameObject.activeSelf)
		{
			return transform;
		}
		int childCount = _parent.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = _parent.GetChild(i);
			if (child.gameObject.activeSelf)
			{
				transform = GameUtils.FindDeepChildActive(child, _transformName);
				if (transform != null)
				{
					return transform;
				}
			}
		}
		return transform;
	}

	// Token: 0x060092B1 RID: 37553 RVA: 0x003786A5 File Offset: 0x003768A5
	public static int GetViewDistance()
	{
		if (GamePrefs.GetString(EnumGamePrefs.GameWorld) == "Empty")
		{
			return 12;
		}
		return GameStats.GetInt(EnumGameStats.AllowedViewDistance);
	}

	// Token: 0x060092B2 RID: 37554 RVA: 0x003786C4 File Offset: 0x003768C4
	public static Vector3 GetUpdatedNormalAtPosition(Vector3i _worldPos, bool _saveNrmToChunk = false)
	{
		int terrainHeight = (int)GameManager.Instance.World.GetTerrainHeight(_worldPos.x, _worldPos.z);
		int terrainHeight2 = (int)GameManager.Instance.World.GetTerrainHeight(_worldPos.x + 1, _worldPos.z);
		float terrainHeight3 = (float)GameManager.Instance.World.GetTerrainHeight(_worldPos.x, _worldPos.z + 1);
		float num = (float)GameManager.Instance.World.GetDensity(_worldPos.x, terrainHeight, _worldPos.z) / -128f;
		float num2 = (float)GameManager.Instance.World.GetDensity(_worldPos.x + 1, terrainHeight, _worldPos.z) / -128f;
		float num3 = (float)GameManager.Instance.World.GetDensity(_worldPos.x, terrainHeight, _worldPos.z + 1) / -128f;
		float num4 = (float)GameManager.Instance.World.GetDensity(_worldPos.x, terrainHeight + 1, _worldPos.z) / -128f;
		float num5 = (float)GameManager.Instance.World.GetDensity(_worldPos.x + 1, terrainHeight + 1, _worldPos.z) / -128f;
		float num6 = (float)GameManager.Instance.World.GetDensity(_worldPos.x, terrainHeight + 1, _worldPos.z + 1) / -128f;
		if (num > 0.999f && num4 > 0.999f)
		{
			num = 0.5f;
		}
		if (num2 > 0.999f && num5 > 0.999f)
		{
			num2 = 0.5f;
		}
		if (num3 > 0.999f && num6 > 0.999f)
		{
			num3 = 0.5f;
		}
		float num7 = (float)terrainHeight + num;
		float num8 = (float)terrainHeight2 + num2;
		float num9 = terrainHeight3 + num3;
		Vector3 vector = new Vector3(0f, num9, 1f) - new Vector3(0f, num7, 0f);
		Vector3 vector2 = new Vector3(1f, num8, 0f) - new Vector3(0f, num7, 0f);
		return Vector3.Cross(vector, vector2).normalized;
	}

	// Token: 0x060092B3 RID: 37555 RVA: 0x003788CF File Offset: 0x00376ACF
	public static GameUtils.DirEightWay GetDirByNormal(Vector2 _normal)
	{
		_normal.Normalize();
		return GameUtils.GetDirByNormal(new Vector2i(Mathf.RoundToInt(_normal.x), Mathf.RoundToInt(_normal.y)));
	}

	// Token: 0x060092B4 RID: 37556 RVA: 0x003788F8 File Offset: 0x00376AF8
	public static GameUtils.DirEightWay GetDirByNormal(Vector2i _normal)
	{
		for (int i = 0; i < GameUtils.NeighborsEightWay.Count; i++)
		{
			if (GameUtils.NeighborsEightWay[i] == _normal)
			{
				return (GameUtils.DirEightWay)i;
			}
		}
		return GameUtils.DirEightWay.None;
	}

	// Token: 0x060092B5 RID: 37557 RVA: 0x00378934 File Offset: 0x00376B34
	public static GameUtils.DirEightWay GetClosestDirection(float _rotation, bool _limitTo90Degress = false)
	{
		_rotation = MathUtils.Mod(_rotation, 360f);
		if (_limitTo90Degress)
		{
			if (_rotation > 315f || _rotation <= 45f)
			{
				return GameUtils.DirEightWay.N;
			}
			if (_rotation <= 135f)
			{
				return GameUtils.DirEightWay.E;
			}
			if (_rotation <= 225f)
			{
				return GameUtils.DirEightWay.S;
			}
			return GameUtils.DirEightWay.W;
		}
		else
		{
			if ((double)_rotation > 337.5 || (double)_rotation <= 22.5)
			{
				return GameUtils.DirEightWay.N;
			}
			if ((double)_rotation <= 67.5)
			{
				return GameUtils.DirEightWay.NE;
			}
			if ((double)_rotation <= 112.5)
			{
				return GameUtils.DirEightWay.E;
			}
			if ((double)_rotation <= 157.5)
			{
				return GameUtils.DirEightWay.SE;
			}
			if ((double)_rotation <= 202.5)
			{
				return GameUtils.DirEightWay.S;
			}
			if ((double)_rotation <= 247.5)
			{
				return GameUtils.DirEightWay.SW;
			}
			if ((double)_rotation <= 292.5)
			{
				return GameUtils.DirEightWay.W;
			}
			return GameUtils.DirEightWay.NW;
		}
	}

	// Token: 0x060092B6 RID: 37558 RVA: 0x003789F0 File Offset: 0x00376BF0
	public static void DestroyAllChildrenBut(Transform t, string _excluded)
	{
		bool isPlaying = Application.isPlaying;
		int num = 0;
		List<string> list = new List<string>(_excluded.Split(',', StringSplitOptions.None));
		while (t.childCount != num)
		{
			Transform child = t.GetChild(num);
			if (list.Contains(child.name))
			{
				num++;
			}
			else if (isPlaying)
			{
				child.parent = null;
				global::UnityEngine.Object.Destroy(child.gameObject);
			}
			else
			{
				global::UnityEngine.Object.DestroyImmediate(child.gameObject);
			}
		}
	}

	// Token: 0x060092B7 RID: 37559 RVA: 0x00378A60 File Offset: 0x00376C60
	public static void DestroyAllChildrenBut(Transform t, List<string> _excluded)
	{
		bool isPlaying = Application.isPlaying;
		for (int i = t.childCount - 1; i >= 0; i--)
		{
			Transform child = t.GetChild(i);
			if (!_excluded.Contains(child.name))
			{
				if (isPlaying)
				{
					child.SetParent(null, false);
					global::UnityEngine.Object.Destroy(child.gameObject);
				}
				else
				{
					global::UnityEngine.Object.DestroyImmediate(child.gameObject);
				}
			}
		}
	}

	// Token: 0x060092B8 RID: 37560 RVA: 0x00378AC0 File Offset: 0x00376CC0
	public static void DestroyAllChildrenImmediatelyBut(Transform t, List<string> _excluded)
	{
		for (int i = t.childCount - 1; i >= 0; i--)
		{
			Transform child = t.GetChild(i);
			if (!_excluded.Contains(child.name))
			{
				global::UnityEngine.Object.DestroyImmediate(child.gameObject);
			}
		}
	}

	// Token: 0x060092B9 RID: 37561 RVA: 0x00378B01 File Offset: 0x00376D01
	public static void NullThenDestroy<T>(ref T obj) where T : global::UnityEngine.Object
	{
		T t = obj;
		obj = default(T);
		global::UnityEngine.Object.Destroy(t);
	}

	// Token: 0x060092BA RID: 37562 RVA: 0x00378B1C File Offset: 0x00376D1C
	public static void SetMeshVertexAttributes(Mesh mesh, bool compressPosition = true)
	{
		VertexAttributeDescriptor[] vertexAttributes = mesh.GetVertexAttributes();
		GameUtils.ApplyCompressedVertexAttributes(vertexAttributes, compressPosition);
		mesh.SetVertexBufferParams(mesh.vertexCount, vertexAttributes);
	}

	// Token: 0x060092BB RID: 37563 RVA: 0x00378B4C File Offset: 0x00376D4C
	public unsafe static void ApplyCompressedVertexAttributes(Span<VertexAttributeDescriptor> attributes, bool compressPosition = true)
	{
		for (int i = 0; i < attributes.Length; i++)
		{
			VertexAttributeDescriptor vertexAttributeDescriptor = *attributes[i];
			VertexAttribute attribute = vertexAttributeDescriptor.attribute;
			if (attribute == VertexAttribute.Position && !compressPosition)
			{
				vertexAttributeDescriptor.format = VertexAttributeFormat.Float32;
				vertexAttributeDescriptor.dimension = 3;
			}
			else if (attribute == VertexAttribute.Position || attribute == VertexAttribute.Normal)
			{
				vertexAttributeDescriptor.format = VertexAttributeFormat.Float16;
				vertexAttributeDescriptor.dimension = 4;
			}
			else if (attribute == VertexAttribute.Color || attribute == VertexAttribute.Tangent)
			{
				vertexAttributeDescriptor.format = VertexAttributeFormat.Float16;
				vertexAttributeDescriptor.dimension = 4;
			}
			else if (attribute == VertexAttribute.TexCoord0 || attribute == VertexAttribute.TexCoord1 || attribute == VertexAttribute.TexCoord2 || attribute == VertexAttribute.TexCoord3)
			{
				vertexAttributeDescriptor.format = VertexAttributeFormat.Float16;
				vertexAttributeDescriptor.dimension = 2;
			}
			*attributes[i] = vertexAttributeDescriptor;
		}
	}

	// Token: 0x04006D9B RID: 28059
	[PublicizedFrom(EAccessModifier.Private)]
	public static Collider[] overlapBoxHits = new Collider[50];

	// Token: 0x04006D9C RID: 28060
	[PublicizedFrom(EAccessModifier.Private)]
	public static GameRandom random;

	// Token: 0x04006D9D RID: 28061
	[PublicizedFrom(EAccessModifier.Private)]
	public static FastTags<TagGroup.Global> oreTag = FastTags<TagGroup.Global>.Parse("ore");

	// Token: 0x04006D9E RID: 28062
	[PublicizedFrom(EAccessModifier.Private)]
	public static FastTags<TagGroup.Global> cropTag = FastTags<TagGroup.Global>.Parse("crops");

	// Token: 0x04006D9F RID: 28063
	[PublicizedFrom(EAccessModifier.Private)]
	public static FastTags<TagGroup.Global> seedMultiTag = FastTags<TagGroup.Global>.Parse("seedMulti");

	// Token: 0x04006DA0 RID: 28064
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<Vector3> tempVertices = new List<Vector3>(16384);

	// Token: 0x04006DA1 RID: 28065
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<int> tempTriangles = new List<int>(16384);

	// Token: 0x04006DA2 RID: 28066
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<string, string> arguments;

	// Token: 0x04006DA3 RID: 28067
	public static string lastSavedScreenshotFilename;

	// Token: 0x04006DA4 RID: 28068
	public static List<Vector2i> NeighborsEightWay = new List<Vector2i>
	{
		new Vector2i(0, 1),
		new Vector2i(1, 1),
		new Vector2i(1, 0),
		new Vector2i(1, -1),
		new Vector2i(0, -1),
		new Vector2i(-1, -1),
		new Vector2i(-1, 0),
		new Vector2i(-1, 1)
	};

	// Token: 0x020011ED RID: 4589
	public class WorldInfo
	{
		// Token: 0x17001152 RID: 4434
		// (get) Token: 0x060092BE RID: 37566 RVA: 0x00378CD6 File Offset: 0x00376ED6
		public Vector2i WorldSize
		{
			get
			{
				return this.HeightmapSize * this.Scale;
			}
		}

		// Token: 0x060092BF RID: 37567 RVA: 0x00378CEC File Offset: 0x00376EEC
		public WorldInfo(string _name, string _description, string[] _modes, Vector2i _heightmapSize, int _scale, bool _fixedWaterLevel, bool _randomGeneratedWorld, VersionInformation _gameVersionCreated, DynamicProperties _dynamicProperties = null)
		{
			this.Valid = true;
			this.Name = _name;
			this.Description = _description;
			this.Modes = _modes;
			this.HeightmapSize = _heightmapSize;
			this.Scale = _scale;
			this.FixedWaterLevel = _fixedWaterLevel;
			this.RandomGeneratedWorld = _randomGeneratedWorld;
			this.GameVersionCreated = _gameVersionCreated;
			this.DynamicProperties = _dynamicProperties;
		}

		// Token: 0x060092C0 RID: 37568 RVA: 0x00378D4C File Offset: 0x00376F4C
		public void Save(PathAbstractions.AbstractedLocation _worldLocation)
		{
			if (_worldLocation.Type == PathAbstractions.EAbstractedLocationType.None)
			{
				Log.Warning("No world location given");
				return;
			}
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.CreateXmlDeclaration();
			XmlElement xmlElement = xmlDocument.AddXmlElement("MapInfo");
			xmlElement.AddXmlElement("property").SetAttrib("name", "Name").SetAttrib("value", this.Name);
			xmlElement.AddXmlElement("property").SetAttrib("name", "Modes").SetAttrib("value", string.Join(",", this.Modes));
			xmlElement.AddXmlElement("property").SetAttrib("name", "Description").SetAttrib("value", this.Description);
			xmlElement.AddXmlElement("property").SetAttrib("name", "Scale").SetAttrib("value", this.Scale.ToString());
			xmlElement.AddXmlElement("property").SetAttrib("name", "HeightMapSize").SetAttrib("value", this.HeightmapSize.ToString());
			xmlElement.AddXmlElement("property").SetAttrib("name", "FixedWaterLevel").SetAttrib("value", this.FixedWaterLevel.ToString());
			xmlElement.AddXmlElement("property").SetAttrib("name", "RandomGeneratedWorld").SetAttrib("value", this.RandomGeneratedWorld.ToString());
			xmlElement.AddXmlElement("property").SetAttrib("name", "GameVersion").SetAttrib("value", this.GameVersionCreated.SerializableString);
			xmlDocument.SdSave(_worldLocation.FullPath + "/map_info.xml");
		}

		// Token: 0x060092C1 RID: 37569 RVA: 0x00378F28 File Offset: 0x00377128
		public static GameUtils.WorldInfo LoadWorldInfo(PathAbstractions.AbstractedLocation _worldLocation)
		{
			try
			{
				if (_worldLocation.Type == PathAbstractions.EAbstractedLocationType.None)
				{
					return null;
				}
				string text = _worldLocation.FullPath + "/map_info.xml";
				if (!SdFile.Exists(text))
				{
					return null;
				}
				IEnumerable<XElement> enumerable = from s in SdXDocument.Load(text).Elements("MapInfo")
					from p in s.Elements("property")
					select p;
				DynamicProperties dynamicProperties = new DynamicProperties();
				int num = 0;
				if (dynamicProperties.Values.ContainsKey("SchemaVersion"))
				{
					int.TryParse(dynamicProperties.Values["SchemaVersion"], out num);
				}
				foreach (XElement xelement in enumerable)
				{
					XAttribute xattribute = xelement.Attribute("name");
					string text2 = ((xattribute != null) ? xattribute.Value : null);
					if (!string.IsNullOrEmpty(text2))
					{
						if (num >= 1)
						{
							dynamicProperties.Add(xelement, false);
						}
						else if (text2.StartsWith("Generation.", StringComparison.Ordinal))
						{
							int num2 = text2.IndexOf('.');
							string text3 = text2.Substring(0, num2);
							string text4 = text2.Substring(num2 + 1);
							if (!dynamicProperties.Classes.ContainsKey(text3))
							{
								dynamicProperties.Classes[text3] = new DynamicProperties();
							}
							Dictionary<string, string> values = dynamicProperties.Classes[text3].Values;
							string text5 = text4;
							XAttribute xattribute2 = xelement.Attribute("value");
							values[text5] = ((xattribute2 != null) ? xattribute2.Value : null);
						}
						else
						{
							dynamicProperties.Add(xelement, false);
						}
					}
				}
				string text6 = null;
				string text7 = null;
				string[] array = null;
				int num3 = 4096;
				int num4 = 4096;
				int num5 = 1;
				bool flag = false;
				bool flag2 = false;
				VersionInformation versionInformation = new VersionInformation(VersionInformation.EGameReleaseType.Alpha, -1, -1, -1);
				if (dynamicProperties.Values.ContainsKey("Name"))
				{
					text6 = dynamicProperties.Values["Name"];
				}
				if (dynamicProperties.Values.ContainsKey("Modes"))
				{
					array = dynamicProperties.Values["Modes"].Replace(" ", "").Split(',', StringSplitOptions.None);
				}
				if (dynamicProperties.Values.ContainsKey("Description"))
				{
					text7 = Localization.Get(dynamicProperties.Values["Description"], false, null);
				}
				if (dynamicProperties.Values.ContainsKey("Scale"))
				{
					num5 = int.Parse(dynamicProperties.Values["Scale"]);
				}
				if (dynamicProperties.Values.ContainsKey("HeightMapSize"))
				{
					Vector2i vector2i = StringParsers.ParseVector2i(dynamicProperties.Values["HeightMapSize"], ',');
					num3 = vector2i.x;
					num4 = vector2i.y;
				}
				if (dynamicProperties.Values.ContainsKey("FixedWaterLevel"))
				{
					flag = StringParsers.ParseBool(dynamicProperties.Values["FixedWaterLevel"], 0, -1, true);
				}
				if (dynamicProperties.Values.ContainsKey("RandomGeneratedWorld"))
				{
					flag2 = StringParsers.ParseBool(dynamicProperties.Values["RandomGeneratedWorld"], 0, -1, true);
				}
				if (dynamicProperties.Values.ContainsKey("GameVersion") && !VersionInformation.TryParseSerializedString(dynamicProperties.Values["GameVersion"], out versionInformation))
				{
					versionInformation = new VersionInformation(VersionInformation.EGameReleaseType.Alpha, -1, -1, -1);
					Log.Warning("World '" + _worldLocation.Name + "' has an invalid GameVersion value: " + dynamicProperties.Values["GameVersion"]);
				}
				return new GameUtils.WorldInfo(text6, text7, array, new Vector2i(num3, num4), num5, flag, flag2, versionInformation, dynamicProperties);
			}
			catch (Exception ex)
			{
				Log.Error("Error reading WorldInfo for " + (_worldLocation.FullPath ?? "<null>"));
				Log.Exception(ex);
			}
			return null;
		}

		// Token: 0x04006DA5 RID: 28069
		public readonly bool Valid;

		// Token: 0x04006DA6 RID: 28070
		public readonly string Name;

		// Token: 0x04006DA7 RID: 28071
		public readonly string Description;

		// Token: 0x04006DA8 RID: 28072
		public readonly string[] Modes;

		// Token: 0x04006DA9 RID: 28073
		public readonly Vector2i HeightmapSize;

		// Token: 0x04006DAA RID: 28074
		public readonly int Scale;

		// Token: 0x04006DAB RID: 28075
		public readonly bool FixedWaterLevel;

		// Token: 0x04006DAC RID: 28076
		public readonly bool RandomGeneratedWorld;

		// Token: 0x04006DAD RID: 28077
		public readonly VersionInformation GameVersionCreated;

		// Token: 0x04006DAE RID: 28078
		public readonly DynamicProperties DynamicProperties;
	}

	// Token: 0x020011EF RID: 4591
	// (Invoke) Token: 0x060092C7 RID: 37575
	public delegate void OutputDelegate(string _text);

	// Token: 0x020011F0 RID: 4592
	public enum EKickReason
	{
		// Token: 0x04006DB3 RID: 28083
		EmptyNameOrPlayerID,
		// Token: 0x04006DB4 RID: 28084
		InvalidUserId,
		// Token: 0x04006DB5 RID: 28085
		DuplicatePlayerID,
		// Token: 0x04006DB6 RID: 28086
		InvalidAuthTicket,
		// Token: 0x04006DB7 RID: 28087
		VersionMismatch,
		// Token: 0x04006DB8 RID: 28088
		PlayerLimitExceeded,
		// Token: 0x04006DB9 RID: 28089
		Banned,
		// Token: 0x04006DBA RID: 28090
		NotOnWhitelist,
		// Token: 0x04006DBB RID: 28091
		PlatformAuthenticationBeginFailed,
		// Token: 0x04006DBC RID: 28092
		PlatformAuthenticationFailed,
		// Token: 0x04006DBD RID: 28093
		ManualKick,
		// Token: 0x04006DBE RID: 28094
		EacViolation,
		// Token: 0x04006DBF RID: 28095
		EacBan,
		// Token: 0x04006DC0 RID: 28096
		PlayerLimitExceededNonVIP,
		// Token: 0x04006DC1 RID: 28097
		GameStillLoading,
		// Token: 0x04006DC2 RID: 28098
		GamePaused,
		// Token: 0x04006DC3 RID: 28099
		ModDecision,
		// Token: 0x04006DC4 RID: 28100
		FriendsOnly,
		// Token: 0x04006DC5 RID: 28101
		UnknownNetPackage,
		// Token: 0x04006DC6 RID: 28102
		EncryptionFailure,
		// Token: 0x04006DC7 RID: 28103
		UnsupportedPlatform,
		// Token: 0x04006DC8 RID: 28104
		CrossPlatformAuthenticationBeginFailed,
		// Token: 0x04006DC9 RID: 28105
		CrossPlatformAuthenticationFailed,
		// Token: 0x04006DCA RID: 28106
		WrongCrossPlatform,
		// Token: 0x04006DCB RID: 28107
		EosEacViolation,
		// Token: 0x04006DCC RID: 28108
		MultiplayerBlockedForHostAccount,
		// Token: 0x04006DCD RID: 28109
		BadMTUPackets,
		// Token: 0x04006DCE RID: 28110
		CrossplayDisabled,
		// Token: 0x04006DCF RID: 28111
		InternalNetConnectionError,
		// Token: 0x04006DD0 RID: 28112
		InviteOnly,
		// Token: 0x04006DD1 RID: 28113
		SessionClosed,
		// Token: 0x04006DD2 RID: 28114
		PersistentPlayerDataExceeded,
		// Token: 0x04006DD3 RID: 28115
		PlatformPlayerLimitExceeded,
		// Token: 0x04006DD4 RID: 28116
		EncryptionAgreementInvalidSignature,
		// Token: 0x04006DD5 RID: 28117
		EncryptionAgreementError
	}

	// Token: 0x020011F1 RID: 4593
	public struct KickPlayerData
	{
		// Token: 0x060092CA RID: 37578 RVA: 0x0037934C File Offset: 0x0037754C
		public KickPlayerData(GameUtils.EKickReason _kickReason, int _apiResponseEnum = 0, DateTime _banUntil = default(DateTime), string _customReason = "")
		{
			this.reason = _kickReason;
			this.apiResponseEnum = _apiResponseEnum;
			this.banUntil = _banUntil;
			this.customReason = _customReason ?? string.Empty;
		}

		// Token: 0x060092CB RID: 37579 RVA: 0x00379374 File Offset: 0x00377574
		public string LocalizedMessage()
		{
			switch (this.reason)
			{
			case GameUtils.EKickReason.EmptyNameOrPlayerID:
			case GameUtils.EKickReason.InvalidUserId:
			case GameUtils.EKickReason.DuplicatePlayerID:
			case GameUtils.EKickReason.InvalidAuthTicket:
			case GameUtils.EKickReason.NotOnWhitelist:
			case GameUtils.EKickReason.PersistentPlayerDataExceeded:
				return Localization.Get("auth_" + this.reason.ToStringCached<GameUtils.EKickReason>(), false, null);
			case GameUtils.EKickReason.VersionMismatch:
				return string.Format(Localization.Get("auth_VersionMismatch", false, null), Constants.cVersionInformation.LongStringNoBuild, this.customReason);
			case GameUtils.EKickReason.PlayerLimitExceeded:
				return string.Format(Localization.Get("auth_PlayerLimitExceeded", false, null), this.customReason);
			case GameUtils.EKickReason.Banned:
				return string.Format(Localization.Get("auth_Banned", false, null), this.banUntil.ToCultureInvariantString()) + (string.IsNullOrEmpty(this.customReason) ? string.Empty : ("\n" + string.Format(Localization.Get("auth_reason", false, null), this.customReason)));
			case GameUtils.EKickReason.PlatformAuthenticationBeginFailed:
			{
				EBeginUserAuthenticationResult ebeginUserAuthenticationResult = (EBeginUserAuthenticationResult)this.apiResponseEnum;
				if (ebeginUserAuthenticationResult - EBeginUserAuthenticationResult.InvalidTicket <= 4)
				{
					return string.Format(Localization.Get("platformauth_" + ebeginUserAuthenticationResult.ToStringCached<EBeginUserAuthenticationResult>(), false, null), PlatformManager.NativePlatform.PlatformDisplayName);
				}
				return string.Format(Localization.Get("platformauth_unknown", false, null), PlatformManager.NativePlatform.PlatformDisplayName);
			}
			case GameUtils.EKickReason.PlatformAuthenticationFailed:
			{
				EUserAuthenticationResult euserAuthenticationResult = (EUserAuthenticationResult)this.apiResponseEnum;
				if (euserAuthenticationResult - EUserAuthenticationResult.UserNotConnectedToPlatform <= 7)
				{
					return string.Format(Localization.Get("platformauth_" + euserAuthenticationResult.ToStringCached<EUserAuthenticationResult>(), false, null), PlatformManager.NativePlatform.PlatformDisplayName);
				}
				if (euserAuthenticationResult != EUserAuthenticationResult.PublisherIssuedBan)
				{
					return string.Format(Localization.Get("platformauth_unknown", false, null), PlatformManager.NativePlatform.PlatformDisplayName);
				}
				if (this.banUntil == default(DateTime))
				{
					return string.Format(Localization.Get("platformauth_" + euserAuthenticationResult.ToStringCached<EUserAuthenticationResult>(), false, null), PlatformManager.NativePlatform.PlatformDisplayName) + (string.IsNullOrEmpty(this.customReason) ? string.Empty : ("\n" + string.Format(Localization.Get("auth_reason", false, null), Array.Empty<object>())));
				}
				return string.Format("\n" + Localization.Get("auth_Banned", false, null), this.banUntil.ToCultureInvariantString()) + (string.IsNullOrEmpty(this.customReason) ? string.Empty : ("\n" + string.Format(Localization.Get("auth_reason", false, null), this.customReason)));
			}
			case GameUtils.EKickReason.ManualKick:
				return string.Format(Localization.Get("auth_ManualKick", false, null), Array.Empty<object>()) + (string.IsNullOrEmpty(this.customReason) ? string.Empty : ("\n" + string.Format(Localization.Get("auth_reason", false, null), this.customReason)));
			case GameUtils.EKickReason.PlayerLimitExceededNonVIP:
				return string.Format(Localization.Get("auth_PlayerLimitExceededNonVIP", false, null), this.customReason);
			case GameUtils.EKickReason.GameStillLoading:
				return Localization.Get("auth_stillloading", false, null);
			case GameUtils.EKickReason.GamePaused:
				return Localization.Get("auth_gamepaused", false, null);
			case GameUtils.EKickReason.ModDecision:
			{
				string text = Localization.Get("auth_mod", false, null);
				if (!string.IsNullOrEmpty(this.customReason))
				{
					text = text + "\n" + this.customReason;
				}
				return text;
			}
			case GameUtils.EKickReason.FriendsOnly:
				return Localization.Get("auth_friendsonly", false, null);
			case GameUtils.EKickReason.UnknownNetPackage:
				return Localization.Get("auth_unknownnetpackage", false, null);
			case GameUtils.EKickReason.EncryptionFailure:
				return Localization.Get("auth_encryptionfailure", false, null);
			case GameUtils.EKickReason.UnsupportedPlatform:
				return string.Format(Localization.Get("auth_unsupportedplatform", false, null), Localization.Get("platformName" + this.customReason, false, null));
			case GameUtils.EKickReason.CrossPlatformAuthenticationBeginFailed:
			{
				EBeginUserAuthenticationResult ebeginUserAuthenticationResult2 = (EBeginUserAuthenticationResult)this.apiResponseEnum;
				if (ebeginUserAuthenticationResult2 - EBeginUserAuthenticationResult.InvalidTicket <= 4)
				{
					return string.Format(Localization.Get("platformauth_" + ebeginUserAuthenticationResult2.ToStringCached<EBeginUserAuthenticationResult>(), false, null), PlatformManager.CrossplatformPlatform.PlatformDisplayName);
				}
				return string.Format(Localization.Get("platformauth_unknown", false, null), PlatformManager.CrossplatformPlatform.PlatformDisplayName);
			}
			case GameUtils.EKickReason.CrossPlatformAuthenticationFailed:
			{
				EUserAuthenticationResult euserAuthenticationResult2 = (EUserAuthenticationResult)this.apiResponseEnum;
				if (euserAuthenticationResult2 - EUserAuthenticationResult.UserNotConnectedToPlatform <= 8)
				{
					return string.Format(Localization.Get("platformauth_" + euserAuthenticationResult2.ToStringCached<EUserAuthenticationResult>(), false, null), PlatformManager.CrossplatformPlatform.PlatformDisplayName);
				}
				if (euserAuthenticationResult2 != EUserAuthenticationResult.EosTicketFailed)
				{
					return string.Format(Localization.Get("platformauth_unknown", false, null), PlatformManager.CrossplatformPlatform.PlatformDisplayName);
				}
				return string.Format(Localization.Get("platformauth_" + euserAuthenticationResult2.ToStringCached<EUserAuthenticationResult>(), false, null), PlatformManager.CrossplatformPlatform.PlatformDisplayName, this.customReason);
			}
			case GameUtils.EKickReason.WrongCrossPlatform:
				return string.Format(Localization.Get("auth_wrongcrossplatform", false, null), Localization.Get("platformName" + this.customReason, false, null));
			case GameUtils.EKickReason.EosEacViolation:
			{
				AntiCheatCommonClientActionReason antiCheatCommonClientActionReason = (AntiCheatCommonClientActionReason)this.apiResponseEnum;
				if (antiCheatCommonClientActionReason > AntiCheatCommonClientActionReason.PermanentBanned)
				{
					return Localization.Get("eacauth_unknown", false, null);
				}
				string text2 = Localization.Get("eacauth_known_" + ((AntiCheatCommonClientActionReason)this.apiResponseEnum).ToStringCached<AntiCheatCommonClientActionReason>(), false, null);
				if (string.IsNullOrEmpty(this.customReason))
				{
					return string.Format(Localization.Get("eacauth_known", false, null), text2);
				}
				return string.Format(Localization.Get("eacauth_known_with_text", false, null), text2, this.customReason);
			}
			case GameUtils.EKickReason.MultiplayerBlockedForHostAccount:
				return Localization.Get("auth_multiplayerblocked", false, null);
			case GameUtils.EKickReason.BadMTUPackets:
				return Localization.Get("auth_badPackets", false, null);
			case GameUtils.EKickReason.CrossplayDisabled:
				return Localization.Get("auth_crossplaydisabled", false, null);
			case GameUtils.EKickReason.InternalNetConnectionError:
				return Localization.Get("auth_internalnetconnectionerror", false, null);
			case GameUtils.EKickReason.InviteOnly:
				return Localization.Get("auth_inviteOnly", false, null);
			case GameUtils.EKickReason.SessionClosed:
				return Localization.Get("auth_sessionClosed", false, null);
			case GameUtils.EKickReason.PlatformPlayerLimitExceeded:
				return string.Format(Localization.Get("auth_PlatformPlayerLimitExceeded", false, null), this.customReason);
			case GameUtils.EKickReason.EncryptionAgreementInvalidSignature:
				return Localization.Get("auth_encryptionagreementinvalidsignature", false, null);
			case GameUtils.EKickReason.EncryptionAgreementError:
				return Localization.Get("auth_encryptionagreementerror", false, null);
			}
			return Localization.Get("auth_unknown", false, null);
		}

		// Token: 0x060092CC RID: 37580 RVA: 0x00379958 File Offset: 0x00377B58
		public override string ToString()
		{
			switch (this.reason)
			{
			case GameUtils.EKickReason.EmptyNameOrPlayerID:
				return "Empty name or player ID";
			case GameUtils.EKickReason.InvalidUserId:
				return "Invalid SteamID";
			case GameUtils.EKickReason.DuplicatePlayerID:
				return "Duplicate player ID";
			case GameUtils.EKickReason.InvalidAuthTicket:
				return "Invalid authentication ticket";
			case GameUtils.EKickReason.VersionMismatch:
				return "Version mismatch";
			case GameUtils.EKickReason.PlayerLimitExceeded:
				return "Player limit exceeded";
			case GameUtils.EKickReason.Banned:
				return "Banned until: " + this.banUntil.ToCultureInvariantString() + (string.IsNullOrEmpty(this.customReason) ? "" : (", reason: " + this.customReason));
			case GameUtils.EKickReason.NotOnWhitelist:
				return "Not on whitelist";
			case GameUtils.EKickReason.PlatformAuthenticationBeginFailed:
				return "Platform auth failed: " + ((EBeginUserAuthenticationResult)this.apiResponseEnum).ToStringCached<EBeginUserAuthenticationResult>();
			case GameUtils.EKickReason.PlatformAuthenticationFailed:
				return "Platform auth failed: " + ((EUserAuthenticationResult)this.apiResponseEnum).ToStringCached<EUserAuthenticationResult>();
			case GameUtils.EKickReason.ManualKick:
				return "Kick: " + ((this.customReason != null) ? this.customReason : "no reason given");
			case GameUtils.EKickReason.PlayerLimitExceededNonVIP:
				return "Player limit for non VIPs / unreserved slots exceeded";
			case GameUtils.EKickReason.GameStillLoading:
				return "Server is still initializing";
			case GameUtils.EKickReason.GamePaused:
				return "Server is paused";
			case GameUtils.EKickReason.ModDecision:
				return "Denied by mod";
			case GameUtils.EKickReason.FriendsOnly:
				return "Friends Only host";
			case GameUtils.EKickReason.UnknownNetPackage:
				return "Unknown NetPackage";
			case GameUtils.EKickReason.EncryptionFailure:
				return "Encryption failure";
			case GameUtils.EKickReason.UnsupportedPlatform:
				return "Unsupported client platform: " + this.customReason;
			case GameUtils.EKickReason.CrossPlatformAuthenticationBeginFailed:
				return "Cross platform auth failed: " + ((EBeginUserAuthenticationResult)this.apiResponseEnum).ToStringCached<EBeginUserAuthenticationResult>() + (string.IsNullOrEmpty(this.customReason) ? "" : (" - " + this.customReason));
			case GameUtils.EKickReason.CrossPlatformAuthenticationFailed:
				return "Cross platform auth failed: " + ((EUserAuthenticationResult)this.apiResponseEnum).ToStringCached<EUserAuthenticationResult>() + (string.IsNullOrEmpty(this.customReason) ? "" : (" - " + this.customReason));
			case GameUtils.EKickReason.WrongCrossPlatform:
				return "Unsupported client cross platform: " + this.customReason;
			case GameUtils.EKickReason.EosEacViolation:
				return "EOS-ACS violation: " + ((AntiCheatCommonClientActionReason)this.apiResponseEnum).ToStringCached<AntiCheatCommonClientActionReason>() + (string.IsNullOrEmpty(this.customReason) ? "" : (" - " + this.customReason));
			case GameUtils.EKickReason.MultiplayerBlockedForHostAccount:
				return "Multiplayer blocked for host's account";
			case GameUtils.EKickReason.CrossplayDisabled:
				return "Crossplay disabled for host's account";
			case GameUtils.EKickReason.InviteOnly:
				return "Invite Only host";
			case GameUtils.EKickReason.SessionClosed:
				return "Session is Closed";
			case GameUtils.EKickReason.EncryptionAgreementInvalidSignature:
				return "Encryption key agreement authentication invalid";
			case GameUtils.EKickReason.EncryptionAgreementError:
				return "Error while performing encryption key agreement";
			}
			return "Unknown reason";
		}

		// Token: 0x04006DD6 RID: 28118
		public GameUtils.EKickReason reason;

		// Token: 0x04006DD7 RID: 28119
		public int apiResponseEnum;

		// Token: 0x04006DD8 RID: 28120
		public DateTime banUntil;

		// Token: 0x04006DD9 RID: 28121
		public string customReason;
	}

	// Token: 0x020011F2 RID: 4594
	public enum EScreenshotMode
	{
		// Token: 0x04006DDB RID: 28123
		File,
		// Token: 0x04006DDC RID: 28124
		Clipboard,
		// Token: 0x04006DDD RID: 28125
		Both
	}

	// Token: 0x020011F3 RID: 4595
	public enum EPlayerHomeType
	{
		// Token: 0x04006DDF RID: 28127
		None,
		// Token: 0x04006DE0 RID: 28128
		Landclaim,
		// Token: 0x04006DE1 RID: 28129
		Bedroll
	}

	// Token: 0x020011F4 RID: 4596
	public enum DirEightWay : sbyte
	{
		// Token: 0x04006DE3 RID: 28131
		None = -1,
		// Token: 0x04006DE4 RID: 28132
		N,
		// Token: 0x04006DE5 RID: 28133
		NE,
		// Token: 0x04006DE6 RID: 28134
		E,
		// Token: 0x04006DE7 RID: 28135
		SE,
		// Token: 0x04006DE8 RID: 28136
		S,
		// Token: 0x04006DE9 RID: 28137
		SW,
		// Token: 0x04006DEA RID: 28138
		W,
		// Token: 0x04006DEB RID: 28139
		NW,
		// Token: 0x04006DEC RID: 28140
		COUNT
	}
}
