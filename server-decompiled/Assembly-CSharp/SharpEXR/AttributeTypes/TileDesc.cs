using System;

namespace SharpEXR.AttributeTypes
{
	// Token: 0x020016C1 RID: 5825
	public struct TileDesc
	{
		// Token: 0x0600B5A8 RID: 46504 RVA: 0x00439338 File Offset: 0x00437538
		public TileDesc(uint xSize, uint ySize, byte mode)
		{
			this.XSize = xSize;
			this.YSize = ySize;
			int num = (mode & 240) >> 4;
			int num2 = (int)(mode & 15);
			this.RoundingMode = (RoundingMode)num;
			this.LevelMode = (LevelMode)num2;
		}

		// Token: 0x0600B5A9 RID: 46505 RVA: 0x00439370 File Offset: 0x00437570
		public override string ToString()
		{
			return string.Format("{0}: XSize={1}, YSize={2}", base.GetType().Name, this.XSize, this.YSize);
		}

		// Token: 0x040087C3 RID: 34755
		public readonly uint XSize;

		// Token: 0x040087C4 RID: 34756
		public readonly uint YSize;

		// Token: 0x040087C5 RID: 34757
		public readonly LevelMode LevelMode;

		// Token: 0x040087C6 RID: 34758
		public readonly RoundingMode RoundingMode;
	}
}
