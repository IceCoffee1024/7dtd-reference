using System;

namespace SharpEXR.AttributeTypes
{
	// Token: 0x020016E1 RID: 5857
	public struct Box2I
	{
		// Token: 0x0600B720 RID: 46880 RVA: 0x0043EE0F File Offset: 0x0043D00F
		public Box2I(int xMin, int yMin, int xMax, int yMax)
		{
			this.XMin = xMin;
			this.YMin = yMin;
			this.XMax = xMax;
			this.YMax = yMax;
		}

		// Token: 0x0600B721 RID: 46881 RVA: 0x0043EE30 File Offset: 0x0043D030
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

		// Token: 0x17001657 RID: 5719
		// (get) Token: 0x0600B722 RID: 46882 RVA: 0x0043EE9D File Offset: 0x0043D09D
		public int Width
		{
			get
			{
				return this.XMax - this.XMin + 1;
			}
		}

		// Token: 0x17001658 RID: 5720
		// (get) Token: 0x0600B723 RID: 46883 RVA: 0x0043EEAE File Offset: 0x0043D0AE
		public int Height
		{
			get
			{
				return this.YMax - this.YMin + 1;
			}
		}

		// Token: 0x040088C8 RID: 35016
		public readonly int XMin;

		// Token: 0x040088C9 RID: 35017
		public readonly int YMin;

		// Token: 0x040088CA RID: 35018
		public readonly int XMax;

		// Token: 0x040088CB RID: 35019
		public readonly int YMax;
	}
}
