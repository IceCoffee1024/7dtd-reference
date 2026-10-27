using System;
using UnityEngine;

// Token: 0x02001333 RID: 4915
public struct HitInfoDetails
{
	// Token: 0x17001286 RID: 4742
	// (get) Token: 0x06009B03 RID: 39683 RVA: 0x003AC1C8 File Offset: 0x003AA3C8
	public BlockValue blockValue
	{
		get
		{
			return this.voxelData.BlockValue;
		}
	}

	// Token: 0x17001287 RID: 4743
	// (get) Token: 0x06009B04 RID: 39684 RVA: 0x003AC1D5 File Offset: 0x003AA3D5
	public WaterValue waterValue
	{
		get
		{
			return this.voxelData.WaterValue;
		}
	}

	// Token: 0x17001288 RID: 4744
	// (get) Token: 0x06009B05 RID: 39685 RVA: 0x003AC1E2 File Offset: 0x003AA3E2
	public PropRef propRef
	{
		get
		{
			return this.propData.PropRef;
		}
	}

	// Token: 0x17001289 RID: 4745
	// (get) Token: 0x06009B06 RID: 39686 RVA: 0x003AC1EF File Offset: 0x003AA3EF
	public PropValue propValue
	{
		get
		{
			return this.propData.PropValue;
		}
	}

	// Token: 0x1700128A RID: 4746
	// (get) Token: 0x06009B07 RID: 39687 RVA: 0x003AC1FC File Offset: 0x003AA3FC
	public BlockValueRef blockValueRef
	{
		get
		{
			if (!this.propValue.IsAir)
			{
				return new BlockValueRef(this.propData.PropRef);
			}
			if (!this.blockValue.isair)
			{
				return new BlockValueRef(this.blockPos);
			}
			return BlockValueRef.None;
		}
	}

	// Token: 0x06009B08 RID: 39688 RVA: 0x003AC24B File Offset: 0x003AA44B
	public void Clear()
	{
		this.pos = Vector3.zero;
		this.blockPos = Vector3i.zero;
		this.blockFace = BlockFace.Top;
		this.voxelData.Clear();
		this.propData.Clear();
		this.distanceSq = 0f;
	}

	// Token: 0x06009B09 RID: 39689 RVA: 0x003AC28C File Offset: 0x003AA48C
	public void CopyFrom(HitInfoDetails _other)
	{
		this.pos = _other.pos;
		this.blockPos = _other.blockPos;
		this.blockFace = _other.blockFace;
		this.voxelData = _other.voxelData;
		this.propData = _other.propData;
		this.distanceSq = _other.distanceSq;
	}

	// Token: 0x06009B0A RID: 39690 RVA: 0x003AC2E4 File Offset: 0x003AA4E4
	public HitInfoDetails Clone()
	{
		HitInfoDetails hitInfoDetails = default(HitInfoDetails);
		hitInfoDetails.CopyFrom(this);
		return hitInfoDetails;
	}

	// Token: 0x040074D8 RID: 29912
	public Vector3 pos;

	// Token: 0x040074D9 RID: 29913
	public Vector3i blockPos;

	// Token: 0x040074DA RID: 29914
	public HitInfoDetails.VoxelData voxelData;

	// Token: 0x040074DB RID: 29915
	public HitInfoDetails.PropData propData;

	// Token: 0x040074DC RID: 29916
	public BlockFace blockFace;

	// Token: 0x040074DD RID: 29917
	public float distanceSq;

	// Token: 0x02001334 RID: 4916
	public struct VoxelData : IEquatable<HitInfoDetails.VoxelData>
	{
		// Token: 0x06009B0B RID: 39691 RVA: 0x003AC307 File Offset: 0x003AA507
		public void Set(BlockValue _bv, WaterValue _wv)
		{
			this.BlockValue = _bv;
			this.WaterValue = _wv;
		}

		// Token: 0x06009B0C RID: 39692 RVA: 0x003AC318 File Offset: 0x003AA518
		public static HitInfoDetails.VoxelData GetFrom(ChunkCluster _cc, Vector3i _blockPos)
		{
			return new HitInfoDetails.VoxelData
			{
				BlockValue = _cc.GetBlock(_blockPos),
				WaterValue = _cc.GetWater(_blockPos)
			};
		}

		// Token: 0x06009B0D RID: 39693 RVA: 0x003AC34C File Offset: 0x003AA54C
		public static HitInfoDetails.VoxelData GetFrom(World _world, Vector3i _blockPos)
		{
			return new HitInfoDetails.VoxelData
			{
				BlockValue = _world.GetBlock(_blockPos),
				WaterValue = _world.GetWater(_blockPos)
			};
		}

		// Token: 0x06009B0E RID: 39694 RVA: 0x003AC380 File Offset: 0x003AA580
		public static HitInfoDetails.VoxelData GetFrom(IChunk _chunk, int _x, int _y, int _z)
		{
			return new HitInfoDetails.VoxelData
			{
				BlockValue = _chunk.GetBlock(_x, _y, _z),
				WaterValue = _chunk.GetWater(_x, _y, _z)
			};
		}

		// Token: 0x06009B0F RID: 39695 RVA: 0x003AC3B6 File Offset: 0x003AA5B6
		public bool IsOnlyAir()
		{
			return this.BlockValue.isair && !this.WaterValue.HasMass();
		}

		// Token: 0x06009B10 RID: 39696 RVA: 0x003AC3D5 File Offset: 0x003AA5D5
		public bool IsOnlyWater()
		{
			return this.BlockValue.isair && this.WaterValue.HasMass();
		}

		// Token: 0x06009B11 RID: 39697 RVA: 0x003AC3F1 File Offset: 0x003AA5F1
		public bool Equals(HitInfoDetails.VoxelData _other)
		{
			return this.BlockValue.Equals(_other.BlockValue) && this.WaterValue.HasMass() == _other.WaterValue.HasMass();
		}

		// Token: 0x06009B12 RID: 39698 RVA: 0x003AC421 File Offset: 0x003AA621
		public void Clear()
		{
			this.BlockValue = BlockValue.Air;
			this.WaterValue = WaterValue.Empty;
		}

		// Token: 0x040074DE RID: 29918
		public BlockValue BlockValue;

		// Token: 0x040074DF RID: 29919
		public WaterValue WaterValue;
	}

	// Token: 0x02001335 RID: 4917
	public struct PropData : IEquatable<HitInfoDetails.PropData>
	{
		// Token: 0x1700128B RID: 4747
		// (get) Token: 0x06009B13 RID: 39699 RVA: 0x003AC439 File Offset: 0x003AA639
		// (set) Token: 0x06009B14 RID: 39700 RVA: 0x003AC441 File Offset: 0x003AA641
		public PropRef PropRef
		{
			readonly get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x1700128C RID: 4748
		// (get) Token: 0x06009B15 RID: 39701 RVA: 0x003AC44A File Offset: 0x003AA64A
		// (set) Token: 0x06009B16 RID: 39702 RVA: 0x003AC452 File Offset: 0x003AA652
		public PropValue PropValue
		{
			readonly get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x06009B17 RID: 39703 RVA: 0x003AC45C File Offset: 0x003AA65C
		public static HitInfoDetails.PropData GetFrom(ChunkCluster cc, Transform transform)
		{
			PropReference propReference;
			if (!transform || !transform.TryGetComponent<PropReference>(out propReference))
			{
				return HitInfoDetails.PropData.Empty;
			}
			PropRef propRef = propReference.PropRef;
			PropValue prop = cc.GetProp(propRef);
			if (prop.IsAir)
			{
				return HitInfoDetails.PropData.Empty;
			}
			return new HitInfoDetails.PropData
			{
				PropRef = propRef,
				PropValue = prop
			};
		}

		// Token: 0x06009B18 RID: 39704 RVA: 0x003AC4B8 File Offset: 0x003AA6B8
		public override string ToString()
		{
			return string.Format("PropRef=[{0}], PropValue=[{1}]", this.PropRef, this.PropValue);
		}

		// Token: 0x06009B19 RID: 39705 RVA: 0x003AC4DA File Offset: 0x003AA6DA
		public void Clear()
		{
			this.PropRef = PropRef.Default;
			this.PropValue = HitInfoDetails.PropData.Empty.PropValue;
		}

		// Token: 0x06009B1A RID: 39706 RVA: 0x003AC4F8 File Offset: 0x003AA6F8
		public bool Equals(HitInfoDetails.PropData other)
		{
			return this.PropRef.Equals(other.PropRef) && this.PropValue.Equals(other.PropValue);
		}

		// Token: 0x06009B1B RID: 39707 RVA: 0x003AC534 File Offset: 0x003AA734
		public override bool Equals(object obj)
		{
			if (obj is HitInfoDetails.PropData)
			{
				HitInfoDetails.PropData propData = (HitInfoDetails.PropData)obj;
				return this.Equals(propData);
			}
			return false;
		}

		// Token: 0x06009B1C RID: 39708 RVA: 0x003AC559 File Offset: 0x003AA759
		public override int GetHashCode()
		{
			return HashCode.Combine<PropRef, PropValue>(this.PropRef, this.PropValue);
		}

		// Token: 0x06009B1D RID: 39709 RVA: 0x003AC56C File Offset: 0x003AA76C
		public static bool operator ==(HitInfoDetails.PropData left, HitInfoDetails.PropData right)
		{
			return left.Equals(right);
		}

		// Token: 0x06009B1E RID: 39710 RVA: 0x003AC576 File Offset: 0x003AA776
		public static bool operator !=(HitInfoDetails.PropData left, HitInfoDetails.PropData right)
		{
			return !left.Equals(right);
		}

		// Token: 0x040074E0 RID: 29920
		public static readonly HitInfoDetails.PropData Empty = new HitInfoDetails.PropData
		{
			PropRef = PropRef.Default,
			PropValue = PropValue.AIR
		};
	}
}
