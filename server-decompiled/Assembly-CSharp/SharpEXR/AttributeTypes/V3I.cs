using System;

namespace SharpEXR.AttributeTypes
{
	// Token: 0x020016EC RID: 5868
	public struct V3I
	{
		// Token: 0x0600B73B RID: 46907 RVA: 0x0043F26B File Offset: 0x0043D46B
		public V3I(int v0, int v1, int v2)
		{
			this.V0 = v0;
			this.V1 = v1;
			this.V2 = v2;
		}

		// Token: 0x0600B73C RID: 46908 RVA: 0x0043F284 File Offset: 0x0043D484
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

		// Token: 0x17001661 RID: 5729
		// (get) Token: 0x0600B73D RID: 46909 RVA: 0x0043F2E3 File Offset: 0x0043D4E3
		public int X
		{
			get
			{
				return this.V0;
			}
		}

		// Token: 0x17001662 RID: 5730
		// (get) Token: 0x0600B73E RID: 46910 RVA: 0x0043F2EB File Offset: 0x0043D4EB
		public int Y
		{
			get
			{
				return this.V1;
			}
		}

		// Token: 0x17001663 RID: 5731
		// (get) Token: 0x0600B73F RID: 46911 RVA: 0x0043F2F3 File Offset: 0x0043D4F3
		public int Z
		{
			get
			{
				return this.V2;
			}
		}

		// Token: 0x040088EC RID: 35052
		public int V0;

		// Token: 0x040088ED RID: 35053
		public int V1;

		// Token: 0x040088EE RID: 35054
		public int V2;
	}
}
