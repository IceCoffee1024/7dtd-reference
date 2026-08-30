using System;

namespace SharpEXR.AttributeTypes
{
	// Token: 0x020016E2 RID: 5858
	public struct Chromaticities
	{
		// Token: 0x0600B724 RID: 46884 RVA: 0x0043EEBF File Offset: 0x0043D0BF
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

		// Token: 0x040088CC RID: 35020
		public readonly float RedX;

		// Token: 0x040088CD RID: 35021
		public readonly float RedY;

		// Token: 0x040088CE RID: 35022
		public readonly float GreenX;

		// Token: 0x040088CF RID: 35023
		public readonly float GreenY;

		// Token: 0x040088D0 RID: 35024
		public readonly float BlueX;

		// Token: 0x040088D1 RID: 35025
		public readonly float BlueY;

		// Token: 0x040088D2 RID: 35026
		public readonly float WhiteX;

		// Token: 0x040088D3 RID: 35027
		public readonly float WhiteY;
	}
}
