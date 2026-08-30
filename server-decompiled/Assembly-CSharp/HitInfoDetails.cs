using System;
using UnityEngine;

// Token: 0x02001301 RID: 4865
public struct HitInfoDetails
{
	// Token: 0x17001235 RID: 4661
	// (get) Token: 0x060099FF RID: 39423 RVA: 0x003A5214 File Offset: 0x003A3414
	public BlockValue blockValue
	{
		get
		{
			return this.voxelData.BlockValue;
		}
	}

	// Token: 0x17001236 RID: 4662
	// (get) Token: 0x06009A00 RID: 39424 RVA: 0x003A5221 File Offset: 0x003A3421
	public WaterValue waterValue
	{
		get
		{
			return this.voxelData.WaterValue;
		}
	}

	// Token: 0x17001237 RID: 4663
	// (get) Token: 0x06009A01 RID: 39425 RVA: 0x003A522E File Offset: 0x003A342E
	public PropRef propRef
	{
		get
		{
			return this.propData.PropRef;
		}
	}

	// Token: 0x17001238 RID: 4664
	// (get) Token: 0x06009A02 RID: 39426 RVA: 0x003A523B File Offset: 0x003A343B
	public PropValue propValue
	{
		get
		{
			return this.propData.PropValue;
		}
	}

	// Token: 0x17001239 RID: 4665
	// (get) Token: 0x06009A03 RID: 39427 RVA: 0x003A5248 File Offset: 0x003A3448
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

	// Token: 0x06009A04 RID: 39428 RVA: 0x003A5297 File Offset: 0x003A3497
	public void Clear()
	{
		this.pos = Vector3.zero;
		this.blockPos = Vector3i.zero;
		this.blockFace = BlockFace.Top;
		this.voxelData.Clear();
		this.propData.Clear();
		this.distanceSq = 0f;
	}

	// Token: 0x06009A05 RID: 39429 RVA: 0x003A52D8 File Offset: 0x003A34D8
	public void CopyFrom(HitInfoDetails _other)
	{
		this.pos = _other.pos;
		this.blockPos = _other.blockPos;
		this.blockFace = _other.blockFace;
		this.voxelData = _other.voxelData;
		this.propData = _other.propData;
		this.distanceSq = _other.distanceSq;
	}

	// Token: 0x06009A06 RID: 39430 RVA: 0x003A5330 File Offset: 0x003A3530
	public HitInfoDetails Clone()
	{
		HitInfoDetails hitInfoDetails = default(HitInfoDetails);
		hitInfoDetails.CopyFrom(this);
		return hitInfoDetails;
	}

	// Token: 0x0400741C RID: 29724
	public Vector3 pos;

	// Token: 0x0400741D RID: 29725
	public Vector3i blockPos;

	// Token: 0x0400741E RID: 29726
	public HitInfoDetails.VoxelData voxelData;

	// Token: 0x0400741F RID: 29727
	public HitInfoDetails.PropData propData;

	// Token: 0x04007420 RID: 29728
	public BlockFace blockFace;

	// Token: 0x04007421 RID: 29729
	public float distanceSq;

	// Token: 0x02001302 RID: 4866
	public struct VoxelData : IEquatable<HitInfoDetails.VoxelData>
	{
		// Token: 0x06009A07 RID: 39431 RVA: 0x003A5353 File Offset: 0x003A3553
		public void Set(BlockValue _bv, WaterValue _wv)
		{
			this.BlockValue = _bv;
			this.WaterValue = _wv;
		}

		// Token: 0x06009A08 RID: 39432 RVA: 0x003A5364 File Offset: 0x003A3564
		public static HitInfoDetails.VoxelData GetFrom(ChunkCluster _cc, Vector3i _blockPos)
		{
			return new HitInfoDetails.VoxelData
			{
				BlockValue = _cc.GetBlock(_blockPos),
				WaterValue = _cc.GetWater(_blockPos)
			};
		}

		// Token: 0x06009A09 RID: 39433 RVA: 0x003A5398 File Offset: 0x003A3598
		public static HitInfoDetails.VoxelData GetFrom(World _world, Vector3i _blockPos)
		{
			return new HitInfoDetails.VoxelData
			{
				BlockValue = _world.GetBlock(_blockPos),
				WaterValue = _world.GetWater(_blockPos)
			};
		}

		// Token: 0x06009A0A RID: 39434 RVA: 0x003A53CC File Offset: 0x003A35CC
		public static HitInfoDetails.VoxelData GetFrom(IChunk _chunk, int _x, int _y, int _z)
		{
			return new HitInfoDetails.VoxelData
			{
				BlockValue = _chunk.GetBlock(_x, _y, _z),
				WaterValue = _chunk.GetWater(_x, _y, _z)
			};
		}

		// Token: 0x06009A0B RID: 39435 RVA: 0x003A5402 File Offset: 0x003A3602
		public bool IsOnlyAir()
		{
			return this.BlockValue.isair && !this.WaterValue.HasMass();
		}

		// Token: 0x06009A0C RID: 39436 RVA: 0x003A5421 File Offset: 0x003A3621
		public bool IsOnlyWater()
		{
			return this.BlockValue.isair && this.WaterValue.HasMass();
		}

		// Token: 0x06009A0D RID: 39437 RVA: 0x003A543D File Offset: 0x003A363D
		public bool Equals(HitInfoDetails.VoxelData _other)
		{
			return this.BlockValue.Equals(_other.BlockValue) && this.WaterValue.HasMass() == _other.WaterValue.HasMass();
		}

		// Token: 0x06009A0E RID: 39438 RVA: 0x003A546D File Offset: 0x003A366D
		public void Clear()
		{
			this.BlockValue = BlockValue.Air;
			this.WaterValue = WaterValue.Empty;
		}

		// Token: 0x04007422 RID: 29730
		public BlockValue BlockValue;

		// Token: 0x04007423 RID: 29731
		public WaterValue WaterValue;
	}

	// Token: 0x02001303 RID: 4867
	public struct PropData : IEquatable<HitInfoDetails.PropData>
	{
		// Token: 0x1700123A RID: 4666
		// (get) Token: 0x06009A0F RID: 39439 RVA: 0x003A5485 File Offset: 0x003A3685
		// (set) Token: 0x06009A10 RID: 39440 RVA: 0x003A548D File Offset: 0x003A368D
		public PropRef PropRef
		{
			readonly get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x1700123B RID: 4667
		// (get) Token: 0x06009A11 RID: 39441 RVA: 0x003A5496 File Offset: 0x003A3696
		// (set) Token: 0x06009A12 RID: 39442 RVA: 0x003A549E File Offset: 0x003A369E
		public PropValue PropValue
		{
			readonly get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x06009A13 RID: 39443 RVA: 0x003A54A8 File Offset: 0x003A36A8
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

		// Token: 0x06009A14 RID: 39444 RVA: 0x003A5504 File Offset: 0x003A3704
		public override string ToString()
		{
			return string.Format("PropRef=[{0}], PropValue=[{1}]", this.PropRef, this.PropValue);
		}

		// Token: 0x06009A15 RID: 39445 RVA: 0x003A5526 File Offset: 0x003A3726
		public void Clear()
		{
			this.PropRef = PropRef.Default;
			this.PropValue = HitInfoDetails.PropData.Empty.PropValue;
		}

		// Token: 0x06009A16 RID: 39446 RVA: 0x003A5544 File Offset: 0x003A3744
		public bool Equals(HitInfoDetails.PropData other)
		{
			return this.PropRef.Equals(other.PropRef) && this.PropValue.Equals(other.PropValue);
		}

		// Token: 0x06009A17 RID: 39447 RVA: 0x003A5580 File Offset: 0x003A3780
		public override bool Equals(object obj)
		{
			if (obj is HitInfoDetails.PropData)
			{
				HitInfoDetails.PropData propData = (HitInfoDetails.PropData)obj;
				return this.Equals(propData);
			}
			return false;
		}

		// Token: 0x06009A18 RID: 39448 RVA: 0x003A55A5 File Offset: 0x003A37A5
		public override int GetHashCode()
		{
			return HashCode.Combine<PropRef, PropValue>(this.PropRef, this.PropValue);
		}

		// Token: 0x06009A19 RID: 39449 RVA: 0x003A55B8 File Offset: 0x003A37B8
		public static bool operator ==(HitInfoDetails.PropData left, HitInfoDetails.PropData right)
		{
			return left.Equals(right);
		}

		// Token: 0x06009A1A RID: 39450 RVA: 0x003A55C2 File Offset: 0x003A37C2
		public static bool operator !=(HitInfoDetails.PropData left, HitInfoDetails.PropData right)
		{
			return !left.Equals(right);
		}

		// Token: 0x04007424 RID: 29732
		public static readonly HitInfoDetails.PropData Empty = new HitInfoDetails.PropData
		{
			PropRef = PropRef.Default,
			PropValue = PropValue.AIR
		};
	}
}
