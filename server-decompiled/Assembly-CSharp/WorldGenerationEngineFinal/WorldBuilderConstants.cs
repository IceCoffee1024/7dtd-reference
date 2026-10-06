using System;
using System.Collections.Generic;
using UnityEngine;

namespace WorldGenerationEngineFinal
{
	// Token: 0x0200176E RID: 5998
	public static class WorldBuilderConstants
	{
		// Token: 0x04008B27 RID: 35623
		public static readonly Color32 forestCol = new Color32(0, 64, 0, byte.MaxValue);

		// Token: 0x04008B28 RID: 35624
		public static readonly Color32 burntForestCol = new Color32(186, 0, byte.MaxValue, byte.MaxValue);

		// Token: 0x04008B29 RID: 35625
		public static readonly Color32 desertCol = new Color32(byte.MaxValue, 228, 119, byte.MaxValue);

		// Token: 0x04008B2A RID: 35626
		public static readonly Color32 snowCol = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		// Token: 0x04008B2B RID: 35627
		public static readonly Color32 wastelandCol = new Color32(byte.MaxValue, 168, 0, byte.MaxValue);

		// Token: 0x04008B2C RID: 35628
		public static readonly Color32 waterCol = new Color32(0, 0, 100, byte.MaxValue);

		// Token: 0x04008B2D RID: 35629
		public static readonly List<Color32> biomeColorList = new List<Color32>
		{
			WorldBuilderConstants.forestCol,
			WorldBuilderConstants.burntForestCol,
			WorldBuilderConstants.desertCol,
			WorldBuilderConstants.snowCol,
			WorldBuilderConstants.wastelandCol
		};

		// Token: 0x04008B2E RID: 35630
		public const int ForestBiomeWeightDefault = 13;

		// Token: 0x04008B2F RID: 35631
		public const int BurntForestBiomeWeightDefault = 18;

		// Token: 0x04008B30 RID: 35632
		public const int DesertBiomeWeightDefault = 22;

		// Token: 0x04008B31 RID: 35633
		public const int SnowBiomeWeightDefault = 23;

		// Token: 0x04008B32 RID: 35634
		public const int WastelandBiomeWeightDefault = 24;

		// Token: 0x04008B33 RID: 35635
		public static readonly int[] BiomeWeightDefaults = new int[] { 13, 18, 22, 23, 24 };

		// Token: 0x04008B34 RID: 35636
		public const int PlainsWeightDefault = 4;

		// Token: 0x04008B35 RID: 35637
		public const int HillsWeightDefault = 4;

		// Token: 0x04008B36 RID: 35638
		public const int MountainsWeightDefault = 2;
	}
}
