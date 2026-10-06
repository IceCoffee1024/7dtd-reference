using System;

namespace SharpEXR.AttributeTypes
{
	// Token: 0x02001716 RID: 5910
	public struct V2I
	{
		// Token: 0x0600B889 RID: 47241 RVA: 0x004436A6 File Offset: 0x004418A6
		public V2I(int v0, int v1)
		{
			this.V0 = v0;
			this.V1 = v1;
		}

		// Token: 0x0600B88A RID: 47242 RVA: 0x004436B6 File Offset: 0x004418B6
		public override string ToString()
		{
			return string.Format("{0}: {1}, {2}", base.GetType().Name, this.V0, this.V1);
		}

		// Token: 0x170016FE RID: 5886
		// (get) Token: 0x0600B88B RID: 47243 RVA: 0x004436ED File Offset: 0x004418ED
		public int X
		{
			get
			{
				return this.V0;
			}
		}

		// Token: 0x170016FF RID: 5887
		// (get) Token: 0x0600B88C RID: 47244 RVA: 0x004436F5 File Offset: 0x004418F5
		public int Y
		{
			get
			{
				return this.V1;
			}
		}

		// Token: 0x040089AC RID: 35244
		public int V0;

		// Token: 0x040089AD RID: 35245
		public int V1;
	}
}
