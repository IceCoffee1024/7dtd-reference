using System;
using UnityEngine;

// Token: 0x02001321 RID: 4897
public struct HitInfoDetails
{
	// Token: 0x1700127A RID: 4730
	// (get) Token: 0x06009AAB RID: 39595 RVA: 0x003A9DC4 File Offset: 0x003A7FC4
	public BlockValue blockValue
	{
		get
		{
			return this.voxelData.BlockValue;
		}
	}

	// Token: 0x1700127B RID: 4731
	// (get) Token: 0x06009AAC RID: 39596 RVA: 0x003A9DD1 File Offset: 0x003A7FD1
	public WaterValue waterValue
	{
		get
		{
			return this.voxelData.WaterValue;
		}
	}

	// Token: 0x1700127C RID: 4732
	// (get) Token: 0x06009AAD RID: 39597 RVA: 0x003A9DDE File Offset: 0x003A7FDE
	public PropRef propRef
	{
		get
		{
			return this.propData.PropRef;
		}
	}

	// Token: 0x1700127D RID: 4733
	// (get) Token: 0x06009AAE RID: 39598 RVA: 0x003A9DEB File Offset: 0x003A7FEB
	public PropValue propValue
	{
		get
		{
			return this.propData.PropValue;
		}
	}

	// Token: 0x1700127E RID: 4734
	// (get) Token: 0x06009AAF RID: 39599 RVA: 0x003A9DF8 File Offset: 0x003A7FF8
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

	// Token: 0x06009AB0 RID: 39600 RVA: 0x003A9E47 File Offset: 0x003A8047
	public void Clear()
	{
		this.pos = Vector3.zero;
		this.blockPos = Vector3i.zero;
		this.blockFace = BlockFace.Top;
		this.voxelData.Clear();
		this.propData.Clear();
		this.distanceSq = 0f;
	}

	// Token: 0x06009AB1 RID: 39601 RVA: 0x003A9E88 File Offset: 0x003A8088
	public void CopyFrom(HitInfoDetails _other)
	{
		this.pos = _other.pos;
		this.blockPos = _other.blockPos;
		this.blockFace = _other.blockFace;
		this.voxelData = _other.voxelData;
		this.propData = _other.propData;
		this.distanceSq = _other.distanceSq;
	}

	// Token: 0x06009AB2 RID: 39602 RVA: 0x003A9EE0 File Offset: 0x003A80E0
	public HitInfoDetails Clone()
	{
		HitInfoDetails hitInfoDetails = default(HitInfoDetails);
		hitInfoDetails.CopyFrom(this);
		return hitInfoDetails;
	}

	// Token: 0x04007497 RID: 29847
	public Vector3 pos;

	// Token: 0x04007498 RID: 29848
	public Vector3i blockPos;

	// Token: 0x04007499 RID: 29849
	public HitInfoDetails.VoxelData voxelData;

	// Token: 0x0400749A RID: 29850
	public HitInfoDetails.PropData propData;

	// Token: 0x0400749B RID: 29851
	public BlockFace blockFace;

	// Token: 0x0400749C RID: 29852
	public float distanceSq;

	// Token: 0x02001322 RID: 4898
	public struct VoxelData : IEquatable<HitInfoDetails.VoxelData>
	{
		// Token: 0x06009AB3 RID: 39603 RVA: 0x003A9F03 File Offset: 0x003A8103
		public void Set(BlockValue _bv, WaterValue _wv)
		{
			this.BlockValue = _bv;
			this.WaterValue = _wv;
		}

		// Token: 0x06009AB4 RID: 39604 RVA: 0x003A9F14 File Offset: 0x003A8114
		public static HitInfoDetails.VoxelData GetFrom(ChunkCluster _cc, Vector3i _blockPos)
		{
			return new HitInfoDetails.VoxelData
			{
				BlockValue = _cc.GetBlock(_blockPos),
				WaterValue = _cc.GetWater(_blockPos)
			};
		}

		// Token: 0x06009AB5 RID: 39605 RVA: 0x003A9F48 File Offset: 0x003A8148
		public static HitInfoDetails.VoxelData GetFrom(World _world, Vector3i _blockPos)
		{
			return new HitInfoDetails.VoxelData
			{
				BlockValue = _world.GetBlock(_blockPos),
				WaterValue = _world.GetWater(_blockPos)
			};
		}

		// Token: 0x06009AB6 RID: 39606 RVA: 0x003A9F7C File Offset: 0x003A817C
		public static HitInfoDetails.VoxelData GetFrom(IChunk _chunk, int _x, int _y, int _z)
		{
			return new HitInfoDetails.VoxelData
			{
				BlockValue = _chunk.GetBlock(_x, _y, _z),
				WaterValue = _chunk.GetWater(_x, _y, _z)
			};
		}

		// Token: 0x06009AB7 RID: 39607 RVA: 0x003A9FB2 File Offset: 0x003A81B2
		public bool IsOnlyAir()
		{
			return this.BlockValue.isair && !this.WaterValue.HasMass();
		}

		// Token: 0x06009AB8 RID: 39608 RVA: 0x003A9FD1 File Offset: 0x003A81D1
		public bool IsOnlyWater()
		{
			return this.BlockValue.isair && this.WaterValue.HasMass();
		}

		// Token: 0x06009AB9 RID: 39609 RVA: 0x003A9FED File Offset: 0x003A81ED
		public bool Equals(HitInfoDetails.VoxelData _other)
		{
			return this.BlockValue.Equals(_other.BlockValue) && this.WaterValue.HasMass() == _other.WaterValue.HasMass();
		}

		// Token: 0x06009ABA RID: 39610 RVA: 0x003AA01D File Offset: 0x003A821D
		public void Clear()
		{
			this.BlockValue = BlockValue.Air;
			this.WaterValue = WaterValue.Empty;
		}

		// Token: 0x0400749D RID: 29853
		public BlockValue BlockValue;

		// Token: 0x0400749E RID: 29854
		public WaterValue WaterValue;
	}

	// Token: 0x02001323 RID: 4899
	public struct PropData : IEquatable<HitInfoDetails.PropData>
	{
		// Token: 0x1700127F RID: 4735
		// (get) Token: 0x06009ABB RID: 39611 RVA: 0x003AA035 File Offset: 0x003A8235
		// (set) Token: 0x06009ABC RID: 39612 RVA: 0x003AA03D File Offset: 0x003A823D
		public PropRef PropRef
		{
			readonly get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001280 RID: 4736
		// (get) Token: 0x06009ABD RID: 39613 RVA: 0x003AA046 File Offset: 0x003A8246
		// (set) Token: 0x06009ABE RID: 39614 RVA: 0x003AA04E File Offset: 0x003A824E
		public PropValue PropValue
		{
			readonly get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x06009ABF RID: 39615 RVA: 0x003AA058 File Offset: 0x003A8258
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

		// Token: 0x06009AC0 RID: 39616 RVA: 0x003AA0B4 File Offset: 0x003A82B4
		public override string ToString()
		{
			return string.Format("PropRef=[{0}], PropValue=[{1}]", this.PropRef, this.PropValue);
		}

		// Token: 0x06009AC1 RID: 39617 RVA: 0x003AA0D6 File Offset: 0x003A82D6
		public void Clear()
		{
			this.PropRef = PropRef.Default;
			this.PropValue = HitInfoDetails.PropData.Empty.PropValue;
		}

		// Token: 0x06009AC2 RID: 39618 RVA: 0x003AA0F4 File Offset: 0x003A82F4
		public bool Equals(HitInfoDetails.PropData other)
		{
			return this.PropRef.Equals(other.PropRef) && this.PropValue.Equals(other.PropValue);
		}

		// Token: 0x06009AC3 RID: 39619 RVA: 0x003AA130 File Offset: 0x003A8330
		public override bool Equals(object obj)
		{
			if (obj is HitInfoDetails.PropData)
			{
				HitInfoDetails.PropData propData = (HitInfoDetails.PropData)obj;
				return this.Equals(propData);
			}
			return false;
		}

		// Token: 0x06009AC4 RID: 39620 RVA: 0x003AA155 File Offset: 0x003A8355
		public override int GetHashCode()
		{
			return HashCode.Combine<PropRef, PropValue>(this.PropRef, this.PropValue);
		}

		// Token: 0x06009AC5 RID: 39621 RVA: 0x003AA168 File Offset: 0x003A8368
		public static bool operator ==(HitInfoDetails.PropData left, HitInfoDetails.PropData right)
		{
			return left.Equals(right);
		}

		// Token: 0x06009AC6 RID: 39622 RVA: 0x003AA172 File Offset: 0x003A8372
		public static bool operator !=(HitInfoDetails.PropData left, HitInfoDetails.PropData right)
		{
			return !left.Equals(right);
		}

		// Token: 0x0400749F RID: 29855
		public static readonly HitInfoDetails.PropData Empty = new HitInfoDetails.PropData
		{
			PropRef = PropRef.Default,
			PropValue = PropValue.AIR
		};
	}
}
