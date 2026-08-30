using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;

// Token: 0x02000CCC RID: 3276
public struct WaterDataHandle : IDisposable
{
	// Token: 0x17000A7F RID: 2687
	// (get) Token: 0x06006488 RID: 25736 RVA: 0x00274D8C File Offset: 0x00272F8C
	public UnsafeBitArraySetIndicesEnumerator ActiveVoxelIndices
	{
		get
		{
			return new UnsafeBitArraySetIndicesEnumerator(this.activeVoxels);
		}
	}

	// Token: 0x17000A80 RID: 2688
	// (get) Token: 0x06006489 RID: 25737 RVA: 0x00274D99 File Offset: 0x00272F99
	public UnsafeParallelHashMap<int, int>.Enumerator FlowVoxels
	{
		get
		{
			return this.flowVoxels.GetEnumerator();
		}
	}

	// Token: 0x17000A81 RID: 2689
	// (get) Token: 0x0600648A RID: 25738 RVA: 0x00274DA6 File Offset: 0x00272FA6
	public bool HasActiveWater
	{
		get
		{
			return this.activeVoxels.TestAny(0, this.activeVoxels.Length);
		}
	}

	// Token: 0x17000A82 RID: 2690
	// (get) Token: 0x0600648B RID: 25739 RVA: 0x00274DBF File Offset: 0x00272FBF
	public bool HasFlows
	{
		get
		{
			return !this.flowVoxels.IsEmpty;
		}
	}

	// Token: 0x0600648C RID: 25740 RVA: 0x00274DD0 File Offset: 0x00272FD0
	public static WaterDataHandle AllocateNew(Allocator allocator)
	{
		WaterDataHandle waterDataHandle = default(WaterDataHandle);
		waterDataHandle.Allocate(allocator);
		return waterDataHandle;
	}

	// Token: 0x0600648D RID: 25741 RVA: 0x00274DF0 File Offset: 0x00272FF0
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

	// Token: 0x0600648E RID: 25742 RVA: 0x00274EA0 File Offset: 0x002730A0
	public bool IsInGroundWater(int _x, int _y, int _z)
	{
		GroundWaterBounds groundWaterBounds = this.groundWaterHeights.Get(_x, _z);
		return groundWaterBounds.IsGroundWater && _y >= (int)groundWaterBounds.bottom && _y <= (int)groundWaterBounds.waterHeight;
	}

	// Token: 0x0600648F RID: 25743 RVA: 0x00274EDD File Offset: 0x002730DD
	public void SetVoxelActive(int _x, int _y, int _z)
	{
		this.activeVoxels.Set(WaterDataHandle.GetVoxelIndex(_x, _y, _z), true);
	}

	// Token: 0x06006490 RID: 25744 RVA: 0x00274EF3 File Offset: 0x002730F3
	public void SetVoxelActive(int _index)
	{
		this.activeVoxels.Set(_index, true);
	}

	// Token: 0x06006491 RID: 25745 RVA: 0x00274F02 File Offset: 0x00273102
	public void EnqueueVoxelActive(int _x, int _y, int _z)
	{
		this.EnqueueVoxelActive(WaterDataHandle.GetVoxelIndex(_x, _y, _z));
	}

	// Token: 0x06006492 RID: 25746 RVA: 0x00274F12 File Offset: 0x00273112
	public void EnqueueVoxelActive(int _index)
	{
		this.activationsFromOtherChunks.AddThreadSafe(_index);
	}

	// Token: 0x06006493 RID: 25747 RVA: 0x00274F20 File Offset: 0x00273120
	public void EnqueueVoxelWakeup(int _x, int _y, int _z)
	{
		this.EnqueueVoxelWakeup(WaterDataHandle.GetVoxelIndex(_x, _y, _z));
	}

	// Token: 0x06006494 RID: 25748 RVA: 0x00274F30 File Offset: 0x00273130
	public void EnqueueVoxelWakeup(int _index)
	{
		this.voxelsToWakeup.Add(_index);
	}

	// Token: 0x06006495 RID: 25749 RVA: 0x00274F40 File Offset: 0x00273140
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

	// Token: 0x06006496 RID: 25750 RVA: 0x00274F86 File Offset: 0x00273186
	public void SetVoxelInactive(int _index)
	{
		this.activeVoxels.Set(_index, false);
	}

	// Token: 0x06006497 RID: 25751 RVA: 0x00274F98 File Offset: 0x00273198
	public void SetVoxelMass(int _x, int _y, int _z, int _mass)
	{
		int voxelIndex = WaterDataHandle.GetVoxelIndex(_x, _y, _z);
		this.SetVoxelMass(voxelIndex, _mass);
	}

	// Token: 0x06006498 RID: 25752 RVA: 0x00274FB7 File Offset: 0x002731B7
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

	// Token: 0x06006499 RID: 25753 RVA: 0x00274FEC File Offset: 0x002731EC
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

	// Token: 0x0600649A RID: 25754 RVA: 0x0027513A File Offset: 0x0027333A
	public void ApplyFlow(int _x, int _y, int _z, int _flow)
	{
		this.ApplyFlow(WaterDataHandle.GetVoxelIndex(_x, _y, _z), _flow);
	}

	// Token: 0x0600649B RID: 25755 RVA: 0x0027514C File Offset: 0x0027334C
	public void ApplyFlow(int _index, int _flow)
	{
		int num;
		if (this.flowVoxels.TryGetValue(_index, out num))
		{
			_flow += num;
		}
		this.flowVoxels[_index] = _flow;
	}

	// Token: 0x0600649C RID: 25756 RVA: 0x0027517C File Offset: 0x0027337C
	public void EnqueueFlow(int _voxelIndex, int _flow)
	{
		this.flowsFromOtherChunks.AddThreadSafe(new WaterFlow
		{
			voxelIndex = _voxelIndex,
			flow = _flow
		});
	}

	// Token: 0x0600649D RID: 25757 RVA: 0x002751B0 File Offset: 0x002733B0
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

	// Token: 0x0600649E RID: 25758 RVA: 0x00275204 File Offset: 0x00273404
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

	// Token: 0x0600649F RID: 25759 RVA: 0x00275268 File Offset: 0x00273468
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

	// Token: 0x060064A0 RID: 25760 RVA: 0x002753F8 File Offset: 0x002735F8
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

	// Token: 0x060064A1 RID: 25761 RVA: 0x002754C8 File Offset: 0x002736C8
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

	// Token: 0x060064A2 RID: 25762 RVA: 0x00275598 File Offset: 0x00273798
	public int CalculateOwnedBytes()
	{
		return 0 + this.voxelData.CalculateOwnedBytes() + this.voxelState.CalculateOwnedBytes() + this.groundWaterHeights.CalculateOwnedBytes() + ProfilerUtils.CalculateUnsafeBitArrayBytes(this.activeVoxels) + ProfilerUtils.CalculateUnsafeParallelHashMapBytes<int, int>(this.flowVoxels) + this.flowsFromOtherChunks.CalculateOwnedBytes() + this.activationsFromOtherChunks.CalculateOwnedBytes() + ProfilerUtils.CalculateUnsafeParallelHashSetBytes<int>(this.voxelsToWakeup);
	}

	// Token: 0x060064A3 RID: 25763 RVA: 0x00275608 File Offset: 0x00273808
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

	// Token: 0x060064A4 RID: 25764 RVA: 0x00275757 File Offset: 0x00273957
	public static int GetVoxelIndex(int _x, int _y, int _z)
	{
		return _x + _y * 256 + _z * 16;
	}

	// Token: 0x060064A5 RID: 25765 RVA: 0x00275768 File Offset: 0x00273968
	public static int3 GetVoxelCoords(int index)
	{
		int3 @int = default(int3);
		@int.y = index / 256;
		int num = index % 256;
		@int.z = num / 16;
		@int.x = num % 16;
		return @int;
	}

	// Token: 0x060064A6 RID: 25766 RVA: 0x002757AA File Offset: 0x002739AA
	public static int GetVoxelY(int _index)
	{
		return _index / 256;
	}

	// Token: 0x04004E33 RID: 20019
	public UnsafeChunkData<int> voxelData;

	// Token: 0x04004E34 RID: 20020
	public UnsafeChunkData<WaterVoxelState> voxelState;

	// Token: 0x04004E35 RID: 20021
	public UnsafeChunkXZMap<GroundWaterBounds> groundWaterHeights;

	// Token: 0x04004E36 RID: 20022
	public UnsafeBitArray activeVoxels;

	// Token: 0x04004E37 RID: 20023
	public UnsafeParallelHashMap<int, int> flowVoxels;

	// Token: 0x04004E38 RID: 20024
	public UnsafeFixedBuffer<WaterFlow> flowsFromOtherChunks;

	// Token: 0x04004E39 RID: 20025
	public UnsafeFixedBuffer<int> activationsFromOtherChunks;

	// Token: 0x04004E3A RID: 20026
	public UnsafeParallelHashSet<int> voxelsToWakeup;
}
