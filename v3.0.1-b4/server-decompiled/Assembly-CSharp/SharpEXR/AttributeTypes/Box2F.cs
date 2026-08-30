using System;

namespace SharpEXR.AttributeTypes
{
	// Token: 0x020016BA RID: 5818
	public struct Box2F
	{
		// Token: 0x0600B599 RID: 46489 RVA: 0x00438FDF File Offset: 0x004371DF
		public Box2F(float xMin, float yMin, float xMax, float yMax)
		{
			this.XMin = xMin;
			this.YMin = yMin;
			this.XMax = xMax;
			this.YMax = yMax;
		}

		// Token: 0x0600B59A RID: 46490 RVA: 0x00439000 File Offset: 0x00437200
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

		// Token: 0x170015E3 RID: 5603
		// (get) Token: 0x0600B59B RID: 46491 RVA: 0x0043906D File Offset: 0x0043726D
		public float Width
		{
			get
			{
				return this.XMax - this.XMin + 1f;
			}
		}

		// Token: 0x170015E4 RID: 5604
		// (get) Token: 0x0600B59C RID: 46492 RVA: 0x00439082 File Offset: 0x00437282
		public float Height
		{
			get
			{
				return this.YMax - this.YMin + 1f;
			}
		}

		// Token: 0x040087A8 RID: 34728
		public readonly float XMin;

		// Token: 0x040087A9 RID: 34729
		public readonly float YMin;

		// Token: 0x040087AA RID: 34730
		public readonly float XMax;

		// Token: 0x040087AB RID: 34731
		public readonly float YMax;
	}
}
