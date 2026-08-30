using System;

namespace UnityEngine.Rendering
{
	// Token: 0x0200046E RID: 1134
	public struct RenderTargetBlendState : IEquatable<RenderTargetBlendState>
	{
		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x0600262C RID: 9772 RVA: 0x000417EC File Offset: 0x0003F9EC
		public static RenderTargetBlendState defaultValue
		{
			get
			{
				return new RenderTargetBlendState(ColorWriteMask.All, BlendMode.One, BlendMode.Zero, BlendMode.One, BlendMode.Zero, BlendOp.Add, BlendOp.Add);
			}
		}

		// Token: 0x0600262D RID: 9773 RVA: 0x0004180C File Offset: 0x0003FA0C
		public RenderTargetBlendState(ColorWriteMask writeMask = ColorWriteMask.All, BlendMode sourceColorBlendMode = BlendMode.One, BlendMode destinationColorBlendMode = BlendMode.Zero, BlendMode sourceAlphaBlendMode = BlendMode.One, BlendMode destinationAlphaBlendMode = BlendMode.Zero, BlendOp colorBlendOperation = BlendOp.Add, BlendOp alphaBlendOperation = BlendOp.Add)
		{
			this.m_WriteMask = (byte)writeMask;
			this.m_SourceColorBlendMode = (byte)sourceColorBlendMode;
			this.m_DestinationColorBlendMode = (byte)destinationColorBlendMode;
			this.m_SourceAlphaBlendMode = (byte)sourceAlphaBlendMode;
			this.m_DestinationAlphaBlendMode = (byte)destinationAlphaBlendMode;
			this.m_ColorBlendOperation = (byte)colorBlendOperation;
			this.m_AlphaBlendOperation = (byte)alphaBlendOperation;
			this.m_Padding = 0;
		}

		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x0600262E RID: 9774 RVA: 0x00041860 File Offset: 0x0003FA60
		// (set) Token: 0x0600262F RID: 9775 RVA: 0x00041878 File Offset: 0x0003FA78
		public ColorWriteMask writeMask
		{
			get
			{
				return (ColorWriteMask)this.m_WriteMask;
			}
			set
			{
				this.m_WriteMask = (byte)value;
			}
		}

		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x06002630 RID: 9776 RVA: 0x00041884 File Offset: 0x0003FA84
		// (set) Token: 0x06002631 RID: 9777 RVA: 0x0004189C File Offset: 0x0003FA9C
		public BlendMode sourceColorBlendMode
		{
			get
			{
				return (BlendMode)this.m_SourceColorBlendMode;
			}
			set
			{
				this.m_SourceColorBlendMode = (byte)value;
			}
		}

		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x06002632 RID: 9778 RVA: 0x000418A8 File Offset: 0x0003FAA8
		// (set) Token: 0x06002633 RID: 9779 RVA: 0x000418C0 File Offset: 0x0003FAC0
		public BlendMode destinationColorBlendMode
		{
			get
			{
				return (BlendMode)this.m_DestinationColorBlendMode;
			}
			set
			{
				this.m_DestinationColorBlendMode = (byte)value;
			}
		}

		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x06002634 RID: 9780 RVA: 0x000418CC File Offset: 0x0003FACC
		// (set) Token: 0x06002635 RID: 9781 RVA: 0x000418E4 File Offset: 0x0003FAE4
		public BlendMode sourceAlphaBlendMode
		{
			get
			{
				return (BlendMode)this.m_SourceAlphaBlendMode;
			}
			set
			{
				this.m_SourceAlphaBlendMode = (byte)value;
			}
		}

		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x06002636 RID: 9782 RVA: 0x000418F0 File Offset: 0x0003FAF0
		// (set) Token: 0x06002637 RID: 9783 RVA: 0x00041908 File Offset: 0x0003FB08
		public BlendMode destinationAlphaBlendMode
		{
			get
			{
				return (BlendMode)this.m_DestinationAlphaBlendMode;
			}
			set
			{
				this.m_DestinationAlphaBlendMode = (byte)value;
			}
		}

		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x06002638 RID: 9784 RVA: 0x00041914 File Offset: 0x0003FB14
		// (set) Token: 0x06002639 RID: 9785 RVA: 0x0004192C File Offset: 0x0003FB2C
		public BlendOp colorBlendOperation
		{
			get
			{
				return (BlendOp)this.m_ColorBlendOperation;
			}
			set
			{
				this.m_ColorBlendOperation = (byte)value;
			}
		}

		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x0600263A RID: 9786 RVA: 0x00041938 File Offset: 0x0003FB38
		// (set) Token: 0x0600263B RID: 9787 RVA: 0x00041950 File Offset: 0x0003FB50
		public BlendOp alphaBlendOperation
		{
			get
			{
				return (BlendOp)this.m_AlphaBlendOperation;
			}
			set
			{
				this.m_AlphaBlendOperation = (byte)value;
			}
		}

		// Token: 0x0600263C RID: 9788 RVA: 0x0004195C File Offset: 0x0003FB5C
		public bool Equals(RenderTargetBlendState other)
		{
			return this.m_WriteMask == other.m_WriteMask && this.m_SourceColorBlendMode == other.m_SourceColorBlendMode && this.m_DestinationColorBlendMode == other.m_DestinationColorBlendMode && this.m_SourceAlphaBlendMode == other.m_SourceAlphaBlendMode && this.m_DestinationAlphaBlendMode == other.m_DestinationAlphaBlendMode && this.m_ColorBlendOperation == other.m_ColorBlendOperation && this.m_AlphaBlendOperation == other.m_AlphaBlendOperation;
		}

		// Token: 0x0600263D RID: 9789 RVA: 0x000419D4 File Offset: 0x0003FBD4
		public override bool Equals(object obj)
		{
			bool flag = obj == null;
			return !flag && obj is RenderTargetBlendState && this.Equals((RenderTargetBlendState)obj);
		}

		// Token: 0x0600263E RID: 9790 RVA: 0x00041A0C File Offset: 0x0003FC0C
		public override int GetHashCode()
		{
			int num = this.m_WriteMask.GetHashCode();
			num = (num * 397) ^ this.m_SourceColorBlendMode.GetHashCode();
			num = (num * 397) ^ this.m_DestinationColorBlendMode.GetHashCode();
			num = (num * 397) ^ this.m_SourceAlphaBlendMode.GetHashCode();
			num = (num * 397) ^ this.m_DestinationAlphaBlendMode.GetHashCode();
			num = (num * 397) ^ this.m_ColorBlendOperation.GetHashCode();
			return (num * 397) ^ this.m_AlphaBlendOperation.GetHashCode();
		}

		// Token: 0x0600263F RID: 9791 RVA: 0x00041AA4 File Offset: 0x0003FCA4
		public static bool operator ==(RenderTargetBlendState left, RenderTargetBlendState right)
		{
			return left.Equals(right);
		}

		// Token: 0x06002640 RID: 9792 RVA: 0x00041AC0 File Offset: 0x0003FCC0
		public static bool operator !=(RenderTargetBlendState left, RenderTargetBlendState right)
		{
			return !left.Equals(right);
		}

		// Token: 0x04000E8B RID: 3723
		private byte m_WriteMask;

		// Token: 0x04000E8C RID: 3724
		private byte m_SourceColorBlendMode;

		// Token: 0x04000E8D RID: 3725
		private byte m_DestinationColorBlendMode;

		// Token: 0x04000E8E RID: 3726
		private byte m_SourceAlphaBlendMode;

		// Token: 0x04000E8F RID: 3727
		private byte m_DestinationAlphaBlendMode;

		// Token: 0x04000E90 RID: 3728
		private byte m_ColorBlendOperation;

		// Token: 0x04000E91 RID: 3729
		private byte m_AlphaBlendOperation;

		// Token: 0x04000E92 RID: 3730
		private byte m_Padding;
	}
}
