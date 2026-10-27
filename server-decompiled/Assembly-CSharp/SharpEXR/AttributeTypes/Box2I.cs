using System;

namespace SharpEXR.AttributeTypes
{
	// Token: 0x0200172C RID: 5932
	public struct Box2I
	{
		// Token: 0x0600B94B RID: 47435 RVA: 0x00445DD3 File Offset: 0x00443FD3
		public Box2I(int xMin, int yMin, int xMax, int yMax)
		{
			this.XMin = xMin;
			this.YMin = yMin;
			this.XMax = xMax;
			this.YMax = yMax;
		}

		// Token: 0x0600B94C RID: 47436 RVA: 0x00445DF4 File Offset: 0x00443FF4
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

		// Token: 0x17001740 RID: 5952
		// (get) Token: 0x0600B94D RID: 47437 RVA: 0x00445E61 File Offset: 0x00444061
		public int Width
		{
			get
			{
				return this.XMax - this.XMin + 1;
			}
		}

		// Token: 0x17001741 RID: 5953
		// (get) Token: 0x0600B94E RID: 47438 RVA: 0x00445E72 File Offset: 0x00444072
		public int Height
		{
			get
			{
				return this.YMax - this.YMin + 1;
			}
		}

		// Token: 0x04008A40 RID: 35392
		public readonly int XMin;

		// Token: 0x04008A41 RID: 35393
		public readonly int YMin;

		// Token: 0x04008A42 RID: 35394
		public readonly int XMax;

		// Token: 0x04008A43 RID: 35395
		public readonly int YMax;
	}
}
