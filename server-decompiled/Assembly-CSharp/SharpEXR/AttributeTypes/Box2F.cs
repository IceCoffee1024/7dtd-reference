using System;

namespace SharpEXR.AttributeTypes
{
	// Token: 0x0200170C RID: 5900
	public struct Box2F
	{
		// Token: 0x0600B873 RID: 47219 RVA: 0x00443277 File Offset: 0x00441477
		public Box2F(float xMin, float yMin, float xMax, float yMax)
		{
			this.XMin = xMin;
			this.YMin = yMin;
			this.XMax = xMax;
			this.YMax = yMax;
		}

		// Token: 0x0600B874 RID: 47220 RVA: 0x00443298 File Offset: 0x00441498
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

		// Token: 0x170016F7 RID: 5879
		// (get) Token: 0x0600B875 RID: 47221 RVA: 0x00443305 File Offset: 0x00441505
		public float Width
		{
			get
			{
				return this.XMax - this.XMin + 1f;
			}
		}

		// Token: 0x170016F8 RID: 5880
		// (get) Token: 0x0600B876 RID: 47222 RVA: 0x0044331A File Offset: 0x0044151A
		public float Height
		{
			get
			{
				return this.YMax - this.YMin + 1f;
			}
		}

		// Token: 0x04008989 RID: 35209
		public readonly float XMin;

		// Token: 0x0400898A RID: 35210
		public readonly float YMin;

		// Token: 0x0400898B RID: 35211
		public readonly float XMax;

		// Token: 0x0400898C RID: 35212
		public readonly float YMax;
	}
}
