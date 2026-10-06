using System;

namespace SharpEXR.AttributeTypes
{
	// Token: 0x02001713 RID: 5907
	public struct TileDesc
	{
		// Token: 0x0600B882 RID: 47234 RVA: 0x004435D0 File Offset: 0x004417D0
		public TileDesc(uint xSize, uint ySize, byte mode)
		{
			this.XSize = xSize;
			this.YSize = ySize;
			int num = (mode & 240) >> 4;
			int num2 = (int)(mode & 15);
			this.RoundingMode = (RoundingMode)num;
			this.LevelMode = (LevelMode)num2;
		}

		// Token: 0x0600B883 RID: 47235 RVA: 0x00443608 File Offset: 0x00441808
		public override string ToString()
		{
			return string.Format("{0}: XSize={1}, YSize={2}", base.GetType().Name, this.XSize, this.YSize);
		}

		// Token: 0x040089A4 RID: 35236
		public readonly uint XSize;

		// Token: 0x040089A5 RID: 35237
		public readonly uint YSize;

		// Token: 0x040089A6 RID: 35238
		public readonly LevelMode LevelMode;

		// Token: 0x040089A7 RID: 35239
		public readonly RoundingMode RoundingMode;
	}
}
