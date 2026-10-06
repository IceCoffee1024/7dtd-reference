using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;

// Token: 0x02000CDA RID: 3290
public struct WaterDataHandle : IDisposable
{
	// Token: 0x17000A95 RID: 2709
	// (get) Token: 0x0600647B RID: 25723 RVA: 0x00278190 File Offset: 0x00276390
	public UnsafeBitArraySetIndicesEnumerator ActiveVoxelIndices
	{
		get
		{
			return new UnsafeBitArraySetIndicesEnumerator(this.activeVoxels);
		}
	}

	// Token: 0x17000A96 RID: 2710
	// (get) Token: 0x0600647C RID: 25724 RVA: 0x0027819D File Offset: 0x0027639D
	public UnsafeParallelHashMap<int, int>.Enumerator FlowVoxels
	{
		get
		{
			return this.flowVoxels.GetEnumerator();
		}
	}

	// Token: 0x17000A97 RID: 2711
	// (get) Token: 0x0600647D RID: 25725 RVA: 0x002781AA File Offset: 0x002763AA
	public bool HasActiveWater
	{
		get
		{
			return this.activeVoxels.TestAny(0, this.activeVoxels.Length);
		}
	}

	// Token: 0x17000A98 RID: 2712
	// (get) Token: 0x0600647E RID: 25726 RVA: 0x002781C3 File Offset: 0x002763C3
	public bool HasFlows
	{
		get
		{
			return !this.flowVoxels.IsEmpty;
		}
	}

	// Token: 0x0600647F RID: 25727 RVA: 0x002781D4 File Offset: 0x002763D4
	public static WaterDataHandle AllocateNew(Allocator allocator)
	{
		WaterDataHandle waterDataHandle = default(WaterDataHandle);
		waterDataHandle.Allocate(allocator);
		return waterDataHandle;
	}

	// Token: 0x06006480 RID: 25728 RVA: 0x002781F4 File Offset: 0x002763F4
	[PublicizedFrom(EAccessModifier.Private)]
	public void Allocate(Allocator allocator)
	{
		this.voxelData = new UnsafeChunkData<int>(allocator);
		this.voxelState = new UnsafeChunkData<WaterVoxelState>(allocator);
		this.groundWaterHeights = new UnsafeChunkXZMap<GroundWaterBounds>(allocator);
		this.activeVoxels = new UnsafeBitArray(65536, allocator, NativeArrayOptions.ClearMemory);
		this.flowVoxels = new UnsafeParallelHashMap<int, int>(1000, allocator);
		this.flowsFromOtherChunks = new UnsafeFixedBuffer<WaterFlow>(16384, allocator);
		this.activationsFromOtherChunks = new UnsafeFixedBuffer<int>(16384, allocator);
		this.voxelsToWakeup = new UnsafeParallelHashSet<int>(256, allocator);
	}

	// Token: 0x06006481 RID: 25729 RVA: 0x002782A4 File Offset: 0x002764A4
	public bool IsInGroundWater(int _x, int _y, int _z)
	{
		GroundWaterBounds groundWaterBounds = this.groundWaterHeights.Get(_x, _z);
		return groundWaterBounds.IsGroundWater && _y >= (int)groundWaterBounds.bottom && _y <= (int)groundWaterBounds.waterHeight;
	}

	// Token: 0x06006482 RID: 25730 RVA: 0x002782E1 File Offset: 0x002764E1
	public void SetVoxelActive(int _x, int _y, int _z)
	{
		this.activeVoxels.Set(WaterDataHandle.GetVoxelIndex(_x, _y, _z), true);
	}

	// Token: 0x06006483 RID: 25731 RVA: 0x002782F7 File Offset: 0x002764F7
	public void SetVoxelActive(int _index)
	{
		this.activeVoxels.Set(_index, true);
	}

	// Token: 0x06006484 RID: 25732 RVA: 0x00278306 File Offset: 0x00276506
	public void EnqueueVoxelActive(int _x, int _y, int _z)
	{
		this.EnqueueVoxelActive(WaterDataHandle.GetVoxelIndex(_x, _y, _z));
	}

	// Token: 0x06006485 RID: 25733 RVA: 0x00278316 File Offset: 0x00276516
	public void EnqueueVoxelActive(int _index)
	{
		this.activationsFromOtherChunks.AddThreadSafe(_index);
	}

	// Token: 0x06006486 RID: 25734 RVA: 0x00278324 File Offset: 0x00276524
	public void EnqueueVoxelWakeup(int _x, int _y, int _z)
	{
		this.EnqueueVoxelWakeup(WaterDataHandle.GetVoxelIndex(_x, _y, _z));
	}

	// Token: 0x06006487 RID: 25735 RVA: 0x00278334 File Offset: 0x00276534
	public void EnqueueVoxelWakeup(int _index)
	{
		this.voxelsToWakeup.Add(_index);
	}

	// Token: 0x06006488 RID: 25736 RVA: 0x00278344 File Offset: 0x00276544
	public void ApplyEnqueuedActivations()
	{
		NativeArray<int> nativeArray = this.activationsFromOtherChunks.AsNativeArray();
		for (int i = 0; i < nativeArray.Length; i++)
		{
			int num = nativeArray[i];
			this.SetVoxelActive(num);
		}
		this.activationsFromOtherChunks.Clear();
	}

	// Token: 0x06006489 RID: 25737 RVA: 0x0027838A File Offset: 0x0027658A
	public void SetVoxelInactive(int _index)
	{
		this.activeVoxels.Set(_index, false);
	}

	// Token: 0x0600648A RID: 25738 RVA: 0x0027839C File Offset: 0x0027659C
	public void SetVoxelMass(int _x, int _y, int _z, int _mass)
	{
		int voxelIndex = WaterDataHandle.GetVoxelIndex(_x, _y, _z);
		this.SetVoxelMass(voxelIndex, _mass);
	}

	// Token: 0x0600648B RID: 25739 RVA: 0x002783BB File Offset: 0x002765BB
	public void SetVoxelMass(int _index, int _mass)
	{
		if (_mass > 195)
		{
			this.activeVoxels.Set(_index, true);
		}
		else
		{
			this.activeVoxels.Set(_index, false);
		}
		this.voxelData.Set(_index, _mass);
	}

	// Token: 0x0600648C RID: 25740 RVA: 0x002783F0 File Offset: 0x002765F0
	public void SetVoxelSolid(int _x, int _y, int _z, BlockFaceFlag _flags)
	{
		int voxelIndex = WaterDataHandle.GetVoxelIndex(_x, _y, _z);
		WaterVoxelState waterVoxelState = this.voxelState.Get(voxelIndex);
		WaterVoxelState waterVoxelState2 = default(WaterVoxelState);
		waterVoxelState2.SetSolid(_flags);
		this.voxelState.Set(voxelIndex, waterVoxelState2);
		GroundWaterBounds groundWaterBounds = this.groundWaterHeights.Get(_x, _z);
		if (groundWaterBounds.IsGroundWater)
		{
			if (waterVoxelState.IsSolidYNeg() && !waterVoxelState2.IsSolidYNeg() && _y == (int)groundWaterBounds.bottom)
			{
				groundWaterBounds.bottom = (byte)this.FindGroundWaterBottom(voxelIndex);
				this.groundWaterHeights.Set(_x, _z, groundWaterBounds);
				return;
			}
			if (waterVoxelState.IsSolidYPos() && !waterVoxelState2.IsSolidYPos() && _y + 1 == (int)groundWaterBounds.bottom)
			{
				groundWaterBounds.bottom = (byte)this.FindGroundWaterBottom(voxelIndex);
				this.groundWaterHeights.Set(_x, _z, groundWaterBounds);
				return;
			}
			if (!waterVoxelState.IsSolidYNeg() && waterVoxelState2.IsSolidYNeg() && _y > (int)groundWaterBounds.bottom && _y <= (int)groundWaterBounds.waterHeight)
			{
				groundWaterBounds.bottom = (byte)_y;
				this.groundWaterHeights.Set(_x, _z, groundWaterBounds);
				return;
			}
			if (!waterVoxelState.IsSolidYPos() && waterVoxelState2.IsSolidYPos())
			{
				int num = _y + 1;
				if (num > (int)groundWaterBounds.bottom && num <= (int)groundWaterBounds.waterHeight)
				{
					groundWaterBounds.bottom = (byte)num;
					this.groundWaterHeights.Set(_x, _z, groundWaterBounds);
				}
			}
		}
	}

	// Token: 0x0600648D RID: 25741 RVA: 0x0027853E File Offset: 0x0027673E
	public void ApplyFlow(int _x, int _y, int _z, int _flow)
	{
		this.ApplyFlow(WaterDataHandle.GetVoxelIndex(_x, _y, _z), _flow);
	}

	// Token: 0x0600648E RID: 25742 RVA: 0x00278550 File Offset: 0x00276750
	public void ApplyFlow(int _index, int _flow)
	{
		int num;
		if (this.flowVoxels.TryGetValue(_index, out num))
		{
			_flow += num;
		}
		this.flowVoxels[_index] = _flow;
	}

	// Token: 0x0600648F RID: 25743 RVA: 0x00278580 File Offset: 0x00276780
	public void EnqueueFlow(int _voxelIndex, int _flow)
	{
		this.flowsFromOtherChunks.AddThreadSafe(new WaterFlow
		{
			voxelIndex = _voxelIndex,
			flow = _flow
		});
	}

	// Token: 0x06006490 RID: 25744 RVA: 0x002785B4 File Offset: 0x002767B4
	public void ApplyEnqueuedFlows()
	{
		NativeArray<WaterFlow> nativeArray = this.flowsFromOtherChunks.AsNativeArray();
		for (int i = 0; i < nativeArray.Length; i++)
		{
			WaterFlow waterFlow = nativeArray[i];
			this.ApplyFlow(waterFlow.voxelIndex, waterFlow.flow);
		}
		this.flowsFromOtherChunks.Clear();
	}

	// Token: 0x06006491 RID: 25745 RVA: 0x00278608 File Offset: 0x00276808
	[PublicizedFrom(EAccessModifier.Private)]
	public int FindGroundWaterBottom(int _fromIndex)
	{
		for (int i = _fromIndex; i >= 0; i -= 256)
		{
			WaterVoxelState waterVoxelState = this.voxelState.Get(i);
			if (waterVoxelState.IsSolidYNeg())
			{
				return WaterDataHandle.GetVoxelY(i);
			}
			if (waterVoxelState.IsSolidYPos())
			{
				int num = math.min(i + 256, 255);
				if (num <= _fromIndex)
				{
					return WaterDataHandle.GetVoxelY(num);
				}
			}
		}
		return 0;
	}

	// Token: 0x06006492 RID: 25746 RVA: 0x0027866C File Offset: 0x0027686C
	public void InitializeFromChunk(Chunk _chunk, GroundWaterHeightMap _groundWaterHeightMap)
	{
		if (!this.voxelData.IsCreated || !this.activeVoxels.IsCreated)
		{
			Debug.LogError("Could not initialize WaterDataHandle because it has not been allocated");
			return;
		}
		this.Clear();
		for (int i = 0; i < 256; i++)
		{
			for (int j = 0; j < 16; j++)
			{
				for (int k = 0; k < 16; k++)
				{
					WaterVoxelState waterVoxelState = default(WaterVoxelState);
					BlockValue blockNoDamage = _chunk.GetBlockNoDamage(k, i, j);
					Block block = blockNoDamage.Block;
					byte rotation = blockNoDamage.rotation;
					waterVoxelState.SetSolid(BlockFaceFlags.RotateFlags(block.WaterFlowMask, rotation));
					int voxelIndex = WaterDataHandle.GetVoxelIndex(k, i, j);
					int mass = _chunk.GetWater(k, i, j).GetMass();
					if (mass > 195)
					{
						this.activeVoxels.Set(voxelIndex, true);
						this.voxelData.Set(voxelIndex, mass);
					}
					if (!waterVoxelState.IsDefault())
					{
						this.voxelState.Set(voxelIndex, waterVoxelState);
					}
				}
			}
		}
		this.voxelState.CheckSameValues();
		if (_groundWaterHeightMap.TryInit())
		{
			for (int l = 0; l < 16; l++)
			{
				for (int m = 0; m < 16; m++)
				{
					Vector3i vector3i = _chunk.ToWorldPos(m, 0, l);
					int num;
					if (_groundWaterHeightMap.TryGetWaterHeightAt(vector3i.x, vector3i.z, out num))
					{
						int num2 = this.FindGroundWaterBottom(WaterDataHandle.GetVoxelIndex(m, num, l));
						this.groundWaterHeights.Set(m, l, new GroundWaterBounds(num2, num));
					}
				}
			}
		}
	}

	// Token: 0x06006493 RID: 25747 RVA: 0x002787FC File Offset: 0x002769FC
	public void Clear()
	{
		if (this.voxelData.IsCreated)
		{
			this.voxelData.Clear();
		}
		if (this.voxelState.IsCreated)
		{
			this.voxelState.Clear();
		}
		if (this.groundWaterHeights.IsCreated)
		{
			this.groundWaterHeights.Clear();
		}
		if (this.activeVoxels.IsCreated)
		{
			this.activeVoxels.Clear();
		}
		if (this.flowVoxels.IsCreated)
		{
			this.flowVoxels.Clear();
		}
		if (this.flowsFromOtherChunks.IsCreated)
		{
			this.flowsFromOtherChunks.Clear();
		}
		if (this.activationsFromOtherChunks.IsCreated)
		{
			this.activationsFromOtherChunks.Clear();
		}
		if (this.voxelsToWakeup.IsCreated)
		{
			this.voxelsToWakeup.Clear();
		}
	}

	// Token: 0x06006494 RID: 25748 RVA: 0x002788CC File Offset: 0x00276ACC
	public void Dispose()
	{
		if (this.voxelData.IsCreated)
		{
			this.voxelData.Dispose();
		}
		if (this.voxelState.IsCreated)
		{
			this.voxelState.Dispose();
		}
		if (this.groundWaterHeights.IsCreated)
		{
			this.groundWaterHeights.Dispose();
		}
		if (this.activeVoxels.IsCreated)
		{
			this.activeVoxels.Dispose();
		}
		if (this.flowVoxels.IsCreated)
		{
			this.flowVoxels.Dispose();
		}
		if (this.flowsFromOtherChunks.IsCreated)
		{
			this.flowsFromOtherChunks.Dispose();
		}
		if (this.activationsFromOtherChunks.IsCreated)
		{
			this.activationsFromOtherChunks.Dispose();
		}
		if (this.voxelsToWakeup.IsCreated)
		{
			this.voxelsToWakeup.Dispose();
		}
	}

	// Token: 0x06006495 RID: 25749 RVA: 0x0027899C File Offset: 0x00276B9C
	public int CalculateOwnedBytes()
	{
		return 0 + this.voxelData.CalculateOwnedBytes() + this.voxelState.CalculateOwnedBytes() + this.groundWaterHeights.CalculateOwnedBytes() + ProfilerUtils.CalculateUnsafeBitArrayBytes(this.activeVoxels) + ProfilerUtils.CalculateUnsafeParallelHashMapBytes<int, int>(this.flowVoxels) + this.flowsFromOtherChunks.CalculateOwnedBytes() + this.activationsFromOtherChunks.CalculateOwnedBytes() + ProfilerUtils.CalculateUnsafeParallelHashSetBytes<int>(this.voxelsToWakeup);
	}

	// Token: 0x06006496 RID: 25750 RVA: 0x00278A0C File Offset: 0x00276C0C
	public string GetMemoryStats()
	{
		return string.Format("voxelData: {0:F2} KB, voxelState: {1:F2} KB, groundWaterHeights: {2:F2} KB, activeVoxels: ({3:F2} KB), flowVoxels: ({4},{5},{6:F2} KB), flowsFromOtherChunks: {7:F2} KB, activationsFromOtherChunks: {8:F2} KB, voxelsToWakeup {9:F2} KB, Total: {10:F2} MB", new object[]
		{
			(double)this.voxelData.CalculateOwnedBytes() * 0.0009765625,
			(double)this.voxelState.CalculateOwnedBytes() * 0.0009765625,
			(double)this.groundWaterHeights.CalculateOwnedBytes() * 0.0009765625,
			(double)ProfilerUtils.CalculateUnsafeBitArrayBytes(this.activeVoxels) * 0.0009765625,
			this.flowVoxels.Count(),
			this.flowVoxels.Capacity,
			(double)ProfilerUtils.CalculateUnsafeParallelHashMapBytes<int, int>(this.flowVoxels) * 0.0009765625,
			(double)this.flowsFromOtherChunks.CalculateOwnedBytes() * 0.0009765625,
			(double)this.activationsFromOtherChunks.CalculateOwnedBytes() * 0.0009765625,
			(double)ProfilerUtils.CalculateUnsafeParallelHashSetBytes<int>(this.voxelsToWakeup) * 0.0009765625,
			(double)this.CalculateOwnedBytes() * 9.5367431640625E-07
		});
	}

	// Token: 0x06006497 RID: 25751 RVA: 0x00278B5B File Offset: 0x00276D5B
	public static int GetVoxelIndex(int _x, int _y, int _z)
	{
		return _x + _y * 256 + _z * 16;
	}

	// Token: 0x06006498 RID: 25752 RVA: 0x00278B6C File Offset: 0x00276D6C
	public static int3 GetVoxelCoords(int index)
	{
		int3 @int = default(int3);
		@int.y = index / 256;
		int num = index % 256;
		@int.z = num / 16;
		@int.x = num % 16;
		return @int;
	}

	// Token: 0x06006499 RID: 25753 RVA: 0x00278BAE File Offset: 0x00276DAE
	public static int GetVoxelY(int _index)
	{
		return _index / 256;
	}

	// Token: 0x04004E69 RID: 20073
	public UnsafeChunkData<int> voxelData;

	// Token: 0x04004E6A RID: 20074
	public UnsafeChunkData<WaterVoxelState> voxelState;

	// Token: 0x04004E6B RID: 20075
	public UnsafeChunkXZMap<GroundWaterBounds> groundWaterHeights;

	// Token: 0x04004E6C RID: 20076
	public UnsafeBitArray activeVoxels;

	// Token: 0x04004E6D RID: 20077
	public UnsafeParallelHashMap<int, int> flowVoxels;

	// Token: 0x04004E6E RID: 20078
	public UnsafeFixedBuffer<WaterFlow> flowsFromOtherChunks;

	// Token: 0x04004E6F RID: 20079
	public UnsafeFixedBuffer<int> activationsFromOtherChunks;

	// Token: 0x04004E70 RID: 20080
	public UnsafeParallelHashSet<int> voxelsToWakeup;
}
