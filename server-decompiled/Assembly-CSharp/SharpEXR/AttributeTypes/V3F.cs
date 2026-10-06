using System;

namespace SharpEXR.AttributeTypes
{
	// Token: 0x02001717 RID: 5911
	public struct V3F
	{
		// Token: 0x0600B88D RID: 47245 RVA: 0x004436FD File Offset: 0x004418FD
		public V3F(float v0, float v1, float v2)
		{
			this.V0 = v0;
			this.V1 = v1;
			this.V2 = v2;
		}

		// Token: 0x0600B88E RID: 47246 RVA: 0x00443714 File Offset: 0x00441914
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

		// Token: 0x17001700 RID: 5888
		// (get) Token: 0x0600B88F RID: 47247 RVA: 0x00443773 File Offset: 0x00441973
		public float X
		{
			get
			{
				return this.V0;
			}
		}

		// Token: 0x17001701 RID: 5889
		// (get) Token: 0x0600B890 RID: 47248 RVA: 0x0044377B File Offset: 0x0044197B
		public float Y
		{
			get
			{
				return this.V1;
			}
		}

		// Token: 0x17001702 RID: 5890
		// (get) Token: 0x0600B891 RID: 47249 RVA: 0x00443783 File Offset: 0x00441983
		public float Z
		{
			get
			{
				return this.V2;
			}
		}

		// Token: 0x040089AE RID: 35246
		public float V0;

		// Token: 0x040089AF RID: 35247
		public float V1;

		// Token: 0x040089B0 RID: 35248
		public float V2;
	}
}
