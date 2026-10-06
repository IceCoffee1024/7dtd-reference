using System;

namespace SharpEXR.AttributeTypes
{
	// Token: 0x02001715 RID: 5909
	public struct V2F
	{
		// Token: 0x0600B885 RID: 47237 RVA: 0x0044364F File Offset: 0x0044184F
		public V2F(float v0, float v1)
		{
			this.V0 = v0;
			this.V1 = v1;
		}

		// Token: 0x0600B886 RID: 47238 RVA: 0x0044365F File Offset: 0x0044185F
		public override string ToString()
		{
			return string.Format("{0}: {1}, {2}", base.GetType().Name, this.V0, this.V1);
		}

		// Token: 0x170016FC RID: 5884
		// (get) Token: 0x0600B887 RID: 47239 RVA: 0x00443696 File Offset: 0x00441896
		public float X
		{
			get
			{
				return this.V0;
			}
		}

		// Token: 0x170016FD RID: 5885
		// (get) Token: 0x0600B888 RID: 47240 RVA: 0x0044369E File Offset: 0x0044189E
		public float Y
		{
			get
			{
				return this.V1;
			}
		}

		// Token: 0x040089AA RID: 35242
		public float V0;

		// Token: 0x040089AB RID: 35243
		public float V1;
	}
}
