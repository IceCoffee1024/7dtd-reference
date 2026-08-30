using System;

namespace UnityEngine.Rendering
{
	// Token: 0x02000457 RID: 1111
	public struct DepthState : IEquatable<DepthState>
	{
		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x06002541 RID: 9537 RVA: 0x0003F5BC File Offset: 0x0003D7BC
		public static DepthState defaultValue
		{
			get
			{
				return new DepthState(true, CompareFunction.Less);
			}
		}

		// Token: 0x06002542 RID: 9538 RVA: 0x0003F5D5 File Offset: 0x0003D7D5
		public DepthState(bool writeEnabled = true, CompareFunction compareFunction = CompareFunction.Less)
		{
			this.m_WriteEnabled = Convert.ToByte(writeEnabled);
			this.m_CompareFunction = (sbyte)compareFunction;
		}

		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x06002543 RID: 9539 RVA: 0x0003F5EC File Offset: 0x0003D7EC
		// (set) Token: 0x06002544 RID: 9540 RVA: 0x0003F609 File Offset: 0x0003D809
		public bool writeEnabled
		{
			get
			{
				return Convert.ToBoolean(this.m_WriteEnabled);
			}
			set
			{
				this.m_WriteEnabled = Convert.ToByte(value);
			}
		}

		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x06002545 RID: 9541 RVA: 0x0003F618 File Offset: 0x0003D818
		// (set) Token: 0x06002546 RID: 9542 RVA: 0x0003F630 File Offset: 0x0003D830
		public CompareFunction compareFunction
		{
			get
			{
				return (CompareFunction)this.m_CompareFunction;
			}
			set
			{
				this.m_CompareFunction = (sbyte)value;
			}
		}

		// Token: 0x06002547 RID: 9543 RVA: 0x0003F63C File Offset: 0x0003D83C
		public bool Equals(DepthState other)
		{
			return this.m_WriteEnabled == other.m_WriteEnabled && this.m_CompareFunction == other.m_CompareFunction;
		}

		// Token: 0x06002548 RID: 9544 RVA: 0x0003F670 File Offset: 0x0003D870
		public override bool Equals(object obj)
		{
			bool flag = obj == null;
			return !flag && obj is DepthState && this.Equals((DepthState)obj);
		}

		// Token: 0x06002549 RID: 9545 RVA: 0x0003F6A8 File Offset: 0x0003D8A8
		public override int GetHashCode()
		{
			return (this.m_WriteEnabled.GetHashCode() * 397) ^ this.m_CompareFunction.GetHashCode();
		}

		// Token: 0x0600254A RID: 9546 RVA: 0x0003F6D8 File Offset: 0x0003D8D8
		public static bool operator ==(DepthState left, DepthState right)
		{
			return left.Equals(right);
		}

		// Token: 0x0600254B RID: 9547 RVA: 0x0003F6F4 File Offset: 0x0003D8F4
		public static bool operator !=(DepthState left, DepthState right)
		{
			return !left.Equals(right);
		}

		// Token: 0x04000E11 RID: 3601
		private byte m_WriteEnabled;

		// Token: 0x04000E12 RID: 3602
		private sbyte m_CompareFunction;
	}
}
