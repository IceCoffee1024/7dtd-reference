using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;

// Token: 0x02000CE5 RID: 3301
public struct WaterDataHandle : IDisposable
{
	// Token: 0x17000A9B RID: 2715
	// (get) Token: 0x060064A0 RID: 25760 RVA: 0x002796B8 File Offset: 0x002778B8
	public UnsafeBitArraySetIndicesEnumerator ActiveVoxelIndices
	{
		get
		{
			return new UnsafeBitArraySetIndicesEnumerator(this.activeVoxels);
		}
	}

	// Token: 0x17000A9C RID: 2716
	// (get) Token: 0x060064A1 RID: 25761 RVA: 0x002796C5 File Offset: 0x002778C5
	public UnsafeParallelHashMap<int, int>.Enumerator FlowVoxels
	{
		get
		{
			return this.flowVoxels.GetEnumerator();
		}
	}

	// Token: 0x17000A9D RID: 2717
	// (get) Token: 0x060064A2 RID: 25762 RVA: 0x002796D2 File Offset: 0x002778D2
	public bool HasActiveWater
	{
		get
		{
			return this.activeVoxels.TestAny(0, this.activeVoxels.Length);
		}
	}

	// Token: 0x17000A9E RID: 2718
	// (get) Token: 0x060064A3 RID: 25763 RVA: 0x002796EB File Offset: 0x002778EB
	public bool HasFlows
	{
		get
		{
			return !this.flowVoxels.IsEmpty;
		}
	}

	// Token: 0x060064A4 RID: 25764 RVA: 0x002796FC File Offset: 0x002778FC
	public static WaterDataHandle AllocateNew(Allocator allocator)
	{
		WaterDataHandle waterDataHandle = default(WaterDataHandle);
		waterDataHandle.Allocate(allocator);
		return waterDataHandle;
	}

	// Token: 0x060064A5 RID: 25765 RVA: 0x0027971C File Offset: 0x0027791C
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

	// Token: 0x060064A6 RID: 25766 RVA: 0x002797CC File Offset: 0x002779CC
	public bool IsInGroundWater(int _x, int _y, int _z)
	{
		GroundWaterBounds groundWaterBounds = this.groundWaterHeights.Get(_x, _z);
		return groundWaterBounds.IsGroundWater && _y >= (int)groundWaterBounds.bottom && _y <= (int)groundWaterBounds.waterHeight;
	}

	// Token: 0x060064A7 RID: 25767 RVA: 0x00279809 File Offset: 0x00277A09
	public void SetVoxelActive(int _x, int _y, int _z)
	{
		this.activeVoxels.Set(WaterDataHandle.GetVoxelIndex(_x, _y, _z), true);
	}

	// Token: 0x060064A8 RID: 25768 RVA: 0x0027981F File Offset: 0x00277A1F
	public void SetVoxelActive(int _index)
	{
		this.activeVoxels.Set(_index, true);
	}

	// Token: 0x060064A9 RID: 25769 RVA: 0x0027982E File Offset: 0x00277A2E
	public void EnqueueVoxelActive(int _x, int _y, int _z)
	{
		this.EnqueueVoxelActive(WaterDataHandle.GetVoxelIndex(_x, _y, _z));
	}

	// Token: 0x060064AA RID: 25770 RVA: 0x0027983E File Offset: 0x00277A3E
	public void EnqueueVoxelActive(int _index)
	{
		this.activationsFromOtherChunks.AddThreadSafe(_index);
	}

	// Token: 0x060064AB RID: 25771 RVA: 0x0027984C File Offset: 0x00277A4C
	public void EnqueueVoxelWakeup(int _x, int _y, int _z)
	{
		this.EnqueueVoxelWakeup(WaterDataHandle.GetVoxelIndex(_x, _y, _z));
	}

	// Token: 0x060064AC RID: 25772 RVA: 0x0027985C File Offset: 0x00277A5C
	public void EnqueueVoxelWakeup(int _index)
	{
		this.voxelsToWakeup.Add(_index);
	}

	// Token: 0x060064AD RID: 25773 RVA: 0x0027986C File Offset: 0x00277A6C
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

	// Token: 0x060064AE RID: 25774 RVA: 0x002798B2 File Offset: 0x00277AB2
	public void SetVoxelInactive(int _index)
	{
		this.activeVoxels.Set(_index, false);
	}

	// Token: 0x060064AF RID: 25775 RVA: 0x002798C4 File Offset: 0x00277AC4
	public void SetVoxelMass(int _x, int _y, int _z, int _mass)
	{
		int voxelIndex = WaterDataHandle.GetVoxelIndex(_x, _y, _z);
		this.SetVoxelMass(voxelIndex, _mass);
	}

	// Token: 0x060064B0 RID: 25776 RVA: 0x002798E3 File Offset: 0x00277AE3
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

	// Token: 0x060064B1 RID: 25777 RVA: 0x00279918 File Offset: 0x00277B18
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

	// Token: 0x060064B2 RID: 25778 RVA: 0x00279A66 File Offset: 0x00277C66
	public void ApplyFlow(int _x, int _y, int _z, int _flow)
	{
		this.ApplyFlow(WaterDataHandle.GetVoxelIndex(_x, _y, _z), _flow);
	}

	// Token: 0x060064B3 RID: 25779 RVA: 0x00279A78 File Offset: 0x00277C78
	public void ApplyFlow(int _index, int _flow)
	{
		int num;
		if (this.flowVoxels.TryGetValue(_index, out num))
		{
			_flow += num;
		}
		this.flowVoxels[_index] = _flow;
	}

	// Token: 0x060064B4 RID: 25780 RVA: 0x00279AA8 File Offset: 0x00277CA8
	public void EnqueueFlow(int _voxelIndex, int _flow)
	{
		this.flowsFromOtherChunks.AddThreadSafe(new WaterFlow
		{
			voxelIndex = _voxelIndex,
			flow = _flow
		});
	}

	// Token: 0x060064B5 RID: 25781 RVA: 0x00279ADC File Offset: 0x00277CDC
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

	// Token: 0x060064B6 RID: 25782 RVA: 0x00279B30 File Offset: 0x00277D30
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

	// Token: 0x060064B7 RID: 25783 RVA: 0x00279B94 File Offset: 0x00277D94
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

	// Token: 0x060064B8 RID: 25784 RVA: 0x00279D24 File Offset: 0x00277F24
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

	// Token: 0x060064B9 RID: 25785 RVA: 0x00279DF4 File Offset: 0x00277FF4
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

	// Token: 0x060064BA RID: 25786 RVA: 0x00279EC4 File Offset: 0x002780C4
	public int CalculateOwnedBytes()
	{
		return 0 + this.voxelData.CalculateOwnedBytes() + this.voxelState.CalculateOwnedBytes() + this.groundWaterHeights.CalculateOwnedBytes() + ProfilerUtils.CalculateUnsafeBitArrayBytes(this.activeVoxels) + ProfilerUtils.CalculateUnsafeParallelHashMapBytes<int, int>(this.flowVoxels) + this.flowsFromOtherChunks.CalculateOwnedBytes() + this.activationsFromOtherChunks.CalculateOwnedBytes() + ProfilerUtils.CalculateUnsafeParallelHashSetBytes<int>(this.voxelsToWakeup);
	}

	// Token: 0x060064BB RID: 25787 RVA: 0x00279F34 File Offset: 0x00278134
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

	// Token: 0x060064BC RID: 25788 RVA: 0x0027A083 File Offset: 0x00278283
	public static int GetVoxelIndex(int _x, int _y, int _z)
	{
		return _x + _y * 256 + _z * 16;
	}

	// Token: 0x060064BD RID: 25789 RVA: 0x0027A094 File Offset: 0x00278294
	public static int3 GetVoxelCoords(int index)
	{
		int3 @int = default(int3);
		@int.y = index / 256;
		int num = index % 256;
		@int.z = num / 16;
		@int.x = num % 16;
		return @int;
	}

	// Token: 0x060064BE RID: 25790 RVA: 0x0027A0D6 File Offset: 0x002782D6
	public static int GetVoxelY(int _index)
	{
		return _index / 256;
	}

	// Token: 0x04004E8E RID: 20110
	public UnsafeChunkData<int> voxelData;

	// Token: 0x04004E8F RID: 20111
	public UnsafeChunkData<WaterVoxelState> voxelState;

	// Token: 0x04004E90 RID: 20112
	public UnsafeChunkXZMap<GroundWaterBounds> groundWaterHeights;

	// Token: 0x04004E91 RID: 20113
	public UnsafeBitArray activeVoxels;

	// Token: 0x04004E92 RID: 20114
	public UnsafeParallelHashMap<int, int> flowVoxels;

	// Token: 0x04004E93 RID: 20115
	public UnsafeFixedBuffer<WaterFlow> flowsFromOtherChunks;

	// Token: 0x04004E94 RID: 20116
	public UnsafeFixedBuffer<int> activationsFromOtherChunks;

	// Token: 0x04004E95 RID: 20117
	public UnsafeParallelHashSet<int> voxelsToWakeup;
}
