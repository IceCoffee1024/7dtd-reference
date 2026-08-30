using System;

namespace SharpEXR.AttributeTypes
{
	// Token: 0x020016E7 RID: 5863
	public struct TileDesc
	{
		// Token: 0x0600B72B RID: 46891 RVA: 0x0043F0B0 File Offset: 0x0043D2B0
		public TileDesc(uint xSize, uint ySize, byte mode)
		{
			this.XSize = xSize;
			this.YSize = ySize;
			int num = (mode & 240) >> 4;
			int num2 = (int)(mode & 15);
			this.RoundingMode = (RoundingMode)num;
			this.LevelMode = (LevelMode)num2;
		}

		// Token: 0x0600B72C RID: 46892 RVA: 0x0043F0E8 File Offset: 0x0043D2E8
		public override string ToString()
		{
			return string.Format("{0}: XSize={1}, YSize={2}", base.GetType().Name, this.XSize, this.YSize);
		}

		// Token: 0x040088DF RID: 35039
		public readonly uint XSize;

		// Token: 0x040088E0 RID: 35040
		public readonly uint YSize;

		// Token: 0x040088E1 RID: 35041
		public readonly LevelMode LevelMode;

		// Token: 0x040088E2 RID: 35042
		public readonly RoundingMode RoundingMode;
	}
}
