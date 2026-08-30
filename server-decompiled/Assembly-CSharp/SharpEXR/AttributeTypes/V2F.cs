using System;

namespace SharpEXR.AttributeTypes
{
	// Token: 0x020016C3 RID: 5827
	public struct V2F
	{
		// Token: 0x0600B5AB RID: 46507 RVA: 0x004393B7 File Offset: 0x004375B7
		public V2F(float v0, float v1)
		{
			this.V0 = v0;
			this.V1 = v1;
		}

		// Token: 0x0600B5AC RID: 46508 RVA: 0x004393C7 File Offset: 0x004375C7
		public override string ToString()
		{
			return string.Format("{0}: {1}, {2}", base.GetType().Name, this.V0, this.V1);
		}

		// Token: 0x170015E8 RID: 5608
		// (get) Token: 0x0600B5AD RID: 46509 RVA: 0x004393FE File Offset: 0x004375FE
		public float X
		{
			get
			{
				return this.V0;
			}
		}

		// Token: 0x170015E9 RID: 5609
		// (get) Token: 0x0600B5AE RID: 46510 RVA: 0x00439406 File Offset: 0x00437606
		public float Y
		{
			get
			{
				return this.V1;
			}
		}

		// Token: 0x040087C9 RID: 34761
		public float V0;

		// Token: 0x040087CA RID: 34762
		public float V1;
	}
}
