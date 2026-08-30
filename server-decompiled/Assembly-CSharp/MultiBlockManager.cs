using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000B7A RID: 2938
public class MultiBlockManager
{
	// Token: 0x1700095D RID: 2397
	// (get) Token: 0x060058EE RID: 22766 RVA: 0x002208CA File Offset: 0x0021EACA
	public static MultiBlockManager Instance
	{
		get
		{
			if (MultiBlockManager.m_Instance == null)
			{
				MultiBlockManager.m_Instance = new MultiBlockManager();
			}
			return MultiBlockManager.m_Instance;
		}
	}

	// Token: 0x060058EF RID: 22767 RVA: 0x002208E4 File Offset: 0x0021EAE4
	[PublicizedFrom(EAccessModifier.Private)]
	public MultiBlockManager()
	{
	}

	// Token: 0x060058F0 RID: 22768 RVA: 0x00220939 File Offset: 0x0021EB39
	[PublicizedFrom(EAccessModifier.Private)]
	public bool CheckFeatures(MultiBlockManager.FeatureFlags targetFeatures, MultiBlockManager.FeatureRequirement requirement = MultiBlockManager.FeatureRequirement.AllEnabled)
	{
		this.DoCurrentModeSanityChecks();
		switch (requirement)
		{
		case MultiBlockManager.FeatureRequirement.OneOrMoreEnabled:
			return (this.enabledFeatures & targetFeatures) > MultiBlockManager.FeatureFlags.None;
		case MultiBlockManager.FeatureRequirement.AllDisabled:
			return (this.enabledFeatures & targetFeatures) == MultiBlockManager.FeatureFlags.None;
		}
		return (this.enabledFeatures & targetFeatures) == targetFeatures;
	}

	// Token: 0x1700095E RID: 2398
	// (get) Token: 0x060058F1 RID: 22769 RVA: 0x00220978 File Offset: 0x0021EB78
	public bool NoFeaturesEnabled
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CheckFeatures(MultiBlockManager.FeatureFlags.All, MultiBlockManager.FeatureRequirement.AllDisabled);
		}
	}

	// Token: 0x060058F2 RID: 22770 RVA: 0x00220984 File Offset: 0x0021EB84
	[PublicizedFrom(EAccessModifier.Private)]
	public MultiBlockManager.FeatureFlags GetFeaturesForMode(MultiBlockManager.Mode mode)
	{
		MultiBlockManager.FeatureFlags featureFlags;
		switch (mode)
		{
		case MultiBlockManager.Mode.Normal:
			featureFlags = MultiBlockManager.FeatureFlags.All;
			break;
		case MultiBlockManager.Mode.WorldEditor:
			featureFlags = MultiBlockManager.FeatureFlags.POIMBTracking | MultiBlockManager.FeatureFlags.CrossChunkMBTracking | MultiBlockManager.FeatureFlags.OversizedStability | MultiBlockManager.FeatureFlags.TerrainAlignment;
			break;
		case MultiBlockManager.Mode.PrefabPlaytest:
			featureFlags = MultiBlockManager.FeatureFlags.POIMBTracking | MultiBlockManager.FeatureFlags.OversizedStability | MultiBlockManager.FeatureFlags.TerrainAlignment;
			break;
		case MultiBlockManager.Mode.PrefabEditor:
			featureFlags = MultiBlockManager.FeatureFlags.POIMBTracking | MultiBlockManager.FeatureFlags.TerrainAlignment;
			break;
		case MultiBlockManager.Mode.Client:
			featureFlags = MultiBlockManager.FeatureFlags.TerrainAlignment;
			break;
		default:
			featureFlags = MultiBlockManager.FeatureFlags.None;
			break;
		}
		return featureFlags;
	}

	// Token: 0x1700095F RID: 2399
	// (get) Token: 0x060058F3 RID: 22771 RVA: 0x002209CB File Offset: 0x0021EBCB
	public bool POIMBTrackingEnabled
	{
		get
		{
			return this.CheckFeatures(MultiBlockManager.FeatureFlags.POIMBTracking, MultiBlockManager.FeatureRequirement.AllEnabled);
		}
	}

	// Token: 0x060058F4 RID: 22772 RVA: 0x002209D8 File Offset: 0x0021EBD8
	public void Initialize(RegionFileManager regionFileManager)
	{
		object obj = this.lockObj;
		lock (obj)
		{
			this.regionFileManager = regionFileManager;
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				if (GameManager.Instance.IsEditMode())
				{
					this.currentMode = ((regionFileManager == null) ? MultiBlockManager.Mode.PrefabEditor : MultiBlockManager.Mode.WorldEditor);
				}
				else
				{
					this.currentMode = ((regionFileManager == null) ? MultiBlockManager.Mode.PrefabPlaytest : MultiBlockManager.Mode.Normal);
				}
			}
			else
			{
				this.currentMode = MultiBlockManager.Mode.Client;
			}
			this.enabledFeatures = this.GetFeaturesForMode(this.currentMode);
			this.world = GameManager.Instance.World;
			this.cc = this.world.ChunkCache;
			this.chunkManager = this.world.m_ChunkManager;
			if (this.CheckFeatures(MultiBlockManager.FeatureFlags.OversizedStability | MultiBlockManager.FeatureFlags.TerrainAlignment, MultiBlockManager.FeatureRequirement.OneOrMoreEnabled))
			{
				ChunkManager chunkManager = this.chunkManager;
				chunkManager.OnChunkInitialized = (Action<Chunk>)Delegate.Combine(chunkManager.OnChunkInitialized, new Action<Chunk>(this.OnChunkInitialized));
			}
			ChunkManager chunkManager2 = this.chunkManager;
			chunkManager2.OnChunkRegenerated = (Action<Chunk>)Delegate.Combine(chunkManager2.OnChunkRegenerated, new Action<Chunk>(this.OnChunkRegeneratedOrDisplayed));
			ChunkManager chunkManager3 = this.chunkManager;
			chunkManager3.OnChunkCopiedToUnity = (Action<Chunk>)Delegate.Combine(chunkManager3.OnChunkCopiedToUnity, new Action<Chunk>(this.OnChunkRegeneratedOrDisplayed));
			ChunkManager chunkManager4 = this.chunkManager;
			chunkManager4.OnChunkStabilityCalculationEnabled = (Action<Chunk>)Delegate.Combine(chunkManager4.OnChunkStabilityCalculationEnabled, new Action<Chunk>(this.OnChunkStabilityCalculationEnabled));
			this.filePath = Path.Combine(GameIO.GetSaveGameDir(), "multiblocks.7dt");
			this.<Initialize>g__TryLoad|33_0();
			this.isDirty = false;
		}
	}

	// Token: 0x060058F5 RID: 22773 RVA: 0x00220B70 File Offset: 0x0021ED70
	public void Cleanup()
	{
		if (this.NoFeaturesEnabled)
		{
			return;
		}
		object obj = this.lockObj;
		lock (obj)
		{
			ChunkManager chunkManager = this.chunkManager;
			chunkManager.OnChunkInitialized = (Action<Chunk>)Delegate.Remove(chunkManager.OnChunkInitialized, new Action<Chunk>(this.OnChunkInitialized));
			ChunkManager chunkManager2 = this.chunkManager;
			chunkManager2.OnChunkRegenerated = (Action<Chunk>)Delegate.Remove(chunkManager2.OnChunkRegenerated, new Action<Chunk>(this.OnChunkRegeneratedOrDisplayed));
			ChunkManager chunkManager3 = this.chunkManager;
			chunkManager3.OnChunkCopiedToUnity = (Action<Chunk>)Delegate.Remove(chunkManager3.OnChunkCopiedToUnity, new Action<Chunk>(this.OnChunkRegeneratedOrDisplayed));
			ChunkManager chunkManager4 = this.chunkManager;
			chunkManager4.OnChunkStabilityCalculationEnabled = (Action<Chunk>)Delegate.Remove(chunkManager4.OnChunkStabilityCalculationEnabled, new Action<Chunk>(this.OnChunkStabilityCalculationEnabled));
			this.SaveIfDirty();
			this.trackedDataMap.Clear();
			this.oversizedBlocksWithDirtyStability.Clear();
			this.blocksWithDirtyAlignment.Clear();
			this.deregisteredMultiBlockCount = 0;
			this.filePath = null;
			this.regionFileManager = null;
			this.cc = null;
			this.currentMode = MultiBlockManager.Mode.Disabled;
			this.enabledFeatures = MultiBlockManager.FeatureFlags.None;
		}
	}

	// Token: 0x060058F6 RID: 22774 RVA: 0x00220CA0 File Offset: 0x0021EEA0
	public void SaveIfDirty()
	{
		if (!this.CheckFeatures(MultiBlockManager.FeatureFlags.Serialization, MultiBlockManager.FeatureRequirement.AllEnabled))
		{
			return;
		}
		object obj = this.lockObj;
		lock (obj)
		{
			if (this.isDirty)
			{
				if (string.IsNullOrEmpty(this.filePath))
				{
					Log.Error("[MultiBlockManager] Failed to save MultiBlock data; MultiBlockManager has not been initialized with a valid filepath.");
				}
				else
				{
					this.CullCompletePOIPlacements();
					using (Stream stream = SdFile.OpenWrite(this.filePath))
					{
						using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
						{
							pooledBinaryWriter.SetBaseStream(stream);
							pooledBinaryWriter.Write(6);
							pooledBinaryWriter.Write(this.trackedDataMap.TrackedDataByPosition.Count);
							foreach (KeyValuePair<Vector3i, MultiBlockManager.TrackedBlockData> keyValuePair in this.trackedDataMap.TrackedDataByPosition)
							{
								StreamUtils.Write(pooledBinaryWriter, keyValuePair.Key);
								MultiBlockManager.TrackedBlockData value = keyValuePair.Value;
								pooledBinaryWriter.Write(value.rawData);
								pooledBinaryWriter.Write((byte)value.trackingTypeFlags);
							}
							stream.SetLength(stream.Position);
							pooledBinaryWriter.Flush();
						}
					}
					this.isDirty = false;
				}
			}
		}
	}

	// Token: 0x060058F7 RID: 22775 RVA: 0x00220E38 File Offset: 0x0021F038
	public void CullChunklessData()
	{
		if (!this.CheckFeatures(MultiBlockManager.FeatureFlags.CrossChunkMBTracking, MultiBlockManager.FeatureRequirement.AllEnabled))
		{
			return;
		}
		object obj = this.lockObj;
		lock (obj)
		{
			foreach (KeyValuePair<Vector3i, MultiBlockManager.TrackedBlockData> keyValuePair in this.trackedDataMap.TrackedDataByPosition)
			{
				Vector3i key = keyValuePair.Key;
				RectInt flatChunkBounds = keyValuePair.Value.flatChunkBounds;
				if (!this.<CullChunklessData>g__AnyOverlappedChunkIsSyncedOrInSaveDir|36_0(flatChunkBounds))
				{
					this.keysToRemove.Enqueue(key);
				}
			}
			Vector3i vector3i;
			while (this.keysToRemove.TryDequeue(out vector3i))
			{
				this.DeregisterTrackedBlockDataInternal(vector3i);
			}
			this.ProcessDeregistrationCleanup();
		}
	}

	// Token: 0x060058F8 RID: 22776 RVA: 0x00220F04 File Offset: 0x0021F104
	public void UpdateTrackedBlockData(Vector3i worldPos, BlockValue blockValue, bool poiOwned)
	{
		if (this.NoFeaturesEnabled)
		{
			return;
		}
		object obj = this.lockObj;
		lock (obj)
		{
			MultiBlockManager.TrackingTypeFlags trackingTypeFlags = MultiBlockManager.TrackingTypeFlags.None;
			MultiBlockManager.TrackedBlockData trackedBlockData;
			if (this.trackedDataMap.TryGetValue(worldPos, out trackedBlockData))
			{
				if (trackedBlockData.rawData != blockValue.rawData)
				{
					this.DeregisterTrackedBlockDataInternal(worldPos);
					this.ProcessDeregistrationCleanup();
				}
				else
				{
					trackingTypeFlags = trackedBlockData.trackingTypeFlags;
					if (poiOwned)
					{
						if ((trackedBlockData.trackingTypeFlags & MultiBlockManager.TrackingTypeFlags.CrossChunkMultiBlock) != MultiBlockManager.TrackingTypeFlags.None)
						{
							this.trackedDataMap.RemoveTrackedData(worldPos, MultiBlockManager.TrackingTypeFlags.CrossChunkMultiBlock);
							this.deregisteredMultiBlockCount++;
							this.ProcessDeregistrationCleanup();
							trackingTypeFlags &= ~MultiBlockManager.TrackingTypeFlags.CrossChunkMultiBlock;
						}
					}
					else if ((trackedBlockData.trackingTypeFlags & MultiBlockManager.TrackingTypeFlags.PoiMultiBlock) != MultiBlockManager.TrackingTypeFlags.None)
					{
						this.trackedDataMap.RemoveTrackedData(worldPos, MultiBlockManager.TrackingTypeFlags.PoiMultiBlock);
						trackingTypeFlags &= ~MultiBlockManager.TrackingTypeFlags.PoiMultiBlock;
					}
				}
			}
			if (blockValue.Block.isMultiBlock)
			{
				if (poiOwned)
				{
					if ((trackingTypeFlags & MultiBlockManager.TrackingTypeFlags.PoiMultiBlock) == MultiBlockManager.TrackingTypeFlags.None)
					{
					}
				}
				else if ((trackingTypeFlags & MultiBlockManager.TrackingTypeFlags.CrossChunkMultiBlock) == MultiBlockManager.TrackingTypeFlags.None)
				{
					this.TryRegisterCrossChunkMultiBlock(worldPos, blockValue);
				}
			}
			if (blockValue.Block.isOversized && (trackingTypeFlags & MultiBlockManager.TrackingTypeFlags.OversizedBlock) == MultiBlockManager.TrackingTypeFlags.None)
			{
				this.TryRegisterOversizedBlock(worldPos, blockValue);
			}
			if (blockValue.Block.terrainAlignmentMode != TerrainAlignmentMode.None && (trackingTypeFlags & MultiBlockManager.TrackingTypeFlags.TerrainAlignedBlock) == MultiBlockManager.TrackingTypeFlags.None)
			{
				this.TryRegisterTerrainAlignedBlockInternal(worldPos, blockValue);
			}
		}
	}

	// Token: 0x060058F9 RID: 22777 RVA: 0x00221034 File Offset: 0x0021F234
	public void DeregisterTrackedBlockData(Vector3i worldPos)
	{
		if (this.NoFeaturesEnabled)
		{
			return;
		}
		object obj = this.lockObj;
		lock (obj)
		{
			this.DeregisterTrackedBlockDataInternal(worldPos);
			this.ProcessDeregistrationCleanup();
		}
	}

	// Token: 0x060058FA RID: 22778 RVA: 0x00221084 File Offset: 0x0021F284
	[PublicizedFrom(EAccessModifier.Private)]
	public void DeregisterTrackedBlockDataInternal(Vector3i worldPos)
	{
		MultiBlockManager.TrackedBlockData trackedBlockData;
		if (this.trackedDataMap.TryGetValue(worldPos, out trackedBlockData))
		{
			if ((trackedBlockData.trackingTypeFlags & MultiBlockManager.TrackingTypeFlags.CrossChunkMultiBlock) != MultiBlockManager.TrackingTypeFlags.None)
			{
				this.deregisteredMultiBlockCount++;
			}
			this.trackedDataMap.RemoveTrackedData(worldPos, MultiBlockManager.TrackingTypeFlags.All);
			this.blocksWithDirtyAlignment.Remove(worldPos);
			this.oversizedBlocksWithDirtyStability.Remove(worldPos);
			this.isDirty = true;
		}
	}

	// Token: 0x060058FB RID: 22779 RVA: 0x002210E8 File Offset: 0x0021F2E8
	public void DeregisterTrackedBlockDatas(Bounds bounds)
	{
		if (this.NoFeaturesEnabled)
		{
			return;
		}
		object obj = this.lockObj;
		lock (obj)
		{
			List<Vector3i> list = null;
			foreach (Vector3i vector3i in this.trackedDataMap.TrackedDataByPosition.Keys)
			{
				if (bounds.Contains(vector3i))
				{
					if (list == null)
					{
						list = new List<Vector3i>();
					}
					list.Add(vector3i);
				}
			}
			if (list != null)
			{
				foreach (Vector3i vector3i2 in list)
				{
					this.DeregisterTrackedBlockDataInternal(vector3i2);
				}
				this.ProcessDeregistrationCleanup();
			}
		}
	}

	// Token: 0x060058FC RID: 22780 RVA: 0x002211DC File Offset: 0x0021F3DC
	public void MainThreadUpdate()
	{
		this.UpdateAlignment();
		this.UpdateOversizedStability();
	}

	// Token: 0x060058FD RID: 22781 RVA: 0x002211EA File Offset: 0x0021F3EA
	public bool TryRegisterTerrainAlignedBlock(Vector3i worldPos, BlockValue blockValue)
	{
		return this.TryRegisterTerrainAlignedBlockInternal(worldPos, blockValue);
	}

	// Token: 0x060058FE RID: 22782 RVA: 0x002211F4 File Offset: 0x0021F3F4
	[PublicizedFrom(EAccessModifier.Private)]
	public bool TryRegisterTerrainAlignedBlockInternal(Vector3i worldPos, BlockValue blockValue)
	{
		if (!this.CheckFeatures(MultiBlockManager.FeatureFlags.TerrainAlignment, MultiBlockManager.FeatureRequirement.AllEnabled))
		{
			return false;
		}
		if (blockValue.Block.terrainAlignmentMode == TerrainAlignmentMode.None)
		{
			Log.Error(string.Format("[MultiBlockManager] TryRegisterTerrainAlignedBlock failed: target block of type {0} at {1} is not a terrain-aligned block.", blockValue.Block.GetBlockName(), worldPos));
			return false;
		}
		if (blockValue.ischild)
		{
			Log.Error(string.Format("[MultiBlockManager] TryRegisterTerrainAlignedBlock failed: target block is not a parent at position {0}.", worldPos));
			return false;
		}
		object obj = this.lockObj;
		bool flag2;
		lock (obj)
		{
			MultiBlockManager.TrackedBlockData trackedBlockData;
			if (this.trackedDataMap.TryGetValue(worldPos, out trackedBlockData))
			{
				if (blockValue.rawData != trackedBlockData.rawData)
				{
					Log.Error(string.Format("Unexpected condition in TryRegisterTerrainAlignedBlock: encountered raw data mismatch at position {0}.", worldPos));
					flag2 = false;
				}
				else if ((trackedBlockData.trackingTypeFlags & MultiBlockManager.TrackingTypeFlags.TerrainAlignedBlock) != MultiBlockManager.TrackingTypeFlags.None)
				{
					flag2 = true;
				}
				else
				{
					this.<TryRegisterTerrainAlignedBlockInternal>g__RegisterTerrainAlignedBlock|43_0(worldPos, blockValue, trackedBlockData.flatChunkBounds);
					flag2 = true;
				}
			}
			else
			{
				bool flag3 = false;
				if (blockValue.Block.isMultiBlock)
				{
					Vector3i vector3i;
					Vector3i vector3i2;
					MultiBlockManager.GetMinMaxWorldPositions(worldPos, blockValue, out vector3i, out vector3i2);
					Vector2i vector2i = World.toChunkXZ(vector3i);
					Vector2i vector2i2 = World.toChunkXZ(vector3i2);
					RectInt rectInt = new RectInt(vector2i, vector2i2 - vector2i);
					this.<TryRegisterTerrainAlignedBlockInternal>g__RegisterTerrainAlignedBlock|43_0(worldPos, blockValue, rectInt);
					flag3 = true;
				}
				if (blockValue.Block.isOversized)
				{
					Vector3i vector3i3;
					Vector3i vector3i4;
					OversizedBlockUtils.GetWorldAlignedBoundsExtents(worldPos, blockValue.Block.shape.GetRotation(blockValue), blockValue.Block.oversizedBounds, out vector3i3, out vector3i4);
					Vector2i vector2i3 = World.toChunkXZ(vector3i3);
					Vector2i vector2i4 = World.toChunkXZ(vector3i4);
					RectInt rectInt2 = new RectInt(vector2i3, vector2i4 - vector2i3);
					this.<TryRegisterTerrainAlignedBlockInternal>g__RegisterTerrainAlignedBlock|43_0(worldPos, blockValue, rectInt2);
					flag3 = true;
				}
				flag2 = flag3;
			}
		}
		return flag2;
	}

	// Token: 0x060058FF RID: 22783 RVA: 0x002213C8 File Offset: 0x0021F5C8
	public void SetTerrainAlignmentDirty(Vector3i worldPos)
	{
		if (!this.CheckFeatures(MultiBlockManager.FeatureFlags.TerrainAlignment, MultiBlockManager.FeatureRequirement.AllEnabled))
		{
			return;
		}
		object obj = this.lockObj;
		lock (obj)
		{
			if (!this.blocksWithDirtyAlignment.Contains(worldPos))
			{
				if (!this.trackedDataMap.TerrainAlignedBlocks.ContainsKey(worldPos))
				{
					Log.Warning(string.Format("[MultiBlockManager][Alignment] SetTerrainAlignmentDirty failed; no terrain-aligned block has been registered at the specified world position: {0}", worldPos));
				}
				else
				{
					this.blocksWithDirtyAlignment.Add(worldPos);
				}
			}
		}
	}

	// Token: 0x06005900 RID: 22784 RVA: 0x00221458 File Offset: 0x0021F658
	public void SetTerrainAlignmentDirty(BlockValueRef bvRef)
	{
		switch (bvRef.Type)
		{
		case BlockValueRefType.None:
		case BlockValueRefType.Prop:
			return;
		case BlockValueRefType.Block:
			this.SetTerrainAlignmentDirty(bvRef.BlockPosition);
			return;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	// Token: 0x06005901 RID: 22785 RVA: 0x00221494 File Offset: 0x0021F694
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateAlignment()
	{
		if (!this.CheckFeatures(MultiBlockManager.FeatureFlags.TerrainAlignment, MultiBlockManager.FeatureRequirement.AllEnabled))
		{
			return;
		}
		object obj = this.lockObj;
		lock (obj)
		{
			foreach (Vector3i vector3i in this.blocksWithDirtyAlignment)
			{
				MultiBlockManager.TrackedBlockData trackedBlockData;
				if (!this.trackedDataMap.TerrainAlignedBlocks.TryGetValue(vector3i, out trackedBlockData))
				{
					Log.Warning(string.Format("[MultiBlockManager][Alignment] Failed to retrieve registered terrain-aligned block at expected location: {0}", vector3i));
				}
				else
				{
					this.<UpdateAlignment>g__TryAlignBlock|46_1(vector3i, trackedBlockData);
				}
			}
			this.blocksWithDirtyAlignment.Clear();
		}
	}

	// Token: 0x06005902 RID: 22786 RVA: 0x0022155C File Offset: 0x0021F75C
	public void CullChunklessDataOnClient(List<long> removedChunks)
	{
		MultiBlockManager.<>c__DisplayClass47_0 CS$<>8__locals1;
		CS$<>8__locals1.removedChunks = removedChunks;
		CS$<>8__locals1.<>4__this = this;
		if (this.currentMode != MultiBlockManager.Mode.Client)
		{
			return;
		}
		object obj = this.lockObj;
		lock (obj)
		{
			foreach (KeyValuePair<Vector3i, MultiBlockManager.TrackedBlockData> keyValuePair in this.trackedDataMap.TrackedDataByPosition)
			{
				Vector3i key = keyValuePair.Key;
				RectInt flatChunkBounds = keyValuePair.Value.flatChunkBounds;
				if (!this.<CullChunklessDataOnClient>g__AnyOverlappedChunkIsSynced|47_0(flatChunkBounds, ref CS$<>8__locals1))
				{
					this.keysToRemove.Enqueue(key);
				}
			}
			Vector3i vector3i;
			while (this.keysToRemove.TryDequeue(out vector3i))
			{
				this.DeregisterTrackedBlockDataInternal(vector3i);
			}
		}
	}

	// Token: 0x06005903 RID: 22787 RVA: 0x00221634 File Offset: 0x0021F834
	public bool TryRegisterPOIMultiBlock(Vector3i parentWorldPos, BlockValue blockValue)
	{
		if (!this.CheckFeatures(MultiBlockManager.FeatureFlags.POIMBTracking, MultiBlockManager.FeatureRequirement.AllEnabled))
		{
			return false;
		}
		object obj = this.lockObj;
		bool flag2;
		lock (obj)
		{
			MultiBlockManager.TrackedBlockData trackedBlockData;
			MultiBlockManager.TrackedBlockData trackedBlockData2;
			if (this.trackedDataMap.CrossChunkMultiBlocks.TryGetValue(parentWorldPos, out trackedBlockData))
			{
				Log.Error(string.Format("[MultiBlockManager] Failed to register POI multiblock at {0} due to previously registered CrossChunk data.", parentWorldPos) + string.Format("\nOld value: {0} ", trackedBlockData.rawData) + string.Format("\nNew value: {0}", blockValue.rawData));
				flag2 = false;
			}
			else if (this.trackedDataMap.PoiMultiBlocks.TryGetValue(parentWorldPos, out trackedBlockData2))
			{
				Log.Error(string.Format("[MultiBlockManager] Duplicate multiblock placement at {0}. New value will not be applied.", parentWorldPos) + string.Format("\nOld value: {0} ", trackedBlockData2.rawData) + string.Format("\nNew value: {0}", blockValue.rawData));
				flag2 = false;
			}
			else if (blockValue.ischild)
			{
				Log.Error("[MultiBlockManager] TryAddPOIMultiBlock failed: target block is not a parent.");
				flag2 = false;
			}
			else
			{
				RectInt rectInt;
				if (!blockValue.Block.isMultiBlock)
				{
					Vector2i vector2i = World.toChunkXZ(parentWorldPos);
					rectInt = new RectInt(vector2i, Vector2Int.zero);
				}
				else
				{
					Vector3i vector3i;
					Vector3i vector3i2;
					MultiBlockManager.GetMinMaxWorldPositions(parentWorldPos, blockValue, out vector3i, out vector3i2);
					Vector2i vector2i2 = World.toChunkXZ(vector3i);
					Vector2i vector2i3 = World.toChunkXZ(vector3i2);
					rectInt = new RectInt(vector2i2, vector2i3 - vector2i2);
				}
				this.trackedDataMap.AddOrMergeTrackedData(parentWorldPos, blockValue.rawData, rectInt, MultiBlockManager.TrackingTypeFlags.PoiMultiBlock);
				this.isDirty = true;
				flag2 = true;
			}
		}
		return flag2;
	}

	// Token: 0x06005904 RID: 22788 RVA: 0x002217F0 File Offset: 0x0021F9F0
	public static void GetMinMaxWorldPositions(Vector3i parentWorldPos, BlockValue blockValue, out Vector3i minPos, out Vector3i maxPos)
	{
		minPos = parentWorldPos;
		maxPos = parentWorldPos;
		for (int i = 0; i < blockValue.Block.multiBlockPos.Length; i++)
		{
			Vector3i vector3i = parentWorldPos + blockValue.Block.multiBlockPos.Get(i, blockValue.type, (int)blockValue.rotation);
			minPos = Vector3i.Min(minPos, vector3i);
			maxPos = Vector3i.Max(maxPos, vector3i);
		}
	}

	// Token: 0x06005905 RID: 22789 RVA: 0x00221874 File Offset: 0x0021FA74
	public bool TryGetPOIMultiBlock(Vector3i parentWorldPos, out MultiBlockManager.TrackedBlockData poiMultiBlock)
	{
		if (!this.CheckFeatures(MultiBlockManager.FeatureFlags.POIMBTracking, MultiBlockManager.FeatureRequirement.AllEnabled))
		{
			poiMultiBlock = default(MultiBlockManager.TrackedBlockData);
			return false;
		}
		object obj = this.lockObj;
		bool flag2;
		lock (obj)
		{
			flag2 = this.trackedDataMap.PoiMultiBlocks.TryGetValue(parentWorldPos, out poiMultiBlock);
		}
		return flag2;
	}

	// Token: 0x06005906 RID: 22790 RVA: 0x002218D8 File Offset: 0x0021FAD8
	public bool TryGetOversizedBlock(Vector3i worldPos, out BlockValue oversizedBlock, out Vector3i oversizedBlockParent, out Vector3i oversizedBlockOffset)
	{
		object obj = this.lockObj;
		lock (obj)
		{
			Vector2i vector2i = World.toChunkXZ(worldPos);
			HashSet<Vector3i> hashSet;
			if (this.trackedDataMap.OversizedBlocksByChunkPosition.TryGetValue(vector2i, out hashSet))
			{
				foreach (Vector3i vector3i in hashSet)
				{
					MultiBlockManager.TrackedBlockData trackedBlockData;
					if (this.trackedDataMap.OversizedBlocks.TryGetValue(vector3i, out trackedBlockData))
					{
						oversizedBlock = new BlockValue(trackedBlockData.rawData);
						if (oversizedBlock.Block.isOversized)
						{
							oversizedBlockParent = vector3i;
							Bounds oversizedBounds = oversizedBlock.Block.oversizedBounds;
							Quaternion rotation = oversizedBlock.Block.shape.GetRotation(oversizedBlock);
							Matrix4x4 blockWorldToLocalMatrix = OversizedBlockUtils.GetBlockWorldToLocalMatrix(vector3i, rotation);
							if (OversizedBlockUtils.IsBlockCenterWithinBounds(worldPos, oversizedBounds, blockWorldToLocalMatrix))
							{
								Vector3 vector = blockWorldToLocalMatrix.MultiplyPoint3x4(worldPos + new Vector3(0.5f, 0.5f, 0.5f));
								oversizedBlockOffset = new Vector3i(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y), Mathf.RoundToInt(vector.z));
								return true;
							}
						}
					}
				}
			}
		}
		oversizedBlock = default(BlockValue);
		oversizedBlockParent = default(Vector3i);
		oversizedBlockOffset = default(Vector3i);
		return false;
	}

	// Token: 0x06005907 RID: 22791 RVA: 0x00221A84 File Offset: 0x0021FC84
	public void CullCompletePOIPlacements()
	{
		if (!this.CheckFeatures(MultiBlockManager.FeatureFlags.POIMBTracking | MultiBlockManager.FeatureFlags.Serialization, MultiBlockManager.FeatureRequirement.AllEnabled))
		{
			return;
		}
		object obj = this.lockObj;
		lock (obj)
		{
			foreach (KeyValuePair<Vector3i, MultiBlockManager.TrackedBlockData> keyValuePair in this.trackedDataMap.PoiMultiBlocks)
			{
				Vector3i key = keyValuePair.Key;
				RectInt flatChunkBounds = keyValuePair.Value.flatChunkBounds;
				if (this.<CullCompletePOIPlacements>g__AllOverlappedChunksAreSavedAndDormant|52_0(flatChunkBounds))
				{
					this.keysToRemove.Enqueue(key);
				}
			}
			Vector3i vector3i;
			while (this.keysToRemove.TryDequeue(out vector3i))
			{
				this.trackedDataMap.RemoveTrackedData(vector3i, MultiBlockManager.TrackingTypeFlags.PoiMultiBlock);
				this.isDirty = true;
			}
		}
	}

	// Token: 0x06005908 RID: 22792 RVA: 0x00221B64 File Offset: 0x0021FD64
	[PublicizedFrom(EAccessModifier.Private)]
	public bool TryRegisterCrossChunkMultiBlock(Vector3i parentWorldPos, BlockValue parentBlockValue)
	{
		if (!this.CheckFeatures(MultiBlockManager.FeatureFlags.CrossChunkMBTracking, MultiBlockManager.FeatureRequirement.AllEnabled))
		{
			return false;
		}
		object obj = this.lockObj;
		bool flag2;
		lock (obj)
		{
			if (this.trackedDataMap.PoiMultiBlocks.ContainsKey(parentWorldPos))
			{
				flag2 = false;
			}
			else if (!parentBlockValue.Block.isMultiBlock)
			{
				Log.Error(string.Format("[MultiBlockManager] TryRegisterCrossChunkMultiBlock failed: target block of type {0} at {1} is not a MultiBlock.", parentBlockValue.Block.GetBlockName(), parentWorldPos));
				flag2 = false;
			}
			else if (parentBlockValue.ischild)
			{
				Log.Error("[MultiBlockManager] TryRegisterCrossChunkMultiBlock failed: target block is not a parent.");
				flag2 = false;
			}
			else
			{
				Vector3 vector = parentBlockValue.Block.shape.GetRotation(parentBlockValue) * parentBlockValue.Block.multiBlockPos.dim;
				if (Mathf.Approximately(Mathf.Abs(vector.x), 1f) && Mathf.Approximately(Mathf.Abs(vector.z), 1f))
				{
					flag2 = false;
				}
				else
				{
					Vector3i vector3i;
					Vector3i vector3i2;
					MultiBlockManager.GetMinMaxWorldPositions(parentWorldPos, parentBlockValue, out vector3i, out vector3i2);
					if (vector3i.x == vector3i2.x && vector3i.z == vector3i2.z)
					{
						flag2 = false;
					}
					else
					{
						Vector2i vector2i = World.toChunkXZ(vector3i);
						Vector2i vector2i2 = World.toChunkXZ(vector3i2);
						if (vector2i == vector2i2)
						{
							flag2 = false;
						}
						else
						{
							RectInt rectInt = new RectInt(vector2i, vector2i2 - vector2i);
							this.trackedDataMap.AddOrMergeTrackedData(parentWorldPos, parentBlockValue.rawData, rectInt, MultiBlockManager.TrackingTypeFlags.CrossChunkMultiBlock);
							this.isDirty = true;
							this.tempChunksToGroup.Clear();
							for (int i = rectInt.yMin; i <= rectInt.yMax; i++)
							{
								for (int j = rectInt.xMin; j <= rectInt.xMax; j++)
								{
									long num = WorldChunkCache.MakeChunkKey(j, i);
									this.tempChunksToGroup.Add(num);
								}
							}
							this.regionFileManager.AddGroupedChunks(this.tempChunksToGroup);
							this.tempChunksToGroup.Clear();
							flag2 = true;
						}
					}
				}
			}
		}
		return flag2;
	}

	// Token: 0x06005909 RID: 22793 RVA: 0x00221D94 File Offset: 0x0021FF94
	[PublicizedFrom(EAccessModifier.Private)]
	public void ProcessDeregistrationCleanup()
	{
		if (!this.CheckFeatures(MultiBlockManager.FeatureFlags.CrossChunkMBTracking, MultiBlockManager.FeatureRequirement.AllEnabled))
		{
			return;
		}
		if (this.deregisteredMultiBlockCount <= 20)
		{
			return;
		}
		this.regionFileManager.RebuildChunkGroupsFromPOIs();
		this.tempChunksToGroup.Clear();
		foreach (KeyValuePair<Vector3i, MultiBlockManager.TrackedBlockData> keyValuePair in this.trackedDataMap.CrossChunkMultiBlocks)
		{
			RectInt flatChunkBounds = keyValuePair.Value.flatChunkBounds;
			for (int i = flatChunkBounds.yMin; i <= flatChunkBounds.yMax; i++)
			{
				for (int j = flatChunkBounds.xMin; j <= flatChunkBounds.xMax; j++)
				{
					long num = WorldChunkCache.MakeChunkKey(j, i);
					this.tempChunksToGroup.Add(num);
				}
			}
			this.regionFileManager.AddGroupedChunks(this.tempChunksToGroup);
			this.tempChunksToGroup.Clear();
		}
		this.deregisteredMultiBlockCount = 0;
	}

	// Token: 0x0600590A RID: 22794 RVA: 0x00221E9C File Offset: 0x0022009C
	[PublicizedFrom(EAccessModifier.Private)]
	public bool TryRegisterOversizedBlock(Vector3i worldPos, BlockValue blockValue)
	{
		if (!this.CheckFeatures(MultiBlockManager.FeatureFlags.OversizedStability, MultiBlockManager.FeatureRequirement.AllEnabled))
		{
			return false;
		}
		object obj = this.lockObj;
		lock (obj)
		{
			if (!blockValue.Block.isOversized)
			{
				Log.Error(string.Format("[MultiBlockManager] TryRegisterOversizedBlock failed: target block of type {0} at {1} is not an Oversized Block.", blockValue.Block.GetBlockName(), worldPos));
				return false;
			}
			Vector3i vector3i;
			Vector3i vector3i2;
			OversizedBlockUtils.GetWorldAlignedBoundsExtents(worldPos, blockValue.Block.shape.GetRotation(blockValue), blockValue.Block.oversizedBounds, out vector3i, out vector3i2);
			Vector2i vector2i = World.toChunkXZ(vector3i);
			Vector2i vector2i2 = World.toChunkXZ(vector3i2);
			RectInt rectInt = new RectInt(vector2i, vector2i2 - vector2i);
			this.trackedDataMap.AddOrMergeTrackedData(worldPos, blockValue.rawData, rectInt, MultiBlockManager.TrackingTypeFlags.OversizedBlock);
			this.oversizedBlocksWithDirtyStability.Add(worldPos);
			this.isDirty = true;
		}
		return true;
	}

	// Token: 0x0600590B RID: 22795 RVA: 0x00221F9C File Offset: 0x0022019C
	public void SetOversizedStabilityDirty(Vector3i worldPos)
	{
		if (!this.CheckFeatures(MultiBlockManager.FeatureFlags.OversizedStability, MultiBlockManager.FeatureRequirement.AllEnabled))
		{
			return;
		}
		object obj = this.lockObj;
		lock (obj)
		{
			Vector2i vector2i = World.toChunkXZ(worldPos);
			HashSet<Vector3i> hashSet;
			if (this.trackedDataMap.OversizedBlocksByChunkPosition.TryGetValue(vector2i, out hashSet))
			{
				foreach (Vector3i vector3i in hashSet)
				{
					if (!this.oversizedBlocksWithDirtyStability.Contains(vector3i))
					{
						MultiBlockManager.TrackedBlockData trackedBlockData;
						if (!this.trackedDataMap.OversizedBlocks.TryGetValue(vector3i, out trackedBlockData))
						{
							Log.Error("Tracked data mismatch: Oversized block chunk bin contains position without valid oversized block data.");
						}
						else
						{
							BlockValue blockValue = new BlockValue(trackedBlockData.rawData);
							Quaternion rotation = blockValue.Block.shape.GetRotation(blockValue);
							Bounds localStabilityBounds = OversizedBlockUtils.GetLocalStabilityBounds(blockValue.Block.oversizedBounds, rotation);
							localStabilityBounds.extents += Vector3Int.one;
							Matrix4x4 blockWorldToLocalMatrix = OversizedBlockUtils.GetBlockWorldToLocalMatrix(vector3i, rotation);
							if (OversizedBlockUtils.IsBlockCenterWithinBounds(worldPos, localStabilityBounds, blockWorldToLocalMatrix))
							{
								this.oversizedBlocksWithDirtyStability.Add(vector3i);
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x0600590C RID: 22796 RVA: 0x00222108 File Offset: 0x00220308
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnChunkRegeneratedOrDisplayed(Chunk chunk)
	{
		if (!this.CheckFeatures(MultiBlockManager.FeatureFlags.TerrainAlignment, MultiBlockManager.FeatureRequirement.AllEnabled))
		{
			return;
		}
		Vector2i vector2i = new Vector2i(chunk.X, chunk.Z);
		object obj = this.lockObj;
		lock (obj)
		{
			MultiBlockManager.AddChunkOverlappingBlocksToSet(vector2i, this.trackedDataMap.TerrainAlignedBlocks, this.blocksWithDirtyAlignment);
		}
	}

	// Token: 0x0600590D RID: 22797 RVA: 0x00222178 File Offset: 0x00220378
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnChunkInitialized(Chunk chunk)
	{
		if (!this.CheckFeatures(MultiBlockManager.FeatureFlags.OversizedStability | MultiBlockManager.FeatureFlags.TerrainAlignment, MultiBlockManager.FeatureRequirement.OneOrMoreEnabled))
		{
			return;
		}
		Vector2i vector2i = new Vector2i(chunk.X, chunk.Z);
		object obj = this.lockObj;
		lock (obj)
		{
			if (this.CheckFeatures(MultiBlockManager.FeatureFlags.OversizedStability, MultiBlockManager.FeatureRequirement.AllEnabled))
			{
				MultiBlockManager.AddChunkOverlappingBlocksToSet(vector2i, this.trackedDataMap.OversizedBlocks, this.oversizedBlocksWithDirtyStability);
			}
			if (this.CheckFeatures(MultiBlockManager.FeatureFlags.TerrainAlignment, MultiBlockManager.FeatureRequirement.AllEnabled))
			{
				MultiBlockManager.AddChunkOverlappingBlocksToSet(vector2i, this.trackedDataMap.TerrainAlignedBlocks, this.blocksWithDirtyAlignment);
			}
		}
	}

	// Token: 0x0600590E RID: 22798 RVA: 0x00222214 File Offset: 0x00220414
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnChunkStabilityCalculationEnabled(Chunk chunk)
	{
		if (!this.CheckFeatures(MultiBlockManager.FeatureFlags.OversizedStability, MultiBlockManager.FeatureRequirement.AllEnabled))
		{
			return;
		}
		Vector2i vector2i = new Vector2i(chunk.X, chunk.Z);
		object obj = this.lockObj;
		lock (obj)
		{
			MultiBlockManager.AddChunkOverlappingBlocksToSet(vector2i, this.trackedDataMap.OversizedBlocks, this.oversizedBlocksWithDirtyStability);
		}
	}

	// Token: 0x0600590F RID: 22799 RVA: 0x00222284 File Offset: 0x00220484
	[PublicizedFrom(EAccessModifier.Private)]
	public static void AddChunkOverlappingBlocksToSet(Vector2i chunkPos, MultiBlockManager.TrackedDataMap.SubsetAccessor blocksMap, HashSet<Vector3i> targetSet)
	{
		foreach (KeyValuePair<Vector3i, MultiBlockManager.TrackedBlockData> keyValuePair in blocksMap)
		{
			Vector3i key = keyValuePair.Key;
			if (!targetSet.Contains(key))
			{
				RectInt flatChunkBounds = keyValuePair.Value.flatChunkBounds;
				flatChunkBounds.max += Vector2Int.one;
				if (flatChunkBounds.Contains(chunkPos))
				{
					targetSet.Add(key);
				}
			}
		}
	}

	// Token: 0x06005910 RID: 22800 RVA: 0x00222318 File Offset: 0x00220518
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateOversizedStability()
	{
		if (!this.CheckFeatures(MultiBlockManager.FeatureFlags.OversizedStability, MultiBlockManager.FeatureRequirement.AllEnabled))
		{
			return;
		}
		object obj = this.lockObj;
		lock (obj)
		{
			foreach (Vector3i vector3i in this.oversizedBlocksWithDirtyStability)
			{
				MultiBlockManager.TrackedBlockData trackedBlockData;
				if (!this.trackedDataMap.OversizedBlocks.TryGetValue(vector3i, out trackedBlockData))
				{
					Log.Warning(string.Format("[MultiBlockManager][Stability] Failed to retrieve registered Oversized Block at expected location: {0}", vector3i));
				}
				else if (!this.<UpdateOversizedStability>g__IsOversizedBlockStable|61_1(vector3i, trackedBlockData))
				{
					GameManager.Instance.World.AddFallingBlock(vector3i, true);
				}
			}
			this.oversizedBlocksWithDirtyStability.Clear();
		}
	}

	// Token: 0x06005911 RID: 22801 RVA: 0x00067DD8 File Offset: 0x00065FD8
	[Conditional("MBM_ENABLE_GENERAL_LOG")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void DebugLogGeneral(string message)
	{
		Log.Out(message);
	}

	// Token: 0x06005912 RID: 22802 RVA: 0x00067DD8 File Offset: 0x00065FD8
	[Conditional("MBM_ENABLE_PLACEMENT_LOG")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void DebugLogPlacement(string message)
	{
		Log.Out(message);
	}

	// Token: 0x06005913 RID: 22803 RVA: 0x00067DD8 File Offset: 0x00065FD8
	[Conditional("MBM_ENABLE_REGISTRATION_LOG")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void DebugLogRegistration(string message)
	{
		Log.Out(message);
	}

	// Token: 0x06005914 RID: 22804 RVA: 0x00067DD8 File Offset: 0x00065FD8
	[Conditional("MBM_ENABLE_STABILITY_LOG")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void DebugLogStability(string message)
	{
		Log.Out(message);
	}

	// Token: 0x06005915 RID: 22805 RVA: 0x00067DD8 File Offset: 0x00065FD8
	[Conditional("MBM_ENABLE_ALIGNMENT_LOG")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void DebugLogAlignment(string message)
	{
		Log.Out(message);
	}

	// Token: 0x06005916 RID: 22806 RVA: 0x000027FC File Offset: 0x000009FC
	[Conditional("MBM_ENABLE_PROFILER_MARKERS")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateProfilerCounters()
	{
	}

	// Token: 0x06005917 RID: 22807 RVA: 0x002223F0 File Offset: 0x002205F0
	[Conditional("MBM_ENABLED_SANITY_CHECKS")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void DoCurrentModeSanityChecks()
	{
		MultiBlockManager.Mode mode = MultiBlockManager.Mode.Client;
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			if (GameManager.Instance.IsEditMode())
			{
				mode = ((this.regionFileManager == null) ? MultiBlockManager.Mode.PrefabEditor : MultiBlockManager.Mode.WorldEditor);
			}
			else
			{
				mode = ((this.regionFileManager == null) ? MultiBlockManager.Mode.PrefabPlaytest : MultiBlockManager.Mode.Normal);
			}
		}
		if (this.currentMode != mode)
		{
			Log.Error("[MultiBlockManager] Unexpected mode state. \n" + string.Format("Current mode: {0}. Expected mode: {1}. \n", this.currentMode, mode) + string.Format("GameManager.Instance.IsEditMode(): {0}, ", GameManager.Instance.IsEditMode()) + string.Format("ConnectionManager.Instance.IsServer: {0}, ", SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer));
		}
	}

	// Token: 0x06005918 RID: 22808 RVA: 0x00222498 File Offset: 0x00220698
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool <Initialize>g__TryLoad|33_0()
	{
		this.trackedDataMap.Clear();
		this.deregisteredMultiBlockCount = 0;
		if (!this.CheckFeatures(MultiBlockManager.FeatureFlags.Serialization, MultiBlockManager.FeatureRequirement.AllEnabled))
		{
			return false;
		}
		if (!SdFile.Exists(this.filePath))
		{
			return false;
		}
		bool flag;
		using (Stream stream = SdFile.OpenRead(this.filePath))
		{
			using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
			{
				pooledBinaryReader.SetBaseStream(stream);
				byte b = pooledBinaryReader.ReadByte();
				if (b != 6)
				{
					Log.Error(string.Format("[MultiBlockManager] Saved MultiBlock data is out of date. Saved version is ({0}). Current version is ({1}). ", b, 6) + "This data is no longer compatible and will be ignored. MultiBlock-related bugs are likely to occur if you continue with this save. Please start a fresh game to avoid these issues.");
					flag = false;
				}
				else
				{
					int num = pooledBinaryReader.ReadInt32();
					for (int i = 0; i < num; i++)
					{
						Vector3i vector3i = StreamUtils.ReadVector3i(pooledBinaryReader);
						uint num2 = pooledBinaryReader.ReadUInt32();
						BlockValue blockValue = new BlockValue(num2);
						byte b2 = pooledBinaryReader.ReadByte();
						if ((b2 & 1) != 0)
						{
							this.TryRegisterPOIMultiBlock(vector3i, blockValue);
						}
						if ((b2 & 2) != 0)
						{
							this.TryRegisterCrossChunkMultiBlock(vector3i, blockValue);
						}
						if ((b2 & 4) != 0)
						{
							this.TryRegisterOversizedBlock(vector3i, blockValue);
						}
						if ((b2 & 8) != 0)
						{
							this.TryRegisterTerrainAlignedBlockInternal(vector3i, blockValue);
						}
					}
					flag = true;
				}
			}
		}
		return flag;
	}

	// Token: 0x06005919 RID: 22809 RVA: 0x002225D8 File Offset: 0x002207D8
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool <CullChunklessData>g__AnyOverlappedChunkIsSyncedOrInSaveDir|36_0(RectInt flatChunkBounds)
	{
		for (int i = flatChunkBounds.yMin; i <= flatChunkBounds.yMax; i++)
		{
			for (int j = flatChunkBounds.xMin; j <= flatChunkBounds.xMax; j++)
			{
				long num = WorldChunkCache.MakeChunkKey(j, i);
				if (this.regionFileManager.ContainsChunkSync(num) || this.cc.ContainsChunkSync(num))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600591A RID: 22810 RVA: 0x0022263C File Offset: 0x0022083C
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <TryRegisterTerrainAlignedBlockInternal>g__RegisterTerrainAlignedBlock|43_0(Vector3i worldPos, BlockValue blockValue, RectInt flatChunkBounds)
	{
		this.trackedDataMap.AddOrMergeTrackedData(worldPos, blockValue.rawData, flatChunkBounds, MultiBlockManager.TrackingTypeFlags.TerrainAlignedBlock);
		this.blocksWithDirtyAlignment.Add(worldPos);
		this.isDirty = true;
	}

	// Token: 0x0600591B RID: 22811 RVA: 0x00222668 File Offset: 0x00220868
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool <UpdateAlignment>g__AllOverlappedChunksAreReady|46_0(RectInt flatChunkBounds)
	{
		for (int i = flatChunkBounds.yMin; i <= flatChunkBounds.yMax; i++)
		{
			for (int j = flatChunkBounds.xMin; j <= flatChunkBounds.xMax; j++)
			{
				long num = WorldChunkCache.MakeChunkKey(j, i);
				Chunk chunkSync = this.cc.GetChunkSync(num);
				if (chunkSync == null || !chunkSync.IsInitialized || chunkSync.NeedsRegeneration || chunkSync.InProgressRegeneration || chunkSync.NeedsCopying || chunkSync.InProgressCopying)
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x0600591C RID: 22812 RVA: 0x002226EC File Offset: 0x002208EC
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool <UpdateAlignment>g__TryAlignBlock|46_1(Vector3i worldPos, MultiBlockManager.TrackedBlockData blockData)
	{
		RectInt flatChunkBounds = blockData.flatChunkBounds;
		if (!this.<UpdateAlignment>g__AllOverlappedChunksAreReady|46_0(flatChunkBounds))
		{
			return false;
		}
		BlockEntityData blockEntity = ((Chunk)this.world.GetChunkFromWorldPos(worldPos)).GetBlockEntity(worldPos);
		BlockValue blockValue = new BlockValue(blockData.rawData);
		Block block = blockValue.Block;
		TerrainAlignmentMode terrainAlignmentMode = block.terrainAlignmentMode;
		if (terrainAlignmentMode != TerrainAlignmentMode.None && terrainAlignmentMode - TerrainAlignmentMode.Vehicle <= 1)
		{
			return TerrainAlignmentUtils.AlignToTerrain(block, worldPos, blockValue, blockEntity, block.terrainAlignmentMode);
		}
		Log.Error(string.Format("[MultiBlockManager][Alignment] TryAlignBlock cannot align block with TerrainAlignmentMode \"{0}\" of type {1} at {2}", block.terrainAlignmentMode, block.GetBlockName(), worldPos));
		return false;
	}

	// Token: 0x0600591D RID: 22813 RVA: 0x00222784 File Offset: 0x00220984
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool <CullChunklessDataOnClient>g__AnyOverlappedChunkIsSynced|47_0(RectInt flatChunkBounds, ref MultiBlockManager.<>c__DisplayClass47_0 A_2)
	{
		for (int i = flatChunkBounds.yMin; i <= flatChunkBounds.yMax; i++)
		{
			for (int j = flatChunkBounds.xMin; j <= flatChunkBounds.xMax; j++)
			{
				long num = WorldChunkCache.MakeChunkKey(j, i);
				if (!A_2.removedChunks.Contains(num) && this.cc.ContainsChunkSync(num))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600591E RID: 22814 RVA: 0x002227E8 File Offset: 0x002209E8
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool <CullCompletePOIPlacements>g__AllOverlappedChunksAreSavedAndDormant|52_0(RectInt flatChunkBounds)
	{
		for (int i = flatChunkBounds.yMin; i <= flatChunkBounds.yMax; i++)
		{
			for (int j = flatChunkBounds.xMin; j <= flatChunkBounds.xMax; j++)
			{
				long num = WorldChunkCache.MakeChunkKey(j, i);
				if (this.cc.ContainsChunkSync(num) || !this.regionFileManager.IsChunkSavedAndDormant(num))
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x0600591F RID: 22815 RVA: 0x0022284C File Offset: 0x00220A4C
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool <UpdateOversizedStability>g__AllOverlappedChunksAreSyncedAndInitialized|61_0(RectInt flatChunkBounds)
	{
		for (int i = flatChunkBounds.yMin; i <= flatChunkBounds.yMax; i++)
		{
			for (int j = flatChunkBounds.xMin; j <= flatChunkBounds.xMax; j++)
			{
				long num = WorldChunkCache.MakeChunkKey(j, i);
				Chunk chunkSync = this.cc.GetChunkSync(num);
				if (chunkSync == null || !chunkSync.IsInitialized || chunkSync.StopStabilityCalculation)
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06005920 RID: 22816 RVA: 0x002228B4 File Offset: 0x00220AB4
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool <UpdateOversizedStability>g__IsOversizedBlockStable|61_1(Vector3i worldPos, MultiBlockManager.TrackedBlockData blockData)
	{
		RectInt flatChunkBounds = blockData.flatChunkBounds;
		if (!this.<UpdateOversizedStability>g__AllOverlappedChunksAreSyncedAndInitialized|61_0(flatChunkBounds))
		{
			return true;
		}
		BlockValue blockValue = new BlockValue(blockData.rawData);
		Quaternion rotation = blockValue.Block.shape.GetRotation(blockValue);
		Bounds localStabilityBounds = OversizedBlockUtils.GetLocalStabilityBounds(blockValue.Block.oversizedBounds, rotation);
		Vector3i vector3i;
		Vector3i vector3i2;
		OversizedBlockUtils.GetWorldAlignedBoundsExtents(worldPos, rotation, localStabilityBounds, out vector3i, out vector3i2);
		Matrix4x4 blockWorldToLocalMatrix = OversizedBlockUtils.GetBlockWorldToLocalMatrix(worldPos, rotation);
		World world = GameManager.Instance.World;
		for (int i = vector3i.x; i <= vector3i2.x; i++)
		{
			for (int j = vector3i.y; j <= vector3i2.y; j++)
			{
				for (int k = vector3i.z; k <= vector3i2.z; k++)
				{
					Vector3i vector3i3 = new Vector3i(i, j, k);
					if (OversizedBlockUtils.IsBlockCenterWithinBounds(vector3i3, localStabilityBounds, blockWorldToLocalMatrix) && world.GetStability(vector3i3) > 1)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	// Token: 0x04004516 RID: 17686
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte FILEVERSION = 6;

	// Token: 0x04004517 RID: 17687
	[PublicizedFrom(EAccessModifier.Private)]
	public const int c_deregisteredMultiBlockLimit = 20;

	// Token: 0x04004518 RID: 17688
	[PublicizedFrom(EAccessModifier.Private)]
	public object lockObj = new object();

	// Token: 0x04004519 RID: 17689
	[PublicizedFrom(EAccessModifier.Private)]
	public static MultiBlockManager m_Instance;

	// Token: 0x0400451A RID: 17690
	[PublicizedFrom(EAccessModifier.Private)]
	public RegionFileManager regionFileManager;

	// Token: 0x0400451B RID: 17691
	[PublicizedFrom(EAccessModifier.Private)]
	public MultiBlockManager.TrackedDataMap trackedDataMap = new MultiBlockManager.TrackedDataMap();

	// Token: 0x0400451C RID: 17692
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<Vector3i> oversizedBlocksWithDirtyStability = new HashSet<Vector3i>();

	// Token: 0x0400451D RID: 17693
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<Vector3i> blocksWithDirtyAlignment = new HashSet<Vector3i>();

	// Token: 0x0400451E RID: 17694
	[PublicizedFrom(EAccessModifier.Private)]
	public Queue<Vector3i> keysToRemove = new Queue<Vector3i>();

	// Token: 0x0400451F RID: 17695
	[PublicizedFrom(EAccessModifier.Private)]
	public List<long> tempChunksToGroup = new List<long>();

	// Token: 0x04004520 RID: 17696
	[PublicizedFrom(EAccessModifier.Private)]
	public World world;

	// Token: 0x04004521 RID: 17697
	[PublicizedFrom(EAccessModifier.Private)]
	public ChunkCluster cc;

	// Token: 0x04004522 RID: 17698
	[PublicizedFrom(EAccessModifier.Private)]
	public ChunkManager chunkManager;

	// Token: 0x04004523 RID: 17699
	[PublicizedFrom(EAccessModifier.Private)]
	public string filePath;

	// Token: 0x04004524 RID: 17700
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isDirty;

	// Token: 0x04004525 RID: 17701
	[PublicizedFrom(EAccessModifier.Private)]
	public int deregisteredMultiBlockCount;

	// Token: 0x04004526 RID: 17702
	[PublicizedFrom(EAccessModifier.Private)]
	public MultiBlockManager.Mode currentMode;

	// Token: 0x04004527 RID: 17703
	[PublicizedFrom(EAccessModifier.Private)]
	public MultiBlockManager.FeatureFlags enabledFeatures;

	// Token: 0x02000B7B RID: 2939
	[Flags]
	public enum TrackingTypeFlags : byte
	{
		// Token: 0x04004529 RID: 17705
		None = 0,
		// Token: 0x0400452A RID: 17706
		PoiMultiBlock = 1,
		// Token: 0x0400452B RID: 17707
		CrossChunkMultiBlock = 2,
		// Token: 0x0400452C RID: 17708
		OversizedBlock = 4,
		// Token: 0x0400452D RID: 17709
		TerrainAlignedBlock = 8,
		// Token: 0x0400452E RID: 17710
		All = 15
	}

	// Token: 0x02000B7C RID: 2940
	public struct TrackedBlockData
	{
		// Token: 0x06005921 RID: 22817 RVA: 0x002229A3 File Offset: 0x00220BA3
		public TrackedBlockData(uint rawData, RectInt flatChunkBounds, MultiBlockManager.TrackingTypeFlags trackingTypeFlags)
		{
			this.rawData = rawData;
			this.flatChunkBounds = flatChunkBounds;
			this.trackingTypeFlags = trackingTypeFlags;
		}

		// Token: 0x0400452F RID: 17711
		public readonly uint rawData;

		// Token: 0x04004530 RID: 17712
		public readonly RectInt flatChunkBounds;

		// Token: 0x04004531 RID: 17713
		public readonly MultiBlockManager.TrackingTypeFlags trackingTypeFlags;
	}

	// Token: 0x02000B7D RID: 2941
	[PublicizedFrom(EAccessModifier.Private)]
	public class TrackedDataMap
	{
		// Token: 0x06005922 RID: 22818 RVA: 0x002229BC File Offset: 0x00220BBC
		public TrackedDataMap()
		{
			this.TrackedDataByPosition = new ReadOnlyDictionary<Vector3i, MultiBlockManager.TrackedBlockData>(this.trackedDataByPosition);
			this.OversizedBlocksByChunkPosition = new ReadOnlyDictionary<Vector2i, HashSet<Vector3i>>(this.oversizedBlocksByChunkPosition);
		}

		// Token: 0x06005923 RID: 22819 RVA: 0x00222A33 File Offset: 0x00220C33
		public bool ContainsKey(Vector3i key)
		{
			return this.trackedDataByPosition.ContainsKey(key);
		}

		// Token: 0x17000960 RID: 2400
		// (get) Token: 0x06005924 RID: 22820 RVA: 0x00222A41 File Offset: 0x00220C41
		public int Count
		{
			get
			{
				return this.trackedDataByPosition.Count;
			}
		}

		// Token: 0x06005925 RID: 22821 RVA: 0x00222A4E File Offset: 0x00220C4E
		public bool TryGetValue(Vector3i key, out MultiBlockManager.TrackedBlockData value)
		{
			return this.trackedDataByPosition.TryGetValue(key, out value);
		}

		// Token: 0x06005926 RID: 22822 RVA: 0x00222A60 File Offset: 0x00220C60
		public void Clear()
		{
			this.trackedDataByPosition.Clear();
			this.poiMultiBlocks.Clear();
			this.crossChunkMultiBlocks.Clear();
			this.oversizedBlocks.Clear();
			this.terrainAlignedBlocks.Clear();
			this.oversizedBlocksByChunkPosition.Clear();
		}

		// Token: 0x06005927 RID: 22823 RVA: 0x00222AB0 File Offset: 0x00220CB0
		public void AddOrMergeTrackedData(Vector3i worldPos, uint rawData, RectInt flatChunkBounds, MultiBlockManager.TrackingTypeFlags trackingTypeFlags)
		{
			if (trackingTypeFlags == MultiBlockManager.TrackingTypeFlags.None)
			{
				Log.Error(string.Format("AddOrMergeTrackedData failed: Cannot add or merge tracked data with no tracking flags set. At position {0}.", worldPos));
				return;
			}
			MultiBlockManager.TrackedBlockData trackedBlockData;
			MultiBlockManager.TrackingTypeFlags trackingTypeFlags2;
			if (this.trackedDataByPosition.TryGetValue(worldPos, out trackedBlockData))
			{
				if (rawData != trackedBlockData.rawData)
				{
					Log.Warning(string.Format("Unexpected condition in AddOrMergeTrackedData: encountered raw data mismatch at position {0}.", worldPos));
				}
				RectInt rectInt = flatChunkBounds;
				if (!rectInt.Equals(trackedBlockData.flatChunkBounds))
				{
					rectInt.SetMinMax(Vector2Int.Min(rectInt.min, trackedBlockData.flatChunkBounds.min), Vector2Int.Max(rectInt.max, trackedBlockData.flatChunkBounds.max));
					if ((trackedBlockData.trackingTypeFlags & MultiBlockManager.TrackingTypeFlags.OversizedBlock) != MultiBlockManager.TrackingTypeFlags.None)
					{
						this.AddOversizedBlockToChunkBins(worldPos);
					}
				}
				trackingTypeFlags2 = trackingTypeFlags & ~trackedBlockData.trackingTypeFlags;
				if (trackingTypeFlags2 != trackingTypeFlags)
				{
					Log.Warning(string.Format("Unexpected condition in AddOrMergeTrackedData: tracked data already has one or more target flag(s) set at position {0}.", worldPos));
				}
				MultiBlockManager.TrackingTypeFlags trackingTypeFlags3 = trackingTypeFlags | trackedBlockData.trackingTypeFlags;
				this.trackedDataByPosition[worldPos] = new MultiBlockManager.TrackedBlockData(rawData, rectInt, trackingTypeFlags3);
			}
			else
			{
				trackingTypeFlags2 = trackingTypeFlags;
				this.trackedDataByPosition[worldPos] = new MultiBlockManager.TrackedBlockData(rawData, flatChunkBounds, trackingTypeFlags);
			}
			if (trackingTypeFlags2 == MultiBlockManager.TrackingTypeFlags.None)
			{
				return;
			}
			if ((trackingTypeFlags2 & MultiBlockManager.TrackingTypeFlags.PoiMultiBlock) != MultiBlockManager.TrackingTypeFlags.None)
			{
				this.poiMultiBlocks.Add(worldPos);
			}
			if ((trackingTypeFlags2 & MultiBlockManager.TrackingTypeFlags.CrossChunkMultiBlock) != MultiBlockManager.TrackingTypeFlags.None)
			{
				this.crossChunkMultiBlocks.Add(worldPos);
			}
			if ((trackingTypeFlags2 & MultiBlockManager.TrackingTypeFlags.OversizedBlock) != MultiBlockManager.TrackingTypeFlags.None)
			{
				this.oversizedBlocks.Add(worldPos);
				this.AddOversizedBlockToChunkBins(worldPos);
			}
			if ((trackingTypeFlags2 & MultiBlockManager.TrackingTypeFlags.TerrainAlignedBlock) != MultiBlockManager.TrackingTypeFlags.None)
			{
				this.terrainAlignedBlocks.Add(worldPos);
			}
		}

		// Token: 0x06005928 RID: 22824 RVA: 0x00222C18 File Offset: 0x00220E18
		[PublicizedFrom(EAccessModifier.Private)]
		public void AddOversizedBlockToChunkBins(Vector3i worldPos)
		{
			MultiBlockManager.TrackedBlockData trackedBlockData;
			if (!this.trackedDataByPosition.TryGetValue(worldPos, out trackedBlockData))
			{
				return;
			}
			RectInt flatChunkBounds = trackedBlockData.flatChunkBounds;
			for (int i = flatChunkBounds.min.y; i <= flatChunkBounds.max.y; i++)
			{
				for (int j = flatChunkBounds.min.x; j <= flatChunkBounds.max.x; j++)
				{
					Vector2i vector2i = new Vector2i(j, i);
					HashSet<Vector3i> hashSet;
					if (!this.oversizedBlocksByChunkPosition.TryGetValue(vector2i, out hashSet))
					{
						hashSet = new HashSet<Vector3i>();
						this.oversizedBlocksByChunkPosition[vector2i] = hashSet;
					}
					hashSet.Add(worldPos);
				}
			}
		}

		// Token: 0x06005929 RID: 22825 RVA: 0x00222CCC File Offset: 0x00220ECC
		[PublicizedFrom(EAccessModifier.Private)]
		public void RemoveOversizedBlockFromChunkBins(Vector3i worldPos)
		{
			MultiBlockManager.TrackedBlockData trackedBlockData;
			if (!this.trackedDataByPosition.TryGetValue(worldPos, out trackedBlockData))
			{
				return;
			}
			RectInt flatChunkBounds = trackedBlockData.flatChunkBounds;
			for (int i = flatChunkBounds.min.y; i <= flatChunkBounds.max.y; i++)
			{
				for (int j = flatChunkBounds.min.x; j <= flatChunkBounds.max.x; j++)
				{
					Vector2i vector2i = new Vector2i(j, i);
					HashSet<Vector3i> hashSet;
					if (this.oversizedBlocksByChunkPosition.TryGetValue(vector2i, out hashSet))
					{
						hashSet.Remove(worldPos);
					}
				}
			}
		}

		// Token: 0x0600592A RID: 22826 RVA: 0x00222D6C File Offset: 0x00220F6C
		public void RemoveTrackedData(Vector3i worldPos, MultiBlockManager.TrackingTypeFlags flagsToRemove)
		{
			MultiBlockManager.TrackedBlockData trackedBlockData;
			if (!this.trackedDataByPosition.TryGetValue(worldPos, out trackedBlockData))
			{
				Log.Error(string.Format("RemoveTrackedData failed; no tracked data at position {0}.", worldPos));
				return;
			}
			MultiBlockManager.TrackingTypeFlags trackingTypeFlags = flagsToRemove & trackedBlockData.trackingTypeFlags;
			if (trackingTypeFlags == MultiBlockManager.TrackingTypeFlags.None)
			{
				Log.Error(string.Format("RemoveTrackedData failed; tracked data at position {0} does not have the target flag(s).", worldPos));
				return;
			}
			if ((trackingTypeFlags & MultiBlockManager.TrackingTypeFlags.PoiMultiBlock) != MultiBlockManager.TrackingTypeFlags.None)
			{
				this.poiMultiBlocks.Remove(worldPos);
			}
			if ((trackingTypeFlags & MultiBlockManager.TrackingTypeFlags.CrossChunkMultiBlock) != MultiBlockManager.TrackingTypeFlags.None)
			{
				this.crossChunkMultiBlocks.Remove(worldPos);
			}
			if ((trackingTypeFlags & MultiBlockManager.TrackingTypeFlags.OversizedBlock) != MultiBlockManager.TrackingTypeFlags.None)
			{
				this.RemoveOversizedBlockFromChunkBins(worldPos);
				this.oversizedBlocks.Remove(worldPos);
			}
			if ((trackingTypeFlags & MultiBlockManager.TrackingTypeFlags.TerrainAlignedBlock) != MultiBlockManager.TrackingTypeFlags.None)
			{
				this.terrainAlignedBlocks.Remove(worldPos);
			}
			MultiBlockManager.TrackingTypeFlags trackingTypeFlags2 = trackedBlockData.trackingTypeFlags & ~flagsToRemove;
			if (trackingTypeFlags2 == MultiBlockManager.TrackingTypeFlags.None)
			{
				this.trackedDataByPosition.Remove(worldPos);
				return;
			}
			this.trackedDataByPosition[worldPos] = new MultiBlockManager.TrackedBlockData(trackedBlockData.rawData, trackedBlockData.flatChunkBounds, trackingTypeFlags2);
		}

		// Token: 0x17000961 RID: 2401
		// (get) Token: 0x0600592B RID: 22827 RVA: 0x00222E4A File Offset: 0x0022104A
		public MultiBlockManager.TrackedDataMap.SubsetAccessor PoiMultiBlocks
		{
			get
			{
				return new MultiBlockManager.TrackedDataMap.SubsetAccessor(this.trackedDataByPosition, this.poiMultiBlocks);
			}
		}

		// Token: 0x17000962 RID: 2402
		// (get) Token: 0x0600592C RID: 22828 RVA: 0x00222E5D File Offset: 0x0022105D
		public MultiBlockManager.TrackedDataMap.SubsetAccessor CrossChunkMultiBlocks
		{
			get
			{
				return new MultiBlockManager.TrackedDataMap.SubsetAccessor(this.trackedDataByPosition, this.crossChunkMultiBlocks);
			}
		}

		// Token: 0x17000963 RID: 2403
		// (get) Token: 0x0600592D RID: 22829 RVA: 0x00222E70 File Offset: 0x00221070
		public MultiBlockManager.TrackedDataMap.SubsetAccessor OversizedBlocks
		{
			get
			{
				return new MultiBlockManager.TrackedDataMap.SubsetAccessor(this.trackedDataByPosition, this.oversizedBlocks);
			}
		}

		// Token: 0x17000964 RID: 2404
		// (get) Token: 0x0600592E RID: 22830 RVA: 0x00222E83 File Offset: 0x00221083
		public MultiBlockManager.TrackedDataMap.SubsetAccessor TerrainAlignedBlocks
		{
			get
			{
				return new MultiBlockManager.TrackedDataMap.SubsetAccessor(this.trackedDataByPosition, this.terrainAlignedBlocks);
			}
		}

		// Token: 0x04004532 RID: 17714
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<Vector3i, MultiBlockManager.TrackedBlockData> trackedDataByPosition = new Dictionary<Vector3i, MultiBlockManager.TrackedBlockData>();

		// Token: 0x04004533 RID: 17715
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HashSet<Vector3i> poiMultiBlocks = new HashSet<Vector3i>();

		// Token: 0x04004534 RID: 17716
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HashSet<Vector3i> crossChunkMultiBlocks = new HashSet<Vector3i>();

		// Token: 0x04004535 RID: 17717
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HashSet<Vector3i> oversizedBlocks = new HashSet<Vector3i>();

		// Token: 0x04004536 RID: 17718
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HashSet<Vector3i> terrainAlignedBlocks = new HashSet<Vector3i>();

		// Token: 0x04004537 RID: 17719
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<Vector2i, HashSet<Vector3i>> oversizedBlocksByChunkPosition = new Dictionary<Vector2i, HashSet<Vector3i>>();

		// Token: 0x04004538 RID: 17720
		public readonly ReadOnlyDictionary<Vector3i, MultiBlockManager.TrackedBlockData> TrackedDataByPosition;

		// Token: 0x04004539 RID: 17721
		public readonly ReadOnlyDictionary<Vector2i, HashSet<Vector3i>> OversizedBlocksByChunkPosition;

		// Token: 0x02000B7E RID: 2942
		public struct SubsetAccessor : IEnumerator<KeyValuePair<Vector3i, MultiBlockManager.TrackedBlockData>>, IEnumerator, IDisposable
		{
			// Token: 0x0600592F RID: 22831 RVA: 0x00222E96 File Offset: 0x00221096
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public SubsetAccessor(Dictionary<Vector3i, MultiBlockManager.TrackedBlockData> trackedData, HashSet<Vector3i> subset)
			{
				this._trackedData = trackedData;
				this._subset = subset;
				this._subsetEnumerator = subset.GetEnumerator();
			}

			// Token: 0x17000965 RID: 2405
			// (get) Token: 0x06005930 RID: 22832 RVA: 0x00222EB4 File Offset: 0x002210B4
			public KeyValuePair<Vector3i, MultiBlockManager.TrackedBlockData> Current
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					Vector3i vector3i = this._subsetEnumerator.Current;
					return new KeyValuePair<Vector3i, MultiBlockManager.TrackedBlockData>(vector3i, this._trackedData[vector3i]);
				}
			}

			// Token: 0x06005931 RID: 22833 RVA: 0x00222EDF File Offset: 0x002210DF
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool MoveNext()
			{
				return this._subsetEnumerator.MoveNext();
			}

			// Token: 0x06005932 RID: 22834 RVA: 0x00006566 File Offset: 0x00004766
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x06005933 RID: 22835 RVA: 0x00222EEC File Offset: 0x002210EC
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Dispose()
			{
				this._subsetEnumerator.Dispose();
			}

			// Token: 0x06005934 RID: 22836 RVA: 0x00222EF9 File Offset: 0x002210F9
			public bool ContainsKey(Vector3i key)
			{
				return this._subset.Contains(key);
			}

			// Token: 0x17000966 RID: 2406
			// (get) Token: 0x06005935 RID: 22837 RVA: 0x00222F07 File Offset: 0x00221107
			public int Count
			{
				get
				{
					return this._subset.Count;
				}
			}

			// Token: 0x06005936 RID: 22838 RVA: 0x00222F14 File Offset: 0x00221114
			public bool TryGetValue(Vector3i key, out MultiBlockManager.TrackedBlockData value)
			{
				if (this._subset.Contains(key) && this._trackedData.TryGetValue(key, out value))
				{
					return true;
				}
				value = default(MultiBlockManager.TrackedBlockData);
				return false;
			}

			// Token: 0x06005937 RID: 22839 RVA: 0x00222F3D File Offset: 0x0022113D
			public MultiBlockManager.TrackedDataMap.SubsetAccessor GetEnumerator()
			{
				return this;
			}

			// Token: 0x17000967 RID: 2407
			// (get) Token: 0x06005938 RID: 22840 RVA: 0x00222F45 File Offset: 0x00221145
			public object System.Collections.IEnumerator.Current
			{
				[PublicizedFrom(EAccessModifier.Private)]
				get
				{
					return this.Current;
				}
			}

			// Token: 0x17000968 RID: 2408
			public MultiBlockManager.TrackedBlockData this[Vector3i key]
			{
				get
				{
					if (!this._subset.Contains(key))
					{
						throw new KeyNotFoundException(string.Format("The key \"{0}\" was not found in the subset.", key));
					}
					return this._trackedData[key];
				}
			}

			// Token: 0x0400453A RID: 17722
			[PublicizedFrom(EAccessModifier.Private)]
			public readonly Dictionary<Vector3i, MultiBlockManager.TrackedBlockData> _trackedData;

			// Token: 0x0400453B RID: 17723
			[PublicizedFrom(EAccessModifier.Private)]
			public readonly HashSet<Vector3i> _subset;

			// Token: 0x0400453C RID: 17724
			[PublicizedFrom(EAccessModifier.Private)]
			public HashSet<Vector3i>.Enumerator _subsetEnumerator;
		}
	}

	// Token: 0x02000B7F RID: 2943
	public enum Mode
	{
		// Token: 0x0400453E RID: 17726
		Disabled,
		// Token: 0x0400453F RID: 17727
		Normal,
		// Token: 0x04004540 RID: 17728
		WorldEditor,
		// Token: 0x04004541 RID: 17729
		PrefabPlaytest,
		// Token: 0x04004542 RID: 17730
		PrefabEditor,
		// Token: 0x04004543 RID: 17731
		Client
	}

	// Token: 0x02000B80 RID: 2944
	[Flags]
	public enum FeatureFlags
	{
		// Token: 0x04004545 RID: 17733
		None = 0,
		// Token: 0x04004546 RID: 17734
		POIMBTracking = 1,
		// Token: 0x04004547 RID: 17735
		Serialization = 2,
		// Token: 0x04004548 RID: 17736
		CrossChunkMBTracking = 4,
		// Token: 0x04004549 RID: 17737
		OversizedStability = 8,
		// Token: 0x0400454A RID: 17738
		TerrainAlignment = 16,
		// Token: 0x0400454B RID: 17739
		All = 31
	}

	// Token: 0x02000B81 RID: 2945
	[PublicizedFrom(EAccessModifier.Private)]
	public enum FeatureRequirement
	{
		// Token: 0x0400454D RID: 17741
		AllEnabled,
		// Token: 0x0400454E RID: 17742
		OneOrMoreEnabled,
		// Token: 0x0400454F RID: 17743
		AllDisabled
	}
}
