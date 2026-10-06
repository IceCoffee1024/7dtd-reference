using System;

namespace SharpEXR.AttributeTypes
{
	// Token: 0x02001718 RID: 5912
	public struct V3I
	{
		// Token: 0x0600B892 RID: 47250 RVA: 0x0044378B File Offset: 0x0044198B
		public V3I(int v0, int v1, int v2)
		{
			this.V0 = v0;
			this.V1 = v1;
			this.V2 = v2;
		}

		// Token: 0x0600B893 RID: 47251 RVA: 0x004437A4 File Offset: 0x004419A4
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

		// Token: 0x17001703 RID: 5891
		// (get) Token: 0x0600B894 RID: 47252 RVA: 0x00443803 File Offset: 0x00441A03
		public int X
		{
			get
			{
				return this.V0;
			}
		}

		// Token: 0x17001704 RID: 5892
		// (get) Token: 0x0600B895 RID: 47253 RVA: 0x0044380B File Offset: 0x00441A0B
		public int Y
		{
			get
			{
				return this.V1;
			}
		}

		// Token: 0x17001705 RID: 5893
		// (get) Token: 0x0600B896 RID: 47254 RVA: 0x00443813 File Offset: 0x00441A13
		public int Z
		{
			get
			{
				return this.V2;
			}
		}

		// Token: 0x040089B1 RID: 35249
		public int V0;

		// Token: 0x040089B2 RID: 35250
		public int V1;

		// Token: 0x040089B3 RID: 35251
		public int V2;
	}
}
