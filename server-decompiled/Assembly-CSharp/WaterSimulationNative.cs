using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;

// Token: 0x02000CE9 RID: 3305
public class WaterSimulationNative
{
	// Token: 0x17000A9F RID: 2719
	// (get) Token: 0x060064CE RID: 25806 RVA: 0x0027AC2E File Offset: 0x00278E2E
	// (set) Token: 0x060064CF RID: 25807 RVA: 0x0027AC35 File Offset: 0x00278E35
	public static WaterSimulationNative Instance
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	} = new WaterSimulationNative();

	// Token: 0x17000AA0 RID: 2720
	// (get) Token: 0x060064D0 RID: 25808 RVA: 0x0027AC3D File Offset: 0x00278E3D
	// (set) Token: 0x060064D1 RID: 25809 RVA: 0x0027AC45 File Offset: 0x00278E45
	public bool IsInitialized
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000AA1 RID: 2721
	// (get) Token: 0x060064D2 RID: 25810 RVA: 0x0027AC4E File Offset: 0x00278E4E
	public bool IsPaused
	{
		get
		{
			return this.isPaused;
		}
	}

	// Token: 0x060064D3 RID: 25811 RVA: 0x0027AC58 File Offset: 0x00278E58
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

	// Token: 0x060064D4 RID: 25812 RVA: 0x0027AD10 File Offset: 0x00278F10
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsChunkInWorldBounds(Chunk _c)
	{
		Vector3i vector3i;
		Vector3i vector3i2;
		GameManager.Instance.World.GetWorldExtent(out vector3i, out vector3i2);
		return _c.X * 16 >= vector3i.x && _c.X * 16 < vector3i2.x && _c.Z * 16 >= vector3i.z && _c.Z * 16 < vector3i2.z;
	}

	// Token: 0x060064D5 RID: 25813 RVA: 0x0027AD78 File Offset: 0x00278F78
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

	// Token: 0x060064D6 RID: 25814 RVA: 0x0027AE0C File Offset: 0x0027900C
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

	// Token: 0x060064D7 RID: 25815 RVA: 0x0027AEA0 File Offset: 0x002790A0
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

	// Token: 0x060064D8 RID: 25816 RVA: 0x0027AF01 File Offset: 0x00279101
	public void SetPaused(bool _isPaused)
	{
		this.isPaused = _isPaused;
	}

	// Token: 0x060064D9 RID: 25817 RVA: 0x0027AF0A File Offset: 0x0027910A
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

	// Token: 0x060064DA RID: 25818 RVA: 0x0027AF30 File Offset: 0x00279130
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

	// Token: 0x060064DB RID: 25819 RVA: 0x0027B220 File Offset: 0x00279420
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

	// Token: 0x060064DC RID: 25820 RVA: 0x0027B2E0 File Offset: 0x002794E0
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

	// Token: 0x060064DD RID: 25821 RVA: 0x0027B3D8 File Offset: 0x002795D8
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

	// Token: 0x060064DE RID: 25822 RVA: 0x0027B5A0 File Offset: 0x002797A0
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

	// Token: 0x04004EAD RID: 20141
	public bool ShouldEnable = true;

	// Token: 0x04004EAF RID: 20143
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isPaused;

	// Token: 0x04004EB0 RID: 20144
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<ChunkKey, NativeSafeHandle<WaterDataHandle>> usedHandles = new Dictionary<ChunkKey, NativeSafeHandle<WaterDataHandle>>();

	// Token: 0x04004EB1 RID: 20145
	[PublicizedFrom(EAccessModifier.Private)]
	public ConcurrentQueue<NativeSafeHandle<WaterDataHandle>> freeHandles = new ConcurrentQueue<NativeSafeHandle<WaterDataHandle>>();

	// Token: 0x04004EB2 RID: 20146
	[PublicizedFrom(EAccessModifier.Private)]
	public ConcurrentQueue<WaterSimulationNative.HandleInitRequest> newInitializedHandles = new ConcurrentQueue<WaterSimulationNative.HandleInitRequest>();

	// Token: 0x04004EB3 RID: 20147
	[PublicizedFrom(EAccessModifier.Private)]
	public ConcurrentQueue<ChunkKey> handlesToRemove = new ConcurrentQueue<ChunkKey>();

	// Token: 0x04004EB4 RID: 20148
	[PublicizedFrom(EAccessModifier.Private)]
	public UnsafeParallelHashSet<ChunkKey> activeHandles;

	// Token: 0x04004EB5 RID: 20149
	[PublicizedFrom(EAccessModifier.Private)]
	public UnsafeParallelHashMap<ChunkKey, WaterDataHandle> waterDataHandles;

	// Token: 0x04004EB6 RID: 20150
	[PublicizedFrom(EAccessModifier.Private)]
	public UnsafeParallelHashSet<ChunkKey> modifiedChunks;

	// Token: 0x04004EB7 RID: 20151
	[PublicizedFrom(EAccessModifier.Private)]
	public GroundWaterHeightMap groundWaterHeightMap;

	// Token: 0x04004EB8 RID: 20152
	public WaterSimulationApplyChanges changeApplier;

	// Token: 0x02000CEA RID: 3306
	[PublicizedFrom(EAccessModifier.Private)]
	public struct HandleInitRequest
	{
		// Token: 0x04004EB9 RID: 20153
		public ChunkKey chunkKey;

		// Token: 0x04004EBA RID: 20154
		public NativeSafeHandle<WaterDataHandle> safeHandle;
	}

	// Token: 0x02000CEB RID: 3307
	public struct ChunkHandle
	{
		// Token: 0x17000AA2 RID: 2722
		// (get) Token: 0x060064E1 RID: 25825 RVA: 0x0027B693 File Offset: 0x00279893
		public bool IsValid
		{
			get
			{
				return this.sim != null;
			}
		}

		// Token: 0x060064E2 RID: 25826 RVA: 0x0027B69E File Offset: 0x0027989E
		public ChunkHandle(WaterSimulationNative _sim, Chunk _chunk)
		{
			this.sim = _sim;
			this.chunkKey = new ChunkKey(_chunk);
		}

		// Token: 0x060064E3 RID: 25827 RVA: 0x0027B6B4 File Offset: 0x002798B4
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

		// Token: 0x060064E4 RID: 25828 RVA: 0x0027B710 File Offset: 0x00279910
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

		// Token: 0x060064E5 RID: 25829 RVA: 0x0027B75C File Offset: 0x0027995C
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

		// Token: 0x060064E6 RID: 25830 RVA: 0x0027B7AD File Offset: 0x002799AD
		public void Reset()
		{
			if (this.IsValid && this.sim.IsInitialized)
			{
				this.sim.handlesToRemove.Enqueue(this.chunkKey);
			}
			this.sim = null;
			this.chunkKey = default(ChunkKey);
		}

		// Token: 0x04004EBB RID: 20155
		[PublicizedFrom(EAccessModifier.Private)]
		public WaterSimulationNative sim;

		// Token: 0x04004EBC RID: 20156
		[PublicizedFrom(EAccessModifier.Private)]
		public ChunkKey chunkKey;
	}
}
