using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000B97 RID: 2967
public class MultiBlockManager
{
	// Token: 0x17000985 RID: 2437
	// (get) Token: 0x0600592D RID: 22829 RVA: 0x002250B2 File Offset: 0x002232B2
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

	// Token: 0x0600592E RID: 22830 RVA: 0x002250CC File Offset: 0x002232CC
	[PublicizedFrom(EAccessModifier.Private)]
	public MultiBlockManager()
	{
	}

	// Token: 0x0600592F RID: 22831 RVA: 0x00225121 File Offset: 0x00223321
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

	// Token: 0x17000986 RID: 2438
	// (get) Token: 0x06005930 RID: 22832 RVA: 0x00225160 File Offset: 0x00223360
	public bool NoFeaturesEnabled
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CheckFeatures(MultiBlockManager.FeatureFlags.All, MultiBlockManager.FeatureRequirement.AllDisabled);
		}
	}

	// Token: 0x06005931 RID: 22833 RVA: 0x0022516C File Offset: 0x0022336C
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

	// Token: 0x17000987 RID: 2439
	// (get) Token: 0x06005932 RID: 22834 RVA: 0x002251B3 File Offset: 0x002233B3
	public bool POIMBTrackingEnabled
	{
		get
		{
			return this.CheckFeatures(MultiBlockManager.FeatureFlags.POIMBTracking, MultiBlockManager.FeatureRequirement.AllEnabled);
		}
	}

	// Token: 0x06005933 RID: 22835 RVA: 0x002251C0 File Offset: 0x002233C0
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

	// Token: 0x06005934 RID: 22836 RVA: 0x00225358 File Offset: 0x00223558
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

	// Token: 0x06005935 RID: 22837 RVA: 0x00225488 File Offset: 0x00223688
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

	// Token: 0x06005936 RID: 22838 RVA: 0x00225620 File Offset: 0x00223820
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

	// Token: 0x06005937 RID: 22839 RVA: 0x002256EC File Offset: 0x002238EC
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

	// Token: 0x06005938 RID: 22840 RVA: 0x0022581C File Offset: 0x00223A1C
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

	// Token: 0x06005939 RID: 22841 RVA: 0x0022586C File Offset: 0x00223A6C
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

	// Token: 0x0600593A RID: 22842 RVA: 0x002258D0 File Offset: 0x00223AD0
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

	// Token: 0x0600593B RID: 22843 RVA: 0x002259C4 File Offset: 0x00223BC4
	public void MainThreadUpdate()
	{
		this.UpdateAlignment();
		this.UpdateOversizedStability();
	}

	// Token: 0x0600593C RID: 22844 RVA: 0x002259D2 File Offset: 0x00223BD2
	public bool TryRegisterTerrainAlignedBlock(Vector3i worldPos, BlockValue blockValue)
	{
		return this.TryRegisterTerrainAlignedBlockInternal(worldPos, blockValue);
	}

	// Token: 0x0600593D RID: 22845 RVA: 0x002259DC File Offset: 0x00223BDC
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

	// Token: 0x0600593E RID: 22846 RVA: 0x00225BB0 File Offset: 0x00223DB0
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

	// Token: 0x0600593F RID: 22847 RVA: 0x00225C40 File Offset: 0x00223E40
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

	// Token: 0x06005940 RID: 22848 RVA: 0x00225C7C File Offset: 0x00223E7C
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

	// Token: 0x06005941 RID: 22849 RVA: 0x00225D44 File Offset: 0x00223F44
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

	// Token: 0x06005942 RID: 22850 RVA: 0x00225E1C File Offset: 0x0022401C
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

	// Token: 0x06005943 RID: 22851 RVA: 0x00225FD8 File Offset: 0x002241D8
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

	// Token: 0x06005944 RID: 22852 RVA: 0x0022605C File Offset: 0x0022425C
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

	// Token: 0x06005945 RID: 22853 RVA: 0x002260C0 File Offset: 0x002242C0
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

	// Token: 0x06005946 RID: 22854 RVA: 0x0022626C File Offset: 0x0022446C
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

	// Token: 0x06005947 RID: 22855 RVA: 0x0022634C File Offset: 0x0022454C
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

	// Token: 0x06005948 RID: 22856 RVA: 0x0022657C File Offset: 0x0022477C
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

	// Token: 0x06005949 RID: 22857 RVA: 0x00226684 File Offset: 0x00224884
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

	// Token: 0x0600594A RID: 22858 RVA: 0x00226784 File Offset: 0x00224984
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

	// Token: 0x0600594B RID: 22859 RVA: 0x002268F0 File Offset: 0x00224AF0
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

	// Token: 0x0600594C RID: 22860 RVA: 0x00226960 File Offset: 0x00224B60
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

	// Token: 0x0600594D RID: 22861 RVA: 0x002269FC File Offset: 0x00224BFC
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

	// Token: 0x0600594E RID: 22862 RVA: 0x00226A6C File Offset: 0x00224C6C
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

	// Token: 0x0600594F RID: 22863 RVA: 0x00226B00 File Offset: 0x00224D00
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

	// Token: 0x06005950 RID: 22864 RVA: 0x00067F98 File Offset: 0x00066198
	[Conditional("MBM_ENABLE_GENERAL_LOG")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void DebugLogGeneral(string message)
	{
		Log.Out(message);
	}

	// Token: 0x06005951 RID: 22865 RVA: 0x00067F98 File Offset: 0x00066198
	[Conditional("MBM_ENABLE_PLACEMENT_LOG")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void DebugLogPlacement(string message)
	{
		Log.Out(message);
	}

	// Token: 0x06005952 RID: 22866 RVA: 0x00067F98 File Offset: 0x00066198
	[Conditional("MBM_ENABLE_REGISTRATION_LOG")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void DebugLogRegistration(string message)
	{
		Log.Out(message);
	}

	// Token: 0x06005953 RID: 22867 RVA: 0x00067F98 File Offset: 0x00066198
	[Conditional("MBM_ENABLE_STABILITY_LOG")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void DebugLogStability(string message)
	{
		Log.Out(message);
	}

	// Token: 0x06005954 RID: 22868 RVA: 0x00067F98 File Offset: 0x00066198
	[Conditional("MBM_ENABLE_ALIGNMENT_LOG")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void DebugLogAlignment(string message)
	{
		Log.Out(message);
	}

	// Token: 0x06005955 RID: 22869 RVA: 0x000027FC File Offset: 0x000009FC
	[Conditional("MBM_ENABLE_PROFILER_MARKERS")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateProfilerCounters()
	{
	}

	// Token: 0x06005956 RID: 22870 RVA: 0x00226BD8 File Offset: 0x00224DD8
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

	// Token: 0x06005957 RID: 22871 RVA: 0x00226C80 File Offset: 0x00224E80
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

	// Token: 0x06005958 RID: 22872 RVA: 0x00226DC0 File Offset: 0x00224FC0
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

	// Token: 0x06005959 RID: 22873 RVA: 0x00226E24 File Offset: 0x00225024
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <TryRegisterTerrainAlignedBlockInternal>g__RegisterTerrainAlignedBlock|43_0(Vector3i worldPos, BlockValue blockValue, RectInt flatChunkBounds)
	{
		this.trackedDataMap.AddOrMergeTrackedData(worldPos, blockValue.rawData, flatChunkBounds, MultiBlockManager.TrackingTypeFlags.TerrainAlignedBlock);
		this.blocksWithDirtyAlignment.Add(worldPos);
		this.isDirty = true;
	}

	// Token: 0x0600595A RID: 22874 RVA: 0x00226E50 File Offset: 0x00225050
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

	// Token: 0x0600595B RID: 22875 RVA: 0x00226ED4 File Offset: 0x002250D4
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

	// Token: 0x0600595C RID: 22876 RVA: 0x00226F6C File Offset: 0x0022516C
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

	// Token: 0x0600595D RID: 22877 RVA: 0x00226FD0 File Offset: 0x002251D0
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

	// Token: 0x0600595E RID: 22878 RVA: 0x00227034 File Offset: 0x00225234
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

	// Token: 0x0600595F RID: 22879 RVA: 0x0022709C File Offset: 0x0022529C
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

	// Token: 0x04004579 RID: 17785
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte FILEVERSION = 6;

	// Token: 0x0400457A RID: 17786
	[PublicizedFrom(EAccessModifier.Private)]
	public const int c_deregisteredMultiBlockLimit = 20;

	// Token: 0x0400457B RID: 17787
	[PublicizedFrom(EAccessModifier.Private)]
	public object lockObj = new object();

	// Token: 0x0400457C RID: 17788
	[PublicizedFrom(EAccessModifier.Private)]
	public static MultiBlockManager m_Instance;

	// Token: 0x0400457D RID: 17789
	[PublicizedFrom(EAccessModifier.Private)]
	public RegionFileManager regionFileManager;

	// Token: 0x0400457E RID: 17790
	[PublicizedFrom(EAccessModifier.Private)]
	public MultiBlockManager.TrackedDataMap trackedDataMap = new MultiBlockManager.TrackedDataMap();

	// Token: 0x0400457F RID: 17791
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<Vector3i> oversizedBlocksWithDirtyStability = new HashSet<Vector3i>();

	// Token: 0x04004580 RID: 17792
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<Vector3i> blocksWithDirtyAlignment = new HashSet<Vector3i>();

	// Token: 0x04004581 RID: 17793
	[PublicizedFrom(EAccessModifier.Private)]
	public Queue<Vector3i> keysToRemove = new Queue<Vector3i>();

	// Token: 0x04004582 RID: 17794
	[PublicizedFrom(EAccessModifier.Private)]
	public List<long> tempChunksToGroup = new List<long>();

	// Token: 0x04004583 RID: 17795
	[PublicizedFrom(EAccessModifier.Private)]
	public World world;

	// Token: 0x04004584 RID: 17796
	[PublicizedFrom(EAccessModifier.Private)]
	public ChunkCluster cc;

	// Token: 0x04004585 RID: 17797
	[PublicizedFrom(EAccessModifier.Private)]
	public ChunkManager chunkManager;

	// Token: 0x04004586 RID: 17798
	[PublicizedFrom(EAccessModifier.Private)]
	public string filePath;

	// Token: 0x04004587 RID: 17799
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isDirty;

	// Token: 0x04004588 RID: 17800
	[PublicizedFrom(EAccessModifier.Private)]
	public int deregisteredMultiBlockCount;

	// Token: 0x04004589 RID: 17801
	[PublicizedFrom(EAccessModifier.Private)]
	public MultiBlockManager.Mode currentMode;

	// Token: 0x0400458A RID: 17802
	[PublicizedFrom(EAccessModifier.Private)]
	public MultiBlockManager.FeatureFlags enabledFeatures;

	// Token: 0x02000B98 RID: 2968
	[Flags]
	public enum TrackingTypeFlags : byte
	{
		// Token: 0x0400458C RID: 17804
		None = 0,
		// Token: 0x0400458D RID: 17805
		PoiMultiBlock = 1,
		// Token: 0x0400458E RID: 17806
		CrossChunkMultiBlock = 2,
		// Token: 0x0400458F RID: 17807
		OversizedBlock = 4,
		// Token: 0x04004590 RID: 17808
		TerrainAlignedBlock = 8,
		// Token: 0x04004591 RID: 17809
		All = 15
	}

	// Token: 0x02000B99 RID: 2969
	public struct TrackedBlockData
	{
		// Token: 0x06005960 RID: 22880 RVA: 0x0022718B File Offset: 0x0022538B
		public TrackedBlockData(uint rawData, RectInt flatChunkBounds, MultiBlockManager.TrackingTypeFlags trackingTypeFlags)
		{
			this.rawData = rawData;
			this.flatChunkBounds = flatChunkBounds;
			this.trackingTypeFlags = trackingTypeFlags;
		}

		// Token: 0x04004592 RID: 17810
		public readonly uint rawData;

		// Token: 0x04004593 RID: 17811
		public readonly RectInt flatChunkBounds;

		// Token: 0x04004594 RID: 17812
		public readonly MultiBlockManager.TrackingTypeFlags trackingTypeFlags;
	}

	// Token: 0x02000B9A RID: 2970
	[PublicizedFrom(EAccessModifier.Private)]
	public class TrackedDataMap
	{
		// Token: 0x06005961 RID: 22881 RVA: 0x002271A4 File Offset: 0x002253A4
		public TrackedDataMap()
		{
			this.TrackedDataByPosition = new ReadOnlyDictionary<Vector3i, MultiBlockManager.TrackedBlockData>(this.trackedDataByPosition);
			this.OversizedBlocksByChunkPosition = new ReadOnlyDictionary<Vector2i, HashSet<Vector3i>>(this.oversizedBlocksByChunkPosition);
		}

		// Token: 0x06005962 RID: 22882 RVA: 0x0022721B File Offset: 0x0022541B
		public bool ContainsKey(Vector3i key)
		{
			return this.trackedDataByPosition.ContainsKey(key);
		}

		// Token: 0x17000988 RID: 2440
		// (get) Token: 0x06005963 RID: 22883 RVA: 0x00227229 File Offset: 0x00225429
		public int Count
		{
			get
			{
				return this.trackedDataByPosition.Count;
			}
		}

		// Token: 0x06005964 RID: 22884 RVA: 0x00227236 File Offset: 0x00225436
		public bool TryGetValue(Vector3i key, out MultiBlockManager.TrackedBlockData value)
		{
			return this.trackedDataByPosition.TryGetValue(key, out value);
		}

		// Token: 0x06005965 RID: 22885 RVA: 0x00227248 File Offset: 0x00225448
		public void Clear()
		{
			this.trackedDataByPosition.Clear();
			this.poiMultiBlocks.Clear();
			this.crossChunkMultiBlocks.Clear();
			this.oversizedBlocks.Clear();
			this.terrainAlignedBlocks.Clear();
			this.oversizedBlocksByChunkPosition.Clear();
		}

		// Token: 0x06005966 RID: 22886 RVA: 0x00227298 File Offset: 0x00225498
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

		// Token: 0x06005967 RID: 22887 RVA: 0x00227400 File Offset: 0x00225600
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

		// Token: 0x06005968 RID: 22888 RVA: 0x002274B4 File Offset: 0x002256B4
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

		// Token: 0x06005969 RID: 22889 RVA: 0x00227554 File Offset: 0x00225754
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

		// Token: 0x17000989 RID: 2441
		// (get) Token: 0x0600596A RID: 22890 RVA: 0x00227632 File Offset: 0x00225832
		public MultiBlockManager.TrackedDataMap.SubsetAccessor PoiMultiBlocks
		{
			get
			{
				return new MultiBlockManager.TrackedDataMap.SubsetAccessor(this.trackedDataByPosition, this.poiMultiBlocks);
			}
		}

		// Token: 0x1700098A RID: 2442
		// (get) Token: 0x0600596B RID: 22891 RVA: 0x00227645 File Offset: 0x00225845
		public MultiBlockManager.TrackedDataMap.SubsetAccessor CrossChunkMultiBlocks
		{
			get
			{
				return new MultiBlockManager.TrackedDataMap.SubsetAccessor(this.trackedDataByPosition, this.crossChunkMultiBlocks);
			}
		}

		// Token: 0x1700098B RID: 2443
		// (get) Token: 0x0600596C RID: 22892 RVA: 0x00227658 File Offset: 0x00225858
		public MultiBlockManager.TrackedDataMap.SubsetAccessor OversizedBlocks
		{
			get
			{
				return new MultiBlockManager.TrackedDataMap.SubsetAccessor(this.trackedDataByPosition, this.oversizedBlocks);
			}
		}

		// Token: 0x1700098C RID: 2444
		// (get) Token: 0x0600596D RID: 22893 RVA: 0x0022766B File Offset: 0x0022586B
		public MultiBlockManager.TrackedDataMap.SubsetAccessor TerrainAlignedBlocks
		{
			get
			{
				return new MultiBlockManager.TrackedDataMap.SubsetAccessor(this.trackedDataByPosition, this.terrainAlignedBlocks);
			}
		}

		// Token: 0x04004595 RID: 17813
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<Vector3i, MultiBlockManager.TrackedBlockData> trackedDataByPosition = new Dictionary<Vector3i, MultiBlockManager.TrackedBlockData>();

		// Token: 0x04004596 RID: 17814
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HashSet<Vector3i> poiMultiBlocks = new HashSet<Vector3i>();

		// Token: 0x04004597 RID: 17815
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HashSet<Vector3i> crossChunkMultiBlocks = new HashSet<Vector3i>();

		// Token: 0x04004598 RID: 17816
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HashSet<Vector3i> oversizedBlocks = new HashSet<Vector3i>();

		// Token: 0x04004599 RID: 17817
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HashSet<Vector3i> terrainAlignedBlocks = new HashSet<Vector3i>();

		// Token: 0x0400459A RID: 17818
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<Vector2i, HashSet<Vector3i>> oversizedBlocksByChunkPosition = new Dictionary<Vector2i, HashSet<Vector3i>>();

		// Token: 0x0400459B RID: 17819
		public readonly ReadOnlyDictionary<Vector3i, MultiBlockManager.TrackedBlockData> TrackedDataByPosition;

		// Token: 0x0400459C RID: 17820
		public readonly ReadOnlyDictionary<Vector2i, HashSet<Vector3i>> OversizedBlocksByChunkPosition;

		// Token: 0x02000B9B RID: 2971
		public struct SubsetAccessor : IEnumerator<KeyValuePair<Vector3i, MultiBlockManager.TrackedBlockData>>, IEnumerator, IDisposable
		{
			// Token: 0x0600596E RID: 22894 RVA: 0x0022767E File Offset: 0x0022587E
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public SubsetAccessor(Dictionary<Vector3i, MultiBlockManager.TrackedBlockData> trackedData, HashSet<Vector3i> subset)
			{
				this._trackedData = trackedData;
				this._subset = subset;
				this._subsetEnumerator = subset.GetEnumerator();
			}

			// Token: 0x1700098D RID: 2445
			// (get) Token: 0x0600596F RID: 22895 RVA: 0x0022769C File Offset: 0x0022589C
			public KeyValuePair<Vector3i, MultiBlockManager.TrackedBlockData> Current
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					Vector3i vector3i = this._subsetEnumerator.Current;
					return new KeyValuePair<Vector3i, MultiBlockManager.TrackedBlockData>(vector3i, this._trackedData[vector3i]);
				}
			}

			// Token: 0x06005970 RID: 22896 RVA: 0x002276C7 File Offset: 0x002258C7
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool MoveNext()
			{
				return this._subsetEnumerator.MoveNext();
			}

			// Token: 0x06005971 RID: 22897 RVA: 0x00006566 File Offset: 0x00004766
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x06005972 RID: 22898 RVA: 0x002276D4 File Offset: 0x002258D4
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Dispose()
			{
				this._subsetEnumerator.Dispose();
			}

			// Token: 0x06005973 RID: 22899 RVA: 0x002276E1 File Offset: 0x002258E1
			public bool ContainsKey(Vector3i key)
			{
				return this._subset.Contains(key);
			}

			// Token: 0x1700098E RID: 2446
			// (get) Token: 0x06005974 RID: 22900 RVA: 0x002276EF File Offset: 0x002258EF
			public int Count
			{
				get
				{
					return this._subset.Count;
				}
			}

			// Token: 0x06005975 RID: 22901 RVA: 0x002276FC File Offset: 0x002258FC
			public bool TryGetValue(Vector3i key, out MultiBlockManager.TrackedBlockData value)
			{
				if (this._subset.Contains(key) && this._trackedData.TryGetValue(key, out value))
				{
					return true;
				}
				value = default(MultiBlockManager.TrackedBlockData);
				return false;
			}

			// Token: 0x06005976 RID: 22902 RVA: 0x00227725 File Offset: 0x00225925
			public MultiBlockManager.TrackedDataMap.SubsetAccessor GetEnumerator()
			{
				return this;
			}

			// Token: 0x1700098F RID: 2447
			// (get) Token: 0x06005977 RID: 22903 RVA: 0x0022772D File Offset: 0x0022592D
			public object System.Collections.IEnumerator.Current
			{
				[PublicizedFrom(EAccessModifier.Private)]
				get
				{
					return this.Current;
				}
			}

			// Token: 0x17000990 RID: 2448
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

			// Token: 0x0400459D RID: 17821
			[PublicizedFrom(EAccessModifier.Private)]
			public readonly Dictionary<Vector3i, MultiBlockManager.TrackedBlockData> _trackedData;

			// Token: 0x0400459E RID: 17822
			[PublicizedFrom(EAccessModifier.Private)]
			public readonly HashSet<Vector3i> _subset;

			// Token: 0x0400459F RID: 17823
			[PublicizedFrom(EAccessModifier.Private)]
			public HashSet<Vector3i>.Enumerator _subsetEnumerator;
		}
	}

	// Token: 0x02000B9C RID: 2972
	public enum Mode
	{
		// Token: 0x040045A1 RID: 17825
		Disabled,
		// Token: 0x040045A2 RID: 17826
		Normal,
		// Token: 0x040045A3 RID: 17827
		WorldEditor,
		// Token: 0x040045A4 RID: 17828
		PrefabPlaytest,
		// Token: 0x040045A5 RID: 17829
		PrefabEditor,
		// Token: 0x040045A6 RID: 17830
		Client
	}

	// Token: 0x02000B9D RID: 2973
	[Flags]
	public enum FeatureFlags
	{
		// Token: 0x040045A8 RID: 17832
		None = 0,
		// Token: 0x040045A9 RID: 17833
		POIMBTracking = 1,
		// Token: 0x040045AA RID: 17834
		Serialization = 2,
		// Token: 0x040045AB RID: 17835
		CrossChunkMBTracking = 4,
		// Token: 0x040045AC RID: 17836
		OversizedStability = 8,
		// Token: 0x040045AD RID: 17837
		TerrainAlignment = 16,
		// Token: 0x040045AE RID: 17838
		All = 31
	}

	// Token: 0x02000B9E RID: 2974
	[PublicizedFrom(EAccessModifier.Private)]
	public enum FeatureRequirement
	{
		// Token: 0x040045B0 RID: 17840
		AllEnabled,
		// Token: 0x040045B1 RID: 17841
		OneOrMoreEnabled,
		// Token: 0x040045B2 RID: 17842
		AllDisabled
	}
}
