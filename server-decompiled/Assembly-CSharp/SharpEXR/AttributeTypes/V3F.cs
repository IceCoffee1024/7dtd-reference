using System;

namespace SharpEXR.AttributeTypes
{
	// Token: 0x02001736 RID: 5942
	public struct V3F
	{
		// Token: 0x0600B961 RID: 47457 RVA: 0x004461A1 File Offset: 0x004443A1
		public V3F(float v0, float v1, float v2)
		{
			this.V0 = v0;
			this.V1 = v1;
			this.V2 = v2;
		}

		// Token: 0x0600B962 RID: 47458 RVA: 0x004461B8 File Offset: 0x004443B8
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

		// Token: 0x17001747 RID: 5959
		// (get) Token: 0x0600B963 RID: 47459 RVA: 0x00446217 File Offset: 0x00444417
		public float X
		{
			get
			{
				return this.V0;
			}
		}

		// Token: 0x17001748 RID: 5960
		// (get) Token: 0x0600B964 RID: 47460 RVA: 0x0044621F File Offset: 0x0044441F
		public float Y
		{
			get
			{
				return this.V1;
			}
		}

		// Token: 0x17001749 RID: 5961
		// (get) Token: 0x0600B965 RID: 47461 RVA: 0x00446227 File Offset: 0x00444427
		public float Z
		{
			get
			{
				return this.V2;
			}
		}

		// Token: 0x04008A61 RID: 35425
		public float V0;

		// Token: 0x04008A62 RID: 35426
		public float V1;

		// Token: 0x04008A63 RID: 35427
		public float V2;
	}
}
