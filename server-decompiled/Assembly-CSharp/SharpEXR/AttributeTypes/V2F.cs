using System;

namespace SharpEXR.AttributeTypes
{
	// Token: 0x020016E9 RID: 5865
	public struct V2F
	{
		// Token: 0x0600B72E RID: 46894 RVA: 0x0043F12F File Offset: 0x0043D32F
		public V2F(float v0, float v1)
		{
			this.V0 = v0;
			this.V1 = v1;
		}

		// Token: 0x0600B72F RID: 46895 RVA: 0x0043F13F File Offset: 0x0043D33F
		public override string ToString()
		{
			return string.Format("{0}: {1}, {2}", base.GetType().Name, this.V0, this.V1);
		}

		// Token: 0x1700165A RID: 5722
		// (get) Token: 0x0600B730 RID: 46896 RVA: 0x0043F176 File Offset: 0x0043D376
		public float X
		{
			get
			{
				return this.V0;
			}
		}

		// Token: 0x1700165B RID: 5723
		// (get) Token: 0x0600B731 RID: 46897 RVA: 0x0043F17E File Offset: 0x0043D37E
		public float Y
		{
			get
			{
				return this.V1;
			}
		}

		// Token: 0x040088E5 RID: 35045
		public float V0;

		// Token: 0x040088E6 RID: 35046
		public float V1;
	}
}
