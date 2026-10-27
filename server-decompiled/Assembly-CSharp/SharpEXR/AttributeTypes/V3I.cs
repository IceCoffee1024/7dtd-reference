using System;

namespace SharpEXR.AttributeTypes
{
	// Token: 0x02001737 RID: 5943
	public struct V3I
	{
		// Token: 0x0600B966 RID: 47462 RVA: 0x0044622F File Offset: 0x0044442F
		public V3I(int v0, int v1, int v2)
		{
			this.V0 = v0;
			this.V1 = v1;
			this.V2 = v2;
		}

		// Token: 0x0600B967 RID: 47463 RVA: 0x00446248 File Offset: 0x00444448
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

		// Token: 0x1700174A RID: 5962
		// (get) Token: 0x0600B968 RID: 47464 RVA: 0x004462A7 File Offset: 0x004444A7
		public int X
		{
			get
			{
				return this.V0;
			}
		}

		// Token: 0x1700174B RID: 5963
		// (get) Token: 0x0600B969 RID: 47465 RVA: 0x004462AF File Offset: 0x004444AF
		public int Y
		{
			get
			{
				return this.V1;
			}
		}

		// Token: 0x1700174C RID: 5964
		// (get) Token: 0x0600B96A RID: 47466 RVA: 0x004462B7 File Offset: 0x004444B7
		public int Z
		{
			get
			{
				return this.V2;
			}
		}

		// Token: 0x04008A64 RID: 35428
		public int V0;

		// Token: 0x04008A65 RID: 35429
		public int V1;

		// Token: 0x04008A66 RID: 35430
		public int V2;
	}
}
