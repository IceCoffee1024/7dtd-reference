using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using PrefabVolumes;
using UnityEngine;

// Token: 0x0200093C RID: 2364
public class PrefabInstance
{
	// Token: 0x1700074F RID: 1871
	// (get) Token: 0x06004548 RID: 17736 RVA: 0x001B67E9 File Offset: 0x001B49E9
	public float RotationAngle
	{
		get
		{
			return (float)((this.rotation & 3) * 90);
		}
	}

	// Token: 0x17000750 RID: 1872
	// (get) Token: 0x06004549 RID: 17737 RVA: 0x001B67F7 File Offset: 0x001B49F7
	public Quaternion RotationAroundY
	{
		get
		{
			return Quaternion.AngleAxis(this.RotationAngle, Vector3.up);
		}
	}

	// Token: 0x0600454A RID: 17738 RVA: 0x001B680C File Offset: 0x001B4A0C
	public PrefabInstance(int _id, PathAbstractions.AbstractedLocation _location, Vector3i _position, byte _rotation, Prefab _bad, int _standaloneBlockSize)
	{
		this.id = _id;
		if (_bad != null)
		{
			_bad.location = _location;
			this.boundingBoxSize = _bad.size;
		}
		this.name = _location.Name + "." + _id.ToString();
		this.location = _location;
		this.boundingBoxPosition = _position;
		this.lastCopiedPrefabPosition = Vector3i.zero;
		this.bPrefabCopiedIntoWorld = false;
		this.lastCopiedRotation = _rotation;
		this.rotation = _rotation;
		this.prefab = _bad;
		this.standaloneBlockSize = _standaloneBlockSize;
	}

	// Token: 0x0600454B RID: 17739 RVA: 0x001B68E0 File Offset: 0x001B4AE0
	public Bounds GetAABB()
	{
		return BoundsUtils.BoundsForMinMax((float)this.boundingBoxPosition.x, (float)this.boundingBoxPosition.y, (float)this.boundingBoxPosition.z, (float)(this.boundingBoxPosition.x + this.boundingBoxSize.x), (float)(this.boundingBoxPosition.y + this.boundingBoxSize.y), (float)(this.boundingBoxPosition.z + this.boundingBoxSize.z));
	}

	// Token: 0x0600454C RID: 17740 RVA: 0x001B6960 File Offset: 0x001B4B60
	public Vector2 GetCenterXZ()
	{
		return new Vector2((float)this.boundingBoxPosition.x + (float)this.boundingBoxSize.x * 0.5f, (float)this.boundingBoxPosition.z + (float)this.boundingBoxSize.z * 0.5f);
	}

	// Token: 0x0600454D RID: 17741 RVA: 0x001B69B0 File Offset: 0x001B4BB0
	public bool IsBBInSyncWithPrefab()
	{
		return this.bPrefabCopiedIntoWorld && this.lastCopiedPrefabPosition.Equals(this.boundingBoxPosition) && this.prefab.size.Equals(this.boundingBoxSize) && this.lastCopiedRotation == this.rotation;
	}

	// Token: 0x0600454E RID: 17742 RVA: 0x001B6A00 File Offset: 0x001B4C00
	public void CopyIntoWorld(World _world, bool _copyEntities, bool _bOverwriteExistingBlocks, FastTags<TagGroup.Global> _tags)
	{
		if (this.lastCopiedRotation != this.rotation)
		{
			if (this.lastCopiedRotation < this.rotation)
			{
				int num = (int)(this.rotation - this.lastCopiedRotation);
				this.prefab.RotateY(true, num);
			}
			else
			{
				int num2 = (int)(this.lastCopiedRotation - this.rotation);
				this.prefab.RotateY(false, num2);
			}
			this.lastCopiedRotation = this.rotation;
			this.UpdateBoundingBoxPosAndScale(this.boundingBoxPosition, this.prefab.size, true);
		}
		this.prefab.CopyIntoLocal(_world.ChunkCache, this.boundingBoxPosition, _bOverwriteExistingBlocks, true, _tags);
		if (_copyEntities)
		{
			bool flag = _world.IsEditor() || GameStats.GetBool(EnumGameStats.IsSpawnEnemies);
			this.entityInstanceIds.Clear();
			this.prefab.CopyEntitiesIntoWorld(_world, this.boundingBoxPosition, this.entityInstanceIds, flag);
		}
		this.lastCopiedPrefabPosition = this.boundingBoxPosition;
		this.bPrefabCopiedIntoWorld = true;
	}

	// Token: 0x0600454F RID: 17743 RVA: 0x001B6AF0 File Offset: 0x001B4CF0
	public static void RefreshSwitchesInContainingPoi(Quest _q)
	{
		if (GameManager.Instance.World.IsEditor())
		{
			return;
		}
		Vector3 vector;
		if (!_q.GetPositionData(out vector, Quest.PositionDataTypes.POIPosition))
		{
			return;
		}
		World world = GameManager.Instance.World;
		PrefabInstance prefabAtPosition = GameManager.Instance.GetDynamicPrefabDecorator().GetPrefabAtPosition(vector, null, null);
		if (prefabAtPosition == null)
		{
			return;
		}
		for (int i = 0; i < prefabAtPosition.boundingBoxSize.z; i++)
		{
			for (int j = 0; j < prefabAtPosition.boundingBoxSize.x; j++)
			{
				for (int k = 0; k < prefabAtPosition.boundingBoxSize.y; k++)
				{
					Vector3i vector3i = World.worldToBlockPos(prefabAtPosition.boundingBoxPosition + new Vector3i(j, k, i));
					BlockValue block = world.GetBlock(vector3i);
					if (block.Block is BlockActivateSwitch || block.Block is BlockActivateSingle)
					{
						block.Block.Refresh(world, null, vector3i, block);
					}
				}
			}
		}
	}

	// Token: 0x06004550 RID: 17744 RVA: 0x001B6BFC File Offset: 0x001B4DFC
	public static void RefreshTriggersInContainingPoi(Vector3 _v)
	{
		if (!GameManager.Instance.World.IsEditor())
		{
			PrefabInstance prefabAtPosition = GameManager.Instance.GetDynamicPrefabDecorator().GetPrefabAtPosition(_v, null, null);
			if (prefabAtPosition != null)
			{
				GameManager.Instance.World.triggerManager.RefreshTriggers(prefabAtPosition, prefabAtPosition.LastRefreshType);
			}
		}
	}

	// Token: 0x06004551 RID: 17745 RVA: 0x001B6C5C File Offset: 0x001B4E5C
	public void CleanFromWorld(World _world, bool _bRemoveEntities)
	{
		if (!this.bPrefabCopiedIntoWorld)
		{
			return;
		}
		BlockTools.ClearRPC(_world, this.lastCopiedPrefabPosition, this.prefab.size.x, this.prefab.size.y, this.prefab.size.z, true);
		if (_bRemoveEntities)
		{
			List<int> list = new List<int>();
			for (int i = 0; i < this.entityInstanceIds.Count; i++)
			{
				int num = this.entityInstanceIds[i];
				Entity entity = _world.GetEntity(num);
				if (entity != null && !entity.IsDead())
				{
					_world.RemoveEntity(num, EnumRemoveEntityReason.Unloaded);
				}
				else
				{
					list.Add(num);
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				this.entityInstanceIds.Remove(list[j]);
			}
		}
		this.lastCopiedPrefabPosition = Vector3i.zero;
		this.bPrefabCopiedIntoWorld = false;
	}

	// Token: 0x06004552 RID: 17746 RVA: 0x001B6D41 File Offset: 0x001B4F41
	public void AddSleeperVolume(SleeperVolume _volume)
	{
		if (!this.sleeperVolumes.Contains(_volume))
		{
			this.sleeperVolumes.Add(_volume);
		}
	}

	// Token: 0x06004553 RID: 17747 RVA: 0x001B6D5D File Offset: 0x001B4F5D
	public void AddTriggerVolume(TriggerVolume _volume)
	{
		if (!this.triggerVolumes.Contains(_volume))
		{
			this.triggerVolumes.Add(_volume);
		}
	}

	// Token: 0x06004554 RID: 17748 RVA: 0x001B6D79 File Offset: 0x001B4F79
	public void AddWallVolume(WallVolume _volume)
	{
		if (!this.wallVolumes.Contains(_volume))
		{
			this.wallVolumes.Add(_volume);
		}
	}

	// Token: 0x06004555 RID: 17749 RVA: 0x001B6D98 File Offset: 0x001B4F98
	public void ResizeBoundingBox(Vector3i _deltaVec)
	{
		Vector3i vector3i = this.boundingBoxSize + _deltaVec;
		if (vector3i.x <= 1)
		{
			vector3i.x = 1;
		}
		if (vector3i.y <= 1)
		{
			vector3i.y = 1;
		}
		if (vector3i.z <= 1)
		{
			vector3i.z = 1;
		}
		this.UpdateBoundingBoxPosAndScale(this.boundingBoxPosition, vector3i, true);
	}

	// Token: 0x06004556 RID: 17750 RVA: 0x001B6DF3 File Offset: 0x001B4FF3
	public void MoveBoundingBox(Vector3i _deltaVec)
	{
		this.UpdateBoundingBoxPosAndScale(this.boundingBoxPosition + _deltaVec, this.boundingBoxSize, true);
	}

	// Token: 0x06004557 RID: 17751 RVA: 0x001B6E0E File Offset: 0x001B500E
	public void SetBoundingBoxPosition(Vector3i _position)
	{
		this.UpdateBoundingBoxPosAndScale(_position, this.boundingBoxSize, true);
	}

	// Token: 0x06004558 RID: 17752 RVA: 0x001B6E1E File Offset: 0x001B501E
	public void SetBoundingBoxSize(World _world, Vector3i _size)
	{
		this.UpdateBoundingBoxPosAndScale(this.boundingBoxPosition, _size, true);
	}

	// Token: 0x06004559 RID: 17753 RVA: 0x001B6E30 File Offset: 0x001B5030
	public void CreateBoundingBox(bool _alsoCreateOtherBoxes = true)
	{
		SelectionBox selectionBox = SelectionBoxManager.Instance.CategoryDynamicPrefab.AddBox(this.name, this.boundingBoxPosition, this.boundingBoxSize, true, true);
		selectionBox.FacingDirection = (float)(-(float)((this.prefab.rotationToFaceNorth + (int)this.rotation) % 4) * 90);
		selectionBox.UserData = this;
		selectionBox.SetCaption(this.prefab.PrefabName);
		if (_alsoCreateOtherBoxes)
		{
			foreach (PrefabVolumeListAbs prefabVolumeListAbs in this.prefab.AllVolumeLists)
			{
				prefabVolumeListAbs.CreateSelectionBoxes(this);
			}
		}
	}

	// Token: 0x0600455A RID: 17754 RVA: 0x001B6EE4 File Offset: 0x001B50E4
	public void UpdateBoundingBoxPosAndScale(Vector3i _pos, Vector3i _size, bool _moveVolumes = true)
	{
		if (_moveVolumes)
		{
			this.prefab.MoveVolumes(this.boundingBoxPosition - _pos);
		}
		this.boundingBoxPosition = _pos;
		this.boundingBoxSize = _size;
		SelectionBox box = this.GetBox();
		box.SetPositionAndSize(this.boundingBoxPosition, this.boundingBoxSize);
		box.FacingDirection = (float)((this.prefab.rotationToFaceNorth + (int)this.rotation) % 4 * 90);
		if (box.FacingDirection == 90f)
		{
			box.FacingDirection = 270f;
		}
		else if (box.FacingDirection == 270f)
		{
			box.FacingDirection = 90f;
		}
		foreach (PrefabVolumeListAbs prefabVolumeListAbs in this.prefab.AllVolumeLists)
		{
			prefabVolumeListAbs.ApplyVolumesToSelectionBoxes(this);
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			DynamicPrefabDecorator dynamicPrefabDecorator = GameManager.Instance.GetDynamicPrefabDecorator();
			if (dynamicPrefabDecorator != null)
			{
				dynamicPrefabDecorator.CallPrefabChangedEvent(this);
			}
		}
	}

	// Token: 0x0600455B RID: 17755 RVA: 0x001B6FEC File Offset: 0x001B51EC
	public SelectionBox GetBox()
	{
		SelectionBox selectionBox;
		SelectionBoxManager.Instance.CategoryDynamicPrefab.TryGetBox(this.name, out selectionBox);
		return selectionBox;
	}

	// Token: 0x0600455C RID: 17756 RVA: 0x001B7014 File Offset: 0x001B5214
	public void RotateAroundY()
	{
		this.rotation = (this.rotation + 1) % 4;
		MathUtils.Swap(ref this.boundingBoxSize.x, ref this.boundingBoxSize.z);
		this.UpdateBoundingBoxPosAndScale(this.boundingBoxPosition, this.boundingBoxSize, true);
	}

	// Token: 0x0600455D RID: 17757 RVA: 0x001B7060 File Offset: 0x001B5260
	public void SetRotation(byte _rotation)
	{
		while (this.rotation != (_rotation & 3))
		{
			this.rotation = (this.rotation + 1) & 3;
			this.prefab.RotateY(false, 1);
			this.boundingBoxSize = this.prefab.size;
		}
	}

	// Token: 0x0600455E RID: 17758 RVA: 0x001B70A0 File Offset: 0x001B52A0
	public bool Overlaps(Chunk _chunk)
	{
		Vector3i worldPosIMax = _chunk.worldPosIMax;
		if (worldPosIMax.x >= this.boundingBoxPosition.x && worldPosIMax.y >= this.boundingBoxPosition.y && worldPosIMax.z >= this.boundingBoxPosition.z)
		{
			Vector3i worldPosIMin = _chunk.worldPosIMin;
			Vector3i vector3i = this.boundingBoxPosition + this.boundingBoxSize;
			return worldPosIMin.x < vector3i.x && worldPosIMin.y < vector3i.y && worldPosIMin.z < vector3i.z;
		}
		return false;
	}

	// Token: 0x0600455F RID: 17759 RVA: 0x001B7134 File Offset: 0x001B5334
	public bool Overlaps(Vector3 _pos, float _expandBounds = 0f)
	{
		Bounds aabb = this.GetAABB();
		aabb.Expand(_expandBounds);
		Vector3 max = aabb.max;
		Vector3 min = aabb.min;
		return _pos.x <= max.x && _pos.x >= min.x && _pos.y <= max.y && _pos.y >= min.y && _pos.z <= max.z && _pos.z >= min.z;
	}

	// Token: 0x06004560 RID: 17760 RVA: 0x001B71BC File Offset: 0x001B53BC
	public bool IsWithinInfoArea(Vector3 _pos)
	{
		if (this.prefab.InfoVolumeList.Count == 0)
		{
			return true;
		}
		for (int i = 0; i < this.prefab.InfoVolumeList.Count; i++)
		{
			PrefabInfoVolume prefabInfoVolume = this.prefab.InfoVolumeList[i];
			Vector3i vector3i = this.boundingBoxPosition + prefabInfoVolume.startPos;
			if ((float)(vector3i.x - 1) <= _pos.x && _pos.x <= (float)(vector3i.x + prefabInfoVolume.size.x + 1) && (float)(vector3i.y - 1) <= _pos.y && _pos.y <= (float)(vector3i.y + prefabInfoVolume.size.y + 1) && (float)(vector3i.z - 1) <= _pos.z && _pos.z <= (float)(vector3i.z + prefabInfoVolume.size.z + 1))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004561 RID: 17761 RVA: 0x001B72B8 File Offset: 0x001B54B8
	public void CopyIntoChunk(World _world, Chunk _chunk, bool _bForceOverwriteBlocks = false, FastTags<TagGroup.Global> _questTags = default(FastTags<TagGroup.Global>))
	{
		this.prefab.CopyBlocksIntoChunkNoEntities(_world, _chunk, this.boundingBoxPosition, _bForceOverwriteBlocks, _questTags);
		bool flag = _world.IsEditor() || GameStats.GetBool(EnumGameStats.IsSpawnEnemies);
		this.prefab.CopyEntitiesIntoChunkStub(_chunk, this.boundingBoxPosition, this.entityInstanceIds, flag);
		this.lastCopiedPrefabPosition = this.boundingBoxPosition;
		this.bPrefabCopiedIntoWorld = true;
	}

	// Token: 0x06004562 RID: 17762 RVA: 0x001B731C File Offset: 0x001B551C
	public HashSetLong GetOccupiedChunks()
	{
		if (this.occupiedChunks != null)
		{
			return this.occupiedChunks;
		}
		this.occupiedChunks = new HashSetLong();
		int num = World.toChunkXZ(this.boundingBoxPosition.x);
		int num2 = World.toChunkXZ(this.boundingBoxPosition.x + this.boundingBoxSize.x);
		int num3 = World.toChunkXZ(this.boundingBoxPosition.z);
		int num4 = World.toChunkXZ(this.boundingBoxPosition.z + this.boundingBoxSize.z);
		for (int i = num; i <= num2; i++)
		{
			for (int j = num3; j <= num4; j++)
			{
				this.occupiedChunks.Add(WorldChunkCache.MakeChunkKey(i, j));
			}
		}
		return this.occupiedChunks;
	}

	// Token: 0x06004563 RID: 17763 RVA: 0x001B73D3 File Offset: 0x001B55D3
	public IEnumerator ResetBlocksAndRebuild(World _world, FastTags<TagGroup.Global> _questTags, long _initialChunkKey = 0L)
	{
		PrefabInstance.<>c__DisplayClass52_0 CS$<>8__locals1;
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1._world = _world;
		CS$<>8__locals1._questTags = _questTags;
		this.LastRefreshType = CS$<>8__locals1._questTags;
		CS$<>8__locals1.cc = CS$<>8__locals1._world.ChunkCache;
		HashSetLong allChunks = this.GetOccupiedChunks();
		LockManager.Instance.ForceUnlockByChunk(allChunks);
		CS$<>8__locals1.cc.ClearStabilityForChunks(allChunks);
		this.prefab.DestroyAllMultiblocks(CS$<>8__locals1.cc, this.boundingBoxPosition);
		long[] neighbors = new long[4];
		CS$<>8__locals1.teToRemove = new List<TileEntity>(10);
		HashSetLong seen = new HashSetLong();
		HashSetLong copied = new HashSetLong();
		HashSetLong regenerated = new HashSetLong();
		Queue<long> chunksToCopy = new Queue<long>();
		Queue<ValueTuple<long, long>> chunksToRegen = new Queue<ValueTuple<long, long>>();
		if (_initialChunkKey == 0L || !allChunks.Contains(_initialChunkKey))
		{
			_initialChunkKey = allChunks.First<long>();
		}
		chunksToCopy.Enqueue(_initialChunkKey);
		seen.Add(_initialChunkKey);
		while (chunksToCopy.Count > 0)
		{
			long num = chunksToCopy.Dequeue();
			Chunk chunkSync = CS$<>8__locals1.cc.GetChunkSync(num);
			if (chunkSync != null && !this.<ResetBlocksAndRebuild>g__CopyChunk|52_0(num, ref CS$<>8__locals1))
			{
				Log.Error("[PrefabInstance] Chunk not found in cache during rebuild.");
				yield break;
			}
			chunkSync.ResetStability();
			copied.Add(num);
			CS$<>8__locals1.cc.GetNeighborChunkKeys(num, false, ref neighbors);
			long num2 = num;
			foreach (long num3 in neighbors)
			{
				if (allChunks.Contains(num3) && !seen.Contains(num3) && !copied.Contains(num3))
				{
					seen.Add(num3);
					chunksToCopy.Enqueue(num3);
					num2 = num3;
				}
			}
			chunksToRegen.Enqueue(new ValueTuple<long, long>(num2, num));
			if (copied.Contains(chunksToRegen.Peek().Item1))
			{
				long item = chunksToRegen.Dequeue().Item2;
				if (this.<ResetBlocksAndRebuild>g__RegenerateChunk|52_1(item, ref CS$<>8__locals1))
				{
					regenerated.Add(item);
				}
			}
			yield return null;
		}
		if (copied.Count != allChunks.Count)
		{
			Log.Warning("[PrefabInstance] Some chunks were not copied during ResetBlocksAndRebuild. Copying them to world now.");
			foreach (long num4 in allChunks)
			{
				if (!copied.Contains(num4))
				{
					this.<ResetBlocksAndRebuild>g__CopyChunk|52_0(num4, ref CS$<>8__locals1);
				}
			}
		}
		using (HashSetLong.Enumerator enumerator = allChunks.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				long num5 = enumerator.Current;
				if (!regenerated.Contains(num5))
				{
					this.<ResetBlocksAndRebuild>g__RegenerateChunk|52_1(num5, ref CS$<>8__locals1);
				}
			}
			yield break;
		}
		yield break;
	}

	// Token: 0x06004564 RID: 17764 RVA: 0x001B73F7 File Offset: 0x001B55F7
	public GameUtils.EPlayerHomeType CheckForAnyPlayerHome(World _world)
	{
		return GameUtils.CheckForAnyPlayerHome(_world, this.boundingBoxPosition, this.boundingBoxPosition + this.boundingBoxSize);
	}

	// Token: 0x06004565 RID: 17765 RVA: 0x001B7418 File Offset: 0x001B5618
	public bool AddChunksToUncull(World _world, HashSetList<Chunk> _chunksToUncull)
	{
		bool flag = false;
		foreach (long num in this.GetOccupiedChunks())
		{
			Chunk chunkSync = _world.ChunkCache.GetChunkSync(num);
			if (chunkSync != null && chunkSync.IsInternalBlocksCulled && !_chunksToUncull.hashSet.Contains(chunkSync))
			{
				_chunksToUncull.Add(chunkSync);
				flag = true;
			}
		}
		return flag;
	}

	// Token: 0x06004566 RID: 17766 RVA: 0x001B7498 File Offset: 0x001B5698
	public PathAbstractions.AbstractedLocation GetImposterLocation()
	{
		if (this.imposterLookupDone)
		{
			return this.imposterLocation;
		}
		Prefab prefab = this.prefab;
		string text = ((prefab != null) ? prefab.distantPOIOverride : null) ?? this.location.Name;
		this.imposterLocation = PathAbstractions.PrefabImpostersSearchPaths.GetLocation(text, null, null);
		this.imposterLookupDone = true;
		return this.imposterLocation;
	}

	// Token: 0x06004567 RID: 17767 RVA: 0x001B7506 File Offset: 0x001B5706
	public bool Contains(int _entityId)
	{
		return this.entityInstanceIds.Contains(_entityId);
	}

	// Token: 0x06004568 RID: 17768 RVA: 0x001B7514 File Offset: 0x001B5714
	public override bool Equals(object _obj)
	{
		PrefabInstance prefabInstance = _obj as PrefabInstance;
		return prefabInstance != null && this.boundingBoxPosition == prefabInstance.boundingBoxPosition;
	}

	// Token: 0x06004569 RID: 17769 RVA: 0x001B753E File Offset: 0x001B573E
	public override int GetHashCode()
	{
		return this.boundingBoxPosition.GetHashCode();
	}

	// Token: 0x0600456A RID: 17770 RVA: 0x001B7554 File Offset: 0x001B5754
	public override string ToString()
	{
		return string.Concat(new string[]
		{
			"[DynamicPrefabDecorator ",
			this.id.ToString(),
			" ",
			(this.prefab != null) ? this.prefab.PrefabName : string.Empty,
			"]"
		});
	}

	// Token: 0x0600456B RID: 17771 RVA: 0x001B75B0 File Offset: 0x001B57B0
	public void UpdateImposterView()
	{
		if (!GameManager.Instance.IsEditMode() || PrefabEditModeManager.Instance.IsActive())
		{
			return;
		}
		SelectionBox box = this.GetBox();
		if (box == null)
		{
			Log.Error("PrefabInstance has not SelectionBox! (UIV)");
			return;
		}
		Transform transform = box.transform.Find("PrefabImposter");
		if (transform == null)
		{
			ThreadManager.RunCoroutineSync(this.prefab.ToTransform(true, true, true, false, box.transform, "PrefabImposter", new Vector3((float)(-(float)this.boundingBoxSize.x) / 2f, 0.15f, (float)(-(float)this.boundingBoxSize.z) / 2f), DynamicPrefabDecorator.PrefabPreviewLimit));
			transform = box.transform.Find("PrefabImposter");
			this.imposterBaseRotation = this.lastCopiedRotation;
		}
		int num = MathUtils.Mod((int)(this.rotation - this.imposterBaseRotation), 4);
		Vector3 localEulerAngles = transform.localEulerAngles;
		localEulerAngles.y = -90f * (float)num;
		transform.localEulerAngles = localEulerAngles;
		Vector3 localPosition = transform.localPosition;
		localPosition.x = (float)this.boundingBoxSize.x / 2f * (float)((num % 3 == 0) ? (-1) : 1);
		localPosition.z = (float)this.boundingBoxSize.z / 2f * (float)((num < 2) ? (-1) : 1);
		transform.localPosition = localPosition;
		transform.gameObject.SetActive(!this.IsBBInSyncWithPrefab());
	}

	// Token: 0x0600456C RID: 17772 RVA: 0x001B7718 File Offset: 0x001B5918
	public void DestroyImposterView()
	{
		SelectionBox box = this.GetBox();
		if (box == null)
		{
			Log.Error("PrefabInstance has not SelectionBox! (DIV)");
			return;
		}
		Transform transform = box.transform.Find("PrefabImposter");
		if (transform != null)
		{
			global::UnityEngine.Object.DestroyImmediate(transform.gameObject);
		}
	}

	// Token: 0x0600456D RID: 17773 RVA: 0x001B7768 File Offset: 0x001B5968
	public Vector3i GetPositionRelativeToPoi(Vector3i _pos)
	{
		Vector3i vector3i = _pos - this.boundingBoxPosition;
		if ((this.rotation & 1) != 0)
		{
			MathUtils.Swap(ref vector3i.x, ref vector3i.z);
		}
		Vector3i vector3i2;
		switch (this.rotation & 3)
		{
		case 0:
			vector3i2 = vector3i;
			break;
		case 1:
			vector3i2 = new Vector3i(vector3i.x, vector3i.y, this.boundingBoxSize.z - 1 - vector3i.z);
			break;
		case 2:
			vector3i2 = new Vector3i(this.boundingBoxSize.x - 1 - vector3i.x, vector3i.y, this.boundingBoxSize.z - 1 - vector3i.z);
			break;
		case 3:
			vector3i2 = new Vector3i(this.boundingBoxSize.x - 1 - vector3i.x, vector3i.y, vector3i.z);
			break;
		default:
			vector3i2 = vector3i;
			break;
		}
		return vector3i2;
	}

	// Token: 0x0600456E RID: 17774 RVA: 0x001B7854 File Offset: 0x001B5A54
	public Vector3i GetWorldPositionOfPoiOffset(Vector3i _offset)
	{
		Vector3i vector3i = this.boundingBoxSize;
		Vector3i vector3i2;
		switch (this.rotation & 3)
		{
		case 1:
			vector3i2 = new Vector3i(_offset.x, _offset.y, vector3i.z - 1 - _offset.z);
			break;
		case 2:
			vector3i2 = new Vector3i(vector3i.x - 1 - _offset.x, _offset.y, vector3i.z - 1 - _offset.z);
			break;
		case 3:
			vector3i2 = new Vector3i(vector3i.x - 1 - _offset.x, _offset.y, _offset.z);
			break;
		default:
			vector3i2 = _offset;
			break;
		}
		_offset = vector3i2;
		if ((this.rotation & 1) != 0)
		{
			MathUtils.Swap(ref _offset.x, ref _offset.z);
		}
		_offset += this.boundingBoxPosition;
		return _offset;
	}

	// Token: 0x0600456F RID: 17775 RVA: 0x001B792A File Offset: 0x001B5B2A
	public PrefabInstance.Serializable GetSerializable()
	{
		return new PrefabInstance.Serializable(this);
	}

	// Token: 0x06004570 RID: 17776 RVA: 0x001B7934 File Offset: 0x001B5B34
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool <ResetBlocksAndRebuild>g__CopyChunk|52_0(long chunkKey, ref PrefabInstance.<>c__DisplayClass52_0 A_2)
	{
		Chunk chunkSync = A_2.cc.GetChunkSync(chunkKey);
		if (chunkSync == null)
		{
			return false;
		}
		A_2.cc.ChunkPosNeedsRegeneration_DelayedStart();
		chunkSync.ResetWaterDebugHandle();
		chunkSync.ResetWaterSimHandle();
		chunkSync.StopStabilityCalculation = true;
		this.CopyIntoChunk(A_2._world, chunkSync, true, A_2._questTags);
		A_2.cc.ChunkPosNeedsRegeneration_DelayedStop();
		return true;
	}

	// Token: 0x06004571 RID: 17777 RVA: 0x001B7994 File Offset: 0x001B5B94
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool <ResetBlocksAndRebuild>g__RegenerateChunk|52_1(long chunkKey, ref PrefabInstance.<>c__DisplayClass52_0 A_2)
	{
		Chunk chunkSync = A_2.cc.GetChunkSync(chunkKey);
		if (chunkSync == null)
		{
			return false;
		}
		chunkSync.NeedsDecoration = true;
		chunkSync.NeedsLightDecoration = true;
		chunkSync.NeedsLightCalculation = true;
		A_2.teToRemove.Clear();
		List<TileEntity> list = chunkSync.GetTileEntities().list;
		for (int i = list.Count - 1; i >= 0; i--)
		{
			if (!chunkSync.GetBlock(list[i].localChunkPos).Block.HasTileEntity)
			{
				A_2.teToRemove.Add(list[i]);
			}
			else
			{
				Vector3i vector3i = list[i].ToWorldPos();
				if (this.boundingBoxPosition.x <= vector3i.x && this.boundingBoxPosition.y <= vector3i.y && this.boundingBoxPosition.z <= vector3i.z && this.boundingBoxPosition.x + this.boundingBoxSize.x > vector3i.x && this.boundingBoxPosition.y + this.boundingBoxSize.y > vector3i.y && this.boundingBoxPosition.z + this.boundingBoxSize.z > vector3i.z)
				{
					list[i].Reset(A_2._questTags);
				}
			}
		}
		foreach (TileEntity tileEntity in A_2.teToRemove)
		{
			chunkSync.RemoveTileEntity(A_2._world, tileEntity);
		}
		A_2._world.m_ChunkManager.ResendChunksToClients(new HashSetLong { chunkSync.Key });
		return true;
	}

	// Token: 0x04003787 RID: 14215
	public int id;

	// Token: 0x04003788 RID: 14216
	public byte rotation;

	// Token: 0x04003789 RID: 14217
	public byte imposterBaseRotation;

	// Token: 0x0400378A RID: 14218
	public Prefab prefab;

	// Token: 0x0400378B RID: 14219
	public byte lastCopiedRotation;

	// Token: 0x0400378C RID: 14220
	public Vector3i lastCopiedPrefabPosition;

	// Token: 0x0400378D RID: 14221
	public bool bPrefabCopiedIntoWorld;

	// Token: 0x0400378E RID: 14222
	public Vector3i boundingBoxPosition;

	// Token: 0x0400378F RID: 14223
	public Vector3i boundingBoxSize;

	// Token: 0x04003790 RID: 14224
	public string name;

	// Token: 0x04003791 RID: 14225
	public PathAbstractions.AbstractedLocation location;

	// Token: 0x04003792 RID: 14226
	[PublicizedFrom(EAccessModifier.Private)]
	public bool imposterLookupDone;

	// Token: 0x04003793 RID: 14227
	[PublicizedFrom(EAccessModifier.Private)]
	public PathAbstractions.AbstractedLocation imposterLocation = PathAbstractions.AbstractedLocation.None;

	// Token: 0x04003794 RID: 14228
	public int standaloneBlockSize;

	// Token: 0x04003795 RID: 14229
	public float yOffsetOfPrefab;

	// Token: 0x04003796 RID: 14230
	public QuestLockInstance lockInstance;

	// Token: 0x04003797 RID: 14231
	public readonly List<SleeperVolume> sleeperVolumes = new List<SleeperVolume>();

	// Token: 0x04003798 RID: 14232
	public readonly List<TriggerVolume> triggerVolumes = new List<TriggerVolume>();

	// Token: 0x04003799 RID: 14233
	public readonly List<WallVolume> wallVolumes = new List<WallVolume>();

	// Token: 0x0400379A RID: 14234
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<int> entityInstanceIds = new List<int>();

	// Token: 0x0400379B RID: 14235
	public FastTags<TagGroup.Global> LastRefreshType = FastTags<TagGroup.Global>.none;

	// Token: 0x0400379C RID: 14236
	public QuestClass LastQuestClass;

	// Token: 0x0400379D RID: 14237
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSetLong occupiedChunks;

	// Token: 0x0200093D RID: 2365
	public struct Serializable
	{
		// Token: 0x06004572 RID: 17778 RVA: 0x001B7B6C File Offset: 0x001B5D6C
		public Serializable(PooledBinaryReader _br)
		{
			this.id = _br.ReadInt32();
			this.prefabName = _br.ReadString();
			this.position = StreamUtils.ReadVector3i(_br);
			this.rotation = _br.ReadByte();
		}

		// Token: 0x06004573 RID: 17779 RVA: 0x001B7B9E File Offset: 0x001B5D9E
		public Serializable(PrefabInstance _pi)
		{
			this.id = _pi.id;
			this.prefabName = _pi.prefab.PrefabName;
			this.position = _pi.boundingBoxPosition;
			this.rotation = _pi.rotation;
		}

		// Token: 0x06004574 RID: 17780 RVA: 0x001B7BD5 File Offset: 0x001B5DD5
		public void Write(PooledBinaryWriter _bw)
		{
			_bw.Write(this.id);
			_bw.Write(this.prefabName);
			StreamUtils.Write(_bw, this.position);
			_bw.Write(this.rotation);
		}

		// Token: 0x06004575 RID: 17781 RVA: 0x001B7C07 File Offset: 0x001B5E07
		public int GetLength()
		{
			return 17 + ((this.prefabName != null) ? this.prefabName.Length : 0);
		}

		// Token: 0x0400379E RID: 14238
		public int id;

		// Token: 0x0400379F RID: 14239
		public string prefabName;

		// Token: 0x040037A0 RID: 14240
		public Vector3i position;

		// Token: 0x040037A1 RID: 14241
		public byte rotation;
	}
}
