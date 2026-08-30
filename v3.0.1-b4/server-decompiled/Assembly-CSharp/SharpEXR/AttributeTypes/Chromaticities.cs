using System;

namespace SharpEXR.AttributeTypes
{
	// Token: 0x020016BC RID: 5820
	public struct Chromaticities
	{
		// Token: 0x0600B5A1 RID: 46497 RVA: 0x00439147 File Offset: 0x00437347
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

		// Token: 0x040087B0 RID: 34736
		public readonly float RedX;

		// Token: 0x040087B1 RID: 34737
		public readonly float RedY;

		// Token: 0x040087B2 RID: 34738
		public readonly float GreenX;

		// Token: 0x040087B3 RID: 34739
		public readonly float GreenY;

		// Token: 0x040087B4 RID: 34740
		public readonly float BlueX;

		// Token: 0x040087B5 RID: 34741
		public readonly float BlueY;

		// Token: 0x040087B6 RID: 34742
		public readonly float WhiteX;

		// Token: 0x040087B7 RID: 34743
		public readonly float WhiteY;
	}
}
