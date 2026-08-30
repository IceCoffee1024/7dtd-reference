using System;

namespace UnityEngine.Rendering
{
	// Token: 0x0200044B RID: 1099
	public struct BlendState : IEquatable<BlendState>
	{
		// Token: 0x17000695 RID: 1685
		// (get) Token: 0x060024C0 RID: 9408 RVA: 0x0003DAAC File Offset: 0x0003BCAC
		public static BlendState defaultValue
		{
			get
			{
				return new BlendState(false, false);
			}
		}

		// Token: 0x060024C1 RID: 9409 RVA: 0x0003DAC8 File Offset: 0x0003BCC8
		public BlendState(bool separateMRTBlend = false, bool alphaToMask = false)
		{
			this.m_BlendState0 = RenderTargetBlendState.defaultValue;
			this.m_BlendState1 = RenderTargetBlendState.defaultValue;
			this.m_BlendState2 = RenderTargetBlendState.defaultValue;
			this.m_BlendState3 = RenderTargetBlendState.defaultValue;
			this.m_BlendState4 = RenderTargetBlendState.defaultValue;
			this.m_BlendState5 = RenderTargetBlendState.defaultValue;
			this.m_BlendState6 = RenderTargetBlendState.defaultValue;
			this.m_BlendState7 = RenderTargetBlendState.defaultValue;
			this.m_SeparateMRTBlendStates = Convert.ToByte(separateMRTBlend);
			this.m_AlphaToMask = Convert.ToByte(alphaToMask);
			this.m_Padding = 0;
		}

		// Token: 0x17000696 RID: 1686
		// (get) Token: 0x060024C2 RID: 9410 RVA: 0x0003DB50 File Offset: 0x0003BD50
		// (set) Token: 0x060024C3 RID: 9411 RVA: 0x0003DB6D File Offset: 0x0003BD6D
		public bool separateMRTBlendStates
		{
			get
			{
				return Convert.ToBoolean(this.m_SeparateMRTBlendStates);
			}
			set
			{
				this.m_SeparateMRTBlendStates = Convert.ToByte(value);
			}
		}

		// Token: 0x17000697 RID: 1687
		// (get) Token: 0x060024C4 RID: 9412 RVA: 0x0003DB7C File Offset: 0x0003BD7C
		// (set) Token: 0x060024C5 RID: 9413 RVA: 0x0003DB99 File Offset: 0x0003BD99
		public bool alphaToMask
		{
			get
			{
				return Convert.ToBoolean(this.m_AlphaToMask);
			}
			set
			{
				this.m_AlphaToMask = Convert.ToByte(value);
			}
		}

		// Token: 0x17000698 RID: 1688
		// (get) Token: 0x060024C6 RID: 9414 RVA: 0x0003DBA8 File Offset: 0x0003BDA8
		// (set) Token: 0x060024C7 RID: 9415 RVA: 0x0003DBC0 File Offset: 0x0003BDC0
		public RenderTargetBlendState blendState0
		{
			get
			{
				return this.m_BlendState0;
			}
			set
			{
				this.m_BlendState0 = value;
			}
		}

		// Token: 0x17000699 RID: 1689
		// (get) Token: 0x060024C8 RID: 9416 RVA: 0x0003DBCC File Offset: 0x0003BDCC
		// (set) Token: 0x060024C9 RID: 9417 RVA: 0x0003DBE4 File Offset: 0x0003BDE4
		public RenderTargetBlendState blendState1
		{
			get
			{
				return this.m_BlendState1;
			}
			set
			{
				this.m_BlendState1 = value;
			}
		}

		// Token: 0x1700069A RID: 1690
		// (get) Token: 0x060024CA RID: 9418 RVA: 0x0003DBF0 File Offset: 0x0003BDF0
		// (set) Token: 0x060024CB RID: 9419 RVA: 0x0003DC08 File Offset: 0x0003BE08
		public RenderTargetBlendState blendState2
		{
			get
			{
				return this.m_BlendState2;
			}
			set
			{
				this.m_BlendState2 = value;
			}
		}

		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x060024CC RID: 9420 RVA: 0x0003DC14 File Offset: 0x0003BE14
		// (set) Token: 0x060024CD RID: 9421 RVA: 0x0003DC2C File Offset: 0x0003BE2C
		public RenderTargetBlendState blendState3
		{
			get
			{
				return this.m_BlendState3;
			}
			set
			{
				this.m_BlendState3 = value;
			}
		}

		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x060024CE RID: 9422 RVA: 0x0003DC38 File Offset: 0x0003BE38
		// (set) Token: 0x060024CF RID: 9423 RVA: 0x0003DC50 File Offset: 0x0003BE50
		public RenderTargetBlendState blendState4
		{
			get
			{
				return this.m_BlendState4;
			}
			set
			{
				this.m_BlendState4 = value;
			}
		}

		// Token: 0x1700069D RID: 1693
		// (get) Token: 0x060024D0 RID: 9424 RVA: 0x0003DC5C File Offset: 0x0003BE5C
		// (set) Token: 0x060024D1 RID: 9425 RVA: 0x0003DC74 File Offset: 0x0003BE74
		public RenderTargetBlendState blendState5
		{
			get
			{
				return this.m_BlendState5;
			}
			set
			{
				this.m_BlendState5 = value;
			}
		}

		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x060024D2 RID: 9426 RVA: 0x0003DC80 File Offset: 0x0003BE80
		// (set) Token: 0x060024D3 RID: 9427 RVA: 0x0003DC98 File Offset: 0x0003BE98
		public RenderTargetBlendState blendState6
		{
			get
			{
				return this.m_BlendState6;
			}
			set
			{
				this.m_BlendState6 = value;
			}
		}

		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x060024D4 RID: 9428 RVA: 0x0003DCA4 File Offset: 0x0003BEA4
		// (set) Token: 0x060024D5 RID: 9429 RVA: 0x0003DCBC File Offset: 0x0003BEBC
		public RenderTargetBlendState blendState7
		{
			get
			{
				return this.m_BlendState7;
			}
			set
			{
				this.m_BlendState7 = value;
			}
		}

		// Token: 0x060024D6 RID: 9430 RVA: 0x0003DCC8 File Offset: 0x0003BEC8
		public bool Equals(BlendState other)
		{
			return this.m_BlendState0.Equals(other.m_BlendState0) && this.m_BlendState1.Equals(other.m_BlendState1) && this.m_BlendState2.Equals(other.m_BlendState2) && this.m_BlendState3.Equals(other.m_BlendState3) && this.m_BlendState4.Equals(other.m_BlendState4) && this.m_BlendState5.Equals(other.m_BlendState5) && this.m_BlendState6.Equals(other.m_BlendState6) && this.m_BlendState7.Equals(other.m_BlendState7) && this.m_SeparateMRTBlendStates == other.m_SeparateMRTBlendStates && this.m_AlphaToMask == other.m_AlphaToMask;
		}

		// Token: 0x060024D7 RID: 9431 RVA: 0x0003DD98 File Offset: 0x0003BF98
		public override bool Equals(object obj)
		{
			bool flag = obj == null;
			return !flag && obj is BlendState && this.Equals((BlendState)obj);
		}

		// Token: 0x060024D8 RID: 9432 RVA: 0x0003DDD0 File Offset: 0x0003BFD0
		public override int GetHashCode()
		{
			int num = this.m_BlendState0.GetHashCode();
			num = (num * 397) ^ this.m_BlendState1.GetHashCode();
			num = (num * 397) ^ this.m_BlendState2.GetHashCode();
			num = (num * 397) ^ this.m_BlendState3.GetHashCode();
			num = (num * 397) ^ this.m_BlendState4.GetHashCode();
			num = (num * 397) ^ this.m_BlendState5.GetHashCode();
			num = (num * 397) ^ this.m_BlendState6.GetHashCode();
			num = (num * 397) ^ this.m_BlendState7.GetHashCode();
			num = (num * 397) ^ this.m_SeparateMRTBlendStates.GetHashCode();
			return (num * 397) ^ this.m_AlphaToMask.GetHashCode();
		}

		// Token: 0x060024D9 RID: 9433 RVA: 0x0003DED4 File Offset: 0x0003C0D4
		public static bool operator ==(BlendState left, BlendState right)
		{
			return left.Equals(right);
		}

		// Token: 0x060024DA RID: 9434 RVA: 0x0003DEF0 File Offset: 0x0003C0F0
		public static bool operator !=(BlendState left, BlendState right)
		{
			return !left.Equals(right);
		}

		// Token: 0x04000DAD RID: 3501
		private RenderTargetBlendState m_BlendState0;

		// Token: 0x04000DAE RID: 3502
		private RenderTargetBlendState m_BlendState1;

		// Token: 0x04000DAF RID: 3503
		private RenderTargetBlendState m_BlendState2;

		// Token: 0x04000DB0 RID: 3504
		private RenderTargetBlendState m_BlendState3;

		// Token: 0x04000DB1 RID: 3505
		private RenderTargetBlendState m_BlendState4;

		// Token: 0x04000DB2 RID: 3506
		private RenderTargetBlendState m_BlendState5;

		// Token: 0x04000DB3 RID: 3507
		private RenderTargetBlendState m_BlendState6;

		// Token: 0x04000DB4 RID: 3508
		private RenderTargetBlendState m_BlendState7;

		// Token: 0x04000DB5 RID: 3509
		private byte m_SeparateMRTBlendStates;

		// Token: 0x04000DB6 RID: 3510
		private byte m_AlphaToMask;

		// Token: 0x04000DB7 RID: 3511
		private short m_Padding;
	}
}
