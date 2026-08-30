using System;
using System.Collections.Generic;
using UnityEngine;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001742 RID: 5954
	public static class WorldBuilderConstants
	{
		// Token: 0x04008A62 RID: 35426
		public static readonly Color32 forestCol = new Color32(0, 64, 0, byte.MaxValue);

		// Token: 0x04008A63 RID: 35427
		public static readonly Color32 burntForestCol = new Color32(186, 0, byte.MaxValue, byte.MaxValue);

		// Token: 0x04008A64 RID: 35428
		public static readonly Color32 desertCol = new Color32(byte.MaxValue, 228, 119, byte.MaxValue);

		// Token: 0x04008A65 RID: 35429
		public static readonly Color32 snowCol = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		// Token: 0x04008A66 RID: 35430
		public static readonly Color32 wastelandCol = new Color32(byte.MaxValue, 168, 0, byte.MaxValue);

		// Token: 0x04008A67 RID: 35431
		public static readonly Color32 waterCol = new Color32(0, 0, 100, byte.MaxValue);

		// Token: 0x04008A68 RID: 35432
		public static readonly List<Color32> biomeColorList = new List<Color32>
		{
			WorldBuilderConstants.forestCol,
			WorldBuilderConstants.burntForestCol,
			WorldBuilderConstants.desertCol,
			WorldBuilderConstants.snowCol,
			WorldBuilderConstants.wastelandCol
		};

		// Token: 0x04008A69 RID: 35433
		public const int ForestBiomeWeightDefault = 13;

		// Token: 0x04008A6A RID: 35434
		public const int BurntForestBiomeWeightDefault = 18;

		// Token: 0x04008A6B RID: 35435
		public const int DesertBiomeWeightDefault = 22;

		// Token: 0x04008A6C RID: 35436
		public const int SnowBiomeWeightDefault = 23;

		// Token: 0x04008A6D RID: 35437
		public const int WastelandBiomeWeightDefault = 24;

		// Token: 0x04008A6E RID: 35438
		public static readonly int[] BiomeWeightDefaults = new int[] { 13, 18, 22, 23, 24 };

		// Token: 0x04008A6F RID: 35439
		public const int PlainsWeightDefault = 4;

		// Token: 0x04008A70 RID: 35440
		public const int HillsWeightDefault = 4;

		// Token: 0x04008A71 RID: 35441
		public const int MountainsWeightDefault = 2;
	}
}
