using System;

namespace SharpEXR.AttributeTypes
{
	// Token: 0x020016EA RID: 5866
	public struct V2I
	{
		// Token: 0x0600B732 RID: 46898 RVA: 0x0043F186 File Offset: 0x0043D386
		public V2I(int v0, int v1)
		{
			this.V0 = v0;
			this.V1 = v1;
		}

		// Token: 0x0600B733 RID: 46899 RVA: 0x0043F196 File Offset: 0x0043D396
		public override string ToString()
		{
			return string.Format("{0}: {1}, {2}", base.GetType().Name, this.V0, this.V1);
		}

		// Token: 0x1700165C RID: 5724
		// (get) Token: 0x0600B734 RID: 46900 RVA: 0x0043F1CD File Offset: 0x0043D3CD
		public int X
		{
			get
			{
				return this.V0;
			}
		}

		// Token: 0x1700165D RID: 5725
		// (get) Token: 0x0600B735 RID: 46901 RVA: 0x0043F1D5 File Offset: 0x0043D3D5
		public int Y
		{
			get
			{
				return this.V1;
			}
		}

		// Token: 0x040088E7 RID: 35047
		public int V0;

		// Token: 0x040088E8 RID: 35048
		public int V1;
	}
}
