using System;

namespace UnityEngine.Rendering
{
	// Token: 0x0200043D RID: 1085
	public struct BatchPackedCullingViewID : IEquatable<BatchPackedCullingViewID>
	{
		// Token: 0x06002456 RID: 9302 RVA: 0x0003D0F4 File Offset: 0x0003B2F4
		public override int GetHashCode()
		{
			return this.handle.GetHashCode();
		}

		// Token: 0x06002457 RID: 9303 RVA: 0x0003D114 File Offset: 0x0003B314
		public bool Equals(BatchPackedCullingViewID other)
		{
			return this.handle == other.handle;
		}

		// Token: 0x06002458 RID: 9304 RVA: 0x0003D134 File Offset: 0x0003B334
		public override bool Equals(object obj)
		{
			bool flag = !(obj is BatchPackedCullingViewID);
			return !flag && this.Equals((BatchPackedCullingViewID)obj);
		}

		// Token: 0x06002459 RID: 9305 RVA: 0x0003D168 File Offset: 0x0003B368
		public static bool operator ==(BatchPackedCullingViewID lhs, BatchPackedCullingViewID rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x0600245A RID: 9306 RVA: 0x0003D184 File Offset: 0x0003B384
		public static bool operator !=(BatchPackedCullingViewID lhs, BatchPackedCullingViewID rhs)
		{
			return !lhs.Equals(rhs);
		}

		// Token: 0x0600245B RID: 9307 RVA: 0x0003D1A1 File Offset: 0x0003B3A1
		public BatchPackedCullingViewID(int instanceID, int sliceIndex)
		{
			this.handle = (ulong)instanceID | (ulong)((ulong)((long)sliceIndex) << 32);
		}

		// Token: 0x0600245C RID: 9308 RVA: 0x0003D1B4 File Offset: 0x0003B3B4
		public int GetInstanceID()
		{
			return (int)(this.handle & (ulong)(-1));
		}

		// Token: 0x0600245D RID: 9309 RVA: 0x0003D1D0 File Offset: 0x0003B3D0
		public int GetSliceIndex()
		{
			return (int)(this.handle >> 32);
		}

		// Token: 0x04000D5F RID: 3423
		internal ulong handle;
	}
}
