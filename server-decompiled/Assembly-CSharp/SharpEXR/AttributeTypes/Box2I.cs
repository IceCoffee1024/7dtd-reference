using System;

namespace SharpEXR.AttributeTypes
{
	// Token: 0x0200170D RID: 5901
	public struct Box2I
	{
		// Token: 0x0600B877 RID: 47223 RVA: 0x0044332F File Offset: 0x0044152F
		public Box2I(int xMin, int yMin, int xMax, int yMax)
		{
			this.XMin = xMin;
			this.YMin = yMin;
			this.XMax = xMax;
			this.YMax = yMax;
		}

		// Token: 0x0600B878 RID: 47224 RVA: 0x00443350 File Offset: 0x00441550
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

		// Token: 0x170016F9 RID: 5881
		// (get) Token: 0x0600B879 RID: 47225 RVA: 0x004433BD File Offset: 0x004415BD
		public int Width
		{
			get
			{
				return this.XMax - this.XMin + 1;
			}
		}

		// Token: 0x170016FA RID: 5882
		// (get) Token: 0x0600B87A RID: 47226 RVA: 0x004433CE File Offset: 0x004415CE
		public int Height
		{
			get
			{
				return this.YMax - this.YMin + 1;
			}
		}

		// Token: 0x0400898D RID: 35213
		public readonly int XMin;

		// Token: 0x0400898E RID: 35214
		public readonly int YMin;

		// Token: 0x0400898F RID: 35215
		public readonly int XMax;

		// Token: 0x04008990 RID: 35216
		public readonly int YMax;
	}
}
