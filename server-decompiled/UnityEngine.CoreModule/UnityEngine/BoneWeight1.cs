using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x020001CC RID: 460
	[UsedByNativeCode]
	[Serializable]
	public struct BoneWeight1 : IEquatable<BoneWeight1>
	{
		// Token: 0x1700039E RID: 926
		// (get) Token: 0x06001208 RID: 4616 RVA: 0x00019254 File Offset: 0x00017454
		// (set) Token: 0x06001209 RID: 4617 RVA: 0x0001926C File Offset: 0x0001746C
		public float weight
		{
			get
			{
				return this.m_Weight;
			}
			set
			{
				this.m_Weight = value;
			}
		}

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x0600120A RID: 4618 RVA: 0x00019278 File Offset: 0x00017478
		// (set) Token: 0x0600120B RID: 4619 RVA: 0x00019290 File Offset: 0x00017490
		public int boneIndex
		{
			get
			{
				return this.m_BoneIndex;
			}
			set
			{
				this.m_BoneIndex = value;
			}
		}

		// Token: 0x0600120C RID: 4620 RVA: 0x0001929C File Offset: 0x0001749C
		public override bool Equals(object other)
		{
			return other is BoneWeight1 && this.Equals((BoneWeight1)other);
		}

		// Token: 0x0600120D RID: 4621 RVA: 0x000192C8 File Offset: 0x000174C8
		public bool Equals(BoneWeight1 other)
		{
			return this.boneIndex.Equals(other.boneIndex) && this.weight.Equals(other.weight);
		}

		// Token: 0x0600120E RID: 4622 RVA: 0x0001930C File Offset: 0x0001750C
		public override int GetHashCode()
		{
			return this.boneIndex.GetHashCode() ^ this.weight.GetHashCode();
		}

		// Token: 0x0600120F RID: 4623 RVA: 0x0001933C File Offset: 0x0001753C
		public static bool operator ==(BoneWeight1 lhs, BoneWeight1 rhs)
		{
			return lhs.boneIndex == rhs.boneIndex && lhs.weight == rhs.weight;
		}

		// Token: 0x06001210 RID: 4624 RVA: 0x00019374 File Offset: 0x00017574
		public static bool operator !=(BoneWeight1 lhs, BoneWeight1 rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x0400064E RID: 1614
		[SerializeField]
		private float m_Weight;

		// Token: 0x0400064F RID: 1615
		[SerializeField]
		private int m_BoneIndex;
	}
}
