using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000B8C RID: 2956
public class MultiBlockManager
{
	// Token: 0x1700097F RID: 2431
	// (get) Token: 0x06005908 RID: 22792 RVA: 0x00223B56 File Offset: 0x00221D56
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

	// Token: 0x06005909 RID: 22793 RVA: 0x00223B70 File Offset: 0x00221D70
	[PublicizedFrom(EAccessModifier.Private)]
	public MultiBlockManager()
	{
	}

	// Token: 0x0600590A RID: 22794 RVA: 0x00223BC5 File Offset: 0x00221DC5
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

	// Token: 0x17000980 RID: 2432
	// (get) Token: 0x0600590B RID: 22795 RVA: 0x00223C04 File Offset: 0x00221E04
	public bool NoFeaturesEnabled
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CheckFeatures(MultiBlockManager.FeatureFlags.All, MultiBlockManager.FeatureRequirement.AllDisabled);
		}
	}

	// Token: 0x0600590C RID: 22796 RVA: 0x00223C10 File Offset: 0x00221E10
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

	// Token: 0x17000981 RID: 2433
	// (get) Token: 0x0600590D RID: 22797 RVA: 0x00223C57 File Offset: 0x00221E57
	public bool POIMBTrackingEnabled
	{
		get
		{
			return this.CheckFeatures(MultiBlockManager.FeatureFlags.POIMBTracking, MultiBlockManager.FeatureRequirement.AllEnabled);
		}
	}

	// Token: 0x0600590E RID: 22798 RVA: 0x00223C64 File Offset: 0x00221E64
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

	// Token: 0x0600590F RID: 22799 RVA: 0x00223DFC File Offset: 0x00221FFC
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

	// Token: 0x06005910 RID: 22800 RVA: 0x00223F2C File Offset: 0x0022212C
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

	// Token: 0x06005911 RID: 22801 RVA: 0x002240C4 File Offset: 0x002222C4
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

	// Token: 0x06005912 RID: 22802 RVA: 0x00224190 File Offset: 0x00222390
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

	// Token: 0x06005913 RID: 22803 RVA: 0x002242C0 File Offset: 0x002224C0
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

	// Token: 0x06005914 RID: 22804 RVA: 0x00224310 File Offset: 0x00222510
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

	// Token: 0x06005915 RID: 22805 RVA: 0x00224374 File Offset: 0x00222574
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

	// Token: 0x06005916 RID: 22806 RVA: 0x00224468 File Offset: 0x00222668
	public void MainThreadUpdate()
	{
		this.UpdateAlignment();
		this.UpdateOversizedStability();
	}

	// Token: 0x06005917 RID: 22807 RVA: 0x00224476 File Offset: 0x00222676
	public bool TryRegisterTerrainAlignedBlock(Vector3i worldPos, BlockValue blockValue)
	{
		return this.TryRegisterTerrainAlignedBlockInternal(worldPos, blockValue);
	}

	// Token: 0x06005918 RID: 22808 RVA: 0x00224480 File Offset: 0x00222680
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

	// Token: 0x06005919 RID: 22809 RVA: 0x00224654 File Offset: 0x00222854
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

	// Token: 0x0600591A RID: 22810 RVA: 0x002246E4 File Offset: 0x002228E4
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

	// Token: 0x0600591B RID: 22811 RVA: 0x00224720 File Offset: 0x00222920
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

	// Token: 0x0600591C RID: 22812 RVA: 0x002247E8 File Offset: 0x002229E8
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

	// Token: 0x0600591D RID: 22813 RVA: 0x002248C0 File Offset: 0x00222AC0
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

	// Token: 0x0600591E RID: 22814 RVA: 0x00224A7C File Offset: 0x00222C7C
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

	// Token: 0x0600591F RID: 22815 RVA: 0x00224B00 File Offset: 0x00222D00
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

	// Token: 0x06005920 RID: 22816 RVA: 0x00224B64 File Offset: 0x00222D64
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

	// Token: 0x06005921 RID: 22817 RVA: 0x00224D10 File Offset: 0x00222F10
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

	// Token: 0x06005922 RID: 22818 RVA: 0x00224DF0 File Offset: 0x00222FF0
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

	// Token: 0x06005923 RID: 22819 RVA: 0x00225020 File Offset: 0x00223220
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

	// Token: 0x06005924 RID: 22820 RVA: 0x00225128 File Offset: 0x00223328
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

	// Token: 0x06005925 RID: 22821 RVA: 0x00225228 File Offset: 0x00223428
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

	// Token: 0x06005926 RID: 22822 RVA: 0x00225394 File Offset: 0x00223594
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

	// Token: 0x06005927 RID: 22823 RVA: 0x00225404 File Offset: 0x00223604
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

	// Token: 0x06005928 RID: 22824 RVA: 0x002254A0 File Offset: 0x002236A0
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

	// Token: 0x06005929 RID: 22825 RVA: 0x00225510 File Offset: 0x00223710
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

	// Token: 0x0600592A RID: 22826 RVA: 0x002255A4 File Offset: 0x002237A4
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

	// Token: 0x0600592B RID: 22827 RVA: 0x00067DC8 File Offset: 0x00065FC8
	[Conditional("MBM_ENABLE_GENERAL_LOG")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void DebugLogGeneral(string message)
	{
		Log.Out(message);
	}

	// Token: 0x0600592C RID: 22828 RVA: 0x00067DC8 File Offset: 0x00065FC8
	[Conditional("MBM_ENABLE_PLACEMENT_LOG")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void DebugLogPlacement(string message)
	{
		Log.Out(message);
	}

	// Token: 0x0600592D RID: 22829 RVA: 0x00067DC8 File Offset: 0x00065FC8
	[Conditional("MBM_ENABLE_REGISTRATION_LOG")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void DebugLogRegistration(string message)
	{
		Log.Out(message);
	}

	// Token: 0x0600592E RID: 22830 RVA: 0x00067DC8 File Offset: 0x00065FC8
	[Conditional("MBM_ENABLE_STABILITY_LOG")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void DebugLogStability(string message)
	{
		Log.Out(message);
	}

	// Token: 0x0600592F RID: 22831 RVA: 0x00067DC8 File Offset: 0x00065FC8
	[Conditional("MBM_ENABLE_ALIGNMENT_LOG")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void DebugLogAlignment(string message)
	{
		Log.Out(message);
	}

	// Token: 0x06005930 RID: 22832 RVA: 0x000027FC File Offset: 0x000009FC
	[Conditional("MBM_ENABLE_PROFILER_MARKERS")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateProfilerCounters()
	{
	}

	// Token: 0x06005931 RID: 22833 RVA: 0x0022567C File Offset: 0x0022387C
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

	// Token: 0x06005932 RID: 22834 RVA: 0x00225724 File Offset: 0x00223924
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

	// Token: 0x06005933 RID: 22835 RVA: 0x00225864 File Offset: 0x00223A64
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

	// Token: 0x06005934 RID: 22836 RVA: 0x002258C8 File Offset: 0x00223AC8
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <TryRegisterTerrainAlignedBlockInternal>g__RegisterTerrainAlignedBlock|43_0(Vector3i worldPos, BlockValue blockValue, RectInt flatChunkBounds)
	{
		this.trackedDataMap.AddOrMergeTrackedData(worldPos, blockValue.rawData, flatChunkBounds, MultiBlockManager.TrackingTypeFlags.TerrainAlignedBlock);
		this.blocksWithDirtyAlignment.Add(worldPos);
		this.isDirty = true;
	}

	// Token: 0x06005935 RID: 22837 RVA: 0x002258F4 File Offset: 0x00223AF4
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

	// Token: 0x06005936 RID: 22838 RVA: 0x00225978 File Offset: 0x00223B78
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

	// Token: 0x06005937 RID: 22839 RVA: 0x00225A10 File Offset: 0x00223C10
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

	// Token: 0x06005938 RID: 22840 RVA: 0x00225A74 File Offset: 0x00223C74
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

	// Token: 0x06005939 RID: 22841 RVA: 0x00225AD8 File Offset: 0x00223CD8
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

	// Token: 0x0600593A RID: 22842 RVA: 0x00225B40 File Offset: 0x00223D40
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

	// Token: 0x04004554 RID: 17748
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte FILEVERSION = 6;

	// Token: 0x04004555 RID: 17749
	[PublicizedFrom(EAccessModifier.Private)]
	public const int c_deregisteredMultiBlockLimit = 20;

	// Token: 0x04004556 RID: 17750
	[PublicizedFrom(EAccessModifier.Private)]
	public object lockObj = new object();

	// Token: 0x04004557 RID: 17751
	[PublicizedFrom(EAccessModifier.Private)]
	public static MultiBlockManager m_Instance;

	// Token: 0x04004558 RID: 17752
	[PublicizedFrom(EAccessModifier.Private)]
	public RegionFileManager regionFileManager;

	// Token: 0x04004559 RID: 17753
	[PublicizedFrom(EAccessModifier.Private)]
	public MultiBlockManager.TrackedDataMap trackedDataMap = new MultiBlockManager.TrackedDataMap();

	// Token: 0x0400455A RID: 17754
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<Vector3i> oversizedBlocksWithDirtyStability = new HashSet<Vector3i>();

	// Token: 0x0400455B RID: 17755
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<Vector3i> blocksWithDirtyAlignment = new HashSet<Vector3i>();

	// Token: 0x0400455C RID: 17756
	[PublicizedFrom(EAccessModifier.Private)]
	public Queue<Vector3i> keysToRemove = new Queue<Vector3i>();

	// Token: 0x0400455D RID: 17757
	[PublicizedFrom(EAccessModifier.Private)]
	public List<long> tempChunksToGroup = new List<long>();

	// Token: 0x0400455E RID: 17758
	[PublicizedFrom(EAccessModifier.Private)]
	public World world;

	// Token: 0x0400455F RID: 17759
	[PublicizedFrom(EAccessModifier.Private)]
	public ChunkCluster cc;

	// Token: 0x04004560 RID: 17760
	[PublicizedFrom(EAccessModifier.Private)]
	public ChunkManager chunkManager;

	// Token: 0x04004561 RID: 17761
	[PublicizedFrom(EAccessModifier.Private)]
	public string filePath;

	// Token: 0x04004562 RID: 17762
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isDirty;

	// Token: 0x04004563 RID: 17763
	[PublicizedFrom(EAccessModifier.Private)]
	public int deregisteredMultiBlockCount;

	// Token: 0x04004564 RID: 17764
	[PublicizedFrom(EAccessModifier.Private)]
	public MultiBlockManager.Mode currentMode;

	// Token: 0x04004565 RID: 17765
	[PublicizedFrom(EAccessModifier.Private)]
	public MultiBlockManager.FeatureFlags enabledFeatures;

	// Token: 0x02000B8D RID: 2957
	[Flags]
	public enum TrackingTypeFlags : byte
	{
		// Token: 0x04004567 RID: 17767
		None = 0,
		// Token: 0x04004568 RID: 17768
		PoiMultiBlock = 1,
		// Token: 0x04004569 RID: 17769
		CrossChunkMultiBlock = 2,
		// Token: 0x0400456A RID: 17770
		OversizedBlock = 4,
		// Token: 0x0400456B RID: 17771
		TerrainAlignedBlock = 8,
		// Token: 0x0400456C RID: 17772
		All = 15
	}

	// Token: 0x02000B8E RID: 2958
	public struct TrackedBlockData
	{
		// Token: 0x0600593B RID: 22843 RVA: 0x00225C2F File Offset: 0x00223E2F
		public TrackedBlockData(uint rawData, RectInt flatChunkBounds, MultiBlockManager.TrackingTypeFlags trackingTypeFlags)
		{
			this.rawData = rawData;
			this.flatChunkBounds = flatChunkBounds;
			this.trackingTypeFlags = trackingTypeFlags;
		}

		// Token: 0x0400456D RID: 17773
		public readonly uint rawData;

		// Token: 0x0400456E RID: 17774
		public readonly RectInt flatChunkBounds;

		// Token: 0x0400456F RID: 17775
		public readonly MultiBlockManager.TrackingTypeFlags trackingTypeFlags;
	}

	// Token: 0x02000B8F RID: 2959
	[PublicizedFrom(EAccessModifier.Private)]
	public class TrackedDataMap
	{
		// Token: 0x0600593C RID: 22844 RVA: 0x00225C48 File Offset: 0x00223E48
		public TrackedDataMap()
		{
			this.TrackedDataByPosition = new ReadOnlyDictionary<Vector3i, MultiBlockManager.TrackedBlockData>(this.trackedDataByPosition);
			this.OversizedBlocksByChunkPosition = new ReadOnlyDictionary<Vector2i, HashSet<Vector3i>>(this.oversizedBlocksByChunkPosition);
		}

		// Token: 0x0600593D RID: 22845 RVA: 0x00225CBF File Offset: 0x00223EBF
		public bool ContainsKey(Vector3i key)
		{
			return this.trackedDataByPosition.ContainsKey(key);
		}

		// Token: 0x17000982 RID: 2434
		// (get) Token: 0x0600593E RID: 22846 RVA: 0x00225CCD File Offset: 0x00223ECD
		public int Count
		{
			get
			{
				return this.trackedDataByPosition.Count;
			}
		}

		// Token: 0x0600593F RID: 22847 RVA: 0x00225CDA File Offset: 0x00223EDA
		public bool TryGetValue(Vector3i key, out MultiBlockManager.TrackedBlockData value)
		{
			return this.trackedDataByPosition.TryGetValue(key, out value);
		}

		// Token: 0x06005940 RID: 22848 RVA: 0x00225CEC File Offset: 0x00223EEC
		public void Clear()
		{
			this.trackedDataByPosition.Clear();
			this.poiMultiBlocks.Clear();
			this.crossChunkMultiBlocks.Clear();
			this.oversizedBlocks.Clear();
			this.terrainAlignedBlocks.Clear();
			this.oversizedBlocksByChunkPosition.Clear();
		}

		// Token: 0x06005941 RID: 22849 RVA: 0x00225D3C File Offset: 0x00223F3C
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

		// Token: 0x06005942 RID: 22850 RVA: 0x00225EA4 File Offset: 0x002240A4
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

		// Token: 0x06005943 RID: 22851 RVA: 0x00225F58 File Offset: 0x00224158
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

		// Token: 0x06005944 RID: 22852 RVA: 0x00225FF8 File Offset: 0x002241F8
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

		// Token: 0x17000983 RID: 2435
		// (get) Token: 0x06005945 RID: 22853 RVA: 0x002260D6 File Offset: 0x002242D6
		public MultiBlockManager.TrackedDataMap.SubsetAccessor PoiMultiBlocks
		{
			get
			{
				return new MultiBlockManager.TrackedDataMap.SubsetAccessor(this.trackedDataByPosition, this.poiMultiBlocks);
			}
		}

		// Token: 0x17000984 RID: 2436
		// (get) Token: 0x06005946 RID: 22854 RVA: 0x002260E9 File Offset: 0x002242E9
		public MultiBlockManager.TrackedDataMap.SubsetAccessor CrossChunkMultiBlocks
		{
			get
			{
				return new MultiBlockManager.TrackedDataMap.SubsetAccessor(this.trackedDataByPosition, this.crossChunkMultiBlocks);
			}
		}

		// Token: 0x17000985 RID: 2437
		// (get) Token: 0x06005947 RID: 22855 RVA: 0x002260FC File Offset: 0x002242FC
		public MultiBlockManager.TrackedDataMap.SubsetAccessor OversizedBlocks
		{
			get
			{
				return new MultiBlockManager.TrackedDataMap.SubsetAccessor(this.trackedDataByPosition, this.oversizedBlocks);
			}
		}

		// Token: 0x17000986 RID: 2438
		// (get) Token: 0x06005948 RID: 22856 RVA: 0x0022610F File Offset: 0x0022430F
		public MultiBlockManager.TrackedDataMap.SubsetAccessor TerrainAlignedBlocks
		{
			get
			{
				return new MultiBlockManager.TrackedDataMap.SubsetAccessor(this.trackedDataByPosition, this.terrainAlignedBlocks);
			}
		}

		// Token: 0x04004570 RID: 17776
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<Vector3i, MultiBlockManager.TrackedBlockData> trackedDataByPosition = new Dictionary<Vector3i, MultiBlockManager.TrackedBlockData>();

		// Token: 0x04004571 RID: 17777
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HashSet<Vector3i> poiMultiBlocks = new HashSet<Vector3i>();

		// Token: 0x04004572 RID: 17778
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HashSet<Vector3i> crossChunkMultiBlocks = new HashSet<Vector3i>();

		// Token: 0x04004573 RID: 17779
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HashSet<Vector3i> oversizedBlocks = new HashSet<Vector3i>();

		// Token: 0x04004574 RID: 17780
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HashSet<Vector3i> terrainAlignedBlocks = new HashSet<Vector3i>();

		// Token: 0x04004575 RID: 17781
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<Vector2i, HashSet<Vector3i>> oversizedBlocksByChunkPosition = new Dictionary<Vector2i, HashSet<Vector3i>>();

		// Token: 0x04004576 RID: 17782
		public readonly ReadOnlyDictionary<Vector3i, MultiBlockManager.TrackedBlockData> TrackedDataByPosition;

		// Token: 0x04004577 RID: 17783
		public readonly ReadOnlyDictionary<Vector2i, HashSet<Vector3i>> OversizedBlocksByChunkPosition;

		// Token: 0x02000B90 RID: 2960
		public struct SubsetAccessor : IEnumerator<KeyValuePair<Vector3i, MultiBlockManager.TrackedBlockData>>, IEnumerator, IDisposable
		{
			// Token: 0x06005949 RID: 22857 RVA: 0x00226122 File Offset: 0x00224322
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public SubsetAccessor(Dictionary<Vector3i, MultiBlockManager.TrackedBlockData> trackedData, HashSet<Vector3i> subset)
			{
				this._trackedData = trackedData;
				this._subset = subset;
				this._subsetEnumerator = subset.GetEnumerator();
			}

			// Token: 0x17000987 RID: 2439
			// (get) Token: 0x0600594A RID: 22858 RVA: 0x00226140 File Offset: 0x00224340
			public KeyValuePair<Vector3i, MultiBlockManager.TrackedBlockData> Current
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					Vector3i vector3i = this._subsetEnumerator.Current;
					return new KeyValuePair<Vector3i, MultiBlockManager.TrackedBlockData>(vector3i, this._trackedData[vector3i]);
				}
			}

			// Token: 0x0600594B RID: 22859 RVA: 0x0022616B File Offset: 0x0022436B
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool MoveNext()
			{
				return this._subsetEnumerator.MoveNext();
			}

			// Token: 0x0600594C RID: 22860 RVA: 0x00006566 File Offset: 0x00004766
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x0600594D RID: 22861 RVA: 0x00226178 File Offset: 0x00224378
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Dispose()
			{
				this._subsetEnumerator.Dispose();
			}

			// Token: 0x0600594E RID: 22862 RVA: 0x00226185 File Offset: 0x00224385
			public bool ContainsKey(Vector3i key)
			{
				return this._subset.Contains(key);
			}

			// Token: 0x17000988 RID: 2440
			// (get) Token: 0x0600594F RID: 22863 RVA: 0x00226193 File Offset: 0x00224393
			public int Count
			{
				get
				{
					return this._subset.Count;
				}
			}

			// Token: 0x06005950 RID: 22864 RVA: 0x002261A0 File Offset: 0x002243A0
			public bool TryGetValue(Vector3i key, out MultiBlockManager.TrackedBlockData value)
			{
				if (this._subset.Contains(key) && this._trackedData.TryGetValue(key, out value))
				{
					return true;
				}
				value = default(MultiBlockManager.TrackedBlockData);
				return false;
			}

			// Token: 0x06005951 RID: 22865 RVA: 0x002261C9 File Offset: 0x002243C9
			public MultiBlockManager.TrackedDataMap.SubsetAccessor GetEnumerator()
			{
				return this;
			}

			// Token: 0x17000989 RID: 2441
			// (get) Token: 0x06005952 RID: 22866 RVA: 0x002261D1 File Offset: 0x002243D1
			public object System.Collections.IEnumerator.Current
			{
				[PublicizedFrom(EAccessModifier.Private)]
				get
				{
					return this.Current;
				}
			}

			// Token: 0x1700098A RID: 2442
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

			// Token: 0x04004578 RID: 17784
			[PublicizedFrom(EAccessModifier.Private)]
			public readonly Dictionary<Vector3i, MultiBlockManager.TrackedBlockData> _trackedData;

			// Token: 0x04004579 RID: 17785
			[PublicizedFrom(EAccessModifier.Private)]
			public readonly HashSet<Vector3i> _subset;

			// Token: 0x0400457A RID: 17786
			[PublicizedFrom(EAccessModifier.Private)]
			public HashSet<Vector3i>.Enumerator _subsetEnumerator;
		}
	}

	// Token: 0x02000B91 RID: 2961
	public enum Mode
	{
		// Token: 0x0400457C RID: 17788
		Disabled,
		// Token: 0x0400457D RID: 17789
		Normal,
		// Token: 0x0400457E RID: 17790
		WorldEditor,
		// Token: 0x0400457F RID: 17791
		PrefabPlaytest,
		// Token: 0x04004580 RID: 17792
		PrefabEditor,
		// Token: 0x04004581 RID: 17793
		Client
	}

	// Token: 0x02000B92 RID: 2962
	[Flags]
	public enum FeatureFlags
	{
		// Token: 0x04004583 RID: 17795
		None = 0,
		// Token: 0x04004584 RID: 17796
		POIMBTracking = 1,
		// Token: 0x04004585 RID: 17797
		Serialization = 2,
		// Token: 0x04004586 RID: 17798
		CrossChunkMBTracking = 4,
		// Token: 0x04004587 RID: 17799
		OversizedStability = 8,
		// Token: 0x04004588 RID: 17800
		TerrainAlignment = 16,
		// Token: 0x04004589 RID: 17801
		All = 31
	}

	// Token: 0x02000B93 RID: 2963
	[PublicizedFrom(EAccessModifier.Private)]
	public enum FeatureRequirement
	{
		// Token: 0x0400458B RID: 17803
		AllEnabled,
		// Token: 0x0400458C RID: 17804
		OneOrMoreEnabled,
		// Token: 0x0400458D RID: 17805
		AllDisabled
	}
}
