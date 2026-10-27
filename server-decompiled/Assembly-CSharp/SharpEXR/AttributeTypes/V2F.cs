using System;

namespace SharpEXR.AttributeTypes
{
	// Token: 0x02001734 RID: 5940
	public struct V2F
	{
		// Token: 0x0600B959 RID: 47449 RVA: 0x004460F3 File Offset: 0x004442F3
		public V2F(float v0, float v1)
		{
			this.V0 = v0;
			this.V1 = v1;
		}

		// Token: 0x0600B95A RID: 47450 RVA: 0x00446103 File Offset: 0x00444303
		public override string ToString()
		{
			return string.Format("{0}: {1}, {2}", base.GetType().Name, this.V0, this.V1);
		}

		// Token: 0x17001743 RID: 5955
		// (get) Token: 0x0600B95B RID: 47451 RVA: 0x0044613A File Offset: 0x0044433A
		public float X
		{
			get
			{
				return this.V0;
			}
		}

		// Token: 0x17001744 RID: 5956
		// (get) Token: 0x0600B95C RID: 47452 RVA: 0x00446142 File Offset: 0x00444342
		public float Y
		{
			get
			{
				return this.V1;
			}
		}

		// Token: 0x04008A5D RID: 35421
		public float V0;

		// Token: 0x04008A5E RID: 35422
		public float V1;
	}
}
