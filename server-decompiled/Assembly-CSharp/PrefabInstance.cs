using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using PrefabVolumes;
using UnityEngine;

// Token: 0x02000952 RID: 2386
public class PrefabInstance
{
	// Token: 0x17000781 RID: 1921
	// (get) Token: 0x0600456A RID: 17770 RVA: 0x001B92C9 File Offset: 0x001B74C9
	public float RotationAngle
	{
		get
		{
			return (float)((this.rotation & 3) * 90);
		}
	}

	// Token: 0x17000782 RID: 1922
	// (get) Token: 0x0600456B RID: 17771 RVA: 0x001B92D7 File Offset: 0x001B74D7
	public Quaternion RotationAroundY
	{
		get
		{
			return Quaternion.AngleAxis(this.RotationAngle, Vector3.up);
		}
	}

	// Token: 0x0600456C RID: 17772 RVA: 0x001B92EC File Offset: 0x001B74EC
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

	// Token: 0x0600456D RID: 17773 RVA: 0x001B93C0 File Offset: 0x001B75C0
	public Bounds GetAABB()
	{
		return BoundsUtils.BoundsForMinMax((float)this.boundingBoxPosition.x, (float)this.boundingBoxPosition.y, (float)this.boundingBoxPosition.z, (float)(this.boundingBoxPosition.x + this.boundingBoxSize.x), (float)(this.boundingBoxPosition.y + this.boundingBoxSize.y), (float)(this.boundingBoxPosition.z + this.boundingBoxSize.z));
	}

	// Token: 0x0600456E RID: 17774 RVA: 0x001B9440 File Offset: 0x001B7640
	public Vector2 GetCenterXZ()
	{
		return new Vector2((float)this.boundingBoxPosition.x + (float)this.boundingBoxSize.x * 0.5f, (float)this.boundingBoxPosition.z + (float)this.boundingBoxSize.z * 0.5f);
	}

	// Token: 0x0600456F RID: 17775 RVA: 0x001B9490 File Offset: 0x001B7690
	public bool IsBBInSyncWithPrefab()
	{
		return this.bPrefabCopiedIntoWorld && this.lastCopiedPrefabPosition.Equals(this.boundingBoxPosition) && this.prefab.size.Equals(this.boundingBoxSize) && this.lastCopiedRotation == this.rotation;
	}

	// Token: 0x06004570 RID: 17776 RVA: 0x001B94E0 File Offset: 0x001B76E0
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

	// Token: 0x06004571 RID: 17777 RVA: 0x001B95D0 File Offset: 0x001B77D0
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

	// Token: 0x06004572 RID: 17778 RVA: 0x001B96DC File Offset: 0x001B78DC
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

	// Token: 0x06004573 RID: 17779 RVA: 0x001B973C File Offset: 0x001B793C
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

	// Token: 0x06004574 RID: 17780 RVA: 0x001B9821 File Offset: 0x001B7A21
	public void AddSleeperVolume(SleeperVolume _volume)
	{
		if (!this.sleeperVolumes.Contains(_volume))
		{
			this.sleeperVolumes.Add(_volume);
		}
	}

	// Token: 0x06004575 RID: 17781 RVA: 0x001B983D File Offset: 0x001B7A3D
	public void AddTriggerVolume(TriggerVolume _volume)
	{
		if (!this.triggerVolumes.Contains(_volume))
		{
			this.triggerVolumes.Add(_volume);
		}
	}

	// Token: 0x06004576 RID: 17782 RVA: 0x001B9859 File Offset: 0x001B7A59
	public void AddWallVolume(WallVolume _volume)
	{
		if (!this.wallVolumes.Contains(_volume))
		{
			this.wallVolumes.Add(_volume);
		}
	}

	// Token: 0x06004577 RID: 17783 RVA: 0x001B9878 File Offset: 0x001B7A78
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

	// Token: 0x06004578 RID: 17784 RVA: 0x001B98D3 File Offset: 0x001B7AD3
	public void MoveBoundingBox(Vector3i _deltaVec)
	{
		this.UpdateBoundingBoxPosAndScale(this.boundingBoxPosition + _deltaVec, this.boundingBoxSize, true);
	}

	// Token: 0x06004579 RID: 17785 RVA: 0x001B98EE File Offset: 0x001B7AEE
	public void SetBoundingBoxPosition(Vector3i _position)
	{
		this.UpdateBoundingBoxPosAndScale(_position, this.boundingBoxSize, true);
	}

	// Token: 0x0600457A RID: 17786 RVA: 0x001B98FE File Offset: 0x001B7AFE
	public void SetBoundingBoxSize(World _world, Vector3i _size)
	{
		this.UpdateBoundingBoxPosAndScale(this.boundingBoxPosition, _size, true);
	}

	// Token: 0x0600457B RID: 17787 RVA: 0x001B9910 File Offset: 0x001B7B10
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

	// Token: 0x0600457C RID: 17788 RVA: 0x001B99C4 File Offset: 0x001B7BC4
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

	// Token: 0x0600457D RID: 17789 RVA: 0x001B9ACC File Offset: 0x001B7CCC
	public SelectionBox GetBox()
	{
		SelectionBox selectionBox;
		SelectionBoxManager.Instance.CategoryDynamicPrefab.TryGetBox(this.name, out selectionBox);
		return selectionBox;
	}

	// Token: 0x0600457E RID: 17790 RVA: 0x001B9AF4 File Offset: 0x001B7CF4
	public void RotateAroundY()
	{
		this.rotation = (this.rotation + 1) % 4;
		MathUtils.Swap(ref this.boundingBoxSize.x, ref this.boundingBoxSize.z);
		this.UpdateBoundingBoxPosAndScale(this.boundingBoxPosition, this.boundingBoxSize, true);
	}

	// Token: 0x0600457F RID: 17791 RVA: 0x001B9B40 File Offset: 0x001B7D40
	public void SetRotation(byte _rotation)
	{
		while (this.rotation != (_rotation & 3))
		{
			this.rotation = (this.rotation + 1) & 3;
			this.prefab.RotateY(false, 1);
			this.boundingBoxSize = this.prefab.size;
		}
	}

	// Token: 0x06004580 RID: 17792 RVA: 0x001B9B80 File Offset: 0x001B7D80
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

	// Token: 0x06004581 RID: 17793 RVA: 0x001B9C14 File Offset: 0x001B7E14
	public bool Overlaps(Vector3 _pos, float _expandBounds = 0f)
	{
		Bounds aabb = this.GetAABB();
		aabb.Expand(_expandBounds);
		Vector3 max = aabb.max;
		Vector3 min = aabb.min;
		return _pos.x <= max.x && _pos.x >= min.x && _pos.y <= max.y && _pos.y >= min.y && _pos.z <= max.z && _pos.z >= min.z;
	}

	// Token: 0x06004582 RID: 17794 RVA: 0x001B9C9C File Offset: 0x001B7E9C
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

	// Token: 0x06004583 RID: 17795 RVA: 0x001B9D98 File Offset: 0x001B7F98
	public void CopyIntoChunk(World _world, Chunk _chunk, bool _bForceOverwriteBlocks = false, FastTags<TagGroup.Global> _questTags = default(FastTags<TagGroup.Global>))
	{
		this.prefab.CopyBlocksIntoChunkNoEntities(_world, _chunk, this.boundingBoxPosition, _bForceOverwriteBlocks, _questTags);
		bool flag = _world.IsEditor() || GameStats.GetBool(EnumGameStats.IsSpawnEnemies);
		this.prefab.CopyEntitiesIntoChunkStub(_chunk, this.boundingBoxPosition, this.entityInstanceIds, flag);
		this.lastCopiedPrefabPosition = this.boundingBoxPosition;
		this.bPrefabCopiedIntoWorld = true;
	}

	// Token: 0x06004584 RID: 17796 RVA: 0x001B9DFC File Offset: 0x001B7FFC
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

	// Token: 0x06004585 RID: 17797 RVA: 0x001B9EB3 File Offset: 0x001B80B3
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

	// Token: 0x06004586 RID: 17798 RVA: 0x001B9ED7 File Offset: 0x001B80D7
	public GameUtils.EPlayerHomeType CheckForAnyPlayerHome(World _world)
	{
		return GameUtils.CheckForAnyPlayerHome(_world, this.boundingBoxPosition, this.boundingBoxPosition + this.boundingBoxSize);
	}

	// Token: 0x06004587 RID: 17799 RVA: 0x001B9EF8 File Offset: 0x001B80F8
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

	// Token: 0x06004588 RID: 17800 RVA: 0x001B9F78 File Offset: 0x001B8178
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

	// Token: 0x06004589 RID: 17801 RVA: 0x001B9FE6 File Offset: 0x001B81E6
	public bool Contains(int _entityId)
	{
		return this.entityInstanceIds.Contains(_entityId);
	}

	// Token: 0x0600458A RID: 17802 RVA: 0x001B9FF4 File Offset: 0x001B81F4
	public override bool Equals(object _obj)
	{
		PrefabInstance prefabInstance = _obj as PrefabInstance;
		return prefabInstance != null && this.boundingBoxPosition == prefabInstance.boundingBoxPosition;
	}

	// Token: 0x0600458B RID: 17803 RVA: 0x001BA01E File Offset: 0x001B821E
	public override int GetHashCode()
	{
		return this.boundingBoxPosition.GetHashCode();
	}

	// Token: 0x0600458C RID: 17804 RVA: 0x001BA034 File Offset: 0x001B8234
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

	// Token: 0x0600458D RID: 17805 RVA: 0x001BA090 File Offset: 0x001B8290
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

	// Token: 0x0600458E RID: 17806 RVA: 0x001BA1F8 File Offset: 0x001B83F8
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

	// Token: 0x0600458F RID: 17807 RVA: 0x001BA248 File Offset: 0x001B8448
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

	// Token: 0x06004590 RID: 17808 RVA: 0x001BA334 File Offset: 0x001B8534
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

	// Token: 0x06004591 RID: 17809 RVA: 0x001BA40A File Offset: 0x001B860A
	public PrefabInstance.Serializable GetSerializable()
	{
		return new PrefabInstance.Serializable(this);
	}

	// Token: 0x06004592 RID: 17810 RVA: 0x001BA412 File Offset: 0x001B8612
	public PrefabInstance.POIMetadata GetPOIMetadata()
	{
		return new PrefabInstance.POIMetadata(this);
	}

	// Token: 0x06004593 RID: 17811 RVA: 0x001BA41C File Offset: 0x001B861C
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

	// Token: 0x06004594 RID: 17812 RVA: 0x001BA47C File Offset: 0x001B867C
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

	// Token: 0x0400380C RID: 14348
	public int id;

	// Token: 0x0400380D RID: 14349
	public byte rotation;

	// Token: 0x0400380E RID: 14350
	public byte imposterBaseRotation;

	// Token: 0x0400380F RID: 14351
	public Prefab prefab;

	// Token: 0x04003810 RID: 14352
	public byte lastCopiedRotation;

	// Token: 0x04003811 RID: 14353
	public Vector3i lastCopiedPrefabPosition;

	// Token: 0x04003812 RID: 14354
	public bool bPrefabCopiedIntoWorld;

	// Token: 0x04003813 RID: 14355
	public Vector3i boundingBoxPosition;

	// Token: 0x04003814 RID: 14356
	public Vector3i boundingBoxSize;

	// Token: 0x04003815 RID: 14357
	public string name;

	// Token: 0x04003816 RID: 14358
	public PathAbstractions.AbstractedLocation location;

	// Token: 0x04003817 RID: 14359
	[PublicizedFrom(EAccessModifier.Private)]
	public bool imposterLookupDone;

	// Token: 0x04003818 RID: 14360
	[PublicizedFrom(EAccessModifier.Private)]
	public PathAbstractions.AbstractedLocation imposterLocation = PathAbstractions.AbstractedLocation.None;

	// Token: 0x04003819 RID: 14361
	public int standaloneBlockSize;

	// Token: 0x0400381A RID: 14362
	public float yOffsetOfPrefab;

	// Token: 0x0400381B RID: 14363
	public QuestLockInstance lockInstance;

	// Token: 0x0400381C RID: 14364
	public readonly List<SleeperVolume> sleeperVolumes = new List<SleeperVolume>();

	// Token: 0x0400381D RID: 14365
	public readonly List<TriggerVolume> triggerVolumes = new List<TriggerVolume>();

	// Token: 0x0400381E RID: 14366
	public readonly List<WallVolume> wallVolumes = new List<WallVolume>();

	// Token: 0x0400381F RID: 14367
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<int> entityInstanceIds = new List<int>();

	// Token: 0x04003820 RID: 14368
	public FastTags<TagGroup.Global> LastRefreshType = FastTags<TagGroup.Global>.none;

	// Token: 0x04003821 RID: 14369
	public QuestClass LastQuestClass;

	// Token: 0x04003822 RID: 14370
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSetLong occupiedChunks;

	// Token: 0x02000953 RID: 2387
	public struct Serializable
	{
		// Token: 0x06004595 RID: 17813 RVA: 0x001BA654 File Offset: 0x001B8854
		public Serializable(PooledBinaryReader _br)
		{
			this.id = _br.ReadInt32();
			this.prefabName = _br.ReadString();
			this.position = StreamUtils.ReadVector3i(_br);
			this.rotation = _br.ReadByte();
		}

		// Token: 0x06004596 RID: 17814 RVA: 0x001BA686 File Offset: 0x001B8886
		public Serializable(PrefabInstance _pi)
		{
			this.id = _pi.id;
			this.prefabName = _pi.prefab.PrefabName;
			this.position = _pi.boundingBoxPosition;
			this.rotation = _pi.rotation;
		}

		// Token: 0x06004597 RID: 17815 RVA: 0x001BA6BD File Offset: 0x001B88BD
		public void Write(PooledBinaryWriter _bw)
		{
			_bw.Write(this.id);
			_bw.Write(this.prefabName);
			StreamUtils.Write(_bw, this.position);
			_bw.Write(this.rotation);
		}

		// Token: 0x06004598 RID: 17816 RVA: 0x001BA6EF File Offset: 0x001B88EF
		public int GetLength()
		{
			return 17 + ((this.prefabName != null) ? this.prefabName.Length : 0);
		}

		// Token: 0x04003823 RID: 14371
		public int id;

		// Token: 0x04003824 RID: 14372
		public string prefabName;

		// Token: 0x04003825 RID: 14373
		public Vector3i position;

		// Token: 0x04003826 RID: 14374
		public byte rotation;
	}

	// Token: 0x02000954 RID: 2388
	public class POIMetadata
	{
		// Token: 0x06004599 RID: 17817 RVA: 0x001BA70C File Offset: 0x001B890C
		public POIMetadata(PrefabInstance _pi)
		{
			Prefab prefab = _pi.prefab;
			this.position = _pi.boundingBoxPosition;
			this.size = _pi.boundingBoxSize;
			this.rotation = _pi.rotation;
			this.tier = prefab.DifficultyTier;
			this.traderArea = prefab.bTraderArea;
			this.prefabName = prefab.PrefabName;
			this.tags = prefab.Tags.ToString();
			this.questTags = prefab.GetQuestTags().ToString();
		}

		// Token: 0x0600459A RID: 17818 RVA: 0x001BA7A4 File Offset: 0x001B89A4
		public POIMetadata(PooledBinaryReader _br)
		{
			this.position = StreamUtils.ReadVector3i(_br);
			this.size = StreamUtils.ReadVector3i(_br);
			this.rotation = _br.ReadByte();
			this.tier = _br.ReadByte();
			this.traderArea = _br.ReadBoolean();
			this.prefabName = _br.ReadString();
			this.tags = _br.ReadString();
			this.questTags = _br.ReadString();
		}

		// Token: 0x0600459B RID: 17819 RVA: 0x001BA818 File Offset: 0x001B8A18
		public void Write(PooledBinaryWriter _bw)
		{
			StreamUtils.Write(_bw, this.position);
			StreamUtils.Write(_bw, this.size);
			_bw.Write(this.rotation);
			_bw.Write(this.tier);
			_bw.Write(this.traderArea);
			_bw.Write(this.prefabName);
			_bw.Write(this.tags);
			_bw.Write(this.questTags);
		}

		// Token: 0x0600459C RID: 17820 RVA: 0x001BA888 File Offset: 0x001B8A88
		public PrefabInstance ToPrefabInstance(int _id)
		{
			Prefab prefab = new Prefab(this.size);
			prefab.DifficultyTier = this.tier;
			prefab.bTraderArea = this.traderArea;
			prefab.Tags = FastTags<TagGroup.Poi>.Parse(this.tags);
			prefab.location = new PathAbstractions.AbstractedLocation(PathAbstractions.EAbstractedLocationType.GameData, this.prefabName, null, null, this.prefabName, ".tts", false, null);
			FastTags<TagGroup.Global> fastTags = FastTags<TagGroup.Global>.Parse(this.questTags);
			if (!fastTags.IsEmpty)
			{
				prefab.ToggleQuestTag(fastTags);
			}
			return new PrefabInstance(_id, prefab.location, this.position, this.rotation, prefab, 0);
		}

		// Token: 0x04003827 RID: 14375
		public Vector3i position;

		// Token: 0x04003828 RID: 14376
		public Vector3i size;

		// Token: 0x04003829 RID: 14377
		public byte rotation;

		// Token: 0x0400382A RID: 14378
		public byte tier;

		// Token: 0x0400382B RID: 14379
		public bool traderArea;

		// Token: 0x0400382C RID: 14380
		public string prefabName;

		// Token: 0x0400382D RID: 14381
		public string tags;

		// Token: 0x0400382E RID: 14382
		public string questTags;
	}
}
