using System;

namespace SharpEXR.AttributeTypes
{
	// Token: 0x020016C5 RID: 5829
	public struct V3F
	{
		// Token: 0x0600B5B3 RID: 46515 RVA: 0x00439465 File Offset: 0x00437665
		public V3F(float v0, float v1, float v2)
		{
			this.V0 = v0;
			this.V1 = v1;
			this.V2 = v2;
		}

		// Token: 0x0600B5B4 RID: 46516 RVA: 0x0043947C File Offset: 0x0043767C
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

		// Token: 0x170015EC RID: 5612
		// (get) Token: 0x0600B5B5 RID: 46517 RVA: 0x004394DB File Offset: 0x004376DB
		public float X
		{
			get
			{
				return this.V0;
			}
		}

		// Token: 0x170015ED RID: 5613
		// (get) Token: 0x0600B5B6 RID: 46518 RVA: 0x004394E3 File Offset: 0x004376E3
		public float Y
		{
			get
			{
				return this.V1;
			}
		}

		// Token: 0x170015EE RID: 5614
		// (get) Token: 0x0600B5B7 RID: 46519 RVA: 0x004394EB File Offset: 0x004376EB
		public float Z
		{
			get
			{
				return this.V2;
			}
		}

		// Token: 0x040087CD RID: 34765
		public float V0;

		// Token: 0x040087CE RID: 34766
		public float V1;

		// Token: 0x040087CF RID: 34767
		public float V2;
	}
}
