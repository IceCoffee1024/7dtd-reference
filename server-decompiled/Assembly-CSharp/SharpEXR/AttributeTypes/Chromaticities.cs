using System;

namespace SharpEXR.AttributeTypes
{
	// Token: 0x0200170E RID: 5902
	public struct Chromaticities
	{
		// Token: 0x0600B87B RID: 47227 RVA: 0x004433DF File Offset: 0x004415DF
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

		// Token: 0x04008991 RID: 35217
		public readonly float RedX;

		// Token: 0x04008992 RID: 35218
		public readonly float RedY;

		// Token: 0x04008993 RID: 35219
		public readonly float GreenX;

		// Token: 0x04008994 RID: 35220
		public readonly float GreenY;

		// Token: 0x04008995 RID: 35221
		public readonly float BlueX;

		// Token: 0x04008996 RID: 35222
		public readonly float BlueY;

		// Token: 0x04008997 RID: 35223
		public readonly float WhiteX;

		// Token: 0x04008998 RID: 35224
		public readonly float WhiteY;
	}
}
