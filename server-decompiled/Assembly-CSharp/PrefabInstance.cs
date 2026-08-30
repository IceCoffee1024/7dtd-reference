using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using PrefabVolumes;
using UnityEngine;

// Token: 0x0200094C RID: 2380
public class PrefabInstance
{
	// Token: 0x17000761 RID: 1889
	// (get) Token: 0x060045C9 RID: 17865 RVA: 0x001B8DC1 File Offset: 0x001B6FC1
	public float RotationAngle
	{
		get
		{
			return (float)((this.rotation & 3) * 90);
		}
	}

	// Token: 0x17000762 RID: 1890
	// (get) Token: 0x060045CA RID: 17866 RVA: 0x001B8DCF File Offset: 0x001B6FCF
	public Quaternion RotationAroundY
	{
		get
		{
			return Quaternion.AngleAxis(this.RotationAngle, Vector3.up);
		}
	}

	// Token: 0x060045CB RID: 17867 RVA: 0x001B8DE4 File Offset: 0x001B6FE4
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

	// Token: 0x060045CC RID: 17868 RVA: 0x001B8EB8 File Offset: 0x001B70B8
	public Bounds GetAABB()
	{
		return BoundsUtils.BoundsForMinMax((float)this.boundingBoxPosition.x, (float)this.boundingBoxPosition.y, (float)this.boundingBoxPosition.z, (float)(this.boundingBoxPosition.x + this.boundingBoxSize.x), (float)(this.boundingBoxPosition.y + this.boundingBoxSize.y), (float)(this.boundingBoxPosition.z + this.boundingBoxSize.z));
	}

	// Token: 0x060045CD RID: 17869 RVA: 0x001B8F38 File Offset: 0x001B7138
	public Vector2 GetCenterXZ()
	{
		return new Vector2((float)this.boundingBoxPosition.x + (float)this.boundingBoxSize.x * 0.5f, (float)this.boundingBoxPosition.z + (float)this.boundingBoxSize.z * 0.5f);
	}

	// Token: 0x060045CE RID: 17870 RVA: 0x001B8F88 File Offset: 0x001B7188
	public bool IsBBInSyncWithPrefab()
	{
		return this.bPrefabCopiedIntoWorld && this.lastCopiedPrefabPosition.Equals(this.boundingBoxPosition) && this.prefab.size.Equals(this.boundingBoxSize) && this.lastCopiedRotation == this.rotation;
	}

	// Token: 0x060045CF RID: 17871 RVA: 0x001B8FD8 File Offset: 0x001B71D8
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

	// Token: 0x060045D0 RID: 17872 RVA: 0x001B90C8 File Offset: 0x001B72C8
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

	// Token: 0x060045D1 RID: 17873 RVA: 0x001B91D4 File Offset: 0x001B73D4
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

	// Token: 0x060045D2 RID: 17874 RVA: 0x001B9234 File Offset: 0x001B7434
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

	// Token: 0x060045D3 RID: 17875 RVA: 0x001B9319 File Offset: 0x001B7519
	public void AddSleeperVolume(SleeperVolume _volume)
	{
		if (!this.sleeperVolumes.Contains(_volume))
		{
			this.sleeperVolumes.Add(_volume);
		}
	}

	// Token: 0x060045D4 RID: 17876 RVA: 0x001B9335 File Offset: 0x001B7535
	public void AddTriggerVolume(TriggerVolume _volume)
	{
		if (!this.triggerVolumes.Contains(_volume))
		{
			this.triggerVolumes.Add(_volume);
		}
	}

	// Token: 0x060045D5 RID: 17877 RVA: 0x001B9351 File Offset: 0x001B7551
	public void AddWallVolume(WallVolume _volume)
	{
		if (!this.wallVolumes.Contains(_volume))
		{
			this.wallVolumes.Add(_volume);
		}
	}

	// Token: 0x060045D6 RID: 17878 RVA: 0x001B9370 File Offset: 0x001B7570
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

	// Token: 0x060045D7 RID: 17879 RVA: 0x001B93CB File Offset: 0x001B75CB
	public void MoveBoundingBox(Vector3i _deltaVec)
	{
		this.UpdateBoundingBoxPosAndScale(this.boundingBoxPosition + _deltaVec, this.boundingBoxSize, true);
	}

	// Token: 0x060045D8 RID: 17880 RVA: 0x001B93E6 File Offset: 0x001B75E6
	public void SetBoundingBoxPosition(Vector3i _position)
	{
		this.UpdateBoundingBoxPosAndScale(_position, this.boundingBoxSize, true);
	}

	// Token: 0x060045D9 RID: 17881 RVA: 0x001B93F6 File Offset: 0x001B75F6
	public void SetBoundingBoxSize(World _world, Vector3i _size)
	{
		this.UpdateBoundingBoxPosAndScale(this.boundingBoxPosition, _size, true);
	}

	// Token: 0x060045DA RID: 17882 RVA: 0x001B9408 File Offset: 0x001B7608
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

	// Token: 0x060045DB RID: 17883 RVA: 0x001B94BC File Offset: 0x001B76BC
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

	// Token: 0x060045DC RID: 17884 RVA: 0x001B95C4 File Offset: 0x001B77C4
	public SelectionBox GetBox()
	{
		SelectionBox selectionBox;
		SelectionBoxManager.Instance.CategoryDynamicPrefab.TryGetBox(this.name, out selectionBox);
		return selectionBox;
	}

	// Token: 0x060045DD RID: 17885 RVA: 0x001B95EC File Offset: 0x001B77EC
	public void RotateAroundY()
	{
		this.rotation = (this.rotation + 1) % 4;
		MathUtils.Swap(ref this.boundingBoxSize.x, ref this.boundingBoxSize.z);
		this.UpdateBoundingBoxPosAndScale(this.boundingBoxPosition, this.boundingBoxSize, true);
	}

	// Token: 0x060045DE RID: 17886 RVA: 0x001B9638 File Offset: 0x001B7838
	public void SetRotation(byte _rotation)
	{
		while (this.rotation != (_rotation & 3))
		{
			this.rotation = (this.rotation + 1) & 3;
			this.prefab.RotateY(false, 1);
			this.boundingBoxSize = this.prefab.size;
		}
	}

	// Token: 0x060045DF RID: 17887 RVA: 0x001B9678 File Offset: 0x001B7878
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

	// Token: 0x060045E0 RID: 17888 RVA: 0x001B970C File Offset: 0x001B790C
	public bool Overlaps(Vector3 _pos, float _expandBounds = 0f)
	{
		Bounds aabb = this.GetAABB();
		aabb.Expand(_expandBounds);
		Vector3 max = aabb.max;
		Vector3 min = aabb.min;
		return _pos.x <= max.x && _pos.x >= min.x && _pos.y <= max.y && _pos.y >= min.y && _pos.z <= max.z && _pos.z >= min.z;
	}

	// Token: 0x060045E1 RID: 17889 RVA: 0x001B9794 File Offset: 0x001B7994
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

	// Token: 0x060045E2 RID: 17890 RVA: 0x001B9890 File Offset: 0x001B7A90
	public void CopyIntoChunk(World _world, Chunk _chunk, bool _bForceOverwriteBlocks = false, FastTags<TagGroup.Global> _questTags = default(FastTags<TagGroup.Global>))
	{
		this.prefab.CopyBlocksIntoChunkNoEntities(_world, _chunk, this.boundingBoxPosition, _bForceOverwriteBlocks, _questTags);
		bool flag = _world.IsEditor() || GameStats.GetBool(EnumGameStats.IsSpawnEnemies);
		this.prefab.CopyEntitiesIntoChunkStub(_chunk, this.boundingBoxPosition, this.entityInstanceIds, flag);
		this.lastCopiedPrefabPosition = this.boundingBoxPosition;
		this.bPrefabCopiedIntoWorld = true;
	}

	// Token: 0x060045E3 RID: 17891 RVA: 0x001B98F4 File Offset: 0x001B7AF4
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

	// Token: 0x060045E4 RID: 17892 RVA: 0x001B99AB File Offset: 0x001B7BAB
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

	// Token: 0x060045E5 RID: 17893 RVA: 0x001B99CF File Offset: 0x001B7BCF
	public GameUtils.EPlayerHomeType CheckForAnyPlayerHome(World _world)
	{
		return GameUtils.CheckForAnyPlayerHome(_world, this.boundingBoxPosition, this.boundingBoxPosition + this.boundingBoxSize);
	}

	// Token: 0x060045E6 RID: 17894 RVA: 0x001B99F0 File Offset: 0x001B7BF0
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

	// Token: 0x060045E7 RID: 17895 RVA: 0x001B9A70 File Offset: 0x001B7C70
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

	// Token: 0x060045E8 RID: 17896 RVA: 0x001B9ADE File Offset: 0x001B7CDE
	public bool Contains(int _entityId)
	{
		return this.entityInstanceIds.Contains(_entityId);
	}

	// Token: 0x060045E9 RID: 17897 RVA: 0x001B9AEC File Offset: 0x001B7CEC
	public override bool Equals(object _obj)
	{
		PrefabInstance prefabInstance = _obj as PrefabInstance;
		return prefabInstance != null && this.boundingBoxPosition == prefabInstance.boundingBoxPosition;
	}

	// Token: 0x060045EA RID: 17898 RVA: 0x001B9B16 File Offset: 0x001B7D16
	public override int GetHashCode()
	{
		return this.boundingBoxPosition.GetHashCode();
	}

	// Token: 0x060045EB RID: 17899 RVA: 0x001B9B2C File Offset: 0x001B7D2C
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

	// Token: 0x060045EC RID: 17900 RVA: 0x001B9B88 File Offset: 0x001B7D88
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

	// Token: 0x060045ED RID: 17901 RVA: 0x001B9CF0 File Offset: 0x001B7EF0
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

	// Token: 0x060045EE RID: 17902 RVA: 0x001B9D40 File Offset: 0x001B7F40
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

	// Token: 0x060045EF RID: 17903 RVA: 0x001B9E2C File Offset: 0x001B802C
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

	// Token: 0x060045F0 RID: 17904 RVA: 0x001B9F02 File Offset: 0x001B8102
	public PrefabInstance.Serializable GetSerializable()
	{
		return new PrefabInstance.Serializable(this);
	}

	// Token: 0x060045F1 RID: 17905 RVA: 0x001B9F0A File Offset: 0x001B810A
	public PrefabInstance.POIMetadata GetPOIMetadata()
	{
		return new PrefabInstance.POIMetadata(this);
	}

	// Token: 0x060045F2 RID: 17906 RVA: 0x001B9F14 File Offset: 0x001B8114
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

	// Token: 0x060045F3 RID: 17907 RVA: 0x001B9F74 File Offset: 0x001B8174
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

	// Token: 0x04003810 RID: 14352
	public int id;

	// Token: 0x04003811 RID: 14353
	public byte rotation;

	// Token: 0x04003812 RID: 14354
	public byte imposterBaseRotation;

	// Token: 0x04003813 RID: 14355
	public Prefab prefab;

	// Token: 0x04003814 RID: 14356
	public byte lastCopiedRotation;

	// Token: 0x04003815 RID: 14357
	public Vector3i lastCopiedPrefabPosition;

	// Token: 0x04003816 RID: 14358
	public bool bPrefabCopiedIntoWorld;

	// Token: 0x04003817 RID: 14359
	public Vector3i boundingBoxPosition;

	// Token: 0x04003818 RID: 14360
	public Vector3i boundingBoxSize;

	// Token: 0x04003819 RID: 14361
	public string name;

	// Token: 0x0400381A RID: 14362
	public PathAbstractions.AbstractedLocation location;

	// Token: 0x0400381B RID: 14363
	[PublicizedFrom(EAccessModifier.Private)]
	public bool imposterLookupDone;

	// Token: 0x0400381C RID: 14364
	[PublicizedFrom(EAccessModifier.Private)]
	public PathAbstractions.AbstractedLocation imposterLocation = PathAbstractions.AbstractedLocation.None;

	// Token: 0x0400381D RID: 14365
	public int standaloneBlockSize;

	// Token: 0x0400381E RID: 14366
	public float yOffsetOfPrefab;

	// Token: 0x0400381F RID: 14367
	public QuestLockInstance lockInstance;

	// Token: 0x04003820 RID: 14368
	public readonly List<SleeperVolume> sleeperVolumes = new List<SleeperVolume>();

	// Token: 0x04003821 RID: 14369
	public readonly List<TriggerVolume> triggerVolumes = new List<TriggerVolume>();

	// Token: 0x04003822 RID: 14370
	public readonly List<WallVolume> wallVolumes = new List<WallVolume>();

	// Token: 0x04003823 RID: 14371
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<int> entityInstanceIds = new List<int>();

	// Token: 0x04003824 RID: 14372
	public FastTags<TagGroup.Global> LastRefreshType = FastTags<TagGroup.Global>.none;

	// Token: 0x04003825 RID: 14373
	public QuestClass LastQuestClass;

	// Token: 0x04003826 RID: 14374
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSetLong occupiedChunks;

	// Token: 0x0200094D RID: 2381
	public struct Serializable
	{
		// Token: 0x060045F4 RID: 17908 RVA: 0x001BA14C File Offset: 0x001B834C
		public Serializable(PooledBinaryReader _br)
		{
			this.id = _br.ReadInt32();
			this.prefabName = _br.ReadString();
			this.position = StreamUtils.ReadVector3i(_br);
			this.rotation = _br.ReadByte();
		}

		// Token: 0x060045F5 RID: 17909 RVA: 0x001BA17E File Offset: 0x001B837E
		public Serializable(PrefabInstance _pi)
		{
			this.id = _pi.id;
			this.prefabName = _pi.prefab.PrefabName;
			this.position = _pi.boundingBoxPosition;
			this.rotation = _pi.rotation;
		}

		// Token: 0x060045F6 RID: 17910 RVA: 0x001BA1B5 File Offset: 0x001B83B5
		public void Write(PooledBinaryWriter _bw)
		{
			_bw.Write(this.id);
			_bw.Write(this.prefabName);
			StreamUtils.Write(_bw, this.position);
			_bw.Write(this.rotation);
		}

		// Token: 0x060045F7 RID: 17911 RVA: 0x001BA1E7 File Offset: 0x001B83E7
		public int GetLength()
		{
			return 17 + ((this.prefabName != null) ? this.prefabName.Length : 0);
		}

		// Token: 0x04003827 RID: 14375
		public int id;

		// Token: 0x04003828 RID: 14376
		public string prefabName;

		// Token: 0x04003829 RID: 14377
		public Vector3i position;

		// Token: 0x0400382A RID: 14378
		public byte rotation;
	}

	// Token: 0x0200094E RID: 2382
	public class POIMetadata
	{
		// Token: 0x060045F8 RID: 17912 RVA: 0x001BA204 File Offset: 0x001B8404
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

		// Token: 0x060045F9 RID: 17913 RVA: 0x001BA29C File Offset: 0x001B849C
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

		// Token: 0x060045FA RID: 17914 RVA: 0x001BA310 File Offset: 0x001B8510
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

		// Token: 0x060045FB RID: 17915 RVA: 0x001BA380 File Offset: 0x001B8580
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

		// Token: 0x0400382B RID: 14379
		public Vector3i position;

		// Token: 0x0400382C RID: 14380
		public Vector3i size;

		// Token: 0x0400382D RID: 14381
		public byte rotation;

		// Token: 0x0400382E RID: 14382
		public byte tier;

		// Token: 0x0400382F RID: 14383
		public bool traderArea;

		// Token: 0x04003830 RID: 14384
		public string prefabName;

		// Token: 0x04003831 RID: 14385
		public string tags;

		// Token: 0x04003832 RID: 14386
		public string questTags;
	}
}
