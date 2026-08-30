using System;

namespace SharpEXR.AttributeTypes
{
	// Token: 0x020016C4 RID: 5828
	public struct V2I
	{
		// Token: 0x0600B5AF RID: 46511 RVA: 0x0043940E File Offset: 0x0043760E
		public V2I(int v0, int v1)
		{
			this.V0 = v0;
			this.V1 = v1;
		}

		// Token: 0x0600B5B0 RID: 46512 RVA: 0x0043941E File Offset: 0x0043761E
		public override string ToString()
		{
			return string.Format("{0}: {1}, {2}", base.GetType().Name, this.V0, this.V1);
		}

		// Token: 0x170015EA RID: 5610
		// (get) Token: 0x0600B5B1 RID: 46513 RVA: 0x00439455 File Offset: 0x00437655
		public int X
		{
			get
			{
				return this.V0;
			}
		}

		// Token: 0x170015EB RID: 5611
		// (get) Token: 0x0600B5B2 RID: 46514 RVA: 0x0043945D File Offset: 0x0043765D
		public int Y
		{
			get
			{
				return this.V1;
			}
		}

		// Token: 0x040087CB RID: 34763
		public int V0;

		// Token: 0x040087CC RID: 34764
		public int V1;
	}
}
