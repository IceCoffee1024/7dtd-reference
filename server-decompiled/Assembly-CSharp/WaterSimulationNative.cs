using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;

// Token: 0x02000CDE RID: 3294
public class WaterSimulationNative
{
	// Token: 0x17000A99 RID: 2713
	// (get) Token: 0x060064A9 RID: 25769 RVA: 0x00279706 File Offset: 0x00277906
	// (set) Token: 0x060064AA RID: 25770 RVA: 0x0027970D File Offset: 0x0027790D
	public static WaterSimulationNative Instance
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	} = new WaterSimulationNative();

	// Token: 0x17000A9A RID: 2714
	// (get) Token: 0x060064AB RID: 25771 RVA: 0x00279715 File Offset: 0x00277915
	// (set) Token: 0x060064AC RID: 25772 RVA: 0x0027971D File Offset: 0x0027791D
	public bool IsInitialized
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000A9B RID: 2715
	// (get) Token: 0x060064AD RID: 25773 RVA: 0x00279726 File Offset: 0x00277926
	public bool IsPaused
	{
		get
		{
			return this.isPaused;
		}
	}

	// Token: 0x060064AE RID: 25774 RVA: 0x00279730 File Offset: 0x00277930
	public void Init(ChunkCluster _cc)
	{
		this.changeApplier = new WaterSimulationApplyChanges(_cc);
		if (!this.ShouldEnable)
		{
			return;
		}
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return;
		}
		if (this.waterDataHandles.IsCreated || this.modifiedChunks.IsCreated)
		{
			Debug.LogError("Last water simulation data was disposed of and may have leaked");
		}
		this.activeHandles = new UnsafeParallelHashSet<ChunkKey>(500, AllocatorManager.Persistent);
		this.waterDataHandles = new UnsafeParallelHashMap<ChunkKey, WaterDataHandle>(500, AllocatorManager.Persistent);
		this.modifiedChunks = new UnsafeParallelHashSet<ChunkKey>(500, AllocatorManager.Persistent);
		this.groundWaterHeightMap = new GroundWaterHeightMap(GameManager.Instance.World);
		this.IsInitialized = true;
		this.isPaused = false;
	}

	// Token: 0x060064AF RID: 25775 RVA: 0x002797E8 File Offset: 0x002779E8
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsChunkInWorldBounds(Chunk _c)
	{
		Vector3i vector3i;
		Vector3i vector3i2;
		GameManager.Instance.World.GetWorldExtent(out vector3i, out vector3i2);
		return _c.X * 16 >= vector3i.x && _c.X * 16 < vector3i2.x && _c.Z * 16 >= vector3i.z && _c.Z * 16 < vector3i2.z;
	}

	// Token: 0x060064B0 RID: 25776 RVA: 0x00279850 File Offset: 0x00277A50
	public void InitializeChunk(Chunk _c)
	{
		if (!this.IsInitialized)
		{
			return;
		}
		if (!this.IsChunkInWorldBounds(_c))
		{
			return;
		}
		NativeSafeHandle<WaterDataHandle> nativeSafeHandle;
		WaterDataHandle waterDataHandle;
		if (this.freeHandles.TryDequeue(out nativeSafeHandle))
		{
			waterDataHandle = nativeSafeHandle.Target;
			waterDataHandle.Clear();
		}
		else
		{
			waterDataHandle = WaterDataHandle.AllocateNew(Allocator.Persistent);
			nativeSafeHandle = new NativeSafeHandle<WaterDataHandle>(ref waterDataHandle, Allocator.Persistent);
		}
		waterDataHandle.InitializeFromChunk(_c, this.groundWaterHeightMap);
		this.newInitializedHandles.Enqueue(new WaterSimulationNative.HandleInitRequest
		{
			chunkKey = new ChunkKey(_c),
			safeHandle = nativeSafeHandle
		});
		_c.AssignWaterSimHandle(new WaterSimulationNative.ChunkHandle(this, _c));
	}

	// Token: 0x060064B1 RID: 25777 RVA: 0x002798E4 File Offset: 0x00277AE4
	[PublicizedFrom(EAccessModifier.Private)]
	public void CopyInitializedChunksToNative()
	{
		WaterSimulationNative.HandleInitRequest handleInitRequest;
		while (this.newInitializedHandles.TryDequeue(out handleInitRequest))
		{
			ChunkKey chunkKey = handleInitRequest.chunkKey;
			NativeSafeHandle<WaterDataHandle> safeHandle = handleInitRequest.safeHandle;
			NativeSafeHandle<WaterDataHandle> nativeSafeHandle;
			if (this.usedHandles.TryGetValue(chunkKey, out nativeSafeHandle))
			{
				this.freeHandles.Enqueue(nativeSafeHandle);
				this.usedHandles[chunkKey] = safeHandle;
			}
			else
			{
				this.usedHandles.Add(chunkKey, safeHandle);
			}
			WaterDataHandle target = safeHandle.Target;
			this.waterDataHandles[chunkKey] = target;
			if (target.HasActiveWater)
			{
				this.activeHandles.Add(chunkKey);
			}
		}
	}

	// Token: 0x060064B2 RID: 25778 RVA: 0x00279978 File Offset: 0x00277B78
	[PublicizedFrom(EAccessModifier.Private)]
	public void ProcessPendingRemoves()
	{
		ChunkKey chunkKey;
		while (this.handlesToRemove.TryDequeue(out chunkKey))
		{
			NativeSafeHandle<WaterDataHandle> nativeSafeHandle;
			if (this.usedHandles.TryGetValue(chunkKey, out nativeSafeHandle))
			{
				this.usedHandles.Remove(chunkKey);
				this.freeHandles.Enqueue(nativeSafeHandle);
			}
			this.waterDataHandles.Remove(chunkKey);
			this.activeHandles.Remove(chunkKey);
		}
	}

	// Token: 0x060064B3 RID: 25779 RVA: 0x002799D9 File Offset: 0x00277BD9
	public void SetPaused(bool _isPaused)
	{
		this.isPaused = _isPaused;
	}

	// Token: 0x060064B4 RID: 25780 RVA: 0x002799E2 File Offset: 0x00277BE2
	public void Step()
	{
		if (!this.isPaused)
		{
			this.SetPaused(true);
			return;
		}
		this.isPaused = false;
		this.Update();
		this.isPaused = true;
	}

	// Token: 0x060064B5 RID: 25781 RVA: 0x00279A08 File Offset: 0x00277C08
	public unsafe void Update()
	{
		if (!this.IsInitialized)
		{
			return;
		}
		this.ProcessPendingRemoves();
		this.CopyInitializedChunksToNative();
		if (this.isPaused)
		{
			return;
		}
		if (this.changeApplier.HasNetWorkLimitBeenReached())
		{
			return;
		}
		WaterStats waterStats = default(WaterStats);
		if (!this.modifiedChunks.IsEmpty || !this.activeHandles.IsEmpty)
		{
			new WaterSimulationPreProcess
			{
				activeChunks = this.activeHandles,
				waterDataHandles = this.waterDataHandles,
				modifiedChunks = this.modifiedChunks
			}.Run<WaterSimulationPreProcess>();
			if (!this.activeHandles.IsEmpty)
			{
				NativeArray<ChunkKey> nativeArray = this.activeHandles.ToNativeArray(Allocator.TempJob);
				NativeList<ChunkKey> nativeList = new NativeList<ChunkKey>(nativeArray.Length, AllocatorManager.TempJob);
				NativeArray<WaterStats> nativeArray2 = new NativeArray<WaterStats>(nativeArray.Length, Allocator.TempJob, NativeArrayOptions.ClearMemory);
				WaterSimulationCalcFlows waterSimulationCalcFlows = new WaterSimulationCalcFlows
				{
					processingChunks = nativeArray,
					waterStats = nativeArray2,
					waterDataHandles = this.waterDataHandles
				};
				WaterSimulationApplyFlows waterSimulationApplyFlows = new WaterSimulationApplyFlows
				{
					processingChunks = nativeArray,
					nonFlowingChunks = nativeList.AsParallelWriter(),
					waterStats = nativeArray2,
					waterDataHandles = this.waterDataHandles,
					activeChunkSet = this.activeHandles.AsParallelWriter()
				};
				WaterSimulationPostProcess waterSimulationPostProcess = new WaterSimulationPostProcess
				{
					processingChunks = nativeArray,
					nonFlowingChunks = nativeList,
					activeChunks = this.activeHandles,
					waterDataHandles = this.waterDataHandles
				};
				int num = nativeArray.Length / JobsUtility.JobWorkerCount + 1;
				JobHandle jobHandle = waterSimulationCalcFlows.Schedule(nativeArray.Length, num, default(JobHandle));
				JobHandle jobHandle2 = waterSimulationApplyFlows.Schedule(nativeArray.Length, num, jobHandle);
				JobHandle jobHandle3 = waterSimulationPostProcess.Schedule(jobHandle2);
				JobHandle.ScheduleBatchedJobs();
				jobHandle3.Complete();
				waterStats += WaterStats.Sum(nativeArray2);
				nativeArray2.Dispose();
				nativeArray.Dispose();
				nativeList.Dispose();
			}
		}
		this.ProcessPendingRemoves();
		foreach (KeyValue<ChunkKey, WaterDataHandle> keyValue in this.waterDataHandles)
		{
			ChunkKey key = keyValue.Key;
			UnsafeParallelHashMap<ChunkKey, WaterDataHandle>.Enumerator enumerator;
			keyValue = enumerator.Current;
			WaterDataHandle waterDataHandle = *keyValue.Value;
			if (waterDataHandle.HasFlows)
			{
				using (WaterSimulationApplyChanges.ChangesForChunk.Writer changeWriter = this.changeApplier.GetChangeWriter(WorldChunkCache.MakeChunkKey(key.x, key.z)))
				{
					UnsafeParallelHashMap<int, int>.Enumerator flowVoxels = waterDataHandle.FlowVoxels;
					while (flowVoxels.MoveNext())
					{
						KeyValue<int, int> keyValue2 = flowVoxels.Current;
						int key2 = keyValue2.Key;
						int num2 = waterDataHandle.voxelData.Get(key2);
						WaterValue waterValue = new WaterValue(num2);
						changeWriter.RecordChange(key2, waterValue);
					}
					waterDataHandle.flowVoxels.Clear();
				}
			}
		}
		WaterStatsProfiler.SampleTick(waterStats);
	}

	// Token: 0x060064B6 RID: 25782 RVA: 0x00279CF8 File Offset: 0x00277EF8
	public void Clear()
	{
		if (!this.IsInitialized)
		{
			return;
		}
		foreach (NativeSafeHandle<WaterDataHandle> nativeSafeHandle in this.usedHandles.Values)
		{
			this.freeHandles.Enqueue(nativeSafeHandle);
		}
		this.usedHandles.Clear();
		this.waterDataHandles.Clear();
		WaterSimulationNative.HandleInitRequest handleInitRequest;
		while (this.newInitializedHandles.TryDequeue(out handleInitRequest))
		{
			this.freeHandles.Enqueue(handleInitRequest.safeHandle);
		}
		this.handlesToRemove = new ConcurrentQueue<ChunkKey>();
		this.activeHandles.Clear();
		this.modifiedChunks.Clear();
	}

	// Token: 0x060064B7 RID: 25783 RVA: 0x00279DB8 File Offset: 0x00277FB8
	public void Cleanup()
	{
		WaterSimulationApplyChanges waterSimulationApplyChanges = this.changeApplier;
		if (waterSimulationApplyChanges != null)
		{
			waterSimulationApplyChanges.Cleanup();
		}
		this.changeApplier = null;
		this.groundWaterHeightMap = null;
		if (!this.IsInitialized)
		{
			return;
		}
		this.Clear();
		NativeSafeHandle<WaterDataHandle> nativeSafeHandle;
		while (this.freeHandles.TryDequeue(out nativeSafeHandle))
		{
			nativeSafeHandle.Dispose();
		}
		foreach (NativeSafeHandle<WaterDataHandle> nativeSafeHandle2 in this.usedHandles.Values)
		{
			nativeSafeHandle2.Dispose();
		}
		this.usedHandles.Clear();
		if (this.activeHandles.IsCreated)
		{
			this.activeHandles.Dispose();
		}
		if (this.waterDataHandles.IsCreated)
		{
			this.waterDataHandles.Dispose();
		}
		if (this.modifiedChunks.IsCreated)
		{
			this.modifiedChunks.Dispose();
		}
		this.IsInitialized = false;
	}

	// Token: 0x060064B8 RID: 25784 RVA: 0x00279EB0 File Offset: 0x002780B0
	public string GetMemoryStats()
	{
		int count = this.usedHandles.Count;
		int count2 = this.freeHandles.Count;
		int count3 = this.newInitializedHandles.Count;
		int num = 0;
		foreach (NativeSafeHandle<WaterDataHandle> nativeSafeHandle in this.usedHandles.Values)
		{
			num += nativeSafeHandle.Target.CalculateOwnedBytes();
		}
		foreach (NativeSafeHandle<WaterDataHandle> nativeSafeHandle2 in this.freeHandles)
		{
			num += nativeSafeHandle2.Target.CalculateOwnedBytes();
		}
		foreach (WaterSimulationNative.HandleInitRequest handleInitRequest in this.newInitializedHandles)
		{
			int num2 = num;
			NativeSafeHandle<WaterDataHandle> safeHandle = handleInitRequest.safeHandle;
			num = num2 + safeHandle.Target.CalculateOwnedBytes();
		}
		int num3 = ProfilerUtils.CalculateUnsafeParallelHashSetBytes<ChunkKey>(this.activeHandles);
		num3 += ProfilerUtils.CalculateUnsafeParallelHashMapBytes<ChunkKey, WaterDataHandle>(this.waterDataHandles);
		return string.Format("Allocated Handles: {0}, Used Handles: {1}, Free Handles: {2}, Pending Handles: {3}, Handle Contents (MB): {4:F2}, Other Memory (MB): {5:F2}, Total Memory (MB): {6:F2}", new object[]
		{
			count + count2 + count3,
			count,
			count2,
			count3,
			(double)num * 9.5367431640625E-07,
			(double)num3 * 9.5367431640625E-07,
			(double)(num + num3) * 9.5367431640625E-07
		});
	}

	// Token: 0x060064B9 RID: 25785 RVA: 0x0027A078 File Offset: 0x00278278
	public string GetMemoryStatsDetailed()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("Used Handles:");
		foreach (KeyValuePair<ChunkKey, NativeSafeHandle<WaterDataHandle>> keyValuePair in this.usedHandles)
		{
			stringBuilder.AppendFormat("Chunk ({0},{1}): {2}\n", keyValuePair.Key.x, keyValuePair.Key.z, keyValuePair.Value.Target.GetMemoryStats());
		}
		return stringBuilder.ToString();
	}

	// Token: 0x04004E88 RID: 20104
	public bool ShouldEnable = true;

	// Token: 0x04004E8A RID: 20106
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isPaused;

	// Token: 0x04004E8B RID: 20107
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<ChunkKey, NativeSafeHandle<WaterDataHandle>> usedHandles = new Dictionary<ChunkKey, NativeSafeHandle<WaterDataHandle>>();

	// Token: 0x04004E8C RID: 20108
	[PublicizedFrom(EAccessModifier.Private)]
	public ConcurrentQueue<NativeSafeHandle<WaterDataHandle>> freeHandles = new ConcurrentQueue<NativeSafeHandle<WaterDataHandle>>();

	// Token: 0x04004E8D RID: 20109
	[PublicizedFrom(EAccessModifier.Private)]
	public ConcurrentQueue<WaterSimulationNative.HandleInitRequest> newInitializedHandles = new ConcurrentQueue<WaterSimulationNative.HandleInitRequest>();

	// Token: 0x04004E8E RID: 20110
	[PublicizedFrom(EAccessModifier.Private)]
	public ConcurrentQueue<ChunkKey> handlesToRemove = new ConcurrentQueue<ChunkKey>();

	// Token: 0x04004E8F RID: 20111
	[PublicizedFrom(EAccessModifier.Private)]
	public UnsafeParallelHashSet<ChunkKey> activeHandles;

	// Token: 0x04004E90 RID: 20112
	[PublicizedFrom(EAccessModifier.Private)]
	public UnsafeParallelHashMap<ChunkKey, WaterDataHandle> waterDataHandles;

	// Token: 0x04004E91 RID: 20113
	[PublicizedFrom(EAccessModifier.Private)]
	public UnsafeParallelHashSet<ChunkKey> modifiedChunks;

	// Token: 0x04004E92 RID: 20114
	[PublicizedFrom(EAccessModifier.Private)]
	public GroundWaterHeightMap groundWaterHeightMap;

	// Token: 0x04004E93 RID: 20115
	public WaterSimulationApplyChanges changeApplier;

	// Token: 0x02000CDF RID: 3295
	[PublicizedFrom(EAccessModifier.Private)]
	public struct HandleInitRequest
	{
		// Token: 0x04004E94 RID: 20116
		public ChunkKey chunkKey;

		// Token: 0x04004E95 RID: 20117
		public NativeSafeHandle<WaterDataHandle> safeHandle;
	}

	// Token: 0x02000CE0 RID: 3296
	public struct ChunkHandle
	{
		// Token: 0x17000A9C RID: 2716
		// (get) Token: 0x060064BC RID: 25788 RVA: 0x0027A16B File Offset: 0x0027836B
		public bool IsValid
		{
			get
			{
				return this.sim != null;
			}
		}

		// Token: 0x060064BD RID: 25789 RVA: 0x0027A176 File Offset: 0x00278376
		public ChunkHandle(WaterSimulationNative _sim, Chunk _chunk)
		{
			this.sim = _sim;
			this.chunkKey = new ChunkKey(_chunk);
		}

		// Token: 0x060064BE RID: 25790 RVA: 0x0027A18C File Offset: 0x0027838C
		public void SetWaterMass(int _x, int _y, int _z, int _mass)
		{
			if (!this.IsValid)
			{
				return;
			}
			WaterDataHandle waterDataHandle;
			if (this.sim.waterDataHandles.TryGetValue(this.chunkKey, out waterDataHandle))
			{
				int voxelIndex = WaterDataHandle.GetVoxelIndex(_x, _y, _z);
				waterDataHandle.SetVoxelMass(voxelIndex, _mass);
				this.sim.activeHandles.Add(this.chunkKey);
			}
		}

		// Token: 0x060064BF RID: 25791 RVA: 0x0027A1E8 File Offset: 0x002783E8
		public void SetVoxelSolid(int _x, int _y, int _z, BlockFaceFlag _flags)
		{
			if (!this.IsValid)
			{
				return;
			}
			WaterDataHandle waterDataHandle;
			if (this.sim.waterDataHandles.TryGetValue(this.chunkKey, out waterDataHandle))
			{
				waterDataHandle.SetVoxelSolid(_x, _y, _z, _flags);
				if (_flags != BlockFaceFlag.All)
				{
					this.WakeNeighbours(_x, _y, _z);
				}
			}
		}

		// Token: 0x060064C0 RID: 25792 RVA: 0x0027A234 File Offset: 0x00278434
		public void WakeNeighbours(int _x, int _y, int _z)
		{
			if (!this.IsValid)
			{
				return;
			}
			WaterDataHandle waterDataHandle;
			if (this.sim.waterDataHandles.TryGetValue(this.chunkKey, out waterDataHandle))
			{
				waterDataHandle.EnqueueVoxelWakeup(_x, _y, _z);
			}
			this.sim.modifiedChunks.Add(this.chunkKey);
		}

		// Token: 0x060064C1 RID: 25793 RVA: 0x0027A285 File Offset: 0x00278485
		public void Reset()
		{
			if (this.IsValid && this.sim.IsInitialized)
			{
				this.sim.handlesToRemove.Enqueue(this.chunkKey);
			}
			this.sim = null;
			this.chunkKey = default(ChunkKey);
		}

		// Token: 0x04004E96 RID: 20118
		[PublicizedFrom(EAccessModifier.Private)]
		public WaterSimulationNative sim;

		// Token: 0x04004E97 RID: 20119
		[PublicizedFrom(EAccessModifier.Private)]
		public ChunkKey chunkKey;
	}
}
