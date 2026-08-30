using System;

namespace SharpEXR.AttributeTypes
{
	// Token: 0x020016EB RID: 5867
	public struct V3F
	{
		// Token: 0x0600B736 RID: 46902 RVA: 0x0043F1DD File Offset: 0x0043D3DD
		public V3F(float v0, float v1, float v2)
		{
			this.V0 = v0;
			this.V1 = v1;
			this.V2 = v2;
		}

		// Token: 0x0600B737 RID: 46903 RVA: 0x0043F1F4 File Offset: 0x0043D3F4
		public override string ToString()
		{
			return string.Format("{0}: {1}, {2}, {3}", new object[]
			{
				base.GetType().Name,
				this.V0,
				this.V1,
				this.V2
			});
		}

		// Token: 0x1700165E RID: 5726
		// (get) Token: 0x0600B738 RID: 46904 RVA: 0x0043F253 File Offset: 0x0043D453
		public float X
		{
			get
			{
				return this.V0;
			}
		}

		// Token: 0x1700165F RID: 5727
		// (get) Token: 0x0600B739 RID: 46905 RVA: 0x0043F25B File Offset: 0x0043D45B
		public float Y
		{
			get
			{
				return this.V1;
			}
		}

		// Token: 0x17001660 RID: 5728
		// (get) Token: 0x0600B73A RID: 46906 RVA: 0x0043F263 File Offset: 0x0043D463
		public float Z
		{
			get
			{
				return this.V2;
			}
		}

		// Token: 0x040088E9 RID: 35049
		public float V0;

		// Token: 0x040088EA RID: 35050
		public float V1;

		// Token: 0x040088EB RID: 35051
		public float V2;
	}
}
