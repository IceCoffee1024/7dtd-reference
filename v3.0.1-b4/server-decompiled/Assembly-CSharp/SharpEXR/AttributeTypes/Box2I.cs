using System;

namespace SharpEXR.AttributeTypes
{
	// Token: 0x020016BB RID: 5819
	public struct Box2I
	{
		// Token: 0x0600B59D RID: 46493 RVA: 0x00439097 File Offset: 0x00437297
		public Box2I(int xMin, int yMin, int xMax, int yMax)
		{
			this.XMin = xMin;
			this.YMin = yMin;
			this.XMax = xMax;
			this.YMax = yMax;
		}

		// Token: 0x0600B59E RID: 46494 RVA: 0x004390B8 File Offset: 0x004372B8
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

		// Token: 0x170015E5 RID: 5605
		// (get) Token: 0x0600B59F RID: 46495 RVA: 0x00439125 File Offset: 0x00437325
		public int Width
		{
			get
			{
				return this.XMax - this.XMin + 1;
			}
		}

		// Token: 0x170015E6 RID: 5606
		// (get) Token: 0x0600B5A0 RID: 46496 RVA: 0x00439136 File Offset: 0x00437336
		public int Height
		{
			get
			{
				return this.YMax - this.YMin + 1;
			}
		}

		// Token: 0x040087AC RID: 34732
		public readonly int XMin;

		// Token: 0x040087AD RID: 34733
		public readonly int YMin;

		// Token: 0x040087AE RID: 34734
		public readonly int XMax;

		// Token: 0x040087AF RID: 34735
		public readonly int YMax;
	}
}
