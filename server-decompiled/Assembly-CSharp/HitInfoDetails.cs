using System;
using UnityEngine;

// Token: 0x020012E7 RID: 4839
public struct HitInfoDetails
{
	// Token: 0x170011FC RID: 4604
	// (get) Token: 0x06009904 RID: 39172 RVA: 0x0039FF7C File Offset: 0x0039E17C
	public BlockValue blockValue
	{
		get
		{
			return this.voxelData.BlockValue;
		}
	}

	// Token: 0x170011FD RID: 4605
	// (get) Token: 0x06009905 RID: 39173 RVA: 0x0039FF89 File Offset: 0x0039E189
	public WaterValue waterValue
	{
		get
		{
			return this.voxelData.WaterValue;
		}
	}

	// Token: 0x170011FE RID: 4606
	// (get) Token: 0x06009906 RID: 39174 RVA: 0x0039FF96 File Offset: 0x0039E196
	public PropRef propRef
	{
		get
		{
			return this.propData.PropRef;
		}
	}

	// Token: 0x170011FF RID: 4607
	// (get) Token: 0x06009907 RID: 39175 RVA: 0x0039FFA3 File Offset: 0x0039E1A3
	public PropValue propValue
	{
		get
		{
			return this.propData.PropValue;
		}
	}

	// Token: 0x17001200 RID: 4608
	// (get) Token: 0x06009908 RID: 39176 RVA: 0x0039FFB0 File Offset: 0x0039E1B0
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

	// Token: 0x06009909 RID: 39177 RVA: 0x0039FFFF File Offset: 0x0039E1FF
	public void Clear()
	{
		this.pos = Vector3.zero;
		this.blockPos = Vector3i.zero;
		this.blockFace = BlockFace.Top;
		this.voxelData.Clear();
		this.propData.Clear();
		this.distanceSq = 0f;
	}

	// Token: 0x0600990A RID: 39178 RVA: 0x003A0040 File Offset: 0x0039E240
	public void CopyFrom(HitInfoDetails _other)
	{
		this.pos = _other.pos;
		this.blockPos = _other.blockPos;
		this.blockFace = _other.blockFace;
		this.voxelData = _other.voxelData;
		this.propData = _other.propData;
		this.distanceSq = _other.distanceSq;
	}

	// Token: 0x0600990B RID: 39179 RVA: 0x003A0098 File Offset: 0x0039E298
	public HitInfoDetails Clone()
	{
		HitInfoDetails hitInfoDetails = default(HitInfoDetails);
		hitInfoDetails.CopyFrom(this);
		return hitInfoDetails;
	}

	// Token: 0x04007344 RID: 29508
	public Vector3 pos;

	// Token: 0x04007345 RID: 29509
	public Vector3i blockPos;

	// Token: 0x04007346 RID: 29510
	public HitInfoDetails.VoxelData voxelData;

	// Token: 0x04007347 RID: 29511
	public HitInfoDetails.PropData propData;

	// Token: 0x04007348 RID: 29512
	public BlockFace blockFace;

	// Token: 0x04007349 RID: 29513
	public float distanceSq;

	// Token: 0x020012E8 RID: 4840
	public struct VoxelData : IEquatable<HitInfoDetails.VoxelData>
	{
		// Token: 0x0600990C RID: 39180 RVA: 0x003A00BB File Offset: 0x0039E2BB
		public void Set(BlockValue _bv, WaterValue _wv)
		{
			this.BlockValue = _bv;
			this.WaterValue = _wv;
		}

		// Token: 0x0600990D RID: 39181 RVA: 0x003A00CC File Offset: 0x0039E2CC
		public static HitInfoDetails.VoxelData GetFrom(ChunkCluster _cc, Vector3i _blockPos)
		{
			return new HitInfoDetails.VoxelData
			{
				BlockValue = _cc.GetBlock(_blockPos),
				WaterValue = _cc.GetWater(_blockPos)
			};
		}

		// Token: 0x0600990E RID: 39182 RVA: 0x003A0100 File Offset: 0x0039E300
		public static HitInfoDetails.VoxelData GetFrom(World _world, Vector3i _blockPos)
		{
			return new HitInfoDetails.VoxelData
			{
				BlockValue = _world.GetBlock(_blockPos),
				WaterValue = _world.GetWater(_blockPos)
			};
		}

		// Token: 0x0600990F RID: 39183 RVA: 0x003A0134 File Offset: 0x0039E334
		public static HitInfoDetails.VoxelData GetFrom(IChunk _chunk, int _x, int _y, int _z)
		{
			return new HitInfoDetails.VoxelData
			{
				BlockValue = _chunk.GetBlock(_x, _y, _z),
				WaterValue = _chunk.GetWater(_x, _y, _z)
			};
		}

		// Token: 0x06009910 RID: 39184 RVA: 0x003A016A File Offset: 0x0039E36A
		public bool IsOnlyAir()
		{
			return this.BlockValue.isair && !this.WaterValue.HasMass();
		}

		// Token: 0x06009911 RID: 39185 RVA: 0x003A0189 File Offset: 0x0039E389
		public bool IsOnlyWater()
		{
			return this.BlockValue.isair && this.WaterValue.HasMass();
		}

		// Token: 0x06009912 RID: 39186 RVA: 0x003A01A5 File Offset: 0x0039E3A5
		public bool Equals(HitInfoDetails.VoxelData _other)
		{
			return this.BlockValue.Equals(_other.BlockValue) && this.WaterValue.HasMass() == _other.WaterValue.HasMass();
		}

		// Token: 0x06009913 RID: 39187 RVA: 0x003A01D5 File Offset: 0x0039E3D5
		public void Clear()
		{
			this.BlockValue = BlockValue.Air;
			this.WaterValue = WaterValue.Empty;
		}

		// Token: 0x0400734A RID: 29514
		public BlockValue BlockValue;

		// Token: 0x0400734B RID: 29515
		public WaterValue WaterValue;
	}

	// Token: 0x020012E9 RID: 4841
	public struct PropData : IEquatable<HitInfoDetails.PropData>
	{
		// Token: 0x17001201 RID: 4609
		// (get) Token: 0x06009914 RID: 39188 RVA: 0x003A01ED File Offset: 0x0039E3ED
		// (set) Token: 0x06009915 RID: 39189 RVA: 0x003A01F5 File Offset: 0x0039E3F5
		public PropRef PropRef
		{
			readonly get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001202 RID: 4610
		// (get) Token: 0x06009916 RID: 39190 RVA: 0x003A01FE File Offset: 0x0039E3FE
		// (set) Token: 0x06009917 RID: 39191 RVA: 0x003A0206 File Offset: 0x0039E406
		public PropValue PropValue
		{
			readonly get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x06009918 RID: 39192 RVA: 0x003A0210 File Offset: 0x0039E410
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

		// Token: 0x06009919 RID: 39193 RVA: 0x003A026C File Offset: 0x0039E46C
		public override string ToString()
		{
			return string.Format("PropRef=[{0}], PropValue=[{1}]", this.PropRef, this.PropValue);
		}

		// Token: 0x0600991A RID: 39194 RVA: 0x003A028E File Offset: 0x0039E48E
		public void Clear()
		{
			this.PropRef = PropRef.Default;
			this.PropValue = HitInfoDetails.PropData.Empty.PropValue;
		}

		// Token: 0x0600991B RID: 39195 RVA: 0x003A02AC File Offset: 0x0039E4AC
		public bool Equals(HitInfoDetails.PropData other)
		{
			return this.PropRef.Equals(other.PropRef) && this.PropValue.Equals(other.PropValue);
		}

		// Token: 0x0600991C RID: 39196 RVA: 0x003A02E8 File Offset: 0x0039E4E8
		public override bool Equals(object obj)
		{
			if (obj is HitInfoDetails.PropData)
			{
				HitInfoDetails.PropData propData = (HitInfoDetails.PropData)obj;
				return this.Equals(propData);
			}
			return false;
		}

		// Token: 0x0600991D RID: 39197 RVA: 0x003A030D File Offset: 0x0039E50D
		public override int GetHashCode()
		{
			return HashCode.Combine<PropRef, PropValue>(this.PropRef, this.PropValue);
		}

		// Token: 0x0600991E RID: 39198 RVA: 0x003A0320 File Offset: 0x0039E520
		public static bool operator ==(HitInfoDetails.PropData left, HitInfoDetails.PropData right)
		{
			return left.Equals(right);
		}

		// Token: 0x0600991F RID: 39199 RVA: 0x003A032A File Offset: 0x0039E52A
		public static bool operator !=(HitInfoDetails.PropData left, HitInfoDetails.PropData right)
		{
			return !left.Equals(right);
		}

		// Token: 0x0400734C RID: 29516
		public static readonly HitInfoDetails.PropData Empty = new HitInfoDetails.PropData
		{
			PropRef = PropRef.Default,
			PropValue = PropValue.AIR
		};
	}
}
