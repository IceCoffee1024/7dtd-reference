using System;

namespace SharpEXR.AttributeTypes
{
	// Token: 0x0200172D RID: 5933
	public struct Chromaticities
	{
		// Token: 0x0600B94F RID: 47439 RVA: 0x00445E83 File Offset: 0x00444083
		public Chromaticities(float redX, float redY, float greenX, float greenY, float blueX, float blueY, float whiteX, float whiteY)
		{
			this.RedX = redX;
			this.RedY = redY;
			this.GreenX = greenX;
			this.GreenY = greenY;
			this.BlueX = blueX;
			this.BlueY = blueY;
			this.WhiteX = whiteX;
			this.WhiteY = whiteY;
		}

		// Token: 0x04008A44 RID: 35396
		public readonly float RedX;

		// Token: 0x04008A45 RID: 35397
		public readonly float RedY;

		// Token: 0x04008A46 RID: 35398
		public readonly float GreenX;

		// Token: 0x04008A47 RID: 35399
		public readonly float GreenY;

		// Token: 0x04008A48 RID: 35400
		public readonly float BlueX;

		// Token: 0x04008A49 RID: 35401
		public readonly float BlueY;

		// Token: 0x04008A4A RID: 35402
		public readonly float WhiteX;

		// Token: 0x04008A4B RID: 35403
		public readonly float WhiteY;
	}
}
