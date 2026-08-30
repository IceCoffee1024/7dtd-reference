using System;
using System.Collections.Generic;
using UnityEngine;

namespace WorldGenerationEngineFinal
{
	// Token: 0x0200171C RID: 5916
	public static class WorldBuilderConstants
	{
		// Token: 0x04008946 RID: 35142
		public static readonly Color32 forestCol = new Color32(0, 64, 0, byte.MaxValue);

		// Token: 0x04008947 RID: 35143
		public static readonly Color32 burntForestCol = new Color32(186, 0, byte.MaxValue, byte.MaxValue);

		// Token: 0x04008948 RID: 35144
		public static readonly Color32 desertCol = new Color32(byte.MaxValue, 228, 119, byte.MaxValue);

		// Token: 0x04008949 RID: 35145
		public static readonly Color32 snowCol = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		// Token: 0x0400894A RID: 35146
		public static readonly Color32 wastelandCol = new Color32(byte.MaxValue, 168, 0, byte.MaxValue);

		// Token: 0x0400894B RID: 35147
		public static readonly Color32 waterCol = new Color32(0, 0, 100, byte.MaxValue);

		// Token: 0x0400894C RID: 35148
		public static readonly List<Color32> biomeColorList = new List<Color32>
		{
			WorldBuilderConstants.forestCol,
			WorldBuilderConstants.burntForestCol,
			WorldBuilderConstants.desertCol,
			WorldBuilderConstants.snowCol,
			WorldBuilderConstants.wastelandCol
		};

		// Token: 0x0400894D RID: 35149
		public const int ForestBiomeWeightDefault = 13;

		// Token: 0x0400894E RID: 35150
		public const int BurntForestBiomeWeightDefault = 18;

		// Token: 0x0400894F RID: 35151
		public const int DesertBiomeWeightDefault = 22;

		// Token: 0x04008950 RID: 35152
		public const int SnowBiomeWeightDefault = 23;

		// Token: 0x04008951 RID: 35153
		public const int WastelandBiomeWeightDefault = 24;

		// Token: 0x04008952 RID: 35154
		public static readonly int[] BiomeWeightDefaults = new int[] { 13, 18, 22, 23, 24 };

		// Token: 0x04008953 RID: 35155
		public const int PlainsWeightDefault = 4;

		// Token: 0x04008954 RID: 35156
		public const int HillsWeightDefault = 4;

		// Token: 0x04008955 RID: 35157
		public const int MountainsWeightDefault = 2;
	}
}
