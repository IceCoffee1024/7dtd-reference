using System;

namespace SharpEXR.AttributeTypes
{
	// Token: 0x02001735 RID: 5941
	public struct V2I
	{
		// Token: 0x0600B95D RID: 47453 RVA: 0x0044614A File Offset: 0x0044434A
		public V2I(int v0, int v1)
		{
			this.V0 = v0;
			this.V1 = v1;
		}

		// Token: 0x0600B95E RID: 47454 RVA: 0x0044615A File Offset: 0x0044435A
		public override string ToString()
		{
			return string.Format("{0}: {1}, {2}", base.GetType().Name, this.V0, this.V1);
		}

		// Token: 0x17001745 RID: 5957
		// (get) Token: 0x0600B95F RID: 47455 RVA: 0x00446191 File Offset: 0x00444391
		public int X
		{
			get
			{
				return this.V0;
			}
		}

		// Token: 0x17001746 RID: 5958
		// (get) Token: 0x0600B960 RID: 47456 RVA: 0x00446199 File Offset: 0x00444399
		public int Y
		{
			get
			{
				return this.V1;
			}
		}

		// Token: 0x04008A5F RID: 35423
		public int V0;

		// Token: 0x04008A60 RID: 35424
		public int V1;
	}
}
