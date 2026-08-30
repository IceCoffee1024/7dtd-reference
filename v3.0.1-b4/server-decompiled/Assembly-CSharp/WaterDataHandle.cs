using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;

// Token: 0x02000CBA RID: 3258
public struct WaterDataHandle : IDisposable
{
	// Token: 0x17000A6C RID: 2668
	// (get) Token: 0x060063FC RID: 25596 RVA: 0x00271E90 File Offset: 0x00270090
	public UnsafeBitArraySetIndicesEnumerator ActiveVoxelIndices
	{
		get
		{
			return new UnsafeBitArraySetIndicesEnumerator(this.activeVoxels);
		}
	}

	// Token: 0x17000A6D RID: 2669
	// (get) Token: 0x060063FD RID: 25597 RVA: 0x00271E9D File Offset: 0x0027009D
	public UnsafeParallelHashMap<int, int>.Enumerator FlowVoxels
	{
		get
		{
			return this.flowVoxels.GetEnumerator();
		}
	}

	// Token: 0x17000A6E RID: 2670
	// (get) Token: 0x060063FE RID: 25598 RVA: 0x00271EAA File Offset: 0x002700AA
	public bool HasActiveWater
	{
		get
		{
			return this.activeVoxels.TestAny(0, this.activeVoxels.Length);
		}
	}

	// Token: 0x17000A6F RID: 2671
	// (get) Token: 0x060063FF RID: 25599 RVA: 0x00271EC3 File Offset: 0x002700C3
	public bool HasFlows
	{
		get
		{
			return !this.flowVoxels.IsEmpty;
		}
	}

	// Token: 0x06006400 RID: 25600 RVA: 0x00271ED4 File Offset: 0x002700D4
	public static WaterDataHandle AllocateNew(Allocator allocator)
	{
		WaterDataHandle waterDataHandle = default(WaterDataHandle);
		waterDataHandle.Allocate(allocator);
		return waterDataHandle;
	}

	// Token: 0x06006401 RID: 25601 RVA: 0x00271EF4 File Offset: 0x002700F4
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

	// Token: 0x06006402 RID: 25602 RVA: 0x00271FA4 File Offset: 0x002701A4
	public bool IsInGroundWater(int _x, int _y, int _z)
	{
		GroundWaterBounds groundWaterBounds = this.groundWaterHeights.Get(_x, _z);
		return groundWaterBounds.IsGroundWater && _y >= (int)groundWaterBounds.bottom && _y <= (int)groundWaterBounds.waterHeight;
	}

	// Token: 0x06006403 RID: 25603 RVA: 0x00271FE1 File Offset: 0x002701E1
	public void SetVoxelActive(int _x, int _y, int _z)
	{
		this.activeVoxels.Set(WaterDataHandle.GetVoxelIndex(_x, _y, _z), true);
	}

	// Token: 0x06006404 RID: 25604 RVA: 0x00271FF7 File Offset: 0x002701F7
	public void SetVoxelActive(int _index)
	{
		this.activeVoxels.Set(_index, true);
	}

	// Token: 0x06006405 RID: 25605 RVA: 0x00272006 File Offset: 0x00270206
	public void EnqueueVoxelActive(int _x, int _y, int _z)
	{
		this.EnqueueVoxelActive(WaterDataHandle.GetVoxelIndex(_x, _y, _z));
	}

	// Token: 0x06006406 RID: 25606 RVA: 0x00272016 File Offset: 0x00270216
	public void EnqueueVoxelActive(int _index)
	{
		this.activationsFromOtherChunks.AddThreadSafe(_index);
	}

	// Token: 0x06006407 RID: 25607 RVA: 0x00272024 File Offset: 0x00270224
	public void EnqueueVoxelWakeup(int _x, int _y, int _z)
	{
		this.EnqueueVoxelWakeup(WaterDataHandle.GetVoxelIndex(_x, _y, _z));
	}

	// Token: 0x06006408 RID: 25608 RVA: 0x00272034 File Offset: 0x00270234
	public void EnqueueVoxelWakeup(int _index)
	{
		this.voxelsToWakeup.Add(_index);
	}

	// Token: 0x06006409 RID: 25609 RVA: 0x00272044 File Offset: 0x00270244
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

	// Token: 0x0600640A RID: 25610 RVA: 0x0027208A File Offset: 0x0027028A
	public void SetVoxelInactive(int _index)
	{
		this.activeVoxels.Set(_index, false);
	}

	// Token: 0x0600640B RID: 25611 RVA: 0x0027209C File Offset: 0x0027029C
	public void SetVoxelMass(int _x, int _y, int _z, int _mass)
	{
		int voxelIndex = WaterDataHandle.GetVoxelIndex(_x, _y, _z);
		this.SetVoxelMass(voxelIndex, _mass);
	}

	// Token: 0x0600640C RID: 25612 RVA: 0x002720BB File Offset: 0x002702BB
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

	// Token: 0x0600640D RID: 25613 RVA: 0x002720F0 File Offset: 0x002702F0
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

	// Token: 0x0600640E RID: 25614 RVA: 0x0027223E File Offset: 0x0027043E
	public void ApplyFlow(int _x, int _y, int _z, int _flow)
	{
		this.ApplyFlow(WaterDataHandle.GetVoxelIndex(_x, _y, _z), _flow);
	}

	// Token: 0x0600640F RID: 25615 RVA: 0x00272250 File Offset: 0x00270450
	public void ApplyFlow(int _index, int _flow)
	{
		int num;
		if (this.flowVoxels.TryGetValue(_index, out num))
		{
			_flow += num;
		}
		this.flowVoxels[_index] = _flow;
	}

	// Token: 0x06006410 RID: 25616 RVA: 0x00272280 File Offset: 0x00270480
	public void EnqueueFlow(int _voxelIndex, int _flow)
	{
		this.flowsFromOtherChunks.AddThreadSafe(new WaterFlow
		{
			voxelIndex = _voxelIndex,
			flow = _flow
		});
	}

	// Token: 0x06006411 RID: 25617 RVA: 0x002722B4 File Offset: 0x002704B4
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

	// Token: 0x06006412 RID: 25618 RVA: 0x00272308 File Offset: 0x00270508
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

	// Token: 0x06006413 RID: 25619 RVA: 0x0027236C File Offset: 0x0027056C
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

	// Token: 0x06006414 RID: 25620 RVA: 0x002724FC File Offset: 0x002706FC
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

	// Token: 0x06006415 RID: 25621 RVA: 0x002725CC File Offset: 0x002707CC
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

	// Token: 0x06006416 RID: 25622 RVA: 0x0027269C File Offset: 0x0027089C
	public int CalculateOwnedBytes()
	{
		return 0 + this.voxelData.CalculateOwnedBytes() + this.voxelState.CalculateOwnedBytes() + this.groundWaterHeights.CalculateOwnedBytes() + ProfilerUtils.CalculateUnsafeBitArrayBytes(this.activeVoxels) + ProfilerUtils.CalculateUnsafeParallelHashMapBytes<int, int>(this.flowVoxels) + this.flowsFromOtherChunks.CalculateOwnedBytes() + this.activationsFromOtherChunks.CalculateOwnedBytes() + ProfilerUtils.CalculateUnsafeParallelHashSetBytes<int>(this.voxelsToWakeup);
	}

	// Token: 0x06006417 RID: 25623 RVA: 0x0027270C File Offset: 0x0027090C
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

	// Token: 0x06006418 RID: 25624 RVA: 0x0027285B File Offset: 0x00270A5B
	public static int GetVoxelIndex(int _x, int _y, int _z)
	{
		return _x + _y * 256 + _z * 16;
	}

	// Token: 0x06006419 RID: 25625 RVA: 0x0027286C File Offset: 0x00270A6C
	public static int3 GetVoxelCoords(int index)
	{
		int3 @int = default(int3);
		@int.y = index / 256;
		int num = index % 256;
		@int.z = num / 16;
		@int.x = num % 16;
		return @int;
	}

	// Token: 0x0600641A RID: 25626 RVA: 0x002728AE File Offset: 0x00270AAE
	public static int GetVoxelY(int _index)
	{
		return _index / 256;
	}

	// Token: 0x04004D95 RID: 19861
	public UnsafeChunkData<int> voxelData;

	// Token: 0x04004D96 RID: 19862
	public UnsafeChunkData<WaterVoxelState> voxelState;

	// Token: 0x04004D97 RID: 19863
	public UnsafeChunkXZMap<GroundWaterBounds> groundWaterHeights;

	// Token: 0x04004D98 RID: 19864
	public UnsafeBitArray activeVoxels;

	// Token: 0x04004D99 RID: 19865
	public UnsafeParallelHashMap<int, int> flowVoxels;

	// Token: 0x04004D9A RID: 19866
	public UnsafeFixedBuffer<WaterFlow> flowsFromOtherChunks;

	// Token: 0x04004D9B RID: 19867
	public UnsafeFixedBuffer<int> activationsFromOtherChunks;

	// Token: 0x04004D9C RID: 19868
	public UnsafeParallelHashSet<int> voxelsToWakeup;
}
