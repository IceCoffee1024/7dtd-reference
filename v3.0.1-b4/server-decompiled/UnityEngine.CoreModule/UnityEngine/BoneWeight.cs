using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x020001CB RID: 459
	[UsedByNativeCode]
	[Serializable]
	public struct BoneWeight : IEquatable<BoneWeight>
	{
		// Token: 0x17000396 RID: 918
		// (get) Token: 0x060011F3 RID: 4595 RVA: 0x00018EFC File Offset: 0x000170FC
		// (set) Token: 0x060011F4 RID: 4596 RVA: 0x00018F14 File Offset: 0x00017114
		public float weight0
		{
			get
			{
				return this.m_Weight0;
			}
			set
			{
				this.m_Weight0 = value;
			}
		}

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x060011F5 RID: 4597 RVA: 0x00018F20 File Offset: 0x00017120
		// (set) Token: 0x060011F6 RID: 4598 RVA: 0x00018F38 File Offset: 0x00017138
		public float weight1
		{
			get
			{
				return this.m_Weight1;
			}
			set
			{
				this.m_Weight1 = value;
			}
		}

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x060011F7 RID: 4599 RVA: 0x00018F44 File Offset: 0x00017144
		// (set) Token: 0x060011F8 RID: 4600 RVA: 0x00018F5C File Offset: 0x0001715C
		public float weight2
		{
			get
			{
				return this.m_Weight2;
			}
			set
			{
				this.m_Weight2 = value;
			}
		}

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x060011F9 RID: 4601 RVA: 0x00018F68 File Offset: 0x00017168
		// (set) Token: 0x060011FA RID: 4602 RVA: 0x00018F80 File Offset: 0x00017180
		public float weight3
		{
			get
			{
				return this.m_Weight3;
			}
			set
			{
				this.m_Weight3 = value;
			}
		}

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x060011FB RID: 4603 RVA: 0x00018F8C File Offset: 0x0001718C
		// (set) Token: 0x060011FC RID: 4604 RVA: 0x00018FA4 File Offset: 0x000171A4
		public int boneIndex0
		{
			get
			{
				return this.m_BoneIndex0;
			}
			set
			{
				this.m_BoneIndex0 = value;
			}
		}

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x060011FD RID: 4605 RVA: 0x00018FB0 File Offset: 0x000171B0
		// (set) Token: 0x060011FE RID: 4606 RVA: 0x00018FC8 File Offset: 0x000171C8
		public int boneIndex1
		{
			get
			{
				return this.m_BoneIndex1;
			}
			set
			{
				this.m_BoneIndex1 = value;
			}
		}

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x060011FF RID: 4607 RVA: 0x00018FD4 File Offset: 0x000171D4
		// (set) Token: 0x06001200 RID: 4608 RVA: 0x00018FEC File Offset: 0x000171EC
		public int boneIndex2
		{
			get
			{
				return this.m_BoneIndex2;
			}
			set
			{
				this.m_BoneIndex2 = value;
			}
		}

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x06001201 RID: 4609 RVA: 0x00018FF8 File Offset: 0x000171F8
		// (set) Token: 0x06001202 RID: 4610 RVA: 0x00019010 File Offset: 0x00017210
		public int boneIndex3
		{
			get
			{
				return this.m_BoneIndex3;
			}
			set
			{
				this.m_BoneIndex3 = value;
			}
		}

		// Token: 0x06001203 RID: 4611 RVA: 0x0001901C File Offset: 0x0001721C
		public override int GetHashCode()
		{
			return this.boneIndex0.GetHashCode() ^ (this.boneIndex1.GetHashCode() << 2) ^ (this.boneIndex2.GetHashCode() >> 2) ^ (this.boneIndex3.GetHashCode() >> 1) ^ (this.weight0.GetHashCode() << 5) ^ (this.weight1.GetHashCode() << 4) ^ (this.weight2.GetHashCode() >> 4) ^ (this.weight3.GetHashCode() >> 3);
		}

		// Token: 0x06001204 RID: 4612 RVA: 0x000190B4 File Offset: 0x000172B4
		public override bool Equals(object other)
		{
			return other is BoneWeight && this.Equals((BoneWeight)other);
		}

		// Token: 0x06001205 RID: 4613 RVA: 0x000190E0 File Offset: 0x000172E0
		public bool Equals(BoneWeight other)
		{
			return this.boneIndex0.Equals(other.boneIndex0) && this.boneIndex1.Equals(other.boneIndex1) && this.boneIndex2.Equals(other.boneIndex2) && this.boneIndex3.Equals(other.boneIndex3) && new Vector4(this.weight0, this.weight1, this.weight2, this.weight3).Equals(new Vector4(other.weight0, other.weight1, other.weight2, other.weight3));
		}

		// Token: 0x06001206 RID: 4614 RVA: 0x0001919C File Offset: 0x0001739C
		public static bool operator ==(BoneWeight lhs, BoneWeight rhs)
		{
			return lhs.boneIndex0 == rhs.boneIndex0 && lhs.boneIndex1 == rhs.boneIndex1 && lhs.boneIndex2 == rhs.boneIndex2 && lhs.boneIndex3 == rhs.boneIndex3 && new Vector4(lhs.weight0, lhs.weight1, lhs.weight2, lhs.weight3) == new Vector4(rhs.weight0, rhs.weight1, rhs.weight2, rhs.weight3);
		}

		// Token: 0x06001207 RID: 4615 RVA: 0x00019238 File Offset: 0x00017438
		public static bool operator !=(BoneWeight lhs, BoneWeight rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x04000646 RID: 1606
		[SerializeField]
		private float m_Weight0;

		// Token: 0x04000647 RID: 1607
		[SerializeField]
		private float m_Weight1;

		// Token: 0x04000648 RID: 1608
		[SerializeField]
		private float m_Weight2;

		// Token: 0x04000649 RID: 1609
		[SerializeField]
		private float m_Weight3;

		// Token: 0x0400064A RID: 1610
		[SerializeField]
		private int m_BoneIndex0;

		// Token: 0x0400064B RID: 1611
		[SerializeField]
		private int m_BoneIndex1;

		// Token: 0x0400064C RID: 1612
		[SerializeField]
		private int m_BoneIndex2;

		// Token: 0x0400064D RID: 1613
		[SerializeField]
		private int m_BoneIndex3;
	}
}
