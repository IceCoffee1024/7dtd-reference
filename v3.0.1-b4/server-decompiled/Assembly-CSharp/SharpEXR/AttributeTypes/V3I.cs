using System;

namespace SharpEXR.AttributeTypes
{
	// Token: 0x020016C6 RID: 5830
	public struct V3I
	{
		// Token: 0x0600B5B8 RID: 46520 RVA: 0x004394F3 File Offset: 0x004376F3
		public V3I(int v0, int v1, int v2)
		{
			this.V0 = v0;
			this.V1 = v1;
			this.V2 = v2;
		}

		// Token: 0x0600B5B9 RID: 46521 RVA: 0x0043950C File Offset: 0x0043770C
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

		// Token: 0x170015EF RID: 5615
		// (get) Token: 0x0600B5BA RID: 46522 RVA: 0x0043956B File Offset: 0x0043776B
		public int X
		{
			get
			{
				return this.V0;
			}
		}

		// Token: 0x170015F0 RID: 5616
		// (get) Token: 0x0600B5BB RID: 46523 RVA: 0x00439573 File Offset: 0x00437773
		public int Y
		{
			get
			{
				return this.V1;
			}
		}

		// Token: 0x170015F1 RID: 5617
		// (get) Token: 0x0600B5BC RID: 46524 RVA: 0x0043957B File Offset: 0x0043777B
		public int Z
		{
			get
			{
				return this.V2;
			}
		}

		// Token: 0x040087D0 RID: 34768
		public int V0;

		// Token: 0x040087D1 RID: 34769
		public int V1;

		// Token: 0x040087D2 RID: 34770
		public int V2;
	}
}
