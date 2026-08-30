using System;

namespace SharpEXR.AttributeTypes
{
	// Token: 0x020016E0 RID: 5856
	public struct Box2F
	{
		// Token: 0x0600B71C RID: 46876 RVA: 0x0043ED57 File Offset: 0x0043CF57
		public Box2F(float xMin, float yMin, float xMax, float yMax)
		{
			this.XMin = xMin;
			this.YMin = yMin;
			this.XMax = xMax;
			this.YMax = yMax;
		}

		// Token: 0x0600B71D RID: 46877 RVA: 0x0043ED78 File Offset: 0x0043CF78
		public override string ToString()
		{
			return string.Format("{0}: ({1}, {2})-({3}, {4})", new object[]
			{
				base.GetType().Name,
				this.XMin,
				this.YMin,
				this.XMax,
				this.YMax
			});
		}

		// Token: 0x17001655 RID: 5717
		// (get) Token: 0x0600B71E RID: 46878 RVA: 0x0043EDE5 File Offset: 0x0043CFE5
		public float Width
		{
			get
			{
				return this.XMax - this.XMin + 1f;
			}
		}

		// Token: 0x17001656 RID: 5718
		// (get) Token: 0x0600B71F RID: 46879 RVA: 0x0043EDFA File Offset: 0x0043CFFA
		public float Height
		{
			get
			{
				return this.YMax - this.YMin + 1f;
			}
		}

		// Token: 0x040088C4 RID: 35012
		public readonly float XMin;

		// Token: 0x040088C5 RID: 35013
		public readonly float YMin;

		// Token: 0x040088C6 RID: 35014
		public readonly float XMax;

		// Token: 0x040088C7 RID: 35015
		public readonly float YMax;
	}
}
