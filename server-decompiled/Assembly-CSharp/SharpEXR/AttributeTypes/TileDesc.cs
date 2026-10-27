using System;

namespace SharpEXR.AttributeTypes
{
	// Token: 0x02001732 RID: 5938
	public struct TileDesc
	{
		// Token: 0x0600B956 RID: 47446 RVA: 0x00446074 File Offset: 0x00444274
		public TileDesc(uint xSize, uint ySize, byte mode)
		{
			this.XSize = xSize;
			this.YSize = ySize;
			int num = (mode & 240) >> 4;
			int num2 = (int)(mode & 15);
			this.RoundingMode = (RoundingMode)num;
			this.LevelMode = (LevelMode)num2;
		}

		// Token: 0x0600B957 RID: 47447 RVA: 0x004460AC File Offset: 0x004442AC
		public override string ToString()
		{
			return string.Format("{0}: XSize={1}, YSize={2}", base.GetType().Name, this.XSize, this.YSize);
		}

		// Token: 0x04008A57 RID: 35415
		public readonly uint XSize;

		// Token: 0x04008A58 RID: 35416
		public readonly uint YSize;

		// Token: 0x04008A59 RID: 35417
		public readonly LevelMode LevelMode;

		// Token: 0x04008A5A RID: 35418
		public readonly RoundingMode RoundingMode;
	}
}
