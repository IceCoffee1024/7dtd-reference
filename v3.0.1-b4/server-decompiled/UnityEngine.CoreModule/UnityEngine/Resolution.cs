using System;
using System.ComponentModel;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000156 RID: 342
	[RequiredByNativeCode]
	public struct Resolution
	{
		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000AD3 RID: 2771 RVA: 0x0001178C File Offset: 0x0000F98C
		// (set) Token: 0x06000AD4 RID: 2772 RVA: 0x000117A4 File Offset: 0x0000F9A4
		public int width
		{
			get
			{
				return this.m_Width;
			}
			set
			{
				this.m_Width = value;
			}
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000AD5 RID: 2773 RVA: 0x000117B0 File Offset: 0x0000F9B0
		// (set) Token: 0x06000AD6 RID: 2774 RVA: 0x000117C8 File Offset: 0x0000F9C8
		public int height
		{
			get
			{
				return this.m_Height;
			}
			set
			{
				this.m_Height = value;
			}
		}

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000AD7 RID: 2775 RVA: 0x000117D4 File Offset: 0x0000F9D4
		// (set) Token: 0x06000AD8 RID: 2776 RVA: 0x000117EC File Offset: 0x0000F9EC
		public RefreshRate refreshRateRatio
		{
			get
			{
				return this.m_RefreshRate;
			}
			set
			{
				this.m_RefreshRate = value;
			}
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000AD9 RID: 2777 RVA: 0x000117F8 File Offset: 0x0000F9F8
		// (set) Token: 0x06000ADA RID: 2778 RVA: 0x0001181B File Offset: 0x0000FA1B
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("Resolution.refreshRate is obsolete. Use refreshRateRatio instead.", false)]
		public int refreshRate
		{
			get
			{
				return (int)Math.Round(this.m_RefreshRate.value);
			}
			set
			{
				this.m_RefreshRate.numerator = (uint)value;
				this.m_RefreshRate.denominator = 1U;
			}
		}

		// Token: 0x06000ADB RID: 2779 RVA: 0x00011838 File Offset: 0x0000FA38
		public override string ToString()
		{
			return UnityString.Format("{0} x {1} @ {2}Hz", new object[] { this.m_Width, this.m_Height, this.m_RefreshRate });
		}

		// Token: 0x04000447 RID: 1095
		private int m_Width;

		// Token: 0x04000448 RID: 1096
		private int m_Height;

		// Token: 0x04000449 RID: 1097
		private RefreshRate m_RefreshRate;
	}
}
