using System;

namespace SharpEXR.AttributeTypes
{
	// Token: 0x0200172B RID: 5931
	public struct Box2F
	{
		// Token: 0x0600B947 RID: 47431 RVA: 0x00445D1B File Offset: 0x00443F1B
		public Box2F(float xMin, float yMin, float xMax, float yMax)
		{
			this.XMin = xMin;
			this.YMin = yMin;
			this.XMax = xMax;
			this.YMax = yMax;
		}

		// Token: 0x0600B948 RID: 47432 RVA: 0x00445D3C File Offset: 0x00443F3C
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

		// Token: 0x1700173E RID: 5950
		// (get) Token: 0x0600B949 RID: 47433 RVA: 0x00445DA9 File Offset: 0x00443FA9
		public float Width
		{
			get
			{
				return this.XMax - this.XMin + 1f;
			}
		}

		// Token: 0x1700173F RID: 5951
		// (get) Token: 0x0600B94A RID: 47434 RVA: 0x00445DBE File Offset: 0x00443FBE
		public float Height
		{
			get
			{
				return this.YMax - this.YMin + 1f;
			}
		}

		// Token: 0x04008A3C RID: 35388
		public readonly float XMin;

		// Token: 0x04008A3D RID: 35389
		public readonly float YMin;

		// Token: 0x04008A3E RID: 35390
		public readonly float XMax;

		// Token: 0x04008A3F RID: 35391
		public readonly float YMax;
	}
}
